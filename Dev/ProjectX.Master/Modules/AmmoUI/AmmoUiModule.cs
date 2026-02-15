#if !SERVER
using System;
using System.Reflection;
using HarmonyLib;
using RedLoader;
using Sons.Gui;
using SonsSdk;
using SUI;
using TheForest.Utils;
using UnityEngine;

namespace ProjectX.Master.Modules.AmmoUI
{
    /// <summary>
    /// AmmoUI module — displays current ammo count and weapon icon when holding a ranged weapon.
    /// Port of the standalone AmmoUi 1.1.2 mod, using Piggyback Pattern + Harmony.
    /// Uses AccessTools.TypeByName for string-based type resolution (no compile-time Sons.Weapon dependency).
    /// </summary>
    public static class AmmoUiModule
    {
        // Observables for SUI binding
        private static readonly Observable<string> _ammoInfo = new Observable<string>("");
        private static readonly Observable<bool> _showPanel = new Observable<bool>(false);
        private static readonly Observable<Texture> _ammoIcon = new Observable<Texture>(null);

        // SUI elements
        private static SContainerOptions _ammoPanel;
        private static SImageOptions _icon;
        private static SLabelOptions _text;

        private static bool _initialized;
        private static bool _harmonyApplied;

        // Ammo data (written by Harmony patch, read by OnUpdate)
        public static int RemainingAmmo;
        public static int TotalAmmo;

        // Cached reflection for the Harmony prefix
        private static MethodInfo _getAmmoMethod;
        private static MethodInfo _getRemainingAmmoMethod;
        private static MethodInfo _amountOfMethod;
        private static FieldInfo _ammoTypeField;
        private static PropertyInfo _ammoTypeProperty;

        public static void Init()
        {
            try
            {
                // Create SUI panel — bottom-left, transparent background
                // Smaller base size (200x60) — configurable via AmmoUiSize scale multiplier
                _ammoPanel = SUI.SUI.RegisterNewPanel("AmmoUiPanel", false, default(KeyCode?))
                    .Pivot(0f, 0f)
                    .Background(Color.clear, EBackground.None, default(UnityEngine.UI.Image.Type?))
                    .Anchor(AnchorType.BottomLeft)
                    .Size(350f, 120f)
                    .Position(0f, 0f)
                    .Horizontal(0f, "EE")
                    .BindVisibility(_showPanel);

                float opacity = Config.AmmoUiOpacity.Value;
                Color textColor = new Color(1f, 1f, 1f, opacity);

                _icon = SUI.SUI.SImage.Bind(_ammoIcon).Dock(EDockType.Fill);
                _text = SUI.SUI.SLabel.Bind(_ammoInfo).FontSize(14).Dock(EDockType.Fill)
                    .Alignment(TMPro.TextAlignmentOptions.Left)
                    .FontColor(textColor);

                _ammoPanel.Add(_icon);
                _ammoPanel.Add(_text);

                // Apply configurable size (scale transform like original mod)
                float size = Config.AmmoUiSize.Value;
                _ammoPanel.RectTransform.localScale = new UnityEngine.Vector2(size, size);

                // Apply configurable opacity to icon
                try { _icon.ImageObject.color = new Color(1f, 1f, 1f, opacity); } catch { }

                _showPanel.Set(false);
                _initialized = true;

                RLog.Msg($"[AmmoUI] Module initialized. Size={size:F1}, Opacity={opacity:F1}");
            }
            catch (Exception ex)
            {
                RLog.Error($"[AmmoUI] Init error: {ex}");
            }
        }

        /// <summary>
        /// Apply Harmony patch using string-based type resolution (no compile-time Sons.Weapon dependency).
        /// </summary>
        public static void ApplyHarmonyPatch(HarmonyLib.Harmony harmony)
        {
            if (_harmonyApplied) return;

            try
            {
                // Resolve RangedWeapon type at runtime via string name
                var rangedWeaponType = AccessTools.TypeByName("Sons.Weapon.RangedWeapon");
                if (rangedWeaponType == null)
                {
                    RLog.Warning("[AmmoUI] RangedWeapon type not found — cannot apply Harmony patch.");
                    return;
                }

                // Cache reflection methods for use in the prefix
                _getAmmoMethod = AccessTools.Method(rangedWeaponType, "GetAmmo");
                if (_getAmmoMethod == null)
                {
                    RLog.Warning("[AmmoUI] GetAmmo method not found on RangedWeapon.");
                    return;
                }

                var ammoReturnType = _getAmmoMethod.ReturnType;
                _getRemainingAmmoMethod = AccessTools.Method(ammoReturnType, "GetRemainingAmmo");

                // Find _type — try field first (public, then nonpublic), then property
                _ammoTypeField = ammoReturnType.GetField("_type", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (_ammoTypeField == null)
                {
                    _ammoTypeProperty = ammoReturnType.GetProperty("_type", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        ?? ammoReturnType.GetProperty("Type", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }

                // Dump ammo type members for diagnostics
                RLog.Msg($"[AmmoUI] Ammo type: {ammoReturnType.FullName}");
                RLog.Msg($"[AmmoUI] GetRemainingAmmo: {_getRemainingAmmoMethod != null}");
                RLog.Msg($"[AmmoUI] _type field: {_ammoTypeField != null}, _type property: {_ammoTypeProperty != null}");

                var allFields = ammoReturnType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var f in allFields)
                    RLog.Msg($"[AmmoUI]   Field: {f.FieldType.Name} {f.Name}");

                var allProps = ammoReturnType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var p in allProps)
                    RLog.Msg($"[AmmoUI]   Prop: {p.PropertyType.Name} {p.Name}");

                if (_getRemainingAmmoMethod == null)
                {
                    RLog.Warning("[AmmoUI] GetRemainingAmmo not found — cannot get ammo count.");
                    return;
                }

                // Apply the prefix patch
                var targetMethod = AccessTools.Method(rangedWeaponType, "LateUpdate");
                if (targetMethod == null)
                {
                    RLog.Warning("[AmmoUI] RangedWeapon.LateUpdate not found — trying Update.");
                    targetMethod = AccessTools.Method(rangedWeaponType, "Update");
                }

                if (targetMethod == null)
                {
                    RLog.Warning("[AmmoUI] No suitable RangedWeapon method found to patch.");
                    return;
                }

                var prefix = new HarmonyMethod(typeof(AmmoUiModule), nameof(AmmoPrefix));
                harmony.Patch(targetMethod, prefix);
                _harmonyApplied = true;
                RLog.Msg($"[AmmoUI] Harmony patch applied to {rangedWeaponType.Name}.{targetMethod.Name}.");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[AmmoUI] Harmony patch failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Harmony Prefix — takes object __instance to avoid compile-time type dependency.
        /// Captures ammo data to static fields.
        /// </summary>
        public static void AmmoPrefix(object __instance)
        {
            try
            {
                var ammo = _getAmmoMethod.Invoke(__instance, null);
                if (ammo == null) return;

                // TODO: Test using _currentCount property instead of GetRemainingAmmo() for more accurate magazine tracking
                RemainingAmmo = (int)_getRemainingAmmoMethod.Invoke(ammo, null);

                // Get ammo type for inventory count
                object ammoItemType = null;
                if (_ammoTypeField != null)
                    ammoItemType = _ammoTypeField.GetValue(ammo);
                else if (_ammoTypeProperty != null)
                    ammoItemType = _ammoTypeProperty.GetValue(ammo);

                if (ammoItemType != null && LocalPlayer.Inventory != null)
                {
                    // Cache AmountOf method on first use
                    if (_amountOfMethod == null)
                    {
                        _amountOfMethod = LocalPlayer.Inventory.GetType().GetMethod("AmountOf",
                            new[] { ammoItemType.GetType(), typeof(bool), typeof(bool) });
                    }

                    if (_amountOfMethod != null)
                    {
                        TotalAmmo = (int)_amountOfMethod.Invoke(LocalPlayer.Inventory,
                            new object[] { ammoItemType, true, false });
                    }
                }
            }
            catch { }
        }

        public static void OnUpdate()
        {
            if (!_initialized) return;

            try
            {
                // Hide during pause or no inventory
                if (PauseMenu.IsActive || LocalPlayer.Inventory == null)
                {
                    _showPanel.Set(false);
                    return;
                }

                // Check if holding a ranged weapon (weaponType == 2)
                var rightHandItem = LocalPlayer.Inventory.RightHandItem;
                if (rightHandItem != null && (int)rightHandItem.Data._weaponType == 2 && !LocalPlayer.IsInInventory)
                {
                    _ammoInfo.Set($"<size=36>{RemainingAmmo}</size><size=28>/{TotalAmmo}</size>");
                    _ammoIcon.Set(rightHandItem.Data.UiData?._icon);
                    _showPanel.Set(true);
                }
                else
                {
                    _showPanel.Set(false);
                }
            }
            catch { }
        }
    }
}
#endif

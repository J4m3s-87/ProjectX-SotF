using System.Reflection;
using HarmonyLib;
using RedLoader;

namespace ProjectX.Master.Modules.WeaponDamage
{
    /// <summary>
    /// Harmony patch for RangedWeapon.LateUpdate to track shotgun ammo type.
    /// Uses Pattern #13 (String-Based Harmony) — RangedWeapon has a deep assembly
    /// dependency chain that prevents compile-time typed references.
    /// </summary>
    public static class WeaponDamagePatches
    {
        /// <summary>Current ammo type of held ranged weapon. 364=buckshot, 363=slug, -1=other.</summary>
        public static int CurrentAmmoType = -1;

        // Cached reflection members (Pattern #13 — cache on first use)
        private static PropertyInfo _weaponItemIdProp;
        private static PropertyInfo _ammoProp;
        private static PropertyInfo _ammoTypeProp;

        /// <summary>
        /// Apply the RangedWeapon.LateUpdate Harmony postfix manually.
        /// Uses AccessTools.TypeByName to bypass assembly chain trap (Phase 301).
        /// </summary>
        public static void ApplyPatch(HarmonyLib.Harmony harmony)
        {
            var rwType = AccessTools.TypeByName("Sons.Weapon.RangedWeapon");
            if (rwType == null)
            {
                RLog.Warning("[WeaponDamage] RangedWeapon type not found — ammo tracking disabled");
                return;
            }

            var target = AccessTools.Method(rwType, "LateUpdate");
            if (target == null)
            {
                RLog.Warning("[WeaponDamage] RangedWeapon.LateUpdate not found — ammo tracking disabled");
                return;
            }

            var postfix = AccessTools.Method(typeof(WeaponDamagePatches), nameof(LateUpdatePostfix));
            harmony.Patch(target, postfix: new HarmonyMethod(postfix));
            RLog.Msg("[WeaponDamage] Patched RangedWeapon.LateUpdate for ammo type tracking");
        }

        /// <summary>
        /// Postfix on RangedWeapon.LateUpdate — uses object __instance (Pattern #13).
        /// Tracks whether shotgun (ID 358) is using buckshot (364) or slug (363).
        /// </summary>
        private static void LateUpdatePostfix(object __instance)
        {
            try
            {
                // Cache property lookups on first call
                if (_weaponItemIdProp == null)
                {
                    var type = __instance.GetType();
                    _weaponItemIdProp = type.GetProperty("_weaponItemId",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    _ammoProp = type.GetProperty("_ammo",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }

                if (_weaponItemIdProp == null) return;

                int weaponId = (int)_weaponItemIdProp.GetValue(__instance);
                
                // Only track ammo type for shotgun (ID 358)
                if (weaponId == 358)
                {
                    if (_ammoProp != null)
                    {
                        var ammo = _ammoProp.GetValue(__instance);
                        if (ammo != null)
                        {
                            // Cache _type property on ammo object
                            if (_ammoTypeProp == null)
                            {
                                _ammoTypeProp = ammo.GetType().GetProperty("_type",
                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                            }

                            if (_ammoTypeProp != null)
                            {
                                CurrentAmmoType = (int)_ammoTypeProp.GetValue(ammo);
                                return;
                            }
                        }
                    }
                }

                CurrentAmmoType = -1;
            }
            catch
            {
                // Silent — runs every frame
            }
        }
    }
}

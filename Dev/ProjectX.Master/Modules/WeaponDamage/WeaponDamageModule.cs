#if !SERVER
using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes;
using RedLoader;
using Sons.Items.Core;
using TheForest.Utils;
using TheForest.Items.Inventory;
using UnityEngine;

namespace ProjectX.Master.Modules.WeaponDamage
{
    /// <summary>
    /// Weapon Damage Module — configurable per-weapon damage multipliers for all weapons.
    /// Expanded from "Less Useless Guns" by SKN The Lisper.
    /// 
    /// Architecture:
    /// - Config values set by Server/Owner, synced to Client via ConfigSyncEvent
    /// - Each client applies multipliers locally via ItemDatabaseManager.AmmoDamageMult
    /// - Solafite plating checked locally per player (cached on weapon swap)
    /// - Weapon inspect is a local cosmetic feature
    /// </summary>
    public static class WeaponDamageModule
    {
        // ======================== WEAPON ID CONSTANTS ========================
        // Ranged
        private const int ID_PISTOL = 355;
        private const int ID_REVOLVER = 386;
        private const int ID_RIFLE = 361;
        private const int ID_SHOTGUN = 358;
        private const int ID_BUCKSHOT = 364;
        private const int ID_SLUG = 363;
        private const int ID_COMPOUND_BOW = 360;
        private const int ID_CRAFTED_BOW = 443;
        private const int ID_CROSSBOW = 365;
        private const int ID_SLINGSHOT = 459;
        private const int ID_STUN_GUN = 353;
        
        // Explosives & Ammo
        private const int ID_GRENADE = 381;
        private const int ID_MOLOTOV = 388;
        private const int ID_STICKY_BOMB = 417;
        private const int ID_C4 = 420;
        private const int ID_STONE_ARROW = 507;
        private const int ID_PRINTED_ARROW = 618;
        private const int ID_CARBON_ARROW = 373;
        private const int ID_FIRE_ARROW = 608;
        private const int ID_SHOCK_ARROW = 609;
        private const int ID_EXPLOSIVE_ARROW = 610;
        
        // Melee
        private const int ID_KATANA = 367;
        private const int ID_MACHETE = 359;
        private const int ID_MODERN_AXE = 356;
        private const int ID_FIRE_AXE = 431;
        private const int ID_TACTICAL_AXE = 379;
        private const int ID_CRAFTED_SPEAR = 474;
        private const int ID_STUN_BATON = 396;
        private const int ID_CRAFTED_CLUB = 477;
        private const int ID_GUITAR = 340;
        private const int ID_CHAINSAW = 394;
        private const int ID_GOLF_PUTTER = 525;
        private const int ID_KNIFE = 380;

        // ======================== STATE ========================
        private static bool _initialized = false;
        
        // Solafite plating cache — only re-check on weapon swap
        private static int _lastHeldItemId = -1;
        private static bool _lastPlatedStatus = false;

        // Item ID → Config getter mapping (DRY approach)
        private static readonly Dictionary<int, Func<float>> _damageMap = new Dictionary<int, Func<float>>
        {
            { ID_PISTOL,        () => Config.WD_PistolDmg.Value },
            { ID_REVOLVER,      () => Config.WD_RevolverDmg.Value },
            { ID_RIFLE,         () => Config.WD_RifleDmg.Value },
            { ID_SHOTGUN,       () => Config.WD_ShotgunDmg.Value },
            { ID_COMPOUND_BOW,  () => Config.WD_CompoundBowDmg.Value },
            { ID_CRAFTED_BOW,   () => Config.WD_CraftedBowDmg.Value },
            { ID_CROSSBOW,      () => Config.WD_CrossbowDmg.Value },
            { ID_SLINGSHOT,     () => Config.WD_SlingshotDmg.Value },
            { ID_STUN_GUN,      () => Config.WD_StunGunDmg.Value },
            { ID_GRENADE,       () => Config.WD_GrenadeDmg.Value },
            { ID_MOLOTOV,       () => Config.WD_MolotovDmg.Value },
            { ID_STICKY_BOMB,   () => Config.WD_StickyBombDmg.Value },
            { ID_C4,            () => Config.WD_C4Dmg.Value },
            { ID_STONE_ARROW,   () => Config.WD_StoneArrowDmg.Value },
            { ID_PRINTED_ARROW, () => Config.WD_PrintedArrowDmg.Value },
            { ID_CARBON_ARROW,  () => Config.WD_CarbonArrowDmg.Value },
            { ID_FIRE_ARROW,    () => Config.WD_FireArrowDmg.Value },
            { ID_SHOCK_ARROW,   () => Config.WD_ShockArrowDmg.Value },
            { ID_EXPLOSIVE_ARROW, () => Config.WD_ExplosiveArrowDmg.Value },
            { ID_KATANA,        () => Config.WD_KatanaDmg.Value },
            { ID_MACHETE,       () => Config.WD_MacheteDmg.Value },
            { ID_MODERN_AXE,    () => Config.WD_ModernAxeDmg.Value },
            { ID_FIRE_AXE,      () => Config.WD_FireAxeDmg.Value },
            { ID_TACTICAL_AXE,  () => Config.WD_TacticalAxeDmg.Value },
            { ID_CRAFTED_SPEAR, () => Config.WD_CraftedSpearDmg.Value },
            { ID_STUN_BATON,    () => Config.WD_StunBatonDmg.Value },
            { ID_CRAFTED_CLUB,  () => Config.WD_CraftedClubDmg.Value },
            { ID_GUITAR,        () => Config.WD_GuitarDmg.Value },
            { ID_CHAINSAW,      () => Config.WD_ChainsawDmg.Value },
            { ID_GOLF_PUTTER,   () => Config.WD_GolfPutterDmg.Value },
            { ID_KNIFE,         () => Config.WD_KnifeDmg.Value },
        };

        // ======================== INIT ========================
        
        /// <summary>
        /// Initialize the module — apply Harmony patch and subscribe to config changes.
        /// Called from MasterPlugin.OnSdkInitialized() (client/owner) or OnGameStart() (server).
        /// </summary>
        public static void Init()
        {
            if (_initialized) return;
            _initialized = true;

            // Apply RangedWeapon.LateUpdate patch for ammo type tracking
            try
            {
                var harmony = new HarmonyLib.Harmony("ProjectX.WeaponDamage");
                WeaponDamagePatches.ApplyPatch(harmony);
            }
            catch (Exception ex)
            {
                RLog.Warning($"[WeaponDamage] Harmony patch failed: {ex.Message}");
            }

            // Subscribe to master toggle changes
            Config.WD_Enabled.OnValueChanged.Subscribe((_, newVal) =>
            {
                ApplyAllDamageSettings();
            });

            RLog.Msg("[WeaponDamage] Module initialized");
        }

        // ======================== APPLY SETTINGS ========================
        
        /// <summary>
        /// Apply all weapon damage multipliers from Config to ItemDatabaseManager.
        /// When WD_Enabled is OFF, resets all to 1.0 (vanilla).
        /// Called on game start and whenever config changes.
        /// </summary>
        public static void ApplyAllDamageSettings()
        {
            try
            {
                bool enabled = Config.WD_Enabled.Value;

                foreach (var kvp in _damageMap)
                {
                    float mult = enabled ? kvp.Value() : 1.0f;
                    
                    // Special case: shotgun uses buckshot calculation when buckshot is loaded
                    if (kvp.Key == ID_SHOTGUN && enabled)
                    {
                        mult = CalculateShotgunDamage();
                    }
                    
                    try
                    {
                        ItemDatabaseManager.ItemById(kvp.Key).AmmoDamageMult = mult;
                    }
                    catch (Exception ex)
                    {
                        RLog.Warning($"[WeaponDamage] Failed to set damage for item {kvp.Key}: {ex.Message}");
                    }
                }

                // Apply buckshot spread angle
                try
                {
                    float spreadAngle = enabled ? Config.WD_BuckshotSpread.Value : 5.0f; // 5.0 = vanilla default
                    ItemDatabaseManager.ItemById(ID_BUCKSHOT)._ammoProperties.ScatterShotConeAngleDegrees = spreadAngle;
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[WeaponDamage] Failed to set buckshot spread: {ex.Message}");
                }

                RLog.Msg($"[WeaponDamage] Applied damage settings (enabled={enabled})");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[WeaponDamage] ApplyAllDamageSettings failed: {ex.Message}");
            }
        }

        // ======================== PER-FRAME UPDATE ========================
        
        /// <summary>
        /// Per-frame update — applies correct damage multiplier for the currently held weapon.
        /// Handles dynamic shotgun buckshot/slug switching, solafite plating bonus,
        /// and weapon inspect keybind.
        /// Called from MasterPlugin.ManagedOnUpdate() — client/owner only (#if !SERVER).
        /// </summary>
        public static void OnWorldUpdate()
        {
            if (!Config.WD_Enabled.Value) return;

            try
            {
                PlayerInventory inventory = LocalPlayer.Inventory;
                if (inventory == null) return;

                var rightHand = inventory.RightHandItem;

                if (rightHand != null)
                {
                    int itemId = rightHand._itemID;

                    // Check if this weapon has a configured multiplier
                    if (_damageMap.TryGetValue(itemId, out var getMultiplier))
                    {
                        float mult = getMultiplier();

                        // Special case: shotgun switches between buckshot and slug dynamically
                        if (itemId == ID_SHOTGUN)
                        {
                            mult = CalculateShotgunDamage();
                        }

                        // Solafite plating bonus — cached per weapon swap
                        if (Config.WD_SolafiteBonus.Value > 1.0f)
                        {
                            if (itemId != _lastHeldItemId)
                            {
                                _lastHeldItemId = itemId;
                                _lastPlatedStatus = IsPlated(rightHand);
                            }

                            if (_lastPlatedStatus)
                            {
                                mult *= Config.WD_SolafiteBonus.Value;
                            }
                        }
                        else if (itemId != _lastHeldItemId)
                        {
                            // Still track weapon swaps even without solafite
                            _lastHeldItemId = itemId;
                            _lastPlatedStatus = false;
                        }

                        // Apply the calculated multiplier
                        ItemDatabaseManager.ItemById(itemId).AmmoDamageMult = mult;
                    }
                }
                else if (_lastHeldItemId != -1)
                {
                    // Weapon was unequipped — reset tracking
                    _lastHeldItemId = -1;
                    _lastPlatedStatus = false;
                }

                // Weapon Inspect keybind
                HandleInspect(inventory);
            }
            catch
            {
                // Silent — runs every frame
            }
        }

        // ======================== HELPERS ========================

        /// <summary>
        /// Calculate shotgun damage based on current ammo type.
        /// Buckshot = ShotgunMult × BuckshotMult, Slug = ShotgunMult.
        /// </summary>
        private static float CalculateShotgunDamage()
        {
            float shotgunMult = Config.WD_ShotgunDmg.Value;
            
            // Check ammo type from Harmony patch
            if (WeaponDamagePatches.CurrentAmmoType == ID_BUCKSHOT)
            {
                return shotgunMult * Config.WD_BuckshotDmg.Value;
            }
            
            return shotgunMult;
        }

        /// <summary>
        /// Check if an item instance has solafite plating.
        /// Uses pure reflection — IL2CPP types (ItemInstance, ItemPlatingItemInstanceModule)
        /// are not available at compile time through DummyDlls.
        /// </summary>
        private static PropertyInfo _modulesProperty;
        private static PropertyInfo _isPlatedProperty;

        private static bool IsPlated(object item)
        {
            try
            {
                if (item == null) return false;

                // Get _modules property (cached first call)
                if (_modulesProperty == null)
                {
                    _modulesProperty = item.GetType().GetProperty("_modules",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (_modulesProperty == null) return false;
                }

                var modules = _modulesProperty.GetValue(item);
                if (modules == null) return false;

                // Iterate modules via GetEnumerator pattern
                var getEnumerator = modules.GetType().GetMethod("GetEnumerator");
                if (getEnumerator == null) return false;

                var enumerator = getEnumerator.Invoke(modules, null);
                if (enumerator == null) return false;

                var moveNext = enumerator.GetType().GetMethod("MoveNext");
                var getCurrent = enumerator.GetType().GetProperty("Current");

                if (moveNext == null || getCurrent == null) return false;

                while ((bool)moveNext.Invoke(enumerator, null))
                {
                    var module = getCurrent.GetValue(enumerator);
                    if (module == null) continue;

                    // Check type name (Pattern #14 — is/as don't work in IL2CPP)
                    string typeName = module.GetType().Name;
                    if (typeName.Contains("ItemPlatingItemInstanceModule"))
                    {
                        // Get IsPlated property (cached)
                        if (_isPlatedProperty == null)
                        {
                            _isPlatedProperty = module.GetType().GetProperty("IsPlated",
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        }

                        if (_isPlatedProperty != null)
                        {
                            var val = _isPlatedProperty.GetValue(module);
                            return val != null && (bool)val;
                        }
                    }
                }
            }
            catch
            {
                // Silent — plating check is non-critical
            }

            return false;
        }

        /// <summary>
        /// Handle weapon inspect keybind — unequip and re-equip to replay first-equip animation.
        /// Uses reflection for UnequipItemAtSlot and TryEquip to avoid assembly dependencies.
        /// </summary>
        private static MethodInfo _unequipMethod;
        private static MethodInfo _tryEquipMethod;

        private static void HandleInspect(PlayerInventory inventory)
        {
            try
            {
                // Check if inspect key is pressed
                string keyValue = ((ConfigEntry<string>)(object)Config.WD_InspectKey).Value;
                if (keyValue == null || !Input.GetKeyDown(keyValue))
                    return;

                // Cache methods on first use
                if (_unequipMethod == null)
                {
                    _unequipMethod = typeof(PlayerInventory).GetMethod("UnequipItemAtSlot",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                if (_tryEquipMethod == null)
                {
                    _tryEquipMethod = typeof(PlayerInventory).GetMethod("TryEquip",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }

                var rightHand = inventory.RightHandItem;
                if (rightHand != null)
                {
                    int itemId = rightHand._itemID;
                    // Unequip right hand (slot 0) then re-equip
                    _unequipMethod?.Invoke(inventory, new object[] { 0, false, true, false, false, false, false });
                    _tryEquipMethod?.Invoke(inventory, new object[] { itemId, false, true, true });
                }
                else if (Config.WD_InspectLeftHand.Value)
                {
                    var leftHand = inventory.LeftHandItem;
                    if (leftHand != null)
                    {
                        int itemId = leftHand._itemID;
                        // Unequip left hand (slot 1) then re-equip
                        _unequipMethod?.Invoke(inventory, new object[] { 1, false, true, false, false, false, false });
                        _tryEquipMethod?.Invoke(inventory, new object[] { itemId, false, true, true });
                    }
                }
            }
            catch
            {
                // Silent — inspect is non-critical
            }
        }
    }
}
#endif


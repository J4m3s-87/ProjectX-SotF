#if !SERVER
using System;
using RedLoader;
using SonsSdk;
using Sons.Items.Core;
using UnityEngine;

namespace ProjectX.Master.Modules.Stack
{
    /// <summary>
    /// Stack Module - Full per-item stack configuration matching original StackMod.
    /// Uses ItemDatabaseManager.ItemById() for precise item ID targeting.
    /// </summary>
    public static class StackModule
    {
        private static bool _applied = false;
        
        public static void Init()
        {
            SdkEvents.OnGameActivated.Subscribe(OnGameActivated);
            SdkEvents.OnInWorldUpdate.Subscribe(OnUpdate);
            RLog.Msg("[Stack] Module Initialized");
        }

        private static void OnGameActivated()
        {
            _applied = false;
            Apply();
        }
        
        private static void OnUpdate()
        {
            if (!_applied)
            {
                Apply();
            }
        }

        /// <summary>
        /// Apply stack settings from Config.
        /// Priority: InfiniteInventory > Per-Item Config
        /// Future: Server sync will add host-enforced values
        /// </summary>
        public static void Apply()
        {
            try
            {
                var items = ItemDatabaseManager.Items;
                if (items == null || items.Count == 0) return;
                
                // Priority 1: Infinite Stacks — set all consumables & ammo to 1000
                if (Config.InfiniteInventory.Value)
                {
                    const int INF_STACK = 1000;
                    int count = 0;
                    
                    // All consumable/ammo/arrow item IDs from per-item config
                    int[] infiniteStackIds = {
                        // Ammo (all types)
                        646, 648, 644, 650, 647, 651, 642, 643, 652, 362, 363, 364, 369, 387,
                        // Arrows & Bolts
                        507, 618, 373, 368,
                        // Throwables
                        388, 381, 417, 474, 440,
                        // Medication
                        437, 455, 456, 461, 462,
                        // Food & Drinks
                        517, 433, 436, 438, 441, 439, 434, 464, 421, 425, 466, 401, 570, 571, 569,
                        // Crafting Items
                        419, 502, 418, 527, 403, 416, 420, 414, 410,
                        // Sticks & Rocks
                        392, 393, 476,
                        // Printing Items
                        390, 553, 560, 657,
                        // Electric Items
                        634, 661, 635, 590,
                        // Animal Drops
                        506, 472, 479,
                        // Body Parts & Bones
                        482, 480, 481, 430, 405,
                        // Armor
                        593, 494, 727, 554, 473, 519,
                        // Plants & Seeds
                        484, 451, 454, 595, 445, 465, 449, 453, 400, 594, 450, 399, 398, 447, 397, 448, 446, 452,
                        596, 598, 599, 605, 600, 601, 602, 603, 604, 606, 597,
                        // Misc consumables
                        664, 508, 626, 504, 469, 518, 524, 496,
                        // Zipline
                        523
                    };
                    
                    foreach (int id in infiniteStackIds)
                    {
                        try
                        {
                            var item = ItemDatabaseManager.ItemById(id);
                            if (item != null)
                            {
                                item.MaxAmount = INF_STACK;
                                count++;
                            }
                        }
                        catch { }
                    }
                    
                    _applied = true;
                    RLog.Msg($"[Stack] Infinite Stacks ON: set {count} consumable/ammo items to {INF_STACK}");
                    return;
                }
                
                // Priority 2: Per-Item Configuration (from native Settings menu)
                ApplyPerItemConfig();
                _applied = true;
            }
            catch (Exception ex)
            {
                RLog.Debug($"[Stack] Apply deferred: {ex.Message}");
            }
        }
        
        private static void ApplyPerItemConfig()
        {
            int count = 0;
            
            // Helper to apply stack by item ID
            void SetStack(int id, string value)
            {
                if (string.IsNullOrEmpty(value)) return;
                if (!int.TryParse(value, out int val)) return;
                val = Mathf.Clamp(val, 1, 999999999);
                try
                {
                    var item = ItemDatabaseManager.ItemById(id);
                    if (item != null)
                    {
                        item.MaxAmount = val;
                        count++;
                    }
                }
                catch { }
            }
            
            // Helper for multiple IDs
            void SetStackMulti(int[] ids, string value)
            {
                if (string.IsNullOrEmpty(value)) return;
                if (!int.TryParse(value, out int val)) return;
                val = Mathf.Clamp(val, 1, 999999999);
                foreach (int id in ids)
                {
                    try
                    {
                        var item = ItemDatabaseManager.ItemById(id);
                        if (item != null)
                        {
                            item.MaxAmount = val;
                            count++;
                        }
                    }
                    catch { }
                }
            }
            
            // ===== Crafting Items =====
            SetStack(419, Config.MaxTapeStack?.Value);      // Duct Tape
            SetStack(502, Config.MaxClothStack?.Value);     // Cloth
            SetStack(418, Config.MaxWireStack?.Value);      // Wire
            SetStack(527, Config.MaxBatteriesStack?.Value); // Batteries
            SetStack(403, Config.MaxRopeStack?.Value);      // Rope
            SetStack(416, Config.MaxBoardStack?.Value);     // Circuit Board
            SetStack(420, Config.MaxC4Stack?.Value);        // C4
            SetStack(414, Config.MaxVodkaStack?.Value);     // Vodka
            SetStack(410, Config.MaxWatchStack?.Value);     // Watch
            SetStack(502, Config.MaxCoinStack?.Value);      // Coins (same ID as cloth in original)
            
            // ===== Sticks & Rocks =====
            SetStack(392, Config.MaxStickStack?.Value);      // Stick
            SetStack(393, Config.MaxRockStack?.Value);       // Rock
            SetStack(476, Config.MaxSmallRockStack?.Value);  // Small Rock
            
            // ===== Printing Items =====
            SetStack(390, Config.MaxResinStack?.Value);      // Resin
            SetStack(553, Config.MaxMeshStack?.Value);       // Tech Mesh
            SetStack(560, Config.MaxHookStack?.Value);       // Grappling Hook
            SetStack(657, Config.MaxGpsCaseStack?.Value);    // GPS Case
            
            // ===== Electric Items =====
            SetStack(634, Config.MaxSolarStack?.Value);      // Solar Panel
            SetStack(661, Config.MaxBatteryStack?.Value);    // Golfcart Battery
            SetStack(635, Config.MaxBulbStack?.Value);       // Light Bulb
            SetStack(590, Config.MaxRadioStack?.Value);      // Radio
            
            // ===== Medication =====
            SetStack(437, Config.MaxMedsStack?.Value);            // Meds
            SetStack(455, Config.MaxHealthMixStack?.Value);       // Health Mix
            SetStack(456, Config.MaxHealthMixPlusStack?.Value);   // Health+ Mix
            SetStack(461, Config.MaxEnergyMixStack?.Value);       // Energy Mix
            SetStack(462, Config.MaxEnergyMixPlusStack?.Value);   // Energy+ Mix
            
            // ===== Food & Drinks =====
            SetStack(517, Config.MaxPotStack?.Value);         // Cooking Pot
            SetStack(433, Config.MaxMeatStack?.Value);        // Meat
            SetStack(436, Config.MaxFishStack?.Value);        // Fish
            SetStack(438, Config.MaxMreStack?.Value);         // MRE
            SetStack(441, Config.MaxEnergyBarStack?.Value);   // Energy Bar
            SetStack(439, Config.MaxEnergyDrinkStack?.Value); // Energy Drink
            SetStack(434, Config.MaxCannedFoodStack?.Value);  // Canned Food
            SetStack(464, Config.MaxCatFoodStack?.Value);     // Cat Food
            SetStack(421, Config.MaxRamenStack?.Value);       // Ramen
            SetStack(425, Config.MaxCrunchieStack?.Value);    // Crunchie
            SetStack(466, Config.MaxOysterStack?.Value);      // Oyster
            SetStack(401, Config.MaxEggStack?.Value);         // Egg
            SetStack(570, Config.MaxSteakBiteStack?.Value);   // Steak Bite
            SetStack(571, Config.MaxBaconBiteStack?.Value);   // Bacon Bite
            SetStack(569, Config.MaxBrainBiteStack?.Value);   // Brain Bite
            
            // ===== Animal Drops =====
            SetStack(506, Config.MaxShellStack?.Value);       // Shell
            SetStack(472, Config.MaxHideStack?.Value);        // Hide
            SetStack(479, Config.MaxFeatherStack?.Value);     // Feather
            
            // ===== Throwables =====
            SetStack(388, Config.MaxMolotovStack?.Value);     // Molotov
            SetStack(381, Config.MaxGrenadeStack?.Value);     // Grenade
            SetStack(417, Config.MaxBombStack?.Value);        // Sticky Bomb
            SetStack(474, Config.MaxSpearStack?.Value);       // Spear
            SetStack(440, Config.MaxFlareStack?.Value);       // Flare
            
            // ===== Body Parts & Bones =====
            SetStack(482, Config.MaxHeadStack?.Value);        // Head
            SetStack(480, Config.MaxArmStack?.Value);         // Arm
            SetStack(481, Config.MaxLegStack?.Value);         // Leg
            SetStack(430, Config.MaxSkullStack?.Value);       // Skull
            SetStack(405, Config.MaxBoneStack?.Value);        // Bone
            
            // ===== Armor =====
            SetStack(593, Config.MaxCreepyArmorStack?.Value);    // Creepy Armor
            SetStack(494, Config.MaxBoneArmorStack?.Value);      // Bone Armor
            SetStack(727, Config.MaxSolafiteArmorStack?.Value);  // Solafite Armor
            SetStack(554, Config.MaxTechArmorStack?.Value);      // Tech Armor
            SetStack(473, Config.MaxLeafArmorStack?.Value);      // Leaf Armor
            SetStack(519, Config.MaxHideArmorStack?.Value);      // Hide Armor
            
            // ===== Ammo (all types) =====
            int[] ammoIds = { 646, 648, 644, 650, 647, 651, 642, 643, 652, 362, 363, 364, 369, 387 };
            SetStackMulti(ammoIds, Config.MaxAmmoStack?.Value);
            
            // Arrows & Bolts
            SetStack(507, Config.MaxStoneArrowStack?.Value);      // Stone Arrow
            SetStack(618, Config.MaxPrintedArrowStack?.Value);    // Printed Arrow
            SetStack(373, Config.MaxCarbonArrowStack?.Value);     // Carbon Arrow
            SetStack(368, Config.MaxBoltStack?.Value);            // Crossbow Bolt
            SetStack(523, Config.MaxZiplineStack?.Value);         // Zipline
            
            // ===== Plants & Seeds =====
            SetStack(484, Config.MaxLeafStack?.Value);            // Leaf
            
            // All plant types
            int[] plantIds = { 451, 454, 595, 445, 465, 449, 453, 400, 594, 450, 399, 398, 447, 397, 448, 446, 452 };
            SetStackMulti(plantIds, Config.MaxPlantStack?.Value);
            
            // All seed types
            int[] seedIds = { 596, 454, 598, 599, 605, 600, 601, 602, 603, 604, 605, 606, 597 };
            SetStackMulti(seedIds, Config.MaxSeedStack?.Value);
            
            // ===== Misc =====
            SetStack(664, Config.MaxSolafiteStack?.Value);        // Solafite
            SetStack(508, Config.MaxPouchStack?.Value);           // Skin Pouch
            SetStack(626, Config.MaxGliderStack?.Value);          // Hang Glider
            SetStack(504, Config.MaxTarpStack?.Value);            // Tarp
            SetStack(469, Config.MaxAirTankStack?.Value);         // Air Tank
            SetStack(518, Config.MaxPaperTargetStack?.Value);     // Paper Target
            SetStack(524, Config.MaxGolfBallStack?.Value);        // Golf Ball
            SetStack(496, Config.MaxCashStack?.Value);            // Cash
            
            RLog.Msg($"[Stack] Applied per-item stack config to {count} items");
        }
        
        public static void ForceReapply()
        {
            _applied = false;
            Apply();
        }
        /// <summary>
        /// Reset ALL item MaxAmounts to safe defaults before addallitems.
        /// Reads the game's original _maxAmount (serialized, private) via reflection.
        /// Falls back to a safe cap of 99 if reflection fails.
        /// Prevents 999M fills from residual Infinite Stacks toggle state.
        /// </summary>
        public static void ResetToSafeDefaults()
        {
            try
            {
                var items = ItemDatabaseManager.Items;
                if (items == null || items.Count == 0) return;
                
                // Try to get the private _maxAmount field (original game default)
                var f_maxAmount = HarmonyLib.AccessTools.Field(typeof(Sons.Items.Core.ItemData), "_maxAmount");
                
                int resetCount = 0;
                foreach (var item in items)
                {
                    if (item == null || item.MaxAmount <= 9999) continue;
                    
                    // Read original default from serialized field
                    int safeDefault = 99;
                    if (f_maxAmount != null)
                    {
                        try
                        {
                            int original = (int)f_maxAmount.GetValue(item);
                            if (original > 0 && original <= 9999)
                                safeDefault = original;
                        }
                        catch { }
                    }
                    
                    item.MaxAmount = safeDefault;
                    resetCount++;
                }
                
                if (resetCount > 0)
                    RLog.Msg($"[Stack] Reset {resetCount} items to safe defaults (for Fill Inventory)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[Stack] ResetToSafeDefaults failed: {ex.Message}");
            }
        }
    }
}
#endif

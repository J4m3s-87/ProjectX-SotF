#if !SERVER
using System;
using HarmonyLib;
using RedLoader;
using Sons.Crafting;
using UnityEngine;

namespace ProjectX.Master.Modules.Crafting
{
    /// <summary>
    /// Controls backpack/inventory crafting speed via manual Harmony patch.
    /// 
    /// Patches CraftingCog.OnCraftBeginEvent to set CraftingSystem speed properties
    /// before the crafting animation plays.
    /// 
    /// Uses the same manual patching pattern as LootRespawn:
    ///   _harmony.PatchAll(typeof(CraftingCogPatch))
    /// 
    /// This avoids HarmonyPatchAll=true which corrupts IL2CPP vtables.
    /// </summary>
    public static class CraftingSpeed
    {
        private static bool _initialized = false;
        private static HarmonyLib.Harmony _harmony;
        private static bool _enabled = true;
        private static float _speedMultiplier = 5f;

        public static bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        public static float SpeedMultiplier
        {
            get => _speedMultiplier;
            set => _speedMultiplier = Mathf.Clamp(value, 1f, 10f);
        }

        public static void Init()
        {
            if (_initialized) return;
            _initialized = true;

            try
            {
                _harmony = new HarmonyLib.Harmony("ProjectX.CraftingSpeed");
                _harmony.PatchAll(typeof(CraftingCogPatch));
                RLog.Msg($"[CraftingSpeed] Harmony patch applied on CraftingCog.OnCraftBeginEvent (multiplier={_speedMultiplier}x)");
            }
            catch (Exception ex)
            {
                RLog.Error($"[CraftingSpeed] Failed to apply Harmony patch: {ex.Message}");
                RLog.Error($"[CraftingSpeed] Stack: {ex.StackTrace}");
            }
        }

        // Overload for backward compatibility with MasterPlugin
        public static void Init(HarmonyLib.Harmony harmony)
        {
            Init();
        }

        /// <summary>
        /// Called from the Harmony Prefix on CraftingCog.OnCraftBeginEvent.
        /// Sets crafting speed properties on the CraftingSystem before the craft begins.
        /// </summary>
        internal static void OnCraftBegin(CraftingCog cog, CraftingRecipe recipe)
        {
            if (!_enabled || cog == null) return;

            try
            {
                var craftingSystem = cog._craftingSystem;
                if (craftingSystem == null) return;

                // Set the consecutive crafts counter to skip the ramp-up period
                craftingSystem._consecutiveCraftsOfTheSameItem = 100;
                
                // Set speed increase per craft (irrelevant when at max already)
                craftingSystem._consecutiveItemCraftingSpeedIncrease = 0.25f;
                
                // Set max crafting speed — this is the multiplier that matters
                craftingSystem._maxConsecutiveItemCraftingSpeed = _speedMultiplier;

                // Special case: if crafting a fish trap (ID 707), don't speed up
                // (mirrors original FasterCrafting mod behavior)
                if (recipe != null && recipe._resultingItems != null)
                {
                    foreach (var item in recipe._resultingItems)
                    {
                        if (item != null && item.Id == 707)
                        {
                            craftingSystem._maxConsecutiveItemCraftingSpeed = 1f;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[CraftingSpeed] OnCraftBegin error: {ex.Message}");
            }
        }
    }

    // ===== Harmony Patch (manually applied, NOT auto-discovered) =====

    [HarmonyPatch(typeof(Sons.Crafting.CraftingCog), "OnCraftBeginEvent")]
    internal static class CraftingCogPatch
    {
        [HarmonyPrefix]
        private static void Prefix(Sons.Crafting.CraftingCog __instance, Sons.Crafting.CraftingRecipe recipe)
        {
            CraftingSpeed.OnCraftBegin(__instance, recipe);
        }
    }
}
#endif

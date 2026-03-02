#if !SERVER
using System;
using System.Collections.Generic;
using Construction;
using HarmonyLib;
using RedLoader;
using Sons.Crafting.Structures;

namespace ProjectX.Master.Modules.StructureDurability
{
    /// <summary>
    /// Structure Durability module.
    /// 
    /// Main structures (walls, floors): POSTFIX on GetStructureInfo multiplies MaxHealth
    /// using base-tracking dict to prevent compounding on repeated calls.
    /// 
    /// Screw structures (shelves, traps, spiked walls): Awake POSTFIX caches instances
    /// and applies multiplier via direct field access (ssd._structureHp, ssd._currentHp).
    /// Direct field access is the proven pattern — used by ScaryCross (MakeCrossScary.cs:745)
    /// and the original PrefabRepair mod (ScrewStructureRepairHandler.cs:47-66).
    /// AccessTools.Field returns null for these fields in some contexts.
    /// </summary>
    public static class StructureDurabilityModule
    {
        private static bool _logged = false;

        /// <summary>
        /// Cached StructureDestructionManager instance from POSTFIX.
        /// Used by RepairAllStructures.
        /// </summary>
        public static StructureDestructionManager CachedSDM { get; private set; }

        /// <summary>
        /// All ScrewStructureDestruction instances tracked via Awake POSTFIX.
        /// Used for durability application and repair (FindObjectsOfType is IL2CPP stripped).
        /// Pattern: Harmony Awake Interception (technical_retrospective.md §7).
        /// </summary>
        public static List<ScrewStructureDestruction> TrackedScrewStructures { get; } 
            = new List<ScrewStructureDestruction>();

        // Track base MaxHealth per Structure (by IL2CPP pointer) — prevents compounding
        private static Dictionary<IntPtr, float> _structureBaseMaxHealth = new Dictionary<IntPtr, float>();
        // Track base _structureHp per ScrewStructureDestruction — prevents compounding
        private static Dictionary<IntPtr, int> _screwBaseHp = new Dictionary<IntPtr, int>();
        private static float _lastLoggedMultiplier = -1f;

        public static void Init()
        {
            try
            {
                var harmony = new HarmonyLib.Harmony("ProjectX.StructureDurability");
                var sdmType = typeof(StructureDestructionManager);

                // Patch 1: POSTFIX on GetStructureInfo — multiply health with base tracking
                var getInfoMethod = AccessTools.Method(sdmType, "GetStructureInfo");
                if (getInfoMethod != null)
                {
                    var postfix = typeof(StructureDurabilityModule).GetMethod(
                        nameof(PostfixGetStructureInfo),
                        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                    harmony.Patch(getInfoMethod, postfix: new HarmonyMethod(postfix));
                    RLog.Msg("[StructureDurability] Harmony POSTFIX on GetStructureInfo — PATCHED OK");
                }
                else
                {
                    RLog.Warning("[StructureDurability] GetStructureInfo method NOT found");
                }

                // Patch 2: ScrewStructureDestruction.Awake POSTFIX
                // Pattern: ScaryCrossModule.cs:46 — proven safe from Master assembly
                try
                {
                    var awakeMethod = AccessTools.Method(typeof(ScrewStructureDestruction), "Awake");
                    if (awakeMethod != null)
                    {
                        var screwPostfix = typeof(StructureDurabilityModule).GetMethod(
                            nameof(PostfixScrewAwake),
                            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                        harmony.Patch(awakeMethod, postfix: new HarmonyMethod(screwPostfix));
                        RLog.Msg("[StructureDurability] Harmony POSTFIX on ScrewStructureDestruction.Awake — PATCHED OK");
                    }
                    else
                    {
                        RLog.Warning("[StructureDurability] ScrewStructureDestruction.Awake not found");
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[StructureDurability] Screw Awake patch failed: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[StructureDurability] Init failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Called when the slider changes. Retroactively applies to cached screw structures.
        /// Main structures are handled by the GetStructureInfo POSTFIX (base tracking).
        /// </summary>
        public static void Apply()
        {
            float multiplier = Config.StructureDurabilityMultiplier.Value;
            
            if (Math.Abs(multiplier - _lastLoggedMultiplier) > 1f)
            {
                RLog.Msg($"[StructureDurability] Multiplier set to {multiplier:F1}x");
                _lastLoggedMultiplier = multiplier;
            }

            // Retroactively apply to ALL cached ScrewStructureDestruction instances
            // Uses direct field access — proven by ScaryCross (MakeCrossScary.cs:745)
            TrackedScrewStructures.RemoveAll(s => s == null);
            int updated = 0;

            foreach (var ssd in TrackedScrewStructures)
            {
                try
                {
                    if (ssd == null) continue;
                    var il2cppObj = (Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)(object)ssd;
                    IntPtr ptr = il2cppObj.Pointer;
                    if (ptr == IntPtr.Zero) continue;

                    // Get or store the unmultiplied base HP
                    int baseHp;
                    if (!_screwBaseHp.TryGetValue(ptr, out baseHp))
                    {
                        baseHp = ssd._structureHp;
                        _screwBaseHp[ptr] = baseHp;
                    }

                    int newMaxHp = (int)(baseHp * multiplier);
                    if (newMaxHp < 1) newMaxHp = 1;

                    ssd._structureHp = newMaxHp;

                    // If at full health or at old base, update current HP too
                    int curHp = ssd._currentHp;
                    if (curHp >= newMaxHp || curHp == baseHp)
                    {
                        ssd._currentHp = newMaxHp;
                    }
                    updated++;
                }
                catch (Exception ex) 
                { 
                    RLog.Warning($"[StructureDurability] Apply screw failed: {ex.Message}");
                }
            }

            if (updated > 0)
                RLog.Msg($"[StructureDurability] Applied x{multiplier:F0} to {updated} screw structures");
        }

        /// <summary>
        /// Repair all tracked screw structures by setting _currentHp = _structureHp.
        /// Uses direct field access — same pattern as PrefabRepair mod (ScrewStructureRepairHandler.cs:47-66).
        /// </summary>
        public static int RepairAllScrewStructures()
        {
            TrackedScrewStructures.RemoveAll(s => s == null);

            int repaired = 0;
            int total = 0;

            foreach (var ssd in TrackedScrewStructures)
            {
                try
                {
                    if (ssd == null) continue;
                    total++;

                    int maxHp = ssd._structureHp;
                    int curHp = ssd._currentHp;

                    if (curHp < maxHp)
                    {
                        ssd._currentHp = maxHp;
                        repaired++;
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[RepairAll] Screw repair item failed: {ex.Message}");
                }
            }

            RLog.Msg($"[RepairAll] Screw structures: repaired {repaired}/{total}");
            return repaired;
        }

        // ===================== HARMONY PATCHES =====================

        /// <summary>
        /// POSTFIX on StructureDestructionManager.GetStructureInfo.
        /// Multiplies MaxHealth using BASE tracking to prevent compounding.
        /// Stores the original MaxHealth the first time a structure is seen,
        /// then always outputs: baseMaxHealth × multiplier.
        /// Health is scaled proportionally to maintain damage percentage.
        /// </summary>
        public static void PostfixGetStructureInfo(StructureDestructionManager __instance, ref StructureInfo __result, Structure structure)
        {
            try
            {
                if (__instance != null)
                    CachedSDM = __instance;

                if (!_logged)
                {
                    RLog.Msg("[StructureDurability] POSTFIX firing — GetStructureInfo intercepted");
                    _logged = true;
                }

                if (structure == null) return;
                if (__result.MaxHealth <= 0f) return;

                float multiplier = Config.StructureDurabilityMultiplier.Value;
                if (multiplier < 1.01f && multiplier > 0.99f) return;

                var il2cppObj = (Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)(object)structure;
                IntPtr ptr = il2cppObj.Pointer;

                float baseMaxHealth;
                if (!_structureBaseMaxHealth.TryGetValue(ptr, out baseMaxHealth))
                {
                    baseMaxHealth = __result.MaxHealth;
                    _structureBaseMaxHealth[ptr] = baseMaxHealth;
                }

                float newMaxHealth = baseMaxHealth * multiplier;
                float healthPercent = (__result.MaxHealth > 0f) ? (__result.Health / __result.MaxHealth) : 1f;
                float newHealth = newMaxHealth * healthPercent;

                __result.MaxHealth = newMaxHealth;
                __result.Health = newHealth;
            }
            catch { }
        }

        /// <summary>
        /// POSTFIX on ScrewStructureDestruction.Awake.
        /// Caches instance and applies durability multiplier at spawn time.
        /// Uses direct field access — proven by ScaryCross and PrefabRepair.
        /// </summary>
        public static void PostfixScrewAwake(ScrewStructureDestruction __instance)
        {
            try
            {
                if (__instance == null) return;

                // Cache this instance (Harmony Awake Interception pattern, §7)
                if (!TrackedScrewStructures.Contains(__instance))
                    TrackedScrewStructures.Add(__instance);

                // Track base HP using direct field access
                var il2cppObj = (Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)(object)__instance;
                IntPtr ptr = il2cppObj.Pointer;

                int baseHp = __instance._structureHp;
                _screwBaseHp[ptr] = baseHp;

                float multiplier = Config.StructureDurabilityMultiplier.Value;
                if (multiplier < 1.01f && multiplier > 0.99f) return;

                int newHp = (int)(baseHp * multiplier);
                if (newHp < 1) newHp = 1;

                __instance._structureHp = newHp;
                __instance._currentHp = newHp;

                RLog.Msg($"[StructureDurability] ScrewAwake: {baseHp} → {newHp} HP (x{multiplier:F0})");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[StructureDurability] ScrewAwake failed: {ex.Message}");
            }
        }
    }
}
#endif

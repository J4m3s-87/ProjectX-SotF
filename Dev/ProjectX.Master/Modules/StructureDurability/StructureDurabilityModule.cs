using HarmonyLib;
using ProjectX.Master;
using RedLoader;
using Construction; // Assuming Structure is here or Sons.Construction
using Sons.Crafting.Structures;
using UnityEngine;

namespace ProjectX.Master.Modules.StructureDurability
{
    public static class StructureDurabilityModule
    {
        public static void Init()
        {
            RLog.Msg("StructureDurability Module Initialized.");
        }
    }

    // Using AccessTools/Harmony manually if types are hard to reach, but let's try direct Patching first if references allow.
    // Based on decompiled code: StructureDestructionManager is likely in a namespace we have.
    
    [HarmonyPatch(typeof(StructureDestructionManager))]
    public static class StructureDestructionManagerPatches
    {
        [HarmonyPatch("GetStructureInfo")]
        [HarmonyPostfix]
        public static void PostfixGetStructureInfo(ref StructureInfo __result, Structure structure)
        {
            try
            {
                if (structure == null) return;
                if (__result.MaxHealth <= 0f) return; // guard against default/zeroed structs

                float multiplier = Config.StructureDurabilityMultiplier.Value;
                if (multiplier > 1.01f || multiplier < 0.99f)
                {
                    __result.MaxHealth *= multiplier;
                    __result.Health *= multiplier;
                }
            }
            catch { /* Never crash the game from a postfix */ }
        }
    }
}

using RedLoader;

namespace ProjectX.Master.Modules.PrefabRepair
{
    /// <summary>
    /// PrefabRepair Module - DISABLED in Master DLL
    /// 
    /// Harmony patches on ScrewStructureDestruction inside the Master assembly
    /// cause IL2CPP vtable corruption (Tab/backpack crash). This is NOT a problem
    /// with the patches themselves — the original standalone PrefabRepair.dll uses
    /// the same patches and works fine.
    /// 
    /// The issue is assembly-level: having these patches + [RegisterTypeInIl2Cpp]
    /// MonoBehaviour in the same assembly as the rest of Project X causes crashes.
    /// 
    /// SOLUTION: Use the original standalone PrefabRepair.dll alongside Project X.
    /// </summary>
    public static class PrefabRepairModule
    {
        public static void Init()
        {
            RLog.Msg("[PrefabRepair] Delegated to standalone PrefabRepair.dll");
        }
    }
}

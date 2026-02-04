using System.Collections.Generic;
using RedLoader;
using UnityEngine;
using HarmonyLib;
using Construction; // For DoorLock

namespace ProjectX.Master.Modules.OpenSesame
{
    public static class OpenSesameModule
    {
        private static bool _initialized = false;
        private static Transform _iconTr;
        
        // Dictionary embedded from original mod
        private static readonly Dictionary<string, Vector3> _positions = new Dictionary<string, Vector3>
        {
            { "front", new Vector3(0f, 0f, 0.1f) },
            { "behind", new Vector3(0.1f, 0f, -0.3f) }
        };

        public static void Init()
        {
            if (_initialized) return;
            // Harmony patches are applied globally by MasterPlugin if we use HarmonyPatchAll, 
            // BUT since we are manually structuring modules, let's verify if we need manual patching.
            // MasterPlugin usually does `_harmony.PatchAll()`.
            _initialized = true;
        }

        [HarmonyPatch(typeof(DoorLock))]
        public static class DoorLockPatch
        {
            [HarmonyPatch("IsInFront")]
            [HarmonyPostfix]
            public static void PostfixIsInFront(DoorLock __instance, ref bool __result)
            {
                if (!Config.OpenSesameEnabled.Value) return;

                if (__instance && __instance._icon)
                {
                    _iconTr = __instance._icon.gameObject.transform;
                    
                    // Logic to move icon if we are behind the door but it's "locked" logic-wise
                    // Original: if (!__result && z > 0) -> move to "behind"
                    
                    if (!__result && _iconTr.localPosition.z > 0f)
                    {
                        _iconTr.localPosition = _positions["behind"];
                    }
                    else if (__result && _iconTr.localPosition.z < 0f)
                    {
                        _iconTr.localPosition = _positions["front"];
                    }
                }
                
                // Force Allow
                __result = true;
            }
        }
    }
}

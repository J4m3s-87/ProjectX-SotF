using RedLoader;
using UnityEngine;

namespace ProjectX.Master.Modules.WaterCollectors
{
    /// <summary>
    /// Stub only — all logic moved to WaterCollectorsModule (static class).
    /// 
    /// This MonoBehaviour was a victim of the MonoBehaviour Callback Trap:
    /// Start() and Update() never fire on IL2CPP-injected types.
    /// Kept as empty type for IL2CPP registration.
    /// </summary>
    [RegisterTypeInIl2Cpp]
    public class FireProximityTrigger : MonoBehaviour
    {
        // Intentionally empty — all logic in WaterCollectorsModule
    }
}

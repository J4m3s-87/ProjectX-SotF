using RedLoader;
using UnityEngine;

namespace ProjectX.Master.Modules.MeatDryer
{
    /// <summary>
    /// Legacy stub — kept for compatibility. All drying logic is now in MeatDryerModule (static).
    /// The MonoBehaviour lifecycle callbacks (Start/Update) don't fire on IL2CPP-injected types,
    /// so all logic was moved to MeatDryerModule.OnGuiTick() + Harmony Postfix.
    /// </summary>
    [RegisterTypeInIl2Cpp]
    public class MeatDryerController : MonoBehaviour
    {
        // Intentionally empty — all logic in MeatDryerModule
    }
}

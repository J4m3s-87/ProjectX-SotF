using RedLoader;

namespace ProjectX.Master.Modules.OpenSesame
{
    /// <summary>
    /// OpenSesame Module - DISABLED in Master DLL
    /// 
    /// Manual Harmony patches in the Master assembly may contribute to IL2CPP
    /// vtable corruption when opening backpack. The original standalone
    /// OpenSesame.dll uses the same DoorLock.IsInFront patch and works fine.
    /// 
    /// SOLUTION: Use the original standalone OpenSesame.dll alongside Project X.
    /// </summary>
    public static class OpenSesameModule
    {
        public static void Init()
        {
            RLog.Msg("[OpenSesame] Delegated to standalone OpenSesame.dll");
        }
    }
}

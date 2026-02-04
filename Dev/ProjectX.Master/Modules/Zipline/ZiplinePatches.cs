using HarmonyLib;
using Sons.Weapon;
using ProjectX.Master;
using RedLoader;

namespace ProjectX.Master.Modules.Zipline
{
    [HarmonyPatch(typeof(RopeGunController), "Initialize")]
    public class RopeGunControllerInitializePatch
    {
        [HarmonyPostfix]
        public static void Postfix(RopeGunController __instance)
        {
            if (__instance == null) return;
            
            // Override max length and firing range from Config using Reflection
            AccessTools.Field(typeof(RopeGunController), "_maxRopeLength").SetValue(__instance, Config.MaxZipLineLength.Value);
            AccessTools.Field(typeof(RopeGunController), "_maxFiringRange").SetValue(__instance, Config.MaxShootingDistance.Value);
            
            // RLog.Msg($"[Zipline] Set RopeLength: {Config.MaxZipLineLength.Value}, FiringRange: {Config.MaxShootingDistance.Value}");
        }
    }
}

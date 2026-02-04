using RedLoader;
using ProjectX.Master;
using SonsSdk;
using Sons.Weapon;
using TheForest.Utils;
using HarmonyLib;

namespace ProjectX.Master.Modules.Zipline
{
    /// <summary>
    /// Zipline Module - Extends zipline max length and firing range
    /// Original mod used Harmony patches on RopeGunController.Initialize
    /// We use polling since HarmonyPatchAll is disabled
    /// </summary>
    public static class ZiplineModule
    {
        private static bool _applied = false;
        
        public static void Init()
        {
            RLog.Msg("[Zipline] Module Initialized (Always-On)");
            SdkEvents.OnGameStart.Subscribe(OnGameStart);
            SdkEvents.OnInWorldUpdate.Subscribe(OnUpdate);
        }

        private static void OnGameStart()
        {
            _applied = false;
            ApplySettings();
        }
        
        private static void OnUpdate()
        {
            // Keep applying until successful
            if (!_applied)
            {
                ApplySettings();
            }
        }

        private static void ApplySettings()
        {
            try
            {
                // Apply RopeBridge length (construction)
                // Config stores user-friendly METERS, divide by 400 for internal API
                var target = GameState.ConstructionManager?._constructionSettings?.Targeting;
                if (target != null)
                {
                    // User enters meters (e.g., 200), internal API needs meters/400 (e.g., 0.5)
                    target.MaxRopeBridgeLength = Config.MaxRopeBridgeLength.Value / 400f;
                }
                
                // Apply RopeGunController settings (zipline gun range indicator)
                // Original patch: __instance._maxRopeLength and __instance._maxFiringRange
                if (LocalPlayer.FpCharacter != null)
                {
                    var ropeGun = LocalPlayer.FpCharacter.GetComponentInChildren<RopeGunController>();
                    if (ropeGun != null)
                    {
                        var maxRopeLengthField = AccessTools.Field(typeof(RopeGunController), "_maxRopeLength");
                        var maxFiringRangeField = AccessTools.Field(typeof(RopeGunController), "_maxFiringRange");
                        
                        if (maxRopeLengthField != null)
                        {
                            maxRopeLengthField.SetValue(ropeGun, Config.MaxZipLineLength.Value);
                        }
                        
                        if (maxFiringRangeField != null)
                        {
                            maxFiringRangeField.SetValue(ropeGun, Config.MaxShootingDistance.Value);
                        }
                        
                        _applied = true;
                        RLog.Msg($"[Zipline] Set max length={Config.MaxZipLineLength.Value}, range={Config.MaxShootingDistance.Value}");
                    }
                }
            }
            catch (System.Exception ex)
            {
                RLog.Warning($"[Zipline] Error applying settings: {ex.Message}");
            }
        }
        
        public static void ForceReapply()
        {
            _applied = false;
            ApplySettings();
        }
    }
}

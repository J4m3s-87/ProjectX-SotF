#if !SERVER
using System;
using HarmonyLib;
using RedLoader;
using TheForest;
using TheForest.Utils;

namespace ProjectX.Master.Modules.Building
{
    /// <summary>
    /// Building enhancement features: FreeForm placement, InstantBuild, No Cuttings
    /// Uses game's built-in console commands for safe execution
    /// </summary>
    public static class BuilderEnhancements
    {
        private static bool _freeFormEnabled = false;
        private static bool _instantBuildEnabled = false;
        private static bool _noCuttingsEnabled = false;
        private static bool _logHackEnabled = false;
        private static bool _stoneHackEnabled = false;

        /// <summary>
        /// Enable/disable free-form building placement
        /// Uses game's freeformplace command
        /// </summary>
        public static bool FreeFormPlacement
        {
            get => _freeFormEnabled;
            set
            {
                _freeFormEnabled = value;
                ApplyFreeForm();
            }
        }

        /// <summary>
        /// Enable/disable instant structure building
        /// Uses game's instantbookbuild command
        /// </summary>
        public static bool InstantBuild
        {
            get => _instantBuildEnabled;
            set
            {
                _instantBuildEnabled = value;
                ApplyInstantBuild();
            }
        }

        /// <summary>
        /// Enable/disable wood cuttings when chopping trees
        /// Uses game's skipwoodcuttings command
        /// </summary>
        public static bool NoCuttingsSpawn
        {
            get => _noCuttingsEnabled;
            set
            {
                _noCuttingsEnabled = value;
                ApplyNoCuttings();
            }
        }

        /// <summary>
        /// Enable/disable infinite logs when holding logs
        /// Uses game's _loghack command
        /// </summary>
        public static bool LogHack
        {
            get => _logHackEnabled;
            set
            {
                _logHackEnabled = value;
                ApplyLogHack();
            }
        }

        /// <summary>
        /// Enable/disable infinite stones when holding stones
        /// Uses game's stonehack command
        /// </summary>
        public static bool StoneHack
        {
            get => _stoneHackEnabled;
            set
            {
                _stoneHackEnabled = value;
                ApplyStoneHack();
            }
        }

        private static void ApplyFreeForm()
        {
            try
            {
                Sons.Gameplay.GameSetup.GameSetupManager.SetFreeFormForcePlaceFullLoadSetting(_freeFormEnabled);
                RLog.Msg($"[BuilderEnhancements] FreeForm placement: {(_freeFormEnabled ? "ON" : "OFF")}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderEnhancements] FreeForm failed: {ex.Message}");
            }
        }

        private static void ApplyInstantBuild()
        {
            try
            {
                // Use game's built-in console command instead of reflection
                string cmd = _instantBuildEnabled ? "instantbookbuild on" : "instantbookbuild off";
                DebugConsole.Instance.SendCommand(cmd);
                RLog.Msg($"[BuilderEnhancements] InstantBuild: {(_instantBuildEnabled ? "ON" : "OFF")}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderEnhancements] InstantBuild failed: {ex.Message}");
            }
        }

        private static void ApplyNoCuttings()
        {
            try
            {
                Sons.Gameplay.GameSetup.GameSetupManager.SetConstructionSkipCuttingsSpawnSetting(_noCuttingsEnabled);
                RLog.Msg($"[BuilderEnhancements] NoCuttings: {(_noCuttingsEnabled ? "ON" : "OFF")}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderEnhancements] NoCuttings failed: {ex.Message}");
            }
        }

        private static void ApplyLogHack()
        {
            try
            {
                // Use game's _loghack method
                DebugConsole.Instance._loghack(_logHackEnabled ? "on" : "off");
                RLog.Msg($"[BuilderEnhancements] LogHack: {(_logHackEnabled ? "ON" : "OFF")}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderEnhancements] LogHack failed: {ex.Message}");
            }
        }

        private static void ApplyStoneHack()
        {
            try
            {
                string cmd = _stoneHackEnabled ? "stonehack on" : "stonehack off";
                DebugConsole.Instance.SendCommand(cmd);
                RLog.Msg($"[BuilderEnhancements] StoneHack: {(_stoneHackEnabled ? "ON" : "OFF")}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderEnhancements] StoneHack failed: {ex.Message}");
            }
        }
    }
}
#endif

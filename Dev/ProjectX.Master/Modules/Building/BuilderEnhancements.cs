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

        // ======================== AI GHOST PLAYER ========================

        private static bool _aiGhostEnabled = false;

        /// <summary>
        /// Enable/disable AI ghost mode (enemies can't see you)
        /// Uses game's aighostplayer command
        /// </summary>
        public static bool AIGhostPlayer
        {
            get => _aiGhostEnabled;
            set
            {
                _aiGhostEnabled = value;
                ApplyAIGhost();
            }
        }

        private static void ApplyAIGhost()
        {
            try
            {
                string cmd = _aiGhostEnabled ? "aighostplayer on" : "aighostplayer off";
                DebugConsole.Instance.SendCommand(cmd);
                RLog.Msg($"[BuilderEnhancements] AIGhostPlayer: {(_aiGhostEnabled ? "ON" : "OFF")}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderEnhancements] AIGhostPlayer failed: {ex.Message}");
            }
        }

        // ======================== BLUEPRINT ACTIONS ========================

        /// <summary>
        /// Cancel all placed blueprints (one-shot action)
        /// </summary>
        public static void CancelBlueprints()
        {
            try
            {
                DebugConsole.Instance.SendCommand("cancelblueprints");
                RLog.Msg("[BuilderEnhancements] Cancel Blueprints executed");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderEnhancements] CancelBlueprints failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Finish/complete all placed blueprints (one-shot action)
        /// </summary>
        public static void FinishBlueprints()
        {
            try
            {
                DebugConsole.Instance.SendCommand("finishblueprints");
                RLog.Msg("[BuilderEnhancements] Finish Blueprints executed");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderEnhancements] FinishBlueprints failed: {ex.Message}");
            }
        }

        // ======================== HARMONY PATCHES ========================
        // In multiplayer, PlaceStructureNode is NEVER called client-side.
        // The actual MP path is NetworkPlaceStructureNode(PrefabId, Vector3, 
        // Quaternion, WorldLocatorId, bool placeBuilt, BoltEntity).
        // We override placeBuilt=true when Config.InstantBookBuild is enabled.
        
        /// <summary>
        /// Initialize Harmony patches for building enhancements.
        /// Patches NetworkPlaceStructureNode (MP) and PlaceStructureNode (SP/P2P).
        /// </summary>
        public static void InitPatches()
        {
            try
            {
                var scsType = HarmonyLib.AccessTools.TypeByName("Sons.Crafting.Structures.StructureCraftingSystem");
                if (scsType == null)
                {
                    RLog.Warning("[BuilderEnhancements] StructureCraftingSystem type not found — skipping patch");
                    return;
                }
                
                var harmony = new HarmonyLib.Harmony("ProjectX.BuilderEnhancements.InstantBuild");
                
                // PRIMARY: Patch NetworkPlaceStructureNode — the actual MP building path
                var networkPlaceMethod = HarmonyLib.AccessTools.Method(scsType, "NetworkPlaceStructureNode");
                if (networkPlaceMethod != null)
                {
                    var netPrefix = typeof(BuilderEnhancements).GetMethod(nameof(NetworkPlaceStructureNode_Prefix),
                        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                    harmony.Patch(networkPlaceMethod, prefix: new HarmonyLib.HarmonyMethod(netPrefix));
                    RLog.Msg("[BuilderEnhancements] Harmony PREFIX on NetworkPlaceStructureNode — OK (MP instant build)");
                }
                else
                {
                    RLog.Warning("[BuilderEnhancements] NetworkPlaceStructureNode not found");
                }
                
                // SECONDARY: Also patch PlaceStructureNode for singleplayer/P2P
                var placeMethod = scsType.GetMethod("PlaceStructureNode",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                if (placeMethod != null)
                {
                    var placePrefix = typeof(BuilderEnhancements).GetMethod(nameof(PlaceStructureNode_Prefix),
                        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                    harmony.Patch(placeMethod, prefix: new HarmonyLib.HarmonyMethod(placePrefix));
                    RLog.Msg("[BuilderEnhancements] Harmony PREFIX on PlaceStructureNode — OK (SP/P2P)");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderEnhancements] InitPatches failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// PRIMARY: Overrides placeBuilt on NetworkPlaceStructureNode (the MP building path).
        /// NetworkPlaceStructureNode(PrefabId, Vector3, Quaternion, WorldLocatorId, bool placeBuilt, BoltEntity)
        /// </summary>
        private static void NetworkPlaceStructureNode_Prefix(ref bool placeBuilt)
        {
            if (Config.InstantBookBuild?.Value == true)
                placeBuilt = true;
        }
        
        /// <summary>
        /// SECONDARY: Overrides instantBuild on PlaceStructureNode (the SP/P2P path).
        /// </summary>
        private static void PlaceStructureNode_Prefix(ref bool instantBuild)
        {
            if (Config.InstantBookBuild?.Value == true)
                instantBuild = true;
        }
    }
}
#endif


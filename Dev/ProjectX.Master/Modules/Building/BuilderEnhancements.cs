#if !SERVER
using System;
using HarmonyLib;
using RedLoader;
using TheForest;
using TheForest.Utils;
using UnityEngine;
using Construction;
using Sons.Crafting.Structures;
using System.Runtime.InteropServices;

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

        // ======================== SLAP CHOP ========================

        private static bool _slapChopEnabled = false;

        /// <summary>
        /// Enable/disable fast wood chopping animation
        /// Uses game's _slapchop command
        /// </summary>
        public static bool SlapChop
        {
            get => _slapChopEnabled;
            set
            {
                _slapChopEnabled = value;
                ApplySlapChop();
            }
        }

        private static void ApplySlapChop()
        {
            try
            {
                string cmd = _slapChopEnabled ? "slapchop on" : "slapchop off";
                DebugConsole.Instance.SendCommand(cmd);
                RLog.Msg($"[BuilderEnhancements] SlapChop: {(_slapChopEnabled ? "ON" : "OFF")}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderEnhancements] SlapChop failed: {ex.Message}");
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

        /// <summary>
        /// Repair all structures on the map by resetting their health.
        /// Uses the cached StructureDestructionManager instance (populated by Harmony PREFIX)
        /// to access _structureHealth dictionary and reset each damaged structure.
        /// </summary>
        public static void RepairAllStructures()
        {
            try
            {
                // Get cached SDM instance from Harmony POSTFIX
                var sdm = Modules.StructureDurability.StructureDurabilityModule.CachedSDM;
                if (sdm == null)
                {
                    // Fallback: find the SDM MonoBehaviour in the scene
                    RLog.Msg("[RepairAll] CachedInstance is null, trying AccessTools...");
                    try
                    {
                        // Try to get the singleton _instance field from the base type
                        var sdmType = typeof(StructureDestructionManager);
                        // Try _instance or Instance on the type hierarchy
                        var field = AccessTools.Field(sdmType, "_instance") 
                                    ?? AccessTools.Field(sdmType, "instance")
                                    ?? AccessTools.Field(sdmType, "_Instance");
                        if (field != null)
                        {
                            sdm = field.GetValue(null) as StructureDestructionManager;
                            RLog.Msg($"[RepairAll] Found via field: {field.Name}");
                        }
                        
                        // If field didn't work, try all static fields on base types
                        if (sdm == null)
                        {
                            var baseType = sdmType.BaseType;
                            while (baseType != null && sdm == null)
                            {
                                foreach (var f in baseType.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
                                {
                                    if (f.FieldType == sdmType || f.FieldType.IsAssignableFrom(sdmType))
                                    {
                                        var val = f.GetValue(null);
                                        if (val != null)
                                        {
                                            sdm = val as StructureDestructionManager;
                                            RLog.Msg($"[RepairAll] Found via base field: {baseType.Name}.{f.Name}");
                                            break;
                                        }
                                    }
                                }
                                baseType = baseType.BaseType;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        RLog.Warning($"[RepairAll] AccessTools fallback failed: {ex.Message}");
                    }
                }
                
                if (sdm == null)
                {
                    SonsSdk.SonsTools.ShowMessage("Repair: no StructureDestructionManager found");
                    RLog.Warning("[RepairAll] SDM instance is null after all attempts");
                    return;
                }
                
                RLog.Msg("[RepairAll] Using cached SDM instance from Harmony PREFIX");
                
                // Access _structureHealth dictionary directly (like the original mod does)
                int repaired = 0;
                int total = 0;
                int failedRepairs = 0;
                try
                {
                    var dict = sdm._structureHealth;
                    total = dict.Count;
                    RLog.Msg($"[RepairAll] Found {total} tracked structures in _structureHealth");
                    
                    if (total == 0)
                    {
                        RLog.Msg("[RepairAll] No structures in dict, skipping to screw structures");
                    }
                    else
                    {
                        // Copy keys to avoid modifying during iteration
                        var keyList = new System.Collections.Generic.List<Structure>();
                        var enumerator = dict.Keys.GetEnumerator();
                        while (enumerator.MoveNext())
                            keyList.Add(enumerator.Current);
                        
                        foreach (var structure in keyList)
                        {
                            try
                            {
                                // First try TryRepair (game's own repair)
                                bool completedRepair = false;
                                if (StructureDestructionManager.TryRepair(structure, out completedRepair) && completedRepair)
                                {
                                    repaired++;
                                    continue;
                                }
                                
                                // Fallback: directly set Health = MaxHealth on the StructureInfo
                                // NOTE: We only set Health, NOT ElementCount. Setting ElementCount
                                // triggers BeamStructure.CalcSupportedBeamElementPosition internally
                                // which crashes on beams with null prefab references (game engine bug).
                                StructureInfo info = default;
                                if (dict.TryGetValue(structure, out info) && info != null)
                                {
                                    float maxHp = info.MaxHealth;
                                    if (maxHp > 0f)
                                    {
                                        info.Health = maxHp;
                                        dict[structure] = info;
                                        repaired++;
                                        RLog.Msg($"[RepairAll] Direct repair: set Health={maxHp}, Elements={info.MaxElements}");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                failedRepairs++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[RepairAll] Dict iteration failed: {ex.Message}");
                }
                
                // Phase 2: Repair ScrewStructureDestruction items (shelves, chairs, spiked walls)
                int screwRepaired = 0;
                try
                {
                    screwRepaired = Modules.StructureDurability.StructureDurabilityModule.RepairAllScrewStructures();
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[RepairAll] Screw repair failed: {ex.Message}");
                }
                
                int totalRepaired = repaired + screwRepaired;
                SonsSdk.SonsTools.ShowMessage($"Repaired {repaired} structures + {screwRepaired} prefabs");
                RLog.Msg($"[RepairAll] Done: {repaired} structures, {screwRepaired} screw prefabs{(failedRepairs > 0 ? $", {failedRepairs} beam(s) skipped" : "")}");
            }
            catch (Exception ex)
            {
                SonsSdk.SonsTools.ShowMessage($"Repair error: {ex.Message}");
                RLog.Warning($"[RepairAll] Failed: {ex}");
            }
        }

        /// <summary>
        /// Add all book pages — unlocks all blueprint book building recipes.
        /// Uses a Harmony PREFIX on BlueprintBookController.CheckPageIsDiscovered
        /// to force it to return true for all recipes.
        /// </summary>
        private static bool _allBookPagesUnlocked = false;

        public static void AddAllBookPages()
        {
            try
            {
                if (_allBookPagesUnlocked)
                {
                    SonsSdk.SonsTools.ShowMessage("Book pages already unlocked");
                    return;
                }

                var controllerType = AccessTools.TypeByName("Sons.Weapon.BlueprintBookController");
                if (controllerType == null)
                {
                    RLog.Warning("[AddAllBookPages] BlueprintBookController type not found");
                    SonsSdk.SonsTools.ShowMessage("Blueprint controller type not found");
                    return;
                }

                var harmony = new HarmonyLib.Harmony("ProjectX.AddAllBookPages");

                // Patch 1: CheckPageIsDiscovered — force all recipes to show as discovered
                var checkMethod = AccessTools.Method(controllerType, "CheckPageIsDiscovered");
                if (checkMethod != null)
                {
                    var prefix = typeof(BuilderEnhancements).GetMethod(
                        nameof(CheckPageIsDiscovered_Prefix),
                        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                    harmony.Patch(checkMethod, prefix: new HarmonyLib.HarmonyMethod(prefix));
                    RLog.Msg("[AddAllBookPages] Patched CheckPageIsDiscovered");
                }
                else
                {
                    RLog.Warning("[AddAllBookPages] CheckPageIsDiscovered method not found");
                }

                // Patch 2: StartBlueprintSelectionMode — clear _blockedPages and unlock tabs
                var startMethod = AccessTools.Method(controllerType, "StartBlueprintSelectionMode");
                if (startMethod != null)
                {
                    var prefix2 = typeof(BuilderEnhancements).GetMethod(
                        nameof(StartBlueprintSelectionMode_Prefix),
                        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                    harmony.Patch(startMethod, prefix: new HarmonyLib.HarmonyMethod(prefix2));
                    RLog.Msg("[AddAllBookPages] Patched StartBlueprintSelectionMode");
                }
                else
                {
                    RLog.Warning("[AddAllBookPages] StartBlueprintSelectionMode not found");
                }

                _allBookPagesUnlocked = true;
                RLog.Msg("[AddAllBookPages] All patches applied — pages unlocked");
                SonsSdk.SonsTools.ShowMessage("All book pages unlocked! Open book to see.");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[AddAllBookPages] Failed: {ex.Message}");
                SonsSdk.SonsTools.ShowMessage($"Book pages failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Harmony PREFIX on BlueprintBookController.CheckPageIsDiscovered.
        /// Forces all pages to show as discovered.
        /// </summary>
        private static bool CheckPageIsDiscovered_Prefix(ref bool __result, ref bool newlyDiscovered)
        {
            __result = true;
            newlyDiscovered = false;
            return false; // Skip original method
        }

        /// <summary>
        /// Harmony PREFIX on BlueprintBookController.StartBlueprintSelectionMode.
        /// Clears _blockedPages, sets _isInCreativeMode = true, and unlocks all tabs
        /// so every page and category is visible.
        /// </summary>
        private static void StartBlueprintSelectionMode_Prefix(object __instance)
        {
            try
            {
                var bbc = __instance as Sons.Weapon.BlueprintBookController;
                if (bbc == null)
                {
                    RLog.Warning("[AddAllBookPages] Failed to cast to BlueprintBookController");
                    return;
                }

                // 1. Clear _blockedPages — typed access (AccessTools.Field returns null on IL2CPP types)
                try
                {
                    bbc._blockedPages?.Clear();
                    RLog.Msg("[AddAllBookPages] Cleared _blockedPages");
                }
                catch (Exception ex) { RLog.Warning($"[AddAllBookPages] _blockedPages clear failed: {ex.Message}"); }

                // 2. Set _isInCreativeMode = true — this makes CheckPageIsDiscovered pass creativeMode=true
                try
                {
                    bbc._isInCreativeMode = true;
                    RLog.Msg("[AddAllBookPages] Set _isInCreativeMode = true");
                }
                catch (Exception ex) { RLog.Warning($"[AddAllBookPages] _isInCreativeMode set failed: {ex.Message}"); }

                // 3. Enable all _discoverableTabs GameObjects (e.g., Notes tab)
                try
                {
                    var tabs = bbc._discoverableTabs;
                    if (tabs != null)
                    {
                        int count = tabs.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var tab = tabs[i];
                            if (tab != null && tab.gameObject != null)
                            {
                                tab.gameObject.SetActive(true);
                            }
                        }
                        RLog.Msg($"[AddAllBookPages] Enabled {count} discoverable tabs");
                    }
                }
                catch (Exception ex) { RLog.Warning($"[AddAllBookPages] _discoverableTabs enable failed: {ex.Message}"); }

                // 4. Unlock all tabs via LockTabs(false)
                try
                {
                    var lockTabsMethod = AccessTools.Method(bbc.GetType(), "LockTabs");
                    lockTabsMethod?.Invoke(bbc, new object[] { false });
                    RLog.Msg("[AddAllBookPages] Called LockTabs(false)");
                }
                catch (Exception ex) { RLog.Warning($"[AddAllBookPages] LockTabs failed: {ex.Message}"); }

                // 5. Trigger OnUnlockTabs to finalize
                try
                {
                    var onUnlockMethod = AccessTools.Method(bbc.GetType(), "OnUnlockTabs");
                    onUnlockMethod?.Invoke(bbc, null);
                    RLog.Msg("[AddAllBookPages] Called OnUnlockTabs()");
                }
                catch (Exception ex) { RLog.Warning($"[AddAllBookPages] OnUnlockTabs failed: {ex.Message}"); }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[AddAllBookPages] PREFIX failed: {ex.Message}");
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


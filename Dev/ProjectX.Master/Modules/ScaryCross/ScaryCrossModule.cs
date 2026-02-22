using System;
using System.Collections.Generic;
using RedLoader;
using SonsSdk;
using UnityEngine;
using Sons.Crafting.Structures;
using Sons.Items.Core;
using Sons.Electricity;
using HarmonyLib;
using Sons.Ai.Vail;

namespace ProjectX.Master.Modules.ScaryCross
{
    /// <summary>
    /// ScaryCross Module — injects MakeCrossScary into powered cross structures.
    /// 
    /// Triple-pronged injection strategy:
    ///   1. Harmony Postfix on ScrewStructureDestruction.Awake() — catches newly
    ///      built crosses (confirmed working, no vtable corruption).
    ///   2. Harmony Postfix on ElectricLight.Awake() — catches save-loaded
    ///      crosses (every cross has an ElectricLight, fires during Bolt load).
    ///   3. Delayed GameObject.Find scan on OnInWorldUpdate — fallback.
    /// 
    /// IL2CPP lifecycle note: [RegisterTypeInIl2Cpp] types do NOT receive
    /// Start()/Update() callbacks. Initialize() must be called explicitly.
    /// </summary>
    public static class ScaryCrossModule
    {
        public static GameObject _bonFireElementPrefab;
        public static GameObject _heldCrossPrefab;
        private static bool _prefabPatched = false;
        private static string _crossPrefabName;
        private static HarmonyLib.Harmony _harmony;
        private static bool _retroScanDone = false;
        private static readonly List<MakeCrossScary> _trackedCrosses = new List<MakeCrossScary>();
        private static readonly HashSet<int> _trackedActorIds = new HashSet<int>();

        public static void Init()
        {
            _harmony = new HarmonyLib.Harmony("ProjectX.ScaryCross");
            
            // Prong 1: Harmony Postfix on ScrewStructureDestruction.Awake —
            // catches newly built crosses. Does NOT catch save-loaded crosses.
            try
            {
                var awakeMethod = AccessTools.Method(typeof(ScrewStructureDestruction), "Awake");
                var postfix = AccessTools.Method(typeof(ScaryCrossModule), nameof(OnStructureAwake));
                
                if (awakeMethod != null && postfix != null)
                {
                    _harmony.Patch(awakeMethod, postfix: new HarmonyMethod(postfix));
                    RLog.Msg("[ScaryCross] Harmony patch on ScrewStructureDestruction.Awake — OK");
                }
                else
                {
                    RLog.Warning($"[ScaryCross] Patch failed: Awake={awakeMethod != null}, Postfix={postfix != null}");
                }
            }
            catch (Exception ex)
            {
                RLog.Error($"[ScaryCross] ScrewStructureDestruction patch error: {ex}");
            }
            
            // Prong 2: Harmony Postfix on ElectricLight.Awake —
            // catches save-loaded crosses (Bolt-networked structures bypass
            // ScrewStructureDestruction.Awake but ElectricLight still fires).
            try
            {
                var elAwake = AccessTools.Method(typeof(ElectricLight), "Awake");
                var elPostfix = AccessTools.Method(typeof(ScaryCrossModule), nameof(OnElectricLightAwake));
                
                if (elAwake != null && elPostfix != null)
                {
                    _harmony.Patch(elAwake, postfix: new HarmonyMethod(elPostfix));
                    RLog.Msg("[ScaryCross] Harmony patch on ElectricLight.Awake — OK");
                }
                else
                {
                    RLog.Warning($"[ScaryCross] ElectricLight patch failed: Awake={elAwake != null}, Postfix={elPostfix != null}");
                }
            }
            catch (Exception ex2)
            {
                RLog.Error($"[ScaryCross] ElectricLight patch error: {ex2}");
            }
            
            // Prong 3: VailActor tracking — Harmony hooks on OnEnable/OnDisable.
            // Physics.OverlapSphere is FULLY stripped by IL2CPP (both 2-param and 3-param).
            // Instead, we maintain a global list of active VailActors and check distances.
            try
            {
                var actorEnable = AccessTools.Method(typeof(VailActor), "OnEnable");
                var actorDisable = AccessTools.Method(typeof(VailActor), "OnDisable");
                
                if (actorEnable != null)
                {
                    var onEnablePostfix = AccessTools.Method(typeof(ScaryCrossModule), nameof(OnVailActorEnable));
                    _harmony.Patch(actorEnable, postfix: new HarmonyMethod(onEnablePostfix));
                }
                if (actorDisable != null)
                {
                    var onDisablePostfix = AccessTools.Method(typeof(ScaryCrossModule), nameof(OnVailActorDisable));
                    _harmony.Patch(actorDisable, postfix: new HarmonyMethod(onDisablePostfix));
                }
                
                RLog.Msg($"[ScaryCross] VailActor tracking hooks — OnEnable={actorEnable != null}, OnDisable={actorDisable != null}");
            }
            catch (Exception ex3)
            {
                RLog.Error($"[ScaryCross] VailActor tracking patch error: {ex3}");
            }
            
            SdkEvents.OnGameActivated.Subscribe(OnGameActivated);
            SdkEvents.OnInWorldUpdate.Subscribe(OnInWorldUpdate);
            
            RLog.Msg("ScaryCross Module Initialized.");
        }

        /// <summary>
        /// Prong 2: ElectricLight.Awake Postfix — catches save-loaded crosses.
        /// ElectricLight fires for all electric devices; we filter by cross name.
        /// </summary>
        private static int _elAwakeCount = 0;
        
        private static void OnElectricLightAwake(ElectricLight __instance)
        {
            try
            {
                if (__instance == null) return;
                
                var go = __instance.gameObject;
                if (go == null) return;
                
                // Diagnostic: log first 10 ElectricLight names to discover cross naming
                _elAwakeCount++;
                if (_elAwakeCount <= 10)
                {
                    string parentName = go.transform.parent != null ? go.transform.parent.name : "(no parent)";
                    RLog.Msg($"[ScaryCross] ElectricLight.Awake #{_elAwakeCount}: '{go.name}' parent='{parentName}'");
                }
                else if (_elAwakeCount == 11)
                {
                    RLog.Msg($"[ScaryCross] (suppressing further ElectricLight logs, {_elAwakeCount}+ seen)");
                }
                
                string nameToCheck = _crossPrefabName ?? "PoweredCrossStructure";
                
                if (!go.name.Contains(nameToCheck)) return;
                
                InjectAndInitialize(go, "ElectricLight.Awake hook");
            }
            catch { }
        }

        /// <summary>
        /// Prong 1: Harmony Postfix — catches newly built crosses.
        /// </summary>
        private static void OnStructureAwake(ScrewStructureDestruction __instance)
        {
            try
            {
                if (__instance == null) return;
                
                var go = __instance.gameObject;
                if (go == null) return;
                
                string nameToCheck = _crossPrefabName ?? "PoweredCrossStructure";
                
                if (!go.name.Contains(nameToCheck)) return;
                
                InjectAndInitialize(go, "Awake hook");
            }
            catch { }
        }

        /// <summary>
        /// Prong 2: Delayed scan for save-loaded crosses.
        /// Uses GameObject.Find with known cross names — simple, IL2CPP-safe.
        /// </summary>
        private static void OnInWorldUpdate()
        {
            // Tick all tracked crosses (IL2CPP doesn't fire Update)
            TickTrackedCrosses();
            
            if (_retroScanDone) return;
            _retroScanDone = true;
            
            RunRetroactiveScan();
        }
        
        /// <summary>
        /// Server-side tick — called from MasterPlugin.ServerTickPostfix.
        /// OnInWorldUpdate does NOT fire on headless servers.
        /// Also handles the one-time retroactive scan on the server (since
        /// OnInWorldUpdate's retroactive scan is inside #if !SERVER).
        /// </summary>
        public static void ServerTick()
        {
            // One-time retroactive scan — discover save-loaded crosses that
            // Awake hooks missed (they fire before Init patches them)
            if (!_retroScanDone)
            {
                _retroScanDone = true;
                RunRetroactiveScan();
            }
            
            TickTrackedCrosses();
        }
        
        /// <summary>
        /// Shared cross-ticking logic — used by both OnInWorldUpdate (client) and ServerTick (server).
        /// </summary>
        private static void TickTrackedCrosses()
        {
            for (int i = _trackedCrosses.Count - 1; i >= 0; i--)
            {
                try
                {
                    if (_trackedCrosses[i] == null || _trackedCrosses[i].gameObject == null)
                    {
                        RLog.Msg($"[ScaryCross] Removing null cross at index {i}");
                        _trackedCrosses.RemoveAt(i);
                        continue;
                    }
                    _trackedCrosses[i].ManualUpdate();
                }
                catch (Exception tickEx)
                {
                    RLog.Warning($"[ScaryCross] ManualUpdate CRASHED on cross {i}: {tickEx.Message}");
                    RLog.Warning($"[ScaryCross]   Stack: {tickEx.StackTrace}");
                    _trackedCrosses.RemoveAt(i);
                }
            }
        }
        
        /// <summary>
        /// Scans the scene for save-loaded crosses that the Awake hooks missed
        /// (Awake fires during deserialization BEFORE Init patches them).
        /// Called from OnInWorldUpdate (client) and ServerTick (server).
        /// </summary>
        private static void RunRetroactiveScan()
        {
            string nameToCheck = _crossPrefabName ?? "PoweredCrossStructure";
            
            try
            {
                int count = 0;
                
                // Try finding crosses by known clone names.
                // Save-loaded crosses are named "PoweredCrossStructureNode(Clone)"
                // or "PoweredCrossStructure(Clone)".
                string[] searchNames = new[]
                {
                    nameToCheck + "Node",           // e.g. "PoweredCrossStructureNode"
                    nameToCheck + "Node(Clone)",     // e.g. "PoweredCrossStructureNode(Clone)"
                    nameToCheck + "(Clone)",          // e.g. "PoweredCrossStructure(Clone)"
                    nameToCheck                      // exact prefab name
                };
                
                foreach (var searchName in searchNames)
                {
                    // GameObject.Find finds the first active object with that name.
                    // Keep searching until no more are found.
                    var found = GameObject.Find(searchName);
                    while (found != null)
                    {
                        if (InjectAndInitialize(found, "retroactive scan"))
                        {
                            count++;
                        }
                        
                        // Search for the next one — but we need to scan siblings.
                        // After injecting, search the parent's children for more.
                        var parent = found.transform.parent;
                        if (parent != null)
                        {
                            for (int i = 0; i < parent.childCount; i++)
                            {
                                try
                                {
                                    var child = parent.GetChild(i);
                                    if (child == null) continue;
                                    var childGO = child.gameObject;
                                    if (childGO == null || childGO == found) continue;
                                    
                                    if (childGO.name.Contains(nameToCheck))
                                    {
                                        if (InjectAndInitialize(childGO, "retroactive scan (sibling)"))
                                            count++;
                                    }
                                }
                                catch { }
                            }
                        }
                        
                        // Try to find another one (will return null if no more exist,
                        // or might return the same one if the name didn't change)
                        var next = GameObject.Find(searchName);
                        if (next == found || next == null) break;
                        found = next;
                    }
                }
                
                RLog.Msg($"[ScaryCross] Retroactive scan complete — injected MakeCrossScary into {count} existing cross(es).");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ScaryCross] Retroactive scan failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Shared injection + initialization logic. Returns true if injected or initialized.
        /// Handles both new injection AND pre-existing components from prefab injection
        /// that need Initialize() called (IL2CPP doesn't fire lifecycle callbacks).
        /// </summary>
        private static bool InjectAndInitialize(GameObject go, string source)
        {
            var scary = go.GetComponent<MakeCrossScary>();
            
            if (scary == null)
            {
                // No component — inject and initialize
                scary = go.AddComponent<MakeCrossScary>();
                RLog.Msg($"[ScaryCross] Injected MakeCrossScary into '{go.name}' via {source}.");
            }
            else if (scary._initialized)
            {
                // Already has component AND already initialized — skip
                return false;
            }
            else
            {
                RLog.Msg($"[ScaryCross] Found existing un-initialized MakeCrossScary on '{go.name}' via {source}.");
            }
            
            // IL2CPP doesn't fire lifecycle callbacks on [RegisterTypeInIl2Cpp] types.
            try
            {
                scary.Initialize();
                _trackedCrosses.Add(scary);
                RLog.Msg($"[ScaryCross] Initialize() OK on '{go.name}'. Tracked crosses: {_trackedCrosses.Count}");
            }
            catch (Exception initEx)
            {
                RLog.Warning($"[ScaryCross] Initialize() failed on '{go.name}': {initEx.Message}");
            }
            
            return true;
        }

        public static void OnGameActivated()
        {
            // Step 1: Resolve cross prefab name and inject component into prefab template
            try 
            {
                var recipe = ConstructionTools.GetRecipe(71);
                if (recipe != null)
                {
                    var builtPrefab = recipe._builtPrefab;
                    if (builtPrefab != null)
                    {
                        _crossPrefabName = builtPrefab.name;
                        
                        if (!_prefabPatched)
                        {
                            builtPrefab.AddComponent<MakeCrossScary>();
                            _prefabPatched = true;
                            RLog.Msg($"[ScaryCross] Injected MakeCrossScary into Cross Prefab (name='{_crossPrefabName}').");
                        }
                    }
                    else
                    {
                        RLog.Warning("[ScaryCross] Recipe(71)._builtPrefab is null.");
                    }
                }
                else
                {
                    RLog.Warning("[ScaryCross] Failed to find Cross Recipe (71).");
                }
            }
            catch (Exception ex)
            {
                RLog.Error($"[ScaryCross] Prefab injection failed: {ex.Message}");
            }

            // Step 2: Get fire prefab
            try
            {
                var fireProfile = ConstructionTools.GetProfile(403);
                if (fireProfile != null && fireProfile.Prefab != null)
                {
                    _bonFireElementPrefab = fireProfile.Prefab.gameObject;
                    RLog.Msg("[ScaryCross] Fire prefab (403) loaded.");
                }
                else
                {
                    RLog.Warning("[ScaryCross] Standing Fire profile (403) not found or has null prefab.");
                }
            }
            catch (Exception fireEx)
            {
                RLog.Warning($"[ScaryCross] Fire profile access failed: {fireEx.Message}");
            }

            // Step 3: Get held cross prefab
            try
            {
                var heldCross = ItemTools.GetHeldPrefab(468);
                if (heldCross != null)
                {
                    _heldCrossPrefab = heldCross.gameObject;
                    RLog.Msg("[ScaryCross] Held cross prefab (468) loaded.");
                }
                else
                {
                    RLog.Warning("[ScaryCross] Held Cross prefab (468) not found.");
                }
            }
            catch (Exception heldEx)
            {
                RLog.Warning($"[ScaryCross] Held prefab access failed: {heldEx.Message}");
            }
        }
        
        // ─── VailActor Tracking (Pattern #11: Harmony OnEnable Tracking) ───
        // Physics.OverlapSphere is FULLY stripped. Track actors via lifecycle hooks.
        
        private static void OnVailActorEnable(VailActor __instance)
        {
            try
            {
                if (__instance == null) return;
                int id = __instance.GetInstanceID();
                if (_trackedActorIds.Add(id))
                    DemonDetector.TrackedActors.Add(__instance);
            }
            catch { }
        }
        
        private static void OnVailActorDisable(VailActor __instance)
        {
            try
            {
                if (__instance == null) return;
                _trackedActorIds.Remove(__instance.GetInstanceID());
                DemonDetector.TrackedActors.Remove(__instance);
            }
            catch { }
        }
    }
}

#if !SERVER
using System;
using System.Collections.Generic;

using HarmonyLib;
using RedLoader;
using Sons.Crafting.Structures;
using Sons.Inventory;
using TheForest.Utils;
using UnityEngine;

namespace ProjectX.Master.Modules.PrefabRepair
{
    /// <summary>
    /// PrefabRepair Module — Harmony-only reimplementation (no MonoBehaviour injection).
    /// 
    /// Allows players to repair damaged screw structures (shelves, traps, spiked walls,
    /// chairs, etc.) by hitting them with the Repair Tool (item 422).
    /// 
    /// Original standalone PrefabRepair.dll used [RegisterTypeInIl2Cpp] MonoBehaviour
    /// which causes IL2CPP vtable corruption (Tab/backpack crash) when compiled in the
    /// same assembly as Project X. This reimplementation stores all state in static
    /// fields/dictionaries — no MonoBehaviour needed.
    /// 
    /// Hooks:
    ///   1. PREFIX on ScrewStructureDestruction.IImpactReceiver_OnImpact — repair logic
    ///   2. POSTFIX on ScrewStructureDestruction.Awake — detect gold plating at spawn
    ///   3. POSTFIX on ScrewStructure.ApplyGoldPlating — double HP when plating applied
    ///   4. POSTFIX on ElectricDeviceScrewStructure.ApplyGoldPlating — same for electric
    /// </summary>
    public static class PrefabRepairModule
    {
        /// <summary>Repair tool item ID.</summary>
        private const int RepairToolId = 422;

        /// <summary>Default structure HP.</summary>
        private const int DefaultHp = 6;

        /// <summary>Gold-plated structure HP (doubled).</summary>
        private const int GoldPlatedHp = 12;

        /// <summary>Minimum time between impact events (debounce).</summary>
        private const double DebounceSeconds = 0.05;

        /// <summary>Debounce: last accepted impact time.</summary>
        private static float _lastImpact;

        /// <summary>
        /// Tracks which ScrewStructureDestruction instances are gold plated.
        /// Keyed by IL2CPP pointer (IntPtr) for identity — GameObject refs can go stale.
        /// </summary>
        private static readonly HashSet<IntPtr> _platedStructures = new HashSet<IntPtr>();

        /// <summary>
        /// Cached MethodInfo for FMODCommon.PlayOneshot. IL2CPP signature may differ
        /// from decompiled source — use reflection per EventAnnouncer proven pattern.
        /// </summary>
        private static System.Reflection.MethodInfo _cachedPlayOneshot;
        private static bool _playOneshotCached;

        public static void Init()
        {
            try
            {
                var harmony = new HarmonyLib.Harmony("ProjectX.PrefabRepair");

                // Hook 1: PREFIX on ScrewStructureDestruction.IImpactReceiver_OnImpact
                var impactMethod = AccessTools.Method(typeof(ScrewStructureDestruction), "IImpactReceiver_OnImpact");
                if (impactMethod != null)
                {
                    var prefix = typeof(PrefabRepairModule).GetMethod(
                        nameof(PrefixOnImpact),
                        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                    harmony.Patch(impactMethod, prefix: new HarmonyMethod(prefix));
                    RLog.Msg("[PrefabRepair] Harmony PREFIX on IImpactReceiver_OnImpact — PATCHED OK");
                }
                else
                {
                    RLog.Warning("[PrefabRepair] IImpactReceiver_OnImpact method NOT found");
                }

                // Hook 2: POSTFIX on ScrewStructureDestruction.Awake — detect gold plating at spawn
                var awakeMethod = AccessTools.Method(typeof(ScrewStructureDestruction), "Awake");
                if (awakeMethod != null)
                {
                    var postfix = typeof(PrefabRepairModule).GetMethod(
                        nameof(PostfixAwake),
                        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                    harmony.Patch(awakeMethod, postfix: new HarmonyMethod(postfix));
                    RLog.Msg("[PrefabRepair] Harmony POSTFIX on ScrewStructureDestruction.Awake — PATCHED OK");
                }
                else
                {
                    RLog.Warning("[PrefabRepair] ScrewStructureDestruction.Awake not found");
                }

                // Hook 3: POSTFIX on ApplyGoldPlating — double HP for gold-plated structures
                // ScrewStructureBase<TState> is an open generic — IL2CPP can't patch it
                // (Type.ContainsGenericParameters error). Patch the two CONCRETE subclasses instead.
                var postfixGold = typeof(PrefabRepairModule).GetMethod(
                    nameof(PostfixGoldPlating),
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

                // Patch ScrewStructure.ApplyGoldPlating
                try
                {
                    var goldMethod1 = AccessTools.Method(typeof(ScrewStructure), "ApplyGoldPlating");
                    if (goldMethod1 != null)
                    {
                        harmony.Patch(goldMethod1, postfix: new HarmonyMethod(postfixGold));
                        RLog.Msg("[PrefabRepair] Harmony POSTFIX on ScrewStructure.ApplyGoldPlating — PATCHED OK");
                    }
                    else
                    {
                        RLog.Warning("[PrefabRepair] ScrewStructure.ApplyGoldPlating not found");
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[PrefabRepair] ScrewStructure.ApplyGoldPlating patch failed: {ex.Message}");
                }

                // Patch ElectricDeviceScrewStructure.ApplyGoldPlating
                try
                {
                    var goldMethod2 = AccessTools.Method(typeof(ElectricDeviceScrewStructure), "ApplyGoldPlating");
                    if (goldMethod2 != null)
                    {
                        harmony.Patch(goldMethod2, postfix: new HarmonyMethod(postfixGold));
                        RLog.Msg("[PrefabRepair] Harmony POSTFIX on ElectricDeviceScrewStructure.ApplyGoldPlating — PATCHED OK");
                    }
                    else
                    {
                        RLog.Warning("[PrefabRepair] ElectricDeviceScrewStructure.ApplyGoldPlating not found");
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[PrefabRepair] ElectricDeviceScrewStructure.ApplyGoldPlating patch failed: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                RLog.Error($"[PrefabRepair] Init failed: {ex}");
            }
        }

        // ===================== HARMONY PATCHES =====================

        /// <summary>
        /// PREFIX on ScrewStructureDestruction.IImpactReceiver_OnImpact.
        /// Checks if the local player hit the structure with the repair tool,
        /// then increments HP. Plays a completion sound when fully repaired.
        /// </summary>
        public static void PrefixOnImpact(ScrewStructureDestruction __instance, IImpactSender sender, IImpactData impactData)
        {
            try
            {
                // Only player impacts
                if (!impactData.IsPlayer) return;

                // Debounce: prevent duplicate impacts within 0.05s
                float currentTime = __instance._lastImpactTime;
                if ((double)_lastImpact + DebounceSeconds > (double)currentTime) return;
                _lastImpact = currentTime;

                // Must be the local player (not NPC or remote player)
                if (sender.GetRootTransform().root != LocalPlayer.Transform) return;

                // Must be holding the repair tool (item 422)
                ItemInstance rightHandItem = LocalPlayer.Inventory.RightHandItem;
                if ((rightHandItem != null ? rightHandItem._itemID : 0) != RepairToolId) return;

                // Only repair if damaged
                if (__instance._currentHp >= __instance.StructureHp) return;

                // Increment HP
                __instance._currentHp++;

                // Play completion sound when fully repaired
                if (__instance._currentHp == __instance.StructureHp)
                {
                    PlayRepairCompleteSound(__instance.transform);
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PrefabRepair] OnImpact failed: {ex.Message}");
            }
        }

        /// <summary>
        /// POSTFIX on ScrewStructureDestruction.Awake.
        /// Detects gold-plated structures at spawn and adjusts HP.
        /// </summary>
        public static void PostfixAwake(ScrewStructureDestruction __instance)
        {
            try
            {
                if (__instance == null) return;

                // Check for gold plating via ScrewStructure or ElectricDeviceScrewStructure
                var screwStructure = __instance.transform.gameObject.GetComponent<ScrewStructure>();
                var electricStructure = __instance.transform.gameObject.GetComponent<ElectricDeviceScrewStructure>();

                bool isPlated = false;

                if (screwStructure != null && screwStructure.IsGoldPlated)
                    isPlated = true;
                else if (electricStructure != null && electricStructure.IsGoldPlated)
                    isPlated = true;

                if (isPlated)
                {
                    var il2cppObj = (Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)(object)__instance;
                    IntPtr ptr = il2cppObj.Pointer;
                    _platedStructures.Add(ptr);

                    __instance._structureHp = GoldPlatedHp;

                    // If at default HP, set to gold-plated HP
                    if (__instance._currentHp == DefaultHp)
                    {
                        __instance._currentHp = GoldPlatedHp;
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PrefabRepair] PostfixAwake failed: {ex.Message}");
            }
        }

        /// <summary>
        /// POSTFIX on ScrewStructureBase.ApplyGoldPlating.
        /// Doubles structure HP when gold plating is applied in-game.
        /// Uses object __instance because ScrewStructureBase generic type arg
        /// is not resolvable in the DummyDll context. We get the Transform from it.
        /// </summary>
        public static void PostfixGoldPlating(object __instance)
        {
            try
            {
                if (__instance == null) return;

                // Get Transform from the instance to find the ScrewStructureDestruction on the same GO
                var monoBehaviour = __instance as UnityEngine.Component;
                if (monoBehaviour == null) return;

                var destruction = monoBehaviour.transform.gameObject.GetComponent<ScrewStructureDestruction>();
                if (destruction == null) return;

                var il2cppObj = (Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)(object)destruction;
                IntPtr ptr = il2cppObj.Pointer;
                _platedStructures.Add(ptr);

                destruction._structureHp = GoldPlatedHp;
                destruction._currentHp += DefaultHp; // Add 6 HP when plating is applied
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PrefabRepair] PostfixGoldPlating failed: {ex.Message}");
            }
        }

        // ===================== SOUND =====================

        /// <summary>
        /// Plays the repair completion sound effect.
        /// Uses reflection to find FMODCommon.PlayOneshot — IL2CPP signature may differ.
        /// Pattern: EventAnnouncer.cs proven approach.
        /// </summary>
        private static void PlayRepairCompleteSound(Transform position)
        {
            try
            {
                if (!_playOneshotCached)
                {
                    _playOneshotCached = true;
                    var fmodType = AccessTools.TypeByName("FMODCommon")
                                ?? AccessTools.TypeByName("Endnight.Utilities.FMODCommon");
                    if (fmodType == null)
                    {
                        RLog.Warning("[PrefabRepair] FMODCommon type not found — sound disabled");
                        return;
                    }
                    var methods = fmodType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                    foreach (var m in methods)
                    {
                        if (m.Name == "PlayOneshot")
                        {
                            var parms = m.GetParameters();
                            // Look for (string, Transform) overload first
                            if (parms.Length == 2 && parms[1].ParameterType == typeof(Transform))
                            {
                                _cachedPlayOneshot = m;
                                break;
                            }
                            // Fall back to any 2+ param overload
                            if (_cachedPlayOneshot == null && parms.Length >= 2)
                                _cachedPlayOneshot = m;
                        }
                    }

                    if (_cachedPlayOneshot != null)
                    {
                        var p = _cachedPlayOneshot.GetParameters();
                        string sig = string.Join(", ", Array.ConvertAll(p, x => $"{x.ParameterType.Name} {x.Name}"));
                        RLog.Msg($"[PrefabRepair] Using FMODCommon.PlayOneshot({sig})");
                    }
                    else
                    {
                        RLog.Warning("[PrefabRepair] No FMODCommon.PlayOneshot method found!");
                    }
                }

                if (_cachedPlayOneshot == null) return;

                var parameters = _cachedPlayOneshot.GetParameters();
                object[] args;

                if (parameters.Length == 2 && parameters[1].ParameterType == typeof(Transform))
                    args = new object[] { "event:/ui/ingame/ui_crafting_complete2", position };
                else if (parameters.Length == 2)
                    args = new object[] { "event:/ui/ingame/ui_crafting_complete2", position.position };
                else if (parameters.Length == 3)
                    args = new object[] { "event:/ui/ingame/ui_crafting_complete2", position.position, null };
                else
                    args = new object[] { "event:/ui/ingame/ui_crafting_complete2", position.position };

                _cachedPlayOneshot.Invoke(null, args);
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PrefabRepair] Sound failed: {ex.Message}");
            }
        }
    }
}
#endif

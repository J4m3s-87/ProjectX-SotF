using SonsSdk;
using UnityEngine;
using RedLoader;
using HarmonyLib;
using Sons.Gameplay;
using Sons.Atmosphere;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ProjectX.Master.Modules.WaterCollectors
{
    /// <summary>
    /// Water Collectors module — prevents rain catchers from freezing in winter
    /// when a fire/heat source is nearby.
    /// 
    /// Architecture:
    ///   - Harmony Postfix on RainCatcher.Attached() catches all catchers at spawn
    ///   - Harmony Postfix on TemperatureModifierVolume.OnEnable() tracks heat sources
    ///   - OnGuiTick() processes logic every 5s (piggybacked from ProjectXGUI)
    ///   - Simple Vector3.Distance check (Physics.OverlapSphere is IL2CPP stripped!)
    ///   - AccessTools.Field for RainCatcher fields, Marshal offset fallback
    /// 
    /// IL2CPP offsets from dump.cs:
    ///   RainCatcher:
    ///     _currentVolume  : 0x6C (float)
    ///     _isFrozen       : 0x70 (bool)
    ///     _currentSeason  : 0x7C (int, SeasonsManager.Season)
    ///     _fillRate       : 0x68 (float)
    ///     _maxVolume      : 0x60 (float)
    /// </summary>
    public static class WaterCollectorsModule
    {
        private static bool _activated = false;
        private static int _attachedCount = 0;
        private static float _lastTickTime = 0f;
        private static HarmonyLib.Harmony _harmony;
        
        // Tracked rain catchers
        private static readonly List<RainCatcher> _catchers = new List<RainCatcher>();
        
        // Tracked heat sources (fires etc) — replaces Physics.OverlapSphere which is stripped
        private static readonly List<TemperatureModifierVolume> _heatSources = new List<TemperatureModifierVolume>();
        private static int _heatSourceCount = 0;
        
        // Field access (try AccessTools first, fall back to Marshal offsets)
        private static FieldInfo _currentSeasonField;
        private static FieldInfo _isFrozenField;
        private static MethodInfo _setFrozenMethod;
        private static bool _fieldsCached = false;
        private static bool _useOffsets = false;
        
        // Marshal offset fallback
        private const int OFFSET_CURRENT_SEASON = 0x7C;
        private const int OFFSET_IS_FROZEN      = 0x70;

        public static void Init()
        {
            _harmony = new HarmonyLib.Harmony("ProjectX.WaterCollectors");
            
            // Patch 1: RainCatcher.Attached() to track catchers
            try
            {
                var attachedMethod = AccessTools.Method(typeof(RainCatcher), "Attached");
                var postfix = AccessTools.Method(typeof(WaterCollectorsModule), nameof(OnRainCatcherAttached));
                
                if (attachedMethod != null && postfix != null)
                {
                    _harmony.Patch(attachedMethod, postfix: new HarmonyMethod(postfix));
                    RLog.Msg("[WaterCollectors] Harmony patch on RainCatcher.Attached — OK");
                }
                else
                {
                    RLog.Warning($"[WaterCollectors] RainCatcher patch failed: Attached={attachedMethod != null}, Postfix={postfix != null}");
                }
            }
            catch (System.Exception ex)
            {
                RLog.Error($"[WaterCollectors] RainCatcher Harmony error: {ex}");
            }
            
            // Patch 2: TemperatureModifierVolume.OnEnable() to track heat sources
            // Physics.OverlapSphere is stripped by IL2CPP — use Harmony tracking instead
            try
            {
                var onEnableMethod = AccessTools.Method(typeof(TemperatureModifierVolume), "OnEnable");
                var heatPostfix = AccessTools.Method(typeof(WaterCollectorsModule), nameof(OnHeatSourceEnabled));
                
                if (onEnableMethod != null && heatPostfix != null)
                {
                    _harmony.Patch(onEnableMethod, postfix: new HarmonyMethod(heatPostfix));
                    RLog.Msg("[WaterCollectors] Harmony patch on TemperatureModifierVolume.OnEnable — OK");
                }
                else
                {
                    RLog.Warning($"[WaterCollectors] HeatSource patch failed: OnEnable={onEnableMethod != null}, Postfix={heatPostfix != null}");
                }
            }
            catch (System.Exception ex)
            {
                RLog.Error($"[WaterCollectors] HeatSource Harmony error: {ex}");
            }
            
            // Cache field access
            CacheFields();
            
            SdkEvents.OnGameActivated.Subscribe(() => { 
                _activated = true; 
                RLog.Msg("[WaterCollectors] Game activated.");
            });
            
            RLog.Msg("[WaterCollectors] Module initialized.");
        }

        private static void CacheFields()
        {
            if (_fieldsCached) return;
            _fieldsCached = true;
            
            try
            {
                _currentSeasonField = AccessTools.Field(typeof(RainCatcher), "_currentSeason");
                _isFrozenField = AccessTools.Field(typeof(RainCatcher), "_isFrozen");
                _setFrozenMethod = AccessTools.Method(typeof(RainCatcher), "SetFrozen");
                
                RLog.Msg($"[WaterCollectors] AccessTools: Season={_currentSeasonField != null}, " +
                         $"Frozen={_isFrozenField != null}, " +
                         $"SetFrozen={_setFrozenMethod != null}");
                
                if (_currentSeasonField == null || _isFrozenField == null)
                {
                    _useOffsets = true;
                    RLog.Msg("[WaterCollectors] AccessTools failed — using Marshal offset access");
                }
            }
            catch (System.Exception ex)
            {
                _useOffsets = true;
                RLog.Warning($"[WaterCollectors] Field cache error: {ex.Message} — using offsets");
            }
        }

        // ===== IL2CPP DIRECT MEMORY ACCESS (fallback) =====
        
        private static System.IntPtr GetPointer(RainCatcher catcher)
        {
            var baseObj = (Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)(object)catcher;
            return baseObj.Pointer;
        }

        private static int ReadInt(System.IntPtr ptr, int offset)
        {
            return Marshal.ReadInt32(ptr + offset);
        }
        
        private static void WriteInt(System.IntPtr ptr, int offset, int value)
        {
            Marshal.WriteInt32(ptr + offset, value);
        }
        
        private static bool ReadBool(System.IntPtr ptr, int offset)
        {
            return Marshal.ReadByte(ptr + offset) != 0;
        }

        /// <summary>
        /// Called from ProjectXGUI.OnGUI()
        /// </summary>
        public static void OnGuiTick()
        {
            if (!_activated || _catchers.Count == 0) return;

            float now = Time.time;
            if (now - _lastTickTime < 5f) return; // Check every 5 seconds
            _lastTickTime = now;

            try
            {
                ProcessAllCatchers();
            }
            catch (System.Exception ex)
            {
                RLog.Error($"[WaterCollectors] Tick error: {ex.Message}");
            }
        }

        /// <summary>
        /// Harmony Postfix — fired after every RainCatcher.Attached()
        /// </summary>
        private static void OnRainCatcherAttached(RainCatcher __instance)
        {
            try
            {
                if (__instance == null) return;
                
                for (int i = 0; i < _catchers.Count; i++)
                    if (_catchers[i] == __instance) return;
                
                _catchers.Add(__instance);
                _attachedCount++;
                
                var go = __instance.gameObject;
                string name = go != null ? go.name : "unknown";
                RLog.Msg($"[WaterCollectors] Tracked catcher #{_attachedCount}: {name}");
            }
            catch (System.Exception ex)
            {
                RLog.Error($"[WaterCollectors] OnRainCatcherAttached error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Harmony Postfix — fired after every TemperatureModifierVolume.OnEnable()
        /// Replaces Physics.OverlapSphere (which is stripped by IL2CPP)
        /// </summary>
        private static void OnHeatSourceEnabled(TemperatureModifierVolume __instance)
        {
            try
            {
                if (__instance == null) return;
                
                // Skip heat sources parented to scary crosses — their fire prefab
                // creates TemperatureModifierVolumes that falsely un-freeze water collectors
                if (IsParentedToScaryCross(__instance.transform)) return;
                
                for (int i = 0; i < _heatSources.Count; i++)
                    if (_heatSources[i] == __instance) return;
                
                _heatSources.Add(__instance);
                _heatSourceCount++;
            }
            catch { }
        }
        
        /// <summary>
        /// Walk up the parent hierarchy (max 5 levels) to check if this transform
        /// belongs to a scary cross structure. Their fire prefab creates
        /// TemperatureModifierVolumes that should not count as campfire heat.
        /// </summary>
        private static bool IsParentedToScaryCross(Transform t)
        {
            Transform current = t;
            for (int i = 0; i < 5 && current != null; i++)
            {
                try
                {
                    if (current.gameObject.name.StartsWith("PoweredCrossStructure"))
                        return true;
                }
                catch { }
                current = current.parent;
            }
            return false;
        }

        // Track which catchers we've already unfrozen to avoid repeated SetFrozen calls
        private static readonly HashSet<int> _unfrozenCatchers = new HashSet<int>();
        private static int _tickCount = 0;
        
        private static void ProcessAllCatchers()
        {
            _tickCount++;
            
            // Clean up destroyed catchers
            for (int i = _catchers.Count - 1; i >= 0; i--)
            {
                try { if (_catchers[i] == null) _catchers.RemoveAt(i); }
                catch { _catchers.RemoveAt(i); }
            }
            
            // Clean up heat sources less frequently (every 60s)
            if (_tickCount % 6 == 0)
            {
                for (int i = _heatSources.Count - 1; i >= 0; i--)
                {
                    try { if (_heatSources[i] == null) _heatSources.RemoveAt(i); }
                    catch { _heatSources.RemoveAt(i); }
                }
            }
            
            if (_catchers.Count == 0) return;
            
            // Get season via public static property
            int activeSeason = -1;
            try { activeSeason = (int)SeasonsManager.ActiveSeason; } catch { }
            

            
            // Only apply winter-fire logic in Winter (season 3)
            if (activeSeason != 3)
            {
                // If not winter, clear unfrozen tracking so it resets when winter returns
                if (_unfrozenCatchers.Count > 0) _unfrozenCatchers.Clear();
                return;
            }
            
            foreach (var catcher in _catchers)
            {
                if (catcher == null) continue;
                try
                {
                    ApplyFireProximity(catcher);
                }
                catch { }
            }
        }

        private static void ApplyFireProximity(RainCatcher catcher)
        {
            var go = catcher.gameObject;
            if (go == null) return;
            
            int id = catcher.GetInstanceID();
            bool fireNearby = IsHeatSourceNearby(go.transform, Config.WaterCollectorHeatRadius.Value);
            bool alreadyUnfrozen = _unfrozenCatchers.Contains(id);
            
            if (fireNearby && !alreadyUnfrozen)
            {
                // Fire nearby and not yet unfrozen — do it once
                if (_useOffsets)
                {
                    var ptr = GetPointer(catcher);
                    WriteInt(ptr, OFFSET_CURRENT_SEASON, 1); // Summer
                }
                else
                {
                    try { _currentSeasonField.SetValue(catcher, 1); } catch { }
                }
                
                if (_setFrozenMethod != null)
                {
                    try { _setFrozenMethod.Invoke(catcher, new object[] { false }); } catch { }
                }
                
                _unfrozenCatchers.Add(id);
            }
            else if (!fireNearby && alreadyUnfrozen)
            {
                // No fire and was unfrozen — freeze it back
                
                if (_useOffsets)
                {
                    var ptr = GetPointer(catcher);
                    WriteInt(ptr, OFFSET_CURRENT_SEASON, 3); // Winter
                }
                else
                {
                    try { _currentSeasonField.SetValue(catcher, 3); } catch { }
                }
                
                if (_setFrozenMethod != null)
                {
                    try { _setFrozenMethod.Invoke(catcher, new object[] { true }); } catch { }
                }
                
                _unfrozenCatchers.Remove(id);
            }
            // else: state unchanged, do nothing (no-op = no lag)
        }

        /// <summary>
        /// Check if any tracked heat source is within radius of the catcher.
        /// Uses simple Vector3.Distance on tracked TemperatureModifierVolume instances.
        /// </summary>
        public static bool IsHeatSourceNearby(Transform origin, float radius)
        {
            Vector3 pos = origin.position;
            
            foreach (var heat in _heatSources)
            {
                if (heat == null) continue;
                try
                {
                    float dist = Vector3.Distance(pos, heat.transform.position);
                    if (dist <= radius)
                        return true;
                }
                catch { }
            }
            return false;
        }
    }
}

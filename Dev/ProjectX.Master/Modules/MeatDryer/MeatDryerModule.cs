using SonsSdk;
using UnityEngine;
using RedLoader;
using HarmonyLib;
using Sons.Crafting.Structures;
using Sons.Gameplay;
using Sons.Atmosphere;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ProjectX.Master.Modules.MeatDryer
{
    /// <summary>
    /// MeatDryer module — attaches to drying racks via Harmony and manages drying speed.
    /// 
    /// IL2CPP field access strategy:
    ///   All reflection (AccessTools.Field, GetFields, TypeByName) FAILS for DryingRackHookPoint.
    ///   Solution: Direct memory offset writes via Marshal + Il2CppObjectBase.Pointer.
    ///   Offsets from Il2CppDumper dump.cs:
    ///     DryingRackHookPoint:
    ///       _requiredCureTimeGameDays : 0x60 (float, SerializeField)
    ///       _remainingCureTimeSeconds : 0x64 (float)
    ///       _dryBoostMultiplier       : 0x74 (float, SerializeField)
    ///       _isInsideTemperatureVolume: 0x90 (bool)
    ///
    ///   Public APIs used where available:
    ///     SeasonsManager.ActiveSeason (static property)
    ///     DryingRackHookPoint.SetTemperatureVolumeSettings(float, bool)
    /// </summary>
    public static class MeatDryerModule
    {
        private static int _attachedCount = 0;
        private static float _lastTickTime = 0f;
        private static bool _tickLogged = false;
        private static HarmonyLib.Harmony _harmony;
        
        // IL2CPP field offsets from dump.cs
        private const int OFFSET_REQUIRED_CURE_TIME   = 0x60; // float _requiredCureTimeGameDays
        private const int OFFSET_REMAINING_CURE_TIME  = 0x64; // float _remainingCureTimeSeconds
        private const int OFFSET_DRY_BOOST_MULTIPLIER = 0x74; // float _dryBoostMultiplier
        
        // Tracked racks
        private static readonly List<RackData> _racks = new List<RackData>();
        private static bool _offsetsVerified = false;

        private class RackData
        {
            public DryingRackScrewStructure Structure;
            public List<DryingRackHookPoint> HookPoints;
        }

        public static void Init()
        {
            _harmony = new HarmonyLib.Harmony("ProjectX.MeatDryer");
            
            // Hook 1: Awake — track racks as they spawn
            try
            {
                var awakeMethod = AccessTools.Method(typeof(DryingRackScrewStructure), "Awake");
                var postfix = AccessTools.Method(typeof(MeatDryerModule), nameof(OnDryingRackAwake));
                
                if (awakeMethod != null && postfix != null)
                {
                    _harmony.Patch(awakeMethod, postfix: new HarmonyMethod(postfix));
                    RLog.Msg("[MeatDryer] Harmony patch on DryingRackScrewStructure.Awake — OK");
                }
                else
                {
                    RLog.Warning($"[MeatDryer] Patch failed: Awake={awakeMethod != null}, Postfix={postfix != null}");
                }
            }
            catch (System.Exception ex)
            {
                RLog.Error($"[MeatDryer] Harmony Awake patch error: {ex}");
            }
            // Hook 2: SeasonsManager.LateUpdate — server-safe tick driver.
            // SdkEvents.OnInWorldUpdate does NOT fire on headless servers.
            // SeasonsManager is proven-safe to hook (WorldCommands already patches it)
            // and runs every frame on all tiers including headless.
            // NOTE: Do NOT hook ScrewStructure types — VTable corruption risk (§62).
            try
            {
                var lateUpdate = AccessTools.Method(typeof(SeasonsManager), "LateUpdate");
                var tickPostfix = AccessTools.Method(typeof(MeatDryerModule), nameof(OnSeasonsLateUpdate));
                
                if (lateUpdate != null && tickPostfix != null)
                {
                    _harmony.Patch(lateUpdate, postfix: new HarmonyMethod(tickPostfix));
                    RLog.Msg("[MeatDryer] Harmony patch on SeasonsManager.LateUpdate — OK (server tick driver)");
                }
                else
                {
                    RLog.Warning($"[MeatDryer] LateUpdate patch failed: LateUpdate={lateUpdate != null}, Postfix={tickPostfix != null}");
                }
            }
            catch (System.Exception ex)
            {
                RLog.Error($"[MeatDryer] Harmony SeasonsManager patch error: {ex}");
            }
            
            RLog.Msg("[MeatDryer] Module initialized (offset-based field access).");
        }

        // ===== IL2CPP DIRECT MEMORY ACCESS =====
        
        private static System.IntPtr GetPointer(DryingRackHookPoint hook)
        {
            // Cast through the IL2CPP type hierarchy to access Pointer property
            var baseObj = (Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)(object)hook;
            return baseObj.Pointer;
        }
        
        private static float ReadFloat(System.IntPtr ptr, int offset)
        {
            var bytes = new byte[4];
            Marshal.Copy(ptr + offset, bytes, 0, 4);
            return System.BitConverter.ToSingle(bytes, 0);
        }
        
        private static void WriteFloat(System.IntPtr ptr, int offset, float value)
        {
            var bytes = System.BitConverter.GetBytes(value);
            Marshal.Copy(bytes, 0, ptr + offset, 4);
        }

        /// <summary>
        /// Find DryingRackHookPoint components via Transform.GetChild recursion.
        /// GetComponent (singular) works; GetComponentsInChildren (generic plural) is stripped.
        /// </summary>
        private static List<DryingRackHookPoint> FindHookPoints(GameObject root)
        {
            var result = new List<DryingRackHookPoint>();
            try
            {
                CollectHookPoints(root.transform, result);
            }
            catch { }
            return result;
        }

        private static void CollectHookPoints(Transform parent, List<DryingRackHookPoint> result)
        {
            var hook = parent.GetComponent<DryingRackHookPoint>();
            if (hook != null)
                result.Add(hook);
            
            int childCount = parent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                try
                {
                    var child = parent.GetChild(i);
                    if (child != null)
                        CollectHookPoints(child, result);
                }
                catch { }
            }
        }

        /// <summary>
        /// Harmony Postfix on SeasonsManager.LateUpdate — server-safe tick driver.
        /// SeasonsManager runs every frame on ALL tiers including headless servers.
        /// This drives MeatDryer polling without relying on SdkEvents (dead on headless).
        /// </summary>
        private static void OnSeasonsLateUpdate()
        {
            OnGuiTick();
            
            // Welcome message main-thread dispatch (server/owner)
#if SERVER || OWNER
            Modules.BroadcastMessage.WelcomeHandler.Tick();
#endif
        }

        /// <summary>
        /// Primary tick — called from OnSeasonsLateUpdate (Harmony hook) on servers,
        /// and from ManagedOnUpdate (SdkEvents) on Owner/Client tiers.
        /// Throttled to Config.MeatDryerProximityCheckInterval (default 2s).
        /// </summary>
        public static void OnGuiTick()
        {
            if (_racks.Count == 0) return;

            float now = Time.time;
            if (now - _lastTickTime < Config.MeatDryerProximityCheckInterval.Value) return;
            _lastTickTime = now;
            
            if (!_tickLogged)
            {
                _tickLogged = true;
                RLog.Msg($"[MeatDryer] First tick — {_racks.Count} rack(s), InstantDry={Config.MeatDryerInstantDry.Value}");
            }

            try { ProcessAllRacks(); }
            catch (System.Exception ex) { RLog.Error($"[MeatDryer] Tick error: {ex.Message}"); }
        }

        /// <summary>
        /// Harmony Postfix — captures every DryingRackScrewStructure as it spawns.
        /// </summary>
        private static void OnDryingRackAwake(DryingRackScrewStructure __instance)
        {
            try
            {
                if (__instance == null) return;
                
                for (int i = 0; i < _racks.Count; i++)
                    if (_racks[i].Structure == __instance) return;
                
                var go = __instance.gameObject;
                if (go == null) return;
                
                var hooks = FindHookPoints(go);
                
                _racks.Add(new RackData
                {
                    Structure = __instance,
                    HookPoints = hooks
                });
                _attachedCount++;
                
                RLog.Msg($"[MeatDryer] Tracked rack #{_attachedCount}: {go.name} ({hooks.Count} hooks)");
                
                // Verify offsets on first hook
                if (!_offsetsVerified && hooks.Count > 0)
                {
                    _offsetsVerified = true;
                    try
                    {
                        var h = hooks[0];
                        var ptr = GetPointer(h);
                        float cureTime = ReadFloat(ptr, OFFSET_REQUIRED_CURE_TIME);
                        float remaining = ReadFloat(ptr, OFFSET_REMAINING_CURE_TIME);
                        float boost = ReadFloat(ptr, OFFSET_DRY_BOOST_MULTIPLIER);
                        RLog.Msg($"[MeatDryer] OFFSET VERIFY: cureTimeDays={cureTime}, remainingSec={remaining}, boostMul={boost}");
                    }
                    catch (System.Exception ex)
                    {
                        RLog.Warning($"[MeatDryer] OFFSET VERIFY failed: {ex.Message}");
                    }
                }
            }
            catch (System.Exception ex)
            {
                RLog.Error($"[MeatDryer] OnDryingRackAwake error: {ex.Message}");
            }
        }

        private static void ProcessAllRacks()
        {
            // Clean up destroyed racks — accessing .gameObject on a destroyed
            // IL2CPP object throws, which the catch block handles.
            for (int i = _racks.Count - 1; i >= 0; i--)
            {
                try
                {
                    var s = _racks[i].Structure;
                    if (s == null || s.gameObject == null)
                        _racks.RemoveAt(i);
                }
                catch { _racks.RemoveAt(i); }
            }
            
            if (_racks.Count == 0) return;
            
            // Get season via public static property
            int activeSeason = -1;
            try { activeSeason = (int)SeasonsManager.ActiveSeason; } catch { }
            
            foreach (var rack in _racks)
            {
                if (rack.HookPoints == null) continue;
                foreach (var hook in rack.HookPoints)
                {
                    // Destroyed IL2CPP objects throw on access — catch handles it
                    try
                    {
                        if (hook == null || hook.gameObject == null) continue;
                        ApplyConfigToHook(hook, activeSeason);
                    }
                    catch { /* hook was destroyed mid-iteration */ }
                }
            }
        }

        private static bool _instantDryLogged = false;
        
        private static void ApplyConfigToHook(DryingRackHookPoint hook, int activeSeason)
        {
            var ptr = GetPointer(hook);
            
            // ===== INSTANT DRY (highest priority — overrides all other settings) =====
            // Zeroing _remainingCureTimeSeconds alone does NOT work because the game
            // recalculates it each frame from _requiredCureTimeGameDays and elapsed time.
            // Multi-pronged approach: crush cure time + extreme boost + zero remaining.
            if (Config.MeatDryerInstantDry.Value)
            {
                try
                {
                    // Force fire proximity so drying can proceed at all
                    hook.SetTemperatureVolumeSettings(100f, true);
                    
                    // Set cure time to near-zero so the game calculates near-zero remaining
                    WriteFloat(ptr, OFFSET_REQUIRED_CURE_TIME, 0.0001f);
                    
                    // Set boost multiplier to extreme so any remaining time drains instantly
                    WriteFloat(ptr, OFFSET_DRY_BOOST_MULTIPLIER, 99999f);
                    
                    // Also zero remaining time directly as belt-and-suspenders
                    float remaining = ReadFloat(ptr, OFFSET_REMAINING_CURE_TIME);
                    if (remaining > 0f)
                        WriteFloat(ptr, OFFSET_REMAINING_CURE_TIME, 0f);
                    
                    // One-time diagnostic log
                    if (!_instantDryLogged)
                    {
                        _instantDryLogged = true;
                        RLog.Msg($"[MeatDryer] INSTANT DRY active — cureTime=0.0001, boost=99999, remaining={remaining}");
                    }
                }
                catch { }
                return; // Skip normal season/speed logic when instant dry is on
            }
            
            // ===== NORMAL MODE: season-based fire proximity =====
            if (activeSeason >= 0)
            {
                float baseBoost;
                switch (activeSeason)
                {
                    case 0: baseBoost = 30f; break; // Spring — best
                    case 1: baseBoost = 25f; break; // Summer
                    case 2: baseBoost = 15f; break; // Autumn
                    case 3: baseBoost = 5f;  break; // Winter — slowest
                    default: baseBoost = 15f; break;
                }
                
                // No Fire Required — force fire proximity via public API
                if (Config.MeatDryerNoFireRequired.Value)
                {
                    hook.SetTemperatureVolumeSettings(100f, true);
                }
                
                // Speed Multiplier — only override when user has changed the multiplier
                // At 1.0 (default), leave the game's native boost value untouched.
                // Without this guard, baseBoost (30 in Spring) replaces the game's
                // default (2), causing a 15x unintended speed increase.
                float speedMul = Config.MeatDryerSpeedMultiplier.Value;
                if (System.Math.Abs(speedMul - 1.0f) > 0.01f)
                {
                    float finalBoost = baseBoost * speedMul;
                    try { WriteFloat(ptr, OFFSET_DRY_BOOST_MULTIPLIER, finalBoost); } catch { }
                }
            }
            
            // Cure Time Override — write via memory offset
            float cureTimeOverride = Config.MeatDryerCureTimeDays.Value;
            if (cureTimeOverride >= 0f)
            {
                try { WriteFloat(ptr, OFFSET_REQUIRED_CURE_TIME, cureTimeOverride); } catch { }
            }
        }
    }
}

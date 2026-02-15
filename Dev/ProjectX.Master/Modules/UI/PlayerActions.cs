#if !SERVER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RedLoader;
using Sons.Ai.Vail;
using SonsSdk;
using TheForest;
using TheForest.Utils;
using UnityEngine;

namespace ProjectX.Master.Modules.UI
{
    /// <summary>
    /// Player actions that execute when menu toggles are changed
    /// Uses reflection to access DebugConsole.Instance and other game APIs
    /// </summary>
    public static class PlayerActions
    {
        private static object _debugConsoleInstance;
        private static MethodInfo _godmodeMethod;
        private static MethodInfo _energyhackMethod;
        private static bool _initialized;
        
        // NoClip state
        private static bool _noClipActive;
        private static float? _originalFallDamage;

        private static void EnsureInit()
        {
            if (_initialized) return;
            _initialized = true;

            try
            {
                // Find DebugConsole type - it's in the root namespace in Sons.dll
                var debugConsoleType = AccessTools.TypeByName("DebugConsole");
                if (debugConsoleType == null)
                {
                    RLog.Warning("[PlayerActions] DebugConsole type not found");
                    return;
                }

                // Get the Instance property
                var instanceProp = AccessTools.Property(debugConsoleType, "Instance");
                if (instanceProp != null)
                {
                    _debugConsoleInstance = instanceProp.GetValue(null);
                }

                // Cache methods
                _godmodeMethod = AccessTools.Method(debugConsoleType, "_godmode", new[] { typeof(string) });
                _energyhackMethod = AccessTools.Method(debugConsoleType, "_energyhack", new[] { typeof(string) });

                RLog.Msg($"[PlayerActions] DebugConsole initialized: Instance={_debugConsoleInstance != null}, GodMode={_godmodeMethod != null}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] Init failed: {ex.Message}");
            }
        }

        public static void ToggleGodMode(bool on)
        {
            Config.IsGodMode.Value = on;
            try
            {
                // Use SendCommand instead of direct Invoke to avoid CLR/Harmony detour conflicts
                string cmd = on ? "godmode on" : "godmode off";
                DebugConsole.Instance.SendCommand(cmd);
                RLog.Msg($"[PlayerActions] GodMode: {(on ? "on" : "off")}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] GodMode failed: {ex.Message}");
            }
        }

        public static void ToggleInfStamina(bool on)
        {
            Config.IsInfStamina.Value = on;
            try
            {
                // Use SendCommand instead of direct Invoke to avoid CLR/Harmony detour conflicts
                string cmd = on ? "energyhack on" : "energyhack off";
                DebugConsole.Instance.SendCommand(cmd);
                RLog.Msg($"[PlayerActions] InfStamina: {(on ? "on" : "off")}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] InfStamina failed: {ex.Message}");
            }
        }

        public static void ToggleNoHunger(bool on)
        {
            Config.IsNoHungry.Value = on;
            try
            {
                // SetMin is protected, use reflection like original mod at runtime
                var fullness = LocalPlayer.Vitals.Fullness;
                var setMinMethod = AccessTools.Method(fullness.GetType(), "SetMin");
                setMinMethod?.Invoke(fullness, new object[] { on ? 100f : 0f });
                RLog.Msg($"[PlayerActions] NoHunger: {on}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] NoHunger failed: {ex.Message}");
            }
        }

        public static void ToggleNoDehydration(bool on)
        {
            Config.IsNoDehydration.Value = on;
            try
            {
                var hydration = LocalPlayer.Vitals.Hydration;
                var setMinMethod = AccessTools.Method(hydration.GetType(), "SetMin");
                setMinMethod?.Invoke(hydration, new object[] { on ? 100f : 0f });
                RLog.Msg($"[PlayerActions] NoDehydration: {on}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] NoDehydration failed: {ex.Message}");
            }
        }

        public static void ToggleNoSleep(bool on)
        {
            Config.IsNoSleep.Value = on;
            try
            {
                var rested = LocalPlayer.Vitals.Rested;
                var setMinMethod = AccessTools.Method(rested.GetType(), "SetMin");
                setMinMethod?.Invoke(rested, new object[] { on ? 100f : 0f });
                RLog.Msg($"[PlayerActions] NoSleep: {on}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] NoSleep failed: {ex.Message}");
            }
        }
        
        public static void ToggleInfAmmo(bool on)
        {
            Config.IsInfiniteAmmo.Value = on;
            // TODO: Requires Harmony patch on RangedWeapon.Ammo.Remove
            RLog.Msg($"[PlayerActions] InfAmmo: {on} (needs Harmony patch)");
        }
        
        public static void ToggleNoFallDamage(bool on)
        {
            Config.IsNoFallDamage.Value = on;
            try
            {
                if (LocalPlayer.FpCharacter != null)
                {
                    if (_originalFallDamage == null)
                        _originalFallDamage = LocalPlayer.FpCharacter._baseFallDamage;
                    
                    LocalPlayer.FpCharacter._baseFallDamage = on ? 0f : _originalFallDamage.Value;
                    RLog.Msg($"[PlayerActions] NoFallDamage: {on}");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] NoFallDamage failed: {ex.Message}");
            }
        }
        
        // ========== NO CLIP / FLY ==========
        public static void ToggleNoClip(bool on)
        {
            Config.IsNoClip.Value = on;
            _noClipActive = on;
            
            try
            {
                if (LocalPlayer.FpCharacter != null)
                {
                    // Toggle rigidbody kinematic state
                    LocalPlayer.FpCharacter._rigidbody.isKinematic = on;
                    
                    // Toggle animator on camera shake driver
                    var cameraShake = LocalPlayer.Transform?.Find("PlayerAnimator/CameraShakeDriver");
                    if (cameraShake != null)
                    {
                        var animator = cameraShake.GetComponent<Animator>();
                        if (animator != null)
                            animator.enabled = !on;
                    }
                    
                    RLog.Msg($"[PlayerActions] NoClip: {on}");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] NoClip toggle failed: {ex.Message}");
            }
        }
        
        // Movement
        public static void SetWalkSpeed(float value)
        {
            Config.WalkSpeed.Value = value;
            try { LocalPlayer.FpCharacter?.SetWalkSpeed(value); } catch { }
        }

        public static void SetRunSpeed(float value)
        {
            Config.RunSpeed.Value = value;
            try { LocalPlayer.FpCharacter?.SetRunSpeed(value); } catch { }
        }

        public static void SetSwimSpeed(float value)
        {
            Config.SwimSpeed.Value = value;
            try { LocalPlayer.FpCharacter?.SetSwimSpeed(value); } catch { }
        }

        public static void SetJumpMultiplier(float value)
        {
            Config.JumpMultiplier.Value = value;
            try { LocalPlayer.FpCharacter?.SetSuperJump(value); } catch { }
        }

        /// <summary>
        /// Called every frame from MasterPlugin - handles NoClip movement
        /// </summary>
        public static void OnUpdate()
        {
            if (!_noClipActive) return;
            if (LocalPlayer._instance == null) return;
            
            try
            {
                // Ensure kinematic is still active
                if (!LocalPlayer.FpCharacter._rigidbody.isKinematic)
                {
                    LocalPlayer.FpCharacter._rigidbody.isKinematic = true;
                }
                
                // Get movement input
                float vertical = Input.GetAxisRaw("Vertical");
                float horizontal = Input.GetAxisRaw("Horizontal");
                float upDown = 0f;
                
                // Jump = go up, Crouch = go down
                if (Input.GetKey(KeyCode.Space))
                    upDown = Config.NoClipUpDownSpeed.Value;
                else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
                    upDown = -Config.NoClipUpDownSpeed.Value;
                
                if (Camera.main == null) return;
                
                // Calculate movement direction based on camera
                Vector3 forward = Camera.main.transform.forward;
                Vector3 right = Camera.main.transform.right;
                Vector3 moveDir = forward * vertical + right * horizontal;
                if (moveDir.magnitude > 1f)
                    moveDir.Normalize();
                
                // Apply movement
                Vector3 newPos = LocalPlayer.Transform.position;
                float speed = Config.NoClipSpeed.Value;
                
                // Shift = move faster
                if (Input.GetKey(KeyCode.LeftShift))
                    speed *= 10f;
                
                newPos += moveDir * speed * 0.1f + LocalPlayer.Transform.up * upDown;
                LocalPlayer.Transform.position = Vector3.Lerp(LocalPlayer.Transform.position, newPos, 0.1f);
            }
            catch { }
        }
        
        // ========== ENVIRONMENT ==========
        
        public static void SetSeason(string season)
        {
            try
            {
                // Convert string to Season enum
                Sons.Atmosphere.SeasonsManager.Season seasonEnum;
                switch (season.ToLower())
                {
                    case "spring": seasonEnum = Sons.Atmosphere.SeasonsManager.Season.Spring; break;
                    case "summer": seasonEnum = Sons.Atmosphere.SeasonsManager.Season.Summer; break;
                    case "autumn": case "fall": seasonEnum = Sons.Atmosphere.SeasonsManager.Season.Fall; break;
                    case "winter": seasonEnum = Sons.Atmosphere.SeasonsManager.Season.Winter; break;
                    default:
                        RLog.Warning($"[PlayerActions] Unknown season: {season}");
                        return;
                }
                
                // Use typed IL2CPP call — same pattern as ToggleForceRain() which works reliably.
                // SetSeasonPostDeserialize is PUBLIC and fires the full visual pipeline
                // including UpdateReceiversSeason() to notify all visual systems.
                // DebugConsole._season() only calls LockSeason() which sets fields
                // WITHOUT notifying receivers — that's why visuals never updated.
                var sm = Sons.Atmosphere.SeasonsManager.Instance;
                if (sm != null)
                {
                    float offset = sm.GetStartingDayOffset();
                    sm.SetSeasonPostDeserialize(seasonEnum, offset);
                    RLog.Msg($"[PlayerActions] Season set to: {season} — typed IL2CPP call (SetSeasonPostDeserialize)");
                }
                else
                {
                    // Fallback to debug console command
                    DebugConsole.Instance._season(season.ToLower());
                    RLog.Msg($"[PlayerActions] Season set to: {season} — fallback (_season command)");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] SetSeason failed: {ex.Message}");
            }
        }
        
        // SetDay - uses reflection (button-triggered one-shot is safe, no console cmd exists)
        private static MethodInfo _setDayMethod;
        private static bool _setDaySearched;
        
        public static void SetDay(int day)
        {
            try
            {
                // Cache via reflection - button-triggered so less risk than slider
                if (!_setDaySearched)
                {
                    _setDaySearched = true;
                    var timeOfDayHolderType = AccessTools.TypeByName("Sons.Gameplay.TimeOfDayHolder");
                    if (timeOfDayHolderType != null)
                    {
                        _setDayMethod = AccessTools.Method(timeOfDayHolderType, "SetDay", new[] { typeof(int) });
                    }
                }
                
                if (_setDayMethod != null)
                {
                    _setDayMethod.Invoke(null, new object[] { day });
                    RLog.Msg($"[PlayerActions] Set day to: {day}");
                }
                else
                {
                    RLog.Warning("[PlayerActions] SetDay: TimeOfDayHolder.SetDay not found");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] SetDay failed: {ex.Message}");
            }
        }
        
        // ========== ENVIRONMENT CONTROLS ==========
        
        public static void ToggleNoGravity(bool on)
        {
            Config.IsNoGravity.Value = on;
            try
            {
                LocalPlayer.FpCharacter.SetDisabledGravity(on);
                RLog.Msg($"[PlayerActions] NoGravity: {on}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] ToggleNoGravity failed: {ex.Message}");
            }
        }
        
        public static void ToggleNoGrass()
        {
            try
            {
                // Use SendCommand like AxelModMenu original
                DebugConsole.Instance.SendCommand("togglegrass");
                RLog.Msg("[PlayerActions] Toggled grass");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] ToggleNoGrass failed: {ex.Message}");
            }
        }
        
        public static void ToggleNoForest()
        {
            try
            {
                // Use SendCommand like AxelModMenu original
                DebugConsole.Instance.SendCommand("noforest");
                RLog.Msg("[PlayerActions] Toggled forest");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] ToggleNoForest failed: {ex.Message}");
            }
        }
        
        public static void ToggleForceRain(bool on)
        {
            try
            {
                // Use typed IL2CPP API directly — DebugConsole.SendCommand resolves
                // but has NO visual effect on the client (server-authority issue).
                // ForceRain(3) + StopRaining() are typed calls that work locally.
                // The CheckForRain PREFIX blocker prevents the game from overriding.
                var ws = TheForest.World.WeatherSystem.Instance;
                if (ws != null)
                {
                    if (on) ws.ForceRain(3);
                    else ws.StopRaining();
                    RLog.Msg($"[PlayerActions] Force Rain: {(on ? "ON (ForceRain 3)" : "OFF (StopRaining)")} — typed IL2CPP call");
                }
                else
                {
                    RLog.Warning("[PlayerActions] WeatherSystem.Instance is null");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] ToggleForceRain failed: {ex.Message}");
            }
        }
        
        // Store user-selected time for locking
        private static string _lockedTimeStr = "12:00";
        
        /// <summary>
        /// Set and lock the time of day (e.g., "12:00", "18:30")
        /// </summary>
        public static void SetTimeOfDay(string timeStr)
        {
            try
            {
                _lockedTimeStr = timeStr;
                DebugConsole.Instance.SendCommand($"settimeofday {timeStr}");
                RLog.Msg($"[PlayerActions] Time set to: {timeStr}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] SetTimeOfDay failed: {ex.Message}");
            }
        }
        
        public static void ToggleLockTime(bool on)
        {
            Config.IsLockTime.Value = on;
            try
            {
                if (on)
                {
                    // Lock at stored time (user can set via SetTimeOfDay first)
                    DebugConsole.Instance.SendCommand($"locktimeofday {_lockedTimeStr}");
                    RLog.Msg($"[PlayerActions] Time locked at {_lockedTimeStr}");
                }
                else
                {
                    // Unlock by setting time (which resumes normal flow)
                    DebugConsole.Instance.SendCommand($"settimeofday {_lockedTimeStr}");
                    RLog.Msg("[PlayerActions] Time unlocked");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] ToggleLockTime failed: {ex.Message}");
            }
        }
        
        // Cached reflection for daytime speed only
        private static MethodInfo _setBaseTimeSpeedMethod;
        private static bool _timeMethodsSearched;
        
        private static void InitTimeReflection()
        {
            if (_timeMethodsSearched) return;
            _timeMethodsSearched = true;
            
            // Try multiple possible type names
            Type timeOfDayHolderType = null;
            string[] possibleNames = {
                "Sons.Gameplay.TimeOfDayHolder",
                "TimeOfDayHolder",
                "Sons.TimeOfDayHolder",
                "Sons.Environment.TimeOfDayHolder"
            };
            
            foreach (var name in possibleNames)
            {
                timeOfDayHolderType = AccessTools.TypeByName(name);
                if (timeOfDayHolderType != null)
                {
                    RLog.Msg($"[PlayerActions] Found TimeOfDayHolder as: {name}");
                    break;
                }
            }
            
            if (timeOfDayHolderType != null)
            {
                _setBaseTimeSpeedMethod = AccessTools.Method(timeOfDayHolderType, "SetBaseTimeSpeed", new[] { typeof(float) });
                if (_setBaseTimeSpeedMethod == null)
                {
                    // Try without parameter type constraint
                    _setBaseTimeSpeedMethod = AccessTools.Method(timeOfDayHolderType, "SetBaseTimeSpeed");
                }
                
                if (_setBaseTimeSpeedMethod == null)
                {
                    RLog.Warning("[PlayerActions] SetBaseTimeSpeed method not found on TimeOfDayHolder");
                }
                else
                {
                    RLog.Msg($"[PlayerActions] Found SetBaseTimeSpeed: {_setBaseTimeSpeedMethod.Name}");
                }
            }
            else
            {
                RLog.Warning("[PlayerActions] TimeOfDayHolder type not found in any namespace");
            }
        }
        
        public static void SetDaytimeSpeed(float speed)
        {
            Config.DaytimeSpeed.Value = speed;
            try
            {
                InitTimeReflection();
                if (_setBaseTimeSpeedMethod != null)
                {
                    _setBaseTimeSpeedMethod.Invoke(null, new object[] { speed });
                    RLog.Msg($"[PlayerActions] Daytime speed set to: {speed}");
                }
                else
                {
                    RLog.Warning("[PlayerActions] SetDaytimeSpeed: Method not available");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] SetDaytimeSpeed failed: {ex.Message}");
            }
        }
        
        // Cached reflection for environment
        private static FieldInfo _regrowthFactorField;
        private static bool _treeRegrowSearched;
        
        public static void SetTreeRegrowRate(float rate)
        {
            Config.TreeRegrowRate.Value = rate;
            try
            {
                // FindObjectOfType<TreeRegrowChecker>()._regrowthFactor = rate
                if (!_treeRegrowSearched)
                {
                    _treeRegrowSearched = true;
                    var type = AccessTools.TypeByName("Sons.Environment.TreeRegrowChecker");
                    if (type != null)
                    {
                        _regrowthFactorField = AccessTools.Field(type, "_regrowthFactor");
                    }
                }
                
                if (_regrowthFactorField != null)
                {
                    var checker = UnityEngine.Object.FindObjectOfType(AccessTools.TypeByName("Sons.Environment.TreeRegrowChecker"));
                    if (checker != null)
                    {
                        _regrowthFactorField.SetValue(checker, rate);
                        RLog.Msg($"[PlayerActions] Tree regrow rate set to: {rate}");
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] SetTreeRegrowRate failed: {ex.Message}");
            }
        }
        
        // Cached reflection for wind
        private static MethodInfo _setAndLockIntensityMethod;
        private static MethodInfo _unlockWindMethod;
        private static bool _windMethodsSearched;
        
        public static void SetWindIntensity(float intensity)
        {
            Config.WindIntensity.Value = intensity;
            try
            {
                if (!_windMethodsSearched)
                {
                    _windMethodsSearched = true;
                    var windManagerType = AccessTools.TypeByName("Sons.Atmosphere.WindManager");
                    if (windManagerType != null)
                    {
                        _setAndLockIntensityMethod = AccessTools.Method(windManagerType, "SetAndLockIntensity", new[] { typeof(float) });
                        _unlockWindMethod = AccessTools.Method(windManagerType, "Unlock");
                    }
                }
                
                if (intensity < 0 && _unlockWindMethod != null)
                {
                    _unlockWindMethod.Invoke(null, null);
                    RLog.Msg("[PlayerActions] Wind unlocked (auto)");
                }
                else if (_setAndLockIntensityMethod != null)
                {
                    _setAndLockIntensityMethod.Invoke(null, new object[] { intensity });
                    RLog.Msg($"[PlayerActions] Wind intensity set to: {intensity}");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] SetWindIntensity failed: {ex.Message}");
            }
        }
        
        // ========== AI CONTROL ==========
        
        /// <summary>
        /// Freeze/Unfreeze the world simulation (stops spawning and AI)
        /// Based on original AxelModMenu Actors.FreezeActors()
        /// </summary>
        public static void ToggleFreezeAI(bool on)
        {
            Config.FreezeAI.Value = on;
            try
            {
                VailWorldSimulation.SetPaused(on);
                RLog.Msg($"[PlayerActions] FreezeAI: {(on ? "FROZEN - No spawning/AI" : "UNFROZEN - Normal spawning")}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] FreezeAI failed: {ex.Message}");
            }
        }
        
        // ========== KILL ALL ==========
        
        // Cached reflection for VailActorManager
        private static MethodInfo _getActiveActorsMethod;
        private static bool _actorMethodSearched;
        
        // Cached reflection for IgniteSelf (avoid JIT crash)
        private static MethodInfo _igniteSelfMethod;
        private static bool _igniteSelfSearched;
        
        /// <summary>
        /// Get active actors using reflection (DummyDll signature mismatch workaround)
        /// </summary>
        private static System.Collections.IEnumerable GetActiveActorsReflection()
        {
            try
            {
                if (!_actorMethodSearched)
                {
                    _actorMethodSearched = true;
                    var vailActorManagerType = AccessTools.TypeByName("Sons.Ai.Vail.VailActorManager");
                    if (vailActorManagerType != null)
                    {
                        _getActiveActorsMethod = AccessTools.Method(vailActorManagerType, "GetActiveActors");
                    }
                }
                
                if (_getActiveActorsMethod != null)
                {
                    var result = _getActiveActorsMethod.Invoke(null, null);
                    return result as System.Collections.IEnumerable;
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] GetActiveActorsReflection: {ex.Message}");
            }
            return null;
        }
        
        /// <summary>
        /// Kill all enemies (ClassId 1 = cannibals, ClassId 2 = mutants)
        /// Uses direct interop and per-actor exception handling for IL2CPP stability
        /// Supports radius filtering via Config.KillRadius (0 = no limit)
        /// </summary>
        public static void KillAllEnemies()
        {
            try
            {
                // Use reflection - direct interop has signature mismatch
                var actors = GetActiveActorsReflection();
                if (actors == null)
                {
                    RLog.Warning("[PlayerActions] KillAllEnemies: No active actors found");
                    return;
                }
                
                float radius = Config.KillRadius.Value;
                Vector3 playerPos = LocalPlayer._instance != null ? LocalPlayer._instance.transform.position : Vector3.zero;
                
                int killCount = 0;
                int errorCount = 0;
                
                foreach (var actorObj in actors)
                {
                    var actor = actorObj as VailActor;
                    if (actor == null) continue;
                    
                    try
                    {
                        // ClassId 1 = Cannibals, ClassId 2 = Mutants
                        int classId = (int)actor.ClassId;
                        if (classId == 1 || classId == 2)
                        {
                            // Check radius if set
                            if (radius > 0)
                            {
                                float distance = Vector3.Distance(playerPos, actor.transform.position);
                                if (distance > radius) continue;
                            }
                            
                            // Per-actor try/catch isolates JIT issues
                            actor.ForceDeath();
                            killCount++;
                        }
                    }
                    catch
                    {
                        errorCount++;
                    }
                }
                
                string radiusInfo = radius > 0 ? $" within {radius}m" : " (all loaded)";
                string errors = errorCount > 0 ? $" ({errorCount} errors)" : "";
                RLog.Msg($"[PlayerActions] KillAllEnemies: Killed {killCount} enemies{radiusInfo}{errors}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] KillAllEnemies failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Burn all enemies (ClassId 1 = cannibals, ClassId 2 = mutants)
        /// Uses reflection for actor list, per-actor exception handling for IL2CPP stability
        /// Supports radius filtering via Config.KillRadius (0 = no limit)
        /// </summary>
        public static void BurnAllEnemies()
        {
            try
            {
                // Use reflection - direct interop has signature mismatch
                var actors = GetActiveActorsReflection();
                if (actors == null)
                {
                    RLog.Warning("[PlayerActions] BurnAllEnemies: No active actors found");
                    return;
                }
                
                float radius = Config.KillRadius.Value;
                Vector3 playerPos = LocalPlayer._instance != null ? LocalPlayer._instance.transform.position : Vector3.zero;
                
                int burnCount = 0;
                int errorCount = 0;
                
                foreach (var actorObj in actors)
                {
                    var actor = actorObj as VailActor;
                    if (actor == null) continue;
                    
                    try
                    {
                        int classId = (int)actor.ClassId;
                        if (classId == 1 || classId == 2)
                        {
                            if (radius > 0)
                            {
                                float distance = Vector3.Distance(playerPos, actor.transform.position);
                                if (distance > radius) continue;
                            }
                            
                            actor.IgniteSelf(80f);
                            burnCount++;
                        }
                    }
                    catch
                    {
                        errorCount++;
                    }
                }
                
                string radiusInfo = radius > 0 ? $" within {radius}m" : " (all loaded)";
                string errors = errorCount > 0 ? $" ({errorCount} errors)" : "";
                RLog.Msg($"[PlayerActions] BurnAllEnemies: Ignited {burnCount} enemies{radiusInfo}{errors}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] BurnAllEnemies failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Kill all animals (ClassId 3)
        /// Uses reflection for actor list, per-actor exception handling for IL2CPP stability
        /// Supports radius filtering via Config.KillRadius (0 = no limit)
        /// </summary>
        public static void KillAllAnimals()
        {
            try
            {
                // Use reflection - direct interop has signature mismatch
                var actors = GetActiveActorsReflection();
                if (actors == null)
                {
                    RLog.Warning("[PlayerActions] KillAllAnimals: No active actors found");
                    return;
                }
                
                float radius = Config.KillRadius.Value;
                Vector3 playerPos = LocalPlayer._instance != null ? LocalPlayer._instance.transform.position : Vector3.zero;
                
                int killCount = 0;
                int errorCount = 0;
                int skippedCount = 0;
                
                foreach (var actorObj in actors)
                {
                    try
                    {
                        var actor = actorObj as VailActor;
                        if (actor == null) continue;
                        
                        // Check ClassId with separate try/catch
                        int classId = -1;
                        try { classId = (int)actor.ClassId; } catch { continue; }
                        
                        if (classId != 3) continue;
                        
                        // Check if already dead - accessing IsDead can crash on some actors
                        try
                        {
                            if (actor.IsDead()) 
                            {
                                skippedCount++;
                                continue;
                            }
                        }
                        catch { /* If IsDead throws, try to kill anyway */ }
                        
                        // Check distance with separate try/catch
                        if (radius > 0)
                        {
                            try
                            {
                                float distance = Vector3.Distance(playerPos, actor.transform.position);
                                if (distance > radius) continue;
                            }
                            catch { continue; } // If transform access fails, skip this actor
                        }
                        
                        // Kill with isolated try/catch
                        try
                        {
                            actor.ForceDeath();
                            killCount++;
                        }
                        catch
                        {
                            errorCount++;
                        }
                    }
                    catch
                    {
                        errorCount++;
                    }
                }
                
                string radiusInfo = radius > 0 ? $" within {radius}m" : " (all loaded)";
                string errors = errorCount > 0 ? $" ({errorCount} errors)" : "";
                string skipped = skippedCount > 0 ? $" ({skippedCount} already dead)" : "";
                RLog.Msg($"[PlayerActions] KillAllAnimals: Killed {killCount} animals{radiusInfo}{errors}{skipped}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] KillAllAnimals failed: {ex.Message}");
            }
        }
        
        // ========== INVENTORY / STACK CONTROLS ==========
        
        private static Type _itemDatabaseManagerType;
        private static MethodInfo _itemByIdMethod;
        private static PropertyInfo _itemsProperty;
        private static PropertyInfo _maxAmountProperty;
        private static bool _stackReflectionSearched;
        
        private static void InitStackReflection()
        {
            if (_stackReflectionSearched) return;
            _stackReflectionSearched = true;
            
            // Try to find ItemDatabaseManager
            string[] possibleNames = {
                "Sons.Items.Core.ItemDatabaseManager",
                "ItemDatabaseManager",
                "Sons.Inventory.ItemDatabaseManager"
            };
            
            foreach (var name in possibleNames)
            {
                _itemDatabaseManagerType = AccessTools.TypeByName(name);
                if (_itemDatabaseManagerType != null)
                {
                    RLog.Msg($"[PlayerActions] Found ItemDatabaseManager as: {name}");
                    break;
                }
            }
            
            if (_itemDatabaseManagerType == null)
            {
                RLog.Warning("[PlayerActions] ItemDatabaseManager type not found");
                return;
            }
            
            // Get ItemById method
            _itemByIdMethod = AccessTools.Method(_itemDatabaseManagerType, "ItemById", new[] { typeof(int) });
            if (_itemByIdMethod == null)
            {
                RLog.Warning("[PlayerActions] ItemById method not found");
            }
            
            // Get Items property (for iterating all items)
            _itemsProperty = AccessTools.Property(_itemDatabaseManagerType, "Items");
            if (_itemsProperty == null)
            {
                RLog.Warning("[PlayerActions] Items property not found");
            }
            
            // Find MaxAmount property on ItemData
            var itemDataType = AccessTools.TypeByName("Sons.Items.Core.ItemData") 
                ?? AccessTools.TypeByName("ItemData");
            if (itemDataType != null)
            {
                _maxAmountProperty = AccessTools.Property(itemDataType, "MaxAmount");
                if (_maxAmountProperty == null)
                {
                    // Try field access
                    var maxAmountField = AccessTools.Field(itemDataType, "MaxAmount");
                    if (maxAmountField != null)
                    {
                        RLog.Msg("[PlayerActions] MaxAmount is a field, not property");
                    }
                }
            }
        }
        
        /// <summary>
        /// Set all items to infinite stack (999999999)
        /// NOTE: Disabled due to IL2CPP reflection crashes. Use StackMod.dll instead.
        /// </summary>
        public static void SetInfiniteStacks()
        {
            RLog.Warning("[PlayerActions] Stack modification disabled in Master mod due to IL2CPP instability");
            RLog.Msg("[PlayerActions] Stack settings are managed via StackMod config file");
        }
        
        /// <summary>
        /// Set all items to a specific max stack value
        /// NOTE: Disabled due to IL2CPP reflection crashes. Use StackMod.dll instead.
        /// </summary>
        public static void SetAllItemsMaxStack(int maxAmount)
        {
            RLog.Warning("[PlayerActions] Stack modification disabled in Master mod due to IL2CPP instability");
            RLog.Msg("[PlayerActions] Stack settings are managed via StackMod config file");
        }
        
        // ========== NPC SPAWNING ==========
        
        // Cached reflection for spawn API
        private static MethodInfo _spawnActorMethod;
        private static MethodInfo _convertToRealActorMethod;
        private static PropertyInfo _graphMaskEverythingProp;
        private static MethodInfo _newFamilyMethod;
        private static bool _spawnApiSearched;
        private static System.Random _spawnRandom = new System.Random();
        
        // Cannibal TypeIds that are safe to spawn (regular combat cannibals)
        private static readonly VailActorTypeId[] SpawnableCannibals = new[]
        {
            (VailActorTypeId)11, // Fingers
            (VailActorTypeId)12, // Carl
            (VailActorTypeId)13, // Andy
            (VailActorTypeId)14, // Danny
            (VailActorTypeId)15, // Billy
            (VailActorTypeId)28, // MuddyFemale
            (VailActorTypeId)29, // MuddyMale
            (VailActorTypeId)30, // HeavyMale
            (VailActorTypeId)38, // FatMale
            (VailActorTypeId)39, // FatFemale
            (VailActorTypeId)43, // PaintedMale
            (VailActorTypeId)44, // PaintedFemale
        };
        
        private static void InitSpawnReflection()
        {
            if (_spawnApiSearched) return;
            _spawnApiSearched = true;
            
            try
            {
                // GraphMask.everything (from AstarPathfindingProject)
                var graphMaskType = AccessTools.TypeByName("Pathfinding.GraphMask");
                if (graphMaskType != null)
                {
                    _graphMaskEverythingProp = AccessTools.Property(graphMaskType, "everything");
                }
                
                // VailWorldSimulation.NewFamily()
                _newFamilyMethod = AccessTools.Method(typeof(VailWorldSimulation), "NewFamily");
                
                RLog.Msg($"[PlayerActions] Spawn API init: GraphMask={_graphMaskEverythingProp != null}, NewFamily={_newFamilyMethod != null}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] InitSpawnReflection failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Spawn a single NPC of the given type near the player.
        /// Adapted from VailSpawnControl's VailActorReplacer.Spawn() method.
        /// </summary>
        public static void SpawnNPC(VailActorTypeId typeId)
        {
            try
            {
                InitSpawnReflection();
                
                // Get prefab via SonsSdk ActorTools
                var prefab = ActorTools.GetPrefab(typeId);
                if (prefab == null)
                {
                    RLog.Warning($"[PlayerActions] SpawnNPC: No prefab found for {typeId}");
                    return;
                }
                RLog.Msg($"[PlayerActions] SpawnNPC: Got prefab for {typeId}");
                
                // Get VailWorldSimulation instance
                VailWorldSimulation sim = null;
                if (!VailWorldSimulation.TryGetInstance(out sim) || sim == null)
                {
                    RLog.Warning("[PlayerActions] SpawnNPC: VailWorldSimulation not available");
                    return;
                }
                RLog.Msg("[PlayerActions] SpawnNPC: Got VailWorldSimulation instance");
                
                // Calculate spawn position: 5m in front of player
                Vector3 spawnPos = LocalPlayer.Transform.position + Camera.main.transform.forward * 5f;
                
                // Get GraphMask.everything via reflection
                object graphMask = _graphMaskEverythingProp?.GetValue(null);
                if (graphMask == null)
                {
                    RLog.Warning("[PlayerActions] SpawnNPC: GraphMask.everything is null, trying fallback");
                    // Fallback: use -1 (all bits set) as raw int for GraphMask struct
                    var graphMaskType = AccessTools.TypeByName("Pathfinding.GraphMask");
                    if (graphMaskType != null)
                    {
                        graphMask = Activator.CreateInstance(graphMaskType, new object[] { -1 });
                    }
                }
                RLog.Msg($"[PlayerActions] SpawnNPC: GraphMask={graphMask != null}");
                
                // Get new family ID
                object familyId = _newFamilyMethod?.Invoke(null, null) ?? (object)0;
                RLog.Msg($"[PlayerActions] SpawnNPC: FamilyId={familyId}");
                
                // Find SpawnActor method - resolve by parameter count since there may be overloads
                var simType = typeof(VailWorldSimulation);
                System.Reflection.MethodInfo spawnMethod = null;
                foreach (var m in simType.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
                {
                    if (m.Name == "SpawnActor" && m.GetParameters().Length == 7)
                    {
                        spawnMethod = m;
                        break;
                    }
                }
                
                if (spawnMethod == null)
                {
                    // Try any SpawnActor overload and log all available
                    var allSpawn = simType.GetMethods().Where(m => m.Name == "SpawnActor").ToArray();
                    RLog.Warning($"[PlayerActions] SpawnNPC: No 7-param SpawnActor found. Available overloads: {allSpawn.Length}");
                    foreach (var m in allSpawn)
                    {
                        var parms = m.GetParameters();
                        RLog.Msg($"  SpawnActor({string.Join(", ", parms.Select(p => p.ParameterType.Name))})");
                    }
                    return;
                }
                RLog.Msg($"[PlayerActions] SpawnNPC: Found SpawnActor with params: {string.Join(", ", spawnMethod.GetParameters().Select(p => p.ParameterType.Name))}");
                
                // SpawnActor(prefab, position, graphMask, null, State.None, familyId, variationId)
                var worldSimActor = spawnMethod.Invoke(sim, new object[] 
                { 
                    prefab, spawnPos, graphMask, null, (int)0, familyId, 0 
                });
                
                if (worldSimActor == null)
                {
                    RLog.Warning($"[PlayerActions] SpawnNPC: SpawnActor returned null for {typeId}");
                    return;
                }
                RLog.Msg($"[PlayerActions] SpawnNPC: SpawnActor returned actor of type {worldSimActor.GetType().Name}");
                
                // Handle cave/surface area mask
                try
                {
                    var setKeepAbove = AccessTools.Method(worldSimActor.GetType(), "SetKeepAboveTerrain");
                    var setAreaMask = AccessTools.Method(worldSimActor.GetType(), "SetAreaMask");
                    var setGraphMask = AccessTools.Method(worldSimActor.GetType(), "SetGraphMask");
                    
                    // Check if we're in a cave via CaveEntranceManager.CurrentAreaMask
                    object areaMask = null;
                    var caveManagerType = AccessTools.TypeByName("Sons.Areas.CaveEntranceManager"); 
                    if (caveManagerType != null)
                    {
                        var currentAreaProp = AccessTools.Property(caveManagerType, "CurrentAreaMask");
                        areaMask = currentAreaProp?.GetValue(null);
                    }
                    
                    setKeepAbove?.Invoke(worldSimActor, new object[] { areaMask == null });
                    setAreaMask?.Invoke(worldSimActor, new object[] { areaMask });
                    
                    // Get nav graph mask for area
                    var getNavMethod = AccessTools.Method(typeof(VailWorldSimulation), "GetNavGraphMaskForArea");
                    if (getNavMethod != null && setGraphMask != null)
                    {
                        var navMask = getNavMethod.Invoke(sim, new object[] { areaMask, false });
                        setGraphMask.Invoke(worldSimActor, new object[] { navMask });
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[PlayerActions] SpawnNPC: Area mask setup warning: {ex.Message}");
                }
                
                // Convert to real actor
                var convertMethod = AccessTools.Method(typeof(VailWorldSimulation), "ConvertToRealActor");
                if (convertMethod == null)
                {
                    RLog.Warning("[PlayerActions] SpawnNPC: ConvertToRealActor method not found");
                    return;
                }
                convertMethod.Invoke(sim, new object[] { worldSimActor, prefab });
                
                RLog.Msg($"[PlayerActions] Spawned {typeId} at {spawnPos}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] SpawnNPC({typeId}) failed: {ex}");
            }
        }
        
        /// <summary>
        /// Spawn a group of 3 random cannibals near the player.
        /// Each cannibal spawns at a slightly offset position.
        /// </summary>
        public static void SpawnCannibalGroup()
        {
            try
            {
                int spawnCount = 3;
                for (int i = 0; i < spawnCount; i++)
                {
                    var typeId = SpawnableCannibals[_spawnRandom.Next(SpawnableCannibals.Length)];
                    SpawnNPC(typeId);
                }
                RLog.Msg($"[PlayerActions] Spawned cannibal group ({spawnCount} cannibals)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PlayerActions] SpawnCannibalGroup failed: {ex.Message}");
            }
        }
    }
}
#endif

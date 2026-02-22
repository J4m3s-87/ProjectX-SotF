using System;
using System.Reflection;
using RedLoader;
using HarmonyLib;
using UnityEngine;

namespace ProjectX.Master.Modules.DedicatedSuperuser.Commands
{
    /// <summary>
    /// World control commands — time, season, weather, AI, trees, companions.
    /// Uses AccessTools.TypeByName (IL2CPP-safe) and DebugConsole for server compatibility.
    /// </summary>
    public static class WorldCommands
    {
        public static void Handle(string steamId, string[] args)
        {
            if (args.Length == 0)
            {
                Log("Usage: /px world time|season|freeze|trees|revive|weather");
                return;
            }

            switch (args[0].ToLower())
            {
                case "time":
                    if (args.Length >= 2 && float.TryParse(args[1], out float hour))
                        SetTimeOfDay(hour);
                    else
                        Log("Usage: /px world time <0-24>");
                    break;

                case "season":
                    if (args.Length >= 2)
                        SetSeason(args[1]);
                    else
                        Log("Usage: /px world season spring|summer|fall|winter");
                    break;

                case "freeze":
                    ToggleFreezeAI();
                    break;

                case "trees":
                    ForceTreeRegrow();
                    break;

                case "revive":
                    if (args.Length >= 2)
                        ReviveCompanion(args[1]);
                    else
                        Log("Usage: /px world revive kelvin|virginia");
                    break;

                case "weather":
                    if (args.Length >= 2)
                        SetWeather(args[1]);
                    else
                        Log("Usage: /px world weather sunny|rain|snow");
                    break;

                default:
                    Log($"Unknown world command: {args[0]}");
                    break;
            }
        }

        /// <summary>
        /// Set time of day via DebugConsole (works on both client and server).
        /// Falls back to AccessTools reflection on TimeOfDayHolder.
        /// </summary>
        public static void SetTimeOfDay(float hour)
        {
            try
            {
                hour = Mathf.Clamp(hour, 0f, 24f);
                
                // Format as HH:MM for the debug console command
                int h = (int)hour;
                int min = (int)((hour - h) * 60);
                string timeStr = $"{h}:{min:D2}";
                
                // Primary: Use DebugConsole.SendCommand (works on server — proven by Registered command entries)
                try
                {
                    var dcType = AccessTools.TypeByName("DebugConsole");
                    if (dcType != null)
                    {
                        var instanceProp = AccessTools.Property(dcType, "Instance");
                        var dc = instanceProp?.GetValue(null);
                        if (dc != null)
                        {
                            var sendMethod = AccessTools.Method(dcType, "SendCommand", new[] { typeof(string) });
                            sendMethod?.Invoke(dc, new object[] { $"settimeofday {timeStr}" });
                            Log($"Time set to {hour:F1} via DebugConsole");
                            return;
                        }
                    }
                    Log("DebugConsole not available, trying reflection fallback...");
                }
                catch (Exception ex)
                {
                    Log($"DebugConsole fallback: {ex.Message}");
                }
                
                // Fallback: AccessTools.TypeByName (IL2CPP-safe — scans all loaded assemblies)
                var todType = AccessTools.TypeByName("Sons.Gameplay.TimeOfDayHolder") ??
                              AccessTools.TypeByName("Sons.Environment.TimeOfDayHolder") ??
                              AccessTools.TypeByName("TimeOfDayHolder");
                
                if (todType != null)
                {
                    Log($"Found TimeOfDayHolder type: {todType.FullName}");
                    
                    // Try SetTimeOfDay static method
                    var setTimeMethod = AccessTools.Method(todType, "SetTimeOfDay", new[] { typeof(float) }) ??
                                        AccessTools.Method(todType, "SetTime", new[] { typeof(float) });
                    if (setTimeMethod != null)
                    {
                        setTimeMethod.Invoke(null, new object[] { hour });
                        Log($"Time set to {hour:F1} via reflection");
                        return;
                    }
                    
                    // Try SetDay static method
                    var setDayMethod = AccessTools.Method(todType, "SetDay", new[] { typeof(int) });
                    if (setDayMethod != null)
                    {
                        setDayMethod.Invoke(null, new object[] { (int)hour });
                        Log($"Time set to {(int)hour} via SetDay");
                        return;
                    }
                    
                    // Dump available methods for diagnostics
                    Log("TimeOfDayHolder found but no SetTime/SetDay method. Available methods:");
                    foreach (var method in todType.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public))
                    {
                        Log($"  {(method.IsStatic ? "static " : "")}{method.Name}({string.Join(", ", Array.ConvertAll(method.GetParameters(), p => p.ParameterType.Name))})");
                    }
                }
                else
                {
                    Log("TimeOfDayHolder type not found in any namespace");
                }
            }
            catch (Exception ex)
            {
                Log($"SetTimeOfDay failed: {ex.Message}");
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // Season lock via Harmony PREFIX on SeasonsManager.LateUpdate.
        // Without this, the server's LateUpdate recalculates season from
        // game-day and reverts the one-shot SendCommand within 1 frame.
        // Same proven pattern as ProjectXGUI.LocalSetSeason() on client.
        // Offsets from dump.cs:
        //   SeasonsManager._activeSeason  = 0x70 (int32 enum)
        //   SeasonsManager._seasonIsLocked = 0x75 (bool)
        // ═══════════════════════════════════════════════════════════════
        
        private const int SEASON_ACTIVE_OFFSET = 0x70;
        private const int SEASON_LOCKED_OFFSET = 0x75;
        
        private static int? _serverForcedSeason = null; // null=no override, 0=Spring,1=Summer,2=Fall,3=Winter
        private static bool _serverSeasonPatchApplied = false;
        private static PropertyInfo _serverSmPointerProp;
        private static HarmonyLib.Harmony _serverSeasonHarmony;
        
        /// <summary>
        /// Set the game season with server-side Harmony PREFIX lock.
        /// DebugConsole.SendCommand is still used for the initial trigger,
        /// but the PREFIX holds it locked every frame via Marshal offsets.
        /// </summary>
        public static void SetSeason(string seasonName)
        {
            try
            {
                seasonName = seasonName.ToLower();
                
                // Unlock season — return to natural day-based progression
                if (seasonName == "unlock")
                {
                    _serverForcedSeason = null; // Release PREFIX lock
                    if (TrySendDebugCommand("unlockseason"))
                        Log("Season unlocked — returning to natural progression");
                    else
                        Log("Season unlock failed — DebugConsole not available");
                    return;
                }
                
                // Validate season name
                if (seasonName != "spring" && seasonName != "summer" && 
                    seasonName != "autumn" && seasonName != "fall" && seasonName != "winter")
                {
                    Log($"Invalid season '{seasonName}'. Use: spring, summer, autumn, winter");
                    return;
                }
                
                // Normalize "fall" to "autumn" (game uses "autumn")
                if (seasonName == "fall") seasonName = "autumn";
                
                int seasonVal = seasonName switch
                {
                    "spring" => 0,
                    "summer" => 1,
                    "autumn" => 2,
                    "winter" => 3,
                    _ => -1
                };
                
                // Apply Harmony PREFIX on first use
                if (!_serverSeasonPatchApplied)
                {
                    try
                    {
                        var smType = AccessTools.TypeByName("SeasonsManager");
                        if (smType == null) { Log("SeasonsManager type not found"); return; }
                        
                        var sm = AccessTools.Property(smType, "Instance")?.GetValue(null);
                        if (sm != null)
                        {
                            _serverSmPointerProp = sm.GetType().GetProperty("Pointer",
                                BindingFlags.Instance | BindingFlags.Public |
                                BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                            
                            RLog.Msg($"[Superuser] Season PREFIX: Pointer property {(_serverSmPointerProp != null ? "found" : "NULL")}");
                        }
                        else
                        {
                            RLog.Warning("[Superuser] SeasonsManager.Instance is null");
                        }
                        
                        _serverSeasonHarmony = new HarmonyLib.Harmony("ProjectX.Server.SeasonOverride");
                        
                        // Patch LateUpdate — writes our season value every frame
                        var lateUpdate = AccessTools.Method(smType, "LateUpdate");
                        if (lateUpdate != null)
                        {
                            var prefix = new HarmonyLib.HarmonyMethod(typeof(WorldCommands), nameof(ServerSeasonLateUpdatePrefix));
                            _serverSeasonHarmony.Patch(lateUpdate, prefix: prefix);
                            RLog.Msg("[Superuser] Harmony PREFIX on SeasonsManager.LateUpdate applied (server-side)");
                        }
                        
                        // Patch UpdateTime — BLOCKS the game's time-based season recalculation
                        // UpdateTime is a virtual override from TimeOfDayBehaviour, called on a
                        // separate code path from LateUpdate. Without blocking this, it overrides
                        // our Marshal writes between frames.
                        var updateTime = AccessTools.Method(smType, "UpdateTime");
                        if (updateTime != null)
                        {
                            var utPrefix = new HarmonyLib.HarmonyMethod(typeof(WorldCommands), nameof(ServerSeasonUpdateTimePrefix));
                            _serverSeasonHarmony.Patch(updateTime, prefix: utPrefix);
                            RLog.Msg("[Superuser] Harmony PREFIX on SeasonsManager.UpdateTime applied (server-side)");
                        }
                        
                        _serverSeasonPatchApplied = (lateUpdate != null || updateTime != null);
                        if (!_serverSeasonPatchApplied)
                            RLog.Warning("[Superuser] Neither LateUpdate nor UpdateTime found on SeasonsManager");
                    }
                    catch (Exception ex) { RLog.Warning($"[Superuser] Season PREFIX patch failed: {ex.Message}"); }
                }
                
                // Set forced season FIRST so PREFIX holds the new value
                _serverForcedSeason = seasonVal;
                
                // Re-enable rain system — a prior Sunny toggle may have disabled it
                // via 'enablerain off'. Season change should let natural weather resume.
                TrySendDebugCommand("enablerain on");
                
                // Then trigger the full season pipeline via DebugConsole
                if (TrySendDebugCommand($"season {seasonName}"))
                {
                    Log($"Season set to {seasonName} (weather system re-enabled)");
                }
                else
                {
                    Log($"Season change failed — DebugConsole not available");
                }
            }
            catch (Exception ex)
            {
                Log($"SetSeason failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Harmony PREFIX on SeasonsManager.LateUpdate — runs BEFORE the game's
        /// season recalculation. Writes _activeSeason and _seasonIsLocked=true
        /// via Marshal offsets so LateUpdate skips recalculation.
        /// </summary>
        private static void ServerSeasonLateUpdatePrefix(object __instance)
        {
            if (_serverForcedSeason == null || _serverSmPointerProp == null) return;
            
            try
            {
                System.IntPtr ptr = (System.IntPtr)_serverSmPointerProp.GetValue(__instance);
                if (ptr == System.IntPtr.Zero) return;
                
                // Write _activeSeason and _seasonIsLocked=FALSE
                // _seasonIsLocked=false lets LateUpdate detect the change and fire native SetSeason()
                // which notifies all visual receivers. UpdateTime is blocked separately.
                System.Runtime.InteropServices.Marshal.WriteInt32(ptr + SEASON_ACTIVE_OFFSET, _serverForcedSeason.Value);
                System.Runtime.InteropServices.Marshal.WriteByte(ptr + SEASON_LOCKED_OFFSET, 0);
            }
            catch { } // Silent — runs every frame
        }
        
        /// <summary>
        /// Harmony PREFIX on SeasonsManager.UpdateTime — BLOCKS the game's
        /// time-based season recalculation when a forced season is active.
        /// UpdateTime is called from TimeOfDayBehaviour on a separate path
        /// from LateUpdate, and was overriding our Marshal writes.
        /// </summary>
        private static bool ServerSeasonUpdateTimePrefix()
        {
            if (_serverForcedSeason == null) return true; // No override, run original
            return false; // Block UpdateTime — preserve forced season state
        }

        /// <summary>
        /// Toggle AI freeze on/off.
        /// Uses AccessTools to call VailWorldSimulation.SetPaused() (IL2CPP-safe).
        /// </summary>
        public static void ToggleFreezeAI()
        {
            try
            {
                Config.FreezeAI.Value = !Config.FreezeAI.Value;
                bool on = Config.FreezeAI.Value;

                // Actually pause/unpause the world simulation via reflection
                try
                {
                    var simType = AccessTools.TypeByName("VailWorldSimulation");
                    if (simType != null)
                    {
                        var setPaused = AccessTools.Method(simType, "SetPaused", new[] { typeof(bool) });
                        if (setPaused != null)
                        {
                            setPaused.Invoke(null, new object[] { on });
                            RLog.Msg($"[Superuser] VailWorldSimulation.SetPaused({on})");
                        }
                        else
                        {
                            RLog.Warning("[Superuser] VailWorldSimulation.SetPaused method not found");
                        }
                    }
                    else
                    {
                        RLog.Warning("[Superuser] VailWorldSimulation type not found");
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[Superuser] SetPaused failed: {ex.Message}");
                }

                Config.Save();
                Log($"AI Freeze: {(on ? "ON" : "OFF")}");
            }
            catch (Exception ex)
            {
                Log($"ToggleFreezeAI failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Enable tree regrowth via PlayerPreferences.SetLocalTreeRegrowth (IL2CPP-safe).
        /// There is no "TreeRegrowthSystem" class — the actual API is:
        ///   PlayerPreferences.SetLocalTreeRegrowth(bool) — static method
        ///   PlayerPreferences.TreeRegrowthLocal — property (get/set)
        /// </summary>
        public static void ForceTreeRegrow()
        {
            try
            {
                // Primary: PlayerPreferences.SetLocalTreeRegrowth(true)
                var ppType = AccessTools.TypeByName("PlayerPreferences");
                if (ppType != null)
                {
                    var setMethod = AccessTools.Method(ppType, "SetLocalTreeRegrowth", new[] { typeof(bool) });
                    if (setMethod != null)
                    {
                        setMethod.Invoke(null, new object[] { true });
                        Log("Tree regrowth enabled (PlayerPreferences)");
                        return;
                    }
                    
                    // Fallback: set the property directly
                    var prop = AccessTools.Property(ppType, "TreeRegrowthLocal");
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(null, true);
                        Log("Tree regrowth enabled (TreeRegrowthLocal)");
                        return;
                    }
                }
                
                Log("PlayerPreferences.SetLocalTreeRegrowth not accessible");
            }
            catch (Exception ex)
            {
                Log($"ForceTreeRegrow failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Revive a companion (Kelvin or Virginia).
        /// Step 1: Use DebugConsole "removedead" to clear dead bodies
        /// Step 2: Use "addcharacter robby/virginia" to spawn them fresh
        /// </summary>
        public static void ReviveCompanion(string companion)
        {
            try
            {
                companion = companion.ToLower();
                string targetName = companion switch
                {
                    "kelvin" or "robby" or "robbie" => "robby",
                    "virginia" => "virginia",
                    _ => null
                };

                if (targetName == null)
                {
                    Log($"Unknown companion: {companion}. Use 'kelvin' or 'virginia'");
                    return;
                }

                Log($"Attempting to revive {companion} (internal name: {targetName})...");
                
                // Step 1: Remove dead bodies via DebugConsole
                if (TrySendDebugCommand("removedead"))
                {
                    Log("Dead bodies cleared via 'removedead' console command");
                }
                
                // Step 2: Spawn the companion fresh via addcharacter command
                // This is the correct debug console command for companion revival
                if (TrySendDebugCommand($"addcharacter {targetName}"))
                {
                    Log($"Sent 'addcharacter {targetName}' — companion should spawn");
                }
                else
                {
                    // Fallback: try with count parameter
                    TrySendDebugCommand($"addcharacter {targetName} 1");
                    Log($"Sent 'addcharacter {targetName} 1' fallback");
                }
                
                Log($"Revive sequence complete for {companion}.");
            }
            catch (Exception ex)
            {
                Log($"ReviveCompanion failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Set weather via DebugConsole.SendCommand.
        /// Uses "forcerain" command (discovered via method dump: _forcerain(string arg) on DebugConsole).
        /// Direct property setters (set_IsRaining, set_IsSnowing) modify server state
        /// but don't replicate to clients.
        /// </summary>
        public static void SetWeather(string weather)
        {
            try
            {
                weather = weather.ToLower();
                
                switch (weather)
                {
                    case "sunny" or "clear":
                        // enablerain off = hard toggle, disables entire rain system
                        TrySendDebugCommand("enablerain off");
                        Log("Weather: enablerain off (sunny)");
                        break;
                        
                    case "rain" or "rainy":
                        TrySendDebugCommand("enablerain on");
                        TrySendDebugCommand("forcerain heavy");
                        Log("Weather: enablerain on + forcerain heavy");
                        break;
                        
                    case "snow" or "snowy":
                // Snow = winter season + rain
                TrySendDebugCommand("season winter");
                TrySendDebugCommand("enablerain on");
                TrySendDebugCommand("forcerain heavy");
                Log("Weather: snow (season winter + enablerain on + forcerain heavy)");
                break;
                        
                    default:
                        Log($"Unknown weather: {weather}. Use: sunny, rain, snow");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log($"SetWeather failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Try to send a command via the game's DebugConsole
        /// </summary>
        private static bool TrySendDebugCommand(string command)
        {
            try
            {
                var dcType = AccessTools.TypeByName("DebugConsole");
                if (dcType != null)
                {
                    var instanceProp = AccessTools.Property(dcType, "Instance");
                    var dc = instanceProp?.GetValue(null);
                    if (dc != null)
                    {
                        var sendMethod = AccessTools.Method(dcType, "SendCommand", new[] { typeof(string) });
                        sendMethod?.Invoke(dc, new object[] { command });
                        Log($"DebugConsole: sent '{command}'");
                        return true;
                    }
                }
                Log("DebugConsole not accessible");
            }
            catch (Exception ex)
            {
                Log($"DebugConsole command failed: {ex.Message}");
            }
            return false;
        }

        private static void Log(string msg)
        {
            // Send to all connected players' chat AND server log
            Utility.ChatResponse.Send(msg);
        }
    }
}

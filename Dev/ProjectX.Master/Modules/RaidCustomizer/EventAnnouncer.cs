using RedLoader;
using Sons.Characters;
using SonsSdk;
using UnityEngine;
using System;
using Endnight.Utilities;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Announces incoming raids to the player via on-screen message and optional sound.
    /// Matches original RaidCustomizer mod by ToniMacaroni/codengine.
    /// </summary>
    public static class EventAnnouncer
    {
        private const float SoundCooldown = 5f;
        private static float _lastSoundPlayedTime = -1f;
        
        /// <summary>
        /// Announce an incoming raid with on-screen message
        /// </summary>
        public static void Announce(VailWorldEventData.SearchPartyEvent evt)
        {
            if (!RaidConfig.AnnounceIncomingSearchParties.Value)
                return;
            
            try
            {
                string eventType = Enum.GetName(typeof(VailWorldEventData.TypeOfEvent), evt.type);
                string enemyCount = PrintEnemyCount(evt);
                string description = "Unknown";
                
                try
                {
                    description = evt.GetDescription();
                }
                catch { }
                
                string message = $"Raid \"{description}\" ({eventType}) incoming ({enemyCount})...";
                
                SonsTools.ShowMessage(message, 10f);
                RLog.Msg(System.Drawing.Color.Orange, $"[RaidCustomizer] {message}");
                
                PlayAnnouncementSound();
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] EventAnnouncer failed: {ex.Message}");
            }
        }
        
        private static string PrintEnemyCount(VailWorldEventData.SearchPartyEvent evt)
        {
            if (evt.spawnCount != evt.spawnCountMax)
                return $"{evt.spawnCount}-{evt.spawnCountMax} enemies";
            
            if (evt.spawnCount == 1)
                return $"{evt.spawnCount} enemy";
            
            return $"{evt.spawnCount} enemies";
        }
        
        private static System.Reflection.MethodInfo _cachedPlayOneshot;
        private static bool _playOneshotCached;
        
        private static void PlayAnnouncementSound()
        {
            if (!RaidConfig.PlaySoundWhenAnnounced.Value)
                return;
            
            if (SoundHasCooldown())
                return;
            
            _lastSoundPlayedTime = Time.time;
            
#if !SERVER
            try
            {
                // Cache the PlayOneshot method on first call — IL2CPP signature differs from decompiled source
                if (!_playOneshotCached)
                {
                    _playOneshotCached = true;
                    var fmodType = typeof(FMODCommon);
                    var methods = fmodType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    
                    // Log all PlayOneshot overloads for diagnostics
                    foreach (var m in methods)
                    {
                        if (m.Name == "PlayOneshot")
                        {
                            var parms = m.GetParameters();
                            string sig = string.Join(", ", Array.ConvertAll(parms, p => $"{p.ParameterType.Name} {p.Name}"));
                            RLog.Msg($"[RaidCustomizer] Found FMODCommon.PlayOneshot({sig})");
                            
                            // Try to find the overload: (string, Vector3) or (string, Vector3, int, ...)
                            if (_cachedPlayOneshot == null && parms.Length >= 2)
                                _cachedPlayOneshot = m;
                        }
                    }
                    
                    if (_cachedPlayOneshot == null)
                        RLog.Warning("[RaidCustomizer] No FMODCommon.PlayOneshot method found!");
                    else
                    {
                        var cachedParms = _cachedPlayOneshot.GetParameters();
                        string cachedSig = string.Join(", ", Array.ConvertAll(cachedParms, p => $"{p.ParameterType.Name} {p.Name}"));
                        RLog.Msg($"[RaidCustomizer] Using PlayOneshot({cachedSig})");
                    }
                }
                
                if (_cachedPlayOneshot == null) return;
                
                // Get player position matching original mod
                int playerCount;
                Vector3 playerPos = PlayerLocation.GetFirst(out playerCount, true);
                
                // Build args to match the discovered signature
                var parameters = _cachedPlayOneshot.GetParameters();
                object[] args;
                
                if (parameters.Length == 2)
                    args = new object[] { "event:/music/ambush", playerPos };
                else if (parameters.Length == 3)
                    args = new object[] { "event:/music/ambush", playerPos, null };
                else if (parameters.Length == 4)
                    args = new object[] { "event:/music/ambush", playerPos, 2, null };
                else
                    args = new object[] { "event:/music/ambush", playerPos };
                
                RLog.Msg($"[RaidCustomizer] ★ Playing ambush sound at {playerPos} (args={args.Length})");
                _cachedPlayOneshot.Invoke(null, args);
                RLog.Msg($"[RaidCustomizer] ★ Ambush sound fired OK");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] Sound playback failed: {ex.Message}\n{ex.InnerException?.Message}");
            }
#endif
        }
        
        private static bool SoundHasCooldown()
        {
            return Time.time - SoundCooldown <= _lastSoundPlayedTime;
        }
    }
}


using RedLoader;
using Sons.Characters;
using SonsSdk;
using UnityEngine;
using System;
using System.Reflection;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Announces incoming raids to the player
    /// </summary>
    public static class EventAnnouncer
    {
        private const float SoundCooldown = 5f;
        private static float _lastSoundPlayedTime = -1f;
        
        // Cached reflection for FMODCommon
        private static Type _fmodCommonType;
        private static MethodInfo _playOneshotMethod;
        private static bool _reflectionCached = false;
        private static bool _soundEnabled = true;
        
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
        
        private static void PlayAnnouncementSound()
        {
            if (!RaidConfig.PlaySoundWhenAnnounced.Value || !_soundEnabled)
                return;
            
            if (SoundHasCooldown())
                return;
            
            _lastSoundPlayedTime = Time.time;
            
            try
            {
                CacheReflection();
                
                if (_playOneshotMethod == null)
                {
                    _soundEnabled = false;
                    RLog.Warning("[RaidCustomizer] FMODCommon.PlayOneshot not available - sounds disabled");
                    return;
                }
                
                // Get player position for 3D sound
                var playerPos = TheForest.Utils.LocalPlayer.Transform != null 
                    ? TheForest.Utils.LocalPlayer.Transform.position 
                    : Vector3.zero;
                
                // Call FMODCommon.PlayOneshot(string path, Vector3 position, object[] parameterValues)
                // The original mod used "event:/music/ambush"
                _playOneshotMethod.Invoke(null, new object[] { "event:/music/ambush", playerPos, null });
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] Sound playback failed: {ex.Message}");
                _soundEnabled = false; // Don't keep trying if it fails
            }
        }
        
        private static void CacheReflection()
        {
            if (_reflectionCached) return;
            _reflectionCached = true;
            
            try
            {
                // FMODCommon is in the global namespace in Sons.FMOD assembly
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (asm.GetName().Name.Contains("FMOD") || asm.GetName().Name.Contains("Sons"))
                    {
                        _fmodCommonType = asm.GetType("FMODCommon");
                        if (_fmodCommonType != null)
                        {
                            // Find the PlayOneshot(string, Vector3, object[]) overload
                            _playOneshotMethod = _fmodCommonType.GetMethod("PlayOneshot", 
                                BindingFlags.Public | BindingFlags.Static,
                                null,
                                new Type[] { typeof(string), typeof(Vector3), typeof(object[]) },
                                null);
                            
                            if (_playOneshotMethod != null)
                            {
                                RLog.Msg("[RaidCustomizer] FMODCommon sound system initialized!");
                                return;
                            }
                        }
                    }
                }
                
                RLog.Warning("[RaidCustomizer] FMODCommon type not found in loaded assemblies");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] Failed to cache FMODCommon reflection: {ex.Message}");
            }
        }
        
        private static bool SoundHasCooldown()
        {
            return Time.time - SoundCooldown <= _lastSoundPlayedTime;
        }
    }
}

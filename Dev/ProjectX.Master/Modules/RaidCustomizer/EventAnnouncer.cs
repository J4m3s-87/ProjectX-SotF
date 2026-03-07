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
                // Match original mod: use PlayerLocation.GetFirst() from Endnight.Utilities
                int playerCount;
                Vector3 playerPos = PlayerLocation.GetFirst(out playerCount, true);
                
                RLog.Msg($"[RaidCustomizer] ★ Playing ambush sound at {playerPos} (players={playerCount})");
                FMODCommon.PlayOneshot("event:/music/ambush", playerPos, 2, null);
                RLog.Msg($"[RaidCustomizer] ★ Ambush sound fired OK");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RaidCustomizer] Sound playback failed: {ex.Message}\n{ex.StackTrace}");
            }
#endif
        }
        
        private static bool SoundHasCooldown()
        {
            return Time.time - SoundCooldown <= _lastSoundPlayedTime;
        }
    }
}


using System;
using System.Collections.Generic;
using Endnight.Utilities;
using RedLoader;
using Sons.Characters;
using UnityEngine;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Configures search party events based on config settings
    /// </summary>
    public static class SearchPartyEventConfigurator
    {
        private static readonly Dictionary<int, SearchPartyEventDefaults> SearchPartyDefaults = new();
        
        /// <summary>
        /// Get search party events via typed IL2CPP access (works on both client and server)
        /// Falls back to public property if underscore-prefixed field access fails
        /// </summary>
        private static IEnumerable<VailWorldEventData.SearchPartyEvent> GetSearchPartyEvents(VailWorldEventData data)
        {
            try { return data._searchPartyEvents.ToArray(); }
            catch { }
            try { return data.SearchPartyEvents.ToArray(); }
            catch { }
            RLog.Warning("[RaidCustomizer] Failed to access search party events");
            return Array.Empty<VailWorldEventData.SearchPartyEvent>();
        }
        
        /// <summary>
        /// Get creepy attack events via typed IL2CPP access
        /// </summary>
        private static IEnumerable<VailWorldEventData.SearchPartyEvent> GetCreepyAttackEvents(VailWorldEventData data)
        {
            try { return data._creepyAttackEvents.ToArray(); }
            catch { }
            try { return data.CreepyAttackEvents.ToArray(); }
            catch { }
            RLog.Warning("[RaidCustomizer] Failed to access creepy attack events");
            return Array.Empty<VailWorldEventData.SearchPartyEvent>();
        }
        
        /// <summary>
        /// Prepare all events for a new day with configured settings
        /// </summary>
        public static void PrepareForNewDay(VailWorldEventData vailWorldEventData)
        {
            try
            {
                int spawnCountAdjustment = GetSpawnCountAdjustment();
                
                // Collect all search party events
                var allEvents = new HashSet<VailWorldEventData.SearchPartyEvent>(GetSearchPartyEvents(vailWorldEventData));
                foreach (var evt in GetCreepyAttackEvents(vailWorldEventData))
                {
                    allEvents.Add(evt);
                }
                
                // Configure each event
                foreach (var evt in allEvents)
                {
                    if (!EventTools.IsActualSearchParty(evt)) continue;
                    
                    var defaults = GetOrCreateDefaults(evt);
                    
                    // TimeOfEvent: 2 = AnyTime equivalent (day + night) 
                    evt.timeOfEvent = evt.enabled ? (VailWorldEventData.TimeOfEvent)2 : defaults.TimeOfEvent;
                    evt.chance = 1f;
                    
                    bool isBoss = EventTools.IsBossEvent(defaults);
                    
                    if (isBoss)
                    {
                        ConfigureBossPartySize(evt);
                    }
                    else
                    {
                        ConfigureSearchPartySize(evt, defaults, spawnCountAdjustment);
                    }
                    
                    evt.cooldownDays = CalculateCooldownDays(defaults, isBoss);
                    evt.endgameOnly = !RaidConfig.AlwaysIncludeEndgameRaids.Value && defaults.EndgameOnly;
                    evt.forestOnly = !RaidConfig.AlwaysIncludeForestOnlyRaids.Value && defaults.ForestOnly;
                    evt.minMaxDay = CalculateMinMaxDay(defaults, isBoss);
                    evt.minMaxAnger = CalculateMinMaxAnger(defaults, isBoss);
                }
            }
            catch { }
        }
        
        /// <summary>
        /// Get or create defaults for an event (caches original values)
        /// </summary>
        public static SearchPartyEventDefaults GetOrCreateDefaults(VailWorldEventData.SearchPartyEvent evt)
        {
            int hashCode = evt.GetHashCode();
            
            if (SearchPartyDefaults.TryGetValue(hashCode, out var defaults))
                return defaults;
            
            defaults = new SearchPartyEventDefaults(
                evt.spawnCount,
                evt.spawnCountMax,
                evt.cooldownDays,
                evt.endgameOnly,
                evt.forestOnly,
                evt.minMaxDay,
                evt.minMaxAnger,
                evt.timeOfEvent
            );
            
            SearchPartyDefaults.Add(hashCode, defaults);
            return defaults;
        }
        
        /// <summary>
        /// Check if event should be disabled based on type settings.
        /// Uses evt.type directly (field read, IL2CPP-safe) instead of
        /// IsCannibal()/IsCreepy() methods which may throw on server DummyDll.
        /// TypeOfEvent: 0=Cannibal, 1=Creepy, 3=Muddy
        /// </summary>
        public static bool ShouldDisable(VailWorldEventData.EventBase evt)
        {
            try
            {
                int typeVal = (int)evt.type;
                
                return (typeVal == 0 && !RaidConfig.AllowCannibals.Value) ||
                       (typeVal == 1 && !RaidConfig.AllowCreepy.Value) ||
                       (typeVal == 3 && !RaidConfig.AllowMuddies.Value);
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Reset cached defaults
        /// </summary>
        public static void Reset()
        {
            SearchPartyDefaults.Clear();
        }
        
        private static Vector2 CalculateMinMaxAnger(SearchPartyEventDefaults defaults, bool isBossEvent)
        {
            bool ignore = isBossEvent ? RaidConfig.BossIgnoreMinMaxAnger.Value : RaidConfig.IgnoreMinMaxAnger.Value;
            if (ignore)
                return new Vector2(0f, float.MaxValue);
            return defaults.MinMaxAnger;
        }
        
        private static Vector2Int CalculateMinMaxDay(SearchPartyEventDefaults defaults, bool isBossEvent)
        {
            bool ignore = isBossEvent ? RaidConfig.BossIgnoreMinMaxDay.Value : RaidConfig.IgnoreMinMaxDay.Value;
            if (ignore)
                return new Vector2Int(0, int.MaxValue);
            return defaults.MinMaxDay;
        }
        
        private static int CalculateCooldownDays(SearchPartyEventDefaults defaults, bool isBossEvent)
        {
            int cooldown = isBossEvent ? RaidConfig.BossRaidsCooldown.Value : RaidConfig.NormalRaidsCooldown.Value;
            if (cooldown < 0)
                return defaults.CooldownDays;
            return cooldown;
        }
        
        private static void ConfigureBossPartySize(VailWorldEventData.SearchPartyEvent evt)
        {
            int playerCount = GetPlayerCount();
            int extraBosses = (playerCount - 1) * RaidConfig.ExtraBossesPerPlayer.Value;
            evt.spawnCount = RaidConfig.BossSpawnCount.Value + extraBosses;
            evt.spawnCountMax = evt.spawnCount;
        }
        
        private static int GetSpawnCountAdjustment()
        {
            if (!RaidConfig.AdjustOnPlayerCount.Value)
                return 0;
            return (GetPlayerCount() - 1) * RaidConfig.ExtraSpawnsPerPlayer.Value;
        }
        
        private static int GetPlayerCount()
        {
            try
            {
                // Try via reflection or use default
                return 1;
            }
            catch
            {
                return 1;
            }
        }
        
        private static void ConfigureSearchPartySize(VailWorldEventData.SearchPartyEvent evt, SearchPartyEventDefaults defaults, int spawnCountAdjustment)
        {
            evt.spawnCount = (int)Math.Round(defaults.SpawnCount * RaidConfig.SpawnCountFactor.Value) + spawnCountAdjustment;
            evt.spawnCountMax = (int)Math.Round(defaults.SpawnMax * RaidConfig.MaxSpawnCountFactor.Value) + spawnCountAdjustment;
            
            if (evt.spawnCount == 0)
                evt.spawnCount = 1;
            
            evt.spawnCount = Math.Min(evt.spawnCount, RaidConfig.EnemyLimit.Value);
            evt.spawnCountMax = Math.Min(evt.spawnCountMax, RaidConfig.EnemyLimit.Value);
            
            if (evt.spawnCountMax < evt.spawnCount)
                evt.spawnCountMax = evt.spawnCount;
        }
    }
}

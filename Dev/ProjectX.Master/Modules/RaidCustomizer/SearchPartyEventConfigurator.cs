using System;
using System.Collections.Generic;
using System.Reflection;
using Endnight.Utilities;
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
        /// Prepare all events for a new day with configured settings
        /// </summary>
        public static void PrepareForNewDay(VailWorldEventData vailWorldEventData)
        {
            try
            {
                int spawnCountAdjustment = GetSpawnCountAdjustment();
                
                // Collect all search party events
                var allEvents = new HashSet<VailWorldEventData.SearchPartyEvent>(vailWorldEventData._searchPartyEvents.ToArray());
                foreach (var evt in vailWorldEventData._creepyAttackEvents)
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
        /// Check if event should be disabled based on type settings
        /// </summary>
        public static bool ShouldDisable(VailWorldEventData.EventBase evt)
        {
            try
            {
                bool isCanibal = evt.IsCannibal();
                bool isCreepy = evt.IsCreepy();
                int typeVal = (int)evt.type;
                
                return (isCanibal && !RaidConfig.AllowCannibals.Value) ||
                       (isCreepy && !RaidConfig.AllowCreepy.Value) ||
                       (typeVal == 3 && !RaidConfig.AllowMuddies.Value);  // Muddy = 3
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

using RedLoader;
using RedLoader.Preferences;

namespace ProjectX.Master.Modules.RaidCustomizer
{
    /// <summary>
    /// Proxy class for Raid configuration
    /// All actual config entries are now in Config.cs
    /// This class provides backwards compatibility for RaidCustomizerModule
    /// </summary>
    public static class RaidConfig
    {
        // ===== GENERIC SETTINGS (proxy to Config) =====
        public static ConfigEntry<bool> AllowCannibals => Config.XR_AllowCannibals;
        public static ConfigEntry<bool> AllowCreepy => Config.XR_AllowCreepy;
        public static ConfigEntry<bool> AllowMuddies => Config.XR_AllowMuddies;
        public static ConfigEntry<bool> AnnounceIncomingSearchParties => Config.XR_AnnounceRaids;
        public static ConfigEntry<bool> PlaySoundWhenAnnounced => Config.XR_PlaySoundWhenAnnounced;
        /// <summary>Always include endgame raids regardless of progression</summary>
        public static ConfigEntry<bool> AlwaysIncludeEndgameRaids => Config.XR_IncludeEndgameRaids;
        /// <summary>Always include forest-only raids regardless of location</summary>
        public static ConfigEntry<bool> AlwaysIncludeForestOnlyRaids => Config.XR_IncludeForestOnlyRaids;
        /// <summary>Number of raid events per game day</summary>
        public static ConfigEntry<int> RaidsPerDay => Config.XR_RaidsPerDay;
        /// <summary>Raid distribution pattern (e.g. "random", "spread")</summary>
        public static ConfigEntry<string> RaidDistribution => Config.XR_RaidDistribution;
        /// <summary>Whether raids should consider current time-of-day when scheduling</summary>
        public static ConfigEntry<bool> ConsiderCurrentTime => Config.XR_ConsiderCurrentTime;
        
        // ===== RAID TIMES (proxy to Config) =====
        public static ConfigEntry<bool> SearchPartiesAtMorning => Config.XR_RaidAtMorning;
        public static ConfigEntry<bool> SearchPartiesAtDay => Config.XR_RaidAtDay;
        public static ConfigEntry<bool> SearchPartiesAtEvening => Config.XR_RaidAtEvening;
        public static ConfigEntry<bool> SearchPartiesAtNight => Config.XR_RaidAtNight;
        
        // ===== NORMAL RAIDS (proxy to Config) =====
        public static ConfigEntry<float> SpawnCountFactor => Config.XR_MinSpawnFactor;
        public static ConfigEntry<float> MaxSpawnCountFactor => Config.XR_MaxSpawnFactor;
        public static ConfigEntry<int> EnemyLimit => Config.XR_EnemyLimit;
        public static ConfigEntry<int> NormalRaidsCooldown => Config.XR_NormalCooldown;
        public static ConfigEntry<bool> IgnoreMinMaxDay => Config.XR_IgnoreDayLimit;
        /// <summary>Ignore minimum/maximum anger level requirements for raids</summary>
        public static ConfigEntry<bool> IgnoreMinMaxAnger => Config.XR_IgnoreAngerLimit;
        
        // ===== BOSS RAIDS (proxy to Config) =====
        public static ConfigEntry<int> BossSpawnCount => Config.XR_BossCount;
        public static ConfigEntry<int> BossRaidsCooldown => Config.XR_BossCooldown;
        public static ConfigEntry<bool> BossIgnoreMinMaxDay => Config.XR_BossIgnoreDayLimit;
        public static ConfigEntry<bool> BossIgnoreMinMaxAnger => Config.XR_BossIgnoreAngerLimit;
        
        // ===== ENEMY STATS (proxy to Config) =====
        public static ConfigEntry<bool> StatMultiplierModificationEnabled => Config.XR_StatMultiplierEnabled;
        public static ConfigEntry<bool> OverrideHealthOnLoad => Config.XR_OverrideHealthOnLoad;
        public static ConfigEntry<float> CannibalHealthMultiplier => Config.XR_CannibalHealth;
        public static ConfigEntry<float> CannibalDamageMultiplier => Config.XR_CannibalDamage;
        public static ConfigEntry<float> CannibalAggressionMultiplier => Config.XR_CannibalAggression;
        /// <summary>Creepy mutant health multiplier</summary>
        public static ConfigEntry<float> CreepHealthMultiplier => Config.XR_CreepHealth;
        /// <summary>Creepy mutant damage multiplier</summary>
        public static ConfigEntry<float> CreepDamageMultiplier => Config.XR_CreepDamage;
        /// <summary>Creepy mutant aggression multiplier</summary>
        public static ConfigEntry<float> CreepAggressionMultiplier => Config.XR_CreepAggression;
        /// <summary>Boss health multiplier</summary>
        public static ConfigEntry<float> BossHealthMultiplier => Config.XR_BossHealth;
        /// <summary>Boss damage multiplier</summary>
        public static ConfigEntry<float> BossDamageMultiplier => Config.XR_BossDamage;
        /// <summary>Boss aggression multiplier</summary>
        public static ConfigEntry<float> BossAggressionMultiplier => Config.XR_BossAggression;
        
        // ===== FOLLOWER HEALTH (proxy to Config) =====
        public static ConfigEntry<float> KelvinHealthMultiplier => Config.XR_KelvinHealth;
        public static ConfigEntry<float> VirginiaHealthMultiplier => Config.XR_VirginiaHealth;
        
        // ===== MULTIPLAYER (proxy to Config) =====
        public static ConfigEntry<bool> AdjustOnPlayerCount => Config.XR_AdjustByPlayerCount;
        public static ConfigEntry<int> ExtraSpawnsPerPlayer => Config.XR_ExtraSpawnsPerPlayer;
        public static ConfigEntry<int> ExtraRaidsPerPlayer => Config.XR_ExtraRaidsPerPlayer;
        public static ConfigEntry<int> ExtraBossesPerPlayer => Config.XR_ExtraBossesPerPlayer;

        /// <summary>
        /// Init is now a no-op - all config is initialized in Config.cs
        /// Kept for API compatibility
        /// </summary>
        public static void Init()
        {
            // No-op: All config entries are now initialized in Config.cs
            // This method is kept for backwards compatibility
        }
        
        /// <summary>
        /// Check if any raid time ranges are selected
        /// </summary>
        public static bool HasAnyEventRangesSelected()
        {
            return SearchPartiesAtMorning.Value || 
                   SearchPartiesAtDay.Value || 
                   SearchPartiesAtEvening.Value || 
                   SearchPartiesAtNight.Value;
        }
        
        /// <summary>
        /// Check if custom events per day is set
        /// </summary>
        public static bool HasCustomEventsPerDay(int playerCount, out int eventsPerDay)
        {
            eventsPerDay = RaidsPerDay.Value;
            
            if (AdjustOnPlayerCount.Value && eventsPerDay > 0 && playerCount > 1)
            {
                eventsPerDay += (playerCount - 1) * ExtraRaidsPerPlayer.Value;
            }
            
            return eventsPerDay >= 0;
        }
    }
}

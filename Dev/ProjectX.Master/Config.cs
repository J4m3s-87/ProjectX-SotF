using RedLoader;
using RedLoader.Utils;
using SUI; // For UI interactions if needed
using System;
using SonsSdk;
using SonsSdk.Attributes;

namespace ProjectX.Master
{
    [SettingsUiMode(0)]  // 0 = UseGameSettings - Required for native UI visibility
    public static class Config
    {
        // ======================== CATEGORIES ========================
        // Multi-category segmentation to fix native UI visibility bug
        private static ConfigCategory CheatsCategory { get; set; }
        private static ConfigCategory MovementCategory { get; set; }
        private static ConfigCategory WorldCategory { get; set; }
        private static ConfigCategory ModulesCategory { get; set; }
        private static ConfigCategory ZiplineCategory { get; set; }
        private static ConfigCategory FeaturesCategory { get; set; }
        private static ConfigCategory DiscordCategory { get; set; }
        private static ConfigCategory ScaryCrossCategory { get; set; }
        private static ConfigCategory KeysCategory { get; set; }
        private static ConfigCategory StacksCraftingCategory { get; set; }
        private static ConfigCategory StacksFoodCategory { get; set; }
        private static ConfigCategory StacksCombatCategory { get; set; }
        private static ConfigCategory StacksResourcesCategory { get; set; }
        private static ConfigCategory BuilderStacksCategory { get; set; }
        private static ConfigCategory AmmoUiCategory { get; set; }
        private static ConfigCategory HotbarCategory { get; set; }
        
        // X Raids Categories (merged from RaidConfig)
        private static ConfigCategory XRaidsGeneralCategory { get; set; }
        private static ConfigCategory XRaidsTimesCategory { get; set; }
        private static ConfigCategory XRaidsNormalCategory { get; set; }
        private static ConfigCategory XRaidsBossCategory { get; set; }
        private static ConfigCategory XRaidsEnemyStatsCategory { get; set; }
        private static ConfigCategory XRaidsFollowersCategory { get; set; }
        private static ConfigCategory XRaidsMultiplayerCategory { get; set; }
        
        // Weapon Damage Categories
        private static ConfigCategory WeaponDmgRangedCategory { get; set; }
        private static ConfigCategory WeaponDmgMeleeCategory { get; set; }
        private static ConfigCategory WeaponDmgFeaturesCategory { get; set; }
        
        // Loot Categories
        private static ConfigCategory LootCategoriesCategory { get; set; }
        
        // ======================== PLAYER CHEATS ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> IsGodMode { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> IsInfStamina { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> IsNoHungry { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> IsNoDehydration { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> IsNoSleep { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> IsInfiniteAmmo { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> IsNoFallDamage { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> InfiniteLogs { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> InfiniteStones { get; private set; }
        
        // ======================== INVENTORY / STACKS ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> InfiniteInventory { get; private set; }
        
        // --- Crafting Items ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxTapeStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxClothStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxWireStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxBatteriesStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxRopeStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxBoardStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxC4Stack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxVodkaStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxWatchStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxCoinStack { get; private set; }
        
        // --- Sticks & Rocks ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxStickStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxRockStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxSmallRockStack { get; private set; }
        
        // --- Printing Items ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxResinStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxMeshStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxHookStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxGpsCaseStack { get; private set; }
        
        // --- Electricity ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxSolarStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxBatteryStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxBulbStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxRadioStack { get; private set; }
        
        // --- Medication ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxMedsStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxHealthMixStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxHealthMixPlusStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxEnergyMixStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxEnergyMixPlusStack { get; private set; }
        
        // --- Food & Drinks ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxPotStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxMeatStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxFishStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxMreStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxEnergyBarStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxEnergyDrinkStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxCannedFoodStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxCatFoodStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxRamenStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxCrunchieStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxOysterStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxEggStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxSteakBiteStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxBaconBiteStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxBrainBiteStack { get; private set; }
        
        // --- Animal Drops ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxShellStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxHideStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxFeatherStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxAnimalHeadStack { get; private set; }
        
        // --- Throwables ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxMolotovStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxGrenadeStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxBombStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxSpearStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxFlareStack { get; private set; }
        
        // --- Body Parts & Bones ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxHeadStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxArmStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxLegStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxSkullStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxBoneStack { get; private set; }
        
        // --- Armor ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxCreepyArmorStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxBoneArmorStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxSolafiteArmorStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxTechArmorStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxLeafArmorStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxHideArmorStack { get; private set; }
        
        // --- Ammo ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxAmmoStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxStoneArrowStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxPrintedArrowStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxCarbonArrowStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxBoltStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxZiplineStack { get; private set; }
        
        // --- Plants & Seeds ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxLeafStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxPlantStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxSeedStack { get; private set; }
        
        // --- Misc ---
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxSolafiteStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxPouchStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxGliderStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxTarpStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxAirTankStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxPaperTargetStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxGolfBallStack { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> MaxCashStack { get; private set; }
        
        // ======================== MOVEMENT ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> IsNoClip { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> NoClipSpeed { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> NoClipUpDownSpeed { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WalkSpeed { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> RunSpeed { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SwimSpeed { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> JumpMultiplier { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> IsNoGravity { get; private set; }
        
        // ======================== WORLD / ENVIRONMENT ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> FreezeAI { get; private set; }
        public static ConfigEntry<bool> InstantBookBuild { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> IsLockTime { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> DaytimeSpeed { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> TreeRegrowRate { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WindIntensity { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> KillRadius { get; private set; }
        
        // ======================== KEYBINDS ========================
        [SettingsUiInclude]  // Visible on all builds — local keybind
        public static KeybindConfigEntry OpenKey { get; private set; }
        
        // ======================== MODULES ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> RelocatorEnabled { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> StructureDurabilityMultiplier { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> FasterCraftingEnabled { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> CraftingSpeedMultiplier { get; private set; }
        
        // ======================== ZIPLINE ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> MaxZipLineLength { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> MaxRopeBridgeLength { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> MaxShootingDistance { get; private set; }
        
        // ======================== REALISM FEATURES ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WaterCollectorHeatRadius { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> MeatDryerProximityCheckInterval { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> MeatDryerSpeedMultiplier { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> MeatDryerCureTimeDays { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> MeatDryerInstantDry { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> MeatDryerNoFireRequired { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> MeatDryerFireRadius { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> FlashlightIntensity { get; private set; }
        
        // ======================== AUDIO CONTROL ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WaterfallVolume { get; private set; }
        
        // ======================== LOOT RESPAWN ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LootRespawnEnabled { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<int> LootRespawnDays { get; private set; }

        // ======================== LOOT CATEGORIES ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LR_TrackMelee { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LR_TrackRanged { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LR_TrackWeaponMods { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LR_TrackMaterials { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LR_TrackFood { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LR_TrackMeds { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LR_TrackPlants { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LR_TrackAmmo { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LR_TrackThrowables { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LR_TrackExpendables { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LR_TrackBreakables { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> LR_TrackOpenables { get; private set; }
        
        // ======================== DISCORD BRIDGE ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> EnableDiscordBridge { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> DiscordBotToken { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> DiscordChannelId { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> DiscordWelcomeChannelId { get; private set; }
        
        // ======================== WELCOME MESSAGES ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> EnableWelcomeMessage { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WelcomeMessageDelay { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> WelcomeMessageText { get; private set; }

        // ======================== SCARYCROSS (Advanced) ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_TempRiseThreshold { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_BurnThreshold { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_DamageThreshold { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_DamageAfterSec { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_LightIntensityRise { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_LightIntensityReduce { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_TempRisePerSec { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_TempReducePerSec { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_EffigyMinRange { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_EffigyMaxRange { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_EffigyMinStrength { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_EffigyMaxStrength { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_EffigyDisabledRange { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_EffigyDisabledStrength { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_BurnDemonRangeMin { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_BurnDemonRangeMax { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_BurnDemonTimeConfig { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_DemonDetectRadiusMin { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> SC_DemonDetectRadiusMax { get; private set; }

        // ======================== X RAIDS - GENERAL ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_AllowCannibals { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_AllowCreepy { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_AllowMuddies { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_AnnounceRaids { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_PlaySoundWhenAnnounced { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_IncludeEndgameRaids { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_IncludeForestOnlyRaids { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<int> XR_RaidsPerDay { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> XR_RaidDistribution { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_ConsiderCurrentTime { get; private set; }

        // ======================== X RAIDS - TIMES ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_RaidAtMorning { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_RaidAtDay { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_RaidAtEvening { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_RaidAtNight { get; private set; }

        // ======================== X RAIDS - NORMAL ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_MinSpawnFactor { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_MaxSpawnFactor { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<int> XR_EnemyLimit { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<int> XR_NormalCooldown { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_IgnoreDayLimit { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_IgnoreAngerLimit { get; private set; }

        // ======================== X RAIDS - BOSS ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<int> XR_BossCount { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<int> XR_BossCooldown { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_BossIgnoreDayLimit { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_BossIgnoreAngerLimit { get; private set; }

        // ======================== X RAIDS - ENEMY STATS ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_StatMultiplierEnabled { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_OverrideHealthOnLoad { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_CannibalHealth { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_CannibalDamage { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_CannibalAggression { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_CreepHealth { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_CreepDamage { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_CreepAggression { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_BossHealth { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_BossDamage { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_BossAggression { get; private set; }

        // ======================== X RAIDS - FOLLOWERS ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_KelvinHealth { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> XR_VirginiaHealth { get; private set; }

        // ======================== X RAIDS - MULTIPLAYER ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> XR_AdjustByPlayerCount { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<int> XR_ExtraSpawnsPerPlayer { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<int> XR_ExtraRaidsPerPlayer { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<int> XR_ExtraBossesPerPlayer { get; private set; }

        // Flag for RaidCustomizer module
        public static bool MustRequeueRaids { get; set; }

        // ======================== BUILDER STACKS ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> BuilderStacksEnabled { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> BuilderStacksMaxLogCapacity { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> BuilderStacksMaxPlankCapacity { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<string> BuilderStacksMaxStoneCapacity { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> BuilderStacksGiveDelay { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> BuilderStacksEnableMaxLimit { get; private set; }

        // ======================== AMMO UI ========================
        [SettingsUiInclude]  // Visible on all builds — local HUD preference
        public static ConfigEntry<float> AmmoUiSize { get; private set; }
        [SettingsUiInclude]  // Visible on all builds — local HUD preference
        public static ConfigEntry<float> AmmoUiOpacity { get; private set; }

        // ======================== HOTBAR ========================
        [SettingsUiInclude]  // Visible on all builds — local UI toggle
        public static ConfigEntry<bool> HotbarEnabled { get; private set; }

        // ======================== WEAPON DAMAGE (RANGED) ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_PistolDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_RevolverDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_ShotgunDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_BuckshotDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_BuckshotSpread { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_RifleDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_CompoundBowDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_CraftedBowDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_CrossbowDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_SlingshotDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_StunGunDmg { get; private set; }
        
        // ======================== WEAPON DAMAGE (EXPLOSIVES & AMMO) ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_GrenadeDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_MolotovDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_StickyBombDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_C4Dmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_StoneArrowDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_PrintedArrowDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_CarbonArrowDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_FireArrowDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_ShockArrowDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_ExplosiveArrowDmg { get; private set; }
        
        // ======================== WEAPON DAMAGE (MELEE) ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_KatanaDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_MacheteDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_ModernAxeDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_FireAxeDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_TacticalAxeDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_CraftedSpearDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_StunBatonDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_CraftedClubDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_GuitarDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_ChainsawDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_GolfPutterDmg { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_KnifeDmg { get; private set; }
        
        // ======================== WEAPON FEATURES ========================
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<bool> WD_Enabled { get; private set; }
#if !CLIENT
        [SettingsUiInclude]
#endif
        public static ConfigEntry<float> WD_SolafiteBonus { get; private set; }
        [SettingsUiInclude]  // Visible on all builds — local keybind
        public static KeybindConfigEntry WD_InspectKey { get; private set; }
        [SettingsUiInclude]  // Visible on all builds — local preference
        public static ConfigEntry<bool> WD_InspectLeftHand { get; private set; }

        public static void Init()
        {

            // Config file - same name for backwards compatibility
            const string configFile = "ProjectX.Master.cfg";
            
            // ===== CATEGORY: CHEATS =====
            CheatsCategory = ConfigSystem.CreateFileCategory("ProjectX - Cheats", "ProjectX - Cheats", configFile);
            
            IsGodMode = CheatsCategory.CreateEntry<bool>("GodMode", false, "GodMode", "Invulnerable to damage");
            IsInfStamina = CheatsCategory.CreateEntry<bool>("IsInfStamina", false, "Infinite Stamina", "No energy loss");
            IsNoHungry = CheatsCategory.CreateEntry<bool>("IsNoHungry", false, "No Hunger", "Fullness lock");
            IsNoDehydration = CheatsCategory.CreateEntry<bool>("IsNoDehydration", false, "No Dehydration", "Hydration lock");
            IsNoSleep = CheatsCategory.CreateEntry<bool>("IsNoSleep", false, "No Sleep", "Rested lock");
            IsInfiniteAmmo = CheatsCategory.CreateEntry<bool>("IsInfiniteAmmo", false, "Infinite Ammo", "Never reload");
            IsNoFallDamage = CheatsCategory.CreateEntry<bool>("IsNoFallDamage", false, "No Fall Damage", "No fall damage");
            InfiniteLogs = CheatsCategory.CreateEntry<bool>("InfiniteLogs", false, "Infinite Logs", "Never run out of logs");
            InfiniteStones = CheatsCategory.CreateEntry<bool>("InfiniteStones", false, "Infinite Stones", "Never run out of stones");
            InfiniteInventory = CheatsCategory.CreateEntry<bool>("InfiniteInventory", false, "Infinite Stacks", "Sets ALL item stack caps to 999999999");
            
            // ===== CATEGORY: MOVEMENT =====
            MovementCategory = ConfigSystem.CreateFileCategory("ProjectX - Movement", "ProjectX - Movement", configFile);
            
            IsNoClip = MovementCategory.CreateEntry<bool>("IsNoClip", false, "NoClip", "Fly mode");
            NoClipSpeed = MovementCategory.CreateEntry<float>("NoClipSpeed", 2.5f, "NoClip Speed", "Flight speed");
            NoClipSpeed.SetRange(0.5f, 20f);
            NoClipUpDownSpeed = MovementCategory.CreateEntry<float>("NoClipUpDownSpeed", 0.5f, "NoClip Vertical Speed", "Flight vertical speed");
            NoClipUpDownSpeed.SetRange(0.1f, 10f);
            WalkSpeed = MovementCategory.CreateEntry<float>("WalkSpeed", 1f, "Walk Speed", "Base walk speed");
            WalkSpeed.SetRange(0.1f, 10f);
            RunSpeed = MovementCategory.CreateEntry<float>("RunSpeed", 1f, "Run Speed", "Base run speed");
            RunSpeed.SetRange(0.1f, 10f);
            SwimSpeed = MovementCategory.CreateEntry<float>("SwimSpeed", 1f, "Swim Speed", "Base swim speed");
            SwimSpeed.SetRange(0.1f, 10f);
            JumpMultiplier = MovementCategory.CreateEntry<float>("JumpMultiplier", 1f, "Jump Multiplier", "Super jump");
            JumpMultiplier.SetRange(0.5f, 10f);
            IsNoGravity = MovementCategory.CreateEntry<bool>("IsNoGravity", false, "No Gravity", "Disable gravity - float when jumping");
            
            // ===== CATEGORY: WORLD =====
            WorldCategory = ConfigSystem.CreateFileCategory("ProjectX - World", "ProjectX - World", configFile);
            
            FreezeAI = WorldCategory.CreateEntry<bool>("FreezeAI", false, "Freeze AI", "Pause world simulation - stops all enemy/animal spawning and AI");
            InstantBookBuild = WorldCategory.CreateEntry<bool>("InstantBookBuild", false, "Instant Book Build", "Server-wide instant blueprint completion for all players");
            IsLockTime = WorldCategory.CreateEntry<bool>("IsLockTime", false, "Lock Time", "Freeze current time of day");
            DaytimeSpeed = WorldCategory.CreateEntry<float>("DaytimeSpeed", 1f, "Daytime Speed", "Speed of day/night cycle (1 = normal)");
            DaytimeSpeed.SetRange(0f, 10f);
            TreeRegrowRate = WorldCategory.CreateEntry<float>("TreeRegrowRate", 1f, "Tree Regrow Rate", "Multiplier for tree regrowth speed");
            TreeRegrowRate.SetRange(0f, 10f);
            WindIntensity = WorldCategory.CreateEntry<float>("WindIntensity", -1f, "Wind Intensity", "Wind strength (-1 = auto, 0+ = locked value)");
            WindIntensity.SetRange(-1f, 10f);
            KillRadius = WorldCategory.CreateEntry<float>("KillRadius", 0f, "Kill/Burn Radius", "Radius for Kill/Burn All (0 = all loaded, >0 = meters from player)");
            KillRadius.SetRange(0f, 500f);

            // ===== CATEGORY: MODULES =====
            ModulesCategory = ConfigSystem.CreateFileCategory("ProjectX - Modules", "ProjectX - Modules", configFile);
            
            RelocatorEnabled = ModulesCategory.CreateEntry<bool>("RelocatorEnabled", true, "Enable Relocator", "Always-on: Unlock 'C' to move for all structures");
            LootRespawnEnabled = ModulesCategory.CreateEntry<bool>("LootRespawnEnabled", true, "Enable Loot Respawn", "Respawn picked up items after X days");
            LootRespawnEnabled.OnValueChanged.Subscribe((_, newVal) => {
                Modules.LootRespawn.LootRespawnModule.Enabled = newVal;
            });
            Modules.LootRespawn.LootRespawnModule.Enabled = LootRespawnEnabled.Value;
            LootRespawnDays = ModulesCategory.CreateEntry<int>("LootRespawnDays", 3, "Loot Respawn Days", "Days until items respawn (1-100)");
            LootRespawnDays.SetRange(1, 100);
            LootRespawnDays.OnValueChanged.Subscribe((_, newVal) => {
                Modules.LootRespawn.RespawnConfig.RespawnDays = newVal;
            });
            Modules.LootRespawn.RespawnConfig.RespawnDays = LootRespawnDays.Value;

            // ===== CATEGORY: LOOT CATEGORIES =====
            LootCategoriesCategory = ConfigSystem.CreateFileCategory("ProjectX - Loot Categories", "ProjectX - Loot Categories", configFile);

            LR_TrackMelee = LootCategoriesCategory.CreateEntry<bool>("LR_TrackMelee", true, "Track Melee Weapons", "Track melee weapons for respawn (Modern Axe, Katana, etc.)");
            LR_TrackMelee.OnValueChanged.Subscribe((_, v) => { Modules.LootRespawn.RespawnConfig.TrackMelee = v; });
            Modules.LootRespawn.RespawnConfig.TrackMelee = LR_TrackMelee.Value;

            LR_TrackRanged = LootCategoriesCategory.CreateEntry<bool>("LR_TrackRanged", true, "Track Ranged Weapons", "Track ranged weapons for respawn (Pistol, Shotgun, etc.)");
            LR_TrackRanged.OnValueChanged.Subscribe((_, v) => { Modules.LootRespawn.RespawnConfig.TrackRanged = v; });
            Modules.LootRespawn.RespawnConfig.TrackRanged = LR_TrackRanged.Value;

            LR_TrackWeaponMods = LootCategoriesCategory.CreateEntry<bool>("LR_TrackWeaponMods", true, "Track Weapon Mods", "Track weapon mods for respawn (Silencer, Laser Sight, etc.)");
            LR_TrackWeaponMods.OnValueChanged.Subscribe((_, v) => { Modules.LootRespawn.RespawnConfig.TrackWeaponMods = v; });
            Modules.LootRespawn.RespawnConfig.TrackWeaponMods = LR_TrackWeaponMods.Value;

            LR_TrackMaterials = LootCategoriesCategory.CreateEntry<bool>("LR_TrackMaterials", true, "Track Materials", "Track crafting materials for respawn (Rope, Duct Tape, Coins, etc.)");
            LR_TrackMaterials.OnValueChanged.Subscribe((_, v) => { Modules.LootRespawn.RespawnConfig.TrackMaterials = v; });
            Modules.LootRespawn.RespawnConfig.TrackMaterials = LR_TrackMaterials.Value;

            LR_TrackFood = LootCategoriesCategory.CreateEntry<bool>("LR_TrackFood", true, "Track Food", "Track food items for respawn (MREs, Canned Food, etc.)");
            LR_TrackFood.OnValueChanged.Subscribe((_, v) => { Modules.LootRespawn.RespawnConfig.TrackFood = v; });
            Modules.LootRespawn.RespawnConfig.TrackFood = LR_TrackFood.Value;

            LR_TrackMeds = LootCategoriesCategory.CreateEntry<bool>("LR_TrackMeds", true, "Track Medicine & Energy", "Track meds and energy drinks for respawn");
            LR_TrackMeds.OnValueChanged.Subscribe((_, v) => { Modules.LootRespawn.RespawnConfig.TrackMeds = v; });
            Modules.LootRespawn.RespawnConfig.TrackMeds = LR_TrackMeds.Value;

            LR_TrackPlants = LootCategoriesCategory.CreateEntry<bool>("LR_TrackPlants", true, "Track Plants", "Track plants for respawn (Aloe Vera, Mushrooms, etc.)");
            LR_TrackPlants.OnValueChanged.Subscribe((_, v) => { Modules.LootRespawn.RespawnConfig.TrackPlants = v; });
            Modules.LootRespawn.RespawnConfig.TrackPlants = LR_TrackPlants.Value;

            LR_TrackAmmo = LootCategoriesCategory.CreateEntry<bool>("LR_TrackAmmo", true, "Track Ammunition", "Track ammo for respawn (Bullets, Arrows, Bolts, etc.)");
            LR_TrackAmmo.OnValueChanged.Subscribe((_, v) => { Modules.LootRespawn.RespawnConfig.TrackAmmo = v; });
            Modules.LootRespawn.RespawnConfig.TrackAmmo = LR_TrackAmmo.Value;

            LR_TrackThrowables = LootCategoriesCategory.CreateEntry<bool>("LR_TrackThrowables", true, "Track Throwables", "Track throwables for respawn (Grenades, Sticky Bombs, etc.)");
            LR_TrackThrowables.OnValueChanged.Subscribe((_, v) => { Modules.LootRespawn.RespawnConfig.TrackThrowables = v; });
            Modules.LootRespawn.RespawnConfig.TrackThrowables = LR_TrackThrowables.Value;

            LR_TrackExpendables = LootCategoriesCategory.CreateEntry<bool>("LR_TrackExpendables", true, "Track Expendables", "Track expendables for respawn (Printer Resin, Air Tanks, etc.)");
            LR_TrackExpendables.OnValueChanged.Subscribe((_, v) => { Modules.LootRespawn.RespawnConfig.TrackExpendables = v; });
            Modules.LootRespawn.RespawnConfig.TrackExpendables = LR_TrackExpendables.Value;

            LR_TrackBreakables = LootCategoriesCategory.CreateEntry<bool>("LR_TrackBreakables", true, "Track Breakable Containers", "Track breakable containers for respawn (Wooden Crates, Coffins)");
            LR_TrackBreakables.OnValueChanged.Subscribe((_, v) => { Modules.LootRespawn.RespawnConfig.TrackBreakables = v; });
            Modules.LootRespawn.RespawnConfig.TrackBreakables = LR_TrackBreakables.Value;

            LR_TrackOpenables = LootCategoriesCategory.CreateEntry<bool>("LR_TrackOpenables", true, "Track Openable Containers", "Track openable containers for respawn (Ammo Cases, Pelican Cases, Suitcases)");
            LR_TrackOpenables.OnValueChanged.Subscribe((_, v) => { Modules.LootRespawn.RespawnConfig.TrackOpenables = v; });
            Modules.LootRespawn.RespawnConfig.TrackOpenables = LR_TrackOpenables.Value;
            
            // Crafting Speed — entries on all builds for ConfigSync, module wiring client-only
            FasterCraftingEnabled = ModulesCategory.CreateEntry<bool>("FasterCraftingEnabled", true, "Faster Crafting", "Speed up backpack crafting");
            CraftingSpeedMultiplier = ModulesCategory.CreateEntry<float>("CraftingSpeedMultiplier", 5f, "Crafting Speed Multiplier", "Speed multiplier (1 = normal, 10 = instant)");
            CraftingSpeedMultiplier.SetRange(1f, 10f);
#if !SERVER
            FasterCraftingEnabled.OnValueChanged.Subscribe((_, newVal) => {
                Modules.Crafting.CraftingSpeed.Enabled = newVal;
            });
            Modules.Crafting.CraftingSpeed.Enabled = FasterCraftingEnabled.Value;
            CraftingSpeedMultiplier.OnValueChanged.Subscribe((_, newVal) => {
                Modules.Crafting.CraftingSpeed.SpeedMultiplier = newVal;
            });
            Modules.Crafting.CraftingSpeed.SpeedMultiplier = CraftingSpeedMultiplier.Value;
#endif
            
            StructureDurabilityMultiplier = ModulesCategory.CreateEntry<float>("StructureDurabilityMultiplier", 1f, "Structure Durability", "Health Multiplier (100 = Invincible-ish)");
            StructureDurabilityMultiplier.SetRange(0.1f, 100f);
            
            // ===== CATEGORY: BUILDER STACKS =====
            BuilderStacksCategory = ConfigSystem.CreateFileCategory("ProjectX - Builder Stacks", "ProjectX - Builder Stacks", configFile);
            
            BuilderStacksEnabled = BuilderStacksCategory.CreateEntry<bool>("BuilderStacksEnabled", true, "Enable Builder Stacks", "Carry extra logs, planks, and stones beyond vanilla limit");
            BuilderStacksEnabled.OnValueChanged.Subscribe((_, newVal) => {
#if !SERVER
                Modules.BuilderStacks.BuilderStacksModule.Enabled = newVal;
#endif
            });
#if !SERVER
            Modules.BuilderStacks.BuilderStacksModule.Enabled = BuilderStacksEnabled.Value;
#endif
            

            BuilderStacksMaxLogCapacity = BuilderStacksCategory.CreateEntry<string>("BuilderStacksMaxLogCapacity", "2", "Max Log Capacity", "Maximum logs you can carry at once (vanilla: 2)");
            BuilderStacksMaxLogCapacity.OnValueChanged.Subscribe((_, newVal) => {
#if !SERVER
                if (int.TryParse(newVal, out int v) && v >= 1) {
                    Modules.BuilderStacks.BuilderStacksModule.MaxLogCapacity = v;
                    RedLoader.RLog.Msg($"[BuilderStacks] Config changed: MaxLogCapacity = {v}");
                }
#endif
            });
#if !SERVER
            if (int.TryParse(BuilderStacksMaxLogCapacity.Value, out int logCap) && logCap >= 1)
                Modules.BuilderStacks.BuilderStacksModule.MaxLogCapacity = logCap;
#endif
            
            BuilderStacksMaxPlankCapacity = BuilderStacksCategory.CreateEntry<string>("BuilderStacksMaxPlankCapacity", "4", "Max Plank Capacity", "Maximum planks you can carry at once (vanilla: 4)");
            BuilderStacksMaxPlankCapacity.OnValueChanged.Subscribe((_, newVal) => {
#if !SERVER
                if (int.TryParse(newVal, out int v) && v >= 1) {
                    Modules.BuilderStacks.BuilderStacksModule.MaxPlankCapacity = v;
                    RedLoader.RLog.Msg($"[BuilderStacks] Config changed: MaxPlankCapacity = {v}");
                }
#endif
            });
#if !SERVER
            if (int.TryParse(BuilderStacksMaxPlankCapacity.Value, out int plankCap) && plankCap >= 1)
                Modules.BuilderStacks.BuilderStacksModule.MaxPlankCapacity = plankCap;
#endif
            
            BuilderStacksMaxStoneCapacity = BuilderStacksCategory.CreateEntry<string>("BuilderStacksMaxStoneCapacity", "4", "Max Stone Capacity", "Maximum stones you can carry at once (vanilla: 4)");
            BuilderStacksMaxStoneCapacity.OnValueChanged.Subscribe((_, newVal) => {
#if !SERVER
                if (int.TryParse(newVal, out int v) && v >= 1) {
                    Modules.BuilderStacks.BuilderStacksModule.MaxStoneCapacity = v;
                    RedLoader.RLog.Msg($"[BuilderStacks] Config changed: MaxStoneCapacity = {v}");
                }
#endif
            });
#if !SERVER
            if (int.TryParse(BuilderStacksMaxStoneCapacity.Value, out int stoneCap) && stoneCap >= 1)
                Modules.BuilderStacks.BuilderStacksModule.MaxStoneCapacity = stoneCap;
#endif
            
            BuilderStacksEnableMaxLimit = BuilderStacksCategory.CreateEntry<bool>("BuilderStacksEnableMaxLimit", true, "Enable Max Limit", "If off, carry unlimited materials");
            BuilderStacksEnableMaxLimit.OnValueChanged.Subscribe((_, newVal) => {
#if !SERVER
                Modules.BuilderStacks.BuilderStacksModule.EnableMaxLimit = newVal;
#endif
            });
#if !SERVER
            Modules.BuilderStacks.BuilderStacksModule.EnableMaxLimit = BuilderStacksEnableMaxLimit.Value;
#endif
            
            BuilderStacksGiveDelay = BuilderStacksCategory.CreateEntry<float>("BuilderStacksGiveDelay", 0.2f, "Re-equip Delay", "Seconds before auto re-equipping from buffer (lower = faster)");
            BuilderStacksGiveDelay.SetRange(0.1f, 3f);
            BuilderStacksGiveDelay.OnValueChanged.Subscribe((_, newVal) => {
#if !SERVER
                Modules.BuilderStacks.BuilderStacksModule.GiveDelay = newVal;
#endif
            });
#if !SERVER
            Modules.BuilderStacks.BuilderStacksModule.GiveDelay = BuilderStacksGiveDelay.Value;
#endif
            
            // ===== CATEGORY: ZIPLINE =====
            ZiplineCategory = ConfigSystem.CreateFileCategory("ProjectX - Zipline", "ProjectX - Zipline", configFile);
            
            MaxZipLineLength = ZiplineCategory.CreateEntry<float>("MaxZipLineLength", float.MaxValue, "Max Zipline Length", "Maximum zipline rope length (game default ~150)");
            MaxZipLineLength.DefaultValue = 150f;
            MaxZipLineLength.SetRange(MaxZipLineLength.DefaultValue, 10000f);
            MaxShootingDistance = ZiplineCategory.CreateEntry<float>("MaxShootingDistance", float.MaxValue, "Max Shooting Distance", "Max rope gun targeting distance (game default ~50)");
            MaxShootingDistance.DefaultValue = 50f;
            MaxShootingDistance.SetRange(MaxShootingDistance.DefaultValue, 10000f);
            
            // ===== CATEGORY: FEATURES =====
            FeaturesCategory = ConfigSystem.CreateFileCategory("ProjectX - Features", "ProjectX - Features", configFile);
            
            WaterCollectorHeatRadius = FeaturesCategory.CreateEntry<float>("WaterCollectorHeatRadius", 2.5f, "Water Collector Heat Radius", "Radius to check for fire to melt ice");
            WaterCollectorHeatRadius.SetRange(0.5f, 10f);
            MeatDryerProximityCheckInterval = FeaturesCategory.CreateEntry<float>("MeatDryerProximityCheckInterval", 2.0f, "Meat Dryer Check Interval", "How often (seconds) to check for fire proximity");
            MeatDryerProximityCheckInterval.SetRange(0.5f, 10f);
            MeatDryerSpeedMultiplier = FeaturesCategory.CreateEntry<float>("MeatDryerSpeedMultiplier", 1.0f, "Meat Dryer Speed Multiplier", "Multiplier for drying speed near fires (1 = normal, 10 = 10x faster)");
            MeatDryerSpeedMultiplier.SetRange(1f, 50f);
            MeatDryerCureTimeDays = FeaturesCategory.CreateEntry<float>("MeatDryerCureTimeDays", -1f, "Meat Dryer Cure Time (Days)", "Override cure time in game days (-1 = use default, 0.1 = very fast)");
            MeatDryerCureTimeDays.SetRange(-1f, 10f);
            MeatDryerInstantDry = FeaturesCategory.CreateEntry<bool>("MeatDryerInstantDry", false, "Meat Dryer Instant Dry", "Instantly dry all meat placed on racks");
            MeatDryerNoFireRequired = FeaturesCategory.CreateEntry<bool>("MeatDryerNoFireRequired", false, "Meat Dryer No Fire Required", "Drying boost always active, no fire needed");
            MeatDryerFireRadius = FeaturesCategory.CreateEntry<float>("MeatDryerFireRadius", 10f, "Meat Dryer Fire Radius", "Distance (meters) to detect fire for drying boost");
            MeatDryerFireRadius.SetRange(3f, 30f);
            FlashlightIntensity = FeaturesCategory.CreateEntry<float>("FlashlightIntensity", 1.0f, "Flashlight Intensity", "Brightness multiplier");
            FlashlightIntensity.SetRange(0.1f, 5f);
            MaxRopeBridgeLength = FeaturesCategory.CreateEntry<float>("MaxRopeBridgeLength", float.MaxValue, "Max Rope Bridge Length (m)", "Maximum rope bridge length in meters (game default ~24m)");
            MaxRopeBridgeLength.DefaultValue = 24f;
            MaxRopeBridgeLength.SetRange(5f, 400f);
            
            // Audio Control
            WaterfallVolume = FeaturesCategory.CreateEntry<float>("WaterfallVolume", 1.0f, "Waterfall Volume", "Volume multiplier for waterfalls (0 = mute, 3 = loud)");
            WaterfallVolume.SetRange(0f, 3f);
#if !SERVER
            WaterfallVolume.OnValueChanged.Subscribe((oldVal, newVal) => {
                Modules.Audio.AudioControl.WaterfallVolume = newVal;
            });
#endif
            
            // Loot Respawn - moved to Modules category

            // ===== CATEGORY: DISCORD =====
            DiscordCategory = ConfigSystem.CreateFileCategory("ProjectX - Discord", "ProjectX - Discord", configFile);
            
            EnableDiscordBridge = DiscordCategory.CreateEntry<bool>("EnableDiscordBridge", false, "Enable Discord Bridge", "Broadcast chat to Discord");
            DiscordBotToken = DiscordCategory.CreateEntry<string>("DiscordBotToken", "", "Discord Bot Token", "Your Bot Token");
            DiscordChannelId = DiscordCategory.CreateEntry<string>("DiscordChannelId", "", "Discord Channel ID", "Channel ID to broadcast to");
            DiscordWelcomeChannelId = DiscordCategory.CreateEntry<string>("DiscordWelcomeChannelId", "", "Discord Welcome Channel ID", "Channel ID to fetch welcome/rules from on player join");
            
            // Welcome Messages
            EnableWelcomeMessage = DiscordCategory.CreateEntry<bool>("EnableWelcomeMessage", true, "Enable Welcome Message", "Send welcome to joining players in-game chat");
            WelcomeMessageDelay = DiscordCategory.CreateEntry<float>("WelcomeMessageDelay", 30.0f, "Welcome Message Delay", "Seconds to wait before sending welcome");
            WelcomeMessageDelay.SetRange(1f, 60f);
            WelcomeMessageText = DiscordCategory.CreateEntry<string>("WelcomeMessageText", "Visit https://discord.gg/GQDfkdUD for the Mod Pack and Server Information", "Welcome Message Text", "Custom text shown to players on join");

            // ===== CATEGORY: KEYBINDS =====
            KeysCategory = ConfigSystem.CreateFileCategory("ProjectX - Keys", "ProjectX - Keys", configFile);
            
            OpenKey = KeysCategory.CreateKeybindEntry("OpenKey", "insert", "Open Menu Key", "Key to open Project X menu");

            
            // ===== CATEGORY: STACKS - CRAFTING =====
            StacksCraftingCategory = ConfigSystem.CreateFileCategory("ProjectX - Stacks Crafting", "ProjectX - Stacks Crafting", configFile);
            
            MaxTapeStack = StacksCraftingCategory.CreateEntry<string>("MaxTapeStack", "100", "Max Duct Tape", "1-999999999");
            MaxClothStack = StacksCraftingCategory.CreateEntry<string>("MaxClothStack", "1000", "Max Cloth", "1-999999999");
            MaxWireStack = StacksCraftingCategory.CreateEntry<string>("MaxWireStack", "100", "Max Wire", "1-999999999");
            MaxBatteriesStack = StacksCraftingCategory.CreateEntry<string>("MaxBatteriesStack", "6", "Max Batteries", "1-999999999");
            MaxRopeStack = StacksCraftingCategory.CreateEntry<string>("MaxRopeStack", "8", "Max Ropes", "1-999999999");
            MaxBoardStack = StacksCraftingCategory.CreateEntry<string>("MaxBoardStack", "6", "Max Circuit Boards", "1-999999999");
            MaxC4Stack = StacksCraftingCategory.CreateEntry<string>("MaxC4Stack", "3", "Max C4 Bricks", "1-999999999");
            MaxVodkaStack = StacksCraftingCategory.CreateEntry<string>("MaxVodkaStack", "6", "Max Vodka", "1-999999999");
            MaxWatchStack = StacksCraftingCategory.CreateEntry<string>("MaxWatchStack", "10", "Max Watches", "1-999999999");
            MaxCoinStack = StacksCraftingCategory.CreateEntry<string>("MaxCoinStack", "1000", "Max Coins", "1-999999999");
            MaxResinStack = StacksCraftingCategory.CreateEntry<string>("MaxResinStack", "1000", "Max Resin", "1-999999999");
            MaxMeshStack = StacksCraftingCategory.CreateEntry<string>("MaxMeshStack", "10", "Max Tech Mesh", "1-999999999");
            MaxHookStack = StacksCraftingCategory.CreateEntry<string>("MaxHookStack", "2", "Max Grappling Hooks", "1-999999999");
            MaxGpsCaseStack = StacksCraftingCategory.CreateEntry<string>("MaxGpsCaseStack", "5", "Max GPS Cases", "1-999999999");
            MaxSolarStack = StacksCraftingCategory.CreateEntry<string>("MaxSolarStack", "3", "Max Solar Panels", "1-999999999");
            MaxBatteryStack = StacksCraftingCategory.CreateEntry<string>("MaxBatteryStack", "3", "Max Golfcart Batteries", "1-999999999");
            MaxBulbStack = StacksCraftingCategory.CreateEntry<string>("MaxBulbStack", "8", "Max Light Bulbs", "1-999999999");
            MaxRadioStack = StacksCraftingCategory.CreateEntry<string>("MaxRadioStack", "1", "Max Radios", "1-999999999");
            
            // ===== CATEGORY: STACKS - FOOD =====
            StacksFoodCategory = ConfigSystem.CreateFileCategory("ProjectX - Stacks Food", "ProjectX - Stacks Food", configFile);
            
            MaxMedsStack = StacksFoodCategory.CreateEntry<string>("MaxMedsStack", "6", "Max Meds", "1-999999999");
            MaxHealthMixStack = StacksFoodCategory.CreateEntry<string>("MaxHealthMixStack", "3", "Max Health Mix", "1-999999999");
            MaxHealthMixPlusStack = StacksFoodCategory.CreateEntry<string>("MaxHealthMixPlusStack", "3", "Max Health+ Mix", "1-999999999");
            MaxEnergyMixStack = StacksFoodCategory.CreateEntry<string>("MaxEnergyMixStack", "3", "Max Energy Mix", "1-999999999");
            MaxEnergyMixPlusStack = StacksFoodCategory.CreateEntry<string>("MaxEnergyMixPlusStack", "3", "Max Energy+ Mix", "1-999999999");
            MaxPotStack = StacksFoodCategory.CreateEntry<string>("MaxPotStack", "1", "Max Cooking Pots", "1-999999999");
            MaxMeatStack = StacksFoodCategory.CreateEntry<string>("MaxMeatStack", "7", "Max Meat", "1-999999999");
            MaxFishStack = StacksFoodCategory.CreateEntry<string>("MaxFishStack", "3", "Max Fish", "1-999999999");
            MaxMreStack = StacksFoodCategory.CreateEntry<string>("MaxMreStack", "6", "Max MREs", "1-999999999");
            MaxEnergyBarStack = StacksFoodCategory.CreateEntry<string>("MaxEnergyBarStack", "10", "Max Energy Bars", "1-999999999");
            MaxEnergyDrinkStack = StacksFoodCategory.CreateEntry<string>("MaxEnergyDrinkStack", "10", "Max Energy Drinks", "1-999999999");
            MaxCannedFoodStack = StacksFoodCategory.CreateEntry<string>("MaxCannedFoodStack", "6", "Max Canned Food", "1-999999999");
            MaxCatFoodStack = StacksFoodCategory.CreateEntry<string>("MaxCatFoodStack", "1", "Max Cat Food", "1-999999999");
            MaxRamenStack = StacksFoodCategory.CreateEntry<string>("MaxRamenStack", "4", "Max Ramen", "1-999999999");
            MaxCrunchieStack = StacksFoodCategory.CreateEntry<string>("MaxCrunchieStack", "4", "Max Crunchie Wunchies", "1-999999999");
            MaxOysterStack = StacksFoodCategory.CreateEntry<string>("MaxOysterStack", "5", "Max Oysters", "1-999999999");
            MaxEggStack = StacksFoodCategory.CreateEntry<string>("MaxEggStack", "4", "Max Turtle Eggs", "1-999999999");
            MaxSteakBiteStack = StacksFoodCategory.CreateEntry<string>("MaxSteakBiteStack", "1", "Max Steak Bites", "1-999999999");
            MaxBaconBiteStack = StacksFoodCategory.CreateEntry<string>("MaxBaconBiteStack", "1", "Max Bacon Bites", "1-999999999");
            MaxBrainBiteStack = StacksFoodCategory.CreateEntry<string>("MaxBrainBiteStack", "1", "Max Brain Bites", "1-999999999");

            // ===== CATEGORY: STACKS - COMBAT =====
            StacksCombatCategory = ConfigSystem.CreateFileCategory("ProjectX - Stacks Combat", "ProjectX - Stacks Combat", configFile);
            
            MaxMolotovStack = StacksCombatCategory.CreateEntry<string>("MaxMolotovStack", "6", "Max Molotovs", "1-999999999");
            MaxGrenadeStack = StacksCombatCategory.CreateEntry<string>("MaxGrenadeStack", "5", "Max Grenades", "1-999999999");
            MaxBombStack = StacksCombatCategory.CreateEntry<string>("MaxBombStack", "3", "Max Sticky Bombs", "1-999999999");
            MaxSpearStack = StacksCombatCategory.CreateEntry<string>("MaxSpearStack", "5", "Max Spears", "1-999999999");
            MaxFlareStack = StacksCombatCategory.CreateEntry<string>("MaxFlareStack", "10", "Max Flares", "1-999999999");
            MaxAmmoStack = StacksCombatCategory.CreateEntry<string>("MaxAmmoStack", "1000", "Max All Ammo", "All ammo types");
            MaxStoneArrowStack = StacksCombatCategory.CreateEntry<string>("MaxStoneArrowStack", "20", "Max Stone Arrows", "Requires menu restart");
            MaxPrintedArrowStack = StacksCombatCategory.CreateEntry<string>("MaxPrintedArrowStack", "20", "Max Printed Arrows", "Requires menu restart");
            MaxCarbonArrowStack = StacksCombatCategory.CreateEntry<string>("MaxCarbonArrowStack", "20", "Max Carbon Arrows", "Requires menu restart");
            MaxBoltStack = StacksCombatCategory.CreateEntry<string>("MaxBoltStack", "20", "Max Crossbow Bolts", "1-999999999");
            MaxZiplineStack = StacksCombatCategory.CreateEntry<string>("MaxZiplineStack", "4", "Max Ziplines", "1-999999999");
            MaxCreepyArmorStack = StacksCombatCategory.CreateEntry<string>("MaxCreepyArmorStack", "10", "Max Creepy Armor", "1-999999999");
            MaxBoneArmorStack = StacksCombatCategory.CreateEntry<string>("MaxBoneArmorStack", "10", "Max Bone Armor", "1-999999999");
            MaxSolafiteArmorStack = StacksCombatCategory.CreateEntry<string>("MaxSolafiteArmorStack", "10", "Max Solafite Armor", "1-999999999");
            MaxTechArmorStack = StacksCombatCategory.CreateEntry<string>("MaxTechArmorStack", "10", "Max Tech Armor", "1-999999999");
            MaxLeafArmorStack = StacksCombatCategory.CreateEntry<string>("MaxLeafArmorStack", "10", "Max Leaf Armor", "1-999999999");
            MaxHideArmorStack = StacksCombatCategory.CreateEntry<string>("MaxHideArmorStack", "10", "Max Hide Armor", "1-999999999");

            // ===== CATEGORY: STACKS - RESOURCES =====
            StacksResourcesCategory = ConfigSystem.CreateFileCategory("ProjectX - Stacks Resources", "ProjectX - Stacks Resources", configFile);
            
            MaxStickStack = StacksResourcesCategory.CreateEntry<string>("MaxStickStack", "20", "Max Sticks", "1-999999999");
            MaxRockStack = StacksResourcesCategory.CreateEntry<string>("MaxRockStack", "10", "Max Rocks", "1-999999999");
            MaxSmallRockStack = StacksResourcesCategory.CreateEntry<string>("MaxSmallRockStack", "30", "Max Small Rocks", "1-999999999");
            MaxShellStack = StacksResourcesCategory.CreateEntry<string>("MaxShellStack", "3", "Max Shells", "1-999999999");
            MaxHideStack = StacksResourcesCategory.CreateEntry<string>("MaxHideStack", "5", "Max Hides", "1-999999999");
            MaxFeatherStack = StacksResourcesCategory.CreateEntry<string>("MaxFeatherStack", "1000", "Max Feathers", "1-999999999");
            MaxAnimalHeadStack = StacksResourcesCategory.CreateEntry<string>("MaxAnimalHeadStack", "1", "Max Animal Heads", "1-999999999");
            MaxHeadStack = StacksResourcesCategory.CreateEntry<string>("MaxHeadStack", "1", "Max Heads", "1-999999999");
            MaxArmStack = StacksResourcesCategory.CreateEntry<string>("MaxArmStack", "2", "Max Arms", "1-999999999");
            MaxLegStack = StacksResourcesCategory.CreateEntry<string>("MaxLegStack", "2", "Max Legs", "1-999999999");
            MaxSkullStack = StacksResourcesCategory.CreateEntry<string>("MaxSkullStack", "3", "Max Skulls", "1-999999999");
            MaxBoneStack = StacksResourcesCategory.CreateEntry<string>("MaxBoneStack", "16", "Max Bones", "1-999999999");
            MaxLeafStack = StacksResourcesCategory.CreateEntry<string>("MaxLeafStack", "1000", "Max Leaves", "1-999999999");
            MaxPlantStack = StacksResourcesCategory.CreateEntry<string>("MaxPlantStack", "20", "Max Plants", "All plant types");
            MaxSeedStack = StacksResourcesCategory.CreateEntry<string>("MaxSeedStack", "20", "Max Seeds", "All seed types");
            MaxSolafiteStack = StacksResourcesCategory.CreateEntry<string>("MaxSolafiteStack", "20", "Max Solafite", "1-999999");
            MaxPouchStack = StacksResourcesCategory.CreateEntry<string>("MaxPouchStack", "6", "Max Skin Pouches", "1-999999999");
            MaxGliderStack = StacksResourcesCategory.CreateEntry<string>("MaxGliderStack", "1", "Max Hang Gliders", "1-999999999");
            MaxTarpStack = StacksResourcesCategory.CreateEntry<string>("MaxTarpStack", "4", "Max Tarps", "1-999999999");
            MaxAirTankStack = StacksResourcesCategory.CreateEntry<string>("MaxAirTankStack", "4", "Max Air Tanks", "1-999999999");
            MaxPaperTargetStack = StacksResourcesCategory.CreateEntry<string>("MaxPaperTargetStack", "50", "Max Paper Targets", "1-999999999");
            MaxGolfBallStack = StacksResourcesCategory.CreateEntry<string>("MaxGolfBallStack", "50", "Max Golf Balls", "1-999999999");
            MaxCashStack = StacksResourcesCategory.CreateEntry<string>("MaxCashStack", "1000000", "Max Cash", "1-999999999");

            // ===== CATEGORY: SCARYCROSS =====
            ScaryCrossCategory = ConfigSystem.CreateFileCategory("ProjectX - ScaryCross", "ProjectX - ScaryCross", configFile);
            
            SC_TempRiseThreshold = ScaryCrossCategory.CreateEntry<float>("SC_TempRiseThreshold", 60f, "SC: Temp Rise Threshold", "Light Intensity threshold for heat increase");
            SC_TempRiseThreshold.SetRange(1f, 100f);
            SC_BurnThreshold = ScaryCrossCategory.CreateEntry<float>("SC_BurnThreshold", 60f, "SC: Burn Threshold", "Heat threshold for burning");
            SC_BurnThreshold.SetRange(1f, 100f);
            SC_DamageThreshold = ScaryCrossCategory.CreateEntry<float>("SC_DamageThreshold", 95f, "SC: Damage Threshold", "Heat threshold for damage");
            SC_DamageThreshold.SetRange(1f, 100f);
            SC_DamageAfterSec = ScaryCrossCategory.CreateEntry<float>("SC_DamageAfterSec", 10f, "SC: Damage After Sec", "Seconds before damage tick");
            SC_DamageAfterSec.SetRange(1f, 60f);
            SC_LightIntensityRise = ScaryCrossCategory.CreateEntry<float>("SC_LightIntensityRise", 25f, "SC: Light Rise Rate", "Rate of light increase");
            SC_LightIntensityRise.SetRange(1f, 50f);
            SC_LightIntensityReduce = ScaryCrossCategory.CreateEntry<float>("SC_LightIntensityReduce", 8f, "SC: Light Reduce Rate", "Rate of light decrease");
            SC_LightIntensityReduce.SetRange(1f, 50f);
            SC_TempRisePerSec = ScaryCrossCategory.CreateEntry<float>("SC_TempRisePerSec", 25f, "SC: Temp Rise Rate", "Rate of temp increase");
            SC_TempRisePerSec.SetRange(1f, 50f);
            SC_TempReducePerSec = ScaryCrossCategory.CreateEntry<float>("SC_TempReducePerSec", 8f, "SC: Temp Reduce Rate", "Rate of temp decrease");
            SC_TempReducePerSec.SetRange(1f, 50f);
            SC_EffigyMinRange = ScaryCrossCategory.CreateEntry<float>("SC_EffigyMinRange", 24f, "SC: Effigy Min Range", "Min range of effigy effect");
            SC_EffigyMinRange.SetRange(1f, 100f);
            SC_EffigyMaxRange = ScaryCrossCategory.CreateEntry<float>("SC_EffigyMaxRange", 48f, "SC: Effigy Max Range", "Max range of effigy effect");
            SC_EffigyMaxRange.SetRange(1f, 100f);
            SC_EffigyMinStrength = ScaryCrossCategory.CreateEntry<float>("SC_EffigyMinStrength", 0.5f, "SC: Effigy Min Strength", "Min strength");
            SC_EffigyMinStrength.SetRange(0.1f, 10f);
            SC_EffigyMaxStrength = ScaryCrossCategory.CreateEntry<float>("SC_EffigyMaxStrength", 10f, "SC: Effigy Max Strength", "Max strength");
            SC_EffigyMaxStrength.SetRange(1f, 20f);
            SC_EffigyDisabledRange = ScaryCrossCategory.CreateEntry<float>("SC_EffigyDisabledRange", 14f, "SC: Effigy Disabled Range", "Range when off");
            SC_EffigyDisabledRange.SetRange(1f, 50f);
            SC_EffigyDisabledStrength = ScaryCrossCategory.CreateEntry<float>("SC_EffigyDisabledStrength", 0.25f, "SC: Effigy Disabled Strength", "Strength when off");
            SC_EffigyDisabledStrength.SetRange(0.1f, 5f);
            SC_BurnDemonRangeMin = ScaryCrossCategory.CreateEntry<float>("SC_BurnDemonRangeMin", 18f, "SC: Burn Demon Range Min", "Min range of burn effect");
            SC_BurnDemonRangeMin.SetRange(1f, 50f);
            SC_BurnDemonRangeMax = ScaryCrossCategory.CreateEntry<float>("SC_BurnDemonRangeMax", 36f, "SC: Burn Demon Range Max", "Max range of burn effect");
            SC_BurnDemonRangeMax.SetRange(1f, 100f);
            SC_BurnDemonTimeConfig = ScaryCrossCategory.CreateEntry<float>("SC_BurnDemonTime", 30f, "SC: Burn Demon Time", "Duration of burn effect");
            SC_BurnDemonTimeConfig.SetRange(1f, 120f);
            SC_DemonDetectRadiusMin = ScaryCrossCategory.CreateEntry<float>("SC_DemonDetectRadiusMin", 14f, "SC: Detection Radius Min", "Min detection radius");
            SC_DemonDetectRadiusMin.SetRange(1f, 50f);
            SC_DemonDetectRadiusMax = ScaryCrossCategory.CreateEntry<float>("SC_DemonDetectRadiusMax", 24f, "SC: Detection Radius Max", "Max detection radius");
            SC_DemonDetectRadiusMax.SetRange(1f, 100f);
            
            // ===== CATEGORY: X RAIDS - GENERAL =====
            XRaidsGeneralCategory = ConfigSystem.CreateFileCategory("X Raids - General", "X Raids - General", configFile);
            
            XR_AllowCreepy = XRaidsGeneralCategory.CreateEntry<bool>("XR_AllowCreepy", true, "Enable Creepy Raids");
            XR_AllowCannibals = XRaidsGeneralCategory.CreateEntry<bool>("XR_AllowCannibals", true, "Enable Cannibal Raids");
            XR_AllowMuddies = XRaidsGeneralCategory.CreateEntry<bool>("XR_AllowMuddies", true, "Enable Muddy Raids");
            XR_AnnounceRaids = XRaidsGeneralCategory.CreateEntry<bool>("XR_AnnounceRaids", true, "Announce Incoming Raids");
            XR_PlaySoundWhenAnnounced = XRaidsGeneralCategory.CreateEntry<bool>("XR_PlaySound", true, "Play Sound When Announced");
            XR_IncludeEndgameRaids = XRaidsGeneralCategory.CreateEntry<bool>("XR_IncludeEndgame", false, "Always Include Endgame Raids");
            XR_IncludeForestOnlyRaids = XRaidsGeneralCategory.CreateEntry<bool>("XR_IncludeForest", false, "Always Include Forest-Only Raids");
            XR_RaidsPerDay = XRaidsGeneralCategory.CreateEntry<int>("XR_RaidsPerDay", -1, "Max Raids Per Day (-1=Default)");
            XR_RaidsPerDay.SetRange(-1, 24);
            XR_RaidDistribution = XRaidsGeneralCategory.CreateEntry<string>("XR_Distribution", "Evenly", "Raid Distribution");
            XR_RaidDistribution.SetOptions(new[] { "Evenly", "Randomly", "Stacked" });
            XR_ConsiderCurrentTime = XRaidsGeneralCategory.CreateEntry<bool>("XR_ConsiderTime", false, "Consider Current Time");
            
            // ===== CATEGORY: X RAIDS - TIMES =====
            XRaidsTimesCategory = ConfigSystem.CreateFileCategory("X Raids - Times", "X Raids - Times", configFile);
            
            XR_RaidAtNight = XRaidsTimesCategory.CreateEntry<bool>("XR_RaidAtNight", true, "Raids at Night");
            XR_RaidAtDay = XRaidsTimesCategory.CreateEntry<bool>("XR_RaidAtDay", true, "Raids at Day");
            XR_RaidAtEvening = XRaidsTimesCategory.CreateEntry<bool>("XR_RaidAtEvening", true, "Raids at Evening");
            XR_RaidAtMorning = XRaidsTimesCategory.CreateEntry<bool>("XR_RaidAtMorning", true, "Raids at Morning");
            
            // ===== CATEGORY: X RAIDS - NORMAL =====
            XRaidsNormalCategory = ConfigSystem.CreateFileCategory("X Raids - Normal", "X Raids - Normal", configFile);
            
            XR_MinSpawnFactor = XRaidsNormalCategory.CreateEntry<float>("XR_MinSpawnFactor", 1f, "Min Spawn Count Factor");
            XR_MinSpawnFactor.SetRange(0.1f, 10f);
            XR_MaxSpawnFactor = XRaidsNormalCategory.CreateEntry<float>("XR_MaxSpawnFactor", 1f, "Max Spawn Count Factor");
            XR_MaxSpawnFactor.SetRange(0.1f, 10f);
            XR_EnemyLimit = XRaidsNormalCategory.CreateEntry<int>("XR_EnemyLimit", 20, "Max Enemies Per Raid");
            XR_EnemyLimit.SetRange(1, 30);
            XR_NormalCooldown = XRaidsNormalCategory.CreateEntry<int>("XR_Cooldown", -1, "Cooldown Days (-1=Default, 0=None)");
            XR_NormalCooldown.SetRange(-1, 50);
            XR_IgnoreDayLimit = XRaidsNormalCategory.CreateEntry<bool>("XR_IgnoreDayLimit", false, "Ignore Min/Max Day Limit");
            XR_IgnoreAngerLimit = XRaidsNormalCategory.CreateEntry<bool>("XR_IgnoreAngerLimit", false, "Ignore Min/Max Anger Limit");
            
            // ===== CATEGORY: X RAIDS - BOSS =====
            XRaidsBossCategory = ConfigSystem.CreateFileCategory("X Raids - Boss", "X Raids - Boss", configFile);
            
            XR_BossCount = XRaidsBossCategory.CreateEntry<int>("XR_BossCount", 1, "Boss Spawn Count");
            XR_BossCount.SetRange(1, 30);
            XR_BossCooldown = XRaidsBossCategory.CreateEntry<int>("XR_BossCooldown", -1, "Boss Cooldown Days (-1=Default)");
            XR_BossCooldown.SetRange(-1, 50);
            XR_BossIgnoreDayLimit = XRaidsBossCategory.CreateEntry<bool>("XR_BossIgnoreDay", false, "Ignore Boss Day Limit");
            XR_BossIgnoreAngerLimit = XRaidsBossCategory.CreateEntry<bool>("XR_BossIgnoreAnger", false, "Ignore Boss Anger Limit");
            
            // ===== CATEGORY: X RAIDS - ENEMY STATS =====
            XRaidsEnemyStatsCategory = ConfigSystem.CreateFileCategory("X Raids - Enemy Stats", "X Raids - Enemy Stats", configFile);
            
            XR_StatMultiplierEnabled = XRaidsEnemyStatsCategory.CreateEntry<bool>("XR_EnableStats", true, "Enable Stat Overrides");
            XR_OverrideHealthOnLoad = XRaidsEnemyStatsCategory.CreateEntry<bool>("XR_OverrideOnLoad", true, "Override Health on Save Load");
            XR_CannibalHealth = XRaidsEnemyStatsCategory.CreateEntry<float>("XR_CannibalHealth", 1.0f, "Cannibal Health Multiplier");
            XR_CannibalHealth.SetRange(0.1f, 10f);
            XR_CannibalDamage = XRaidsEnemyStatsCategory.CreateEntry<float>("XR_CannibalDamage", 1.0f, "Cannibal Damage Multiplier");
            XR_CannibalDamage.SetRange(0.1f, 10f);
            XR_CannibalAggression = XRaidsEnemyStatsCategory.CreateEntry<float>("XR_CannibalAggro", 1.0f, "Cannibal Aggression Multiplier");
            XR_CannibalAggression.SetRange(0.1f, 10f);
            XR_CreepHealth = XRaidsEnemyStatsCategory.CreateEntry<float>("XR_CreepHealth", 1.0f, "Creep Health Multiplier");
            XR_CreepHealth.SetRange(0.1f, 10f);
            XR_CreepDamage = XRaidsEnemyStatsCategory.CreateEntry<float>("XR_CreepDamage", 1.0f, "Creep Damage Multiplier");
            XR_CreepDamage.SetRange(0.1f, 10f);
            XR_CreepAggression = XRaidsEnemyStatsCategory.CreateEntry<float>("XR_CreepAggro", 1.0f, "Creep Aggression Multiplier");
            XR_CreepAggression.SetRange(0.1f, 10f);
            XR_BossHealth = XRaidsEnemyStatsCategory.CreateEntry<float>("XR_BossHealth", 1.0f, "Boss Health Multiplier");
            XR_BossHealth.SetRange(0.1f, 10f);
            XR_BossDamage = XRaidsEnemyStatsCategory.CreateEntry<float>("XR_BossDamage", 1.0f, "Boss Damage Multiplier");
            XR_BossDamage.SetRange(0.1f, 10f);
            XR_BossAggression = XRaidsEnemyStatsCategory.CreateEntry<float>("XR_BossAggro", 1.0f, "Boss Aggression Multiplier");
            XR_BossAggression.SetRange(0.1f, 10f);
            
            // ===== CATEGORY: X RAIDS - FOLLOWERS =====
            XRaidsFollowersCategory = ConfigSystem.CreateFileCategory("X Raids - Followers", "X Raids - Followers", configFile);
            
            XR_VirginiaHealth = XRaidsFollowersCategory.CreateEntry<float>("XR_VirginiaHealth", 1.0f, "Virginia Health Multiplier");
            XR_VirginiaHealth.SetRange(1f, 200f);
            XR_KelvinHealth = XRaidsFollowersCategory.CreateEntry<float>("XR_KelvinHealth", 1.0f, "Kelvin Health Multiplier");
            XR_KelvinHealth.SetRange(1f, 200f);
            
            // ===== CATEGORY: X RAIDS - MULTIPLAYER =====
            XRaidsMultiplayerCategory = ConfigSystem.CreateFileCategory("X Raids - Multiplayer", "X Raids - Multiplayer", configFile);
            
            XR_AdjustByPlayerCount = XRaidsMultiplayerCategory.CreateEntry<bool>("XR_AdjustByPlayers", false, "Adjust Raids by Player Count");
            XR_ExtraRaidsPerPlayer = XRaidsMultiplayerCategory.CreateEntry<int>("XR_ExtraRaids", 2, "Extra Raids Per Player");
            XR_ExtraRaidsPerPlayer.SetRange(0, 10);
            XR_ExtraSpawnsPerPlayer = XRaidsMultiplayerCategory.CreateEntry<int>("XR_ExtraSpawns", 2, "Extra Enemies Per Player");
            XR_ExtraSpawnsPerPlayer.SetRange(0, 10);
            XR_ExtraBossesPerPlayer = XRaidsMultiplayerCategory.CreateEntry<int>("XR_ExtraBosses", 1, "Extra Bosses Per Player");
            XR_ExtraBossesPerPlayer.SetRange(0, 10);
            
            // ===== CATEGORY: AMMO UI =====
            AmmoUiCategory = ConfigSystem.CreateFileCategory("ProjectX - AmmoUI", "ProjectX - AmmoUI", configFile);
            
            AmmoUiSize = AmmoUiCategory.CreateEntry<float>("AmmoUiSize", 1.0f, "AmmoUI Size", "Scale of the ammo display (0.1 = tiny, 2.0 = large)");
            AmmoUiSize.SetRange(0.1f, 2.0f);
            AmmoUiOpacity = AmmoUiCategory.CreateEntry<float>("AmmoUiOpacity", 1.0f, "AmmoUI Opacity", "Transparency of the ammo display (0 = invisible, 1 = solid)");
            AmmoUiOpacity.SetRange(0f, 1f);
            
            // ===== CATEGORY: HOTBAR =====
            HotbarCategory = ConfigSystem.CreateFileCategory("ProjectX - Hotbar", "ProjectX - Hotbar", configFile);
            
            HotbarEnabled = HotbarCategory.CreateEntry<bool>("HotbarEnabled", true, "Enable Hotbar", "Show the item hotbar HUD overlay");
#if !SERVER
            HotbarEnabled.OnValueChanged.Subscribe((_, newVal) => {
                Modules.Hotbar.HotbarModule.SetEnabled(newVal);
            });
#endif
            
            // ===== CATEGORY: WEAPON DAMAGE (RANGED) =====
            WeaponDmgRangedCategory = ConfigSystem.CreateFileCategory("ProjectX - Weapon Damage (Ranged)", "ProjectX - Weapon Damage (Ranged)", configFile);
            
            WD_PistolDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_PistolDmg", 2.5f, "Pistol Damage Multiplier", "Damage multiplier for the Pistol");
            WD_PistolDmg.SetRange(1f, 10f);
            WD_RevolverDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_RevolverDmg", 3.5f, "Revolver Damage Multiplier", "Damage multiplier for the Revolver");
            WD_RevolverDmg.SetRange(1f, 10f);
            WD_ShotgunDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_ShotgunDmg", 2.5f, "Shotgun Damage Multiplier", "Damage multiplier for the Shotgun (slugs)");
            WD_ShotgunDmg.SetRange(1f, 10f);
            WD_BuckshotDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_BuckshotDmg", 0.14f, "Buckshot Damage Factor", "Buckshot damage = Shotgun multiplier × this value (default 0.14 = reduced per-pellet)");
            WD_BuckshotDmg.SetRange(0.01f, 1f);
            WD_BuckshotSpread = WeaponDmgRangedCategory.CreateEntry<float>("WD_BuckshotSpread", 7f, "Buckshot Spread Angle", "Buckshot pellet scatter cone in degrees (lower = tighter)");
            WD_BuckshotSpread.SetRange(1f, 20f);
            WD_RifleDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_RifleDmg", 2.0f, "Rifle Damage Multiplier", "Damage multiplier for the Rifle");
            WD_RifleDmg.SetRange(1f, 10f);
            WD_CompoundBowDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_CompoundBowDmg", 1.0f, "Compound Bow Damage Multiplier", "Damage multiplier for the Compound Bow");
            WD_CompoundBowDmg.SetRange(0.5f, 10f);
            WD_CraftedBowDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_CraftedBowDmg", 1.0f, "Crafted Bow Damage Multiplier", "Damage multiplier for the Crafted Bow");
            WD_CraftedBowDmg.SetRange(0.5f, 10f);
            WD_CrossbowDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_CrossbowDmg", 1.0f, "Crossbow Damage Multiplier", "Damage multiplier for the Crossbow");
            WD_CrossbowDmg.SetRange(0.5f, 10f);
            WD_SlingshotDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_SlingshotDmg", 1.0f, "Slingshot Damage Multiplier", "Damage multiplier for the Slingshot");
            WD_SlingshotDmg.SetRange(0.5f, 10f);
            WD_StunGunDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_StunGunDmg", 1.0f, "Stun Gun Damage Multiplier", "Damage multiplier for the Stun Gun/Taser");
            WD_StunGunDmg.SetRange(0.5f, 10f);
            
            // Explosives & Ammo (within Ranged category)
            WD_GrenadeDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_GrenadeDmg", 1.0f, "Grenade Damage Multiplier", "Damage multiplier for Frag Grenades");
            WD_GrenadeDmg.SetRange(0.5f, 10f);
            WD_MolotovDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_MolotovDmg", 1.0f, "Molotov Damage Multiplier", "Damage multiplier for Molotov Cocktails");
            WD_MolotovDmg.SetRange(0.5f, 10f);
            WD_StickyBombDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_StickyBombDmg", 1.0f, "Sticky Bomb Damage Multiplier", "Damage multiplier for Time Bombs/Sticky Bombs");
            WD_StickyBombDmg.SetRange(0.5f, 10f);
            WD_C4Dmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_C4Dmg", 1.0f, "C4 Damage Multiplier", "Damage multiplier for C4 Bricks");
            WD_C4Dmg.SetRange(0.5f, 10f);
            WD_StoneArrowDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_StoneArrowDmg", 1.0f, "Stone Arrow Damage Multiplier", "Damage multiplier for Stone/Crafted Arrows");
            WD_StoneArrowDmg.SetRange(0.5f, 10f);
            WD_PrintedArrowDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_PrintedArrowDmg", 1.0f, "3D Printed Arrow Damage Multiplier", "Damage multiplier for 3D Printed Arrows");
            WD_PrintedArrowDmg.SetRange(0.5f, 10f);
            WD_CarbonArrowDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_CarbonArrowDmg", 1.0f, "Carbon Fiber Arrow Damage Multiplier", "Damage multiplier for Carbon Fiber Arrows");
            WD_CarbonArrowDmg.SetRange(0.5f, 10f);
            WD_FireArrowDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_FireArrowDmg", 1.0f, "Fire Arrow Damage Multiplier", "Damage multiplier for Fire Arrows");
            WD_FireArrowDmg.SetRange(0.5f, 10f);
            WD_ShockArrowDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_ShockArrowDmg", 1.0f, "Shock Arrow Damage Multiplier", "Damage multiplier for Shock Arrows");
            WD_ShockArrowDmg.SetRange(0.5f, 10f);
            WD_ExplosiveArrowDmg = WeaponDmgRangedCategory.CreateEntry<float>("WD_ExplosiveArrowDmg", 1.0f, "Explosive Arrow Damage Multiplier", "Damage multiplier for Explosive Arrows");
            WD_ExplosiveArrowDmg.SetRange(0.5f, 10f);
            
            // ===== CATEGORY: WEAPON DAMAGE (MELEE) =====
            WeaponDmgMeleeCategory = ConfigSystem.CreateFileCategory("ProjectX - Weapon Damage (Melee)", "ProjectX - Weapon Damage (Melee)", configFile);
            
            WD_KatanaDmg = WeaponDmgMeleeCategory.CreateEntry<float>("WD_KatanaDmg", 1.0f, "Katana Damage Multiplier", "Damage multiplier for the Katana");
            WD_KatanaDmg.SetRange(0.5f, 10f);
            WD_MacheteDmg = WeaponDmgMeleeCategory.CreateEntry<float>("WD_MacheteDmg", 1.0f, "Machete Damage Multiplier", "Damage multiplier for the Machete");
            WD_MacheteDmg.SetRange(0.5f, 10f);
            WD_ModernAxeDmg = WeaponDmgMeleeCategory.CreateEntry<float>("WD_ModernAxeDmg", 1.0f, "Modern Axe Damage Multiplier", "Damage multiplier for the Modern Axe");
            WD_ModernAxeDmg.SetRange(0.5f, 10f);
            WD_FireAxeDmg = WeaponDmgMeleeCategory.CreateEntry<float>("WD_FireAxeDmg", 1.0f, "Firefighter Axe Damage Multiplier", "Damage multiplier for the Firefighter Axe");
            WD_FireAxeDmg.SetRange(0.5f, 10f);
            WD_TacticalAxeDmg = WeaponDmgMeleeCategory.CreateEntry<float>("WD_TacticalAxeDmg", 1.0f, "Tactical Axe Damage Multiplier", "Damage multiplier for the Tactical Axe");
            WD_TacticalAxeDmg.SetRange(0.5f, 10f);
            WD_CraftedSpearDmg = WeaponDmgMeleeCategory.CreateEntry<float>("WD_CraftedSpearDmg", 1.0f, "Crafted Spear Damage Multiplier", "Damage multiplier for the Crafted Spear");
            WD_CraftedSpearDmg.SetRange(0.5f, 10f);
            WD_StunBatonDmg = WeaponDmgMeleeCategory.CreateEntry<float>("WD_StunBatonDmg", 1.0f, "Stun Baton Damage Multiplier", "Damage multiplier for the Stun Baton");
            WD_StunBatonDmg.SetRange(0.5f, 10f);
            WD_CraftedClubDmg = WeaponDmgMeleeCategory.CreateEntry<float>("WD_CraftedClubDmg", 1.0f, "Crafted Club Damage Multiplier", "Damage multiplier for the Crafted Club");
            WD_CraftedClubDmg.SetRange(0.5f, 10f);
            WD_GuitarDmg = WeaponDmgMeleeCategory.CreateEntry<float>("WD_GuitarDmg", 1.0f, "Guitar Damage Multiplier", "Damage multiplier for the Guitar");
            WD_GuitarDmg.SetRange(0.5f, 10f);
            WD_ChainsawDmg = WeaponDmgMeleeCategory.CreateEntry<float>("WD_ChainsawDmg", 1.0f, "Chainsaw Damage Multiplier", "Damage multiplier for the Chainsaw");
            WD_ChainsawDmg.SetRange(0.5f, 10f);
            WD_GolfPutterDmg = WeaponDmgMeleeCategory.CreateEntry<float>("WD_GolfPutterDmg", 1.0f, "Golf Putter Damage Multiplier", "Damage multiplier for the Golf Putter");
            WD_GolfPutterDmg.SetRange(0.5f, 10f);
            WD_KnifeDmg = WeaponDmgMeleeCategory.CreateEntry<float>("WD_KnifeDmg", 1.0f, "Knife Damage Multiplier", "Damage multiplier for the Knife");
            WD_KnifeDmg.SetRange(0.5f, 10f);
            
            // ===== CATEGORY: WEAPON FEATURES =====
            WeaponDmgFeaturesCategory = ConfigSystem.CreateFileCategory("ProjectX - Weapon Features", "ProjectX - Weapon Features", configFile);
            
            WD_Enabled = WeaponDmgFeaturesCategory.CreateEntry<bool>("WD_Enabled", true, "Enable Weapon Damage", "Master toggle — OFF restores vanilla damage for all weapons");
            WD_SolafiteBonus = WeaponDmgFeaturesCategory.CreateEntry<float>("WD_SolafiteBonus", 1.25f, "Solafite Plating Bonus", "Damage multiplier when weapon has solafite plating (1.0 = no bonus, 1.25 = +25%)");
            WD_SolafiteBonus.SetRange(1.0f, 3.0f);
            WD_InspectKey = WeaponDmgFeaturesCategory.CreateKeybindEntry("WD_InspectKey", "i", "Weapon Inspect Key", "Key to re-equip weapon and replay first-equip animation");
            WD_InspectLeftHand = WeaponDmgFeaturesCategory.CreateEntry<bool>("WD_InspectLeftHand", false, "Inspect Left Hand", "Allow inspecting left-hand items when no right-hand weapon is held");
            
            // Register config entries for network sync (must be after all entries created)
            Modules.Network.ConfigSyncPayload.RegisterEntries();
        }


        public static void Save()
        {
            // Save all categories
            CheatsCategory?.SaveToFile();
            MovementCategory?.SaveToFile();
            WorldCategory?.SaveToFile();
            ModulesCategory?.SaveToFile();
            ZiplineCategory?.SaveToFile();
            FeaturesCategory?.SaveToFile();
            DiscordCategory?.SaveToFile();
            KeysCategory?.SaveToFile();
            StacksCraftingCategory?.SaveToFile();
            StacksFoodCategory?.SaveToFile();
            StacksCombatCategory?.SaveToFile();
            StacksResourcesCategory?.SaveToFile();
            ScaryCrossCategory?.SaveToFile();
            
            // X Raids Categories
            XRaidsGeneralCategory?.SaveToFile();
            XRaidsTimesCategory?.SaveToFile();
            XRaidsNormalCategory?.SaveToFile();
            XRaidsBossCategory?.SaveToFile();
            XRaidsEnemyStatsCategory?.SaveToFile();
            XRaidsFollowersCategory?.SaveToFile();
            XRaidsMultiplayerCategory?.SaveToFile();
            
            // Weapon Damage Categories
            WeaponDmgRangedCategory?.SaveToFile();
            WeaponDmgMeleeCategory?.SaveToFile();
            WeaponDmgFeaturesCategory?.SaveToFile();
        }
    }
}

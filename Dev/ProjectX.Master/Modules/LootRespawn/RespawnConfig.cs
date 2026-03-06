using System.Collections.Generic;
using RedLoader;

namespace ProjectX.Master.Modules.LootRespawn
{
    /// <summary>
    /// Configuration for loot respawn system.
    /// Per-category toggles live in the native RedLoader prefs menu.
    /// GUI exposes only Enabled / RespawnDays / Reset.
    /// </summary>
    public static class RespawnConfig
    {
        // ── Core settings (GUI-visible) ──────────────────────────────

        private static int _respawnDays = 3;
        private static bool _enabled = true;
        private static bool _consoleLogging = false;

        public static int RespawnDays
        {
            get => _respawnDays;
            set => _respawnDays = System.Math.Clamp(value, 1, 100);
        }

        public static bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        public static bool ConsoleLogging
        {
            get => _consoleLogging;
            set => _consoleLogging = value;
        }

        // ── Per-category tracking toggles (native menu only) ─────────

        public static bool TrackMelee = true;
        public static bool TrackRanged = true;
        public static bool TrackWeaponMods = true;
        public static bool TrackMaterials = true;
        public static bool TrackFood = true;
        public static bool TrackMeds = true;
        public static bool TrackPlants = true;
        public static bool TrackAmmo = true;
        public static bool TrackThrowables = true;
        public static bool TrackExpendables = true;
        public static bool TrackBreakables = true;
        public static bool TrackOpenables = true;

        // ── Item ID lists (from GlaDOS's LootRespawnControl v2) ─────

        public static readonly HashSet<int> MeleeWeaponIds = new()
        {
            340, 356, 359, 367, 379, 394, 396, 431, 474, 477, 503, 525, 663, 485
        };

        public static readonly HashSet<int> RangedWeaponIds = new()
        {
            358, 360, 361, 386, 443, 459, 353, 365, 355
        };

        public static readonly HashSet<int> WeaponModIds = new()
        {
            346, 374, 375, 376, 378
        };

        public static readonly HashSet<int> MaterialIds = new()
        {
            634, 635, 403, 405, 410, 414, 415, 416, 418, 419, 420,
            430, 479, 496, 502, 527, 661, 517, 504, 590
        };

        public static readonly HashSet<int> FoodIds = new()
        {
            421, 425, 433, 434, 436, 438, 464, 569, 570, 571
        };

        public static readonly HashSet<int> MedIds = new()
        {
            437, 441, 439
        };

        public static readonly HashSet<int> PlantIds = new()
        {
            397, 398, 399, 400, 450, 451, 452, 453, 454, 465
        };

        public static readonly HashSet<int> AmmoIds = new()
        {
            373, 368, 362, 363, 364, 369, 387, 370, 371, 372, 457, 523
        };

        public static readonly HashSet<int> ThrowableIds = new()
        {
            524, 381, 417, 440
        };

        public static readonly HashSet<int> ExpendableIds = new()
        {
            390, 469, 508
        };

        /// <summary>Special pseudo-ID for breakable containers</summary>
        public const int BreakableId = 9999;

        /// <summary>Special pseudo-ID for openable containers (GrabBags, pelican cases)</summary>
        public const int OpenableId = 9998;

        /// <summary>Items that should NOT be tracked when inside breakable containers</summary>
        public static readonly HashSet<int> BreakableBlacklist = new() { 392 };

        // ── Filtering logic ──────────────────────────────────────────

        /// <summary>
        /// Returns true if the given item ID should be tracked for respawn,
        /// based on which per-category toggles are enabled.
        /// Items not in any known category are tracked unconditionally.
        /// </summary>
        public static bool ShouldTrackItem(int itemId)
        {
            // Check each category: if the item is in a category whose toggle is OFF, don't track
            if (MeleeWeaponIds.Contains(itemId))   return TrackMelee;
            if (RangedWeaponIds.Contains(itemId))   return TrackRanged;
            if (WeaponModIds.Contains(itemId))      return TrackWeaponMods;
            if (MaterialIds.Contains(itemId))       return TrackMaterials;
            if (FoodIds.Contains(itemId))           return TrackFood;
            if (MedIds.Contains(itemId))            return TrackMeds;
            if (PlantIds.Contains(itemId))          return TrackPlants;
            if (AmmoIds.Contains(itemId))           return TrackAmmo;
            if (ThrowableIds.Contains(itemId))      return TrackThrowables;
            if (ExpendableIds.Contains(itemId))     return TrackExpendables;

            // Breakable containers use a pseudo-ID
            if (itemId == BreakableId) return TrackBreakables;

            // Openable containers use a pseudo-ID
            if (itemId == OpenableId) return TrackOpenables;

            // Unknown category — track it
            return true;
        }

        /// <summary>Log current configuration</summary>
        public static void LogConfig()
        {
            RLog.Msg($"[RespawnConfig] Enabled={_enabled}, Days={_respawnDays}, Logging={_consoleLogging}");
        }
    }
}

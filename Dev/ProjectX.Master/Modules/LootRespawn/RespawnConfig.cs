using RedLoader;

namespace ProjectX.Master.Modules.LootRespawn
{
    /// <summary>
    /// Configuration for loot respawn system
    /// </summary>
    public static class RespawnConfig
    {
        private static int _respawnDays = 3;
        private static bool _enabled = true;

        /// <summary>
        /// Number of in-game days before loot respawns
        /// </summary>
        public static int RespawnDays
        {
            get => _respawnDays;
            set => _respawnDays = System.Math.Clamp(value, 1, 30);
        }

        /// <summary>
        /// Whether loot respawn is enabled
        /// </summary>
        public static bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        /// <summary>
        /// Log current configuration
        /// </summary>
        public static void LogConfig()
        {
            RLog.Msg($"[RespawnConfig] Enabled: {_enabled}, Days: {_respawnDays}");
        }
    }
}

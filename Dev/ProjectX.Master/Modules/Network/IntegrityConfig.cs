using System.Drawing;
using RedLoader;
using RedLoader.Preferences;

namespace ProjectX.Master.Modules.Network
{
    /// <summary>
    /// IntegrityConfig — Server-side anti-cheat configuration.
    /// Defines the mod whitelist, expected client DLL hash, and enforcement settings.
    /// Only the server reads these values; clients never see them.
    /// </summary>
    public static class IntegrityConfig
    {
        public static ConfigCategory Category { get; private set; }
        
        /// <summary>
        /// Master toggle for anti-cheat enforcement.
        /// When disabled, clients are not checked (useful for development).
        /// </summary>
        public static ConfigEntry<bool> Enabled { get; private set; }
        
        /// <summary>
        /// Comma-separated list of allowed mod IDs.
        /// Any mod loaded on the client that isn't in this list triggers a kick.
        /// Example: "ProjectX.Client,CoopServerTools"
        /// </summary>
        public static ConfigEntry<string> ModWhitelist { get; private set; }
        
        /// <summary>
        /// SHA256 hash of the authorized ProjectX.Client.dll.
        /// If set, the client must report a matching hash or get kicked.
        /// Leave blank to skip hash verification.
        /// </summary>
        public static ConfigEntry<string> ExpectedClientHash { get; private set; }
        
        /// <summary>
        /// Seconds to wait for client integrity response before kicking.
        /// Clients running unmodified ProjectX.Client respond within 1-2 seconds.
        /// Clients without it (or tampering with it) will timeout.
        /// </summary>
        public static ConfigEntry<float> GracePeriodSeconds { get; private set; }
        
        /// <summary>
        /// Message shown to kicked players.
        /// </summary>
        public static ConfigEntry<string> KickMessage { get; private set; }
        
        public static void Init()
        {
#if SERVER || OWNER
            Category = ConfigSystem.CreateFileCategory(
                "IntegrityCheck", 
                "Anti-Cheat", 
                "Server-side mod whitelist and integrity verification");
            
            Enabled = Category.CreateEntry<bool>(
                "Enabled", true,
                "Enable Anti-Cheat",
                "When enabled, clients must pass integrity checks to stay connected",
                false, false, null, null);
            
            ModWhitelist = Category.CreateEntry<string>(
                "ModWhitelist", "ProjectX.Master,ProjectX.Owner,ProjectX.Client,ProjectX.Server",
                "Allowed Mod IDs",
                "Comma-separated list of allowed mod IDs. Players with unlisted mods get kicked.",
                false, false, null, null);
            
            ExpectedClientHash = Category.CreateEntry<string>(
                "ExpectedClientHash", "",
                "Expected Client DLL Hash",
                "SHA256 hash of the authorized ProjectX.Client.dll. Leave blank to skip hash check.",
                false, false, null, null);
            
            GracePeriodSeconds = Category.CreateEntry<float>(
                "GracePeriodSeconds", 15f,
                "Grace Period (seconds)",
                "How long to wait for client response before kicking for non-response.",
                false, false, null, null);
            GracePeriodSeconds.SetRange(5f, 60f);
            
            KickMessage = Category.CreateEntry<string>(
                "KickMessage", "Unauthorized mods detected. Contact the server admin.",
                "Kick Message",
                "Message shown to players who fail the integrity check.",
                false, false, null, null);
            
            RLog.Msg(Color.Cyan, "[IntegrityConfig] Loaded — " +
                $"Enabled={Enabled.Value}, Whitelist=[{ModWhitelist.Value}], " +
                $"HashCheck={(!string.IsNullOrEmpty(ExpectedClientHash.Value) ? "ON" : "OFF")}, " +
                $"Grace={GracePeriodSeconds.Value}s");
#endif
        }
        
#if SERVER || OWNER
        /// <summary>
        /// Parse the whitelist string into a set of mod IDs for fast lookup.
        /// </summary>
        public static System.Collections.Generic.HashSet<string> GetWhitelistSet()
        {
            var set = new System.Collections.Generic.HashSet<string>(
                System.StringComparer.OrdinalIgnoreCase);
            
            if (string.IsNullOrWhiteSpace(ModWhitelist?.Value))
                return set;
            
            foreach (var entry in ModWhitelist.Value.Split(','))
            {
                var trimmed = entry.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    set.Add(trimmed);
            }
            
            return set;
        }
#endif
    }
}

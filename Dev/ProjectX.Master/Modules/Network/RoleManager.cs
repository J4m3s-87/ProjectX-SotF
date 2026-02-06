using System;
using System.Collections.Generic;
using System.IO;
using RedLoader;

namespace ProjectX.Master.Modules.Network
{
    /// <summary>
    /// RoleManager - RBAC system using roles.json
    /// Server-side only - loads and manages player roles
    /// </summary>
    public static class RoleManager
    {
        public enum Role
        {
            Player = 0,
            Moderator = 1,
            Admin = 2,
            Owner = 3
        }
        
        private static Dictionary<string, Role> _steamIdToRole = new Dictionary<string, Role>();
        private static bool _initialized;
        
        /// <summary>
        /// Initialize and load roles from config
        /// </summary>
        public static void Init()
        {
#if SERVER
            LoadRoles();
#endif
        }
        
#if SERVER || OWNER
        /// <summary>
        /// Load roles from roles.json
        /// </summary>
        private static void LoadRoles()
        {
            try
            {
                string rolesPath = Path.Combine(
                    Path.GetDirectoryName(typeof(RoleManager).Assembly.Location) ?? "",
                    "roles.json"
                );
                
                if (!File.Exists(rolesPath))
                {
                    // Create default roles file
                    CreateDefaultRolesFile(rolesPath);
                }
                
                string json = File.ReadAllText(rolesPath);
                ParseRoles(json);
                _initialized = true;
                RLog.Msg($"[RoleManager] Loaded {_steamIdToRole.Count} role assignments");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[RoleManager] Failed to load roles: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Create default roles.json template
        /// </summary>
        private static void CreateDefaultRolesFile(string path)
        {
            string template = @"{
  ""owner"": [],
  ""admin"": [],
  ""moderator"": [],
  ""player"": []
}";
            File.WriteAllText(path, template);
            RLog.Msg($"[RoleManager] Created default roles.json at: {path}");
        }
        
        /// <summary>
        /// Parse roles JSON (simple parser - no external dependencies)
        /// </summary>
        private static void ParseRoles(string json)
        {
            _steamIdToRole.Clear();
            
            // Simple JSON parsing for our known structure
            var lines = json.Split('\n');
            Role currentRole = Role.Player;
            
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                
                if (trimmed.Contains("\"owner\""))
                    currentRole = Role.Owner;
                else if (trimmed.Contains("\"admin\""))
                    currentRole = Role.Admin;
                else if (trimmed.Contains("\"moderator\""))
                    currentRole = Role.Moderator;
                else if (trimmed.Contains("\"player\""))
                    currentRole = Role.Player;
                else if (trimmed.StartsWith("\"") && trimmed.Length > 10)
                {
                    // Extract SteamID
                    int start = trimmed.IndexOf('"') + 1;
                    int end = trimmed.LastIndexOf('"');
                    if (end > start)
                    {
                        string steamId = trimmed.Substring(start, end - start);
                        if (steamId.Length >= 10 && char.IsDigit(steamId[0]))
                        {
                            _steamIdToRole[steamId] = currentRole;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Get role for a player by SteamID
        /// </summary>
        public static Role GetRole(string steamId)
        {
            if (_steamIdToRole.TryGetValue(steamId, out Role role))
                return role;
            return Role.Player;
        }
        
        /// <summary>
        /// Check if player has at least the required role level
        /// </summary>
        public static bool HasRole(string steamId, Role requiredRole)
        {
            return GetRole(steamId) >= requiredRole;
        }
        
        /// <summary>
        /// Check if player can use admin commands
        /// </summary>
        public static bool IsAdmin(string steamId)
        {
            return HasRole(steamId, Role.Admin);
        }
        
        /// <summary>
        /// Check if player is the owner
        /// </summary>
        public static bool IsOwner(string steamId)
        {
            return HasRole(steamId, Role.Owner);
        }
#endif
    }
}

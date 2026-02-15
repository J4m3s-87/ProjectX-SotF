using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using RedLoader;
using RedLoader.Utils;
using SonsSdk;

namespace ProjectX.Master.Modules.DedicatedSuperuser.Utility
{
    /// <summary>
    /// Admin authentication with multi-layer fallback:
    ///   1. Custom OwnerWhitelist.txt (our mod's explicit admin list)
    ///   2. Game's native admin system (Sons.Multiplayer.MultiplayerPlayerRoles.IsAdmin)
    ///      — this is what EUgamehost/hosting CPs set via their admin whitelist
    ///   3. Special IDs: "server", "admin", "owner" are always trusted
    /// </summary>
    public static class OwnerWhitelistUtils
    {
        private static HashSet<string> _whitelist = new HashSet<string>();
        private static string _filePath;
        
        // Game's native admin check (resolved once via reflection)
        private static MethodInfo _gameIsAdmin;
        private static PropertyInfo _gameRolesInstance;
        private static bool _gameAdminResolved;

        public static void Init()
        {
            _filePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "OwnerWhitelist.txt");
            LoadWhitelist();
            ResolveGameAdminSystem();
        }

        public static void LoadWhitelist()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "// Add SteamIDs here, one per line\n// Players on the game's admin list (EUgamehost CP) are also recognized automatically.\n");
            }

            _whitelist.Clear();
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (!string.IsNullOrEmpty(trimmed) && !trimmed.StartsWith("//"))
                {
                    _whitelist.Add(trimmed);
                }
            }
            RLog.Msg($"Loaded {_whitelist.Count} Admins from OwnerWhitelist.txt");
        }
        
        /// <summary>
        /// Resolve Sons.Multiplayer.MultiplayerPlayerRoles.IsAdmin(ulong steamId)
        /// This is the game's native admin system — set via hosting control panels.
        /// </summary>
        private static void ResolveGameAdminSystem()
        {
            try
            {
                var rolesType = HarmonyLib.AccessTools.TypeByName("Sons.Multiplayer.MultiplayerPlayerRoles") ??
                                HarmonyLib.AccessTools.TypeByName("MultiplayerPlayerRoles");
                if (rolesType != null)
                {
                    _gameRolesInstance = HarmonyLib.AccessTools.Property(rolesType, "Instance");
                    _gameIsAdmin = rolesType.GetMethod("IsAdmin", 
                        BindingFlags.Instance | BindingFlags.Public, 
                        null, new Type[] { typeof(ulong) }, null);
                    _gameAdminResolved = _gameRolesInstance != null && _gameIsAdmin != null;
                    RLog.Msg($"[OwnerWhitelist] Game admin system resolved: {_gameAdminResolved}");
                }
                else
                {
                    RLog.Msg("[OwnerWhitelist] MultiplayerPlayerRoles not found — game admin fallback unavailable");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[OwnerWhitelist] Game admin resolution failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Check if a user is admin via multi-layer auth:
        ///   1. Special system IDs (server, admin, owner) → always admin
        ///   2. Custom OwnerWhitelist.txt → explicit mod admin list
        ///   3. Game's native MultiplayerPlayerRoles.IsAdmin → hosting CP admin list
        /// </summary>
        public static bool IsAdmin(string steamId)
        {
            // System/internal IDs are always trusted
            if (steamId == "server" || steamId == "admin" || steamId == "owner")
                return true;
            
            // Check our custom whitelist
            if (_whitelist.Contains(steamId))
                return true;
            
            // Fallback: check game's native admin system (EUgamehost CP)
            if (_gameAdminResolved && ulong.TryParse(steamId, out ulong steamIdNum))
            {
                try
                {
                    var instance = _gameRolesInstance.GetValue(null);
                    if (instance != null)
                    {
                        bool isGameAdmin = (bool)_gameIsAdmin.Invoke(instance, new object[] { steamIdNum });
                        if (isGameAdmin)
                        {
                            RLog.Msg($"[OwnerWhitelist] {steamId} authorized via game admin system");
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[OwnerWhitelist] Game admin check failed: {ex.Message}");
                }
            }
            
            return false;
        }
    }
}

using System.Collections.Generic;
using System.IO;
using RedLoader;
using RedLoader.Utils;
using SonsSdk;

namespace ProjectX.Master.Modules.DedicatedSuperuser.Utility
{
    public static class OwnerWhitelistUtils
    {
        private static HashSet<string> _whitelist = new HashSet<string>();
        private static string _filePath;

        public static void Init()
        {
            _filePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "OwnerWhitelist.txt");
            LoadWhitelist();
        }

        public static void LoadWhitelist()
        {
            if (!File.Exists(_filePath))
            {
                // Create default (empty) or with local user?
                File.WriteAllText(_filePath, "// Add SteamIDs here, one per line\n");
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

        public static bool IsAdmin(string steamId)
        {
            // If list is empty, maybe allow everyone? Or allow no one? 
            // DedicatedSuperuser logic usually implies restrictive.
            // For now, let's assume if file exists, enforce it.
            return _whitelist.Contains(steamId);
        }
    }
}

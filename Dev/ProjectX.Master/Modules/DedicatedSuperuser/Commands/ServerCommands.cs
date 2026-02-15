using System;
using System.Collections.Generic;
using System.IO;
using RedLoader;
using ProjectX.Master.Modules.DedicatedSuperuser.Utility;

namespace ProjectX.Master.Modules.DedicatedSuperuser.Commands
{
    /// <summary>
    /// Server management commands — save, status, player info, admin management.
    /// </summary>
    public static class ServerCommands
    {
        public static void Handle(string steamId, string[] args)
        {
            if (args.Length == 0)
            {
                Log("Usage: /px server save|status|admin");
                return;
            }

            switch (args[0].ToLower())
            {
                case "save":    ForceSave(); break;
                case "status":  ShowStatus(); break;
                case "admin":
                    if (args.Length >= 2)
                        HandleAdminCommand(args[1..]);
                    else
                        Log("Usage: /px server admin add|remove|list [steamid]");
                    break;
                default:
                    Log($"Unknown server command: {args[0]}");
                    break;
            }
        }

        /// <summary>
        /// Force a world save
        /// </summary>
        public static void ForceSave()
        {
            try
            {
                // Use reflection to find save method
                var saveToolsType = typeof(SonsSdk.SonsSaveTools);
                var saveMethod = saveToolsType.GetMethod("Save", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                if (saveMethod != null)
                {
                    saveMethod.Invoke(null, null);
                    Log("World save triggered");
                    return;
                }
                
                // Fallback: try SaveGameManager
                var sgmType = Type.GetType("Sons.Save.SaveGameManager, Assembly-CSharp");
                if (sgmType != null)
                {
                    var method = sgmType.GetMethod("Save", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                    method?.Invoke(null, null);
                    Log("World save triggered via SaveGameManager");
                    return;
                }
                
                Log("Save method not found — requires runtime type discovery");
            }
            catch (Exception ex)
            {
                Log($"ForceSave failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Show mod status info
        /// </summary>
        public static void ShowStatus()
        {
            try
            {
                Log("──── Project X Status ────");
                Log($"Version: v1.7.0 Server Edition");
                Log($"Platform: {(UnityEngine.Application.isBatchMode ? "Dedicated Server" : "Client")}");
                Log($"Loot Respawn: {LootRespawn.LootRespawnModule.GetStatus()}");
                
                // Config state
                Log($"Structure Durability: {Config.StructureDurabilityMultiplier.Value}x");
                Log($"Loot Respawn Days: {Config.LootRespawnDays.Value}");
                Log($"AI Freeze: {Config.FreezeAI.Value}");
                
                // Raid state
                Log($"Raids/Day: {Config.XR_RaidsPerDay.Value}");
                Log($"Cannibals: {Config.XR_AllowCannibals.Value} | Creepy: {Config.XR_AllowCreepy.Value} | Muddies: {Config.XR_AllowMuddies.Value}");
                Log("──────────────────────────");
            }
            catch (Exception ex)
            {
                Log($"ShowStatus failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Admin whitelist management
        /// </summary>
        private static void HandleAdminCommand(string[] args)
        {
            if (args.Length == 0)
            {
                Log("Usage: /px server admin add|remove|list [steamid]");
                return;
            }

            switch (args[0].ToLower())
            {
                case "add":
                    if (args.Length >= 2)
                    {
                        AddAdmin(args[1]);
                    }
                    else Log("Usage: /px server admin add <steamid>");
                    break;

                case "remove":
                    if (args.Length >= 2)
                    {
                        RemoveAdmin(args[1]);
                    }
                    else Log("Usage: /px server admin remove <steamid>");
                    break;

                case "list":
                    ListAdmins();
                    break;

                default:
                    Log($"Unknown admin command: {args[0]}");
                    break;
            }
        }

        private static void AddAdmin(string steamId)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "OwnerWhitelist.txt");
                File.AppendAllText(filePath, $"\n{steamId}");
                OwnerWhitelistUtils.LoadWhitelist();
                Log($"Added admin: {steamId}");
            }
            catch (Exception ex)
            {
                Log($"AddAdmin failed: {ex.Message}");
            }
        }

        private static void RemoveAdmin(string steamId)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "OwnerWhitelist.txt");
                if (!File.Exists(filePath))
                {
                    Log("Whitelist file not found");
                    return;
                }

                var lines = File.ReadAllLines(filePath);
                var filtered = new List<string>();
                foreach (var line in lines)
                {
                    if (line.Trim() != steamId)
                        filtered.Add(line);
                }
                File.WriteAllLines(filePath, filtered);
                OwnerWhitelistUtils.LoadWhitelist();
                Log($"Removed admin: {steamId}");
            }
            catch (Exception ex)
            {
                Log($"RemoveAdmin failed: {ex.Message}");
            }
        }

        private static void ListAdmins()
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "OwnerWhitelist.txt");
                if (!File.Exists(filePath))
                {
                    Log("No whitelist file found");
                    return;
                }

                var lines = File.ReadAllLines(filePath);
                Log("──── Admin List ────");
                int count = 0;
                foreach (var line in lines)
                {
                    var trimmed = line.Trim();
                    if (!string.IsNullOrEmpty(trimmed) && !trimmed.StartsWith("//"))
                    {
                        Log($"  {trimmed}");
                        count++;
                    }
                }
                Log($"Total: {count} admins");
            }
            catch (Exception ex)
            {
                Log($"ListAdmins failed: {ex.Message}");
            }
        }

        private static void Log(string msg)
        {
            // Send to all connected players' chat AND server log
            ChatResponse.Send(msg);
        }
    }
}

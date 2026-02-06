using System;
using System.Collections.Generic;
using System.IO;
using RedLoader;
using SonsSdk;
using TheForest.Utils;
using UnityEngine;
using ProjectX.Master.Modules.Network;

namespace ProjectX.Master.Modules.BroadcastMessage
{
    /// <summary>
    /// BroadcastMessage Module - File-Based Implementation
    /// Replaces Harmony ChatBox patching with file-based logging and command triggers.
    /// No Harmony patches = no IL2CPP crashes.
    /// </summary>
    public static class BroadcastMessageModule
    {
        private static bool _initialized;
        private static string _logPath;
        private static HashSet<string> _recentMessages = new HashSet<string>();
        private static float _lastCleanupTime = 0f;
        private const float CLEANUP_INTERVAL = 60f; // Clear seen messages every 60 seconds

        public static void Init()
        {
            if (_initialized) return;

            try
            {
                // Setup log file path in user's local app data
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var logDir = Path.Combine(appDataPath, "SonsOfTheForest", "ProjectX");
                
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                
                _logPath = Path.Combine(logDir, "ChatLog.txt");

                // Write startup marker
                LogToFile("[BroadcastMessage] Module initialized - Chat logging active");

                // Subscribe to update loop for periodic tasks
                SdkEvents.OnInWorldUpdate.Subscribe(OnUpdate);

                RLog.Msg($"[BroadcastMessage] Initialized (File-Based Mode)");
                RLog.Msg($"[BroadcastMessage] Chat log: {_logPath}");
                
                _initialized = true;
            }
            catch (Exception ex)
            {
                RLog.Error($"[BroadcastMessage] Failed to initialize: {ex.Message}");
            }
        }

        private static void OnUpdate()
        {
            if (!Config.EnableDiscordBridge.Value) return;
            if (!LocalPlayer.IsInWorld) return;

            // Periodic cleanup of seen messages
            if (Time.time - _lastCleanupTime > CLEANUP_INTERVAL)
            {
                _recentMessages.Clear();
                _lastCleanupTime = Time.time;
            }
        }

        /// <summary>
        /// Broadcast a message to the chat log and optionally to Discord.
        /// Call this from other modules or when a significant event occurs.
        /// </summary>
        public static void Broadcast(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            // Log locally
            LogToFile($"[BROADCAST] {message}");

            // Send to Discord if enabled
            if (Config.EnableDiscordBridge.Value)
            {
                DiscordBridge.SendMessage($"📢 {message}");
            }
        }

        /// <summary>
        /// Log a player chat message. 
        /// Call this if you have access to chat events from another source.
        /// </summary>
        public static void LogChatMessage(string playerName, string message, string steamId = null)
        {
            if (string.IsNullOrEmpty(message)) return;

            // Check for /px commands (Server/Owner only)
#if SERVER || OWNER
            if (message.StartsWith("/px"))
            {
                string senderSteamId = steamId ?? playerName; // Use steamId if available
                bool handled = CommandBridge.ProcessMessage(senderSteamId, message);
                if (handled)
                {
                    RLog.Msg($"[BroadcastMessage] /px command from {playerName}: {message}");
                    return; // Don't log commands to chat
                }
            }
#endif

            // Deduplicate recent messages
            string key = $"{playerName}:{message}";
            if (_recentMessages.Contains(key)) return;
            _recentMessages.Add(key);

            // Prevent memory bloat
            if (_recentMessages.Count > 500)
            {
                _recentMessages.Clear();
            }

            // Log to file
            LogToFile($"{playerName}: {message}");

            // Send to Discord if enabled
            if (Config.EnableDiscordBridge.Value)
            {
                DiscordBridge.SendMessage($"**{playerName}**: {message}");
            }
        }

        /// <summary>
        /// Log a system event (player join/leave, death, etc.) with rich embeds
        /// </summary>
        public static void LogEvent(string eventType, string details)
        {
            string message = $"[{eventType}] {details}";
            LogToFile(message);

            if (Config.EnableDiscordBridge.Value)
            {
                // Use rich embeds for different event types
                switch (eventType.ToLower())
                {
                    case "join":
                        DiscordBridge.SendPlayerJoin(details, isFirstTime: false);
                        break;
                    case "firstjoin":
                        DiscordBridge.SendPlayerJoin(details, isFirstTime: true);
                        break;
                    case "leave":
                        DiscordBridge.SendPlayerLeave(details);
                        break;
                    case "death":
                        DiscordBridge.SendDeath(details, null);
                        break;
                    case "raid":
                        DiscordBridge.SendRaidAlert(details);
                        break;
                    default:
                        DiscordBridge.SendMessage($"ℹ️ {details}");
                        break;
                }
            }
        }


        private static void LogToFile(string message)
        {
            if (string.IsNullOrEmpty(_logPath)) return;

            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string line = $"[{timestamp}] {message}{Environment.NewLine}";
                File.AppendAllText(_logPath, line);
            }
            catch
            {
                // Silent fail to avoid log spam
            }
        }

        /// <summary>
        /// Get the path to the chat log file for external tools.
        /// </summary>
        public static string GetLogPath()
        {
            return _logPath;
        }

        /// <summary>
        /// Manual trigger for sending a test message to Discord.
        /// Can be called from the game console or other modules.
        /// </summary>
        public static void TestDiscordConnection()
        {
            if (!Config.EnableDiscordBridge.Value)
            {
                RLog.Warning("[BroadcastMessage] Discord Bridge is disabled in config");
                return;
            }

            string testMsg = $"🔧 Test message from Project X at {DateTime.Now:HH:mm:ss}";
            DiscordBridge.SendMessage(testMsg);
            RLog.Msg("[BroadcastMessage] Test message sent to Discord");
        }

        /// <summary>
        /// Test the welcome embed
        /// </summary>
        public static void TestWelcomeEmbed()
        {
            if (!Config.EnableDiscordBridge.Value)
            {
                RLog.Warning("[BroadcastMessage] Discord Bridge is disabled in config");
                return;
            }

            DiscordBridge.SendWelcomeEmbed("TestPlayer");
            RLog.Msg("[BroadcastMessage] Welcome embed sent to Discord");
        }

        /// <summary>
        /// Test player join embed (non-first-time)
        /// </summary>
        public static void TestPlayerJoin()
        {
            if (!Config.EnableDiscordBridge.Value) return;
            DiscordBridge.SendPlayerJoin("TestPlayer", isFirstTime: false);
            RLog.Msg("[BroadcastMessage] Player join embed sent");
        }

        /// <summary>
        /// Test player leave embed
        /// </summary>
        public static void TestPlayerLeave()
        {
            if (!Config.EnableDiscordBridge.Value) return;
            DiscordBridge.SendPlayerLeave("TestPlayer");
            RLog.Msg("[BroadcastMessage] Player leave embed sent");
        }

        /// <summary>
        /// Test death embed
        /// </summary>
        public static void TestDeath()
        {
            if (!Config.EnableDiscordBridge.Value) return;
            DiscordBridge.SendDeath("TestPlayer", "Cannibal");
            RLog.Msg("[BroadcastMessage] Death embed sent");
        }
    }
}


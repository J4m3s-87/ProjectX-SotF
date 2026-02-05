using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RedLoader;

namespace ProjectX.Master.Modules.BroadcastMessage
{
    public static class DiscordBridge
    {
        private static readonly HttpClient _client = new HttpClient();
        
        // Project X Theme Colors
        private const int COLOR_RED = 0xFF0000;      // Red for alerts/deaths
        private const int COLOR_BLACK = 0x1a1a1a;   // Dark for general
        private const int COLOR_GREEN = 0x00FF00;   // Green for joins
        private const int COLOR_GRAY = 0x808080;    // Gray for leaves

        public static void SendMessage(string message)
        {
            if (!Config.EnableDiscordBridge.Value) return;

            string token = Config.DiscordBotToken.Value;
            string channelId = Config.DiscordChannelId.Value;

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(channelId))
            {
                return;
            }

            _ = PostMessageAsync(token, channelId, message);
        }

        /// <summary>
        /// Send a rich embed to Discord
        /// </summary>
        public static void SendEmbed(string title, string description, int color, string footer = null)
        {
            if (!Config.EnableDiscordBridge.Value) return;

            string token = Config.DiscordBotToken.Value;
            string channelId = Config.DiscordChannelId.Value;

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(channelId))
            {
                return;
            }

            _ = PostEmbedAsync(token, channelId, title, description, color, footer);
        }

        /// <summary>
        /// Send welcome embed for new players
        /// </summary>
        public static void SendWelcomeEmbed(string playerName)
        {
            string description = $"**🎮 Welcome, {playerName}!**\n\n" +
                "━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                "**📦 CLIENT MOD PACK**\n" +
                "Get the full experience with our client mods!\n" +
                "Check **#mod-download** for the latest version.\n\n" +
                "**⚙️ FEATURES**\n" +
                "• Custom Stack Sizes\n" +
                "• Structure Durability & Repair\n" +
                "• Raid Customization\n" +
                "• Zipline Building\n" +
                "• And more...\n\n" +
                "**💾 SAVE MECHANICS**\n" +
                "• Server auto-saves every 5 minutes (bases, structures)\n" +
                "• **Your personal progress** (inventory, weapons) is saved\n" +
                "  when YOU use a bed or tent in-game\n\n" +
                "**📜 RULES**\n" +
                "• Do NOT interfere with other players' bases\n" +
                "  without permission from the owner\n\n" +
                "━━━━━━━━━━━━━━━━━━━━━━━━━━━";

            SendEmbed("🎮 WELCOME TO PROJECT X", description, COLOR_RED, "Enjoy your stay!");
        }

        /// <summary>
        /// Send player join notification
        /// </summary>
        public static void SendPlayerJoin(string playerName, bool isFirstTime = false)
        {
            if (isFirstTime)
            {
                SendWelcomeEmbed(playerName);
            }
            else
            {
                SendEmbed("🟢 Player Joined", $"**{playerName}** joined the server", COLOR_GREEN);
            }
        }

        /// <summary>
        /// Send player leave notification
        /// </summary>
        public static void SendPlayerLeave(string playerName)
        {
            SendEmbed("🔴 Player Left", $"**{playerName}** left the server", COLOR_GRAY);
        }

        /// <summary>
        /// Send death notification
        /// </summary>
        public static void SendDeath(string playerName, string cause)
        {
            string desc = string.IsNullOrEmpty(cause) 
                ? $"**{playerName}** died" 
                : $"**{playerName}** was killed by **{cause}**";
            SendEmbed("💀 Player Death", desc, COLOR_RED);
        }

        /// <summary>
        /// Send raid alert
        /// </summary>
        public static void SendRaidAlert(string raidInfo)
        {
            SendEmbed("⚔️ RAID STARTED", raidInfo, COLOR_RED, "Defend your base!");
        }

        private static async Task PostMessageAsync(string token, string channelId, string content)
        {
            try
            {
                string url = $"https://discord.com/api/v9/channels/{channelId}/messages";
                string escapedContent = content.Replace("\\", "\\\\").Replace("\"", "\\\"");
                string jsonBody = $"{{\"content\": \"{escapedContent}\"}}";

                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Headers.Add("Authorization", $"Bot {token}");
                    request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var response = await _client.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        RLog.Warning($"[DiscordBridge] Failed to send message: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DiscordBridge] Error sending message: {ex.Message}");
            }
        }

        private static async Task PostEmbedAsync(string token, string channelId, string title, string description, int color, string footer)
        {
            try
            {
                string url = $"https://discord.com/api/v9/channels/{channelId}/messages";
                
                // Escape content for JSON
                string escTitle = EscapeJson(title);
                string escDesc = EscapeJson(description);
                string escFooter = EscapeJson(footer ?? "");

                // Build embed JSON
                string footerJson = string.IsNullOrEmpty(footer) ? "" : $",\"footer\":{{\"text\":\"{escFooter}\"}}";
                string embedJson = $"{{\"title\":\"{escTitle}\",\"description\":\"{escDesc}\",\"color\":{color}{footerJson}}}";
                string jsonBody = $"{{\"embeds\":[{embedJson}]}}";

                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Headers.Add("Authorization", $"Bot {token}");
                    request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var response = await _client.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        RLog.Warning($"[DiscordBridge] Embed failed: {response.StatusCode} - {responseBody}");
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DiscordBridge] Error sending embed: {ex.Message}");
            }
        }

        private static string EscapeJson(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return input
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "")
                .Replace("\t", "\\t");
        }
    }
}

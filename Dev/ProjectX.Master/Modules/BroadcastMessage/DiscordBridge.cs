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

        public static void SendMessage(string message)
        {
            if (!Config.EnableDiscordBridge.Value) return;

            string token = Config.DiscordBotToken.Value;
            string channelId = Config.DiscordChannelId.Value;

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(channelId))
            {
                // Only warn once or debug logging to avoid spam
                // RLog.Warning("Discord Bridge enabled but Token or ChannelID missing.");
                return;
            }

            // Fire and forget
            _ = PostMessageAsync(token, channelId, message);
        }

        private static async Task PostMessageAsync(string token, string channelId, string content)
        {
            try
            {
                string url = $"https://discord.com/api/v9/channels/{channelId}/messages";
                
                // Simple JSON construction to avoid dependency issues with Newtonsoft if not present
                // Escape quotes and backslashes
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
    }
}

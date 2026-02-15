using System;
using System.Net.Http;
using System.Text;
using System.Threading;
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
        
        // Discord → Game polling state
        private static CancellationTokenSource _pollCts;
        private static string _lastMessageId;
        private static string _botUserId;  // To filter out our own messages
        private static bool _pollingActive;

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
        
        // ====================================================================
        // Discord-Sourced Welcome Messages
        // ====================================================================
        
        /// <summary>
        /// Fetch the latest message from the configured welcome/rules channel,
        /// then post it as a plain text message to the chat channel.
        /// The polling loop picks it up and relays it via the proven Discord→Game path.
        /// </summary>
        public static void FetchAndRelayWelcome(string playerName)
        {
            if (!Config.EnableDiscordBridge.Value) return;
            
            string token = Config.DiscordBotToken.Value;
            string chatChannelId = Config.DiscordChannelId.Value;
            string welcomeChannelId = Config.DiscordWelcomeChannelId?.Value;
            
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(chatChannelId))
            {
                RLog.Msg("[DiscordBridge] Discord bridge not configured — skipping welcome");
                return;
            }
            
            if (string.IsNullOrEmpty(welcomeChannelId))
            {
                RLog.Msg("[DiscordBridge] Welcome channel not configured — skipping welcome fetch");
                return;
            }
            
            _ = FetchAndPostWelcomeAsync(token, chatChannelId, welcomeChannelId, playerName);
        }
        
        private static async Task FetchAndPostWelcomeAsync(string token, string chatChannelId, string welcomeChannelId, string playerName)
        {
            try
            {
                // Step 1: Fetch the latest message from the welcome channel
                string url = $"https://discord.com/api/v9/channels/{welcomeChannelId}/messages?limit=1";
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    request.Headers.Add("Authorization", $"Bot {token}");
                    var response = await _client.SendAsync(request);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        RLog.Warning($"[DiscordBridge] Welcome fetch failed: {response.StatusCode}");
                        return;
                    }
                    
                    string body = await response.Content.ReadAsStringAsync();
                    RLog.Msg($"[DiscordBridge] Welcome raw body ({body.Length} chars)");
                    
                    var messages = ParseMessages(body);
                    RLog.Msg($"[DiscordBridge] Welcome parsed {messages.Count} message(s)");
                    
                    if (messages.Count == 0)
                    {
                        RLog.Msg("[DiscordBridge] No messages parsed from welcome channel");
                        return;
                    }
                    
                    var welcomeMsg = messages[0];
                    string content = welcomeMsg.Content;
                    
                    // If content is empty, try to extract text from embeds
                    if (string.IsNullOrEmpty(content))
                    {
                        var messageJsons = SplitJsonArray(body);
                        if (messageJsons.Count > 0)
                            content = ExtractEmbedText(messageJsons[0]);
                    }
                    
                    if (string.IsNullOrEmpty(content))
                    {
                        RLog.Msg("[DiscordBridge] Welcome message has no text content");
                        return;
                    }
                    
                    // Format with ASCII-safe decorations (each \n becomes a separate chat line)
                    string welcomeText = $"[WELCOME] >>> Welcome to the server, {playerName}! <<<\n{content}";
                    RLog.Msg($"[DiscordBridge] Posting welcome to chat channel: {welcomeText}");
                    
                    bool success = await PostMessageAsync(token, chatChannelId, welcomeText);
                    if (success)
                        RLog.Msg($"[DiscordBridge] Welcome posted to chat channel — polling will relay to game");
                    else
                        RLog.Warning($"[DiscordBridge] Welcome POST failed — message will not appear in game");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DiscordBridge] Welcome fetch/post error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Extract text from Discord embed objects in a message JSON.
        /// Embeds have "title" and "description" fields with the actual text.
        /// </summary>
        private static string ExtractEmbedText(string msgJson)
        {
            // Find "embeds" array
            int embedsIdx = msgJson.IndexOf("\"embeds\"");
            if (embedsIdx < 0) return null;
            
            // Find the array start
            int arrayStart = msgJson.IndexOf('[', embedsIdx);
            if (arrayStart < 0) return null;
            
            // Find the first embed object
            int embedStart = msgJson.IndexOf('{', arrayStart);
            if (embedStart < 0) return null;
            
            string embedJson = ExtractJsonObject(msgJson, embedStart);
            if (string.IsNullOrEmpty(embedJson)) return null;
            
            // Extract title and description
            string title = ExtractJsonString(embedJson, "\"title\"");
            string description = ExtractJsonString(embedJson, "\"description\"");
            
            var parts = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(title)) parts.Add(title);
            if (!string.IsNullOrEmpty(description)) parts.Add(description);
            
            return parts.Count > 0 ? string.Join("\n", parts.ToArray()) : null;
        }
        
        // ====================================================================
        // Discord → Game Chat Relay (Inbound Polling)
        // ====================================================================
        
        /// <summary>
        /// Start polling Discord channel for new messages and relaying them to game chat.
        /// Called from BroadcastMessageModule.Init() on the server.
        /// </summary>
        public static void StartPolling()
        {
            if (_pollingActive) return;
            
            string token = Config.DiscordBotToken.Value;
            string channelId = Config.DiscordChannelId.Value;
            
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(channelId))
            {
                RLog.Warning("[DiscordBridge] Cannot start polling — token or channel not configured");
                return;
            }
            
            _pollCts = new CancellationTokenSource();
            _pollingActive = true;
            
            // Get our own bot user ID first, then start polling
            _ = InitAndPollAsync(token, channelId, _pollCts.Token);
            RLog.Msg("[DiscordBridge] Discord → Game chat polling started (5s interval)");
        }
        
        /// <summary>
        /// Stop polling Discord for messages.
        /// </summary>
        public static void StopPolling()
        {
            _pollingActive = false;
            _pollCts?.Cancel();
            _pollCts?.Dispose();
            _pollCts = null;
            RLog.Msg("[DiscordBridge] Discord → Game chat polling stopped");
        }
        
        /// <summary>
        /// Initialize bot user ID and start the polling loop.
        /// </summary>
        private static async Task InitAndPollAsync(string token, string channelId, CancellationToken ct)
        {
            try
            {
                // Get our bot's user ID so we can filter out our own messages
                await ResolveBotUserId(token);
                
                // Seed the lastMessageId to the most recent message so we don't replay history
                await SeedLastMessageId(token, channelId);
                
                // Start polling loop
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(5000, ct);  // 5 second interval
                        await PollMessages(token, channelId);
                    }
                    catch (TaskCanceledException) { break; }
                    catch (Exception ex)
                    {
                        RLog.Warning($"[DiscordBridge] Poll error: {ex.Message}");
                        await Task.Delay(10000, ct);  // Back off on error
                    }
                }
            }
            catch (TaskCanceledException) { }
            catch (Exception ex)
            {
                RLog.Warning($"[DiscordBridge] Polling init failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Resolve our bot's user ID via GET /users/@me
        /// </summary>
        private static async Task ResolveBotUserId(string token)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/v9/users/@me"))
                {
                    request.Headers.Add("Authorization", $"Bot {token}");
                    var response = await _client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string body = await response.Content.ReadAsStringAsync();
                        // Simple JSON parse for "id":"..."
                        int idIdx = body.IndexOf("\"id\"");
                        if (idIdx >= 0)
                        {
                            int start = body.IndexOf('"', idIdx + 4) + 1;
                            int end = body.IndexOf('"', start);
                            _botUserId = body.Substring(start, end - start);
                            RLog.Msg($"[DiscordBridge] Bot user ID: {_botUserId}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DiscordBridge] Failed to get bot user ID: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Seed lastMessageId to the most recent message in the channel.
        /// This prevents replaying old messages on startup.
        /// </summary>
        private static async Task SeedLastMessageId(string token, string channelId)
        {
            try
            {
                string url = $"https://discord.com/api/v9/channels/{channelId}/messages?limit=1";
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    request.Headers.Add("Authorization", $"Bot {token}");
                    var response = await _client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string body = await response.Content.ReadAsStringAsync();
                        // Parse the first message's ID
                        int idIdx = body.IndexOf("\"id\"");
                        if (idIdx >= 0)
                        {
                            int start = body.IndexOf('"', idIdx + 4) + 1;
                            int end = body.IndexOf('"', start);
                            _lastMessageId = body.Substring(start, end - start);
                            RLog.Msg($"[DiscordBridge] Seeded last message ID: {_lastMessageId}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DiscordBridge] Failed to seed last message ID: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Poll for new messages since lastMessageId and relay them to game chat.
        /// </summary>
        private static int _pollCount;
        
        private static async Task PollMessages(string token, string channelId)
        {
            try
            {
                _pollCount++;
                string url = string.IsNullOrEmpty(_lastMessageId)
                    ? $"https://discord.com/api/v9/channels/{channelId}/messages?limit=10"
                    : $"https://discord.com/api/v9/channels/{channelId}/messages?after={_lastMessageId}&limit=10";
                
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    request.Headers.Add("Authorization", $"Bot {token}");
                    var response = await _client.SendAsync(request);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        RLog.Warning($"[DiscordBridge] Poll #{_pollCount} HTTP {response.StatusCode}");
                        return;
                    }
                    
                    string body = await response.Content.ReadAsStringAsync();
                    
                    // Log every 20th poll or when messages found (to avoid spam)
                    bool verbose = (_pollCount % 20 == 1);
                    if (verbose)
                        RLog.Msg($"[DiscordBridge] Poll #{_pollCount}: lastId={_lastMessageId}, bodyLen={body?.Length ?? 0}");
                    
                    // Simple JSON array parsing — Discord returns messages newest first
                    var messages = ParseMessages(body);
                    
                    if (messages.Count > 0)
                        RLog.Msg($"[DiscordBridge] Poll #{_pollCount}: {messages.Count} message(s) found");
                    
                    // Process in chronological order (reverse since API returns newest first)
                    int relayed = 0;
                    for (int i = messages.Count - 1; i >= 0; i--)
                    {
                        var msg = messages[i];
                        
                        // Log each message for debugging
                        RLog.Msg($"[DiscordBridge]   msg[{i}] id={msg.Id} author={msg.AuthorName}({msg.AuthorId}) bot={msg.IsBot} content='{msg.Content}'");
                        
                        // Skip bot messages (avoid echo loop)
                        // BUT allow bot messages tagged with [WELCOME] — these are our intentional welcome posts
                        bool isWelcome = false;
                        if (msg.IsBot)
                        {
                            if (!string.IsNullOrEmpty(msg.Content) && msg.Content.StartsWith("[WELCOME]"))
                            {
                                RLog.Msg($"[DiscordBridge]   -> Bot message with [WELCOME] tag — allowing relay");
                                // Strip the [WELCOME] tag before relaying
                                msg.Content = msg.Content.Substring("[WELCOME] ".Length).Trim();
                                isWelcome = true;
                            }
                            else
                            {
                                RLog.Msg($"[DiscordBridge]   -> Skipped (bot)");
                                continue;
                            }
                        }
                        
                        // Skip our own bot messages (but not welcome messages we posted)
                        if (!isWelcome && !string.IsNullOrEmpty(_botUserId) && msg.AuthorId == _botUserId)
                        {
                            RLog.Msg($"[DiscordBridge]   -> Skipped (self)");
                            continue;
                        }
                        
                        // Relay to game chat
                        if (isWelcome)
                        {
                            // Welcome messages: relay each line with [Server] attribution
                            string[] lines = msg.Content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string line in lines)
                            {
                                string trimmed = line.Trim();
                                if (!string.IsNullOrEmpty(trimmed))
                                    RelayToGameChat("Server", trimmed);
                            }
                        }
                        else
                        {
                            RelayToGameChat(msg.AuthorName, msg.Content);
                        }
                        relayed++;
                    }
                    
                    if (relayed > 0)
                        RLog.Msg($"[DiscordBridge] Relayed {relayed} message(s) to game chat");
                    
                    // Update lastMessageId to the newest message
                    if (messages.Count > 0)
                    {
                        _lastMessageId = messages[0].Id;  // First element is newest
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DiscordBridge] PollMessages failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Relay a Discord message to the in-game chat.
        /// </summary>
        private static void RelayToGameChat(string author, string content)
        {
            if (string.IsNullOrEmpty(content)) return;
            
            string formatted = $"[Discord] {author}: {content}";
            RLog.Msg($"[DiscordBridge] Relaying: {formatted}");
            
#if SERVER
            // On the dedicated server, broadcast via ChatBox.SendLine (reaches all players)
            DedicatedSuperuser.Utility.ChatResponse.SendLine($"[Discord] {author}: {content}");
#elif OWNER
            // On Owner, display locally via ChatBox.AddLine
            InGameChat.SendMessage(formatted, true);
#endif
        }
        
        /// <summary>
        /// Parse Discord API JSON response into message objects.
        /// Discord returns a JSON array: [{msg1},{msg2},...] with newest first.
        /// We split at top-level array elements by tracking brace depth.
        /// </summary>
        private static System.Collections.Generic.List<DiscordMessage> ParseMessages(string json)
        {
            var result = new System.Collections.Generic.List<DiscordMessage>();
            if (string.IsNullOrEmpty(json) || json.Length < 5) return result;
            
            // Log raw body on first poll for diagnostics
            if (_pollCount <= 2)
            {
                string preview = json.Length > 500 ? json.Substring(0, 500) + "..." : json;
                RLog.Msg($"[DiscordBridge] Raw API body (first {Math.Min(json.Length, 500)} chars): {preview}");
            }
            
            // The response is a JSON array: [{...},{...},...]
            // Split into top-level objects by tracking brace depth
            var messageJsons = SplitJsonArray(json);
            
            foreach (string msgJson in messageJsons)
            {
                try
                {
                    var msg = new DiscordMessage();
                    
                    // Extract the MESSAGE's own "id" — it's the first "id" in the top-level object
                    msg.Id = ExtractJsonString(msgJson, "\"id\"");
                    msg.Content = ExtractJsonString(msgJson, "\"content\"");
                    
                    // Extract author info (nested "author":{...} object)
                    int authorIdx = msgJson.IndexOf("\"author\"");
                    if (authorIdx >= 0)
                    {
                        // Find the author sub-object using brace matching
                        int authorObjStart = msgJson.IndexOf('{', authorIdx);
                        if (authorObjStart >= 0)
                        {
                            string authorJson = ExtractJsonObject(msgJson, authorObjStart);
                            if (authorJson != null)
                            {
                                msg.AuthorName = ExtractJsonString(authorJson, "\"global_name\"");
                                if (string.IsNullOrEmpty(msg.AuthorName))
                                    msg.AuthorName = ExtractJsonString(authorJson, "\"username\"");
                                msg.AuthorId = ExtractJsonString(authorJson, "\"id\"");
                                
                                // Check if author is a bot
                                msg.IsBot = authorJson.Contains("\"bot\":true");
                            }
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(msg.Id))
                        result.Add(msg);
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[DiscordBridge] Parse error for message: {ex.Message}");
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Split a JSON array string into its top-level element strings.
        /// Input: "[{...},{...},{...}]"
        /// Output: List of "{...}" strings
        /// </summary>
        private static System.Collections.Generic.List<string> SplitJsonArray(string json)
        {
            var elements = new System.Collections.Generic.List<string>();
            
            // Find the opening bracket
            int arrayStart = json.IndexOf('[');
            if (arrayStart < 0) return elements;
            
            int pos = arrayStart + 1;
            while (pos < json.Length)
            {
                // Skip whitespace and commas
                while (pos < json.Length && (json[pos] == ' ' || json[pos] == ',' || json[pos] == '\n' || json[pos] == '\r' || json[pos] == '\t'))
                    pos++;
                
                if (pos >= json.Length || json[pos] == ']') break;
                
                if (json[pos] == '{')
                {
                    // Track brace depth to find the end of this object
                    int objStart = pos;
                    int depth = 0;
                    bool inString = false;
                    bool escaped = false;
                    
                    for (int i = pos; i < json.Length; i++)
                    {
                        char c = json[i];
                        
                        if (escaped) { escaped = false; continue; }
                        if (c == '\\') { escaped = true; continue; }
                        
                        if (c == '"') { inString = !inString; continue; }
                        if (inString) continue;
                        
                        if (c == '{') depth++;
                        else if (c == '}')
                        {
                            depth--;
                            if (depth == 0)
                            {
                                elements.Add(json.Substring(objStart, i - objStart + 1));
                                pos = i + 1;
                                break;
                            }
                        }
                    }
                    
                    if (depth != 0) break; // malformed
                }
                else
                {
                    pos++; // skip unexpected character
                }
            }
            
            return elements;
        }
        
        /// <summary>
        /// Extract a JSON sub-object starting at the given brace position, handling nested braces.
        /// </summary>
        private static string ExtractJsonObject(string json, int braceStart)
        {
            int depth = 0;
            bool inString = false;
            bool escaped = false;
            
            for (int i = braceStart; i < json.Length; i++)
            {
                char c = json[i];
                
                if (escaped) { escaped = false; continue; }
                if (c == '\\') { escaped = true; continue; }
                
                if (c == '"') { inString = !inString; continue; }
                if (inString) continue;
                
                if (c == '{') depth++;
                else if (c == '}')
                {
                    depth--;
                    if (depth == 0)
                        return json.Substring(braceStart, i - braceStart + 1);
                }
            }
            
            return null;
        }
        
        /// <summary>
        /// Extract a string value from a JSON key.
        /// </summary>
        private static string ExtractJsonString(string json, string key)
        {
            int keyIdx = json.IndexOf(key);
            if (keyIdx < 0) return null;
            
            // Find the colon after the key
            int colonIdx = json.IndexOf(':', keyIdx + key.Length);
            if (colonIdx < 0) return null;
            
            // Find the opening quote
            int start = json.IndexOf('"', colonIdx) + 1;
            if (start <= 0) return null;
            
            // Find the closing quote (handle escaped quotes)
            int end = start;
            while (end < json.Length)
            {
                if (json[end] == '"' && json[end - 1] != '\\') break;
                end++;
            }
            
            if (end >= json.Length) return null;
            
            return json.Substring(start, end - start)
                .Replace("\\n", "\n")
                .Replace("\\\"", "\"")
                .Replace("\\\\", "\\");
        }
        
        private class DiscordMessage
        {
            public string Id;
            public string Content;
            public string AuthorName;
            public string AuthorId;
            public bool IsBot;
        }

        // ====================================================================
        // HTTP Helpers
        // ====================================================================

        private static async Task<bool> PostMessageAsync(string token, string channelId, string content)
        {
            try
            {
                string url = $"https://discord.com/api/v9/channels/{channelId}/messages";
                string escapedContent = EscapeJson(content);
                string jsonBody = $"{{\"content\": \"{escapedContent}\"}}";

                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Headers.Add("Authorization", $"Bot {token}");
                    request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    var response = await _client.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        RLog.Warning($"[DiscordBridge] Failed to send message: {response.StatusCode} - {responseBody}");
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DiscordBridge] Error sending message: {ex.Message}");
                return false;
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

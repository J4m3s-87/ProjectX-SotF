using System;
using System.Collections.Generic;
using RedLoader;

namespace ProjectX.Master.Modules.BroadcastMessage
{
    /// <summary>
    /// Handles player join/leave events: posts to Discord and triggers welcome messages.
    /// Uses main-thread dispatch (via Tick) because Harmony hooks fire on background threads.
    /// </summary>
    public static class WelcomeHandler
    {
        // Pending welcome messages — delayed to let player finish loading
        private static readonly List<PendingWelcome> _pendingWelcomes = new List<PendingWelcome>();
        private static readonly object _lock = new object();
        private const float WELCOME_DELAY_SECONDS = 60f; // Player needs ~60s to load into game
        
        // Per-session tracking — each player gets ONE welcome per server session
        private static readonly HashSet<string> _welcomedPlayers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        
        private class PendingWelcome
        {
            public object ConnectToken;  // Raw token — name resolved in Tick() (IL2CPP cross-type invoke fails in Harmony postfix)
            public string PlayerName;    // Resolved at connect time for dedup + disconnect cancellation
            public float SendAfter;      // Time.time when player should be loaded
        }
        
        /// <summary>
        /// Register Harmony hooks for player connect/disconnect.
        /// Called from BroadcastMessageModule.Init()
        /// </summary>
        public static void Register()
        {
            try
            {
                // Hook CoopServerCallbacks.Connected for player joins
                HookConnected();
                
                // Hook CoopServerCallbacks.Disconnected for player leaves
                HookDisconnected();
                
                // Hook death events
                HookPlayerDeath();
            }
            catch (Exception ex)
            {
                RLog.Warning($"[WelcomeHandler] Register failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Main-thread tick — processes pending welcome messages.
        /// Called from MeatDryerModule.OnSeasonsLateUpdate() (server) or update loop.
        /// </summary>
        public static void Tick()
        {
            if (_pendingWelcomes.Count == 0) return;
            
            float now = UnityEngine.Time.time;
            List<PendingWelcome> readyToSend = null;
            
            lock (_lock)
            {
                for (int i = _pendingWelcomes.Count - 1; i >= 0; i--)
                {
                    if (now >= _pendingWelcomes[i].SendAfter)
                    {
                        if (readyToSend == null) readyToSend = new List<PendingWelcome>();
                        readyToSend.Add(_pendingWelcomes[i]);
                        _pendingWelcomes.RemoveAt(i);
                    }
                }
            }
            
            if (readyToSend == null) return;
            
            foreach (var pending in readyToSend)
            {
                try
                {
                    if (Config.EnableWelcomeMessage?.Value == true)
                    {
                        string playerName = pending.PlayerName ?? ResolveTokenName(pending.ConnectToken);
                        
                        // Per-session dedup — skip if already welcomed
                        if (_welcomedPlayers.Contains(playerName))
                        {
                            RLog.Msg($"[WelcomeHandler] Skipping duplicate welcome for {playerName} (already welcomed this session)");
                            continue;
                        }
                        
                        _welcomedPlayers.Add(playerName);
                        
                        // Send welcome directly to game chat (no Discord roundtrip)
                        // The old FetchAndRelayWelcome posted [WELCOME] to Discord chat channel,
                        // then the polling loop relayed it back — causing echo/spam.
                        SendInGameWelcome(playerName);
                        
                        // Post Discord embed separately (notification only, not relayed back)
                        DiscordBridge.SendPlayerJoin(playerName, isFirstTime: true);
                        
                        RLog.Msg($"[WelcomeHandler] Welcome sent to game chat + Discord for {playerName} (after {WELCOME_DELAY_SECONDS}s load delay)");
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[WelcomeHandler] Welcome send failed: {ex.Message}");
                }
            }
        }
        
        // ====================================================================
        // Direct In-Game Welcome (no Discord roundtrip)
        // ====================================================================
        
        /// <summary>
        /// Send welcome messages directly to game chat.
        /// Previously this went through Discord (FetchAndRelayWelcome → post [WELCOME] to
        /// chat channel → polling → relay) which caused echo/spam. Now sends directly.
        /// </summary>
        private static void SendInGameWelcome(string playerName)
        {
            try
            {
                string[] lines = new[]
                {
                    $">>> Welcome to the server, {playerName}! <<<",
                    "WELCOME TO PROJECT X",
                    "Please visit the discord to download the Mod Pack and learn more!  https://discord.gg/yMHU3hQt"
                };
                
                foreach (string line in lines)
                {
#if SERVER
                    DedicatedSuperuser.Utility.ChatResponse.SendLine($"[ProjectX] [Discord] Server: {line}");
#elif OWNER
                    InGameChat.SendMessage($"[ProjectX] [Discord] Server: {line}", true);
#endif
                }
                
                RLog.Msg($"[WelcomeHandler] Sent {lines.Length} welcome lines directly to game chat for {playerName}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[WelcomeHandler] SendInGameWelcome failed: {ex.Message}");
            }
        }
        
        // ====================================================================
        // Name Resolution (main thread only — IL2CPP cross-type invoke fails in Harmony postfix)
        // ====================================================================
        
        /// <summary>
        /// Resolves player name from a CoopConnectToken via direct memory read.
        /// 
        /// IL2CPP cross-type invoke fails with "Object does not match target type" —
        /// the token is typed as IProtocolToken (interface), not CoopConnectToken (concrete).
        /// Solution: Il2CppObjectBase.Pointer + Marshal.ReadIntPtr (same pattern as
        /// WaterCollectorsModule / MeatDryerModule).
        /// 
        /// From dump.cs: CoopConnectToken.PlayerName is at offset 0x10
        /// </summary>
        private static string ResolveTokenName(object token)
        {
            if (token == null) return "Unknown";
            
            try
            {
                var il2cppObj = (Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)token;
                var ptr = il2cppObj.Pointer;
                if (ptr == System.IntPtr.Zero) return "Unknown";
                
                // CoopConnectToken.PlayerName at offset 0x10 (from dump.cs)
                System.IntPtr strPtr = System.Runtime.InteropServices.Marshal.ReadIntPtr(ptr + 0x10);
                if (strPtr == System.IntPtr.Zero) return "Unknown";
                
                string name = Il2CppInterop.Runtime.IL2CPP.Il2CppStringToManaged(strPtr);
                if (!string.IsNullOrEmpty(name))
                    return name;
            }
            catch (Exception ex)
            {
                RLog.Warning($"[WelcomeHandler] ResolveTokenName: {ex.GetType().Name}: {ex.Message}");
            }
            
            return "Unknown";
        }
        
        // ====================================================================
        // Harmony Hooks
        // ====================================================================
        
        private static void HookConnected()
        {
            try
            {
                // Find CoopServerCallbacks.Connected method via reflection
                var callbacksType = HarmonyLib.AccessTools.TypeByName("CoopServerCallbacks");
                if (callbacksType == null)
                {
                    RLog.Warning("[WelcomeHandler] Could not find CoopServerCallbacks type");
                    return;
                }
                
                var connectedMethod = callbacksType.GetMethod("Connected",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                
                if (connectedMethod == null)
                {
                    RLog.Warning("[WelcomeHandler] Could not find Connected method");
                    return;
                }
                
                var harmonyInstance = new HarmonyLib.Harmony("ProjectX.WelcomeHandler.Connected");
                var postfix = typeof(WelcomeHandler).GetMethod(nameof(OnPlayerConnected),
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                
                harmonyInstance.Patch(connectedMethod, postfix: new HarmonyLib.HarmonyMethod(postfix));
                RLog.Msg("[WelcomeHandler] Hooked CoopServerCallbacks.Connected (main-thread dispatch)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[WelcomeHandler] Failed to hook Connected: {ex.Message}");
            }
        }
        
        private static void HookDisconnected()
        {
            try
            {
                var callbacksType = HarmonyLib.AccessTools.TypeByName("CoopServerCallbacks");
                if (callbacksType == null) return;
                
                var disconnectedMethod = callbacksType.GetMethod("Disconnected",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                
                if (disconnectedMethod == null)
                {
                    RLog.Warning("[WelcomeHandler] Could not find Disconnected method");
                    return;
                }
                
                var harmonyInstance = new HarmonyLib.Harmony("ProjectX.WelcomeHandler.Disconnected");
                var postfix = typeof(WelcomeHandler).GetMethod(nameof(OnPlayerDisconnected),
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                
                harmonyInstance.Patch(disconnectedMethod, postfix: new HarmonyLib.HarmonyMethod(postfix));
                RLog.Msg("[WelcomeHandler] Hooked CoopServerCallbacks.Disconnected");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[WelcomeHandler] Failed to hook Disconnected: {ex.Message}");
            }
        }
        
        private static void HookPlayerDeath()
        {
            try
            {
                // Attempt to hook death events - known limitation with IL2CPP
                // Multiple candidates have been tried; currently no viable hook found
                RLog.Msg("[WelcomeHandler] No death hook candidate found — death events will not post to Discord");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[WelcomeHandler] Death hook failed: {ex.Message}");
            }
        }
        
        // ====================================================================
        // Harmony Postfixes
        // ====================================================================
        
        /// <summary>
        /// Called when a player connects. Resolves player name from ConnectToken,
        /// queues a welcome message, and posts join notification to Discord.
        /// </summary>
        private static void OnPlayerConnected(object __instance, object connection)
        {
            try
            {
                object token = null;
                
                // Grab ConnectToken — don't read fields here (IL2CPP cross-type invoke fails).
                // Store raw token, resolve name in Tick() via direct memory read.
                try
                {
                    var connType = connection?.GetType();
                    if (connType != null)
                    {
                        var getToken = connType.GetMethod("get_ConnectToken",
                            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                        if (getToken != null)
                            token = getToken.Invoke(connection, null);
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[WelcomeHandler] Token grab failed: {ex.Message}");
                }
                
                // Resolve name now for join log (direct memory read works in any context)
                string playerName = ResolveTokenName(token);
                
                float sendAfter = UnityEngine.Time.time + WELCOME_DELAY_SECONDS;
                RLog.Msg($"[WelcomeHandler] {playerName} connected — welcome in {WELCOME_DELAY_SECONDS}s");
                
                lock (_lock)
                {
                    _pendingWelcomes.Add(new PendingWelcome
                    {
                        ConnectToken = token,
                        PlayerName = playerName,
                        SendAfter = sendAfter
                    });
                }
                
                BroadcastMessageModule.LogEvent("join", playerName);
                
                // Track player count for server idle guard
                ProjectXMaster.OnPlayerConnected();
                
                // Anti-cheat: trigger integrity check on the connecting player
#if SERVER || OWNER
                try
                {
                    Network.IntegrityEvent.TryRequestCheck(connection);
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[WelcomeHandler] IntegrityCheck trigger failed: {ex.Message}");
                }
                
                // Track connection for ConfigSync broadcasts (Owner doesn't send config requests)
                try
                {
                    Network.ConfigSyncEvent.TrackConnection(connection);
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[WelcomeHandler] ConfigSync track failed: {ex.Message}");
                }
#endif
            }
            catch (Exception ex)
            {
                RLog.Warning($"[WelcomeHandler] OnPlayerConnected error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Called when a player disconnects. Posts leave notification to Discord.
        /// Uses same ConnectToken + direct memory read as connect handler.
        /// </summary>
        private static void OnPlayerDisconnected(object __instance, object connection)
        {
            try
            {
                string playerName = "Unknown";
                
                try
                {
                    var connType = connection?.GetType();
                    if (connType != null)
                    {
                        var getToken = connType.GetMethod("get_ConnectToken",
                            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                        if (getToken != null)
                        {
                            var token = getToken.Invoke(connection, null);
                            playerName = ResolveTokenName(token);
                        }
                    }
                }
                catch { }
                
                RLog.Msg($"[WelcomeHandler] {playerName} disconnected");
                
                // Cancel any pending welcome for this player (prevents duplicate on reconnect)
                lock (_lock)
                {
                    int removed = _pendingWelcomes.RemoveAll(p => 
                        string.Equals(p.PlayerName, playerName, StringComparison.OrdinalIgnoreCase));
                    if (removed > 0)
                        RLog.Msg($"[WelcomeHandler] Cancelled {removed} pending welcome(s) for {playerName}");
                }
                
                BroadcastMessageModule.LogEvent("leave", playerName);
                
                // Track player count for server idle guard
                ProjectXMaster.OnPlayerDisconnected();
            }
            catch (Exception ex)
            {
                RLog.Warning($"[WelcomeHandler] OnPlayerDisconnected error: {ex.Message}");
            }
        }
    }
}

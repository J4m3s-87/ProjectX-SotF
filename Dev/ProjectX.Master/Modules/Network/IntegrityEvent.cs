using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using Bolt;
using RedLoader;
using SonsSdk;
using SonsSdk.Networking;
using UdpKit;

namespace ProjectX.Master.Modules.Network
{
    /// <summary>
    /// IntegrityEvent — Anti-cheat network event implementing a 3-way handshake.
    /// 
    /// Protocol (Hybrid — based on AdminCommandEvent pattern):
    ///   Server → Client: Packets.NetEvent 0x10 REQUEST_CHECK (proven by PermissionEvent)
    ///   Client → Server: ChatBox.SendLine("/px integrity mod1,mod2|hash") (Pattern #15)
    ///   Server → Client: Packets.NetEvent 0x12 CHECK_RESULT pass/fail (proven)
    /// 
    /// Why hybrid:
    ///   ❌ Packets.NetEvent client→server is silently dropped on dedicated servers
    ///      (documented in technical_retrospective.md and AdminCommandEvent.cs)
    ///   ✅ Packets.NetEvent server→client works (PermissionEvent, ConfigSyncEvent)
    ///   ✅ ChatBox.SendLine client→server works (AdminCommandEvent, Pattern #15)
    /// </summary>
    public class IntegrityEvent : Packets.NetEvent
    {
        public static IntegrityEvent Instance;
        
        public override string Id => "ProjectX.IntegrityCheck";
        
#if !SERVER
        // Retry mechanism: if ChatBox isn't available during loading, queue the response
        private static string _pendingResponseMessage = null;
        private static float _pendingResponseTime = 0f;
        private static int _pendingRetryCount = 0;
        private const int MAX_RETRIES = 30; // ~30 seconds at 1 retry/sec
        private static float _lastRetryTime = 0f;
#endif
        
#if SERVER || OWNER
        // Track pending checks: ConnectionId → (timestamp, connection reference)
        private static readonly Dictionary<uint, PendingCheck> _pendingChecks = new Dictionary<uint, PendingCheck>();
        
        // Deferred check requests — WelcomeHandler fires on background thread,
        // but Bolt operations must run on main thread
        private static readonly List<BoltConnection> _deferredRequests = new List<BoltConnection>();
        private static readonly object _deferLock = new object();
        
        private class PendingCheck
        {
            public float RequestTime;
            public BoltConnection Connection;
        }
#endif
        
        // ==================== REGISTRATION ====================
        
        /// <summary>
        /// Register the event with SonsSdk networking.
        /// </summary>
        public static void Register()
        {
            Instance = new IntegrityEvent();
            Packets.Register(Instance);
            RLog.Msg(Color.Cyan, "[IntegrityEvent] Registered");
        }
        
#if SERVER || OWNER
        /// <summary>
        /// Static helper for callers that can't reference BoltConnection directly.
        /// Defers the request to main thread since WelcomeHandler fires on a background thread.
        /// 
        /// NOTE: Uses try-catch cast instead of 'is' operator because
        /// C# is/as operators DO NOT WORK on IL2CPP types (Pattern #14).
        /// </summary>
        public static void TryRequestCheck(object connection)
        {
            if (Instance == null) return;
            
            try
            {
                // Direct cast — IL2CPP 'is' operator always returns false (Pattern #14)
                var boltConn = (BoltConnection)connection;
                if (boltConn != null)
                {
                    lock (_deferLock)
                    {
                        _deferredRequests.Add(boltConn);
                    }
                    RLog.Msg(Color.Cyan, "[IntegrityEvent] Queued integrity check for main-thread dispatch");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[IntegrityEvent] TryRequestCheck: cast failed — {ex.GetType().Name}: {ex.Message}");
            }
        }
#endif
        
        // ==================== PACKET ROUTING ====================
        
        /// <summary>
        /// Handle received packet — dispatch to server or client handler.
        /// Note: Only server→client packets arrive here (0x10 REQUEST, 0x12 RESULT).
        /// Client→Server responses come via ChatEvent, not NetEvent.
        /// </summary>
        public override void Read(UdpPacket packet, BoltConnection fromConnection)
        {
            if (BoltNetwork.isServer)
            {
                // Server should NOT receive NetEvent from clients (they use ChatBox.SendLine)
                // but handle gracefully in case it arrives
                ReadMessageServer(packet, fromConnection);
            }
            else
            {
                ReadMessageClient(packet, fromConnection);
            }
        }
        
        // ==================== SERVER SIDE ====================
        
#if SERVER || OWNER
        /// <summary>
        /// [Server] Handle incoming integrity response from client via NetEvent.
        /// This is a legacy path — clients should use ChatBox.SendLine instead.
        /// Kept for Owner edition where NetEvent client→server may still work.
        /// </summary>
        private void ReadMessageServer(UdpPacket packet, BoltConnection fromConnection)
        {
            try
            {
                byte msgType = packet.ReadByte();
                
                if (msgType == 0x11) // CHECK_RESPONSE from client
                {
                    HandleCheckResponse(packet, fromConnection);
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[IntegrityEvent] Server read error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// [Server] Send integrity check request to a connecting client.
        /// Uses Packets.NetEvent server→client (proven working by PermissionEvent).
        /// Must be called from the main thread (use TryRequestCheck from background threads).
        /// Returns false if Bolt isn't ready yet (caller should re-queue).
        /// </summary>
        public bool RequestIntegrityCheck(BoltConnection connection)
        {
            try
            {
                if (!IntegrityConfig.Enabled.Value)
                {
                    RLog.Msg("[IntegrityEvent] Anti-cheat disabled, skipping check");
                    return true; // Don't re-queue — intentionally skipped
                }
                
                if (connection == null)
                {
                    RLog.Warning("[IntegrityEvent] RequestIntegrityCheck: connection is null");
                    return true; // Don't re-queue — invalid
                }
                
                if (!BoltNetwork.isRunning)
                {
                    RLog.Msg(Color.Yellow, "[IntegrityEvent] RequestIntegrityCheck: Bolt not running yet — will retry");
                    return false; // Re-queue — Bolt not ready
                }
                
                uint connId = connection.ConnectionId;
                _pendingChecks[connId] = new PendingCheck
                {
                    RequestTime = UnityEngine.Time.time,
                    Connection = connection
                };
                
                var eventPacket = NewPacket(32, connection);
                eventPacket.Packet.WriteByte(0x10); // REQUEST_CHECK
                
                Send(eventPacket);
                
                string steamId = "unknown";
                try { steamId = connection.RemoteEndPoint.SteamId.ToString(); } catch { }
                
                RLog.Msg(Color.Cyan, $"[IntegrityEvent] Sent integrity check request to {steamId} (conn={connId})");
                return true;
            }
            catch (Exception ex)
            {
                RLog.Warning($"[IntegrityEvent] Request error: {ex.Message}");
                return true; // Don't re-queue on exception — something is fundamentally wrong
            }
        }
        
        /// <summary>
        /// [Server] Handle integrity response received via ChatEvent ("/px integrity ...").
        /// Called from DedicatedSuperuserModule.OnChatEventReceived when it detects
        /// a "/px integrity" message from a client.
        /// 
        /// Message format: "/px integrity mod1,mod2,mod3|HASHVALUE"
        /// </summary>
        public static void HandleChatResponse(string message, object senderEvnt)
        {
            if (Instance == null) return;
            if (!IntegrityConfig.Enabled.Value) return;
            
            try
            {
                // Parse the message: "/px integrity mod1,mod2|hash"
                string payload = message.Substring("/px integrity ".Length).Trim();
                
                // Split on | to separate mods from hash
                string modsStr;
                string clientHash;
                int pipeIdx = payload.LastIndexOf('|');
                if (pipeIdx >= 0)
                {
                    modsStr = payload.Substring(0, pipeIdx);
                    clientHash = payload.Substring(pipeIdx + 1);
                }
                else
                {
                    modsStr = payload;
                    clientHash = "NONE";
                }
                
                // Parse mod list
                var clientMods = new List<string>();
                if (!string.IsNullOrEmpty(modsStr))
                {
                    foreach (var mod in modsStr.Split(','))
                    {
                        string trimmed = mod.Trim();
                        if (!string.IsNullOrEmpty(trimmed))
                            clientMods.Add(trimmed);
                    }
                }
                
                // Try to find the sender's connection to match against pending checks
                // and to perform enforcement (kick).
                // On dedicated server, we try to match by finding who has a pending check.
                BoltConnection senderConnection = null;
                uint matchedConnId = 0;
                
                // Try to get the sender's connection from the ChatEvent via RaisedBy
                try
                {
                    if (senderEvnt != null)
                    {
                        var raisedByProp = senderEvnt.GetType().GetProperty("RaisedBy");
                        if (raisedByProp != null)
                        {
                            var raisedBy = raisedByProp.GetValue(senderEvnt);
                            if (raisedBy != null)
                            {
                                // raisedBy is a BoltConnection
                                senderConnection = (BoltConnection)raisedBy;
                                matchedConnId = senderConnection.ConnectionId;
                                RLog.Msg(Color.Cyan, $"[IntegrityEvent] Got sender connection from ChatEvent: conn={matchedConnId}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[IntegrityEvent] Could not get sender from ChatEvent: {ex.Message}");
                }
                
                // Fallback: if we couldn't get the connection from ChatEvent,
                // match against pending checks (first pending connection)
                if (senderConnection == null && _pendingChecks.Count > 0)
                {
                    // Use the first pending check as a best guess
                    foreach (var kvp in _pendingChecks)
                    {
                        matchedConnId = kvp.Key;
                        senderConnection = kvp.Value.Connection;
                        RLog.Msg(Color.Yellow, $"[IntegrityEvent] Fallback match — using first pending check: conn={matchedConnId}");
                        break;
                    }
                }
                
                string steamId = "unknown";
                try { if (senderConnection != null) steamId = senderConnection.RemoteEndPoint.SteamId.ToString(); } catch { }
                
                // Remove from pending
                if (matchedConnId > 0) _pendingChecks.Remove(matchedConnId);
                
                // Log what the client reported
                RLog.Msg(Color.Cyan, $"[IntegrityEvent] ChatEvent response from {steamId}: {clientMods.Count} mod(s), hash={clientHash.Substring(0, System.Math.Min(16, clientHash.Length))}...");
                foreach (var mod in clientMods)
                {
                    RLog.Msg(Color.Cyan, $"  - {mod}");
                }
                
                // === VALIDATION ===
                var whitelist = IntegrityConfig.GetWhitelistSet();
                var violations = new List<string>();
                
                // Check 1: Look for unauthorized mods
                foreach (var mod in clientMods)
                {
                    if (!whitelist.Contains(mod))
                    {
                        violations.Add($"Unauthorized mod: {mod}");
                    }
                }
                
                // Check 2: Verify DLL hash (if configured)
                string expectedHash = IntegrityConfig.ExpectedClientHash?.Value;
                if (!string.IsNullOrEmpty(expectedHash) && !string.IsNullOrEmpty(clientHash))
                {
                    if (!string.Equals(clientHash, expectedHash, StringComparison.OrdinalIgnoreCase))
                    {
                        violations.Add("Client DLL hash mismatch");
                    }
                }
                
                // === ENFORCE ===
                if (violations.Count > 0)
                {
                    string reason = string.Join("; ", violations);
                    RLog.Msg(Color.Red, $"[IntegrityEvent] FAIL — {steamId}: {reason}");
                    
                    // Send failure result then kick
                    if (senderConnection != null)
                    {
                        Instance.SendCheckResult(senderConnection, false, IntegrityConfig.KickMessage.Value);
                        
                        try
                        {
                            senderConnection.Disconnect();
                            RLog.Msg(Color.Red, $"[IntegrityEvent] Kicked {steamId} (conn={matchedConnId})");
                        }
                        catch (Exception ex)
                        {
                            RLog.Warning($"[IntegrityEvent] Disconnect error: {ex.Message}");
                        }
                    }
                }
                else
                {
                    RLog.Msg(Color.GreenYellow, $"[IntegrityEvent] PASS — {steamId} ({clientMods.Count} authorized mod(s))");
                    if (senderConnection != null)
                    {
                        Instance.SendCheckResult(senderConnection, true, "");
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[IntegrityEvent] ChatEvent response error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// [Server] Process a client's integrity check response via NetEvent (legacy/Owner path).
        /// </summary>
        private void HandleCheckResponse(UdpPacket packet, BoltConnection fromConnection)
        {
            string steamId = "unknown";
            try { steamId = fromConnection.RemoteEndPoint.SteamId.ToString(); } catch { }
            
            uint connId = fromConnection.ConnectionId;
            
            try
            {
                // Read client data
                int modCount = packet.ReadInt();
                var clientMods = new List<string>();
                for (int i = 0; i < modCount && i < 50; i++) // Cap at 50 to prevent abuse
                {
                    clientMods.Add(packet.ReadString());
                }
                string clientHash = packet.ReadString();
                
                // Remove from pending
                _pendingChecks.Remove(connId);
                
                // Log what the client reported
                RLog.Msg(Color.Cyan, $"[IntegrityEvent] NetEvent response from {steamId}: {modCount} mod(s), hash={clientHash.Substring(0, System.Math.Min(16, clientHash.Length))}...");
                foreach (var mod in clientMods)
                {
                    RLog.Msg(Color.Cyan, $"  - {mod}");
                }
                
                // === VALIDATION ===
                var whitelist = IntegrityConfig.GetWhitelistSet();
                var violations = new List<string>();
                
                // Check 1: Look for unauthorized mods
                foreach (var mod in clientMods)
                {
                    if (!whitelist.Contains(mod))
                    {
                        violations.Add($"Unauthorized mod: {mod}");
                    }
                }
                
                // Check 2: Verify DLL hash (if configured)
                string expectedHash = IntegrityConfig.ExpectedClientHash?.Value;
                if (!string.IsNullOrEmpty(expectedHash) && !string.IsNullOrEmpty(clientHash))
                {
                    if (!string.Equals(clientHash, expectedHash, StringComparison.OrdinalIgnoreCase))
                    {
                        violations.Add("Client DLL hash mismatch");
                    }
                }
                
                // === ENFORCE ===
                if (violations.Count > 0)
                {
                    string reason = string.Join("; ", violations);
                    RLog.Msg(Color.Red, $"[IntegrityEvent] FAIL — {steamId}: {reason}");
                    
                    // Send failure result then kick
                    SendCheckResult(fromConnection, false, IntegrityConfig.KickMessage.Value);
                    
                    // Disconnect
                    try
                    {
                        fromConnection.Disconnect();
                        RLog.Msg(Color.Red, $"[IntegrityEvent] Kicked {steamId} (conn={connId})");
                    }
                    catch (Exception ex)
                    {
                        RLog.Warning($"[IntegrityEvent] Disconnect error: {ex.Message}");
                    }
                }
                else
                {
                    RLog.Msg(Color.GreenYellow, $"[IntegrityEvent] PASS — {steamId} ({modCount} authorized mod(s))");
                    SendCheckResult(fromConnection, true, "");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[IntegrityEvent] Validation error for {steamId}: {ex.Message}");
            }
        }
        
        /// <summary>
        /// [Server] Send check result (pass/fail) to client via NetEvent (server→client works).
        /// </summary>
        private void SendCheckResult(BoltConnection connection, bool passed, string reason)
        {
            try
            {
                if (!BoltNetwork.isRunning || connection == null) return;
                
                var eventPacket = NewPacket(256, connection);
                eventPacket.Packet.WriteByte(0x12); // CHECK_RESULT
                eventPacket.Packet.WriteBool(passed);
                eventPacket.Packet.WriteString(reason ?? "");
                
                Send(eventPacket);
            }
            catch (Exception ex)
            {
                RLog.Warning($"[IntegrityEvent] SendResult error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// [Server] Process deferred requests and check for timeouts.
        /// Call from ManagedOnUpdate (main thread).
        /// </summary>
        public static void CheckTimeouts()
        {
            // Process deferred check requests (queued from background threads)
            List<BoltConnection> deferred = null;
            lock (_deferLock)
            {
                if (_deferredRequests.Count > 0)
                {
                    deferred = new List<BoltConnection>(_deferredRequests);
                    _deferredRequests.Clear();
                }
            }
            
            if (deferred != null)
            {
                foreach (var conn in deferred)
                {
                    try
                    {
                        if (Instance == null)
                        {
                            RLog.Warning("[IntegrityEvent] CheckTimeouts: Instance is null!");
                            continue;
                        }
                        
                        bool dispatched = Instance.RequestIntegrityCheck(conn);
                        if (!dispatched)
                        {
                            // Bolt not ready — put it back for next frame
                            lock (_deferLock)
                            {
                                _deferredRequests.Add(conn);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        RLog.Warning($"[IntegrityEvent] Deferred request error: {ex.Message}");
                    }
                }
            }
            
            // Check timeouts
            if (!IntegrityConfig.Enabled.Value) return;
            if (_pendingChecks.Count == 0) return;
            
            float now = UnityEngine.Time.time;
            float grace = IntegrityConfig.GracePeriodSeconds.Value;
            var expired = new List<uint>();
            
            foreach (var kvp in _pendingChecks)
            {
                if (now - kvp.Value.RequestTime > grace)
                {
                    expired.Add(kvp.Key);
                }
            }
            
            foreach (uint connId in expired)
            {
                _pendingChecks.Remove(connId);
                
                // Timeout = no response. This likely means the player has no mods installed
                // (vanilla client) and therefore no IntegrityEvent handler to respond.
                // We ALLOW this — only explicit whitelist violations (unauthorized mods) trigger kicks.
                RLog.Msg(Color.Yellow, $"[IntegrityEvent] No response from conn={connId} within {grace}s — allowing (likely vanilla client)");
            }
        }
        
        /// <summary>
        /// [Server] Remove a connection from pending checks (player disconnected).
        /// </summary>
        public static void OnClientDisconnected(BoltConnection connection)
        {
            try
            {
                _pendingChecks.Remove(connection.ConnectionId);
            }
            catch { }
        }
#else
        // Non-server stub — server-only methods not compiled
        private void ReadMessageServer(UdpPacket packet, BoltConnection fromConnection) { }
#endif
        
        // ==================== CLIENT / OWNER SIDE ====================
        // Compiled for all editions EXCEPT dedicated server
        
#if !SERVER
        /// <summary>
        /// [Client/Owner] Handle incoming messages from server.
        /// Only processes: 0x10 REQUEST_CHECK, 0x12 CHECK_RESULT
        /// </summary>
        private void ReadMessageClient(UdpPacket packet, BoltConnection fromConnection)
        {
            try
            {
                byte msgType = packet.ReadByte();
                RLog.Msg(Color.Cyan, $"[IntegrityEvent] Client received msgType=0x{msgType:X2}");
                
                switch (msgType)
                {
                    case 0x10: // REQUEST_CHECK — server wants our mod list
                        RLog.Msg(Color.Cyan, "[IntegrityEvent] ★ Received REQUEST_CHECK from server");
                        HandleCheckRequest();
                        break;
                        
                    case 0x12: // CHECK_RESULT — pass/fail from server
                        HandleCheckResult(packet);
                        break;
                        
                    default:
                        RLog.Warning($"[IntegrityEvent] Unknown msgType: 0x{msgType:X2}");
                        break;
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[IntegrityEvent] Client read error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// [Client/Owner] Respond to server's integrity check request.
        /// Collects loaded mods via ModTypeBase and hashes own DLL.
        /// 
        /// Sends response via ChatBox.SendLine("/px integrity mod1,mod2|hash")
        /// because Packets.NetEvent client→server is silently dropped on
        /// dedicated servers (Pattern #15 from technical_retrospective.md).
        /// </summary>
        private void HandleCheckRequest()
        {
            try
            {
                RLog.Msg(Color.Cyan, "[IntegrityEvent] ★ Server requested integrity check — collecting mod data...");
                
                // Collect loaded mods (same technique as CoopServerTools)
                var mods = new List<string>();
                try
                {
                    foreach (var mod in ModTypeBase<SonsMod>.RegisteredMods)
                    {
                        if (mod != null)
                        {
                            string modId = mod.ID;
                            if (!string.IsNullOrEmpty(modId))
                            {
                                mods.Add(modId);
                                RLog.Msg(Color.Cyan, $"[IntegrityEvent]   Found mod: {modId}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    RLog.Warning($"[IntegrityEvent] Error enumerating mods: {ex.Message}");
                }
                
                // Hash own DLL
                string dllHash = ComputeOwnDllHash();
                
                RLog.Msg(Color.Cyan, $"[IntegrityEvent] Collected {mods.Count} mod(s), hash={dllHash.Substring(0, System.Math.Min(16, dllHash.Length))}...");
                
                // Build the chat message: "/px integrity mod1,mod2,mod3|HASH"
                string modList = string.Join(",", mods);
                string chatMessage = $"/px integrity {modList}|{dllHash}";
                
                // Try to send immediately
                bool sent = TrySendViaChatBox(chatMessage);
                if (!sent)
                {
                    // ChatBox not available yet (still loading) — queue for retry
                    RLog.Msg(Color.Yellow, "[IntegrityEvent] ChatBox not available — queuing response for retry");
                    _pendingResponseMessage = chatMessage;
                    _pendingResponseTime = UnityEngine.Time.time;
                    _pendingRetryCount = 0;
                    _lastRetryTime = 0f;
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[IntegrityEvent] HandleCheckRequest error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// [Client/Owner] Process pending integrity response — called from ManagedOnUpdate.
        /// Retries sending via ChatBox once per second until it succeeds or max retries reached.
        /// </summary>
        public static void ProcessPendingResponse()
        {
            if (_pendingResponseMessage == null) return;
            
            float now = UnityEngine.Time.time;
            if (now - _lastRetryTime < 1.0f) return; // Throttle to 1 retry/sec
            _lastRetryTime = now;
            _pendingRetryCount++;
            
            if (_pendingRetryCount > MAX_RETRIES)
            {
                RLog.Warning($"[IntegrityEvent] Gave up sending integrity response after {MAX_RETRIES} retries ({now - _pendingResponseTime:F1}s)");
                _pendingResponseMessage = null;
                return;
            }
            
            RLog.Msg(Color.Yellow, $"[IntegrityEvent] Retry #{_pendingRetryCount} sending integrity response...");
            bool sent = TrySendViaChatBox(_pendingResponseMessage);
            if (sent)
            {
                RLog.Msg(Color.GreenYellow, $"[IntegrityEvent] ★ Deferred response sent successfully after {_pendingRetryCount} retries ({now - _pendingResponseTime:F1}s)");
                _pendingResponseMessage = null;
            }
        }
        
        /// <summary>
        /// Send a message via ChatBox.SendLine — the proven client→server path.
        /// Uses the same resolution technique as AdminCommandEvent.
        /// </summary>
        /// <summary>
        /// Try to send a message via ChatBox.SendLine. Returns true if successful, false if ChatBox
        /// not available (caller should retry later).
        /// </summary>
        private static bool TrySendViaChatBox(string message)
        {
            try
            {
                // Resolve ChatBox type
                string[] chatTypeNames = { "Sons.Gui.ChatBox", "TheForest.UI.Multiplayer.ChatBox", "ChatBox" };
                Type chatBoxType = null;
                foreach (var name in chatTypeNames)
                {
                    chatBoxType = HarmonyLib.AccessTools.TypeByName(name);
                    if (chatBoxType != null)
                    {
                        RLog.Msg(Color.Cyan, $"[IntegrityEvent] ChatBox type resolved: {name}");
                        break;
                    }
                }
                
                if (chatBoxType == null)
                {
                    RLog.Warning("[IntegrityEvent] ChatBox type not found — cannot send integrity response");
                    return false;
                }
                
                // Find the ChatBox instance via FindObjectOfType<T> (singular — works in IL2CPP)
                var findMethod = typeof(UnityEngine.Object).GetMethod("FindObjectOfType", Type.EmptyTypes);
                if (findMethod == null)
                {
                    RLog.Warning("[IntegrityEvent] FindObjectOfType method not found on UnityEngine.Object");
                    return false;
                }
                var genericFind = findMethod.MakeGenericMethod(chatBoxType);
                var chatBoxInstance = genericFind.Invoke(null, null);
                
                if (chatBoxInstance == null)
                {
                    RLog.Msg(Color.Yellow, "[IntegrityEvent] ChatBox instance is null — game still loading?");
                    return false; // Caller should retry
                }
                
                // Get SendLine method
                var sendLineMethod = chatBoxType.GetMethod("SendLine", 
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                
                if (sendLineMethod == null)
                {
                    // Log all available methods for diagnostics
                    RLog.Warning("[IntegrityEvent] ChatBox.SendLine method not found. Available methods:");
                    foreach (var m in chatBoxType.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
                    {
                        RLog.Msg($"[IntegrityEvent]   {m.ReturnType.Name} {m.Name}({string.Join(", ", m.GetParameters().Select(p => p.ParameterType.Name))})");
                    }
                    return false;
                }
                
                // Call SendLine with our message
                RLog.Msg(Color.Cyan, $"[IntegrityEvent] Invoking ChatBox.SendLine ({message.Length} chars)...");
                sendLineMethod.Invoke(chatBoxInstance, new object[] { message });
                RLog.Msg(Color.GreenYellow, $"[IntegrityEvent] ★ Integrity response SENT via ChatBox.SendLine");
                return true;
            }
            catch (Exception ex)
            {
                RLog.Warning($"[IntegrityEvent] TrySendViaChatBox error: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// [Client/Owner] Handle the server's pass/fail verdict.
        /// </summary>
        private void HandleCheckResult(UdpPacket packet)
        {
            try
            {
                bool passed = packet.ReadBool();
                string reason = packet.ReadString();
                
                if (passed)
                {
                    RLog.Msg(Color.GreenYellow, "[IntegrityEvent] Integrity check PASSED");
                }
                else
                {
                    RLog.Msg(Color.Red, $"[IntegrityEvent] Integrity check FAILED: {reason}");
                    // Player will be disconnected by the server shortly
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[IntegrityEvent] HandleCheckResult error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Compute SHA256 hash of our own DLL for verification.
        /// </summary>
        private static string ComputeOwnDllHash()
        {
            try
            {
                // Find our own assembly location
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                string location = assembly.Location;
                
                if (string.IsNullOrEmpty(location))
                {
                    // Fallback: search common paths
                    var candidates = new[]
                    {
                        System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Mods", "ProjectX.Client.dll"),
                        System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Mods", "ProjectX.Owner.dll"),
                    };
                    
                    foreach (var candidate in candidates)
                    {
                        if (System.IO.File.Exists(candidate))
                        {
                            location = candidate;
                            break;
                        }
                    }
                }
                
                if (!string.IsNullOrEmpty(location) && System.IO.File.Exists(location))
                {
                    using (var sha256 = SHA256.Create())
                    using (var stream = System.IO.File.OpenRead(location))
                    {
                        byte[] hash = sha256.ComputeHash(stream);
                        return BitConverter.ToString(hash).Replace("-", "");
                    }
                }
                
                return "HASH_UNAVAILABLE";
            }
            catch (Exception ex)
            {
                RLog.Warning($"[IntegrityEvent] Hash computation error: {ex.Message}");
                return "HASH_ERROR";
            }
        }
#else
        // Dedicated server stub — client-only methods not compiled
        private void ReadMessageClient(UdpPacket packet, BoltConnection fromConnection) { }
#endif
    }
}

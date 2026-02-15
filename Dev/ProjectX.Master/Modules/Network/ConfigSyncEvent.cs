using System;
using System.Collections.Generic;
using System.Drawing;
using Bolt;
using RedLoader;
using SonsSdk.Networking;
using UdpKit;

namespace ProjectX.Master.Modules.Network
{
    /// <summary>
    /// ConfigSyncEvent — Bolt NetEvent for server-authoritative config sync.
    /// 
    /// Server broadcasts config to clients on join and on every change.
    /// Clients receive and apply settings — all players get identical limits.
    /// 
    /// Protocol:
    ///   0x01 = Client requests config (client → server)
    ///   0x10 = Server sends full config payload (server → client)
    /// 
    /// Modeled after PermissionEvent pattern.
    /// </summary>
    public class ConfigSyncEvent : Packets.NetEvent
    {
        public static ConfigSyncEvent Instance;
        
        // Track connected clients for re-broadcast on config change
        private static readonly HashSet<BoltConnection> _connectedClients = new HashSet<BoltConnection>();
        
        public override string Id => "ProjectX.ConfigSync";
        
        /// <summary>
        /// Register the event with SonsSdk networking.
        /// </summary>
        public static void Register()
        {
            Instance = new ConfigSyncEvent();
            Packets.Register(Instance);
            RLog.Msg(Color.GreenYellow, "[ConfigSyncEvent] Registered");
        }
        
        /// <summary>
        /// Handle received packet — dispatch to server or client handler.
        /// </summary>
        public override void Read(UdpPacket packet, BoltConnection fromConnection)
        {
            if (BoltNetwork.isServer)
            {
                ReadMessageServer(packet, fromConnection);
            }
            else
            {
                ReadMessageClient(packet, fromConnection);
            }
        }
        
        // ==================== SERVER SIDE ====================
        
        /// <summary>
        /// [Server] Handle incoming config request from client.
        /// </summary>
        private void ReadMessageServer(UdpPacket packet, BoltConnection fromConnection)
        {
            try
            {
                byte requestType = packet.ReadByte();
                
                if (requestType == 0x01) // Config request
                {
                    string steamId = "unknown";
                    try { steamId = fromConnection.RemoteEndPoint.SteamId.ToString(); } catch { }
                    
                    RLog.Msg($"[ConfigSync] Server received config request from: {steamId}");
                    
                    // Track this client for future re-broadcasts
                    _connectedClients.Add(fromConnection);
                    
#if SERVER || OWNER
                    SendConfigToClient(fromConnection);
#endif
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ConfigSync] Server read error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// [Client] Handle incoming config payload from server.
        /// </summary>
        private void ReadMessageClient(UdpPacket packet, BoltConnection fromConnection)
        {
            try
            {
                byte msgType = packet.ReadByte();
                
                if (msgType == 0x10) // Full config payload
                {
                    string payload = packet.ReadString();
                    
                    RLog.Msg($"[ConfigSync] Client received config payload ({payload.Length} chars)");
                    
                    int applied = ConfigSyncPayload.Apply(payload);
                    RLog.Msg($"[ConfigSync] Applied {applied} server config values");
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ConfigSync] Client read error: {ex.Message}");
            }
        }
        
#if SERVER || OWNER
        /// <summary>
        /// [Server] Send full config to a specific client.
        /// Called on client join (after receiving config request).
        /// </summary>
        public void SendConfigToClient(BoltConnection connection)
        {
            try
            {
                if (!BoltNetwork.isRunning || connection == null) return;
                
                string payload = ConfigSyncPayload.Serialize();
                
                // Use large buffer for full config payload (~4KB)
                var eventPacket = NewPacket(8192, connection);
                eventPacket.Packet.WriteByte(0x10); // Config payload
                eventPacket.Packet.WriteString(payload);
                
                Send(eventPacket);
                RLog.Msg($"[ConfigSync] Sent {ConfigSyncPayload.EntryCount} config values to client ({payload.Length} chars)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ConfigSync] Send error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// [Server] Broadcast current config to ALL connected clients.
        /// Called on config change (debounced by ConfigSyncPayload.Update).
        /// </summary>
        public void BroadcastConfig()
        {
            try
            {
                if (!BoltNetwork.isRunning) return;
                
                string payload = ConfigSyncPayload.Serialize();
                
                // Clean up dead connections and send to live ones
                var deadConnections = new List<BoltConnection>();
                int sent = 0;
                
                foreach (var conn in _connectedClients)
                {
                    try
                    {
                        if (conn == null)
                        {
                            deadConnections.Add(conn);
                            continue;
                        }
                        
                        var eventPacket = NewPacket(8192, conn);
                        eventPacket.Packet.WriteByte(0x10);
                        eventPacket.Packet.WriteString(payload);
                        Send(eventPacket);
                        sent++;
                    }
                    catch
                    {
                        deadConnections.Add(conn);
                    }
                }
                
                // Remove dead connections
                foreach (var dead in deadConnections)
                    _connectedClients.Remove(dead);
                
                if (sent > 0)
                    RLog.Msg($"[ConfigSync] Broadcast config to {sent} client(s)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ConfigSync] Broadcast error: {ex.Message}");
            }
        }
#endif

#if CLIENT
        /// <summary>
        /// [Client] Request config from server.
        /// Called after game starts and Bolt connection is established.
        /// </summary>
        public void RequestConfig()
        {
            try
            {
                if (!BoltNetwork.isRunning || !BoltNetwork.isClient) return;
                if (BoltNetwork.server == null) return;
                
                // Use NewPacket(size, targetType) for server — type 6 = server
                var eventPacket = NewPacket(32, 6);
                eventPacket.Packet.WriteByte(0x01); // Config request
                
                Send(eventPacket);
                RLog.Msg("[ConfigSync] Sent config request to server");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[ConfigSync] Request error: {ex.Message}");
            }
        }
#endif
        
        /// <summary>
        /// Remove a client from the tracked connections set.
        /// Call when a player disconnects.
        /// </summary>
        public static void OnClientDisconnected(BoltConnection connection)
        {
            _connectedClients.Remove(connection);
        }
    }
}

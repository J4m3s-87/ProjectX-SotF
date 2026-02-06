using System;
using System.Drawing;
using Bolt;
using RedLoader;
using SonsSdk.Networking;
using UdpKit;

namespace ProjectX.Master.Modules.Network
{
    /// <summary>
    /// PermissionEvent - Network event for syncing permissions between server and clients
    /// Server broadcasts on player join, clients receive and apply permissions
    /// </summary>
    public class PermissionEvent : Packets.NetEvent
    {
        public static PermissionEvent Instance;
        
        public override string Id => "ProjectX.PermissionSync";
        
        /// <summary>
        /// Register the event with SonsSdk networking
        /// </summary>
        public static void Register()
        {
            Instance = new PermissionEvent();
            Packets.Register(Instance);
            RLog.Msg(Color.GreenYellow, "[PermissionEvent] Registered");
        }
        
        /// <summary>
        /// Handle received packet - dispatch to server or client handler
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
        
        /// <summary>
        /// [Server] Handle incoming permission request from client
        /// </summary>
        private void ReadMessageServer(UdpPacket packet, BoltConnection fromConnection)
        {
            try
            {
                // Client is requesting permissions - send them back
                byte requestType = packet.ReadByte();
                
                if (requestType == 0x01) // Permission request
                {
                    // Get SteamID from connection (safely)
                    string steamId = "";
                    try
                    {
                        steamId = fromConnection.RemoteEndPoint.SteamId.ToString();
                    }
                    catch
                    {
                        steamId = "unknown";
                    }
                    
                    RLog.Msg($"[PermissionEvent] Server received permission request from: {steamId}");
                    
#if SERVER || OWNER
                    // Determine permissions for this client
                    bool hasMenu = PermissionSync.PlayerHasMenuAccess(steamId);
                    bool isAdmin = RoleManager.IsAdmin(steamId);
                    string role = RoleManager.GetRole(steamId).ToString().ToLower();
                    
                    // Send permission response
                    SendPermissionsToClient(fromConnection, hasMenu, isAdmin, role);
#endif
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PermissionEvent] Server read error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// [Client] Handle incoming permission update from server
        /// </summary>
        private void ReadMessageClient(UdpPacket packet, BoltConnection fromConnection)
        {
            try
            {
                byte msgType = packet.ReadByte();
                
                if (msgType == 0x02) // Permission response
                {
                    bool hasMenu = packet.ReadBool();
                    bool isAdmin = packet.ReadBool();
                    string role = packet.ReadString();
                    
                    RLog.Msg($"[PermissionEvent] Client received permissions: Menu={hasMenu}, Admin={isAdmin}, Role={role}");
                    
#if CLIENT
                    PermissionSync.OnReceivePermissions(hasMenu, isAdmin, role);
#endif
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PermissionEvent] Client read error: {ex.Message}");
            }
        }
        
#if SERVER || OWNER
        /// <summary>
        /// [Server] Send permissions to specific client
        /// </summary>
        public void SendPermissionsToClient(BoltConnection connection, bool hasMenu, bool isAdmin, string role)
        {
            try
            {
                if (!BoltNetwork.isRunning || connection == null) return;
                
                // Use NewPacket(size, connection) pattern from StoneGate
                var eventPacket = NewPacket(64, connection);
                eventPacket.Packet.WriteByte(0x02); // Permission response
                eventPacket.Packet.WriteBool(hasMenu);
                eventPacket.Packet.WriteBool(isAdmin);
                eventPacket.Packet.WriteString(role);
                
                Send(eventPacket);
                RLog.Msg($"[PermissionEvent] Sent permissions to client: Menu={hasMenu}, Admin={isAdmin}, Role={role}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PermissionEvent] Send error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// [Server] Broadcast permission update to all clients
        /// Note: BoltNetwork.Connections iteration requires reflection in IL2CPP
        /// For now, clients request permissions on connect via CustomEventHandler
        /// </summary>
        public void BroadcastSessionCheats(bool enabled)
        {
            // TODO: Iterate connections via reflection when session cheats toggle mid-game
            // For now, permissions are sent on player connect (see CustomEventHandler.Connected)
            RLog.Msg($"[PermissionEvent] Session cheats set to: {(enabled ? "ON" : "OFF")} (clients get update on reconnect)");
        }
#endif

#if CLIENT
        /// <summary>
        /// [Client] Request permissions from server
        /// </summary>
        public void RequestPermissions()
        {
            try
            {
                if (!BoltNetwork.isRunning || !BoltNetwork.isClient) return;
                if (BoltNetwork.server == null) return;
                
                // Use NewPacket(size, targetType) for server - type 6 = server
                var eventPacket = NewPacket(32, 6);
                eventPacket.Packet.WriteByte(0x01); // Permission request
                
                Send(eventPacket);
                RLog.Msg("[PermissionEvent] Sent permission request to server");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PermissionEvent] Request error: {ex.Message}");
            }
        }
#endif
    }
}

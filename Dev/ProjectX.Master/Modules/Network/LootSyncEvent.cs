using System;
using System.Collections.Generic;
using System.Drawing;
using Bolt;
using RedLoader;
using SonsSdk.Networking;
using UdpKit;
using ProjectX.Master.Modules.LootRespawn;

namespace ProjectX.Master.Modules.Network
{
    /// <summary>
    /// LootSyncEvent — Bolt NetEvent for server-authoritative loot tracking.
    /// 
    /// Clients report collected loot to the server; server records and persists.
    /// When a new client joins, server broadcasts the full suppression list.
    /// 
    /// Protocol:
    ///   0x01 = Client reports pickup collected (client → server)
    ///   0x02 = Client reports container broken (client → server)
    ///   0x03 = Client reports container opened (client → server)
    ///   0x10 = Client requests suppression list (client → server)
    ///   0x20 = Server sends full suppression list (server → client)
    /// 
    /// Modeled after ConfigSyncEvent pattern.
    /// </summary>
    public class LootSyncEvent : Packets.NetEvent
    {
        public static LootSyncEvent Instance;

        public override string Id => "ProjectX.LootSync";

        /// <summary>
        /// Register the event with SonsSdk networking.
        /// </summary>
        public static void Register()
        {
            Instance = new LootSyncEvent();
            Packets.Register(Instance);
            RLog.Msg(Color.GreenYellow, "[LootSyncEvent] Registered");
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
        /// [Server] Handle incoming loot report from client.
        /// </summary>
        private void ReadMessageServer(UdpPacket packet, BoltConnection fromConnection)
        {
            try
            {
                byte msgType = packet.ReadByte();
                string steamId = "unknown";
                try { steamId = fromConnection.RemoteEndPoint.SteamId.ToString(); } catch { }

                switch (msgType)
                {
                    case 0x01: // Pickup collected
                    {
                        string hash = packet.ReadString();
                        int itemId = packet.ReadInt();
                        RLog.Msg($"[LootSync] Server received COLLECT from {steamId}: hash={hash.Substring(0, System.Math.Min(8, hash.Length))}…, itemId={itemId}");
                        LootRespawnModule.OnRemoteLootCollected(hash, itemId);
                        break;
                    }
                    case 0x02: // Container broken
                    {
                        string hash = packet.ReadString();
                        RLog.Msg($"[LootSync] Server received CONTAINER_BREAK from {steamId}: hash={hash.Substring(0, System.Math.Min(8, hash.Length))}…");
                        LootRespawnModule.OnRemoteContainerBroken(hash);
                        break;
                    }
                    case 0x03: // Container opened
                    {
                        string hash = packet.ReadString();
                        RLog.Msg($"[LootSync] Server received CONTAINER_OPEN from {steamId}: hash={hash.Substring(0, System.Math.Min(8, hash.Length))}…");
                        LootRespawnModule.OnRemoteContainerOpened(hash);
                        break;
                    }
                    case 0x10: // Client requests suppression list
                    {
                        RLog.Msg($"[LootSync] Server received STATE_REQUEST from {steamId}");
#if SERVER || OWNER
                        SendSuppressionList(fromConnection);
#endif
                        break;
                    }
                    default:
                        RLog.Warning($"[LootSync] Unknown message type 0x{msgType:X2} from {steamId}");
                        break;
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootSync] Server read error: {ex.Message}");
            }
        }

        /// <summary>
        /// [Client] Handle incoming suppression list from server.
        /// </summary>
        private void ReadMessageClient(UdpPacket packet, BoltConnection fromConnection)
        {
            try
            {
                byte msgType = packet.ReadByte();

                if (msgType == 0x20) // Full suppression list
                {
                    int count = packet.ReadInt();
                    var entries = new List<(string hash, long timestamp, int itemId)>();
                    
                    for (int i = 0; i < count; i++)
                    {
                        string hash = packet.ReadString();
                        long timestamp = packet.ReadLong();
                        int itemId = packet.ReadInt();
                        entries.Add((hash, timestamp, itemId));
                    }

                    RLog.Msg($"[LootSync] Client received suppression list: {count} items");
                    LootRespawnModule.ApplyServerState(entries);
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootSync] Client read error: {ex.Message}");
            }
        }

        // ==================== CLIENT → SERVER REPORTS ====================

        /// <summary>
        /// [Client] Report a pickup was collected.
        /// </summary>
        public void ReportCollected(string hash, int itemId)
        {
            try
            {
                if (!BoltNetwork.isRunning || !BoltNetwork.isClient) return;

                var eventPacket = NewPacket(256, 6); // 6 = server target
                eventPacket.Packet.WriteByte(0x01);
                eventPacket.Packet.WriteString(hash);
                eventPacket.Packet.WriteInt(itemId);
                Send(eventPacket);

                RLog.Msg($"[LootSync] Reported COLLECT to server: hash={hash.Substring(0, System.Math.Min(8, hash.Length))}…, itemId={itemId}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootSync] Report collect error: {ex.Message}");
            }
        }

        /// <summary>
        /// [Client] Report a container was broken.
        /// </summary>
        public void ReportContainerBroken(string hash)
        {
            try
            {
                if (!BoltNetwork.isRunning || !BoltNetwork.isClient) return;

                var eventPacket = NewPacket(256, 6);
                eventPacket.Packet.WriteByte(0x02);
                eventPacket.Packet.WriteString(hash);
                Send(eventPacket);

                RLog.Msg($"[LootSync] Reported CONTAINER_BREAK to server: hash={hash.Substring(0, System.Math.Min(8, hash.Length))}…");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootSync] Report container break error: {ex.Message}");
            }
        }

        /// <summary>
        /// [Client] Report a container was opened (GrabBag).
        /// </summary>
        public void ReportContainerOpened(string hash)
        {
            try
            {
                if (!BoltNetwork.isRunning || !BoltNetwork.isClient) return;

                var eventPacket = NewPacket(256, 6);
                eventPacket.Packet.WriteByte(0x03);
                eventPacket.Packet.WriteString(hash);
                Send(eventPacket);

                RLog.Msg($"[LootSync] Reported CONTAINER_OPEN to server: hash={hash.Substring(0, System.Math.Min(8, hash.Length))}…");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootSync] Report container open error: {ex.Message}");
            }
        }

        /// <summary>
        /// [Client] Request full suppression list from server on join.
        /// </summary>
        public void RequestState()
        {
            try
            {
                if (!BoltNetwork.isRunning || !BoltNetwork.isClient) return;
                if (BoltNetwork.server == null) return;

                var eventPacket = NewPacket(32, 6);
                eventPacket.Packet.WriteByte(0x10);
                Send(eventPacket);

                RLog.Msg("[LootSync] Sent STATE_REQUEST to server");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootSync] Request state error: {ex.Message}");
            }
        }

        // ==================== SERVER → CLIENT RESPONSES ====================

#if SERVER || OWNER
        /// <summary>
        /// [Server] Send full suppression list to a specific client.
        /// </summary>
        public void SendSuppressionList(BoltConnection connection)
        {
            try
            {
                if (!BoltNetwork.isRunning || connection == null) return;

                var collected = LootRespawnModule.GetCollectedEntries();

                // Estimate buffer size: ~50 bytes per entry + overhead
                int bufferSize = System.Math.Max(1024, collected.Count * 64 + 256);
                var eventPacket = NewPacket(bufferSize, connection);
                eventPacket.Packet.WriteByte(0x20); // Suppression list
                eventPacket.Packet.WriteInt(collected.Count);

                foreach (var entry in collected)
                {
                    eventPacket.Packet.WriteString(entry.Hash);
                    eventPacket.Packet.WriteLong(entry.Timestamp);
                    eventPacket.Packet.WriteInt(entry.ItemId);
                }

                Send(eventPacket);
                RLog.Msg($"[LootSync] Sent suppression list to client: {collected.Count} items ({bufferSize} byte buffer)");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[LootSync] Send suppression list error: {ex.Message}");
            }
        }
#endif
    }
}

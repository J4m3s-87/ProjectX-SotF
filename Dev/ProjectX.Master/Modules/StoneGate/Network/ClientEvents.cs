using System;
using System.Runtime.CompilerServices;
using RedLoader;
using SonsSdk.Networking;
using ProjectX.Master.Modules.StoneGate.Objects;
using UdpKit;

namespace ProjectX.Master.Modules.StoneGate.Network
{
	// Token: 0x02000016 RID: 22
	internal class ClientEvents : EventBase<ClientEvents>
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00005A51 File Offset: 0x00003C51
		protected override void ReadMessageServer(UdpPacket packet, BoltConnection fromConnection)
		{
			Misc.Msg("[ClientEvents] [ReadMessageServer] Recived Event", true);
			this.RecivedEvent(packet, fromConnection);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005A6C File Offset: 0x00003C6C
		private void RecivedEvent(UdpPacket packet, BoltConnection fromConnection)
		{
			switch (packet.ReadByte())
			{
			case 0:
				try
				{
					string text = packet.ReadString();
					CreateGateParent.RotateMode rotateMode = (CreateGateParent.RotateMode)packet.ReadByte();
					string text2 = packet.ReadString();
					string text3 = packet.ReadString();
					string text4 = packet.ReadString();
					string text5 = packet.ReadString();
					this.CreateStoneGate(text, rotateMode, text2, text3, text4, text5);
				}
				catch (Exception ex)
				{
					Misc.Msg("[ClientEvents] [RecivedEvent] Error: " + ex.Message, true);
				}
				break;
			case 1:
				try
				{
					int num = packet.ReadInt();
					this.DestroyStoneGate(num);
				}
				catch (Exception ex2)
				{
					Misc.Msg("[ClientEvents] [RecivedEvent] Error: " + ex2.Message, true);
				}
				break;
			case 2:
				try
				{
					int num2 = packet.ReadInt();
					this.OpenStoneGate(num2);
				}
				catch (Exception ex3)
				{
					Misc.Msg("[ClientEvents] [RecivedEvent] Error: " + ex3.Message, true);
				}
				break;
			case 3:
				try
				{
					int num3 = packet.ReadInt();
					this.CloseStoneGate(num3);
				}
				catch (Exception ex4)
				{
					Misc.Msg("[ClientEvents] [RecivedEvent] Error: " + ex4.Message, true);
				}
				break;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005BE0 File Offset: 0x00003DE0
		private void CreateStoneGate(string rotateGoName, CreateGateParent.RotateMode mode, string floorBeamGoName, string topBeamGoName, string rockWallGoName, string extraPillarGoName)
		{
			Misc.Msg($"[ClientEvents] [CreateStoneGate] Recived From Client - Create StoneGate Values: rotateGoName: {rotateGoName}, mode: {mode}, floorBeamGoName: {floorBeamGoName}, topBeamGoName: {topBeamGoName}, rockWallGoName: {rockWallGoName}, extraPillarGoName: {extraPillarGoName}", true);
			CreateGateParent.Instance.AddDoorNetworkHost(rotateGoName, mode, floorBeamGoName, topBeamGoName, rockWallGoName, extraPillarGoName);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00005CB1 File Offset: 0x00003EB1
		private void DestroyStoneGate(int childIndex)
		{
			CreateGateParent.Instance.RemoveDoorNetworkHost(childIndex);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005CC0 File Offset: 0x00003EC0
		private void OpenStoneGate(int childIndex)
		{
			CreateGateParent.Instance.OpenDoorNetworkHost(childIndex);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005CCF File Offset: 0x00003ECF
		private void CloseStoneGate(int childIndex)
		{
			CreateGateParent.Instance.CloseDoorNetworkHost(childIndex);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005CE0 File Offset: 0x00003EE0
		private bool CheckChildIndex(int? childIndex)
		{
			bool flag = childIndex == null;
			bool flag2;
			if (flag)
			{
				RLog.Error("[StoneGate] [Network] [ClientEvents] [CheckChildIndex] ChildIndex is null");
				flag2 = false;
			}
			else
			{
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005D14 File Offset: 0x00003F14
		public void SendClientEvent(ClientEvents.ClientEvent eventType, int? childIndex)
		{
			bool flag = !BoltNetwork.isRunning;
			if (!flag)
			{
				Packets.EventPacket eventPacket = base.NewPacket(128, 6);
				eventPacket.Packet.WriteByte((byte)eventType);
				switch (eventType)
				{
				case ClientEvents.ClientEvent.DestroyStoneGate:
				{
					bool flag2 = !this.CheckChildIndex(childIndex);
					if (flag2)
					{
						return;
					}
					eventPacket.Packet.WriteInt(childIndex.Value);
					break;
				}
				case ClientEvents.ClientEvent.OpenStoneGate:
				{
					bool flag3 = !this.CheckChildIndex(childIndex);
					if (flag3)
					{
						return;
					}
					eventPacket.Packet.WriteInt(childIndex.Value);
					break;
				}
				case ClientEvents.ClientEvent.CloseStoneGate:
				{
					bool flag4 = !this.CheckChildIndex(childIndex);
					if (flag4)
					{
						return;
					}
					eventPacket.Packet.WriteInt(childIndex.Value);
					break;
				}
				}
				base.Send(eventPacket);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00005DE8 File Offset: 0x00003FE8
		public void SendClientEvent(ClientEvents.ClientEvent eventType, string rotateGoName, CreateGateParent.RotateMode mode, string floorBeamGoName, string topBeamGoName, string rockWallGoName, string extraPillarGoName)
		{
			bool flag = !BoltNetwork.isRunning;
			if (!flag)
			{
				Packets.EventPacket eventPacket = base.NewPacket(512, 6);
				eventPacket.Packet.WriteByte((byte)eventType);
				if (eventType != ClientEvents.ClientEvent.CreateStoneGate)
				{
					Misc.Msg("[ClientEvents] [SendClientEvent] Event type requires a child index", true);
				}
				else
				{
					bool flag2 = string.IsNullOrEmpty(rotateGoName);
					if (flag2)
					{
						Misc.Msg("[ClientEvents] [SendClientEvent] RotateGoName is null or empty", true);
						eventPacket.Packet.Dispose();
					}
					else
					{
						bool flag3 = mode == CreateGateParent.RotateMode.None;
						if (flag3)
						{
							Misc.Msg("[ClientEvents] [SendClientEvent] RotateMode is None", true);
							eventPacket.Packet.Dispose();
						}
						else
						{
							eventPacket.Packet.WriteString(rotateGoName);
							eventPacket.Packet.WriteByte((byte)mode);
							eventPacket.Packet.WriteString(floorBeamGoName);
							eventPacket.Packet.WriteString(topBeamGoName);
							eventPacket.Packet.WriteString(rockWallGoName);
							eventPacket.Packet.WriteString(extraPillarGoName);
							Misc.Msg("[ClientEvents] [SendClientEvent] Sending CreateStoneGate Event", true);
							base.Send(eventPacket);
						}
					}
				}
			}
		}

		// Token: 0x02000024 RID: 36
		public enum ClientEvent : byte
		{
			// Token: 0x0400005E RID: 94
			CreateStoneGate,
			// Token: 0x0400005F RID: 95
			DestroyStoneGate,
			// Token: 0x04000060 RID: 96
			OpenStoneGate,
			// Token: 0x04000061 RID: 97
			CloseStoneGate
		}
	}
}



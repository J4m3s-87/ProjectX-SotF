using System;
using RedLoader;
using SonsSdk.Networking;
using ProjectX.Master.Modules.StoneGate.Objects;
using UdpKit;

namespace ProjectX.Master.Modules.StoneGate.Network
{
	// Token: 0x0200001A RID: 26
	internal class HostEvents : EventBase<HostEvents>
	{
		// Token: 0x0600009C RID: 156 RVA: 0x000060DC File Offset: 0x000042DC
		protected override void ReadMessageClient(UdpPacket packet, BoltConnection fromConnection)
		{
			Misc.Msg("[HostEvents] [ReadMessageClient] Recived Event", true);
			this.RecivedEvent(packet, fromConnection);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000060F4 File Offset: 0x000042F4
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
					int num = packet.ReadInt();
					this.CreateStoneGate(text, rotateMode, text2, text3, text4, text5, num);
				}
				catch (Exception ex)
				{
					Misc.Msg("[HostEvents] [RecivedEvent] Error: " + ex.Message, true);
				}
				break;
			case 1:
				try
				{
					int num2 = packet.ReadInt();
					this.DestroyStoneGate(num2);
				}
				catch (Exception ex2)
				{
					Misc.Msg("[HostEvents] [RecivedEvent] Error: " + ex2.Message, true);
				}
				break;
			case 2:
				try
				{
					int num3 = packet.ReadInt();
					this.OpenStoneGate(num3);
				}
				catch (Exception ex3)
				{
					Misc.Msg("[HostEvents] [RecivedEvent] Error: " + ex3.Message, true);
				}
				break;
			case 3:
				try
				{
					int num4 = packet.ReadInt();
					this.CloseStoneGate(num4);
				}
				catch (Exception ex4)
				{
					Misc.Msg("[HostEvents] [RecivedEvent] Error: " + ex4.Message, true);
				}
				break;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00006274 File Offset: 0x00004474
		private void CreateStoneGate(string rotateGoName, CreateGateParent.RotateMode mode, string floorBeamGoName, string topBeamGoName, string rockWallGoName, string extraPillarGoName, int childIndex)
		{
			CreateGateParent.Instance.AddDoorNetworkClient(rotateGoName, mode, floorBeamGoName, topBeamGoName, rockWallGoName, extraPillarGoName, childIndex);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000628D File Offset: 0x0000448D
		private void DestroyStoneGate(int childIndex)
		{
			CreateGateParent.Instance.RemoveDoorNetworkClient(childIndex);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000629C File Offset: 0x0000449C
		private void OpenStoneGate(int childIndex)
		{
			CreateGateParent.Instance.OpenDoorNetworkClient(childIndex);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000062AB File Offset: 0x000044AB
		private void CloseStoneGate(int childIndex)
		{
			CreateGateParent.Instance.CloseDoorNetworkClient(childIndex);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000062BC File Offset: 0x000044BC
		private bool CheckChildIndex(int? childIndex)
		{
			bool flag = childIndex == null;
			bool flag2;
			if (flag)
			{
				RLog.Error("[StoneGate] [Network] [HostEvents] [CheckChildIndex] ChildIndex is null");
				flag2 = false;
			}
			else
			{
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000062F0 File Offset: 0x000044F0
		public void SendHostEvent(HostEvents.HostEvent eventType, int? childIndex)
		{
			bool flag = !BoltNetwork.isRunning;
			if (!flag)
			{
				Packets.EventPacket eventPacket = base.NewPacket(128, 8);
				eventPacket.Packet.WriteByte((byte)eventType);
				switch (eventType)
				{
				case HostEvents.HostEvent.DestroyStoneGate:
				{
					bool flag2 = !this.CheckChildIndex(childIndex);
					if (flag2)
					{
						return;
					}
					eventPacket.Packet.WriteInt(childIndex.Value);
					break;
				}
				case HostEvents.HostEvent.OpenStoneGate:
				{
					bool flag3 = !this.CheckChildIndex(childIndex);
					if (flag3)
					{
						return;
					}
					eventPacket.Packet.WriteInt(childIndex.Value);
					break;
				}
				case HostEvents.HostEvent.CloseStoneGate:
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

		// Token: 0x060000A4 RID: 164 RVA: 0x000063C4 File Offset: 0x000045C4
		public void SendHostEvent(HostEvents.HostEvent eventType, string rotateGoName, CreateGateParent.RotateMode mode, string floorBeamGoName, string topBeamGoName, string rockWallGoName, string extraPillarGoName, int childIndex)
		{
			bool flag = !BoltNetwork.isRunning;
			if (!flag)
			{
				Packets.EventPacket eventPacket = base.NewPacket(512, 8);
				eventPacket.Packet.WriteByte((byte)eventType);
				if (eventType != HostEvents.HostEvent.CreateStoneGate)
				{
					Misc.Msg("[HostEvents] [SendHostEvent] Event type requires a child index", true);
				}
				else
				{
					bool flag2 = string.IsNullOrEmpty(rotateGoName);
					if (flag2)
					{
						Misc.Msg("[HostEvents] [SendHostEvent] RotateGoName is null or empty", true);
						eventPacket.Packet.Dispose();
					}
					else
					{
						bool flag3 = mode == CreateGateParent.RotateMode.None;
						if (flag3)
						{
							Misc.Msg("[HostEvents] [SendHostEvent] RotateMode is None", true);
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
							eventPacket.Packet.WriteInt(childIndex);
							Misc.Msg("[HostEvents] [SendHostEvent] Sending CreateStoneGate Event", true);
							base.Send(eventPacket);
						}
					}
				}
			}
		}

		// Token: 0x02000026 RID: 38
		public enum HostEvent : byte
		{
			// Token: 0x04000067 RID: 103
			CreateStoneGate,
			// Token: 0x04000068 RID: 104
			DestroyStoneGate,
			// Token: 0x04000069 RID: 105
			OpenStoneGate,
			// Token: 0x0400006A RID: 106
			CloseStoneGate
		}
	}
}



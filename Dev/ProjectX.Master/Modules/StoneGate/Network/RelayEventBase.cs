using System;
using Bolt;
using SonsSdk.Networking;
using UdpKit;
using UnityEngine;

namespace ProjectX.Master.Modules.StoneGate.Network
{
	// Token: 0x02000019 RID: 25
	public class RelayEventBase<T, TRelay> : EventBase<T> where T : Packets.NetEvent, new() where TRelay : MonoBehaviour, Packets.IPacketReader
	{
		// Token: 0x06000098 RID: 152 RVA: 0x0000608B File Offset: 0x0000428B
		protected override void ReadMessageClient(UdpPacket packet, BoltConnection fromConnection)
		{
			base.TryRelay<TRelay>(packet, fromConnection);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00006097 File Offset: 0x00004297
		protected override void ReadMessageServer(UdpPacket packet, BoltConnection fromConnection)
		{
			base.TryRelay<TRelay>(packet, fromConnection);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000060A4 File Offset: 0x000042A4
		protected Packets.EventPacket NewPacket(BoltEntity entity, int size, GlobalTargets targets)
		{
			Packets.EventPacket eventPacket = base.NewPacket(size + 128, targets);
			BoltUtils.WriteBoltEntity(eventPacket.Packet, entity);
			return eventPacket;
		}
	}
}



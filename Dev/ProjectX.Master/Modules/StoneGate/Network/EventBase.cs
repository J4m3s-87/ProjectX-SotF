using System;
using System.Drawing;
using RedLoader;
using SonsSdk.Networking;
using UdpKit;

namespace ProjectX.Master.Modules.StoneGate.Network
{
	// Token: 0x02000018 RID: 24
	public class EventBase<T> : Packets.NetEvent where T : Packets.NetEvent, new()
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00005FF4 File Offset: 0x000041F4
		public static void Register()
		{
			EventBase<T>.Instance = new T();
			Packets.Register(EventBase<T>.Instance);
			RLog.Msg(Color.GreenYellow, "Registered " + typeof(T).Name);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00006040 File Offset: 0x00004240
		public override void Read(UdpPacket packet, BoltConnection fromConnection)
		{
			bool isServer = BoltNetwork.isServer;
			if (isServer)
			{
				this.ReadMessageServer(packet, fromConnection);
			}
			else
			{
				this.ReadMessageClient(packet, fromConnection);
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000606B File Offset: 0x0000426B
		protected virtual void ReadMessageServer(UdpPacket packet, BoltConnection fromConnection)
		{
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000606E File Offset: 0x0000426E
		protected virtual void ReadMessageClient(UdpPacket packet, BoltConnection fromConnection)
		{
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00006071 File Offset: 0x00004271
		public override string Id
		{
			get
			{
				return typeof(T).FullName;
			}
		}

		// Token: 0x04000037 RID: 55
		public static T Instance;
	}
}



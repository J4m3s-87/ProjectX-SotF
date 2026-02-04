using System;
using System.Collections;
using Bolt;
using RedLoader;
using ProjectX.Master.Modules.StoneGate.Network.Joining;
using UnityEngine;

namespace ProjectX.Master.Modules.StoneGate.Network
{
	// Token: 0x02000017 RID: 23
	[RegisterTypeInIl2Cpp]
	public class CustomEventHandler : GlobalEventListener
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00005F00 File Offset: 0x00004100
		public static void Create()
		{
			bool flag = CustomEventHandler.Instance;
			if (!flag)
			{
				CustomEventHandler.Instance = new GameObject("CustomEventHandler").AddComponent<CustomEventHandler>();
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005F34 File Offset: 0x00004134
		public override void Connected(BoltConnection connection)
		{
			bool flag = connection == null;
			if (flag)
			{
				Misc.Msg("[CustomEventHandler] [Connected] Connection is null", true);
			}
			else
			{
				bool flag2 = !BoltNetwork.isServer;
				if (flag2)
				{
					Misc.Msg("[CustomEventHandler] [Connected] Not a server, skipping request", true);
				}
				else
				{
					Misc.Msg("[CustomEventHandler] [Connected] Player connected", true);
				}
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005F80 File Offset: 0x00004180
		public void OnEnterWorld()
		{
			Misc.Msg("[CustomEventHandler] [OnEnterWorld] Player entered world", true);
			bool flag = !BoltNetwork.isRunning;
			if (flag)
			{
				Misc.Msg("[CustomEventHandler] [OnEnterWorld] BoltNetwork is not running", true);
			}
			else
			{
				bool flag2 = !BoltNetwork.isClient;
				if (flag2)
				{
					Misc.Msg("[CustomEventHandler] [OnEnterWorld] Not a client, skipping request", true);
				}
				else
				{
					CoroutineExtensions.RunCoro(this.DelayedRequestInfo());
				}
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005FDC File Offset: 0x000041DC
		private IEnumerator DelayedRequestInfo()
		{
			yield return new WaitForSeconds(8f);
			try
			{
				bool flag = EventBase<StoneGateJoin>.Instance != null;
				if (flag)
				{
					Misc.Msg("[CustomEventHandler] [OnEnterWorld] Sending Request StoneGateJoin", true);
					EventBase<StoneGateJoin>.Instance.RequestInfoFromServer();
				}
				else
				{
					Misc.Msg("[CustomEventHandler] [OnEnterWorld] StoneGateJoin.Instance is null", true);
				}
				yield break;
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				Misc.Msg("[CustomEventHandler] [OnEnterWorld] Error requesting info: " + ex.Message, true);
				yield break;
			}
			yield break;
		}

		// Token: 0x04000036 RID: 54
		public static CustomEventHandler Instance;
	}
}



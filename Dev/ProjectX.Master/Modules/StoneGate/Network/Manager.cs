using System;
using RedLoader;
using ProjectX.Master.Modules.StoneGate.Network.Joining;

namespace ProjectX.Master.Modules.StoneGate.Network
{
	// Token: 0x0200001B RID: 27
	public class Manager
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x000064EC File Offset: 0x000046EC
		internal static void Register()
		{
			bool flag = Manager.isRegistered;
			if (!flag)
			{
				Manager.isRegistered = true;
				try
				{
					EventBase<StoneGateJoin>.Register();
					EventBase<ClientEvents>.Register();
					EventBase<HostEvents>.Register();
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("already registered"))
					{
						RLog.Msg("[StoneGate] NetEvents already registered. Skipping.");
					}
					else
					{
						RLog.Error($"[StoneGate] NetEvent Registration Failed: {ex}");
					}
				}
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000651E File Offset: 0x0000471E
		internal static void RegisterEventHandlers()
		{
			CustomEventHandler.Create();
		}

		// Token: 0x04000038 RID: 56
		private static bool isRegistered;
	}
}



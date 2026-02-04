using System;
using RedLoader;
using Sons.Multiplayer;
using Steamworks;
using TheForest.Utils;

namespace ProjectX.Master.Modules.StoneGate {
	// Token: 0x02000006 RID: 6
	internal class Misc
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002624 File Offset: 0x00000824
		internal static void Msg(string msg, bool network = false)
		{
			bool flag = !Config.LoggingToConsole.Value;
			if (!flag)
			{
				RLog.Msg("[StoneGate] " + msg);
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002658 File Offset: 0x00000858
		internal static string GetLocalPlayerSteamId()
		{
			try
			{
				return SteamUser.GetSteamID().ToString();
			}
			catch
			{
				return "0";
			}
		}
	}
}



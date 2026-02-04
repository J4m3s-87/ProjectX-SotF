using System;
using Sons.Gui;
using ProjectX.Master.Modules.StoneGate.Mono;
using TheForest.Utils;

namespace ProjectX.Master.Modules.StoneGate.Objects
{
	// Token: 0x02000010 RID: 16
	internal static class ActiveItem
	{
		// Token: 0x0600005E RID: 94 RVA: 0x0000427C File Offset: 0x0000247C
		public static void OnKeyPress()
		{
			bool flag = !LocalPlayer.IsInWorld || LocalPlayer.IsInInventory || PauseMenu.IsActive;
			if (!flag)
			{
				bool flag2 = ActiveItem.active == null;
				if (!flag2)
				{
					ActiveItem.active.InitHit();
				}
			}
		}

		// Token: 0x0400002F RID: 47
		public static StoneGateItemMono active;
	}
}



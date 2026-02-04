using System;
using System.Collections.Generic;
using RedLoader;
using Sons.Gui;
using TheForest.Utils;

namespace ProjectX.Master.Modules.StoneGate.Objects
{
	// Token: 0x02000015 RID: 21
	internal static class UiController
	{
		// Token: 0x0600007D RID: 125 RVA: 0x000057FC File Offset: 0x000039FC
		public static void ChangeMode()
		{
			bool flag = !LocalPlayer.IsInWorld || LocalPlayer.IsInInventory || PauseMenu.IsActive || LocalPlayer.InWater;
			if (!flag)
			{
				bool flag2 = StoneGateUi.panelText.Value == "UNKOWN";
				if (flag2)
				{
					StoneGateUi.panelText.Value = "MARK";
				}
				else
				{
					bool flag3 = StoneGateUi.panelText.Value == "MARK";
					if (flag3)
					{
						StoneGateUi.panelText.Value = "ROTATE";
					}
					else
					{
						bool flag4 = StoneGateUi.panelText.Value == "ROTATE";
						if (flag4)
						{
							StoneGateUi.panelText.Value = "DELETE";
						}
						else
						{
							bool flag5 = StoneGateUi.panelText.Value == "DELETE";
							if (flag5)
							{
								StoneGateUi.panelText.Value = "MARK";
							}
							else
							{
								StoneGateUi.panelText.Value = "MARK";
							}
						}
					}
				}
				bool flag6 = StoneGateUi.panelText.Value == "MARK" || StoneGateUi.panelText.Value == "ROTATE";
				if (flag6)
				{
					bool flag7 = StoneGateModule.StoneGateToolUI == null;
					if (flag7)
					{
						RLog.Error("[StoneGate] [UiController] [ChangeMode] StoneGateToolUI is null");
					}
					else
					{
						StoneGateModule.StoneGateToolUI.SetActive(true);
					}
				}
				else
				{
					bool flag8 = StoneGateUi.panelText.Value == "DELETE";
					if (flag8)
					{
						bool flag9 = StoneGateModule.StoneGateToolUI == null;
						if (flag9)
						{
							RLog.Error("[StoneGate] [UiController] [ChangeMode] StoneGateToolUI is null");
						}
						else
						{
							StoneGateModule.StoneGateToolUI.SetActive(false);
						}
					}
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000059A4 File Offset: 0x00003BA4
		public static void SetMode(string mode)
		{
			bool flag = UiController.allowedModes.Contains(mode);
			if (flag)
			{
				StoneGateUi.panelText.Value = mode;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000059D0 File Offset: 0x00003BD0
		public static string GetMode()
		{
			return StoneGateUi.panelText.Value;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000059EC File Offset: 0x00003BEC
		public static HashSet<string> GetAllowedModes()
		{
			return UiController.allowedModes;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005A04 File Offset: 0x00003C04
		public static bool IsValidMode(string mode)
		{
			return UiController.allowedModes.Contains(mode);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005A21 File Offset: 0x00003C21
		// Note: this type is marked as 'beforefieldinit'.
		static UiController()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("MARK");
			hashSet.Add("ROTATE");
			hashSet.Add("DELETE");
			UiController.allowedModes = hashSet;
		}

		// Token: 0x04000035 RID: 53
		private static HashSet<string> allowedModes;
	}
}



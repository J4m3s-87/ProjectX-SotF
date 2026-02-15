using System;
using SUI;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectX.Master.Modules.StoneGate {
	// Token: 0x02000008 RID: 8
	public class StoneGateUi
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002AF8 File Offset: 0x00000CF8
		public static void Create()
		{
			SContainerOptions scontainerOptions = global::SUI.SUI.RegisterNewPanel("StoneGatePlacePanel", false, default(KeyCode?)).Anchor((SUI.AnchorType)11).Background(Color.black, (SUI.EBackground)1, default(Image.Type?))
				.Size(new float?((float)120), new float?((float)60))
				.Position(new float?((float)(-360)), new float?((float)100))
				.OverrideSorting(100);
			StoneGateUi.CloseMainPanel();
			SContainerOptions scontainerOptions2 = global::SUI.SUI.SContainer.Dock((SUI.EDockType)5).OverrideSorting(101);
			scontainerOptions.Add(scontainerOptions2);
			SLabelOptions slabelOptions = global::SUI.SUI.SLabel.Text("UNKNOWN").FontColor(Color.white)
				.FontSize(26)
				.Position(new float?(0f), new float?(0f))
				.HFill()
				.VFill()
				.Bind(StoneGateUi.panelText);
			slabelOptions.SetParent(scontainerOptions2);
			Misc.Msg("StoneGatePlacePanel Created", false);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002BF4 File Offset: 0x00000DF4
		internal static void OpenMainPanel()
		{
			global::SUI.SUI.TogglePanel("StoneGatePlacePanel", true);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002C04 File Offset: 0x00000E04
		internal static void CloseMainPanel()
		{
			try { global::SUI.SUI.TogglePanel("StoneGatePlacePanel", false); } catch { }
			bool flag = StoneGateModule.StoneGateToolUI != null;
			if (flag)
			{
				StoneGateModule.StoneGateToolUI.SetActive(false);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002C3B File Offset: 0x00000E3B
		internal static void ToggleMainPanel()
		{
			try { global::SUI.SUI.TogglePanel("StoneGatePlacePanel"); } catch { }
		}

		// Token: 0x04000016 RID: 22
		public const string StoneGatePlacePanel = "StoneGatePlacePanel";

		// Token: 0x04000017 RID: 23
		public static Observable<string> panelText = new Observable<string>("UNKNOWN");

		// Token: 0x04000018 RID: 24
		public const string defaultPanelText = "UNKNOWN";
	}
}



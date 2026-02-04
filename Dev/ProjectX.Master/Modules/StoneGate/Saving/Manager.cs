using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SonsSdk;
using ProjectX.Master.Modules.StoneGate.Mono;
using ProjectX.Master.Modules.StoneGate.Testing;
using ProjectX.Master.Modules.StoneGate.Tools;
using TheForest.Utils;

namespace ProjectX.Master.Modules.StoneGate.Saving
{
	// Token: 0x0200000F RID: 15
	internal class Manager : ICustomSaveable<Manager.GatesManager>
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00004069 File Offset: 0x00002269
		public string Name
		{
			get
			{
				return "GatesManager";
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00004070 File Offset: 0x00002270
		public bool IncludeInPlayerSave
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004074 File Offset: 0x00002274
		public Manager.GatesManager Save()
		{
			bool flag = BoltNetwork.isRunning && BoltNetwork.isClient;
			Manager.GatesManager gatesManager;
			if (flag)
			{
				bool logSavingSystem = Settings.logSavingSystem;
				if (logSavingSystem)
				{
					Misc.Msg("[Saving] Skipped Saving StoneGates On Multiplayer Client", false);
				}
				gatesManager = null;
			}
			else
			{
				Manager.GatesManager gatesManager2 = new Manager.GatesManager();
				HashSet<StoneGateStoreMono> allStoneGateStoreMono = Gates.GetAllStoneGateStoreMono();
				bool flag2 = allStoneGateStoreMono == null;
				if (flag2)
				{
					bool logSavingSystem2 = Settings.logSavingSystem;
					if (logSavingSystem2)
					{
						Misc.Msg("[Saving] No StoneGateStoreMono Found", false);
					}
					gatesManager = null;
				}
				else
				{
					bool flag3 = allStoneGateStoreMono.Count == 0;
					if (flag3)
					{
						bool logSavingSystem3 = Settings.logSavingSystem;
						if (logSavingSystem3)
						{
							Misc.Msg("[Saving] No StoneGateStoreMono Found", false);
						}
						gatesManager = null;
					}
					else
					{
						bool logSavingSystem4 = Settings.logSavingSystem;
						if (logSavingSystem4)
						{
							Misc.Msg($"[Saving] Found {allStoneGateStoreMono.Count} StoneGateStoreMono", false);
						}
						int num = 0;
						foreach (StoneGateStoreMono stoneGateStoreMono in allStoneGateStoreMono)
						{
							bool flag4 = stoneGateStoreMono == null;
							if (flag4)
							{
								bool logSavingSystem5 = Settings.logSavingSystem;
								if (logSavingSystem5)
								{
									Misc.Msg("[Saving] Controller IS NULL", false);
								}
							}
							else
							{
								gatesManager2.Gates.Add(stoneGateStoreMono.GetSaveData());
								num++;
							}
						}
						bool logSavingSystem6 = Settings.logSavingSystem;
						if (logSavingSystem6)
						{
							Misc.Msg($"[Saving] Saved {num} StoneGates", false);
						}
						gatesManager = gatesManager2;
					}
				}
			}
			return gatesManager;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004234 File Offset: 0x00002434
		public void Load(Manager.GatesManager obj)
		{
			bool flag = !LocalPlayer.IsInWorld;
			if (flag)
			{
				Misc.Msg("[Loading] Host mode not ready, deferring load.", false);
				StoneGate.Saving.Load.deferredLoadQueue.Enqueue(obj);
			}
			else
			{
				StoneGate.Saving.Load.ProcessLoadData(obj);
			}
		}

		// Token: 0x02000022 RID: 34
		public class GatesManager
		{
			// Token: 0x04000058 RID: 88
			public List<Manager.GatesManager.GatesModData> Gates = new List<Manager.GatesManager.GatesModData>();

			// Token: 0x0200002A RID: 42
			public class GatesModData
			{
				// Token: 0x0400008C RID: 140
				public string Mode;

				// Token: 0x0400008D RID: 141
				public string FloorBeamName;

				// Token: 0x0400008E RID: 142
				public string TopBeamName;

				// Token: 0x0400008F RID: 143
				public string RockWallName;

				// Token: 0x04000090 RID: 144
				public string ExtraPillarName;

				// Token: 0x04000091 RID: 145
				public string RotationGoName;
			}
		}
	}
}



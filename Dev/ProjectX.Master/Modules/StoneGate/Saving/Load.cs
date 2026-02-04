using System;
using System.Collections.Generic;
using ProjectX.Master.Modules.StoneGate.Testing;
using ProjectX.Master.Modules.StoneGate.Tools;

namespace ProjectX.Master.Modules.StoneGate.Saving
{
	// Token: 0x0200000E RID: 14
	internal class Load
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00003F50 File Offset: 0x00002150
		internal static void ProcessLoadData(Manager.GatesManager obj)
		{
			bool flag = BoltNetwork.isRunning && BoltNetwork.isClient;
			if (flag)
			{
				bool logSavingSystem = Settings.logSavingSystem;
				if (logSavingSystem)
				{
					Misc.Msg("[Loading] Skipped Loading StoneGates On Multiplayer Client", false);
				}
			}
			else
			{
				bool logSavingSystem2 = Settings.logSavingSystem;
				if (logSavingSystem2)
				{
					bool flag2 = obj == null;
					if (flag2)
					{
						Misc.Msg("[Loading] GatesData IS NULL", false);
						return;
					}
					Misc.Msg("[Loading] Gates From Save: " + obj.Gates.Count.ToString(), false);
				}
				foreach (Manager.GatesManager.GatesModData gatesModData in obj.Gates)
				{
					bool logSavingSystem3 = Settings.logSavingSystem;
					if (logSavingSystem3)
					{
						Misc.Msg("[Loading] Creating New Gates", false);
					}
					bool flag3 = gatesModData == null;
					if (flag3)
					{
						Misc.Msg("[Loading] GatesData IS NULL", false);
					}
					else
					{
						Gates.LoadIndivudalSaveData(gatesModData);
					}
				}
			}
		}

		// Token: 0x0400002E RID: 46
		internal static Queue<Manager.GatesManager> deferredLoadQueue = new Queue<Manager.GatesManager>();
	}
}



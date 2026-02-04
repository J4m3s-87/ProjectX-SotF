using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SonsSdk;
using ProjectX.Master.Modules.StoneGate.Mono;
using ProjectX.Master.Modules.StoneGate.Objects;
using ProjectX.Master.Modules.StoneGate.Saving;
using ProjectX.Master.Modules.StoneGate.Testing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectX.Master.Modules.StoneGate.Tools
{
	// Token: 0x02000009 RID: 9
	internal static class Gates
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00002C64 File Offset: 0x00000E64
		public static bool IsObjectOcupied(GameObject go)
		{
			return Gates.ocupiedObjects.Contains(go);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002C8C File Offset: 0x00000E8C
		public static GameObject GetLinkedStoneGate(GameObject go)
		{
			HashSet<StoneGateStoreMono> allStoneGateStoreMono = Gates.GetAllStoneGateStoreMono();
			foreach (StoneGateStoreMono stoneGateStoreMono in allStoneGateStoreMono)
			{
				bool flag = stoneGateStoreMono == null;
				if (flag)
				{
					bool logToolsGates = Settings.logToolsGates;
					if (logToolsGates)
					{
						Misc.Msg("[Tools] [Gates] [GetLinkedStoneGate] Controller IS NULL", false);
					}
				}
				else
				{
					Dictionary<string, GameObject> namesAndGameObjects = stoneGateStoreMono.GetNamesAndGameObjects();
					foreach (KeyValuePair<string, GameObject> keyValuePair in namesAndGameObjects)
					{
						bool flag2 = keyValuePair.Value == go;
						if (flag2)
						{
							bool logToolsGates2 = Settings.logToolsGates;
							if (logToolsGates2)
							{
								Misc.Msg("[Tools] [Gates] [GetLinkedStoneGate] Found StoneGate Linked to GameObject: " + go.name, false);
							}
							return stoneGateStoreMono.gameObject;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002D9C File Offset: 0x00000F9C
		public static GameObject GetLinkedStoneGate(string goName)
		{
			HashSet<StoneGateStoreMono> allStoneGateStoreMono = Gates.GetAllStoneGateStoreMono();
			foreach (StoneGateStoreMono stoneGateStoreMono in allStoneGateStoreMono)
			{
				bool flag = stoneGateStoreMono == null;
				if (flag)
				{
					bool logToolsGates = Settings.logToolsGates;
					if (logToolsGates)
					{
						Misc.Msg("[Tools] [Gates] [GetLinkedStoneGate] Controller IS NULL", false);
					}
				}
				else
				{
					Dictionary<string, GameObject> namesAndGameObjects = stoneGateStoreMono.GetNamesAndGameObjects();
					foreach (KeyValuePair<string, GameObject> keyValuePair in namesAndGameObjects)
					{
						bool flag2 = keyValuePair.Value.name == goName;
						if (flag2)
						{
							bool logToolsGates2 = Settings.logToolsGates;
							if (logToolsGates2)
							{
								Misc.Msg("[Tools] [Gates] [GetLinkedStoneGate] Found StoneGate Linked to GameObject: " + goName, false);
							}
							return stoneGateStoreMono.gameObject;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002EAC File Offset: 0x000010AC
		public static StoneGateStoreMono GetLinkedStoneGateController(GameObject go)
		{
			GameObject linkedStoneGate = Gates.GetLinkedStoneGate(go);
			return linkedStoneGate.GetComponent<StoneGateStoreMono>();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002ECC File Offset: 0x000010CC
		public static StoneGateStoreMono GetLinkedStoneGateController(string goName)
		{
			GameObject linkedStoneGate = Gates.GetLinkedStoneGate(goName);
			return linkedStoneGate.GetComponent<StoneGateStoreMono>();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002EEC File Offset: 0x000010EC
		public static bool DoesGoHaveStoneGateLinked(GameObject rootGo)
		{
			string rootName = rootGo.name;
			bool flag = Enumerable.Any<string>(Config.allowedHits, (string prefix) => rootName.StartsWith(prefix));
			if (flag)
			{
				HashSet<StoneGateStoreMono> allStoneGateStoreMono = Gates.GetAllStoneGateStoreMono();
				foreach (StoneGateStoreMono stoneGateStoreMono in allStoneGateStoreMono)
				{
					bool flag2 = stoneGateStoreMono == null;
					if (flag2)
					{
						bool logToolsGates = Settings.logToolsGates;
						if (logToolsGates)
						{
							Misc.Msg("[Tools] [Gates] [DoesGoHaveStoneGateLinked] Controller IS NULL", false);
						}
					}
					else
					{
						Dictionary<string, GameObject> namesAndGameObjects = stoneGateStoreMono.GetNamesAndGameObjects();
						foreach (KeyValuePair<string, GameObject> keyValuePair in namesAndGameObjects)
						{
							bool flag3 = keyValuePair.Value == rootGo;
							if (flag3)
							{
								bool logToolsGates2 = Settings.logToolsGates;
								if (logToolsGates2)
								{
									Misc.Msg("[Tools] [Gates] [DoesGoHaveStoneGateLinked] Found StoneGate Linked to GameObject: " + rootGo.name, false);
								}
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000302C File Offset: 0x0000122C
		public static HashSet<StoneGateStoreMono> GetAllStoneGateStoreMono()
		{
			HashSet<StoneGateStoreMono> hashSet = new HashSet<StoneGateStoreMono>();
			GameObject storedParent = CreateGateParent.Instance.StoredParent;
			bool flag = storedParent == null;
			HashSet<StoneGateStoreMono> hashSet2;
			if (flag)
			{
				bool logToolsGates = Settings.logToolsGates;
				if (logToolsGates)
				{
					Misc.Msg("[Tools] [Gates] [GetAllStoneGateStoreMono] StoredParent IS NULL", false);
				}
				hashSet2 = hashSet;
			}
			else
			{
				List<Transform> children = CommonExtensions.GetChildren(storedParent);
				if (Settings.logToolsGates)
				{
					Misc.Msg($"[Tools] [Gates] [GetAllStoneGateStoreMono] TempChildTransforms.Count: {children.Count}", false);
				}
				foreach (Transform transform in children)
				{
					StoneGateStoreMono component = transform.GetComponent<StoneGateStoreMono>();
					bool flag2 = component != null;
					if (flag2)
					{
						hashSet.Add(component);
					}
					else
					{
						bool logToolsGates3 = Settings.logToolsGates;
						if (logToolsGates3)
						{
							Misc.Msg("[Tools] [Gates] [GetAllStoneGateStoreMono] StoneGateStoreMono comp IS NULL", false);
						}
					}
				}
				if (Settings.logToolsGates)
				{
					Misc.Msg($"[Tools] [Gates] [GetAllStoneGateStoreMono] AllStoneGateStoreMono.Count: {hashSet.Count}", false);
				}
				hashSet2 = hashSet;
			}
			return hashSet2;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003188 File Offset: 0x00001388
		public static GameObject FindObjectInSpecificScene(string objectName, string sceneName = "BlankScene")
		{
			bool flag = string.IsNullOrEmpty(objectName);
			GameObject gameObject;
			if (flag)
			{
				Misc.Msg("[Tools] [Tools] [FindObjectInSpecificScene] ObjectName is null or empty", false);
				gameObject = null;
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(sceneName);
				if (flag2)
				{
					Misc.Msg("[Tools] [Tools] [FindObjectInSpecificScene] SceneName is null or empty", false);
					gameObject = null;
				}
				else
				{
					bool flag3 = Gates.scene == null;
					if (flag3)
					{
						Gates.scene = new Scene?(SceneManager.GetSceneByName(sceneName));
					}
					Scene scene = Gates.scene ?? SceneManager.GetSceneByName(sceneName);
					bool flag4 = scene.IsValid() && scene.isLoaded;
					if (flag4)
					{
						GameObject[] array = scene.GetRootGameObjects();
						foreach (GameObject gameObject2 in array)
						{
							bool flag5 = gameObject2.name == objectName;
							if (flag5)
							{
								return gameObject2;
							}
						}
					}
					else
					{
						Misc.Msg("Scene is not valid or not loaded: " + sceneName, false);
					}
					gameObject = null;
				}
			}
			return gameObject;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003298 File Offset: 0x00001498
		public static void LoadAllSaveData(Manager.GatesManager obj)
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
					Misc.Msg("[Loading] Gates From Save: " + obj.Gates.Count.ToString(), false);
				}
				foreach (Manager.GatesManager.GatesModData gatesModData in obj.Gates)
				{
					bool logSavingSystem3 = Settings.logSavingSystem;
					if (logSavingSystem3)
					{
						Misc.Msg("[Loading] Creating New Gates", false);
					}
					Gates.LoadIndivudalSaveData(gatesModData);
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003368 File Offset: 0x00001568
		public static void LoadIndivudalSaveData(Manager.GatesManager.GatesModData obj)
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
					Misc.Msg("[Loading] Gates From Save: " + obj.ToString(), false);
				}
				bool flag2 = string.IsNullOrEmpty(obj.FloorBeamName);
				bool flag3 = string.IsNullOrEmpty(obj.TopBeamName);
				bool flag4 = string.IsNullOrEmpty(obj.RockWallName);
				bool flag5 = string.IsNullOrEmpty(obj.ExtraPillarName);
				bool flag6 = string.IsNullOrEmpty(obj.RotationGoName);
				CreateGateParent.RotateMode rotateMode = CreateGateParent.RotateModeFromString(obj.Mode);
				GameObject gameObject = null;
				GameObject gameObject2 = null;
				GameObject gameObject3 = null;
				GameObject gameObject4 = null;
				bool flag7 = !flag2;
				if (flag7)
				{
					GameObject gameObject5 = Gates.FindObjectInSpecificScene(obj.FloorBeamName, "BlankScene");
					bool superLogSavingSystem = Settings.superLogSavingSystem;
					if (superLogSavingSystem)
					{
						bool flag8 = gameObject5 == null;
						if (flag8)
						{
							Misc.Msg("[Tools] [Gates] [LoadIndivudalSaveData] FloorBeam is null, can't reconstruct gate", false);
						}
						else
						{
							Misc.Msg("[Tools] [Gates] [LoadIndivudalSaveData] FloorBeam is not null, can reconstruct gate", false);
						}
					}
					bool flag9 = !flag3;
					if (flag9)
					{
						gameObject = Gates.FindObjectInSpecificScene(obj.TopBeamName, "BlankScene");
						bool superLogSavingSystem2 = Settings.superLogSavingSystem;
						if (superLogSavingSystem2)
						{
							bool flag10 = gameObject == null;
							if (flag10)
							{
								Misc.Msg("[Tools] [Gates] [LoadIndivudalSaveData] TopBeam is null, can't reconstruct gate", false);
							}
							else
							{
								Misc.Msg("[Tools] [Gates] [LoadIndivudalSaveData] TopBeam is not null, can reconstruct gate", false);
							}
						}
					}
					bool flag11 = !flag4;
					if (flag11)
					{
						gameObject2 = Gates.FindObjectInSpecificScene(obj.RockWallName, "BlankScene");
						bool superLogSavingSystem3 = Settings.superLogSavingSystem;
						if (superLogSavingSystem3)
						{
							bool flag12 = gameObject2 == null;
							if (flag12)
							{
								Misc.Msg("[Tools] [Gates] [LoadIndivudalSaveData] RockWall is null, can't reconstruct gate", false);
							}
							else
							{
								Misc.Msg("[Tools] [Gates] [LoadIndivudalSaveData] RockWall is not null, can reconstruct gate", false);
							}
						}
					}
					bool flag13 = !flag5;
					if (flag13)
					{
						gameObject3 = Gates.FindObjectInSpecificScene(obj.ExtraPillarName, "BlankScene");
						bool superLogSavingSystem4 = Settings.superLogSavingSystem;
						if (superLogSavingSystem4)
						{
							bool flag14 = gameObject3 == null;
							if (flag14)
							{
								Misc.Msg("[Tools] [Gates] [LoadIndivudalSaveData] ExtraPillar is null, can't reconstruct gate", false);
							}
							else
							{
								Misc.Msg("[Tools] [Gates] [LoadIndivudalSaveData] ExtraPillar is not null, can reconstruct gate", false);
							}
						}
					}
					bool flag15 = !flag6;
					if (flag15)
					{
						gameObject4 = Gates.FindObjectInSpecificScene(obj.RotationGoName, "BlankScene");
						bool superLogSavingSystem5 = Settings.superLogSavingSystem;
						if (superLogSavingSystem5)
						{
							bool flag16 = gameObject4 == null;
							if (flag16)
							{
								Misc.Msg("[Tools] [Gates] [LoadIndivudalSaveData] Main Rotate is null, can't reconstruct gate", false);
							}
							else
							{
								Misc.Msg("[Tools] [Gates] [LoadIndivudalSaveData] Main Rotate is not null, can reconstruct gate", false);
							}
						}
					}
					bool flag17 = gameObject4 == null;
					if (flag17)
					{
						Misc.Msg("[Tools] [Gates] [LoadIndivudalSaveData] Main Rotate is null, can't reconstruct gate", false);
					}
					else
					{
						int num = 0;
						bool flag18 = gameObject5 != null;
						if (flag18)
						{
							num++;
						}
						bool flag19 = gameObject != null;
						if (flag19)
						{
							num++;
						}
						bool flag20 = gameObject2 != null;
						if (flag20)
						{
							num++;
						}
						bool flag21 = gameObject3 != null;
						if (flag21)
						{
							num++;
						}
						bool flag22 = num <= 0;
						if (flag22)
						{
							Misc.Msg("[Tools] [Gates] [LoadIndivudalSaveData] No GameObjects found, can't reconstruct gate", false);
						}
						else
						{
							CreateGateParent.Instance.AddDoor(gameObject4, rotateMode, gameObject5, gameObject, gameObject2, gameObject3);
						}
					}
				}
			}
		}

		// Token: 0x04000019 RID: 25
		public static Scene? scene = default(Scene?);

		// Token: 0x0400001A RID: 26
		public static HashSet<GameObject> ocupiedObjects = new HashSet<GameObject>(new GameObjectInstanceIDComparer());
	}
}



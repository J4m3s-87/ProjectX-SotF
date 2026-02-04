using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RedLoader;
using SonsSdk;
using ProjectX.Master.Modules.StoneGate.Objects;
using ProjectX.Master.Modules.StoneGate.Testing;
using ProjectX.Master.Modules.StoneGate.Tools;
using TheForest.Utils;
using UnityEngine;

namespace ProjectX.Master.Modules.StoneGate.Mono
{
	// Token: 0x0200001D RID: 29
	internal class StoneGateItemMono : MonoBehaviour
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x000069A0 File Offset: 0x00004BA0
		private void Start()
		{
			Misc.Msg("[StoneGateItemMono] [Start]", false);
			ActiveItem.active = this;
			this.animController = base.GetComponent<Animator>();
			StoneGateUi.OpenMainPanel();
			this.CheckIfReadyToComplete();
			bool flag = StoneGateModule.StoneGateToolUI == null;
			if (flag)
			{
				RLog.Error("[StoneGate] [StoneGateItemMono] [Start()] StoneGateToolUI is null");
			}
			else
			{
				StoneGateModule.StoneGateToolUI.SetActive(true);
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00006A08 File Offset: 0x00004C08
		private void EnableCompleteUI()
		{
			bool flag = StoneGateModule.StoneGateToolUI == null;
			if (flag)
			{
				RLog.Error("[StoneGate] [StoneGateItemMono] [EnableCompleteUI] StoneGateToolUI is null");
			}
			else
			{
				Transform child = StoneGateModule.StoneGateToolUI.transform.GetChild(0).GetChild(0).GetChild(0);
				GameObject gameObject = child.GetChild(0).gameObject;
				GameObject gameObject2 = child.GetChild(2).gameObject;
				gameObject.SetActive(true);
				gameObject2.SetActive(true);
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00006A7C File Offset: 0x00004C7C
		private void DisableCompleteUI()
		{
			bool flag = StoneGateModule.StoneGateToolUI == null;
			if (flag)
			{
				RLog.Error("[StoneGate] [StoneGateItemMono] [DisableCompleteUI] StoneGateToolUI is null");
			}
			else
			{
				Transform child = StoneGateModule.StoneGateToolUI.transform.GetChild(0).GetChild(0).GetChild(0);
				GameObject gameObject = child.GetChild(0).gameObject;
				GameObject gameObject2 = child.GetChild(2).gameObject;
				gameObject.SetActive(false);
				gameObject2.SetActive(false);
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00006AF0 File Offset: 0x00004CF0
		private bool CheckIfReadyToComplete()
		{
			bool flag = this.HasAnyObjectWithMode(Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 1)) && this.HasAnyObjectWithMode(Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 0));
			bool flag2;
			if (flag)
			{
				this.EnableCompleteUI();
				flag2 = true;
			}
			else
			{
				this.DisableCompleteUI();
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00006B44 File Offset: 0x00004D44
		public void Complete()
		{
			bool flag = !this.CheckIfReadyToComplete();
			if (flag)
			{
				Misc.Msg("Structure Not Ready To Be Completed", false);
			}
			else
			{
				Misc.Msg("Structure Ready To Be Completed", false);
				List<GameObject> objectsWithMode = this.GetObjectsWithMode(Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 1));
				List<GameObject> objectsWithMode2 = this.GetObjectsWithMode(Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 0));
				bool flag2 = objectsWithMode.Count == 0 || objectsWithMode2.Count == 0;
				if (flag2)
				{
					Misc.Msg("[StoneGateItemMono] [Complete] No rotation objects or mark objects found", false);
				}
				else
				{
					this.CleanupAllMarkedObjects();
					this.CheckIfReadyToComplete();
					bool isRunning = BoltNetwork.isRunning;
					if (isRunning)
					{
						bool isServer = BoltNetwork.isServer;
						if (isServer)
						{
							CreateGateParent.Instance.AddDoorNetworkHost(Enumerable.ElementAt<GameObject>(objectsWithMode, 0), objectsWithMode2.ToArray());
						}
						else
						{
							bool isClient = BoltNetwork.isClient;
							if (isClient)
							{
								CreateGateParent.Instance.AddDoorNetworkClient(Enumerable.ElementAt<GameObject>(objectsWithMode, 0), objectsWithMode2.ToArray());
							}
							else
							{
								Misc.Msg("[StoneGateItemMono] [Complete] Not Server Or Client", false);
							}
						}
					}
					else
					{
						CreateGateParent.Instance.AddDoor(Enumerable.ElementAt<GameObject>(objectsWithMode, 0), objectsWithMode2.ToArray());
					}
					bool isStoneGateToolOneTimeUse = StoneGateModule.isStoneGateToolOneTimeUse;
					if (isStoneGateToolOneTimeUse)
					{
						LocalPlayer.Inventory.RemoveItem(751152, 1, false, false, true, null, true);
					}
				}
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00006C84 File Offset: 0x00004E84
		private void OnDisable()
		{
			Misc.Msg("[StoneGateItemMono] [OnDisable]", false);
			ActiveItem.active = null;
			this.CleanupAllMarkedObjects();
			StoneGateUi.CloseMainPanel();
			bool flag = StoneGateModule.StoneGateToolUI == null;
			if (flag)
			{
				RLog.Error("[StoneGate] [StoneGateItemMono] [OnDisable] StoneGateToolUI is null");
			}
			else
			{
				StoneGateModule.StoneGateToolUI.SetActive(false);
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00006CE0 File Offset: 0x00004EE0
		private void CleanupAllMarkedObjects()
		{
			GameObject[] array = new GameObject[this.markedObjects.Count];
			this.markedObjects.CopyTo(array);
			foreach (GameObject gameObject in array)
			{
				bool flag = gameObject != null;
				if (flag)
				{
					this.UnmarkHit(gameObject);
				}
			}
			this.markedObjects.Clear();
			this.originalMaterials.Clear();
			this.markedObjectsWithMode.Clear();
			UnityEngine.Object.Destroy(this.axisLineRenderer);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00006D6C File Offset: 0x00004F6C
		public void InitHit()
		{
			bool flag = this.isAnimRunning;
			if (!flag)
			{
				CoroutineExtensions.RunCoro(this.StartAnim());
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00006D93 File Offset: 0x00004F93
		private IEnumerator StartAnim()
		{
			this.isAnimRunning = true;
			this.animController.SetTrigger("Hit");
			yield return new WaitForSeconds(this.animTime / 2f);
			this.TryHitObject();
			yield return new WaitForSeconds(this.animTime / 2f);
			this.isAnimRunning = false;
			yield break;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00006DA4 File Offset: 0x00004FA4
		private void TryHitObject()
		{
			// if (!flag)
			{
				Transform transform = Camera.main.transform;
				RaycastHit raycastHit;
				Physics.Raycast(transform.position, transform.forward, out raycastHit, this.raycastDistance, LayerMask.GetMask(new string[] { "Prop" }));
				bool flag2 = raycastHit.collider != null;
				if (flag2)
				{
					string rootName = raycastHit.collider.gameObject.transform.root.gameObject.name;
					bool flag3 = Enumerable.Any<string>(this.allowedHits, (string prefix) => rootName.StartsWith(prefix));
					if (flag3)
					{
						GameObject gameObject = raycastHit.collider.gameObject.transform.root.gameObject;
						Misc.Msg("[StoneGateItemMono] [TryHitObject] Hit " + rootName, false);
						string mode = UiController.GetMode();
						bool flag4 = mode == Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 2);
						if (flag4)
						{
							bool flag5 = Gates.DoesGoHaveStoneGateLinked(gameObject);
							bool flag6 = flag5;
							if (flag6)
							{
								GameObject linkedStoneGate = Gates.GetLinkedStoneGate(gameObject);
								bool flag7 = linkedStoneGate == null;
								if (flag7)
								{
									RLog.Error("[StoneGate] [StoneGateItemMono] [TryHitObject] [DELETE] [GetLinkedStoneGate] Can't Delete gate, gotten object == NULL");
									SonsTools.ShowMessage("Can't Delete Gate!, Something went wrong", 3f);
									return;
								}
								StoneGateStoreMono component = linkedStoneGate.GetComponent<StoneGateStoreMono>();
								bool flag8 = component.IsGateOpen();
								if (flag8)
								{
									Misc.Msg("[StoneGateItemMono] [TryHitObject] Can't Delete Gate, Gate is Open", false);
									SonsTools.ShowMessage("Can't Delete Gate, Gate is Open", 3f);
									return;
								}
								component.DestroyGate(true);
							}
							else
							{
								Misc.Msg("[StoneGateItemMono] [TryHitObject] Can't Delete Gate, Not Found", false);
								SonsTools.ShowMessage("Can't Delete Gate, No Link Found", 3f);
							}
						}
						bool flag9 = Gates.ocupiedObjects.Contains(gameObject);
						if (flag9)
						{
							Misc.Msg("[StoneGateItemMono] [TryHitObject] Object Already Ocupied", false);
							SonsTools.ShowMessage("Object Already Ocupied", 3f);
						}
						else
						{
							bool flag10 = this.markedObjects.Contains(gameObject);
							if (flag10)
							{
								this.UnmarkHit(gameObject);
								this.RemoveObjectWithoutMode(gameObject);
							}
							else
							{
								bool flag11 = mode == Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 0);
								if (flag11)
								{
									this.ValidateAndPerformMark(gameObject);
								}
								else
								{
									bool flag12 = mode == Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 1);
									if (flag12)
									{
										bool flag13 = this.HasAnyObjectWithMode(Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 1));
										bool flag14;
										if (flag13)
										{
											flag14 = false;
											Misc.Msg("[StoneGateItemMono] [TryHitObject] Already have a rotation point marked", false);
											SonsTools.ShowMessage("Rotation point already marked, Max 1", 3f);
											this.CheckIfReadyToComplete();
										}
										else
										{
											bool flag15 = !gameObject.name.Contains("RockPilar") && !gameObject.name.Contains("RockBeam");
											if (flag15)
											{
												flag14 = false;
												Misc.Msg("[StoneGateItemMono] [TryHitObject] Invalid rotation Item: " + gameObject.name, false);
												SonsTools.ShowMessage("Invalid rotation object", 3f);
											}
											else
											{
												this.MarkHit(gameObject, new Color?(Color.green));
												this.AddObjectWithMode(Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 1), gameObject);
												this.CreateAxisLineRenderer(gameObject);
												flag14 = true;
												this.CheckIfReadyToComplete();
											}
										}
										bool flag16 = !flag14 && Settings.allowMultipleRotationPoints;
										if (flag16)
										{
											this.MarkHit(gameObject, new Color?(Color.green));
											this.AddObjectWithMode(Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 1), gameObject);
											this.CreateAxisLineRenderer(gameObject);
										}
									}
									else
									{
										Misc.Msg("[StoneGateItemMono] [TryHitObject] Unknown Tool Mode: " + mode, false);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00007154 File Offset: 0x00005354
		private void ValidateAndPerformMark(GameObject rootGo)
		{
			List<GameObject> objectsWithMode = this.GetObjectsWithMode(Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 0));
			int num = 1;
			int num2 = 2;
			int num3 = 1;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			string name = rootGo.name;
			foreach (GameObject gameObject in objectsWithMode)
			{
				string name2 = gameObject.name;
				bool flag = name2.Contains("RockPilar");
				if (flag)
				{
					num4++;
				}
				else
				{
					bool flag2 = name2.Contains("RockBeam");
					if (flag2)
					{
						num5++;
					}
					else
					{
						bool flag3 = name2.Contains("RockWall");
						if (flag3)
						{
							num6++;
						}
					}
				}
			}
			bool flag4 = name.Contains("RockPilar");
			if (flag4)
			{
				bool flag5 = num4 >= num;
				if (flag5)
				{
					Misc.Msg($"[StoneGateItemMono] [ValidateAndPerformMark] Max RockPilars Marked: {num4}", false);
					SonsTools.ShowMessage("Max RockPilars Marked", 3f);
					return;
				}
			}
			else
			{
				bool flag6 = name.Contains("RockBeam");
				if (flag6)
				{
					bool flag7 = num5 >= num2;
					if (flag7)
					{
						Misc.Msg($"[StoneGateItemMono] [ValidateAndPerformMark] Max RockBeams Marked: {num5}", false);
						SonsTools.ShowMessage("Max RockBeams Marked", 3f);
						return;
					}
				}
				else
				{
					bool flag8 = name.Contains("RockWall");
					if (flag8)
					{
						bool flag9 = num6 >= num3;
						if (flag9)
						{
							Misc.Msg($"[StoneGateItemMono] [ValidateAndPerformMark] Max RockWalls Marked: {num6}", false);
							SonsTools.ShowMessage("Max RockWalls Marked", 3f);
							return;
						}
					}
				}
			}
			this.MarkHit(rootGo, default(Color?));
			this.AddObjectWithMode(Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 0), rootGo);
			this.CheckIfReadyToComplete();
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000738C File Offset: 0x0000558C
		private void CreateAxisLineRenderer(GameObject go)
		{
			this.axisLineRenderer = go.GetComponent<LineRenderer>();
			bool flag = this.axisLineRenderer == null;
			if (flag)
			{
				this.axisLineRenderer = go.AddComponent<LineRenderer>();
			}
			this.axisLineRenderer.startWidth = 0.05f;
			this.axisLineRenderer.endWidth = 0.05f;
			this.axisLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
			this.axisLineRenderer.startColor = Color.red;
			this.axisLineRenderer.endColor = Color.red;
			this.axisLineRenderer.positionCount = 2;
			string name = go.name;
			Vector3 position = go.transform.position;
			bool flag2 = name.Contains("RockPilar");
			if (flag2)
			{
				this.axisLineRenderer.SetPosition(0, position + Vector3.down * 0.3f);
				this.axisLineRenderer.SetPosition(1, position + Vector3.up * 3.3f);
				Misc.Msg("[StoneGateMono] [CreateAxisLineRenderer] Created vertical line for " + name, false);
			}
			else
			{
				bool flag3 = name.Contains("RockBeam");
				if (flag3)
				{
					Vector3 forward = go.transform.forward;
					this.axisLineRenderer.SetPosition(0, position - forward * 0.3f);
					this.axisLineRenderer.SetPosition(1, position + forward * 3.3f);
					Misc.Msg("[StoneGateMono] [CreateAxisLineRenderer] Created horizontal line for " + name, false);
				}
				else
				{
					Misc.Msg("[StoneGateMono] [CreateAxisLineRenderer] Invalid Hit line for " + name, false);
				}
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00007538 File Offset: 0x00005738
		private void MarkHit(GameObject rootGo, Color? color = null)
		{
			Misc.Msg($"[StoneGateItemMono] [MarkHit] Object: {rootGo.name}, InstanceID: {rootGo.GetInstanceID()}", false);
			Renderer[] array = rootGo.GetComponentsInChildren<Renderer>(true);
			bool flag = array == null || array.Length == 0;
			if (flag)
			{
				bool logMaterialChanges = Settings.logMaterialChanges;
				if (logMaterialChanges)
				{
					Misc.Msg("Failed To Mark Objects - No renderers found", false);
				}
			}
			else
			{
				Shader shader = ShaderRuntimeLookup.Find("Sons/Outline/StructuresGhostHLSL");
				Color color2 = color ?? new Color(0.8784314f, 0.35686275f, 0.43529412f, 1f);
				Material material = new Material(shader)
				{
					name = "StructureGhostMaterial",
					color = color2
				};
				int instanceID = rootGo.GetInstanceID();
				bool flag2 = !this.originalMaterials.ContainsKey(instanceID);
				if (flag2)
				{
					this.originalMaterials[instanceID] = new Dictionary<int, Material[]>();
				}
				foreach (Renderer renderer in array)
				{
					int instanceID2 = renderer.GetInstanceID();
					bool flag3 = !this.originalMaterials[instanceID].ContainsKey(instanceID2);
					if (flag3)
					{
						Material[] array3 = new Material[renderer.sharedMaterials.Length];
						Array.Copy(renderer.sharedMaterials, array3, renderer.sharedMaterials.Length);
						this.originalMaterials[instanceID][instanceID2] = array3;
						bool logMaterialChanges2 = Settings.logMaterialChanges;
						if (logMaterialChanges2)
						{
							Misc.Msg($"[StoneGateItemMono] [MarkHit] Stored {array3.Length} original materials for renderer: {renderer.name}, ID: {instanceID2}", false);
						}
					}
					Material[] array4 = new Material[renderer.sharedMaterials.Length];
					for (int j = 0; j < array4.Length; j++)
					{
						array4[j] = material;
					}
					renderer.sharedMaterials = array4;
					bool logMaterialChanges3 = Settings.logMaterialChanges;
					if (logMaterialChanges3)
					{
						Misc.Msg("[StoneGateItemMono] [MarkHit] Applied ghost materials to: " + renderer.name, false);
					}
				}
				this.markedObjects.Add(rootGo);
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000077D8 File Offset: 0x000059D8
		private void UnmarkHit(GameObject rootGo)
		{
			bool flag = rootGo == null;
			if (flag)
			{
				Misc.Msg("[StoneGateItemMono] [UnmarkHit] GameObject is null", false);
			}
			else
			{
				int instanceID = rootGo.GetInstanceID();
				bool logMaterialChanges = Settings.logMaterialChanges;
				if (logMaterialChanges)
				{
					Misc.Msg($"[StoneGateItemMono] [UnmarkHit] Object: {rootGo.name}, InstanceID: {instanceID}", false);
				}
				bool flag2 = !this.originalMaterials.ContainsKey(instanceID);
				if (flag2)
				{
					bool logMaterialChanges2 = Settings.logMaterialChanges;
					if (logMaterialChanges2)
					{
						Misc.Msg("[StoneGateItemMono] [UnmarkHit] No original materials found for " + rootGo.name, false);
					}
				}
				else
				{
					Renderer[] array = rootGo.GetComponentsInChildren<Renderer>(true);
					bool flag3 = array == null || array.Length == 0;
					if (flag3)
					{
						bool logMaterialChanges3 = Settings.logMaterialChanges;
						if (logMaterialChanges3)
						{
							Misc.Msg("[StoneGateItemMono] [UnmarkHit] Failed To Unmark Objects - No renderers found", false);
						}
					}
					else
					{
						foreach (Renderer renderer in array)
						{
							int instanceID2 = renderer.GetInstanceID();
							bool flag4 = this.originalMaterials[instanceID].ContainsKey(instanceID2);
							if (flag4)
							{
								Material[] array3 = this.originalMaterials[instanceID][instanceID2];
								bool logMaterialChanges4 = Settings.logMaterialChanges;
								if (logMaterialChanges4)
								{
									Misc.Msg($"[StoneGateItemMono] [UnmarkHit] Restoring {array3.Length} materials for {renderer.name}, ID: {instanceID2}", false);
								}
								renderer.sharedMaterials = array3;
								this.originalMaterials[instanceID].Remove(instanceID2);
							}
							else
							{
								bool logMaterialChanges5 = Settings.logMaterialChanges;
								if (logMaterialChanges5)
								{
									Misc.Msg($"[StoneGateItemMono] [UnmarkHit] No materials found for renderer: {renderer.name}, ID: {instanceID2}", false);
								}
							}
						}
						bool flag5 = this.originalMaterials[instanceID].Count == 0;
						if (flag5)
						{
							this.originalMaterials.Remove(instanceID);
						}
						bool flag6 = this.markedObjects.Contains(rootGo);
						if (flag6)
						{
							this.markedObjects.Remove(rootGo);
							Misc.Msg("[StoneGateItemMono] [UnmarkHit] Removed " + rootGo.name + " from marked [NOT MODE] objects", false);
						}
					}
				}
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00007A94 File Offset: 0x00005C94
		public void AddObjectWithMode(string mode, GameObject gameObject)
		{
			bool flag = !UiController.IsValidMode(mode) || gameObject == null;
			if (!flag)
			{
				ModeGameObjectKey modeGameObjectKey = new ModeGameObjectKey(mode, gameObject);
				this.markedObjectsWithMode[modeGameObjectKey] = gameObject;
				Misc.Msg("[StoneGateItemMono] [AddObjectWithMode] Added " + gameObject.name + " with mode " + mode, false);
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00007AF0 File Offset: 0x00005CF0
		public bool HasObjectWithMode(string mode, GameObject gameObject)
		{
			bool flag = gameObject == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				ModeGameObjectKey modeGameObjectKey = new ModeGameObjectKey(mode, gameObject);
				flag2 = this.markedObjectsWithMode.ContainsKey(modeGameObjectKey);
			}
			return flag2;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00007B28 File Offset: 0x00005D28
		public GameObject GetObjectWithMode(string mode, GameObject gameObject)
		{
			ModeGameObjectKey modeGameObjectKey = new ModeGameObjectKey(mode, gameObject);
			GameObject gameObject2;
			return this.markedObjectsWithMode.TryGetValue(modeGameObjectKey, out gameObject2) ? gameObject2 : null;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00007B58 File Offset: 0x00005D58
		public void RemoveObjectWithMode(string mode, GameObject gameObject)
		{
			ModeGameObjectKey modeGameObjectKey = new ModeGameObjectKey(mode, gameObject);
			this.markedObjectsWithMode.Remove(modeGameObjectKey);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00007B7C File Offset: 0x00005D7C
		public void RemoveObjectWithoutMode(GameObject gameObject)
		{
			foreach (string text in UiController.GetAllowedModes())
			{
				ModeGameObjectKey modeGameObjectKey = new ModeGameObjectKey(text, gameObject);
				bool flag = this.markedObjectsWithMode.ContainsKey(modeGameObjectKey);
				if (flag)
				{
					this.markedObjectsWithMode.Remove(modeGameObjectKey);
					bool flag2 = text == Enumerable.ElementAt<string>(UiController.GetAllowedModes(), 1);
					if (flag2)
					{
						Misc.Msg("[StoneGateItemMono] [RemoveObjectWithoutMode] Removing rotation point for " + gameObject.name, false);
						UnityEngine.Object.Destroy(this.axisLineRenderer);
					}
					else
					{
						Misc.Msg("[StoneGateItemMono] [RemoveObjectWithoutMode] Removing marked object for " + gameObject.name, false);
					}
				}
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00007C54 File Offset: 0x00005E54
		public bool HasAnyObjectWithMode(string mode)
		{
			bool flag = !UiController.IsValidMode(mode);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				foreach (ModeGameObjectKey modeGameObjectKey in this.markedObjectsWithMode.Keys)
				{
					bool flag3 = modeGameObjectKey.Mode == mode;
					if (flag3)
					{
						return true;
					}
				}
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00007CD8 File Offset: 0x00005ED8
		public List<GameObject> GetObjectsWithMode(string mode)
		{
			List<GameObject> list = new List<GameObject>();
			foreach (ModeGameObjectKey modeGameObjectKey in this.markedObjectsWithMode.Keys)
			{
				bool flag = modeGameObjectKey.Mode == mode;
				if (flag)
				{
					list.Add(this.markedObjectsWithMode[modeGameObjectKey]);
				}
			}
			return list;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00007D60 File Offset: 0x00005F60
		public StoneGateItemMono()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("RockWall");
			hashSet.Add("RockPilar");
			hashSet.Add("RockBeam");
			this.allowedHits = hashSet;
			this.raycastDistance = 1.5f;
			this.originalMaterials = new Dictionary<int, Dictionary<int, Material[]>>();
			this.markedObjects = new HashSet<GameObject>(new GameObjectInstanceIDComparer());
			this.markedObjectsWithMode = new Dictionary<ModeGameObjectKey, GameObject>();
		}

		// Token: 0x04000039 RID: 57
		private bool isAnimRunning = false;

		// Token: 0x0400003A RID: 58
		private float animTime = 0.3f;

		// Token: 0x0400003B RID: 59
		private Animator animController;

		// Token: 0x0400003C RID: 60
		private HashSet<string> allowedHits;

		// Token: 0x0400003D RID: 61
		private float raycastDistance;

		// Token: 0x0400003E RID: 62
		private Dictionary<int, Dictionary<int, Material[]>> originalMaterials;

		// Token: 0x0400003F RID: 63
		private HashSet<GameObject> markedObjects;

		// Token: 0x04000040 RID: 64
		private Dictionary<ModeGameObjectKey, GameObject> markedObjectsWithMode;

		// Token: 0x04000041 RID: 65
		private LineRenderer axisLineRenderer;
	}
}



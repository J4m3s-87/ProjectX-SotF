using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RedLoader;
using Sons.Gui.Input;
using SonsSdk;
using ProjectX.Master.Modules.StoneGate.Network;
using ProjectX.Master.Modules.StoneGate.Objects;
using ProjectX.Master.Modules.StoneGate.Saving;
using ProjectX.Master.Modules.StoneGate.Testing;
using ProjectX.Master.Modules.StoneGate.Tools;
using TheForest.Utils;
using UnityEngine;

namespace ProjectX.Master.Modules.StoneGate.Mono
{
	// Token: 0x0200001E RID: 30
	internal class StoneGateStoreMono : MonoBehaviour
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00007DE6 File Offset: 0x00005FE6
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00007DEE File Offset: 0x00005FEE
		public LinkUiElement LinkUiElement { get; private set; }

		// Token: 0x060000C8 RID: 200 RVA: 0x00007DF8 File Offset: 0x00005FF8
		public void Init(GameObject rotationGo, CreateGateParent.RotateMode mode, GameObject floorBeam = null, GameObject topBeam = null, GameObject rockWall = null, GameObject extraPillar = null)
		{
			bool flag = rotationGo == null;
			if (flag)
			{
				RLog.Error("[StoneGate] [StoneGateStoreMono] [Init] rotationGo is null");
				UnityEngine.Object.Destroy(base.gameObject);
			}
			bool flag2 = mode == CreateGateParent.RotateMode.None;
			if (flag2)
			{
				RLog.Error("[StoneGate] [StoneGateStoreMono] [Init] mode is None");
				UnityEngine.Object.Destroy(base.gameObject);
			}
			bool logExtraStoneGateStoreMono = Settings.logExtraStoneGateStoreMono;
			if (logExtraStoneGateStoreMono)
			{
				Misc.Msg($"[StoneGate] [StoneGateStoreMono] [Init] Rotation GameObject: {rotationGo.name} Mode: {mode}", false);
			}
			bool flag3 = floorBeam == null && topBeam == null && rockWall == null && extraPillar == null;
			if (flag3)
			{
				RLog.Error("[StoneGate] [StoneGateStoreMono] [Init] All GameObjects are null");
				UnityEngine.Object.Destroy(base.gameObject);
			}
			this._rotateGo = rotationGo;
			this._rotateMode = mode;
			this._floorBeam = floorBeam;
			this._topBeam = topBeam;
			this._rockWall = rockWall;
			this._extraPillar = extraPillar;
			Gates.ocupiedObjects.Add(rotationGo);
			bool flag4 = floorBeam != null;
			if (flag4)
			{
				this.objectsToRotate.Add(floorBeam);
				this.namesOfAllGo.Add(floorBeam.name, floorBeam);
				this.originalRotations[floorBeam.GetInstanceID()] = floorBeam.transform.rotation;
				this.originalPositions[floorBeam.GetInstanceID()] = floorBeam.transform.position;
				Gates.ocupiedObjects.Add(floorBeam);
			}
			bool flag5 = topBeam != null;
			if (flag5)
			{
				this.objectsToRotate.Add(topBeam);
				this.namesOfAllGo.Add(topBeam.name, topBeam);
				this.originalRotations[topBeam.GetInstanceID()] = topBeam.transform.rotation;
				this.originalPositions[topBeam.GetInstanceID()] = topBeam.transform.position;
				Gates.ocupiedObjects.Add(topBeam);
			}
			bool flag6 = rockWall != null;
			if (flag6)
			{
				this.objectsToRotate.Add(rockWall);
				this.namesOfAllGo.Add(rockWall.name, rockWall);
				this.originalRotations[rockWall.GetInstanceID()] = rockWall.transform.rotation;
				this.originalPositions[rockWall.GetInstanceID()] = rockWall.transform.position;
				Gates.ocupiedObjects.Add(rockWall);
			}
			bool flag7 = extraPillar != null;
			if (flag7)
			{
				this.objectsToRotate.Add(extraPillar);
				this.namesOfAllGo.Add(extraPillar.name, extraPillar);
				this.originalRotations[extraPillar.GetInstanceID()] = extraPillar.transform.rotation;
				this.originalPositions[extraPillar.GetInstanceID()] = extraPillar.transform.position;
				Gates.ocupiedObjects.Add(extraPillar);
			}
			this.namesOfAllGo.Add(rotationGo.name, rotationGo);
			if (Settings.logExtraStoneGateStoreMono)
			{
				Misc.Msg($"[StoneGate] [StoneGateStoreMono] [Init] Added {this.objectsToRotate.Count} GameObjects to objectsToRotate", false);
			}
			bool flag8 = this.LinkUiElement == null;
			if (flag8)
			{
				bool logExtraStoneGateStoreMono3 = Settings.logExtraStoneGateStoreMono;
				if (logExtraStoneGateStoreMono3)
				{
					Misc.Msg("[StoneGate] [StoneGateStoreMono] [Init] Creating LinkUI", false);
				}
				this.LinkUiElement = LinkUi.CreateLinkUi(rotationGo, 2f, null, StoneGateModule.stoneGateOpenCloseIcon, default(Vector3?), "screen.take");
			}
			else
			{
				bool logExtraStoneGateStoreMono4 = Settings.logExtraStoneGateStoreMono;
				if (logExtraStoneGateStoreMono4)
				{
					Misc.Msg("[StoneGate] [StoneGateStoreMono] [Init] LinkUI already exists", false);
				}
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000081E4 File Offset: 0x000063E4
		public bool IsGateOpen()
		{
			return this._gateOpen;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000081FC File Offset: 0x000063FC
		public void ToggleGate(bool raiseNetwork = true)
		{
			bool gateOpen = this._gateOpen;
			if (gateOpen)
			{
				this.CloseGate(raiseNetwork);
			}
			else
			{
				this.OpenGate(raiseNetwork);
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000822C File Offset: 0x0000642C
		public void OpenGate(bool raiseNetwork = true)
		{
			bool flag = this._gateOpen || this.isAnimating;
			if (!flag)
			{
				this._gateOpen = true;
				this.DetermineRotationDirection();
				bool flag2 = raiseNetwork && BoltNetwork.isRunning && BoltNetwork.isServer;
				if (flag2)
				{
					EventBase<HostEvents>.Instance.SendHostEvent(HostEvents.HostEvent.OpenStoneGate, new int?(this.GetChildIndex()));
					bool logExtraStoneGateStoreMono = Settings.logExtraStoneGateStoreMono;
					if (logExtraStoneGateStoreMono)
					{
						Misc.Msg("[StoneGate] [StoneGateStoreMono] [OpenGate] Sending OpenStoneGate event to clients", true);
					}
				}
				else
				{
					bool flag3 = raiseNetwork && BoltNetwork.isRunning && BoltNetwork.isClient;
					if (flag3)
					{
						EventBase<ClientEvents>.Instance.SendClientEvent(ClientEvents.ClientEvent.OpenStoneGate, new int?(this.GetChildIndex()));
						bool logExtraStoneGateStoreMono2 = Settings.logExtraStoneGateStoreMono;
						if (logExtraStoneGateStoreMono2)
						{
							Misc.Msg("[StoneGate] [StoneGateStoreMono] [OpenGate] Sending OpenStoneGate event to server", true);
						}
					}
				}
				CoroutineExtensions.RunCoro(this.AnimateGate(true));
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00008300 File Offset: 0x00006500
		public void CloseGate(bool raiseNetwork = true)
		{
			bool flag = !this._gateOpen || this.isAnimating;
			if (!flag)
			{
				this._gateOpen = false;
				bool flag2 = raiseNetwork && BoltNetwork.isRunning && BoltNetwork.isServer;
				if (flag2)
				{
					EventBase<HostEvents>.Instance.SendHostEvent(HostEvents.HostEvent.CloseStoneGate, new int?(this.GetChildIndex()));
					bool logExtraStoneGateStoreMono = Settings.logExtraStoneGateStoreMono;
					if (logExtraStoneGateStoreMono)
					{
						Misc.Msg("[StoneGate] [StoneGateStoreMono] [CloseGate] Sending CloseStoneGate event to clients", true);
					}
				}
				else
				{
					bool flag3 = raiseNetwork && BoltNetwork.isRunning && BoltNetwork.isClient;
					if (flag3)
					{
						EventBase<ClientEvents>.Instance.SendClientEvent(ClientEvents.ClientEvent.CloseStoneGate, new int?(this.GetChildIndex()));
						bool logExtraStoneGateStoreMono2 = Settings.logExtraStoneGateStoreMono;
						if (logExtraStoneGateStoreMono2)
						{
							Misc.Msg("[StoneGate] [StoneGateStoreMono] [CloseGate] Sending CloseStoneGate event to server", true);
						}
					}
				}
				CoroutineExtensions.RunCoro(this.AnimateGate(false));
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000083CD File Offset: 0x000065CD
		private IEnumerator AnimateGate(bool opening)
		{
			this.isAnimating = true;
			float startTime = Time.time;
			float elapsedTime = 0f;
			Vector3 rotationAxis = this.GetRotationAxis();
			Vector3 pivotPoint = this._rotateGo.transform.position;
			Dictionary<int, Vector3> currentPositions = new Dictionary<int, Vector3>();
			Dictionary<int, Quaternion> currentRotations = new Dictionary<int, Quaternion>();
			foreach (GameObject obj in this.objectsToRotate)
			{
				int id = obj.GetInstanceID();
				currentPositions[id] = obj.transform.position;
				currentRotations[id] = obj.transform.rotation;
				currentRotations[id] = obj.transform.rotation;
			}
			if (Settings.logExtraStoneGateStoreMono)
			{
				Misc.Msg($"[StoneGate] [AnimateGate] Starting gate animation: opening={opening}, objects={this.objectsToRotate.Count}", false);
				Misc.Msg($"[StoneGate] [AnimateGate] Pivot point: {pivotPoint}, Axis: {rotationAxis}", false);
			}
			while (elapsedTime < this.animationDuration)
			{
				elapsedTime = Time.time - startTime;
				float t = Mathf.Clamp01(elapsedTime / this.animationDuration);
				float smoothT = this.SmoothStep(0f, 1f, t);
				foreach (GameObject obj2 in this.objectsToRotate)
				{
					int id2 = obj2.GetInstanceID();
					if (opening)
					{
						Vector3 relativePos = this.originalPositions[id2] - pivotPoint;
						Quaternion rotation = Quaternion.AngleAxis(this.rotationAngle * (float)this.rotationDirection * smoothT, rotationAxis);
						Vector3 newPos = pivotPoint + rotation * relativePos;
						Quaternion newRot = rotation * this.originalRotations[id2];
						obj2.transform.position = newPos;
						obj2.transform.rotation = newRot;
						relativePos = default(Vector3);
						rotation = default(Quaternion);
						newPos = default(Vector3);
						newRot = default(Quaternion);
					}
					else
					{
						obj2.transform.position = Vector3.Lerp(currentPositions[id2], this.originalPositions[id2], smoothT);
						obj2.transform.rotation = Quaternion.Slerp(currentRotations[id2], this.originalRotations[id2], smoothT);
					}
				}
				yield return null;
			}
			foreach (GameObject obj3 in this.objectsToRotate)
			{
				int id3 = obj3.GetInstanceID();
				if (opening)
				{
					Vector3 relativePos2 = this.originalPositions[id3] - pivotPoint;
					Quaternion finalRotation = Quaternion.AngleAxis(this.rotationAngle * (float)this.rotationDirection, rotationAxis);
					Vector3 finalPos = pivotPoint + finalRotation * relativePos2;
					Quaternion finalRot = finalRotation * this.originalRotations[id3];
					obj3.transform.position = finalPos;
					obj3.transform.rotation = finalRot;
					relativePos2 = default(Vector3);
					finalRotation = default(Quaternion);
					finalPos = default(Vector3);
					finalRot = default(Quaternion);
				}
				else
				{
					obj3.transform.position = this.originalPositions[id3];
				obj3.transform.rotation = this.originalRotations[id3];
				}
			}
			this.isAnimating = false;
			if (Settings.logExtraStoneGateStoreMono)
			{
				Misc.Msg($"[StoneGate] [AnimateGate] Completed gate animation: opening={opening}", false);
			}
			yield break;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000083E4 File Offset: 0x000065E4
		private Vector3 GetRotationAxis()
		{
			bool flag = this._rotateMode == CreateGateParent.RotateMode.Vertical;
			Vector3 vector;
			if (flag)
			{
				vector = this._rotateGo.transform.up;
			}
			else
			{
				vector = this._rotateGo.transform.right;
			}
			return vector;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00008428 File Offset: 0x00006628
		private float SmoothStep(float edge0, float edge1, float x)
		{
			x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
			return x * x * (3f - 2f * x);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000845C File Offset: 0x0000665C
		private void DetermineRotationDirection()
		{
			Transform transform = Camera.main.transform;
			Vector3 forward = transform.forward;
			Vector3 position = this._rotateGo.transform.position;
			Vector3 forward2 = this._rotateGo.transform.forward;
			bool flag = this._rotateMode == CreateGateParent.RotateMode.Vertical;
			if (flag)
			{
				bool flag2 = Vector3.Dot(forward2, forward) > 0f;
				if (flag2)
				{
					this.rotationDirection = -1;
				}
				else
				{
					this.rotationDirection = 1;
				}
			}
			else
			{
				Vector3 up = this._rotateGo.transform.up;
				bool flag3 = Vector3.Dot(up, forward) < 0f;
				if (flag3)
				{
					this.rotationDirection = -1;
				}
				else
				{
					this.rotationDirection = 1;
				}
			}
			if (Settings.logExtraStoneGateStoreMono)
			{
				Misc.Msg($"[StoneGate] [DetermineRotationDirection] Camera forward: {forward}, Gate forward: {forward2}", false);
				Misc.Msg($"[StoneGate] [DetermineRotationDirection] Rotation direction: {this.rotationDirection}", false);
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000085A0 File Offset: 0x000067A0
		public Dictionary<string, GameObject> GetNamesAndGameObjects()
		{
			return this.namesOfAllGo;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000085B8 File Offset: 0x000067B8
		public void DestroyGate(bool raiseNetwork = true)
		{
			bool flag = BoltNetwork.isRunning && BoltNetwork.isServer && raiseNetwork;
			if (flag)
			{
				EventBase<HostEvents>.Instance.SendHostEvent(HostEvents.HostEvent.DestroyStoneGate, new int?(this.GetChildIndex()));
				bool logExtraStoneGateStoreMono = Settings.logExtraStoneGateStoreMono;
				if (logExtraStoneGateStoreMono)
				{
					Misc.Msg("[StoneGate] [StoneGateStoreMono] [DestroyGate] Sending DestroyStoneGate event to clients", true);
				}
			}
			else
			{
				bool flag2 = BoltNetwork.isRunning && BoltNetwork.isClient && raiseNetwork;
				if (flag2)
				{
					EventBase<ClientEvents>.Instance.SendClientEvent(ClientEvents.ClientEvent.DestroyStoneGate, new int?(this.GetChildIndex()));
					bool logExtraStoneGateStoreMono2 = Settings.logExtraStoneGateStoreMono;
					if (logExtraStoneGateStoreMono2)
					{
						Misc.Msg("[StoneGate] [StoneGateStoreMono] [DestroyGate] Sending DestroyStoneGate event to server", true);
					}
				}
			}
			bool flag3 = this.LinkUiElement != null;
			if (flag3)
			{
				UnityEngine.Object.Destroy(this.LinkUiElement);
			}
			foreach (GameObject gameObject in this.objectsToRotate)
			{
				Gates.ocupiedObjects.Remove(gameObject);
			}
			Gates.ocupiedObjects.Remove(this._rotateGo);
			UnityEngine.Object.Destroy(base.gameObject);
			SonsTools.ShowMessage("Gate Destroyed", 5f);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000086F4 File Offset: 0x000068F4
		private string FindNameFromGameObject(GameObject go)
		{
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.namesOfAllGo)
			{
				bool flag = keyValuePair.Value == go;
				if (flag)
				{
					return keyValuePair.Key;
				}
			}
			return null;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00008768 File Offset: 0x00006968
		public StoneGate.Saving.Manager.GatesManager.GatesModData GetSaveData()
		{
			return new StoneGate.Saving.Manager.GatesManager.GatesModData
			{
				Mode = this._rotateMode.ToString(),
				FloorBeamName = this.FindNameFromGameObject(this._floorBeam),
				TopBeamName = this.FindNameFromGameObject(this._topBeam),
				RockWallName = this.FindNameFromGameObject(this._rockWall),
				ExtraPillarName = this.FindNameFromGameObject(this._extraPillar),
				RotationGoName = this.FindNameFromGameObject(this._rotateGo)
			};
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000087F4 File Offset: 0x000069F4
		public int GetChildIndex()
		{
			return base.gameObject.transform.GetSiblingIndex();
		}

		// Token: 0x04000042 RID: 66
		private GameObject _rotateGo;

		// Token: 0x04000043 RID: 67
		private CreateGateParent.RotateMode _rotateMode;

		// Token: 0x04000044 RID: 68
		private GameObject _floorBeam;

		// Token: 0x04000045 RID: 69
		private GameObject _topBeam;

		// Token: 0x04000046 RID: 70
		private GameObject _rockWall;

		// Token: 0x04000047 RID: 71
		private GameObject _extraPillar;

		// Token: 0x04000048 RID: 72
		private HashSet<GameObject> objectsToRotate = new HashSet<GameObject>(new GameObjectInstanceIDComparer());

		// Token: 0x04000049 RID: 73
		private Dictionary<string, GameObject> namesOfAllGo = new Dictionary<string, GameObject>();

		// Token: 0x0400004A RID: 74
		private Dictionary<int, Vector3> originalPositions = new Dictionary<int, Vector3>();

		// Token: 0x0400004B RID: 75
		private Dictionary<int, Quaternion> originalRotations = new Dictionary<int, Quaternion>();

		// Token: 0x0400004C RID: 76
		private bool isAnimating = false;

		// Token: 0x0400004D RID: 77
		private float animationDuration = 1f;

		// Token: 0x0400004E RID: 78
		private float rotationAngle = 90f;

		// Token: 0x0400004F RID: 79
		private int rotationDirection = 1;

		// Token: 0x04000051 RID: 81
		private bool _gateOpen = false;
	}
}



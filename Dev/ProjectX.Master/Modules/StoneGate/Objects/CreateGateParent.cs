using System;
using System.Collections.Generic;
using System.Linq;
using Endnight.Utilities;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using RedLoader;
using SonsSdk;
using ProjectX.Master.Modules.StoneGate.Mono;
using ProjectX.Master.Modules.StoneGate.Network;
using ProjectX.Master.Modules.StoneGate.Tools;
using UnityEngine;

namespace ProjectX.Master.Modules.StoneGate.Objects
{
	// Token: 0x02000011 RID: 17
	internal class CreateGateParent
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000042C4 File Offset: 0x000024C4
		public static CreateGateParent Instance
		{
			get
			{
				bool flag = CreateGateParent._instance == null;
				if (flag)
				{
					CreateGateParent._instance = new CreateGateParent();
				}
				return CreateGateParent._instance;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000042F4 File Offset: 0x000024F4
		public CreateGateParent()
		{
			this.StoredParent = new GameObject("StoneGateObjects");
			this.StoredParent.transform.position = Vector3.zero;
			this.StoredParent.transform.rotation = Quaternion.identity;
			this.StoredParent.transform.localScale = Vector3.one;
			this.StoredParent.SetActive(true);
			CommonExtensions.HideAndDontSave(CommonExtensions.DontDestroyOnLoad(this.StoredParent));
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000061 RID: 97 RVA: 0x0000437A File Offset: 0x0000257A
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00004382 File Offset: 0x00002582
		public GameObject StoredParent { get; private set; }

		// Token: 0x06000063 RID: 99 RVA: 0x0000438C File Offset: 0x0000258C
		public static CreateGateParent.RotateMode RotateModeFromString(string modeString)
		{
			return (CreateGateParent.RotateMode)System.Enum.Parse(typeof(CreateGateParent.RotateMode), modeString, true);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000043B4 File Offset: 0x000025B4
		public void AddDoor(GameObject rotate, params GameObject[] otherGameObjects)
		{
			bool flag = rotate == null;
			if (flag)
			{
				RLog.Error("[StoneGate] [CreateGateParent] [AddDoor] Rotate is null");
			}
			else
			{
				CreateGateParent.RotateMode rotateMode = CreateGateParent.RotateMode.None;
				bool flag2 = rotate.name.Contains("RockPilar");
				if (flag2)
				{
					rotateMode = CreateGateParent.RotateMode.Vertical;
				}
				else
				{
					bool flag3 = rotate.name.Contains("RockBeam");
					if (flag3)
					{
						rotateMode = CreateGateParent.RotateMode.Horizontal;
					}
				}
				bool flag4 = rotateMode == CreateGateParent.RotateMode.None;
				if (flag4)
				{
					RLog.Error("[StoneGate] [CreateGateParent] [AddDoor] Rotate is not a valid object");
				}
				else
				{
					GameObject gameObject = new GameObject("StoneGate", new System.Type[] { typeof(StoneGateStoreMono) });
					CommonExtensions.SetParent(gameObject, this.StoredParent.transform, false);
					StoneGateStoreMono orAddComponent = TryGetComponentExtensions.GetOrAddComponent<StoneGateStoreMono>(gameObject);
					bool flag5 = otherGameObjects.Length != 0;
					if (flag5)
					{
						Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
						foreach (GameObject gameObject2 in otherGameObjects)
						{
							bool flag6 = gameObject2 == null;
							if (flag6)
							{
								RLog.Error("[StoneGate] [CreateGateParent] [AddDoor] GameObject is null");
							}
							else
							{
								dictionary.Add(gameObject2.name, gameObject2);
							}
						}
						HashSet<GameObject> hashSet = new HashSet<GameObject>(new GameObjectInstanceIDComparer());
						GameObject gameObject3 = null;
						GameObject gameObject4 = null;
						foreach (KeyValuePair<string, GameObject> keyValuePair in dictionary)
						{
							bool flag7 = keyValuePair.Key.Contains("RockBeam");
							if (flag7)
							{
								hashSet.Add(keyValuePair.Value);
							}
							else
							{
								bool flag8 = keyValuePair.Key.Contains("RockWall");
								if (flag8)
								{
									gameObject3 = keyValuePair.Value;
								}
								else
								{
									bool flag9 = keyValuePair.Key.Contains("RockPilar");
									if (flag9)
									{
										gameObject4 = keyValuePair.Value;
									}
								}
							}
						}
						GameObject gameObject5 = null;
						GameObject gameObject6 = null;
						bool flag10 = hashSet.Count <= 0;
						if (flag10)
						{
							gameObject5 = null;
						}
						else
						{
							bool flag11 = hashSet.Count == 1;
							if (flag11)
							{
								gameObject5 = Enumerable.ToArray<GameObject>(hashSet)[0];
							}
							else
							{
								bool flag12 = hashSet.Count == 2;
								if (flag12)
								{
									float y = Enumerable.ToArray<GameObject>(hashSet)[0].transform.position.y;
									float y2 = Enumerable.ToArray<GameObject>(hashSet)[1].transform.position.y;
									bool flag13 = y > y2;
									if (flag13)
									{
										gameObject5 = Enumerable.ToArray<GameObject>(hashSet)[0];
										gameObject6 = Enumerable.ToArray<GameObject>(hashSet)[1];
									}
									else
									{
										gameObject5 = Enumerable.ToArray<GameObject>(hashSet)[1];
										gameObject6 = Enumerable.ToArray<GameObject>(hashSet)[0];
									}
								}
							}
						}
						orAddComponent.Init(rotate, rotateMode, gameObject5, gameObject6, gameObject3, gameObject4);
					}
				}
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004668 File Offset: 0x00002868
		public void AddDoor(GameObject rotate, CreateGateParent.RotateMode mode, GameObject floorBeam = null, GameObject topBeam = null, GameObject rockWall = null, GameObject extraPillar = null)
		{
			bool flag = rotate == null;
			if (flag)
			{
				RLog.Error("[StoneGate] [CreateGateParent] [AddDoor] Rotate is null");
			}
			else
			{
				bool flag2 = mode == CreateGateParent.RotateMode.None;
				if (flag2)
				{
					RLog.Error("[StoneGate] [CreateGateParent] [AddDoor] Rotate Mode is not valid");
				}
				else
				{
					GameObject gameObject = new GameObject("StoneGate", new System.Type[] { typeof(StoneGateStoreMono) });
					CommonExtensions.SetParent(gameObject, this.StoredParent.transform, false);
					StoneGateStoreMono orAddComponent = TryGetComponentExtensions.GetOrAddComponent<StoneGateStoreMono>(gameObject);
					orAddComponent.Init(rotate, mode, floorBeam, topBeam, rockWall, extraPillar);
				}
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000046E8 File Offset: 0x000028E8
		public void AddDoorNetworkClient(GameObject rotate, params GameObject[] otherGameObjects)
		{
			bool flag = !BoltNetwork.isRunning || !BoltNetwork.isClient;
			if (!flag)
			{
				bool flag2 = rotate == null;
				if (flag2)
				{
					RLog.Error("[StoneGate] [CreateGateParent] [AddDoor] Rotate is null");
				}
				else
				{
					CreateGateParent.RotateMode rotateMode = CreateGateParent.RotateMode.None;
					bool flag3 = rotate.name.Contains("RockPilar");
					if (flag3)
					{
						rotateMode = CreateGateParent.RotateMode.Vertical;
					}
					else
					{
						bool flag4 = rotate.name.Contains("RockBeam");
						if (flag4)
						{
							rotateMode = CreateGateParent.RotateMode.Horizontal;
						}
					}
					bool flag5 = rotateMode == CreateGateParent.RotateMode.None;
					if (flag5)
					{
						RLog.Error("[StoneGate] [CreateGateParent] [AddDoor] Rotate is not a valid object");
					}
					else
					{
						bool flag6 = otherGameObjects.Length != 0;
						if (flag6)
						{
							Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
							foreach (GameObject gameObject in otherGameObjects)
							{
								bool flag7 = gameObject == null;
								if (flag7)
								{
									RLog.Error("[StoneGate] [CreateGateParent] [AddDoor] GameObject is null");
								}
								else
								{
									dictionary.Add(gameObject.name, gameObject);
								}
							}
							HashSet<GameObject> hashSet = new HashSet<GameObject>(new GameObjectInstanceIDComparer());
							GameObject gameObject2 = null;
							GameObject gameObject3 = null;
							foreach (KeyValuePair<string, GameObject> keyValuePair in dictionary)
							{
								bool flag8 = keyValuePair.Key.Contains("RockBeam");
								if (flag8)
								{
									hashSet.Add(keyValuePair.Value);
								}
								else
								{
									bool flag9 = keyValuePair.Key.Contains("RockWall");
									if (flag9)
									{
										gameObject2 = keyValuePair.Value;
									}
									else
									{
										bool flag10 = keyValuePair.Key.Contains("RockPilar");
										if (flag10)
										{
											gameObject3 = keyValuePair.Value;
										}
									}
								}
							}
							GameObject gameObject4 = null;
							GameObject gameObject5 = null;
							bool flag11 = hashSet.Count <= 0;
							if (flag11)
							{
								gameObject4 = null;
							}
							else
							{
								bool flag12 = hashSet.Count == 1;
								if (flag12)
								{
									gameObject4 = Enumerable.ToArray<GameObject>(hashSet)[0];
								}
								else
								{
									bool flag13 = hashSet.Count == 2;
									if (flag13)
									{
										float y = Enumerable.ToArray<GameObject>(hashSet)[0].transform.position.y;
										float y2 = Enumerable.ToArray<GameObject>(hashSet)[1].transform.position.y;
										bool flag14 = y > y2;
										if (flag14)
										{
											gameObject4 = Enumerable.ToArray<GameObject>(hashSet)[0];
											gameObject5 = Enumerable.ToArray<GameObject>(hashSet)[1];
										}
										else
										{
											gameObject4 = Enumerable.ToArray<GameObject>(hashSet)[1];
											gameObject5 = Enumerable.ToArray<GameObject>(hashSet)[0];
										}
									}
								}
							}
							string name = rotate.name;
							string text = ((gameObject4 != null) ? gameObject4.name : null);
							string text2 = ((gameObject5 != null) ? gameObject5.name : null);
							string text3 = ((gameObject2 != null) ? gameObject2.name : null);
							string text4 = ((gameObject3 != null) ? gameObject3.name : null);
							bool flag15 = string.IsNullOrEmpty(text);
							if (flag15)
							{
								text = "NONE";
							}
							bool flag16 = string.IsNullOrEmpty(text2);
							if (flag16)
							{
								text2 = "NONE";
							}
							bool flag17 = string.IsNullOrEmpty(text3);
							if (flag17)
							{
								text3 = "NONE";
							}
							bool flag18 = string.IsNullOrEmpty(text4);
							if (flag18)
							{
								text4 = "NONE";
							}
							EventBase<ClientEvents>.Instance.SendClientEvent(ClientEvents.ClientEvent.CreateStoneGate, name, rotateMode, text, text2, text3, text4);
						}
					}
				}
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004A2C File Offset: 0x00002C2C
		public void AddDoorNetworkClient(string rotateGoName, CreateGateParent.RotateMode mode, string floorBeamGoName, string topBeamGoName, string rockWallGoName, string extraPillarGoName, int childIndex)
		{
			bool flag = !BoltNetwork.isRunning || !BoltNetwork.isClient;
			if (!flag)
			{
				bool flag2 = string.IsNullOrEmpty(rotateGoName) || rotateGoName == "NONE";
				if (flag2)
				{
					RLog.Error("[StoneGate] [CreateGateParent] [AddDoorNetworkClient] Rotate String is null");
				}
				else
				{
					bool flag3 = mode == CreateGateParent.RotateMode.None;
					if (flag3)
					{
						RLog.Error("[StoneGate] [CreateGateParent] [AddDoorNetworkClient] Rotate Mode is not valid");
					}
					else
					{
						bool flag4 = (string.IsNullOrEmpty(floorBeamGoName) || floorBeamGoName == "NONE") && (string.IsNullOrEmpty(topBeamGoName) || topBeamGoName == "NONE") && (string.IsNullOrEmpty(rockWallGoName) || rockWallGoName == "NONE") && (string.IsNullOrEmpty(extraPillarGoName) || extraPillarGoName == "NONE");
						if (flag4)
						{
							RLog.Error("[StoneGate] [CreateGateParent] [AddDoorNetworkClient] At least 1 of the other string must be valid");
						}
						else
						{
							GameObject gameObject = Gates.FindObjectInSpecificScene(rotateGoName, "BlankScene");
							bool flag5 = gameObject == null;
							if (flag5)
							{
								RLog.Error("[StoneGate] [CreateGateParent] [AddDoorNetworkClient] Rotate GameObject is null");
							}
							else
							{
								GameObject gameObject2 = null;
								GameObject gameObject3 = null;
								GameObject gameObject4 = null;
								GameObject gameObject5 = null;
								bool flag6 = string.IsNullOrEmpty(floorBeamGoName) || floorBeamGoName != "NONE";
								if (flag6)
								{
									gameObject2 = Gates.FindObjectInSpecificScene(floorBeamGoName, "BlankScene");
								}
								bool flag7 = string.IsNullOrEmpty(topBeamGoName) || topBeamGoName != "NONE";
								if (flag7)
								{
									gameObject3 = Gates.FindObjectInSpecificScene(topBeamGoName, "BlankScene");
								}
								bool flag8 = string.IsNullOrEmpty(rockWallGoName) || rockWallGoName != "NONE";
								if (flag8)
								{
									gameObject4 = Gates.FindObjectInSpecificScene(rockWallGoName, "BlankScene");
								}
								bool flag9 = string.IsNullOrEmpty(extraPillarGoName) || extraPillarGoName != "NONE";
								if (flag9)
								{
									gameObject5 = Gates.FindObjectInSpecificScene(extraPillarGoName, "BlankScene");
								}
								GameObject gameObject6 = new GameObject("StoneGate", new System.Type[] { typeof(StoneGateStoreMono) });
								CommonExtensions.SetParent(gameObject6, this.StoredParent.transform, false).transform.SetSiblingIndex(childIndex);
								StoneGateStoreMono orAddComponent = TryGetComponentExtensions.GetOrAddComponent<StoneGateStoreMono>(gameObject6);
								bool flag10 = orAddComponent != null;
								if (flag10)
								{
									orAddComponent.Init(gameObject, mode, gameObject2, gameObject3, gameObject4, gameObject5);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004C5C File Offset: 0x00002E5C
		public void AddDoorNetworkHost(GameObject rotate, params GameObject[] otherGameObjects)
		{
			bool flag = !BoltNetwork.isRunning || !BoltNetwork.isServer;
			if (!flag)
			{
				bool flag2 = rotate == null;
				if (flag2)
				{
					RLog.Error("[StoneGate] [CreateGateParent] [AddDoor] Rotate is null");
				}
				else
				{
					CreateGateParent.RotateMode rotateMode = CreateGateParent.RotateMode.None;
					bool flag3 = rotate.name.Contains("RockPilar");
					if (flag3)
					{
						rotateMode = CreateGateParent.RotateMode.Vertical;
					}
					else
					{
						bool flag4 = rotate.name.Contains("RockBeam");
						if (flag4)
						{
							rotateMode = CreateGateParent.RotateMode.Horizontal;
						}
					}
					bool flag5 = rotateMode == CreateGateParent.RotateMode.None;
					if (flag5)
					{
						RLog.Error("[StoneGate] [CreateGateParent] [AddDoor] Rotate is not a valid object");
					}
					else
					{
						StoneGateStoreMono stoneGateStoreMono = null;
						int? num = default(int?);
						GameObject gameObject = new GameObject("StoneGate", new System.Type[] { typeof(StoneGateStoreMono) });
						CommonExtensions.SetParent(gameObject, this.StoredParent.transform, false);
						num = new int?(gameObject.transform.GetSiblingIndex());
						stoneGateStoreMono = TryGetComponentExtensions.GetOrAddComponent<StoneGateStoreMono>(gameObject);
						bool flag6 = num == null;
						if (flag6)
						{
							RLog.Error("[StoneGate] [CreateGateParent] [AddDoorNetworkHost] ChildIndex is null");
						}
						else
						{
							bool flag7 = otherGameObjects.Length != 0;
							if (flag7)
							{
								Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
								foreach (GameObject gameObject2 in otherGameObjects)
								{
									bool flag8 = gameObject2 == null;
									if (flag8)
									{
										RLog.Error("[StoneGate] [CreateGateParent] [AddDoor] GameObject is null");
									}
									else
									{
										dictionary.Add(gameObject2.name, gameObject2);
									}
								}
								HashSet<GameObject> hashSet = new HashSet<GameObject>(new GameObjectInstanceIDComparer());
								GameObject gameObject3 = null;
								GameObject gameObject4 = null;
								foreach (KeyValuePair<string, GameObject> keyValuePair in dictionary)
								{
									bool flag9 = keyValuePair.Key.Contains("RockBeam");
									if (flag9)
									{
										hashSet.Add(keyValuePair.Value);
									}
									else
									{
										bool flag10 = keyValuePair.Key.Contains("RockWall");
										if (flag10)
										{
											gameObject3 = keyValuePair.Value;
										}
										else
										{
											bool flag11 = keyValuePair.Key.Contains("RockPilar");
											if (flag11)
											{
												gameObject4 = keyValuePair.Value;
											}
										}
									}
								}
								GameObject gameObject5 = null;
								GameObject gameObject6 = null;
								bool flag12 = hashSet.Count <= 0;
								if (flag12)
								{
									gameObject5 = null;
								}
								else
								{
									bool flag13 = hashSet.Count == 1;
									if (flag13)
									{
										gameObject5 = Enumerable.ToArray<GameObject>(hashSet)[0];
									}
									else
									{
										bool flag14 = hashSet.Count == 2;
										if (flag14)
										{
											float y = Enumerable.ToArray<GameObject>(hashSet)[0].transform.position.y;
											float y2 = Enumerable.ToArray<GameObject>(hashSet)[1].transform.position.y;
											bool flag15 = y > y2;
											if (flag15)
											{
												gameObject5 = Enumerable.ToArray<GameObject>(hashSet)[0];
												gameObject6 = Enumerable.ToArray<GameObject>(hashSet)[1];
											}
											else
											{
												gameObject5 = Enumerable.ToArray<GameObject>(hashSet)[1];
												gameObject6 = Enumerable.ToArray<GameObject>(hashSet)[0];
											}
										}
									}
								}
								bool flag16 = stoneGateStoreMono != null;
								if (flag16)
								{
									stoneGateStoreMono.Init(rotate, rotateMode, gameObject5, gameObject6, gameObject3, gameObject4);
									EventBase<HostEvents>.Instance.SendHostEvent(HostEvents.HostEvent.CreateStoneGate, rotate.name, rotateMode, (gameObject5 != null) ? gameObject5.name : null, (gameObject6 != null) ? gameObject6.name : null, (gameObject3 != null) ? gameObject3.name : null, (gameObject4 != null) ? gameObject4.name : null, num.Value);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004FD0 File Offset: 0x000031D0
		public void AddDoorNetworkHost(string rotateGoName, CreateGateParent.RotateMode mode, string floorBeamGoName, string topBeamGoName, string rockWallGoName, string extraPillarGoName)
		{
			bool flag = !BoltNetwork.isRunning || !BoltNetwork.isServer;
			if (!flag)
			{
				bool flag2 = string.IsNullOrEmpty(rotateGoName) || rotateGoName == "NONE";
				if (flag2)
				{
					RLog.Error("[StoneGate] [CreateGateParent] [AddDoorNetworkHost] Rotate String is null");
				}
				else
				{
					bool flag3 = mode == CreateGateParent.RotateMode.None;
					if (flag3)
					{
						RLog.Error("[StoneGate] [CreateGateParent] [AddDoorNetworkHost] Rotate Mode is not valid");
					}
					else
					{
						bool flag4 = (string.IsNullOrEmpty(floorBeamGoName) || floorBeamGoName == "NONE") && (string.IsNullOrEmpty(topBeamGoName) || topBeamGoName == "NONE") && (string.IsNullOrEmpty(rockWallGoName) || rockWallGoName == "NONE") && (string.IsNullOrEmpty(extraPillarGoName) || extraPillarGoName == "NONE");
						if (flag4)
						{
							RLog.Error("[StoneGate] [CreateGateParent] [AddDoorNetworkHost] At least 1 of the other string must be valid");
						}
						else
						{
							GameObject gameObject = Gates.FindObjectInSpecificScene(rotateGoName, "BlankScene");
							bool flag5 = gameObject == null;
							if (flag5)
							{
								RLog.Error("[StoneGate] [CreateGateParent] [AddDoorNetworkHost] Rotate GameObject is null");
							}
							else
							{
								GameObject gameObject2 = null;
								GameObject gameObject3 = null;
								GameObject gameObject4 = null;
								GameObject gameObject5 = null;
								bool flag6 = string.IsNullOrEmpty(floorBeamGoName) || floorBeamGoName != "NONE";
								if (flag6)
								{
									gameObject2 = Gates.FindObjectInSpecificScene(floorBeamGoName, "BlankScene");
								}
								bool flag7 = string.IsNullOrEmpty(topBeamGoName) || topBeamGoName != "NONE";
								if (flag7)
								{
									gameObject3 = Gates.FindObjectInSpecificScene(topBeamGoName, "BlankScene");
								}
								bool flag8 = string.IsNullOrEmpty(rockWallGoName) || rockWallGoName != "NONE";
								if (flag8)
								{
									gameObject4 = Gates.FindObjectInSpecificScene(rockWallGoName, "BlankScene");
								}
								bool flag9 = string.IsNullOrEmpty(extraPillarGoName) || extraPillarGoName != "NONE";
								if (flag9)
								{
									gameObject5 = Gates.FindObjectInSpecificScene(extraPillarGoName, "BlankScene");
								}
								GameObject gameObject6 = new GameObject("StoneGate", new System.Type[] { typeof(StoneGateStoreMono) });
								CommonExtensions.SetParent(gameObject6, this.StoredParent.transform, false);
								int? num = default(int?);
								num = new int?(gameObject6.transform.GetSiblingIndex());
								bool flag10 = num == null;
								if (flag10)
								{
									RLog.Error("[StoneGate] [CreateGateParent] [AddDoorNetworkHost] ChildIndex is null");
								}
								else
								{
									StoneGateStoreMono orAddComponent = TryGetComponentExtensions.GetOrAddComponent<StoneGateStoreMono>(gameObject6);
									bool flag11 = orAddComponent != null;
									if (flag11)
									{
										orAddComponent.Init(gameObject, mode, gameObject2, gameObject3, gameObject4, gameObject5);
										EventBase<HostEvents>.Instance.SendHostEvent(HostEvents.HostEvent.CreateStoneGate, rotateGoName, mode, floorBeamGoName, topBeamGoName, rockWallGoName, extraPillarGoName, num.Value);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000524C File Offset: 0x0000344C
		public void RemoveDoorNetworkHost(int childIndex)
		{
			bool flag = !BoltNetwork.isRunning || !BoltNetwork.isServer;
			if (!flag)
			{
				bool flag2 = childIndex < 0;
				if (flag2)
				{
					RLog.Error("[StoneGate] [CreateGateParent] [DestoryDorrNetworkHost] ChildIndex is invalid");
				}
				else
				{
					GameObject gameObject = this.StoredParent.transform.GetChild(childIndex).gameObject;
					bool flag3 = gameObject == null;
					if (flag3)
					{
						RLog.Error("[StoneGate] [CreateGateParent] [DestoryDorrNetworkHost] Gate is null");
					}
					else
					{
						StoneGateStoreMono component = gameObject.GetComponent<StoneGateStoreMono>();
						bool flag4 = component == null;
						if (flag4)
						{
							RLog.Error("[StoneGate] [CreateGateParent] [DestoryDorrNetworkHost] StoneGateMono is null");
						}
						else
						{
							component.DestroyGate(true);
						}
					}
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000052E8 File Offset: 0x000034E8
		public void RemoveDoorNetworkClient(int childIndex)
		{
			bool flag = !BoltNetwork.isRunning || !BoltNetwork.isClient;
			if (!flag)
			{
				bool flag2 = childIndex < 0;
				if (flag2)
				{
					RLog.Error("[StoneGate] [CreateGateParent] [DestoryDorrNetworkClient] ChildIndex is invalid");
				}
				else
				{
					GameObject gameObject = this.StoredParent.transform.GetChild(childIndex).gameObject;
					bool flag3 = gameObject == null;
					if (flag3)
					{
						RLog.Error("[StoneGate] [CreateGateParent] [DestoryDorrNetworkClient] Gate is null");
					}
					else
					{
						StoneGateStoreMono component = gameObject.GetComponent<StoneGateStoreMono>();
						bool flag4 = component == null;
						if (flag4)
						{
							RLog.Error("[StoneGate] [CreateGateParent] [DestoryDorrNetworkClient] StoneGateMono is null");
						}
						else
						{
							component.DestroyGate(false);
						}
					}
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00005384 File Offset: 0x00003584
		public void OpenDoorNetworkHost(int childIndex)
		{
			bool flag = !BoltNetwork.isRunning || !BoltNetwork.isServer;
			if (!flag)
			{
				bool flag2 = childIndex < 0;
				if (flag2)
				{
					RLog.Error("[StoneGate] [CreateGateParent] [OpenDoorNetworkHost] ChildIndex is invalid");
				}
				else
				{
					GameObject gameObject = this.StoredParent.transform.GetChild(childIndex).gameObject;
					bool flag3 = gameObject == null;
					if (flag3)
					{
						RLog.Error("[StoneGate] [CreateGateParent] [OpenDoorNetworkHost] Gate is null");
					}
					else
					{
						StoneGateStoreMono component = gameObject.GetComponent<StoneGateStoreMono>();
						bool flag4 = component == null;
						if (flag4)
						{
							RLog.Error("[StoneGate] [CreateGateParent] [OpenDoorNetworkHost] StoneGateMono is null");
						}
						else
						{
							component.OpenGate(true);
						}
					}
				}
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005420 File Offset: 0x00003620
		public void OpenDoorNetworkClient(int childIndex)
		{
			bool flag = !BoltNetwork.isRunning || !BoltNetwork.isClient;
			if (!flag)
			{
				bool flag2 = childIndex < 0;
				if (flag2)
				{
					RLog.Error("[StoneGate] [CreateGateParent] [OpenDoorNetworkClient] ChildIndex is invalid");
				}
				else
				{
					GameObject gameObject = this.StoredParent.transform.GetChild(childIndex).gameObject;
					bool flag3 = gameObject == null;
					if (flag3)
					{
						RLog.Error("[StoneGate] [CreateGateParent] [OpenDoorNetworkClient] Gate is null");
					}
					else
					{
						StoneGateStoreMono component = gameObject.GetComponent<StoneGateStoreMono>();
						bool flag4 = component == null;
						if (flag4)
						{
							RLog.Error("[StoneGate] [CreateGateParent] [OpenDoorNetworkClient] StoneGateMono is null");
						}
						else
						{
							component.OpenGate(false);
						}
					}
				}
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000054BC File Offset: 0x000036BC
		public void CloseDoorNetworkHost(int childIndex)
		{
			bool flag = !BoltNetwork.isRunning || !BoltNetwork.isServer;
			if (!flag)
			{
				bool flag2 = childIndex < 0;
				if (flag2)
				{
					RLog.Error("[StoneGate] [CreateGateParent] [OpenDoorNetworkHost] ChildIndex is invalid");
				}
				else
				{
					GameObject gameObject = this.StoredParent.transform.GetChild(childIndex).gameObject;
					bool flag3 = gameObject == null;
					if (flag3)
					{
						RLog.Error("[StoneGate] [CreateGateParent] [OpenDoorNetworkHost] Gate is null");
					}
					else
					{
						StoneGateStoreMono component = gameObject.GetComponent<StoneGateStoreMono>();
						bool flag4 = component == null;
						if (flag4)
						{
							RLog.Error("[StoneGate] [CreateGateParent] [OpenDoorNetworkHost] StoneGateMono is null");
						}
						else
						{
							component.CloseGate(true);
						}
					}
				}
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005558 File Offset: 0x00003758
		public void CloseDoorNetworkClient(int childIndex)
		{
			bool flag = !BoltNetwork.isRunning || !BoltNetwork.isClient;
			if (!flag)
			{
				bool flag2 = childIndex < 0;
				if (flag2)
				{
					RLog.Error("[StoneGate] [CreateGateParent] [OpenDoorNetworkClient] ChildIndex is invalid");
				}
				else
				{
					GameObject gameObject = this.StoredParent.transform.GetChild(childIndex).gameObject;
					bool flag3 = gameObject == null;
					if (flag3)
					{
						RLog.Error("[StoneGate] [CreateGateParent] [OpenDoorNetworkClient] Gate is null");
					}
					else
					{
						StoneGateStoreMono component = gameObject.GetComponent<StoneGateStoreMono>();
						bool flag4 = component == null;
						if (flag4)
						{
							RLog.Error("[StoneGate] [CreateGateParent] [OpenDoorNetworkClient] StoneGateMono is null");
						}
						else
						{
							component.CloseGate(false);
						}
					}
				}
			}
		}

		// Token: 0x04000030 RID: 48
		private static CreateGateParent _instance;

		// Token: 0x02000023 RID: 35
		public enum RotateMode : byte
		{
			// Token: 0x0400005A RID: 90
			Vertical,
			// Token: 0x0400005B RID: 91
			Horizontal,
			// Token: 0x0400005C RID: 92
			None
		}
	}
}



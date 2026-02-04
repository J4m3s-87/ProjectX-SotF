using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bolt;
using Il2CppSystem;
using Microsoft.CSharp.RuntimeBinder;
using RedLoader;
using Sons.Crafting.Structures;
using SonsSdk;
using SonsSdk.Building;
using SonsSdk.Networking;
using UnityEngine;

namespace ProjectX.Master.Modules.StoneGate.Structure
{
	// Token: 0x0200000D RID: 13
	internal class StructureBase
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00003792 File Offset: 0x00001992
		// (set) Token: 0x0600003A RID: 58 RVA: 0x0000379A File Offset: 0x0000199A
		internal virtual GameObject SetupGameObject { get; set; } = null;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003B RID: 59 RVA: 0x000037A3 File Offset: 0x000019A3
		// (set) Token: 0x0600003C RID: 60 RVA: 0x000037AB File Offset: 0x000019AB
		internal virtual int StructureId { get; set; } = 0;

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003D RID: 61 RVA: 0x000037B4 File Offset: 0x000019B4
		// (set) Token: 0x0600003E RID: 62 RVA: 0x000037BC File Offset: 0x000019BC
		internal virtual string BlueprintName { get; set; } = null;

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000037C5 File Offset: 0x000019C5
		// (set) Token: 0x06000040 RID: 64 RVA: 0x000037CD File Offset: 0x000019CD
		internal virtual bool RegisterInBook { get; set; } = false;

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000037D6 File Offset: 0x000019D6
		// (set) Token: 0x06000042 RID: 66 RVA: 0x000037DE File Offset: 0x000019DE
		internal virtual Texture2D BookPage { get; set; } = null;

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000037E7 File Offset: 0x000019E7
		// (set) Token: 0x06000044 RID: 68 RVA: 0x000037EF File Offset: 0x000019EF
		internal virtual List<System.Type> AddComponents { get; set; } = null;

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000037F8 File Offset: 0x000019F8
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00003800 File Offset: 0x00001A00
		internal virtual System.Type BoltSetterComponent { get; set; } = null;

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003809 File Offset: 0x00001A09
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00003811 File Offset: 0x00001A11
		internal virtual bool RegisterStructure { get; set; } = true;

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000381A File Offset: 0x00001A1A
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00003822 File Offset: 0x00001A22
		internal virtual bool AddGrassAndSnow { get; set; } = false;

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004B RID: 75 RVA: 0x0000382B File Offset: 0x00001A2B
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00003833 File Offset: 0x00001A33
		internal virtual Vector3? GrassSize { get; set; } = default(Vector3?);

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000383C File Offset: 0x00001A3C
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00003844 File Offset: 0x00001A44
		internal virtual Vector3? SnowSize { get; set; } = default(Vector3?);

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004F RID: 79 RVA: 0x0000384D File Offset: 0x00001A4D
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00003855 File Offset: 0x00001A55
		internal virtual float? MaxPlacementAngle { get; set; } = default(float?);

		// Token: 0x06000051 RID: 81 RVA: 0x00003860 File Offset: 0x00001A60
		internal virtual void SetupStructure(GameObject goToInstantiate)
		{
			Misc.Msg("[StructureBase] [Setup] Setting up structure", false);
			bool flag = this.StructureId == 0;
			if (flag)
			{
				throw new System.InvalidOperationException("[StructureBase] [Setup] StructureId Is 0!");
			}
			bool flag2 = string.IsNullOrEmpty(this.BlueprintName);
			if (flag2)
			{
				throw new System.InvalidOperationException("[StructureBase] [Setup] BlueprintName Is Null Or Empty!");
			}
			bool flag3 = goToInstantiate == null;
			if (flag3)
			{
				throw new System.ArgumentNullException("[StructureBase] [Setup] goToInstantiate Is Null!");
			}
			this.SetupGameObject = UnityEngine.Object.Instantiate<GameObject>(goToInstantiate);
			bool flag4 = this.SetupGameObject == null;
			if (flag4)
			{
				throw new System.InvalidOperationException("[StructureBase] [Setup] SetupGameObject Is Null!");
			}
			CommonExtensions.DontDestroyOnLoad(CommonExtensions.HideAndDontSave(this.SetupGameObject));
			List<Transform> children = CommonExtensions.GetChildren(this.SetupGameObject.transform.GetChild(0));
			for (int i = 0; i < children.Count; i++)
			{
				bool flag5 = children[i] == null;
				if (!flag5)
				{
					string name = children[i].gameObject.name;
					int num = this.ExtractIdFromName(name);
					bool flag6 = num == 0;
					if (!flag6)
					{
						children[i].gameObject.AddComponent<StructureCraftingNodeIngredient>().SetId(num);
					}
				}
			}
			bool flag7 = this.AddComponents != null && this.AddComponents.Count > 0;
			if (flag7)
			{
				for (int j = 0; j < this.AddComponents.Count; j++)
				{
					bool flag8 = this.AddComponents[j] == null;
					if (!flag8)
					{
						object obj = this.SetupGameObject.AddComponent(this.AddComponents[j]);
						try
						{
							((dynamic)obj).isSetupPrefab = true;
						}
						catch { /* Ignore if missing */ }
					}
				}
			}
			bool flag9 = this.BoltSetterComponent != null;
			if (flag9)
			{
				this.SetupGameObject.AddComponent(this.BoltSetterComponent);
			}
			bool addGrassAndSnow = this.AddGrassAndSnow;
			if (addGrassAndSnow)
			{
				this.CleanGrassAndSnow(null, this.GrassSize, this.SnowSize);
			}
			BoltEntity boltEntity = this.SetupGameObject.AddComponent<BoltEntity>();
			NetExtensions.Init(boltEntity, this.StructureId, BoltFactories.RigidbodyState);
			EntityManager.RegisterPrefab(boltEntity, null);
			Misc.Msg("[StructureBase] [Setup] " + this.BlueprintName + " BoltEntity Component Added", false);
			bool registerStructure = this.RegisterStructure;
			if (registerStructure)
			{
				CustomBlueprintManager.TryRegister(new ScrewStructureRegistration(this.SetupGameObject, this.StructureId, this.BlueprintName));
				Misc.Msg("[StructureBase] [Setup] " + this.BlueprintName + " Registered", false);
			}
			bool registerInBook = this.RegisterInBook;
			if (registerInBook)
			{
				CustomBlueprintManager.OnCraftingNodeCreated.Subscribe(new LemonAction<StructureCraftingNode>(this.OnCraftingNodeCreated), 0, false);
				Misc.Msg("[StructureBase] [Setup] " + this.BlueprintName + " Trying To Register In Book...", false);
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003B88 File Offset: 0x00001D88
		internal virtual void OnCraftingNodeCreated(StructureCraftingNode craftingNode)
		{
			bool flag = this.RegisterInBook || this.MaxPlacementAngle != null;
			if (flag)
			{
				bool flag2 = craftingNode == null;
				if (flag2)
				{
					throw new System.ArgumentNullException("[StructureBase] [OnCraftingNodeCreated] craftingNode Is Null!");
				}
				bool flag3 = craftingNode.Recipe == null;
				if (flag3)
				{
					throw new System.ArgumentNullException("[StructureBase] [OnCraftingNodeCreated] craftingNode.Recipe Is Null!");
				}
				bool flag4 = craftingNode.Recipe.Id == this.StructureId;
				if (flag4)
				{
					bool flag5 = this.RegisterInBook && this.BookPage != null;
					if (flag5)
					{
						Misc.Msg("[OnCraftingNodeCreated] Creating Book Page", false);
						CustomBlueprintManager.CreateBookPage(craftingNode.Recipe, null, this.BookPage);
					}
					else
					{
						bool flag6 = this.RegisterInBook && this.BookPage == null;
						if (flag6)
						{
							RLog.Error("[OnCraftingNodeCreated] Failed To Create Book Page!");
						}
						else
						{
							bool flag7 = this.MaxPlacementAngle != null;
							if (flag7)
							{
								// craftingNode.Recipe.ForceUpAngleThreshold = this.MaxPlacementAngle.Value;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003CA8 File Offset: 0x00001EA8
		internal int ExtractIdFromName(string name)
		{
			int num = name.IndexOf('(') + 1;
			int num2 = name.IndexOf(')');
			bool flag = num > 0 && num2 > num;
			if (flag)
			{
				string text = name.Substring(num, num2 - num);
				int num3;
				bool flag2 = int.TryParse(text, out num3);
				if (flag2)
				{
					return num3;
				}
			}
			return 0;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003D04 File Offset: 0x00001F04
		internal virtual void CleanGrassAndSnow(GameObject addToObj = null, Vector3? grassSize = null, Vector3? snowSize = null)
		{
			bool flag = addToObj == null;
			if (flag)
			{
				addToObj = this.SetupGameObject;
			}
			bool flag2 = this.SetupGameObject == null;
			if (flag2)
			{
				Misc.Msg("[StructureBase] [CleanGrassAndSnow] Can't add to null object", false);
			}
			else
			{
				PrefabId storageFirewoodStructure = BoltPrefabs.StorageFirewoodStructure;
				PrefabId prefabId = new PrefabId(0);
				bool flag3 = storageFirewoodStructure == prefabId;
				if (flag3)
				{
					Misc.Msg("[StructureBase] [CleanGrassAndSnow] Can't find prefabId", false);
				}
				else
				{
					GameObject gameObject = PrefabDatabase.Find(storageFirewoodStructure);
					bool flag4 = gameObject == null;
					if (flag4)
					{
						Misc.Msg("[StructureBase] [CleanGrassAndSnow] Can't find prefabId", false);
					}
					else
					{
						GameObject gameObject2 = gameObject.transform.Find("GrassRemover").gameObject;
						bool flag5 = gameObject2 != null;
						if (flag5)
						{
							GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
							bool flag6 = grassSize != null;
							if (flag6)
							{
								gameObject3.transform.localScale = grassSize.Value;
							}
							CommonExtensions.SetParent(gameObject3, addToObj.transform, false);
							GameObject gameObject4 = gameObject.transform.Find("SnowRemover").gameObject;
							bool flag7 = gameObject4 != null;
							if (flag7)
							{
								GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(gameObject4);
								bool flag8 = snowSize != null;
								if (flag8)
								{
									gameObject5.transform.localScale = snowSize.Value;
								}
								CommonExtensions.SetParent(gameObject5, addToObj.transform, false);
								GameObject gameObject6 = gameObject.transform.Find("StructureEnvironmentCleaner").gameObject;
								bool flag9 = gameObject6 != null;
								if (flag9)
								{
									GameObject gameObject7 = UnityEngine.Object.Instantiate<GameObject>(gameObject6);
									CommonExtensions.SetParent(gameObject7, addToObj.transform, false);
								}
								else
								{
									Misc.Msg("[StructureBase] [CleanGrassAndSnow] Can't find StructureEnvironmentCleaner", false);
								}
							}
							else
							{
								Misc.Msg("[StructureBase] [CleanGrassAndSnow] Can't find SnowRemover", false);
							}
						}
						else
						{
							Misc.Msg("[StructureBase] [CleanGrassAndSnow] Can't find GrassRemover", false);
						}
					}
				}
			}
		}
	}
}



using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RedLoader;
using SonsSdk;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectX.Master.Modules.StoneGate {
	// Token: 0x02000004 RID: 4
	internal class Assets
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002084 File Offset: 0x00000284
		public static Assets Instance
		{
			get
			{
				bool flag = Assets._instance == null;
				if (flag)
				{
					Assets._instance = new Assets();
				}
				return Assets._instance;
			}
		}

		public GameObject StoneGateTool { get; private set; }
		public GameObject StoneGateToolUI { get; private set; }

		public void LoadAssets()
		{
			if (this._loaded) return;

			string dataPath = Application.dataPath;
			string fullName = Directory.GetParent(dataPath).FullName;
			string modsPath = Path.Combine(fullName, "Mods");
			
			// Check all possible deployment paths (Owner, Master, Client, legacy)
			string[] editionFolders = { "ProjectX.Owner", "ProjectX.Master", "ProjectX.Client", "StoneGate" };
			string bundlePath = null;
			
			foreach (string folder in editionFolders)
			{
				string candidate;
				if (folder == "StoneGate")
					candidate = Path.Combine(modsPath, folder, "stonegate");
				else
					candidate = Path.Combine(modsPath, folder, "Assets", "StoneGate", "stonegate");
				
				if (File.Exists(candidate))
				{
					bundlePath = candidate;
					RLog.Msg($"[StoneGate] Found assets at: {candidate}");
					break;
				}
			}
			
			if (bundlePath == null)
			{
				RLog.Error($"[StoneGate] Assets not found in any expected location under {modsPath}");
				return;
			}

			AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);
			if (assetBundle == null)
			{
				RLog.Error("[StoneGate] AssetBundle Load Failed");
				return;
			}

			this.StoneGateTool = assetBundle.LoadAsset<GameObject>("StoneGateTool");
			if (this.StoneGateTool == null)
			{
				RLog.Error("[StoneGate] StoneGateTool Asset Not Found");
			}
			else
			{
				CommonExtensions.DontDestroyOnLoad(CommonExtensions.HideAndDontSave(this.StoneGateTool));
				this.StoneGateTool.GetComponent<Rigidbody>().isKinematic = true;
				this.StoneGateTool.GetComponent<Rigidbody>().useGravity = false;
				
				Transform transform = TransformDeepChildExtension.FindDeepChild(this.StoneGateTool.transform, "Stick (392)");
				List<Transform> children = CommonExtensions.GetChildren(transform);
				foreach (Transform transform2 in children)
				{
					// Fixed TryGetComponent
					MeshCollider meshCollider = transform2.GetComponent<MeshCollider>();
					if (meshCollider != null)
					{
						Object.Destroy(meshCollider);
						Misc.Msg("[StoneGate] Destroyed MeshCollider on " + transform2.gameObject.name, false);
					}
				}
				
				// Safe Manual Recursion
				List<BoxCollider> boxColliders = StoneGateUtils.GetComponentsInChildrenRecursive<BoxCollider>(this.StoneGateTool.transform);
				foreach(var bc in boxColliders)
				{
					bc.isTrigger = true;
				}
				
				List<CapsuleCollider> list = StoneGateUtils.GetComponentsInChildrenRecursive<CapsuleCollider>(this.StoneGateTool.transform);
				foreach (CapsuleCollider capsuleCollider in list)
				{
					Object.Destroy(capsuleCollider);
				}
				
				this.StoneGateToolUI = assetBundle.LoadAsset<GameObject>("StoneGateToolUI");
				if (this.StoneGateToolUI == null)
				{
					RLog.Error("[StoneGate] StoneGateToolUI Asset Not Found");
				}
				else
				{
					CommonExtensions.DontDestroyOnLoad(CommonExtensions.HideAndDontSave(this.StoneGateToolUI));
					this.StoneGateToolUI.SetActive(false);
					assetBundle.Unload(false);
					RLog.Msg("[StoneGate] All Assets Loaded Successfully");
					this._loaded = true;
				}
			}
		}

		public bool IsLoaded()
		{
			return this._loaded;
		}

		public string GetStoneGateToolPath()
		{
			string dataPath = Application.dataPath;
			string fullName = Directory.GetParent(dataPath).FullName;
			string modsPath = Path.Combine(fullName, "Mods");
			string[] folders = { "ProjectX.Owner", "ProjectX.Master", "ProjectX.Client" };
			foreach (string f in folders)
			{
				string p = Path.Combine(modsPath, f, "Assets", "StoneGate", "StoneGateIcon.png");
				if (File.Exists(p)) return p;
			}
			return Path.Combine(modsPath, "StoneGate", "StoneGateIcon.png");
		}

		public string GetOpenCloseIconPath()
		{
			string dataPath = Application.dataPath;
			string fullName = Directory.GetParent(dataPath).FullName;
			string modsPath = Path.Combine(fullName, "Mods");
			string[] folders = { "ProjectX.Owner", "ProjectX.Master", "ProjectX.Client" };
			foreach (string f in folders)
			{
				string p = Path.Combine(modsPath, f, "Assets", "StoneGate", "OpenCloseIcon.png");
				if (File.Exists(p)) return p;
			}
			return Path.Combine(modsPath, "StoneGate", "OpenCloseIcon.png");
		}

		private static Assets _instance;
		private bool _loaded;
	}
}

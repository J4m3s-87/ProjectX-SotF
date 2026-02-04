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
			
			// New Path
			string projectXDetails = Path.Combine(modsPath, "ProjectX.Master", "Assets", "StoneGate");
			string bundlePath = Path.Combine(projectXDetails, "stonegate");
			
			// Fallback
			if (!File.Exists(bundlePath))
			{
				string oldPath = Path.Combine(modsPath, "StoneGate", "stonegate");
				if (File.Exists(oldPath))
				{
					bundlePath = oldPath;
					RLog.Msg("[StoneGate] Using legacy asset path.");
				}
				else
				{
					RLog.Error($"[StoneGate] Assets not found at {bundlePath} or {oldPath}");
					return;
				}
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
			string newPath = Path.Combine(modsPath, "ProjectX.Master", "Assets", "StoneGate", "StoneGateIcon.png");
			
			if (File.Exists(newPath)) return newPath;
			return Path.Combine(modsPath, "StoneGate", "StoneGateIcon.png");
		}

		public string GetOpenCloseIconPath()
		{
			string dataPath = Application.dataPath;
			string fullName = Directory.GetParent(dataPath).FullName;
			string modsPath = Path.Combine(fullName, "Mods");
			string newPath = Path.Combine(modsPath, "ProjectX.Master", "Assets", "StoneGate", "OpenCloseIcon.png");
			
			if (File.Exists(newPath)) return newPath;
			return Path.Combine(modsPath, "StoneGate", "OpenCloseIcon.png");
		}

		private static Assets _instance;
		private bool _loaded;
	}
}

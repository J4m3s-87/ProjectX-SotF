using System;
using System.Drawing;
using Endnight.Utilities;
using Il2CppInterop.Runtime.Injection;
using RedLoader;
using Sons.Items.Core;
using SonsSdk;
using SonsSdk.Attributes;
using ProjectX.Master.Modules.StoneGate.Mono;
using ProjectX.Master.Modules.StoneGate.Network;
using ProjectX.Master.Modules.StoneGate.Objects;
using ProjectX.Master.Modules.StoneGate.Saving;
using ProjectX.Master.Modules.StoneGate.Structure;
using ProjectX.Master.Modules.StoneGate.Testing;
using SUI;
using UnityEngine;
using Object = UnityEngine.Object;
using TheForest.Utils;

namespace ProjectX.Master.Modules.StoneGate 
{
	// Converted from main Mod class to Module
	public static class StoneGateModule
	{
		// Static references
		public const int ToolItemId = 751152;
		public static ItemData stoneGateCreatorItemData;
		public static GameObject stoneGateCreatorPrefab;
		public static GameObject stoneGateCreatorHeldPrefab;
		public static GameObject stoneGateCreatorPickupPrefab;
		public static Texture2D stoneGateCreatorTexture;
		internal static GameObject StoneGateToolUI;
		internal static Texture2D stoneGateOpenCloseIcon;
		internal static bool isStoneGateToolOneTimeUse = true;

		// Replaces OnInitializeMod — server-only lightweight init
		/// <summary>
		/// Server-only init: registers network events + save system.
		/// No assets, UI, keybinds, or tool prefabs.
		/// </summary>
		public static void InitServer()
		{
			RLog.Msg("[StoneGate] Server-side init (network + save only)");
			
			// IL2CPP type registration (needed for networking MonoBehaviours)
			ClassInjector.RegisterTypeInIl2Cpp<StoneGateStoreMono>();
			ClassInjector.RegisterTypeInIl2Cpp<ProjectX.Master.Modules.StoneGate.Network.CustomEventHandler>();
			
			// Register Bolt network event types
			ProjectX.Master.Modules.StoneGate.Network.Manager.Register();
			ProjectX.Master.Modules.StoneGate.Network.Manager.RegisterEventHandlers();
			
			// Save system
			var manager = new ProjectX.Master.Modules.StoneGate.Saving.Manager();
			try { SonsSaveTools.Register<ProjectX.Master.Modules.StoneGate.Saving.Manager.GatesManager>(manager); }
			catch (Exception) { /* Already registered */ }
			
			// Gate state tracking
			CreateGateParent instance = CreateGateParent.Instance;
		}

		// Replaces OnInitializeMod — full client init
		public static void Init()
		{
			Config.Init();
			RLog.Msg("[StoneGate] Initializing (Fix V3 - Prefab/Save/Net Patched)");
			Assets.Instance.LoadAssets();
			StoneGateModule.stoneGateCreatorTexture = AssetLoaders.LoadTexture(Assets.Instance.GetStoneGateToolPath());
			StoneGateModule.stoneGateCreatorTexture.hideFlags = (HideFlags)61;
			StoneGateModule.stoneGateOpenCloseIcon = AssetLoaders.LoadTexture(Assets.Instance.GetOpenCloseIconPath());
			StoneGateModule.stoneGateOpenCloseIcon.hideFlags = (HideFlags)61;
			
			ClassInjector.RegisterTypeInIl2Cpp<StoneGateItemMono>();
			ClassInjector.RegisterTypeInIl2Cpp<StoneGateStoreMono>();
			
			ProjectX.Master.Modules.StoneGate.Network.Manager.Register();
			
			bool flag = Assets.Instance.IsLoaded();
			if (flag)
			{
				StoneGateModule.stoneGateCreatorHeldPrefab = Assets.Instance.StoneGateTool;
				StoneGateModule.stoneGateCreatorHeldPrefab.transform.localScale = Vector3.one * 2f;
				StoneGateModule.stoneGateCreatorPrefab = CommonExtensions.HideAndDontSave(CommonExtensions.DontDestroyOnLoad(CommonExtensions.Instantiate(StoneGateModule.stoneGateCreatorHeldPrefab, false)));
				// Manual Recursion using Utils
				var renderers = StoneGateUtils.GetComponentsInChildrenRecursive<MeshRenderer>(StoneGateModule.stoneGateCreatorPrefab.transform);
				foreach (MeshRenderer meshRenderer in renderers)
				{
					if (meshRenderer.gameObject.GetComponent<Collider>() == null)
					{
						TryGetComponentExtensions.GetOrAddComponent<BoxCollider>(meshRenderer.gameObject);
					}
					meshRenderer.sharedMaterial.SetFloat("_EnableSnow", 0f);
				}
				StoneGateModule.stoneGateCreatorHeldPrefab.AddComponent<StoneGateItemMono>();
				Misc.Msg("StoneGateTool Set", false);
			}
			else
			{
				RLog.Error("Asset Not Loaded");
			}
		}

		// Replaces OnSdkInitialized
		public static void OnSdkInitialized()
		{
			// SUI-dependent UI creation - guarded to prevent crashes
			try
			{
				StoneGateUi.Create();
			}
			catch (System.Exception ex)
			{
				RLog.Warning($"[StoneGate] UI creation failed (SUI not available): {ex.Message}");
				RLog.Msg("[StoneGate] Core functionality will still work - UI panel disabled.");
			}
			// SettingsRegistry... (Managed by ProjectX Config)
			
			SdkEvents.OnGameActivated.Subscribe(new LemonAction(OnFirstGameActivation), 0, false);
			// OnAfterSpawn is now called by RedLoader via IOnAfterSpawnReceiver on ProjectXMaster
			// (matching the original standalone StoneGate mod's lifecycle exactly)
			
			StoneGateModule.StoneGateToolUI = Object.Instantiate<GameObject>(Assets.Instance.StoneGateToolUI);
			CommonExtensions.HideAndDontSave(CommonExtensions.DontDestroyOnLoad(StoneGateModule.StoneGateToolUI));
			
			if (StoneGateModule.StoneGateToolUI == null)
			{
				RLog.Error("[StoneGate] StoneGateToolUI Asset Not Found");
			}
			else
			{
				RLog.Msg("[StoneGate] StoneGateToolUI Asset Found");
				StoneGateModule.StoneGateToolUI.SetActive(false);
				Misc.Msg("StoneGateToolUI Set", false);
			}
			
			ProjectX.Master.Modules.StoneGate.Saving.Manager manager = new ProjectX.Master.Modules.StoneGate.Saving.Manager();
			try 
			{
				SonsSaveTools.Register<ProjectX.Master.Modules.StoneGate.Saving.Manager.GatesManager>(manager);
			}
			catch(Exception)
			{
				// Already registered, safe to ignore
			}
			
			CreateGateParent instance = CreateGateParent.Instance;
			
			// Keybinds are now registered in StoneGate Config.Init()
		}

		// Replaces OnGameStart
		public static void OnGameStart()
		{
			ProjectX.Master.Modules.StoneGate.Network.Manager.RegisterEventHandlers();
		}

		/// <summary>
		/// Safety method to ensure StoneGate UI is hidden when not actively using the tool.
		/// Uses actual equipped item ID check instead of ActiveItem.active to avoid race
		/// conditions during animation cycles where the MonoBehaviour is briefly disabled.
		/// </summary>
		private static int _safetyFrameCounter = 0;
		private static float _lastActiveTime = 0f;
		public static void EnsureUIHidden()
		{
			// Throttle: only check every 60 frames (~1 second at 60fps)
			if (++_safetyFrameCounter < 60) return;
			_safetyFrameCounter = 0;
			
			// Track when we last saw the tool as active
			if (ActiveItem.active != null)
			{
				_lastActiveTime = Time.time;
				return; // Tool is active, nothing to do
			}
			
			// Grace period: don't force-close within 3 seconds of the tool being active
			// This prevents false triggers during animation disable/enable cycles
			if (Time.time - _lastActiveTime < 3f) return;
			
			// Tool is genuinely not equipped — hide UI
			if (StoneGateToolUI != null && StoneGateToolUI.activeSelf)
			{
				StoneGateToolUI.SetActive(false);
				StoneGateUi.CloseMainPanel();
				RLog.Msg("[StoneGate] Safety: Force-closed UI (tool not equipped)");
			}
		}

		// Helper removed in favor of StoneGateUtils
		// Matches original: OnFirstGameActivation creates ItemData ONLY.
		// ItemBuilder + Recipe run later in OnAfterSpawn (when crafting systems are ready).
		private static void OnFirstGameActivation()
		{
			try
			{
				// Idempotent: skip if already initialized this session
				if (StoneGateModule.stoneGateCreatorItemData != null)
				{
					RLog.Msg("[StoneGate] OnGameActivation — ItemData already exists, skipping");
					return;
				}
				
				RLog.Msg("[StoneGate] OnGameActivation starting...");

				// Try to create and register. If already registered (save reload), retrieve existing.
				StoneGateModule.stoneGateCreatorItemData = ItemTools.CreateAndRegisterItem(751152, "Stone Gate Creator", 1, null, "Create Stone Gates");
				
				if (StoneGateModule.stoneGateCreatorItemData == null)
				{
					// Item already registered from previous session — retrieve it
					RLog.Msg("[StoneGate] Item 751152 already registered, retrieving existing ItemData...");
					StoneGateModule.stoneGateCreatorItemData = ItemDatabaseManager.ItemById(751152);
					
					if (StoneGateModule.stoneGateCreatorItemData == null)
					{
						RLog.Error("[StoneGate] Failed to retrieve ItemData for id=751152!");
						return;
					}
					RLog.Msg($"[StoneGate] Retrieved existing ItemData, id={StoneGateModule.stoneGateCreatorItemData._id}");
				}
				else
				{
					RLog.Msg($"[StoneGate] ItemData created, id={StoneGateModule.stoneGateCreatorItemData._id}");
				}

				ItemDataExtensions.SetIcon(StoneGateModule.stoneGateCreatorItemData, StoneGateModule.stoneGateCreatorTexture);
				ItemDataExtensions.SetupHeld(StoneGateModule.stoneGateCreatorItemData, 0, new AnimatorVariables[] { (AnimatorVariables)12 }, (ItemUiData.LeftClickCommands)1, 0, (ItemData.GuiType)1);
				StoneGateModule.stoneGateCreatorPickupPrefab = StoneGateModule.stoneGateCreatorPrefab;
				StoneGateModule.stoneGateCreatorItemData._heldPrefab = StoneGateModule.stoneGateCreatorHeldPrefab.transform;
				
				RLog.Msg($"[StoneGate] _heldPrefab set to: {StoneGateModule.stoneGateCreatorItemData._heldPrefab?.name ?? "NULL"}");
			}
			catch (Exception ex)
			{
				RLog.Error($"[StoneGate] OnGameActivation FAILED: {ex}");
			}
		}

		// Called by ProjectXMaster.OnAfterSpawn() via IOnAfterSpawnReceiver
		// Matches original: ItemBuilder + Recipe registration runs HERE (crafting systems ready)
		// Then world entry + save loading
		public static void OnAfterSpawn()
		{
			// Block 1: Register /px stonegate command to give player the StoneGate tool
			// NOTE: AddInventoryItem causes an engine-level native crash when the backpack opens
			// (confirmed bug — affects original standalone mod too). Using /px command instead.
			try
			{
				RLog.Msg("[StoneGate] OnAfterSpawn — Registering /px stonegate command");
				
				ProjectX.Master.Modules.Network.CommandBridge.RegisterCommand("stonegate", (string senderSteamId, string[] args) =>
				{
					try
					{
#if !SERVER
						int itemId = StoneGateModule.stoneGateCreatorItemData._id;
						RLog.Msg($"[StoneGate] /px stonegate: Equipping tool (id={itemId}) for {senderSteamId}");
						
						// Parent to player's camera so it follows their view (acts like held item)
						var heldPrefab = ItemTools.GetHeldPrefab(itemId);
						if (heldPrefab != null)
						{
							var cam = UnityEngine.Camera.main.transform;
							var toolInstance = Object.Instantiate(heldPrefab.gameObject, cam);
							toolInstance.transform.localPosition = new Vector3(0.3f, -0.3f, 0.5f);
							toolInstance.transform.localRotation = Quaternion.identity;
							toolInstance.transform.localScale = Vector3.one;
							toolInstance.SetActive(true);
							RLog.Msg("[StoneGate] Tool parented to camera");
							SonsTools.ShowMessage("Stone Gate tool equipped! LMB=Mark, C=Mode, E=Complete", 5f);
						}
						else
						{
							RLog.Error("[StoneGate] Held prefab is null");
							SonsTools.ShowMessage("Stone Gate tool not ready. Try after loading.", 3f);
						}
#else
						RLog.Msg("[StoneGate] /px stonegate: Server-side — no action taken");
#endif
					}
					catch (System.Exception ex)
					{
						RLog.Error($"[StoneGate] /px stonegate failed: {ex.Message}");
						RLog.Warning($"[StoneGate] Stack: {ex.StackTrace}");
					}
				});
				
				RLog.Msg(System.Drawing.Color.SeaGreen, "[ StoneGate: Type '/px stonegate' in chat to get the tool ]");
			}
			catch (System.Exception ex)
			{
				RLog.Error($"[StoneGate] OnAfterSpawn command registration failed: {ex.Message}");
				RLog.Warning($"[StoneGate] Stack: {ex.StackTrace}");
			}
			
			// Block 2: World entry and save loading (independent — NRE here shouldn't break item registration)
			try
			{
				RLog.Msg("[StoneGate] OnAfterSpawn — world entry + save loading");
				
				if (CustomEventHandler.Instance != null)
				{
					CustomEventHandler.Instance.OnEnterWorld();
				}
				else
				{
					RLog.Warning("[StoneGate] CustomEventHandler.Instance is null — skipping OnEnterWorld");
				}
				
				bool isMultiplayerClient = BoltNetwork.isRunning && BoltNetwork.isClient;
				if (isMultiplayerClient)
				{
					Misc.Msg("[Loading] Skipped Loading StoneGates On Multiplayer Client", false);
				}
				else
				{
					while (Load.deferredLoadQueue.Count > 0)
					{
						ProjectX.Master.Modules.StoneGate.Saving.Manager.GatesManager gatesManager = Load.deferredLoadQueue.Dequeue();
						Load.ProcessLoadData(gatesManager);
					}
				}
			}
			catch (System.Exception ex)
			{
				RLog.Warning($"[StoneGate] OnAfterSpawn world-entry failed: {ex.Message}");
				RLog.Warning($"[StoneGate] World-entry stack: {ex.StackTrace}");
			}
		}
	}
}

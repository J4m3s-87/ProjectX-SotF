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

		// Replaces OnInitializeMod
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
			
			SdkEvents.OnGameActivated.Subscribe(new LemonAction(OnFirstGameActivation), 0, true);
			
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
		private static void OnFirstGameActivation()
		{
			try
			{
				RLog.Msg("[StoneGate] OnFirstGameActivation starting...");
				RLog.Msg($"[StoneGate] stoneGateCreatorPrefab null? {StoneGateModule.stoneGateCreatorPrefab == null}");
				RLog.Msg($"[StoneGate] stoneGateCreatorHeldPrefab null? {StoneGateModule.stoneGateCreatorHeldPrefab == null}");
				RLog.Msg($"[StoneGate] stoneGateCreatorTexture null? {StoneGateModule.stoneGateCreatorTexture == null}");

				StoneGateModule.stoneGateCreatorItemData = ItemTools.CreateAndRegisterItem(751152, "Stone Gate Creator", 1, null, "Create Stone Gates");
				RLog.Msg($"[StoneGate] ItemData created, id={StoneGateModule.stoneGateCreatorItemData?._id}");

				ItemDataExtensions.SetIcon(StoneGateModule.stoneGateCreatorItemData, StoneGateModule.stoneGateCreatorTexture);
				ItemDataExtensions.SetupHeld(StoneGateModule.stoneGateCreatorItemData, 0, new AnimatorVariables[] { (AnimatorVariables)12 }, (ItemUiData.LeftClickCommands)1, 0, (ItemData.GuiType)1);
				StoneGateModule.stoneGateCreatorPickupPrefab = StoneGateModule.stoneGateCreatorPrefab;
				StoneGateModule.stoneGateCreatorItemData._heldPrefab = StoneGateModule.stoneGateCreatorHeldPrefab.transform;
				
				RLog.Msg($"[StoneGate] _heldPrefab set to: {StoneGateModule.stoneGateCreatorItemData._heldPrefab?.name ?? "NULL"}");
				RLog.Msg($"[StoneGate] _heldPrefab has StoneGateItemMono? {StoneGateModule.stoneGateCreatorHeldPrefab?.GetComponent<StoneGateItemMono>() != null}");
				
				OnAfterSpawn();
			}
			catch (Exception ex)
			{
				RLog.Error($"[StoneGate] OnFirstGameActivation FAILED: {ex}");
			}
		}

		private static void OnAfterSpawn()
		{
			try
			{
				// Null safety check
				if (StoneGateModule.stoneGateCreatorPrefab == null || StoneGateModule.stoneGateCreatorItemData == null)
				{
					RLog.Warning("[StoneGate] OnAfterSpawn skipped - prefab or itemData is null");
					return;
				}
				
				new ItemTools.ItemBuilder(StoneGateModule.stoneGateCreatorPrefab, StoneGateModule.stoneGateCreatorItemData, false)
					.AddInventoryItem(Array.Empty<Vector3>()).AddIngredientItem(Array.Empty<Vector3>()).AddCraftingResultItem(Array.Empty<Vector3>())
					.SetupHeld(new Vector3?(new Vector3(0f, 0f, 0f)), new Vector3?(new Vector3(0f, 0f, 0f)))
					.SetupPickup(StoneGateModule.stoneGateCreatorPickupPrefab)
					.Recipe.AddIngredient(392, 2, false).AddIngredient(393, 2, false).AddResult(StoneGateModule.stoneGateCreatorItemData._id)
					.Animation("CraftCraftedArrows")
					.BuildAndAdd();
					
				new ItemTools.RecipeBuilder().AddIngredient(StoneGateModule.stoneGateCreatorItemData._id, 2, false).AddResult(392).BuildAndAdd();
				
				RLog.Msg(System.Drawing.Color.SeaGreen, "[ ADDED ITEM: StoneGateTool]");
				
				// Saving loading logic
				// Settings.logSavingSystem check...
				
				if (CustomEventHandler.Instance != null)
				{
					CustomEventHandler.Instance.OnEnterWorld();
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
				RLog.Warning($"[StoneGate] OnAfterSpawn failed: {ex.Message}");
			}
		}
	}
}

#if !SERVER
using System;
using UnityEngine;
using UnityEngine.UI;
using RedLoader;
using SonsSdk;
using Sons.Gui;
using Sons.Items.Core;
using TheForest.Utils;
using TheForest.Items.Inventory;
using Object = UnityEngine.Object;
using Il2CppInterop.Runtime;

namespace ProjectX.Master.Modules.Hotbar
{
    /// <summary>
    /// Hotbar module - Safe implementation with crash guards.
    /// The original SonsHotbar crashes when accessing _hotkeySaveData during inventory transitions.
    /// </summary>
    public static class HotbarModule
    {
        private static Transform _hotbarRoot;
        private static Canvas _hotbarCanvas;
        private static ItemHotkeyController _hotkeyController;
        private static bool _initialized;

        public static void Init()
        {
            // Assets are loaded automatically by SonsSdk via ModAssets class attributes
        }

        public static void OnSdkInitialized()
        {
            // SonsHotbarUi.Create() was called here in original - it was empty
        }

        public static void OnGameStart()
        {
            try
            {
                if (ModAssets.HotbarPrefab == null)
                {
                    RLog.Warning("[Hotbar] HotbarPrefab is null");
                    return;
                }

                RLog.Msg($"[Hotbar] Assets available. Prefab: {ModAssets.HotbarPrefab != null}, Icon: {ModAssets.missingIcon != null}");

                // Exactly like original
                _hotbarRoot = Object.Instantiate(ModAssets.HotbarPrefab).transform.GetChild(0).transform;
                _hotbarCanvas = _hotbarRoot.transform.root.GetComponent<Canvas>();
                
                // Get controller from LocalPlayer - use safe access
                if (LocalPlayer._instance != null)
                {
                    _hotkeyController = LocalPlayer._instance.gameObject.GetComponentInChildren<ItemHotkeyController>();
                }

                if (_hotkeyController != null)
                {
                    RLog.Msg("[Hotbar] Controller found on game start.");
                }
                else
                {
                    RLog.Warning("[Hotbar] Controller not found on game start.");
                }
                
                _initialized = true;
                RLog.Msg("[Hotbar] Hotbar UI instantiated.");
            }
            catch (Exception ex)
            {
                RLog.Error($"[Hotbar] OnGameStart error: {ex}");
            }
        }

        public static void OnUpdate()
        {
            // Guard: not initialized
            if (!_initialized) return;
            if (_hotbarCanvas == null) return;
            
            try
            {
                // Safe pause check (avoid _instance._isActive which can crash)
                bool isPaused = PauseMenu.IsActive;
                
                if (isPaused)
                {
                    _hotbarCanvas.enabled = false;
                    return;
                }
                
                _hotbarCanvas.enabled = true;
                
                // Guard: controller must exist
                if (_hotkeyController == null) return;
                
                // Safe access to hotkey save data - THIS is the crash source
                HotkeySaveData saveData;
                try
                {
                    saveData = _hotkeyController._hotkeySaveData;
                    if (saveData == null) return;
                }
                catch
                {
                    // _hotkeySaveData access crashes during inventory transitions
                    return;
                }
                
                // Guard: lists must exist
                if (saveData.RightHandItemIds == null) return;
                if (saveData.LeftHandItemIds == null) return;
                
                // Update slots
                for (int i = 0; i < saveData.RightHandItemIds.Count; i++)
                {
                    try
                    {
                        UpdateSlot(i, saveData);
                    }
                    catch
                    {
                        // Individual slot update failure - skip
                    }
                }
            }
            catch
            {
                // Silently handle any other crash - hotbar is non-essential
            }
        }

        private static void UpdateSlot(int slotIndex, HotkeySaveData saveData)
        {
            Transform slotTransform = _hotbarRoot.GetChild(GetSlotId(slotIndex));
            
            int leftId = saveData.LeftHandItemIds[slotIndex];
            int rightId = saveData.RightHandItemIds[slotIndex];
            
            Sprite sprite = ModAssets.missingIcon;
            
            if (rightId != -1 || leftId != -1)
            {
                PlayerInventory inventory = LocalPlayer._instance?._inventory;
                if (inventory == null) return;
                
                int itemId = rightId != -1 ? rightId : leftId;
                
                // Check if player has the item
                bool hasItem = inventory._itemInstanceManager.HaveAny(itemId);
                
                if (hasItem)
                {
                    // TODO: Icon loading needs texture conversion research
                    // For now just indicate slot is occupied by leaving missingIcon as-is
                    // The original used: itemData.UiData._icon.Cast<Texture2D>()
                }
            }
            
            var imageComponent = slotTransform.GetChild(1)?.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = sprite;
            }
        }

        private static int GetSlotId(int index)
        {
            return index == 0 ? 9 : index - 1;
        }

        private static Sprite ConvertTextureToSprite(Texture2D texture)
        {
            if (texture == null) return ModAssets.missingIcon;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
#endif

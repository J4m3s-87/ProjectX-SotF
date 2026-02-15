#if !SERVER
using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime;
using UnityEngine;
using UnityEngine.UI;
using RedLoader;
using SonsSdk;
using Sons.Gui;
using Sons.Items.Core;
using TheForest.Utils;
using TheForest.Items.Inventory;
using Object = UnityEngine.Object;

namespace ProjectX.Master.Modules.Hotbar
{
    /// <summary>
    /// Hotbar module — displays assigned hotkey item icons on-screen.
    /// Matches the original SonsHotbar mod logic exactly, with sprite caching added.
    /// </summary>
    public static class HotbarModule
    {
        private static Transform _hotbarRoot;
        private static Canvas _hotbarCanvas;
        private static ItemHotkeyController _hotkeyController;
        private static bool _initialized;

        // Sprite cache: item ID → Sprite (avoid re-creating every frame)
        private static readonly Dictionary<int, Sprite> _spriteCache = new Dictionary<int, Sprite>();

        // Frame throttling
        private const int UPDATE_INTERVAL = 10;
        private static int _frameCounter;

        public static void Init()
        {
            RLog.Msg("[Hotbar] Module initialized.");
        }

        public static void OnGameStart()
        {
            try
            {
                if (ModAssets.HotbarPrefab == null)
                {
                    RLog.Warning("[Hotbar] HotbarPrefab is null.");
                    return;
                }

                // Exactly like the original SonsHotbar
                _hotbarRoot = Object.Instantiate(ModAssets.HotbarPrefab).transform.GetChild(0).transform;
                _hotbarCanvas = _hotbarRoot.transform.root.GetComponent<Canvas>();
                _hotkeyController = LocalPlayer._instance.gameObject.GetComponentInChildren<ItemHotkeyController>();

                if (_hotkeyController != null)
                    RLog.Msg("[Hotbar] Controller found.");
                else
                    RLog.Warning("[Hotbar] Controller not found.");

                _spriteCache.Clear();
                _frameCounter = 0;
                _initialized = true;
                RLog.Msg("[Hotbar] Hotbar active.");
            }
            catch (Exception ex)
            {
                RLog.Error($"[Hotbar] OnGameStart: {ex}");
            }
        }

        public static void OnUpdate()
        {
            if (!_initialized || _hotbarCanvas == null) return;

            try
            {
                // Hide during pause — same as original
                if (PauseMenu.IsActive)
                {
                    _hotbarCanvas.enabled = false;
                    return;
                }

                _hotbarCanvas.enabled = true;

                if (_hotkeyController == null) return;

                // Frame throttle
                _frameCounter++;
                if (_frameCounter < UPDATE_INTERVAL) return;
                _frameCounter = 0;

                // Read save data — same as original
                HotkeySaveData saveData;
                try
                {
                    saveData = _hotkeyController._hotkeySaveData;
                    if (saveData == null) return;
                }
                catch { return; }

                if (saveData.RightHandItemIds == null || saveData.LeftHandItemIds == null) return;

                // Update each slot — same logic as original SonsHotbar.OnUpdate
                for (int i = 0; i < saveData.RightHandItemIds.Count; i++)
                {
                    try
                    {
                        Transform child = _hotbarRoot.GetChild(GetSlotId(i));
                        int leftId = saveData.LeftHandItemIds[i];
                        int rightId = saveData.RightHandItemIds[i];
                        PlayerInventory inventory = LocalPlayer._instance._inventory;
                        Sprite sprite = ModAssets.missingIcon;

                        if (rightId != -1 || leftId != -1)
                        {
                            // Right hand only
                            if (rightId != -1 && leftId == -1)
                            {
                                if (!inventory._itemInstanceManager.HaveAny(rightId))
                                {
                                    sprite = ModAssets.missingIcon;
                                }
                                else
                                {
                                    sprite = GetCachedSprite(rightId);
                                }
                            }
                            // Left hand only (or both)
                            else if (rightId == -1 && leftId != -1)
                            {
                                if (!inventory._itemInstanceManager.HaveAny(leftId))
                                {
                                    sprite = ModAssets.missingIcon;
                                }
                                else
                                {
                                    sprite = GetCachedSprite(leftId);
                                }
                            }
                        }

                        child.GetChild(1).GetComponent<Image>().sprite = sprite;
                    }
                    catch { }
                }
            }
            catch { }
        }

        private static Sprite GetCachedSprite(int itemId)
        {
            if (_spriteCache.TryGetValue(itemId, out Sprite cached) && cached != null)
                return cached;

            try
            {
                var itemData = ItemDatabaseManager.ItemById(itemId);
                if (itemData == null) return ModAssets.missingIcon;

                var uiData = itemData.UiData;
                if (uiData == null) return ModAssets.missingIcon;

                var icon = uiData._icon;
                if (icon == null) return ModAssets.missingIcon;

                // DummyDll compile-time types: Texture and Texture2D don't inherit Il2CppObjectBase.
                // At RUNTIME they DO. Use reflection to bypass the generic constraint on Cast<T>().
                var il2cppBase = (Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase)(object)icon;
                var castMethod = il2cppBase.GetType().GetMethod("Cast");
                var genericCast = castMethod.MakeGenericMethod(typeof(Texture2D));
                var texture = (Texture2D)genericCast.Invoke(il2cppBase, null);

                if (texture == null) return ModAssets.missingIcon;

                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                _spriteCache[itemId] = sprite;
                return sprite;
            }
            catch
            {
                return ModAssets.missingIcon;
            }
        }

        private static int GetSlotId(int index)
        {
            return index == 0 ? 9 : index - 1;
        }
    }
}
#endif

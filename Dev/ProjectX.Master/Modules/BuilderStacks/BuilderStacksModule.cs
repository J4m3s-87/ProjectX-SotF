#if !SERVER
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using HarmonyLib;
using RedLoader;
using SonsSdk;
using Sons.Items.Core;
using SUI;
using TheForest;
using TheForest.Utils;
using UnityEngine;

namespace ProjectX.Master.Modules.BuilderStacks
{
    /// <summary>
    /// Builder Stacks — carry extra logs, planks, and stones beyond vanilla limits.
    ///
    /// Based on ItemCarryAmount (LogCarryAmount) by SmokyAce.
    ///
    /// When player picks up a 2nd item → absorb it into our buffer (_heldCount = 1).
    /// When player places/uses last item → give one back from buffer after a delay.
    /// When at capacity → don't absorb, game keeps amount=2 naturally (no item loss).
    ///
    /// Per-material buffers: logs, planks, and stones each have their own buffer
    /// and configurable max capacity.
    /// </summary>
    public static class BuilderStacksModule
    {
        // ── Config (set by Config.cs via OnValueChanged) ──
        public static int MaxCapacity = 10;        // Global fallback
        public static int MaxLogCapacity = 2;      // Vanilla default
        public static int MaxPlankCapacity = 4;    // Vanilla default
        public static int MaxStoneCapacity = 4;    // Vanilla default
        public static float GiveDelay = 0.8f;
        public static bool EnableMaxLimit = true;
        public static bool Enabled = true;

        // ── Per-material buffer state ──
        private static int _logBuffer;
        private static int _plankBuffer;
        private static int _stoneBuffer;
        private static int _heldItemId;
        private static bool _shouldGiveBack;
        private static float _giveTimer;
        private static bool _forceGive;

        // ── Memory access for _heldCount ──
        private static PropertyInfo _pointerProp;
        private static bool _pointerResolved;
        private const int HELD_COUNT_OFFSET = 0x5C;

        // ── Hack state (transition guards) ──
        private static bool _logHackOn;
        private static bool _stoneHackOn;
        private static bool _hackStateInitialized;

        // ── UI (Observable binding like AmmoUI) ──
        private static SUiElement<SContainerOptions> _panel;
        private static SUiElement<SLabelOptions> _carryAmount;
        private static SImageOptions _icon;
        private static bool _uiLoaded;
        private static readonly Observable<bool> _showPanel = new(false);
        private static readonly Observable<Texture> _matIcon = new(null);
        private static readonly Observable<string> _matText = new("");

        public static void Init()
        {
            try
            {
                // Panel — dark semi-transparent background, horizontal layout
                _panel = SUI.SUI.RegisterNewPanel("BuilderStacks", false, default(KeyCode?))
                    .Pivot(0f, 0f)
                    .Anchor(AnchorType.BottomLeft)
                    .Background(SUI.SUI.SpriteBackground400ppu, new Color?(new Color(0f, 0f, 0f, 0.6f)), UnityEngine.UI.Image.Type.Sliced)
                    .Size(200f, 60f)
                    .Position(10f, 15f)
                    .Horizontal(0f, "EE")
                    .BindVisibility(_showPanel);

                // Icon — exact AmmoUI pattern: SImage.Bind + Add directly to panel
                _icon = SUI.SUI.SImage.Bind(_matIcon).Dock(EDockType.Fill);
                try { _icon.ImageObject.color = new Color(1f, 1f, 1f, 0.9f); } catch { }

                // Text label — bound to Observable<string>
                _carryAmount = SUI.SUI.SLabel.Bind(_matText)
                    .FontColor(new Color(1f, 1f, 1f, 0.85f))
                    .FontSize(26)
                    .Dock(EDockType.Fill)
                    .Alignment(TMPro.TextAlignmentOptions.Left);

                _panel.Add(_icon);
                _panel.Add(_carryAmount);

                _showPanel.Set(false);
                _uiLoaded = true;

                RLog.Msg("[BuilderStacks] HUD initialized (icon + text, dark panel)");
            }
            catch (Exception ex)
            {
                RLog.Error($"[BuilderStacks] Init failed: {ex.Message}");
            }
        }

        public static void OnUpdate()
        {
            if (!LocalPlayer.IsInWorld || !_uiLoaded) return;

            try
            {
                var heldController = LocalPlayer.Inventory?.HeldOnlyItemController;
                if (heldController == null) return;

                int amount = heldController.Amount;

                // Resolve IL2CPP Pointer property once
                if (!_pointerResolved)
                {
                    _pointerResolved = true;
                    try
                    {
                        _pointerProp = heldController.GetType().GetProperty("Pointer",
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                        RLog.Msg($"[BuilderStacks] Pointer property {(_pointerProp != null ? "resolved" : "NOT found")} for _heldCount at offset 0x{HELD_COUNT_OFFSET:X}");
                    }
                    catch (Exception ex)
                    {
                        RLog.Warning($"[BuilderStacks] Pointer resolve failed: {ex.Message}");
                    }
                }

                // ═══════════════════════════════════════════════
                // MODULE DISABLED = vanilla carry (do nothing)
                // ═══════════════════════════════════════════════
                if (!Enabled)
                {
                    _logBuffer = 0;
                    _plankBuffer = 0;
                    _stoneBuffer = 0;
                    CloseUI();
                    return;
                }

                // ═══════════════════════════════════════════════
                // MODULE ENABLED = buffer-based carry limit
                // ═══════════════════════════════════════════════
                SetLogHack(false);
                SetStoneHack(false);

                // ── Amount == 1: record what we're holding ──
                if (amount == 1)
                {
                    try { _heldItemId = heldController.HeldItem._itemID; } catch { }

                    // Sync _heldCount to 1 when buffer is empty — clears stale values
                    // from previous absorption cycle (prevents ghost visual stacking)
                    if (IsBuildingMaterial(_heldItemId) && GetBuffer(_heldItemId) == 0)
                    {
                        SetHeldCount(heldController, 1);
                    }
                }

                // ── Amount >= nativeLimit: absorb excess into buffer ──
                // Only triggers at the native hold limit (2 for logs, 4 for planks/stones).
                // This preserves vanilla visual stacking (1→2→3→4 on shoulder).
                // Buffer is sized so that buffer + nativeLimit = maxCap.
                if (IsBuildingMaterial(_heldItemId))
                {
                    int nativeLimit = GetNativeHoldLimit(_heldItemId);

                    if (amount >= nativeLimit)
                    {
                        int currentBuffer = GetBuffer(_heldItemId);
                        int maxCap = GetMaxCapacity(_heldItemId);
                        int excess = amount - 1;

                        int spaceLeft = EnableMaxLimit
                            ? System.Math.Max(0, maxCap - nativeLimit - currentBuffer)
                            : excess;
                        int toAbsorb = System.Math.Min(excess, spaceLeft);

                        if (toAbsorb > 0)
                        {
                            int newHeld = amount - toAbsorb;
                            if (SetHeldCount(heldController, newHeld))
                            {
                                AddToBuffer(_heldItemId, toAbsorb);
                                RLog.Msg($"[BuilderStacks] Absorbed {GetMaterialName(_heldItemId)} x{toAbsorb} → buffer={GetBuffer(_heldItemId)}/{maxCap}, held={newHeld}");
                            }
                        }
                        // At capacity: do nothing. Game enforces native hold limit.
                    }
                }

                // ── Amount < 1: player placed/used item → give one back from buffer ──
                // Give back ONE at a time. Must wait for full delay between each give.
                if (amount < 1 && GetBuffer(_heldItemId) > 0 && IsBuildingMaterial(_heldItemId))
                {
                    _giveTimer += Time.deltaTime;
                    float minDelay = System.Math.Max(GiveDelay, 0.3f); // enforce minimum 0.3s
                    if (_giveTimer >= minDelay)
                    {
                        GiveFromBuffer();
                        _giveTimer = 0f;
                    }
                }
                else
                {
                    // Reset timer when player is holding something (amount >= 1)
                    // or no buffer to give back
                    _giveTimer = 0f;
                }

                // ── UI ──
                int total = GetBuffer(_heldItemId) + amount;
                if (total >= 1 && IsBuildingMaterial(_heldItemId))
                {
                    string text = total.ToString();
                    if (EnableMaxLimit)
                    {
                        int maxCap = GetMaxCapacity(_heldItemId);
                        text = $"{total}/{maxCap}";
                    }

                    _matText.Set(text);

                    // Set icon from held item data (same as AmmoUI)
                    try
                    {
                        var icon = heldController.HeldItem?.Data?.UiData?._icon;
                        if (icon != null)
                            _matIcon.Set(icon);
                    }
                    catch { }

                    if (!_showPanel.Value)
                    {
                        _showPanel.Set(true);
                    }
                }
                else
                {
                    if (_showPanel.Value)
                    {
                        _showPanel.Set(false);
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderStacks] OnUpdate error: {ex.Message}");
            }
        }

        // ── Per-material buffer access ──

        private static int GetBuffer(int itemId)
        {
            switch (GetMaterialType(itemId))
            {
                case MaterialType.Log:   return _logBuffer;
                case MaterialType.Plank: return _plankBuffer;
                case MaterialType.Stone: return _stoneBuffer;
                default: return 0;
            }
        }

        private static void AddToBuffer(int itemId, int count)
        {
            switch (GetMaterialType(itemId))
            {
                case MaterialType.Log:   _logBuffer += count; break;
                case MaterialType.Plank: _plankBuffer += count; break;
                case MaterialType.Stone: _stoneBuffer += count; break;
            }
        }

        private static void DecrementBuffer(int itemId)
        {
            switch (GetMaterialType(itemId))
            {
                case MaterialType.Log:   if (_logBuffer > 0) _logBuffer--; break;
                case MaterialType.Plank: if (_plankBuffer > 0) _plankBuffer--; break;
                case MaterialType.Stone: if (_stoneBuffer > 0) _stoneBuffer--; break;
            }
        }

        private static int GetMaxCapacity(int itemId)
        {
            switch (GetMaterialType(itemId))
            {
                case MaterialType.Log:   return MaxLogCapacity;
                case MaterialType.Plank: return MaxPlankCapacity;
                case MaterialType.Stone: return MaxStoneCapacity;
                default: return MaxCapacity;
            }
        }

        /// <summary>Vanilla hold limit per material (how many the game natively allows in hand).</summary>
        private static int GetNativeHoldLimit(int itemId)
        {
            switch (GetMaterialType(itemId))
            {
                case MaterialType.Log:   return 2;
                case MaterialType.Plank: return 4;
                case MaterialType.Stone: return 4;
                default:                 return 2;
            }
        }

        // ── Give-back logic (matches original GiveLog + AreEmpty pattern) ──

        private static void GiveFromBuffer()
        {
            if (GetBuffer(_heldItemId) <= 0 || _heldItemId == 0) return;
            DecrementBuffer(_heldItemId);
            try
            {
                LocalPlayer.Inventory.TryEquip(_heldItemId, false, false, false);
                RLog.Msg($"[BuilderStacks] Gave back {GetMaterialName(_heldItemId)} from buffer → buffer={GetBuffer(_heldItemId)}");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderStacks] GiveFromBuffer failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Safety check before giving items back — matches original AreEmpty() pattern.
        /// Only give back when hands are free and player is in a safe state.
        /// </summary>
        private static bool AreHandsEmpty()
        {
            try
            {
                return LocalPlayer.Inventory.IsRightHandEmpty()
                    && LocalPlayer.Inventory.IsLeftHandEmpty()
                    && LocalPlayer.Inventory.HasRoomFor(_heldItemId, 2)
                    && !LocalPlayer.Inventory.IsInventoryToggleBlocked()
                    && !LocalPlayer.IsUnderwater;
            }
            catch { return false; }
        }

        /// <summary>
        /// Set _heldCount via direct IL2CPP pointer + offset (proven pattern).
        /// Returns true if write succeeded, false if pointer unavailable.
        /// </summary>
        private static bool SetHeldCount(object heldController, int value)
        {
            try
            {
                if (_pointerProp == null) return false;
                IntPtr ptr = (IntPtr)_pointerProp.GetValue(heldController);
                if (ptr == IntPtr.Zero) return false;
                Marshal.WriteInt32(ptr + HELD_COUNT_OFFSET, value);
                return true;
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderStacks] SetHeldCount failed: {ex.Message}");
                return false;
            }
        }

        // ── Hack toggles (transition-guarded, only fire on state change) ──

        private static void SetLogHack(bool on)
        {
            if (_hackStateInitialized && on == _logHackOn) return;
            _logHackOn = on;
            _hackStateInitialized = true;
            try { DebugConsole.Instance._loghack(on ? "on" : "off"); }
            catch { }
        }

        private static void SetStoneHack(bool on)
        {
            if (_hackStateInitialized && on == _stoneHackOn) return;
            _stoneHackOn = on;
            try { DebugConsole.Instance._stonehack(on ? "on" : "off"); }
            catch { }
        }

        // ── UI (matches original LogCarryAmountUi pattern) ──

        private static void OpenUI()
        {
            _showPanel.Set(true);
        }

        private static void CloseUI()
        {
            _showPanel.Set(false);
        }

        // ── Helpers ──

        private enum MaterialType { None, Log, Plank, Stone }

        private static MaterialType GetMaterialType(int itemId)
        {
            switch (itemId)
            {
                case 78:  case 406: case 408: case 409:  return MaterialType.Log;
                case 395: case 576: case 577: case 578:  return MaterialType.Plank;
                case 640:                                 return MaterialType.Stone;
                default:                                  return MaterialType.None;
            }
        }

        private static bool IsBuildingMaterial(int itemId)
        {
            return GetMaterialType(itemId) != MaterialType.None;
        }

        private static string GetMaterialName(int itemId)
        {
            switch (GetMaterialType(itemId))
            {
                case MaterialType.Log:   return "Logs";
                case MaterialType.Plank: return "Planks";
                case MaterialType.Stone: return "Stones";
                default:                 return "Items";
            }
        }
    }
}
#endif

#if !SERVER
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using HarmonyLib;
using RedLoader;
using SonsSdk;
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

        // ── UI ──
        private static SUiElement<SContainerOptions> _panel;
        private static SUiElement<SLabelOptions> _carryAmount;
        private static bool _uiLoaded;
        private static bool _uiOpen;

        public static void Init()
        {
            try
            {
                // Match original LogCarryAmountUi.Create() pattern exactly
                _panel = SUI.SUI.RegisterNewPanel("BuilderStacks", false, default(KeyCode?))
                    .Pivot(new float?(0f), default(float?))
                    .Anchor(AnchorType.BottomLeft)
                    .Size(new float?(250f), new float?(60f))
                    .Position(new float?(-450f), new float?(105f))
                    .Background(SUI.SUI.SpriteBackground400ppu, new Color?(new Color(0f, 0f, 0f, 0.8f)), UnityEngine.UI.Image.Type.Sliced);

                _carryAmount = SUI.SUI.SLabel.RichText("1 Log")
                    .FontColor(CommonExtensions.WithAlpha(Color.white, 0.3f))
                    .FontSize(18)
                    .Dock(EDockType.Fill)
                    .Alignment(TMPro.TextAlignmentOptions.Center);

                _carryAmount.SetParent(_panel);
                CloseUI();
                _uiLoaded = true;

                RLog.Msg("[BuilderStacks] HUD initialized (original SUI pattern)");
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
                }

                // ── Amount == 2: player picked up a 2nd item ──
                // Absorb into buffer ONLY if under capacity. If at capacity, do nothing.
                if (amount == 2 && IsBuildingMaterial(_heldItemId))
                {
                    int currentBuffer = GetBuffer(_heldItemId);
                    int maxCap = GetMaxCapacity(_heldItemId);

                    if (!EnableMaxLimit || currentBuffer + amount < maxCap)
                    {
                        // Under capacity → absorb: set _heldCount = 1, increment buffer
                        // ONLY add to buffer if the write succeeds
                        if (SetHeldCount(heldController, 1))
                        {
                            AddToBuffer(_heldItemId, 1);
                            RLog.Msg($"[BuilderStacks] Absorbed {GetMaterialName(_heldItemId)} → buffer={GetBuffer(_heldItemId)}/{maxCap}");
                        }
                    }
                    // else: AT CAPACITY — do nothing. Game keeps amount=2 naturally.
                    // Player cannot pick up more. No item loss.
                }

                // ── Amount == 0: player placed/used last item ──
                // Give one back from buffer after a delay
                if (amount < 1 && GetBuffer(_heldItemId) > 0 && IsBuildingMaterial(_heldItemId))
                {
                    _shouldGiveBack = true;
                }

                // ── Delayed give-back from buffer ──
                if (_shouldGiveBack)
                {
                    _giveTimer += Time.deltaTime;
                    if (_giveTimer >= GiveDelay || _forceGive)
                    {
                        GiveFromBuffer();
                        _giveTimer = 0f;
                        _shouldGiveBack = false;
                        _forceGive = false;
                    }
                    else if (AreHandsEmpty())
                    {
                        // Hands are ready — force give on next frame
                        _forceGive = true;
                    }
                }

                // ── UI ──
                int total = GetBuffer(_heldItemId) + amount;
                if (total >= 1 && IsBuildingMaterial(_heldItemId))
                {
                    string text = total.ToString();
                    string mat = GetMaterialName(_heldItemId);
                    if (EnableMaxLimit)
                    {
                        int maxCap = GetMaxCapacity(_heldItemId);
                        text = $"{total}/{maxCap}";
                    }

                    if (_carryAmount != null)
                    {
                        _carryAmount.RichText($"{text} {mat}");
                    }

                    if (!_uiOpen)
                    {
                        OpenUI();
                    }
                }
                else
                {
                    if (_uiOpen)
                    {
                        CloseUI();
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
            SUI.SUI.TogglePanel("BuilderStacks", true);
            _uiOpen = true;
        }

        private static void CloseUI()
        {
            SUI.SUI.TogglePanel("BuilderStacks", false);
            _uiOpen = false;
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

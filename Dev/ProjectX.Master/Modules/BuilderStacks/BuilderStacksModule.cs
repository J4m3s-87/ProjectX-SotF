#if !SERVER
using System;
using System.Reflection;
using System.Runtime.InteropServices;
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
    /// DISABLED = infinite carry (_loghack on, _stonehack on).
    /// ENABLED  = configurable max carry limit using a virtual buffer.
    ///
    /// Buffer approach (same as original ItemCarryAmount mod):
    ///   When player picks up a 2nd item → absorb it into our buffer (_heldCount = 1).
    ///   When player places last item → give one back from buffer (TryEquip).
    ///   _heldCount is set via direct IL2CPP memory access at offset 0x5C.
    ///
    /// Per-material buffers: logs, planks, and stones each have their own buffer
    /// and configurable max capacity.
    /// </summary>
    public static class BuilderStacksModule
    {
        // ── Config (set by Config.cs via OnValueChanged) ──
        public static int MaxCapacity = 10;        // Global fallback
        public static int MaxLogCapacity = 10;
        public static int MaxPlankCapacity = 10;
        public static int MaxStoneCapacity = 10;
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

        // ── Memory access for _heldCount ──
        private static PropertyInfo _pointerProp;
        private static bool _pointerResolved;
        private const int HELD_COUNT_OFFSET = 0x5C;

        // ── Hack state (transition guards) ──
        private static bool _logHackOn;
        private static bool _stoneHackOn;
        private static bool _hackStateInitialized;

        // ── UI ──
        private static SContainerOptions _panel;
        private static SLabelOptions _label;
        private static bool _uiOpen;

        public static void Init()
        {
            try
            {
                _panel = SUI.SUI.RegisterNewPanel("BuilderStacks", false, default(KeyCode?))
                    .Pivot(new float?(0f), default(float?))
                    .Anchor(AnchorType.BottomLeft)
                    .Size(new float?(250f), new float?(60f))
                    .Position(new float?(-450f), new float?(105f))
                    .Background(new Color(0f, 0f, 0f, 0.8f), EBackground.None, default(UnityEngine.UI.Image.Type?));

                _label = SUI.SUI.SLabel.RichText("1 Log")
                    .FontColor(CommonExtensions.WithAlpha(Color.white, 0.3f))
                    .FontSize(18)
                    .Dock(EDockType.Fill)
                    .Alignment(TMPro.TextAlignmentOptions.Center);

                _label.SetParent(_panel);
                SUI.SUI.TogglePanel("BuilderStacks", false);

                RLog.Msg("[BuilderStacks] Initialized");
            }
            catch (Exception ex)
            {
                RLog.Error($"[BuilderStacks] Init failed: {ex.Message}");
            }
        }

        public static void OnUpdate()
        {
            if (!LocalPlayer.IsInWorld) return;

            try
            {
                var heldController = LocalPlayer.Inventory?.HeldOnlyItemController;
                if (heldController == null) return;

                // Resolve Pointer property once for direct memory access
                if (!_pointerResolved)
                {
                    ResolvePointer(heldController);
                    _pointerResolved = true;
                }

                int amount = heldController.Amount;

                // ═══════════════════════════════════════════════
                // DISABLED = infinite carry
                // ═══════════════════════════════════════════════
                if (!Enabled)
                {
                    SetLogHack(true);
                    SetStoneHack(true);
                    _logBuffer = 0;
                    _plankBuffer = 0;
                    _stoneBuffer = 0;
                    HideUI();
                    return;
                }

                // ═══════════════════════════════════════════════
                // ENABLED = buffer-based carry limit
                // No _loghack — vanilla placement consumes items
                // ═══════════════════════════════════════════════
                SetLogHack(false);
                SetStoneHack(false);

                // Track what the player is holding
                if (amount >= 1)
                {
                    try { _heldItemId = heldController.HeldItem._itemID; } catch { }
                }

                // ── Amount == 2: player picked up a 2nd item ──
                // Absorb it into the per-material buffer by resetting _heldCount to 1
                if (amount == 2 && IsBuildingMaterial(_heldItemId))
                {
                    int currentBuffer = GetBuffer(_heldItemId);
                    int maxCap = GetMaxCapacity(_heldItemId);
                    
                    if (!EnableMaxLimit || currentBuffer + amount < maxCap)
                    {
                        // Absorb: write _heldCount = 1, increment buffer
                        WriteHeldCount(heldController, 1);
                        AddToBuffer(_heldItemId, 1);
                        RLog.Msg($"[BuilderStacks] Absorbed {GetMaterialName(_heldItemId)} → buffer={GetBuffer(_heldItemId)}/{maxCap}");
                    }
                    // else: at capacity, don't absorb → game enforces vanilla limit (stays at 2)
                }

                // ── Amount == 0: player placed/dropped last item ──
                // Give one back from buffer after a delay
                if (amount < 1 && GetBuffer(_heldItemId) > 0 && IsBuildingMaterial(_heldItemId))
                {
                    _shouldGiveBack = true;
                }

                // ── Delayed give-back from buffer ──
                if (_shouldGiveBack)
                {
                    _giveTimer += Time.deltaTime;
                    if (_giveTimer >= GiveDelay)
                    {
                        GiveFromBuffer();
                        _giveTimer = 0f;
                        _shouldGiveBack = false;
                    }
                }

                // ── UI ──
                int total = GetBuffer(_heldItemId) + amount;
                if (total >= 1 && IsBuildingMaterial(_heldItemId))
                {
                    int maxForDisplay = GetMaxCapacity(_heldItemId);
                    string limitText = EnableMaxLimit ? $"/{maxForDisplay}" : "";
                    if (_label != null)
                        _label.RichText($"{total}{limitText} {GetMaterialName(_heldItemId)}");
                    if (!_uiOpen)
                    {
                        SUI.SUI.TogglePanel("BuilderStacks", true);
                        _uiOpen = true;
                    }
                }
                else
                {
                    HideUI();
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

        // ── Direct memory access for _heldCount (offset 0x5C) ──

        private static void ResolvePointer(object heldController)
        {
            try
            {
                // IL2CPP runtime objects have a Pointer property from Il2CppObjectBase
                _pointerProp = heldController.GetType().GetProperty("Pointer",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (_pointerProp != null)
                    RLog.Msg($"[BuilderStacks] Pointer resolved for direct _heldCount access (offset 0x{HELD_COUNT_OFFSET:X})");
                else
                    RLog.Warning("[BuilderStacks] Pointer NOT found — buffer approach unavailable");
            }
            catch (Exception ex)
            {
                RLog.Error($"[BuilderStacks] ResolvePointer failed: {ex.Message}");
            }
        }

        private static void WriteHeldCount(object heldController, int value)
        {
            try
            {
                if (_pointerProp == null) return;
                IntPtr ptr = (IntPtr)_pointerProp.GetValue(heldController);
                if (ptr == IntPtr.Zero) return;
                Marshal.WriteInt32(ptr + HELD_COUNT_OFFSET, value);
            }
            catch (Exception ex)
            {
                RLog.Warning($"[BuilderStacks] WriteHeldCount failed: {ex.Message}");
            }
        }

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

        private static void HideUI()
        {
            if (_uiOpen)
            {
                SUI.SUI.TogglePanel("BuilderStacks", false);
                _uiOpen = false;
            }
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

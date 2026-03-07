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

        // ── UI (Observable binding — same pattern as AmmoUI) ──
        private static SContainerOptions _panel;
        private static SLabelOptions _label;
        private static readonly Observable<bool> _showPanel = new Observable<bool>(false);
        private static readonly Observable<string> _labelText = new Observable<string>("");

        public static void Init()
        {
            try
            {
                // Panel at bottom-left, positioned above the AmmoUI area
                _panel = SUI.SUI.RegisterNewPanel("BuilderStacks", false, default(KeyCode?))
                    .Pivot(0f, 0f)
                    .Anchor(AnchorType.BottomLeft)
                    .Size(250f, 50f)
                    .Position(10f, 130f)
                    .Background(new Color(0f, 0f, 0f, 0.7f), EBackground.None, default(UnityEngine.UI.Image.Type?))
                    .BindVisibility(_showPanel);

                _label = SUI.SUI.SLabel.Bind(_labelText)
                    .FontColor(new Color(1f, 1f, 1f, 0.9f))
                    .FontSize(20)
                    .Dock(EDockType.Fill)
                    .Alignment(TMPro.TextAlignmentOptions.Center);

                _panel.Add(_label);
                _showPanel.Set(false);

                RLog.Msg("[BuilderStacks] HUD initialized (Observable binding)");
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
                // MODULE DISABLED = infinite carry
                // ═══════════════════════════════════════════════
                if (!Enabled)
                {
                    SetLogHack(true);
                    SetStoneHack(true);
                    _logBuffer = 0;
                    _plankBuffer = 0;
                    _stoneBuffer = 0;
                    _showPanel.Set(false);
                    return;
                }

                // ═══════════════════════════════════════════════
                // MODULE ENABLED = buffer-based carry limit
                // No _loghack — vanilla placement consumes items
                // ═══════════════════════════════════════════════
                SetLogHack(false);
                SetStoneHack(false);

                // Always update held item ID from what's actually in hand
                if (amount >= 1)
                {
                    try { _heldItemId = heldController.HeldItem._itemID; } catch { }
                }

                // ── Absorb excess into buffer ──
                // The game always holds 1 in hand. When amount >= 2, the player picked up more.
                // We absorb all excess (amount - 1) into the per-material buffer.
                if (amount >= 2 && IsBuildingMaterial(_heldItemId))
                {
                    int excess = amount - 1;  // Items beyond the 1 we keep in hand
                    int currentBuffer = GetBuffer(_heldItemId);
                    int maxCap = GetMaxCapacity(_heldItemId);

                    if (EnableMaxLimit)
                    {
                        // Only absorb up to capacity: total = buffer + 1 (in hand) + excess
                        int spaceLeft = maxCap - currentBuffer - 1; // -1 for the one in hand
                        if (spaceLeft < 0) spaceLeft = 0;
                        int toAbsorb = Math.Min(excess, spaceLeft);
                        
                        if (toAbsorb > 0)
                        {
                            WriteHeldCount(heldController, 1);
                            AddToBuffer(_heldItemId, toAbsorb);
                            RLog.Msg($"[BuilderStacks] Absorbed {toAbsorb} {GetMaterialName(_heldItemId)} → buffer={GetBuffer(_heldItemId)}/{maxCap}");
                        }
                        else
                        {
                            // At capacity — absorb but drop excess (write held to 1, buffer stays)
                            WriteHeldCount(heldController, 1);
                            RLog.Msg($"[BuilderStacks] At capacity {GetMaterialName(_heldItemId)} ({currentBuffer + 1}/{maxCap}) — excess dropped");
                        }
                    }
                    else
                    {
                        // Unlimited mode: absorb everything
                        WriteHeldCount(heldController, 1);
                        AddToBuffer(_heldItemId, excess);
                    }
                }

                // Clamp buffers to max (safety net)
                if (EnableMaxLimit)
                {
                    ClampBuffers();
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
                    _labelText.Set($"{total}{limitText} {GetMaterialName(_heldItemId)}");
                    _showPanel.Set(true);
                }
                else
                {
                    _showPanel.Set(false);
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

        private static void ClampBuffers()
        {
            // Safety net: ensure no buffer exceeds max - 1 (max minus the 1 in hand)
            int logMax = Math.Max(MaxLogCapacity - 1, 0);
            int plankMax = Math.Max(MaxPlankCapacity - 1, 0);
            int stoneMax = Math.Max(MaxStoneCapacity - 1, 0);
            if (_logBuffer > logMax) _logBuffer = logMax;
            if (_plankBuffer > plankMax) _plankBuffer = plankMax;
            if (_stoneBuffer > stoneMax) _stoneBuffer = stoneMax;
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

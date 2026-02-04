using System;
using System.Reflection;
using RedLoader;
using Sons.Crafting.Structures;
using Sons.Inventory;
using SonsSdk;
using TheForest.Utils;
using UnityEngine;
using HarmonyLib;

namespace ProjectX.Master.Modules.PrefabRepair
{
    /// <summary>
    /// PrefabRepair Module - Polling-Based Implementation
    /// Replaces Harmony patching with raycast detection to avoid IL2CPP crashes.
    /// </summary>
    public static class PrefabRepairModule
    {
        private static bool _initialized = false;
        private static float _lastRepairTime = 0f;
        private const float REPAIR_COOLDOWN = 0.3f;
        private const int REPAIR_TOOL_ID = 422; // Repair tool item ID
        private const float REPAIR_RANGE = 5f;

        // Reflection cache
        private static FieldInfo _currentHpField;
        private static PropertyInfo _structureHpProp;
        private static FieldInfo _structureHpField;
        private static FieldInfo _itemIdField;
        private static bool _reflectionCached = false;

        public static void Init()
        {
            if (_initialized) return;

            try
            {
                // Subscribe to update loop instead of using Harmony patches
                SdkEvents.OnInWorldUpdate.Subscribe(OnUpdate);
                
                RLog.Msg("[PrefabRepair] Initialized (Polling Mode - No Harmony)");
                _initialized = true;
            }
            catch (Exception ex)
            {
                RLog.Error($"[PrefabRepair] Failed to initialize: {ex.Message}");
            }
        }

        private static void CacheReflection()
        {
            if (_reflectionCached) return;

            try
            {
                var destructionType = typeof(ScrewStructureDestruction);
                _currentHpField = AccessTools.Field(destructionType, "_currentHp");
                _structureHpProp = AccessTools.Property(destructionType, "StructureHp");
                _structureHpField = AccessTools.Field(destructionType, "_structureHp");
                _itemIdField = AccessTools.Field(typeof(ItemInstance), "_itemID");

                _reflectionCached = true;
                RLog.Msg("[PrefabRepair] Reflection cache initialized");
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PrefabRepair] Reflection cache failed: {ex.Message}");
            }
        }

        private static void OnUpdate()
        {
            if (!Config.PrefabRepairEnabled.Value) return;
            if (!LocalPlayer.IsInWorld) return;
            if (LocalPlayer.IsInInventory) return;

            // Ensure reflection is cached
            if (!_reflectionCached) CacheReflection();

            // Check for left-click with repair tool
            if (Input.GetMouseButton(0))
            {
                TryRepairStructure();
            }
        }

        private static void TryRepairStructure()
        {
            // Throttle repairs
            if (Time.time - _lastRepairTime < REPAIR_COOLDOWN) return;

            try
            {
                // Check if holding repair tool (ID 422)
                var rightHand = LocalPlayer.Inventory?.RightHandItem;
                if (rightHand == null) return;

                int itemId = GetItemId(rightHand);
                if (itemId != REPAIR_TOOL_ID) return;

                // Raycast from camera center
                var mainCamera = Camera.main;
                if (mainCamera == null) return;

                Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                
                if (Physics.Raycast(ray, out RaycastHit hit, REPAIR_RANGE))
                {
                    // Try to find ScrewStructureDestruction on hit object or parents
                    var destruction = hit.collider.GetComponent<ScrewStructureDestruction>();
                    if (destruction == null)
                    {
                        destruction = hit.collider.GetComponentInParent<ScrewStructureDestruction>();
                    }

                    if (destruction != null)
                    {
                        RepairStructure(destruction);
                    }
                }
            }
            catch (Exception ex)
            {
                // Silent fail to avoid log spam
                // RLog.Warning($"[PrefabRepair] Error in TryRepairStructure: {ex.Message}");
            }
        }

        private static int GetItemId(ItemInstance item)
        {
            if (_itemIdField == null || item == null) return -1;

            try
            {
                return (int)_itemIdField.GetValue(item);
            }
            catch
            {
                return -1;
            }
        }

        private static void RepairStructure(ScrewStructureDestruction destruction)
        {
            if (_currentHpField == null || _structureHpProp == null) return;

            try
            {
                // Get current and max HP via reflection
                int currentHp = (int)_currentHpField.GetValue(destruction);
                int maxHp = (int)_structureHpProp.GetValue(destruction);

                if (currentHp < maxHp)
                {
                    // Increment HP
                    currentHp = Math.Min(currentHp + 1, maxHp);
                    _currentHpField.SetValue(destruction, currentHp);
                    _lastRepairTime = Time.time;

                    // Optionally log progress
                    if (currentHp == maxHp)
                    {
                        RLog.Msg($"[PrefabRepair] Structure fully repaired!");
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PrefabRepair] Error repairing structure: {ex.Message}");
            }
        }

        /// <summary>
        /// Check and upgrade gold-plated structures (called periodically or on demand)
        /// </summary>
        public static void CheckGoldPlating(ScrewStructureDestruction destruction)
        {
            if (destruction == null || _structureHpField == null || _currentHpField == null) return;

            try
            {
                // Check if structure is gold plated
                var screwStructure = destruction.GetComponent<ScrewStructure>();
                var electricStructure = destruction.GetComponent<ElectricDeviceScrewStructure>();

                bool isGoldPlated = false;
                if (screwStructure != null && screwStructure.IsGoldPlated) isGoldPlated = true;
                if (electricStructure != null && electricStructure.IsGoldPlated) isGoldPlated = true;

                if (isGoldPlated)
                {
                    // Boost HP for gold-plated structures
                    _structureHpField.SetValue(destruction, 12);
                    
                    int currentHp = (int)_currentHpField.GetValue(destruction);
                    if (currentHp < 12)
                    {
                        _currentHpField.SetValue(destruction, 12);
                    }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PrefabRepair] Error checking gold plating: {ex.Message}");
            }
        }
    }
}

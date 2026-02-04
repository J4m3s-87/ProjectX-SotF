using System;
using RedLoader;
using Sons.Crafting.Structures;
using Sons.Inventory;
using TheForest.Utils;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace ProjectX.Master.Modules.PrefabRepair
{
    [RegisterTypeInIl2Cpp]
    public class ScrewStructureRepairHandler : MonoBehaviour
    {
        public ScrewStructureDestruction _screwDestruction;
        public ScrewStructure _screwStructure;
        public ElectricDeviceScrewStructure _electricStructure;
        private bool _isPlated;
        private float _lastImpact;

        // Reflection Cache
        private FieldInfo _currentHpField;
        private FieldInfo _structureHpField; // Field
        private PropertyInfo _structureHpProp; // Property
        private FieldInfo _itemIdField;

        private void Awake()
        {
            _screwDestruction = GetComponent<ScrewStructureDestruction>();
            _screwStructure = GetComponent<ScrewStructure>();
            _electricStructure = GetComponent<ElectricDeviceScrewStructure>();

            // Initialize Reflection
            if (_screwDestruction != null)
            {
                var type = _screwDestruction.GetType();
                _currentHpField = AccessTools.Field(type, "_currentHp");
                _structureHpField = AccessTools.Field(type, "_structureHp");
                _structureHpProp = AccessTools.Property(type, "StructureHp");
            }

            _itemIdField = AccessTools.Field(typeof(ItemInstance), "_itemID");
        }

        public void OnImpact(System.Object sender, System.Object impactData)
        {
            if (!Config.PrefabRepairEnabled.Value) return;

            if (!_isPlated)
            {
                CheckForPlated(false);
            }

            // Check IsPlayer (Reflection)
            var isPlayerProp = AccessTools.Property(impactData.GetType(), "IsPlayer");
            if (isPlayerProp != null)
            {
                bool isPlayer = (bool)isPlayerProp.GetValue(impactData);
                if (!isPlayer) return;
            }

            if (Time.time - _lastImpact < 0.1f) return;
            _lastImpact = Time.time;

            // Check Root Transform
            var getRootMethod = AccessTools.Method(sender.GetType(), "GetRootTransform");
            if (getRootMethod != null)
            {
                var transform = getRootMethod.Invoke(sender, null) as Transform;
                if (transform != null && transform.root != LocalPlayer.Transform) return;
            }

            // Check for Repair Tool (ID 422)
            var rightHand = LocalPlayer.Inventory.RightHandItem;
            if (rightHand == null) return;

            // Get ItemID logic
            int itemId = 0;
            if (_itemIdField != null)
            {
                itemId = (int)_itemIdField.GetValue(rightHand);
            }
            
            if (itemId != 422) return;

            // Repair Logic
            if (_currentHpField == null || _structureHpProp == null) return;

            int currentHp = (int)_currentHpField.GetValue(_screwDestruction);
            int maxHp = (int)_structureHpProp.GetValue(_screwDestruction);

            if (currentHp < maxHp)
            {
                currentHp++;
                _currentHpField.SetValue(_screwDestruction, currentHp);
                
                if (currentHp == maxHp)
                {
                    // Sound logic omitted for safety
                }
            }
        }

        public void CheckForPlated(bool gotApplied)
        {
            try 
            {
                bool isGoldPlated = false;
                if (_screwStructure != null && _screwStructure.IsGoldPlated) isGoldPlated = true;
                if (_electricStructure != null && _electricStructure.IsGoldPlated) isGoldPlated = true;

                if (!isGoldPlated) return;

                if (_structureHpField == null || _currentHpField == null) return;

                // Solafite Logic (Boost HP)
                _structureHpField.SetValue(_screwDestruction, 12);
                
                int currentHp = (int)_currentHpField.GetValue(_screwDestruction);

                if (gotApplied)
                {
                    currentHp += 6;
                }
                else if (currentHp == 6)
                {
                    currentHp = 12;
                }
                
                _currentHpField.SetValue(_screwDestruction, currentHp);
                _isPlated = true;
            }
            catch (Exception ex)
            {
                RLog.Warning($"[PrefabRepair] Error in CheckForPlated: {ex}");
            }
        }
    }
}

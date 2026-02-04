using System.Collections.Generic;
using System.Reflection;
using RedLoader;
using Sons.Atmosphere;
using Sons.Gameplay;
using UnityEngine;
using HarmonyLib;

namespace ProjectX.Master.Modules.MeatDryer
{
    [RegisterTypeInIl2Cpp]
    public class MeatDryerController : MonoBehaviour
    {
        private float _timer;
        private DryingRackHookPoint[] _hookPoints;
        private SeasonsManager _seasonsManager;
        private bool[] _lastTempVolumeStates;
        
        // Cached reflection fields (Optimized Hybrid Restoration pattern)
        private static FieldInfo _activeSeasonField;
        private static FieldInfo _isInsideTemperatureVolumeField;
        private static FieldInfo _dryBoostMultiplierField;
        private static FieldInfo _minTemperatureToGainDryBoostField;
        private static bool _reflectionCached = false;

        private void Start()
        {
            _seasonsManager = Object.FindObjectOfType<SeasonsManager>();
            _hookPoints = GetComponentsInChildren<DryingRackHookPoint>();
            
            if (_hookPoints != null && _hookPoints.Length > 0)
            {
                _lastTempVolumeStates = new bool[_hookPoints.Length];
                RLog.Msg($"[MeatDryer] Found {_hookPoints.Length} hook points on drying rack");
            }
            else
            {
                RLog.Warning("[MeatDryer] No DryingRackHookPoints found on DryingRack!");
            }
            
            if (_seasonsManager == null)
            {
                RLog.Warning("[MeatDryer] SeasonsManager not found!");
            }
            
            // Cache reflection fields once
            CacheReflection();
        }
        
        private static void CacheReflection()
        {
            if (_reflectionCached) return;
            
            try
            {
                _activeSeasonField = AccessTools.Field(typeof(SeasonsManager), "_activeSeason");
                _isInsideTemperatureVolumeField = AccessTools.Field(typeof(DryingRackHookPoint), "_isInsideTemperatureVolume");
                _dryBoostMultiplierField = AccessTools.Field(typeof(DryingRackHookPoint), "_dryBoostMultiplier");
                _minTemperatureToGainDryBoostField = AccessTools.Field(typeof(DryingRackHookPoint), "_minTemperatureToGainDryBoost");
                
                // Log discovery results
                RLog.Msg($"[MeatDryer] Field discovery: _activeSeason={_activeSeasonField != null}, " +
                         $"_isInsideTemperatureVolume={_isInsideTemperatureVolumeField != null}, " +
                         $"_dryBoostMultiplier={_dryBoostMultiplierField != null}, " +
                         $"_minTemperatureToGainDryBoost={_minTemperatureToGainDryBoostField != null}");
                
                _reflectionCached = true;
            }
            catch (System.Exception ex)
            {
                RLog.Warning($"[MeatDryer] CacheReflection failed: {ex.Message}");
            }
        }

        private void Update()
        {
            if (_hookPoints == null || _hookPoints.Length == 0 || _seasonsManager == null) return;
            if (_activeSeasonField == null) return; // Reflection failed

            _timer += Time.deltaTime;
            if (_timer >= Config.MeatDryerProximityCheckInterval.Value)
            {
                _timer = 0f;
                ProcessDryingBoost();
            }
        }

        private void ProcessDryingBoost()
        {
            // Get current season via cached reflection
            int activeSeason;
            try
            {
                activeSeason = (int)_activeSeasonField.GetValue(_seasonsManager);
            }
            catch
            {
                return; // Silent fail - reflection issue
            }

            // Determine boost amount based on season
            // 0=Spring, 1=Summer, 2=Autumn, 3=Winter
            float boostAmount;
            float minTemp;
            
            switch (activeSeason)
            {
                case 0: // Spring
                    boostAmount = 30f;
                    minTemp = 20f;
                    break;
                case 1: // Summer
                    boostAmount = 25f;
                    minTemp = 15f;
                    break;
                case 2: // Autumn
                    boostAmount = 15f;
                    minTemp = 30f;
                    break;
                case 3: // Winter
                    boostAmount = 5f;
                    minTemp = 50f;
                    break;
                default:
                    boostAmount = 15f;
                    minTemp = 25f;
                    break;
            }

            for (int i = 0; i < _hookPoints.Length; i++)
            {
                var hook = _hookPoints[i];
                if (hook == null) continue;
                if (_isInsideTemperatureVolumeField == null || _dryBoostMultiplierField == null) continue;

                try
                {
                    // Get current state via cached reflection
                    bool isInside = (bool)_isInsideTemperatureVolumeField.GetValue(hook);
                    
                    // Only update when state changes (optimization from original mod)
                    if (isInside != _lastTempVolumeStates[i])
                    {
                        _lastTempVolumeStates[i] = isInside;
                        
                        // Set boost and temperature values
                        if (_minTemperatureToGainDryBoostField != null)
                        {
                            _minTemperatureToGainDryBoostField.SetValue(hook, minTemp);
                        }
                        _dryBoostMultiplierField.SetValue(hook, isInside ? boostAmount : 0f);
                    }
                }
                catch
                {
                    // Silent fail for individual hook point
                }
            }
        }
    }
}

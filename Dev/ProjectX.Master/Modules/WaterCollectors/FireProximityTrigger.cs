using System.Collections.Generic;
using RedLoader;
using Sons.Atmosphere;
using Sons.Gameplay;
using UnityEngine;
using HarmonyLib;

namespace ProjectX.Master.Modules.WaterCollectors
{
    [RegisterTypeInIl2Cpp]
    public class FireProximityTrigger : MonoBehaviour
    {
        private float _timer;
        private float _checkInterval = 5f; 
        private RainCatcher _rainCatcher;
        private SeasonsManager _seasonsManager;

        private void Start()
        {
            _seasonsManager = Object.FindObjectOfType<SeasonsManager>();
            
            Transform parent = transform.parent;
            if (parent != null)
            {
                var rainCatcherTransform = parent.Find("RainCatcherInteraction");
                if (rainCatcherTransform != null)
                {
                    _rainCatcher = rainCatcherTransform.GetComponent<RainCatcher>();
                }
            }

            if (_rainCatcher == null)
            {
                RLog.Error("[WaterCollectors] RainCatcher component not found!");
            }
        }

        private void Update()
        {
            if (_rainCatcher == null || _seasonsManager == null) return;

            // Reflection to get _activeSeason
            int activeSeason = (int)AccessTools.Field(typeof(SeasonsManager), "_activeSeason").GetValue(_seasonsManager);

            // Only run logic in Winter (Season 3?) - Check original code for enum value
            // Original: if (this._seasonsManager._activeSeason != 3) return;
            if (activeSeason != 3) return;

            _timer += Time.deltaTime;
            if (_timer >= _checkInterval)
            {
                _timer = 0f;
                CheckFire();
            }
        }

        private void CheckFire()
        {
            bool fireNearby = IsFireNearby();
            
            var seasonField = AccessTools.Field(typeof(RainCatcher), "_currentSeason");
            var setFrozenMethod = AccessTools.Method(typeof(RainCatcher), "SetFrozen");

            if (fireNearby)
            {
                // Trick game into thinking it's Summer (1) for this catcher
                seasonField.SetValue(_rainCatcher, 1);
                setFrozenMethod.Invoke(_rainCatcher, new object[] { false });
            }
            else
            {
                // Reset to Winter (3)
                seasonField.SetValue(_rainCatcher, 3);
                setFrozenMethod.Invoke(_rainCatcher, new object[] { true });
            }
        }
        
        private bool IsFireNearby()
        {
            float radius = Config.WaterCollectorHeatRadius.Value;
            
            // Use OverlapSphere to find fires
            // LayerMask: Default or Interaction? Let's check all
            Collider[] hits = Physics.OverlapSphere(transform.position, radius);
            
            foreach (var hit in hits)
            {
                // Check for fire components
                // StructureCampFire, StructureFire, etc.
                // Or check for "FireElement" name like original mod
                
                Transform t = hit.transform;
                while (t != null)
                {
                     if (t.name.Contains("FireElement")) // Weak check but matches original mod logic
                     {
                         // Check if active
                         var tempMod = t.GetComponentInChildren<TemperatureModifierVolume>();
                         if (tempMod != null && tempMod.isActiveAndEnabled)
                         {
                             return true;
                         }
                     }
                     t = t.parent;
                     if (t == null) break; // Safety
                }
            }
            return false;
        }
    }
}

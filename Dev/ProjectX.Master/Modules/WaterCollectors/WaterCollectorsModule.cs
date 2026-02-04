using SonsSdk;
using SonsSdk.Attributes;
using UnityEngine;
using RedLoader;
using HarmonyLib; // For AccessTools
using Sons.Crafting.Structures;

namespace ProjectX.Master.Modules.WaterCollectors
{
    public static class WaterCollectorsModule
    {
        private static bool _initialized = false;

        public static void Init()
        {
            SdkEvents.OnGameActivated.Subscribe(OnGameActivated);
        }

        private static void OnGameActivated()
        {
            if (_initialized) return;

            try
            {
                // ID 56 = Rain Catcher Prefab
                var recipe = ConstructionTools.GetRecipe(56);
                if (recipe != null)
                {
                    // Reflection to get _builtPrefab
                    var prefab = AccessTools.Field(typeof(StructureRecipe), "_builtPrefab")?.GetValue(recipe) as GameObject;
                    
                    if (prefab != null)
                    {
                        // Check if already added (idempotency)
                        if (prefab.transform.Find("FireProximityTrigger") == null)
                        {
                            // Create child object for the trigger
                            GameObject triggerObj = new GameObject("FireProximityTrigger");
                            triggerObj.transform.SetParent(prefab.transform, false);
                            triggerObj.transform.localPosition = Vector3.zero;

                            var rb = triggerObj.AddComponent<Rigidbody>();
                            rb.isKinematic = true;
                            
                            // Add our logic component
                            triggerObj.AddComponent<FireProximityTrigger>();
                            
                            RLog.Msg("[WaterCollectors] Injected FireProximityTrigger into RainCatcher prefab.");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                RLog.Warning($"[WaterCollectors] OnGameActivated failed: {ex.Message}");
            }
            
            _initialized = true;
        }
    }
}

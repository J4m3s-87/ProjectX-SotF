using System.Reflection;
using SonsSdk;
using UnityEngine;
using RedLoader;
using Sons.Crafting.Structures;
using HarmonyLib;

namespace ProjectX.Master.Modules.MeatDryer
{
    public static class MeatDryerModule
    {
        private static bool _initialized = false;
        private static FieldInfo _builtPrefabField;

        public static void Init()
        {
            // Cache reflection for StructureRecipe._builtPrefab
            _builtPrefabField = AccessTools.Field(typeof(StructureRecipe), "_builtPrefab");
            
            if (_builtPrefabField == null)
            {
                RLog.Warning("[MeatDryer] Could not find StructureRecipe._builtPrefab field!");
            }
            else
            {
                RLog.Msg("[MeatDryer] Module registered, waiting for game activation...");
            }
            
            SdkEvents.OnGameActivated.Subscribe(OnGameActivated);
        }

        private static void OnGameActivated()
        {
            if (_initialized) return;

            try
            {
                // ID 19 = Drying Rack Prefab (From original RealisticMeatDryer: boltEntity._prefabId == 19)
                var recipe = ConstructionTools.GetRecipe(19);
                
                if (recipe != null)
                {
                    // Get prefab via cached reflection
                    GameObject prefab = null;
                    
                    if (_builtPrefabField != null)
                    {
                        prefab = _builtPrefabField.GetValue(recipe) as GameObject;
                    }
                    
                    if (prefab != null)
                    {
                        // Check if already added
                        if (prefab.GetComponent<MeatDryerController>() == null)
                        {
                            prefab.AddComponent<MeatDryerController>();
                            RLog.Msg("[MeatDryer] Injected MeatDryerController into DryingRack prefab (ID 19).");
                        }
                        else
                        {
                            RLog.Msg("[MeatDryer] MeatDryerController already exists on DryingRack prefab.");
                        }
                        _initialized = true;
                    }
                    else
                    {
                        RLog.Warning("[MeatDryer] Recipe 19 found but _builtPrefab is null. Will retry next activation.");
                    }
                }
                else
                {
                    RLog.Warning("[MeatDryer] Recipe 19 (Drying Rack) not found! Will retry next activation.");
                }
            }
            catch (System.Exception ex)
            {
                RLog.Warning($"[MeatDryer] OnGameActivated failed: {ex.Message}");
            }
        }
    }
}

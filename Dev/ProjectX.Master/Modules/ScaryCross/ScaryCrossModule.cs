using System;
using RedLoader;
using SonsSdk;
using UnityEngine;
using Sons.Crafting.Structures;
using Sons.Items.Core;

namespace ProjectX.Master.Modules.ScaryCross
{
    public static class ScaryCrossModule
    {
        public static GameObject _bonFireElementPrefab;
        public static GameObject _heldCrossPrefab;
        private static bool _firstRun = true;

        public static void Init()
        {
            RLog.Msg("ScaryCross Module Initialized.");
            SdkEvents.OnGameActivated.Subscribe(OnGameActivated);
        }

        private static void OnGameActivated()
        {
            if (_firstRun)
            {
                try 
                {
                    // ID 71 = Cross Recipe
                    // Original simply does: ConstructionTools.GetRecipe(71)._builtPrefab.AddComponent<MakeCrossScary>();
                    var recipe = ConstructionTools.GetRecipe(71);
                    if (recipe != null)
                    {
                        try
                        {
                            // Try direct access like original mod
                            var builtPrefab = recipe._builtPrefab;
                            if (builtPrefab != null)
                            {
                                builtPrefab.AddComponent<MakeCrossScary>();
                                RLog.Msg("[ScaryCross] Injected MakeCrossScary component to Cross Prefab.");
                            }
                            else
                            {
                                RLog.Warning("[ScaryCross] Recipe(71)._builtPrefab is null.");
                            }
                        }
                        catch (Exception fieldEx)
                        {
                            RLog.Warning($"[ScaryCross] Direct _builtPrefab access failed: {fieldEx.Message}");
                        }
                    }
                    else
                    {
                        RLog.Warning("[ScaryCross] Failed to find Cross Recipe (71).");
                    }

                    // ID 403 = Standing Fire
                    try
                    {
                        var fireProfile = ConstructionTools.GetProfile(403);
                        if (fireProfile != null && fireProfile.Prefab != null)
                        {
                            _bonFireElementPrefab = fireProfile.Prefab.gameObject;
                        }
                        else
                        {
                            RLog.Warning("[ScaryCross] Standing Fire profile (403) not found or has null prefab.");
                        }
                    }
                    catch (Exception fireEx)
                    {
                        RLog.Warning($"[ScaryCross] Fire profile access failed: {fireEx.Message}");
                    }

                    // ID 468 = Cross Item
                    try
                    {
                        var heldCross = ItemTools.GetHeldPrefab(468);
                        if (heldCross != null)
                        {
                            _heldCrossPrefab = heldCross.gameObject;
                        }
                        else
                        {
                            RLog.Warning("[ScaryCross] Held Cross prefab (468) not found.");
                        }
                    }
                    catch (Exception heldEx)
                    {
                        RLog.Warning($"[ScaryCross] Held prefab access failed: {heldEx.Message}");
                    }
                }
                catch (Exception ex)
                {
                    RLog.Error($"[ScaryCross] Error in OnGameActivated: {ex.Message}");
                }

                _firstRun = false;
            }
        }
    }
}

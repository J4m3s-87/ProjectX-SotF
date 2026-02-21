using System;
using System.Collections.Generic;
using RedLoader;
using Sons.Ai.Vail;
using UnityEngine;

namespace ProjectX.Master.Modules.ScaryCross
{
    /// <summary>
    /// Detects burnable enemies (non-cannibal) near the cross using a
    /// static tracked-actor list maintained by Harmony hooks on VailActor.
    /// 
    /// Physics.OverlapSphere is FULLY stripped by IL2CPP (both 2-param and 3-param).
    /// Instead, ScaryCrossModule patches VailActor.OnEnable/OnDisable to maintain
    /// a global list of active VailActors, and this component checks distances.
    /// </summary>
    [RegisterTypeInIl2Cpp]
    public class DemonDetector : MonoBehaviour
    {
        public bool IsEnemyInRange { get; private set; }
        
        public float triggerRadius = 14f;
        
        private float _diagTimer;
        private static int _diagCount;
        
        // VailActorTypeId values for enemies that should trigger the cross burning.
        // Excludes regular cannibals, animals, and friendly NPCs (Robby=9, Virginia=10).
        private static readonly HashSet<int> _burnableTypes = new HashSet<int>
        {
            42, // Demon
            45, // DemonBoss
            48, // BossMutant
            49, // CreepyVirginia
            50, // Armsy
            58, // Legsy
            59, // Holey
        };
        
        // Global tracked VailActors — populated by Harmony hooks in ScaryCrossModule
        internal static readonly List<VailActor> TrackedActors = new List<VailActor>();

        /// <summary>
        /// IL2CPP does NOT call Start() on [RegisterTypeInIl2Cpp] types.
        /// Called explicitly from MakeCrossScary.Initialize().
        /// </summary>
        public void Initialize()
        {
            RLog.Msg($"[DemonDetector] Initialized — radius={this.triggerRadius}, burnableTypes={_burnableTypes.Count}, trackedActors={TrackedActors.Count}");
        }

        public void SetTriggerRadius(float radius)
        {
            this.triggerRadius = radius;
        }

        /// <summary>
        /// Scans the global TrackedActors list for burnable enemies within range.
        /// Uses Vector3.Distance instead of Physics.OverlapSphere (stripped by IL2CPP).
        /// </summary>
        public void ManualUpdate()
        {
            bool found = false;
            var position = base.transform.position;
            float radiusSq = triggerRadius * triggerRadius;
            
            // Diagnostic timer — log every 5 seconds
            _diagTimer += Time.deltaTime;
            bool shouldDiag = _diagTimer >= 5f;
            if (shouldDiag) _diagTimer = 0f;
            
            try
            {
                int actorCount = TrackedActors.Count;
                
                // Heartbeat: log every 5 seconds (first 12 only)
                if (shouldDiag && _diagCount < 12)
                {
                    _diagCount++;
                    RLog.Msg($"[DemonDetector] Tick #{_diagCount}: scanning {actorCount} tracked actors (radius={triggerRadius}m)");
                }
                
                // Iterate backwards to safely remove nulls
                for (int i = actorCount - 1; i >= 0; i--)
                {
                    VailActor actor = null;
                    try { actor = TrackedActors[i]; } catch { }
                    
                    if (actor == null || actor.gameObject == null)
                    {
                        TrackedActors.RemoveAt(i);
                        continue;
                    }
                    
                    try
                    {
                        int typeId = (int)actor.TypeId;
                        if (!_burnableTypes.Contains(typeId)) continue;
                        
                        // Distance check (squared for performance)
                        var actorPos = actor.transform.position;
                        float dx = actorPos.x - position.x;
                        float dy = actorPos.y - position.y;
                        float dz = actorPos.z - position.z;
                        float distSq = dx * dx + dy * dy + dz * dz;
                        
                        if (distSq <= radiusSq)
                        {
                            found = true;
                            
                            if (shouldDiag && _diagCount <= 12)
                            {
                                float dist = (float)Math.Sqrt(distSq);
                                RLog.Msg($"[DemonDetector] DETECTED burnable enemy! typeId={typeId} name='{actor.gameObject.name}' dist={dist:F1}m");
                            }
                            break;
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                RLog.Warning($"[DemonDetector] ManualUpdate error: {ex.Message}");
            }
            
            this.IsEnemyInRange = found;
        }
    }
}

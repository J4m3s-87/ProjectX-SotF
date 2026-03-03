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
    /// 
    /// Dead enemies are detected via position staleness: if a burnable actor
    /// stays at the exact same position for STALE_THRESHOLD seconds, it's
    /// considered dead and removed from tracking. Dead enemies ragdoll to a
    /// fixed point; living enemies always have animation micro-movement.
    /// </summary>
    [RegisterTypeInIl2Cpp]
    public class DemonDetector : MonoBehaviour
    {
        public bool IsEnemyInRange { get; private set; }
        
        /// <summary>
        /// Actors currently in range — populated each ManualUpdate tick.
        /// Used by MakeCrossScary.BurnDemons() for direct IgniteSelf() calls.
        /// </summary>
        public readonly List<VailActor> InRangeActors = new List<VailActor>();
        
        public float triggerRadius = 14f;
        
        private float _diagTimer;
        private int _diagCount;
        
        /// <summary>
        /// Staleness tracking — detect dead enemies by position not changing.
        /// Key: actor instance ID. Value: (last known position, seconds at that position).
        /// </summary>
        private const float STALE_THRESHOLD = 10f;
        private const float STALE_MOVE_EPSILON = 0.05f; // less than this = "not moving"
        private readonly Dictionary<int, StalenessEntry> _stalenessTracker = new Dictionary<int, StalenessEntry>();
        
        private struct StalenessEntry
        {
            public Vector3 lastPos;
            public float staleTime;
        }
        
        // VailActorTypeId values for enemies that should trigger the cross burning.
        // Includes actual mutants/creepies only. Excludes armored cannibals
        // (Frank=51, Eddy=52, Greg=53, Henry=54, Igor=55, Elise=56 wear Creepy
        // Armor but are cannibals), regular cannibals, animals, and friendly NPCs.
        private static readonly HashSet<int> _burnableTypes = new HashSet<int>
        {
            11, // Fingers
            19, // Twins
            40, // John2
            42, // Demon
            45, // DemonBoss
            46, // PuffyBossMale
            47, // PuffyBossFemale
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
        /// Dead enemies filtered via position staleness (no movement for STALE_THRESHOLD seconds).
        /// </summary>
        public void ManualUpdate()
        {
            bool found = false;
            InRangeActors.Clear();
            var position = base.transform.position;
            float radiusSq = triggerRadius * triggerRadius;
            float dt = Time.deltaTime;
            
            // Diagnostic timer — log every 5 seconds
            _diagTimer += dt;
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
                    
                    if (actor == null || actor.gameObject == null || !actor.gameObject.activeInHierarchy)
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
                            // Staleness check — dead enemies ragdoll to a fixed position.
                            // If position hasn't changed by more than STALE_MOVE_EPSILON
                            // for STALE_THRESHOLD seconds, consider the enemy dead.
                            int actorId = actor.GetInstanceID();
                            
                            if (_stalenessTracker.TryGetValue(actorId, out var entry))
                            {
                                float moveDx = actorPos.x - entry.lastPos.x;
                                float moveDy = actorPos.y - entry.lastPos.y;
                                float moveDz = actorPos.z - entry.lastPos.z;
                                float moveSq = moveDx * moveDx + moveDy * moveDy + moveDz * moveDz;
                                
                                if (moveSq < STALE_MOVE_EPSILON * STALE_MOVE_EPSILON)
                                {
                                    // Not moving — accumulate stale time
                                    entry.staleTime += dt;
                                    entry.lastPos = actorPos;
                                    _stalenessTracker[actorId] = entry;
                                    
                                    if (entry.staleTime >= STALE_THRESHOLD)
                                    {
                                        // Dead — remove from tracking
                                        TrackedActors.RemoveAt(i);
                                        _stalenessTracker.Remove(actorId);
                                        RLog.Msg($"[DemonDetector] STALE: removed '{actor.gameObject.name}' (typeId={typeId}) — no movement for {entry.staleTime:F1}s");
                                        continue;
                                    }
                                }
                                else
                                {
                                    // Moving — reset stale timer
                                    entry.lastPos = actorPos;
                                    entry.staleTime = 0f;
                                    _stalenessTracker[actorId] = entry;
                                }
                            }
                            else
                            {
                                // First time seeing this actor — start tracking
                                _stalenessTracker[actorId] = new StalenessEntry { lastPos = actorPos, staleTime = 0f };
                            }
                            
                            found = true;
                            InRangeActors.Add(actor);
                            
                            if (shouldDiag)
                            {
                                float dist = (float)Math.Sqrt(distSq);
                                RLog.Msg($"[DemonDetector] DETECTED burnable enemy! typeId={typeId} name='{actor.gameObject.name}' dist={dist:F1}m stale={(_stalenessTracker.TryGetValue(actorId, out var se) ? se.staleTime : 0f):F1}s");
                            }
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

using System;
using System.Collections.Generic;
using RedLoader;
using UnityEngine;

namespace ProjectX.Master.Modules.ScaryCross
{
    [RegisterTypeInIl2Cpp]
    public class DemonDetector : MonoBehaviour
    {
        public bool IsEnemyInRange { get; private set; }
        
        public float triggerRadius = 5f;
        public string enemyTag = "Enemy";
        
        private SphereCollider _triggerCollider;
        private List<Collider> _demonsInRange = new List<Collider>();

        private void Start()
        {
            this._triggerCollider = base.gameObject.AddComponent<SphereCollider>();
            this._triggerCollider.isTrigger = true;
            this._triggerCollider.radius = this.triggerRadius;
            this._triggerCollider.radius = this.triggerRadius;
            // this._triggerCollider.excludeLayers = ~(1 << LayerMask.NameToLayer("Character"));
        }

        public void SetTriggerRadius(float radius)
        {
            this.triggerRadius = radius;
            if (_triggerCollider)
                this._triggerCollider.radius = radius;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Original logic checks for child "Demon(alive)"
            if (other.transform.Find("Demon(alive)") != null)
            {
                this._demonsInRange.Add(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (this._demonsInRange.Contains(other))
            {
                this._demonsInRange.Remove(other);
            }
            this.IsEnemyInRange = this._demonsInRange.Count > 0;
        }

        private void Update()
        {
            for (int i = this._demonsInRange.Count - 1; i >= 0; i--)
            {
                if (this._demonsInRange[i] == null || !this._demonsInRange[i].gameObject.activeInHierarchy) // Safety check
                {
                     this._demonsInRange.RemoveAt(i);
                     continue;
                }

                var demon = this._demonsInRange[i].transform.Find("Demon(alive)");
                if (demon == null || !demon.gameObject.activeInHierarchy) // Fix obsolete active
                {
                    this._demonsInRange.RemoveAt(i);
                }
            }
            this.IsEnemyInRange = this._demonsInRange.Count > 0;
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class CollisionIgnore : MonoBehaviour
    {
        public bool ignoreCollisionOnAwake = true;
        public List<Collider> colliders = new List<Collider>();

        private void OnValidate()
        {
            if (!gameObject.activeInHierarchy) return;
            if (colliders.Count == 0)
            {
                GetChildColliders();
            }
        }

        protected void Awake()
        {
            if (ignoreCollisionOnAwake)
            {
                Set(true);
            }
        }

        public void Set(bool ignoreCollision)
        {
            foreach (Collider collider1 in colliders)
            {
                foreach (Collider collider2 in colliders)
                {
                    if (collider1 != collider2)
                    {
                        Physics.IgnoreCollision(collider1, collider2, ignoreCollision);
                    }
                }
            }
        }

        [Button]
        public void GetChildColliders()
        {
            colliders = new List<Collider>(this.gameObject.GetComponentsInChildren<Collider>(true));
        }
    }
}

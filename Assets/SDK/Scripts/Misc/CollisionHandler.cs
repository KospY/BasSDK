using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Collision handler")]
    [RequireComponent(typeof(Rigidbody))]
    public class CollisionHandler : MonoBehaviour
    {
        public bool active = true;
        public bool checkMinVelocity = true;
        public bool enterOnly = false;

        public bool customInertiaTensor;
        public CapsuleCollider customInertiaTensorCollider;

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public List<CollisionHandler> penetratedObjects = new List<CollisionHandler>();

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public List<Holder> holders = new List<Holder>();

        private void OnValidate()
        {
            if (customInertiaTensor)
            {
                if (customInertiaTensorCollider == null)
                {
                    Transform customInertiaTensorTransform = this.transform.Find("InertiaTensorCollider");
                    if (customInertiaTensorTransform)
                    {
                        customInertiaTensorCollider = customInertiaTensorTransform.GetComponent<CapsuleCollider>();
                    }
                    else
                    {
                        customInertiaTensorTransform = new GameObject("InertiaTensorCollider").transform;
                        customInertiaTensorTransform.SetParent(this.transform, false);
                    }
                    if (customInertiaTensorCollider == null)
                    {
                        customInertiaTensorCollider = customInertiaTensorTransform.gameObject.AddComponent<CapsuleCollider>();
                        customInertiaTensorCollider.radius = 0.05f;
                        customInertiaTensorCollider.direction = 2;
                    }
                }
                customInertiaTensorCollider.enabled = false;
                customInertiaTensorCollider.isTrigger = true;
                customInertiaTensorCollider.gameObject.layer = 2;
            }
        }

    }
}

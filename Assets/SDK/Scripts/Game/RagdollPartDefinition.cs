using System;
using System.Collections.Generic;
using UnityEngine;

namespace BS
{
    public class RagdollPartDefinition : MonoBehaviour
    {
        public Transform meshBone;
        [NonSerialized]
        public Transform animationBone;
        [NonSerialized]
        public Rigidbody rb;

        public List<RagdollPartDefinition> ignoredParts;

        protected RagdollDefinition ragdollDefinition;

        public void Awake()
        {
            ragdollDefinition = this.GetComponentInParent<RagdollDefinition>();
            rb = this.GetComponent<Rigidbody>();
            foreach (RagdollPartDefinition part in ignoredParts)
            {
                foreach (Collider thisCollider in this.GetComponentsInChildren<Collider>())
                {
                    foreach (Collider ignoredCollider in part.GetComponentsInChildren<Collider>())
                    {
                        Physics.IgnoreCollision(thisCollider, ignoredCollider, true);
                    }
                }
            }
        }
    }
}
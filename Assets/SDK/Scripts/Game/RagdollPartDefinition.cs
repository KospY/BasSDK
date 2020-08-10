using System;
using System.Collections.Generic;
using UnityEngine;

using EasyButtons;

namespace ThunderRoad
{
    public class RagdollPartDefinition : MonoBehaviour
    {
        public Transform meshBone;
        [NonSerialized]
        public Transform animationBone;
        [NonSerialized]
        public Rigidbody rb;

        public List<RagdollPartDefinition> ignoredParts;

        protected CreatureDefinition creatureDefinition;

        public void Awake()
        {
            creatureDefinition = this.GetComponentInParent<CreatureDefinition>();
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

        [Button]
        public void SetPositionToBone()
        {
            this.transform.position = meshBone.position;
            this.transform.rotation = meshBone.rotation;
            this.transform.localScale = meshBone.localScale;
        }

    }
}
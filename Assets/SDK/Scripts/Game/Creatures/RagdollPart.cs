using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Ragdoll part")]
    [RequireComponent(typeof(CollisionHandler))]
    public class RagdollPart : MonoBehaviour
    {
        [Header("Part")]
        public Transform meshBone;
        public Transform[] linkedMeshBones;
        public Type type;
        public bool sliceAllowed;
        public float sliceWidth = 0.04f;
        public float sliceJointBreakForce = 5000;
        public Transform sliceTransform;
        public Material sliceFillMaterial;

        public float springPositionMultiplier = 1;
        public float damperPositionMultiplier = 1;
        public float springRotationMultiplier = 1;
        public float damperRotationMultiplier = 1;

        public List<RagdollPart> ignoredParts;

        protected bool initialized;

        [NonSerialized]
        public bool bodyDamagerIsAttack;
        [NonSerialized]
        public Rigidbody rb;
        [NonSerialized]
        public Ragdoll ragdoll;
        [NonSerialized]
        public ColliderGroup colliderGroup;
        [NonSerialized]
        public CollisionHandler collisionHandler;

        public enum Type
        {
            None,
            Head,
            Neck,
            Torso,
            LeftArm,
            RightArm,
            LeftHand,
            RightHand,
            LeftLeg,
            RightLeg,
            LeftFoot,
            RightFoot,
        }

        [Button]
        public void SetPositionToBone()
        {
            this.transform.position = meshBone.position;
            this.transform.rotation = meshBone.rotation;
            this.transform.localScale = meshBone.localScale;
        }

        [Button]
        public void FindBoneFromName()
        {
            ragdoll = this.GetComponentInParent<Ragdoll>();
            foreach (Transform child in ragdoll.meshRig.GetComponentsInChildren<Transform>())
            {
                if (child.name == this.name)
                {
                    meshBone = child;
                    return;
                }
            }
        }

        protected virtual void Awake()
        {
            rb = this.GetComponent<Rigidbody>();
            colliderGroup = this.GetComponentInChildren<ColliderGroup>();
            this.gameObject.layer = LayerMask.NameToLayer(LayerName.NPC.ToString());
            foreach (Collider collider in colliderGroup.GetComponentsInChildren<Collider>(true))
            {
                collider.gameObject.layer = LayerMask.NameToLayer(LayerName.NPC.ToString());
            }
            foreach (RagdollPart part in ignoredParts)
            {
                foreach (Collider thisCollider in colliderGroup.GetComponentsInChildren<Collider>(true))
                {
                    foreach (Collider ignoredCollider in part.GetComponentsInChildren<Collider>(true))
                    {
                        Physics.IgnoreCollision(thisCollider, ignoredCollider, true);
                    }
                }
            }
        }

    }
}
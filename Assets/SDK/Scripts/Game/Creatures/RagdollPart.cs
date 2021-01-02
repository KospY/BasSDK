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
        public Vector3 boneToChildDirection = Vector3.left;
        public RagdollPart parentPart;

        [Header("Dismemberment")]
        public bool sliceAllowed;
        [Range(0, 1)]
        public float sliceParentAdjust = 0.5f;
        public float sliceWidth = 0.04f;
        public float sliceHeight = 0;
        public float sliceThreshold = 0.5f;
        public Material sliceFillMaterial;
        public RagdollPart sliceByPassPart;
        public bool sliceDisableCollider;
        public bool ripBreak = false;
        public float ripBreakForce = 3000;

        [Header("Forces")]
        public float springPositionMultiplier = 1;
        public float damperPositionMultiplier = 1;
        public float springRotationMultiplier = 1;
        public float damperRotationMultiplier = 1;

        public List<RagdollPart> ignoredParts;

#if PrivateSDK
        [NonSerialized]
        public CreatureData.PartData data;
#endif

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

        protected virtual void OnValidate()
        {
            if (parentPart == null)
            {
                CharacterJoint characterJoint = this.GetComponent<CharacterJoint>();
                if (characterJoint) parentPart = characterJoint.connectedBody.GetComponent<RagdollPart>();
            }
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
        public virtual void GetSlicePositionAndDirection(out Vector3 position, out Vector3 direction)
        {
            direction = GetSliceDirection();
            position = meshBone.transform.position + (direction * sliceHeight);
        }

        public virtual Vector3 GetSliceDirection()
        {
            Vector3 sliceDirection = parentPart ? Vector3.Lerp(meshBone.transform.TransformDirection(boneToChildDirection), parentPart.meshBone.transform.TransformDirection(parentPart.boneToChildDirection), sliceParentAdjust) : meshBone.transform.TransformDirection(boneToChildDirection);
            return sliceDirection;
        }

        protected virtual void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (sliceAllowed)
            {
                GetSlicePositionAndDirection(out Vector3 slicePosition, out Vector3 sliceDirection);
                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.DrawWireDisc(slicePosition + (-sliceDirection * sliceWidth), sliceDirection, 0.1f);
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawWireDisc(slicePosition + (sliceDirection * sliceWidth), sliceDirection, 0.1f);
            }
#endif
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
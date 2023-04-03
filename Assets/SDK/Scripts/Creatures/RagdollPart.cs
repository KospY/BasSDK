using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ThunderRoad.Manikin;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/RagdollPart")]
    [AddComponentMenu("ThunderRoad/Creatures/Ragdoll part")]
    [RequireComponent(typeof(CollisionHandler))]
    public class RagdollPart : ThunderBehaviour
    {
        [Header("Part")]
        public Transform meshBone;
        public Transform[] linkedMeshBones;
        public Type type;
        public Section section = Section.Full;
        [SerializeField]
        private Axis frontAxis = Axis.Forwards;
        [SerializeField]
        private Axis upAxis = Axis.Left;
        public Vector3 boneToChildDirection = Vector3.left;
        public RagdollPart parentPart;
        public bool hasParent { get; protected set; }
        public bool ignoreStaticCollision;

        [Header("Dismemberment")]
        public bool sliceAllowed;
        [Range(0, 1)]
        public float sliceParentAdjust = 0.5f;
        public float sliceWidth = 0.04f;
        public float sliceHeight = 0;
        public float sliceThreshold = 0.5f;
        public Material sliceFillMaterial;
        [Tooltip("Disable this part collider and slice the referenced child part on slice (usefull for necks)")]
        public RagdollPart sliceChildAndDisableSelf;
        public bool ripBreak = false;
        public float ripBreakForce = 3000;

        [Header("Forces")]
        public float springPositionMultiplier = 1;
        public float damperPositionMultiplier = 1;
        public float springRotationMultiplier = 1;
        public float damperRotationMultiplier = 1;

        public List<RagdollPart> ignoredParts;

        [NonSerialized]
        public bool initialized;
        [NonSerialized]
        public bool bodyDamagerIsAttack;
        [NonSerialized]
        public PhysicBody physicBody;
        [NonSerialized]
        public Ragdoll ragdoll;
        
        [NonSerialized]
        public ColliderGroup colliderGroup;
        [NonSerialized]
        public CollisionHandler collisionHandler;

        public Wearable wearable;

        [NonSerialized]
        public bool hasMetalArmor;

        [Flags]
        public enum Type
        {
            Head = (1 << 0),
            Neck = (1 << 1),
            Torso = (1 << 2),
            LeftArm = (1 << 3),
            RightArm = (1 << 4),
            LeftHand = (1 << 5),
            RightHand = (1 << 6),
            LeftLeg = (1 << 7),
            RightLeg = (1 << 8),
            LeftFoot = (1 << 9),
            RightFoot = (1 << 10),
            LeftWing = (1 << 11),
            RightWing = (1 << 12),
            Tail = (1 << 13),
        }

        public enum Section
        {
            Full,
            Lower,
            Mid,
            Upper
        }

        public enum Axis
        {
            Right,
            Left,
            Up,
            Down,
            Forwards,
            Backwards
        }

        protected virtual void OnValidate()
        {
            if (!gameObject.activeInHierarchy) return;
            if (parentPart == null)
            {
                CharacterJoint characterJoint = this.GetComponent<CharacterJoint>();
                if (characterJoint)
                    parentPart = characterJoint.connectedBody.GetComponent<RagdollPart>();
            }
        }

        [Button]
        public void SetAllowSlice(bool allow) => sliceAllowed = allow;

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

        protected virtual void OnDrawGizmosSelected()
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

#if ODIN_INSPECTOR
        [ShowInInspector] 
#endif
        [NonSerialized]
        public List<RagdollPart> childParts = new List<RagdollPart>();
#if ODIN_INSPECTOR
        [ShowInInspector] 
#endif
        [NonSerialized]
        public List<Creature.RendererData> renderers = new List<Creature.RendererData>();
#if ODIN_INSPECTOR
        [ShowInInspector] 
#endif
        [NonSerialized]
        public List<SkinnedMeshRenderer> skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
#if ODIN_INSPECTOR
        [ShowInInspector] 
#endif
        [NonSerialized]
        public List<int> skinnedMeshRendererIndexes = new List<int>();
#if ODIN_INSPECTOR
        [ShowInInspector] 
#endif
        [NonSerialized]
        public List<SkinnedMeshRenderer> meshpartSkinnedMeshRenderers = new List<SkinnedMeshRenderer>();
#if ODIN_INSPECTOR
        [ShowInInspector] 
#endif
        [NonSerialized]
        public List<Creature.RendererData> meshpartRendererList = new List<Creature.RendererData>();
#if ODIN_INSPECTOR
        [ShowInInspector] 
#endif
        [NonSerialized]
        public Ragdoll.Bone bone;
        [NonSerialized]
        public bool isSliced;
        [NonSerialized]
        public Transform slicedMeshRoot;

        [NonSerialized]
        public Transform root;
        [NonSerialized]
        public Vector3 rootOrgLocalPosition;
        [NonSerialized]
        public Quaternion rootOrgLocalRotation;

        public Vector3 forwardDirection
        {
            get => AxisToDirection(frontAxis);
        }
        public Vector3 upDirection
        {
            get => AxisToDirection(upAxis);
        }

        [NonSerialized]
        public Vector3 savedPosition;
        [NonSerialized]
        public Quaternion savedRotation;

        [NonSerialized]
        public List<HandleRagdoll> handles;
        [NonSerialized]
        public bool isGrabbed;
        [NonSerialized]
        public Damager bodyDamager;

        [NonSerialized]
        public CharacterJoint characterJoint;
        public CharacterJointData orgCharacterJointData;

        [NonSerialized]
        public bool characterJointLocked;

        public delegate void TouchActionDelegate(RagdollHand ragdollHand, Interactable interactable, Interactable.Action action);
        public event TouchActionDelegate OnTouchActionEvent;

        public delegate void HeldActionDelegate(RagdollHand ragdollHand, HandleRagdoll handle, Interactable.Action action);
        public event HeldActionDelegate OnHeldActionEvent;

        public class CharacterJointData
        {
            public Vector3 localPosition;
            public Quaternion localRotation;
            public Rigidbody connectedBody;
            public Vector3 anchor;
            public Vector3 axis;
            public bool autoConfigureConnectedAnchor;
            public Vector3 connectedAnchor;
            public Vector3 swingAxis;
            public SoftJointLimitSpring twistLimitSpring;
            public SoftJointLimit lowTwistLimit;
            public SoftJointLimit highTwistLimit;
            public SoftJointLimitSpring swingLimitSpring;
            public SoftJointLimit swing1Limit;
            public SoftJointLimit swing2Limit;
            public bool enableProjection;
            public float projectionDistance;
            public float projectionAngle;
            public float breakForce;
            public float breakTorque;
            public bool enableCollision;
            public bool enablePreprocessing;
            public float massScale;
            public float connectedMassScale;

            public CharacterJointData(CharacterJoint characterJoint)
            {
                localPosition = characterJoint.connectedBody.transform.InverseTransformPoint(characterJoint.transform.position);
                localRotation = Quaternion.Inverse(characterJoint.connectedBody.transform.rotation) * characterJoint.transform.rotation;
                connectedBody = characterJoint.connectedBody;
                anchor = characterJoint.anchor;
                axis = characterJoint.axis;
                autoConfigureConnectedAnchor = characterJoint.autoConfigureConnectedAnchor;
                connectedAnchor = characterJoint.connectedAnchor;
                swingAxis = characterJoint.swingAxis;
                twistLimitSpring = characterJoint.twistLimitSpring;
                lowTwistLimit = characterJoint.lowTwistLimit;
                highTwistLimit = characterJoint.highTwistLimit;
                swingLimitSpring = characterJoint.swingLimitSpring;
                swing1Limit = characterJoint.swing1Limit;
                swing2Limit = characterJoint.swing2Limit;
                enableProjection = characterJoint.enableProjection;
                projectionDistance = characterJoint.projectionDistance;
                projectionAngle = characterJoint.projectionAngle;
                breakForce = characterJoint.breakForce;
                breakTorque = characterJoint.breakTorque;
                enableCollision = characterJoint.enableCollision;
                enablePreprocessing = characterJoint.enablePreprocessing;
                massScale = characterJoint.massScale;
                connectedMassScale = characterJoint.connectedMassScale;
            }

            public CharacterJoint CreateJoint(GameObject gameobject, bool resetPosition = true)
            {
                if (resetPosition)
                {
                    gameobject.transform.position = connectedBody.transform.TransformPoint(localPosition);
                    gameobject.transform.rotation = connectedBody.transform.rotation * localRotation;
                }
                CharacterJoint characterJoint = gameobject.AddComponent<CharacterJoint>();
                characterJoint.anchor = anchor;
                characterJoint.axis = axis;
                characterJoint.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
                characterJoint.connectedAnchor = connectedAnchor;
                characterJoint.swingAxis = swingAxis;
                characterJoint.twistLimitSpring = twistLimitSpring;
                characterJoint.lowTwistLimit = lowTwistLimit;
                characterJoint.highTwistLimit = highTwistLimit;
                characterJoint.swingLimitSpring = swingLimitSpring;
                characterJoint.swing1Limit = swing1Limit;
                characterJoint.swing2Limit = swing2Limit;
                characterJoint.enableProjection = enableProjection;
                characterJoint.projectionDistance = projectionDistance;
                characterJoint.projectionAngle = projectionAngle;
                characterJoint.breakForce = breakForce;
                characterJoint.breakTorque = breakTorque;
                characterJoint.enableCollision = enableCollision;
                characterJoint.enablePreprocessing = enablePreprocessing;
                characterJoint.massScale = massScale;
                characterJoint.connectedMassScale = connectedMassScale;
                characterJoint.connectedBody = connectedBody;
                return characterJoint;
            }
        }

        public virtual void OnRagdollEnable()
        { }

        public virtual void OnRagdollDisable()
        { }

        private Vector3 AxisToDirection(Axis axis)
        {
            switch (axis)
            {
                case Axis.Right:
                    return transform.right;
                case Axis.Left:
                    return -transform.right;
                case Axis.Up:
                    return transform.up;
                case Axis.Down:
                    return -transform.up;
                case Axis.Forwards:
                    return transform.forward;
                case Axis.Backwards:
                    return -transform.forward;
                default:
                    return transform.forward;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ThunderRoad.Manikin;
using ThunderRoad.Skill.SpellPower;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Creatures/RagdollPart.html")]
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
        [Min(0.00001f)]
        public float handledMass = -1f;
        [Min(0.00001f)]
        public float ragdolledMass = -1f;
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
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer) return;
#endif
            if (!gameObject.activeInHierarchy) return;
            if (parentPart == null)
            {
                CharacterJoint characterJoint = this.GetComponent<CharacterJoint>();
                if (characterJoint)
                    parentPart = characterJoint.connectedBody.GetComponent<RagdollPart>();
            }
            if (ragdolledMass < 0 && this.GetPhysicBody() is PhysicBody pb)
            {
                ragdolledMass = pb.mass;
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
        public void SetPositionToBoneLeaveChildren()
        {
            List<Transform> childs = new List<Transform>();
            foreach (Transform child in GetComponentsInChildren<Transform>())
            {
                childs.Add(child);
                child.SetParent(transform.parent, true);
            }
            this.transform.position = meshBone.position;
            this.transform.rotation = meshBone.rotation;
            this.transform.localScale = meshBone.localScale;
            foreach (Transform child in childs)
            {
                child.SetParent(transform, true);
            }
        }

#if UNITY_EDITOR
        [Button]
        public void ConvertToHand()
        {
            RagdollPart.Type type = !this.type.ToString().Contains("Left") ? Type.RightHand : (!this.type.ToString().Contains("Right") ? Type.LeftHand : Type.LeftHand | Type.RightHand);
            ConvertToHandOrFoot(type);
        }

        [Button]
        public void ConvertToFoot()
        {
            RagdollPart.Type type = !this.type.ToString().Contains("Left") ? Type.RightFoot : (!this.type.ToString().Contains("Right") ? Type.LeftFoot : Type.LeftFoot | Type.RightFoot);
            ConvertToHandOrFoot(type);
        }

        private void ConvertToHandOrFoot(RagdollPart.Type type)
        {
            RagdollPart newPart = null;
            if (type.ToString().Contains("Hand"))
            {
                newPart = gameObject.AddComponent<RagdollHand>();
            }
            else if (type.ToString().Contains("Foot"))
            {
                newPart = gameObject.AddComponent<RagdollFoot>();
            }
            if (newPart != null)
            {
                newPart.meshBone = this.meshBone;
                newPart.linkedMeshBones = this.linkedMeshBones;
                newPart.type = this.type | type;
                newPart.section = this.section;
                newPart.frontAxis = this.frontAxis;
                newPart.upAxis = this.upAxis;
                newPart.boneToChildDirection = this.boneToChildDirection;
                newPart.parentPart = this.parentPart;
                newPart.ignoreStaticCollision = this.ignoreStaticCollision;
                newPart.sliceAllowed = this.sliceAllowed;
                newPart.sliceParentAdjust = this.sliceParentAdjust;
                newPart.sliceWidth = this.sliceWidth;
                newPart.sliceHeight = this.sliceHeight;
                newPart.sliceThreshold = this.sliceThreshold;
                newPart.sliceFillMaterial = this.sliceFillMaterial;
                newPart.sliceChildAndDisableSelf = this.sliceChildAndDisableSelf;
                newPart.ripBreak = this.ripBreak;
                newPart.ripBreakForce = this.ripBreakForce;
                newPart.handledMass = this.handledMass;
                newPart.ragdolledMass = this.ragdolledMass;
                newPart.springPositionMultiplier = this.springPositionMultiplier;
                newPart.damperPositionMultiplier = this.damperPositionMultiplier;
                newPart.springRotationMultiplier = this.springRotationMultiplier;
                newPart.damperRotationMultiplier = this.damperRotationMultiplier;
                DestroyImmediate(this);
            }
        }
#endif

        [Button]
        public void IgnoreParentPart()
        {
            if (parentPart == null) return;
            if (ignoredParts == null) ignoredParts = new List<RagdollPart>();
            if (ignoredParts.Contains(parentPart)) return;
            ignoredParts.Add(parentPart);
        }

        [Button]
        public void IgnoreAllParts()
        {
            if (ignoredParts == null) ignoredParts = new List<RagdollPart>();
            ignoredParts.Clear();
            foreach (RagdollPart part in GetComponentInParent<Ragdoll>().GetComponentsInChildren<RagdollPart>())
            {
                if (part == this) continue;
                ignoredParts.Add(part);
            }
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

        [Button]
        public void AssignJointBodyFromParent()
        {
            if (parentPart != null) GetComponent<CharacterJoint>()?.SetConnectedPhysicBody(parentPart.physicBody ?? parentPart.GetPhysicBody());
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
        [ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public float standingMass = -1f;
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

        public int damagerTier = 0;

        public delegate void TouchActionDelegate(RagdollHand ragdollHand, Interactable interactable, Interactable.Action action);
        public event TouchActionDelegate OnTouchActionEvent;

        public delegate void HandlerEvent(RagdollHand ragdollHand, HandleRagdoll handle);
        public event HandlerEvent OnGrabbed;
        public event HandlerEvent OnUngrabbed;
        public delegate void TKHandlerEvent(Skill.SpellPower.SpellTelekinesis spellTelekinesis, HandleRagdoll handle);
        public event TKHandlerEvent OnTKGrab;
        public event TKHandlerEvent OnTKRelease;

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


        [Button]
        public bool TrySlice()
        {
            return false;
        }

    }
}

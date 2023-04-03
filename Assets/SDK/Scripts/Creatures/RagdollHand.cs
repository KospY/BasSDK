using UnityEngine;
using System;
using System.Collections.Generic;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/RagdollHand")]
    [AddComponentMenu("ThunderRoad/Creatures/Ragdoll hand")]
    public class RagdollHand : RagdollPart
    {
        [Header("Hand")]
        public Side side = Side.Right;
        public RagdollPart lowerArmPart;
        public RagdollPart upperArmPart;

        public bool meshFixedScale = true;
        public Vector3 meshGlobalScale = Vector3.one;

        public Vector3 axisThumb = Vector3.up;
        public Vector3 axisPalm = Vector3.left;

        public Collider touchCollider;

        public WristStats wristStats;
        public RagdollHandPoser poser;

        [Header("Fingers")]
        public Finger fingerThumb = new Finger();
        public Finger fingerIndex = new Finger();
        public Finger fingerMiddle = new Finger();
        public Finger fingerRing = new Finger();
        public Finger fingerLittle = new Finger();
        public List<Finger> fingers = new List<Finger>();
        public Collider palmCollider;
        public Collider simplifiedCollider;

        /// <summary>
        /// The colliders of the linked forearm
        /// </summary>
        public List<Collider> ForeArmColliders
        {
            get
            {
                if (foreArmColliders != null) return foreArmColliders;

                if (!lowerArmPart) return null;

                var armColliderGroup = lowerArmPart.colliderGroup;
                if (!armColliderGroup) return null;

                foreArmColliders = armColliderGroup.colliders;
                return foreArmColliders;
            }

            set => foreArmColliders = value;
        }

        private List<Collider> foreArmColliders;

        public Vector3 PalmDir
        {
            get => -transform.forward;
        }

        public Vector3 PointDir
        {
            get => -transform.right;
        }

        public Vector3 ThumbDir
        {
            get => (side == Side.Right) ? transform.up : -transform.up;
        }

        [Serializable]
        public class Finger
        {
            public Bone proximal = new Bone();
            public Bone intermediate = new Bone();
            public Bone distal = new Bone();
            public Transform tip;

            [Serializable]
            public class Bone
            {
                public Transform mesh;
                public Transform animation;
                public CapsuleCollider collider;
                public Transform colliderTransform;
            }
        }


        public Finger GetFinger(HandPoseData.FingerType type)
        {
            return type switch
            {
                HandPoseData.FingerType.Thumb => fingerThumb,
                HandPoseData.FingerType.Index => fingerIndex,
                HandPoseData.FingerType.Middle => fingerMiddle,
                HandPoseData.FingerType.Ring => fingerRing,
                HandPoseData.FingerType.Little => fingerLittle,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        [NonSerialized]
        public RagdollHand otherHand;
        [NonSerialized]
        public Creature creature;
        [NonSerialized]
        public Transform grip;
        [NonSerialized]
        public SpellCaster caster;


        protected override void OnValidate()
        {
            base.OnValidate();
            if (!this.gameObject.activeInHierarchy) return;
            grip = this.transform.Find("Grip");
            if (!grip) grip = CreateDefaultGrip();
            if (creature == null) creature = this.GetComponentInParent<Creature>();
            if (poser == null) poser = this.GetComponent<RagdollHandPoser>();
        }

        [Button]
        public void MirrorFingersToOtherHand()
        {
            if (!creature) creature = this.GetComponentInParent<Creature>();
            foreach (RagdollHand ragdollHand in creature.GetComponentsInChildren<RagdollHand>())
            {
                if (ragdollHand != this)
                {
                    otherHand = ragdollHand;
                    break;
                }
            }
            DestroyImmediate(otherHand.palmCollider.gameObject);
            GameObject otherFingers = Instantiate(palmCollider.gameObject, otherHand.transform);
            otherFingers.name = palmCollider.name;
            otherFingers.transform.MirrorChilds(new Vector3(1, -1, 1));
            // Dirty fix to avoid negative scale
            foreach (Transform transform in otherFingers.GetComponentsInChildren<Transform>())
            {
                transform.localScale = Vector3.one;
            }
            otherHand.SetupFingers();
        }

        [Button]
        public virtual void SetGripToDefaultPosition()
        {
            Transform grip = this.transform.Find("Grip");
            if (grip) DestroyImmediate(grip.gameObject);
            grip = CreateDefaultGrip();
        }

        public virtual Transform CreateDefaultGrip()
        {
            Transform newGrip = new GameObject("Grip").transform;
            newGrip.transform.SetParent(this.transform);
            newGrip.transform.localScale = Vector3.one;
            if (side == Side.Left)
            {
                newGrip.transform.localPosition = new Vector3(-0.042f, -0.01f, 0.003f);
                newGrip.transform.localRotation = Quaternion.Euler(0, 220, -90);
            }
            if (side == Side.Right)
            {
                newGrip.transform.localPosition = new Vector3(0.042f, -0.01f, 0.003f);
                newGrip.transform.localRotation = Quaternion.Euler(0, 140, 90);
            }
            return newGrip;
        }

        [Button]
        public virtual void SetupFingers()
        {
            if (!creature) creature = this.GetComponentInParent<Creature>();

            fingers = new List<Finger>();

            palmCollider = this.transform.Find("Palm")?.GetComponent<Collider>();
            if (palmCollider == null)
            {
                palmCollider = new GameObject("Palm").AddComponent<BoxCollider>();
                palmCollider.transform.SetParentOrigin(this.transform);
                (palmCollider as BoxCollider).size = new Vector3(0.1f, 0.1f, 0.03f);
                palmCollider.gameObject.AddComponent<ColliderGroup>();
            }

            fingerThumb.proximal.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightThumbProximal : HumanBodyBones.LeftThumbProximal); if (!fingerThumb.proximal.mesh) Debug.LogError("Could not find ThumbProximal bone on animator");
            fingerThumb.intermediate.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightThumbIntermediate : HumanBodyBones.LeftThumbIntermediate); if (!fingerThumb.intermediate.mesh) Debug.LogError("Could not find ThumbIntermediate bone on animator");
            fingerThumb.distal.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightThumbDistal : HumanBodyBones.LeftThumbDistal); if (!fingerThumb.distal.mesh) Debug.LogError("Could not find ThumbDistal bone on animator");
            SetupFinger(fingerThumb, "Thumb");
            if (fingerThumb.proximal.mesh) fingers.Add(fingerThumb);

            fingerIndex.proximal.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightIndexProximal : HumanBodyBones.LeftIndexProximal); if (!fingerIndex.proximal.mesh) Debug.LogError("Could not find IndexProximal bone on animator");
            fingerIndex.intermediate.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightIndexIntermediate : HumanBodyBones.LeftIndexIntermediate); if (!fingerIndex.intermediate.mesh) Debug.LogError("Could not find IndexIntermediate bone on animator");
            fingerIndex.distal.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightIndexDistal : HumanBodyBones.LeftIndexDistal); if (!fingerIndex.distal.mesh) Debug.LogError("Could not find IndexDistal bone on animator");
            SetupFinger(fingerIndex, "Index");
            if (fingerIndex.proximal.mesh) fingers.Add(fingerIndex);

            fingerMiddle.proximal.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightMiddleProximal : HumanBodyBones.LeftMiddleProximal); if (!fingerMiddle.proximal.mesh) Debug.LogError("Could not find MiddleProximal bone on animator");
            fingerMiddle.intermediate.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightMiddleIntermediate : HumanBodyBones.LeftMiddleIntermediate); if (!fingerMiddle.intermediate.mesh) Debug.LogError("Could not find MiddleIntermediate bone on animator");
            fingerMiddle.distal.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightMiddleDistal : HumanBodyBones.LeftMiddleDistal); if (!fingerMiddle.distal.mesh) Debug.LogError("Could not find MiddleDistal bone on animator");
            SetupFinger(fingerMiddle, "Middle");
            if (fingerMiddle.proximal.mesh) fingers.Add(fingerMiddle);

            fingerRing.proximal.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightRingProximal : HumanBodyBones.LeftRingProximal); if (!fingerRing.proximal.mesh) Debug.LogError("Could not find RingProximal bone on animator");
            fingerRing.intermediate.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightRingIntermediate : HumanBodyBones.LeftRingIntermediate); if (!fingerRing.intermediate.mesh) Debug.LogError("Could not find RingIntermediate bone on animator");
            fingerRing.distal.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightRingDistal : HumanBodyBones.LeftRingDistal); if (!fingerRing.distal.mesh) Debug.LogError("Could not find RingDistal bone on animator");
            SetupFinger(fingerRing, "Ring");
            if (fingerRing.proximal.mesh) fingers.Add(fingerRing);

            fingerLittle.proximal.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightLittleProximal : HumanBodyBones.LeftLittleProximal); if (!fingerLittle.proximal.mesh) Debug.LogError("Could not find LittleProximal bone on animator");
            fingerLittle.intermediate.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightLittleIntermediate : HumanBodyBones.LeftLittleIntermediate); if (!fingerLittle.intermediate.mesh) Debug.LogError("Could not find LittleIntermediate bone on animator");
            fingerLittle.distal.mesh = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightLittleDistal : HumanBodyBones.LeftLittleDistal); if (!fingerLittle.distal.mesh) Debug.LogError("Could not find LittleDistal bone on animator");
            SetupFinger(fingerLittle, "Little");
            if (fingerLittle.proximal.mesh) fingers.Add(fingerLittle);
        }

        protected virtual void SetupFinger(Finger finger, string name)
        {
            // Ragdoll proximal
            if (!finger.proximal.collider) finger.proximal.collider = palmCollider.transform.Find(name + "Proximal")?.GetComponent<CapsuleCollider>();
            
            if (!finger.proximal.collider)
            {
                finger.proximal.collider = new GameObject(name + "Proximal").AddComponent<CapsuleCollider>();
                finger.proximal.collider.radius = 0.01f;
                finger.proximal.collider.height = 0.05f;
                finger.proximal.collider.direction = 0;
                finger.proximal.collider.transform.SetParent(palmCollider.transform);
            }
            Transform proximalColliderTransform = finger.proximal.collider.transform;
            proximalColliderTransform.SetPositionAndRotation(finger.proximal.mesh.position,finger.proximal.mesh.rotation);
            finger.proximal.colliderTransform = proximalColliderTransform;

            if (!finger.intermediate.collider) finger.intermediate.collider = proximalColliderTransform.Find(name + "Intermediate")?.GetComponent<CapsuleCollider>();
            // Ragdoll intermediate
            
            if (!finger.intermediate.collider)
            {
                finger.intermediate.collider = new GameObject(name + "Intermediate").AddComponent<CapsuleCollider>();
                finger.intermediate.collider.radius = 0.01f;
                finger.intermediate.collider.height = 0.05f;
                finger.intermediate.collider.direction = 0;
                finger.intermediate.collider.transform.SetParent(proximalColliderTransform);
            }
            Transform intermediateColliderTransform = finger.intermediate.collider.transform;
            intermediateColliderTransform.SetPositionAndRotation(finger.intermediate.mesh.position,finger.intermediate.mesh.rotation);
            finger.intermediate.colliderTransform = intermediateColliderTransform;
            
            // Ragdoll distal
            if (!finger.distal.collider) finger.distal.collider = intermediateColliderTransform.Find(name + "Distal")?.GetComponent<CapsuleCollider>();
            
            if (!finger.distal.collider)
            {
                finger.distal.collider = new GameObject(name + "Distal").AddComponent<CapsuleCollider>();
                finger.distal.collider.radius = 0.01f;
                finger.distal.collider.height = 0.05f;
                finger.distal.collider.direction = 0;
                finger.distal.collider.transform.SetParent(intermediateColliderTransform);
            }
            Transform distalColliderTransform = finger.distal.collider.transform;
            distalColliderTransform.SetPositionAndRotation(finger.distal.mesh.position,finger.distal.mesh.rotation);
            finger.distal.colliderTransform = distalColliderTransform;
            
            // Tip
            string tipName = name + "Tip";
            finger.tip = distalColliderTransform.Find(tipName);
            if (!finger.tip)
            {
                finger.tip = new GameObject(tipName).transform;
                finger.tip.SetParent(distalColliderTransform);
            }
            finger.tip.localRotation = Quaternion.identity;
            finger.tip.localPosition = Vector3.zero;
        }

    }
}
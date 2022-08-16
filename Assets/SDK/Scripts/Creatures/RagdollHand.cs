using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;


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
            }
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
            IconManager.SetIcon(this.gameObject, null);
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
            creature = this.GetComponentInParent<Creature>();

            fingers = new List<Finger>();
            Transform meshBone = null;

            palmCollider = this.transform.Find("Palm")?.GetComponent<Collider>();
            if (palmCollider == null)
            {
                palmCollider = new GameObject("Palm").AddComponent<BoxCollider>();
                palmCollider.transform.SetParentOrigin(this.transform);
                (palmCollider as BoxCollider).size = new Vector3(0.1f, 0.1f, 0.03f);
                palmCollider.gameObject.AddComponent<ColliderGroup>();
            }

            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightThumbProximal : HumanBodyBones.LeftThumbProximal); if (meshBone) fingerThumb.proximal.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightThumbIntermediate : HumanBodyBones.LeftThumbIntermediate); if (meshBone) fingerThumb.intermediate.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightThumbDistal : HumanBodyBones.LeftThumbDistal); if (meshBone) fingerThumb.distal.mesh = meshBone;
            SetupFinger(fingerThumb, "Thumb");
            if (fingerThumb.proximal.mesh) fingers.Add(fingerThumb);

            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightIndexProximal : HumanBodyBones.LeftIndexProximal); if (meshBone) fingerIndex.proximal.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightIndexIntermediate : HumanBodyBones.LeftIndexIntermediate); if (meshBone) fingerIndex.intermediate.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightIndexDistal : HumanBodyBones.LeftIndexDistal); if (meshBone) fingerIndex.distal.mesh = meshBone;
            SetupFinger(fingerIndex, "Index");
            if (fingerIndex.proximal.mesh) fingers.Add(fingerIndex);

            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightMiddleProximal : HumanBodyBones.LeftMiddleProximal); if (meshBone) fingerMiddle.proximal.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightMiddleIntermediate : HumanBodyBones.LeftMiddleIntermediate); if (meshBone) fingerMiddle.intermediate.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightMiddleDistal : HumanBodyBones.LeftMiddleDistal); if (meshBone) fingerMiddle.distal.mesh = meshBone;
            SetupFinger(fingerMiddle, "Middle");
            if (fingerMiddle.proximal.mesh) fingers.Add(fingerMiddle);

            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightRingProximal : HumanBodyBones.LeftRingProximal); if (meshBone) fingerRing.proximal.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightRingIntermediate : HumanBodyBones.LeftRingIntermediate); if (meshBone) fingerRing.intermediate.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightRingDistal : HumanBodyBones.LeftRingDistal); if (meshBone) fingerRing.distal.mesh = meshBone;
            SetupFinger(fingerRing, "Ring");
            if (fingerRing.proximal.mesh) fingers.Add(fingerRing);

            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightLittleProximal : HumanBodyBones.LeftLittleProximal); if (meshBone) fingerLittle.proximal.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightLittleIntermediate : HumanBodyBones.LeftLittleIntermediate); if (meshBone) fingerLittle.intermediate.mesh = meshBone;
            meshBone = creature.animator.GetBoneTransform(side == Side.Right ? HumanBodyBones.RightLittleDistal : HumanBodyBones.LeftLittleDistal); if (meshBone) fingerLittle.distal.mesh = meshBone;
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
                finger.proximal.collider.transform.localPosition = finger.proximal.mesh.localPosition;
                finger.proximal.collider.transform.localRotation = finger.proximal.mesh.localRotation;
            }
            if (!finger.intermediate.collider) finger.intermediate.collider = finger.proximal.collider.transform.Find(name + "Intermediate")?.GetComponent<CapsuleCollider>();
            // Ragdoll intermediate
            if (!finger.intermediate.collider)
            {
                finger.intermediate.collider = new GameObject(name + "Intermediate").AddComponent<CapsuleCollider>();
                finger.intermediate.collider.radius = 0.01f;
                finger.intermediate.collider.height = 0.05f;
                finger.intermediate.collider.direction = 0;
                finger.intermediate.collider.transform.SetParent(finger.proximal.collider.transform);
                finger.intermediate.collider.transform.localPosition = finger.intermediate.mesh.localPosition;
                finger.intermediate.collider.transform.localRotation = finger.intermediate.mesh.localRotation;
            }
            // Ragdoll distal
            if (!finger.distal.collider) finger.distal.collider = finger.intermediate.collider.transform.Find(name + "Distal")?.GetComponent<CapsuleCollider>();
            if (!finger.distal.collider)
            {
                finger.distal.collider = new GameObject(name + "Distal").AddComponent<CapsuleCollider>();
                finger.distal.collider.radius = 0.01f;
                finger.distal.collider.height = 0.05f;
                finger.distal.collider.direction = 0;
                finger.distal.collider.transform.SetParent(finger.intermediate.collider.transform);
                finger.distal.collider.transform.localPosition = finger.distal.mesh.localPosition;
                finger.distal.collider.transform.localRotation = finger.distal.mesh.localRotation;
            }
            // Tip
            string tipName = name + "Tip";
            finger.tip = finger.distal.collider.transform.Find(tipName);
            if (!finger.tip)
            {
                finger.tip = new GameObject(tipName).transform;
                finger.tip.SetParent(finger.distal.collider.transform);
                finger.tip.localRotation = Quaternion.identity;
                finger.tip.localPosition = Vector3.zero;
            }
        }

    }
}
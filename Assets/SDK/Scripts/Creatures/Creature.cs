using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Collections;
using UnityEngine.Profiling;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Creature")]
    public class Creature : MonoBehaviour
    {
        public string creatureId;

        public Animator animator;
        public LODGroup lodGroup;
        public Container container;
        public Transform centerEyes;
        public Vector3 eyeCameraOffset;
        public Renderer vfxRenderer;

        [NonSerialized]
        public Ragdoll ragdoll;
        [NonSerialized]
        public Brain brain;
        [NonSerialized]
        public Locomotion locomotion;
        [NonSerialized]
        public Mana mana;
        [NonSerialized]
        public FeetClimber climber;
        [NonSerialized]
        public Equipment equipment;
        [NonSerialized]
        public RagdollHand handLeft;
        [NonSerialized]
        public RagdollHand handRight;
        [NonSerialized]
        public RagdollFoot footLeft;
        [NonSerialized]
        public RagdollFoot footRight;
        [NonSerialized]
        public LightVolumeReceiver lightVolumeReceiver;

        [Header("Speak")]
        public Transform jaw;
        public Vector3 jawMaxRotation = new Vector3(0, -30, 0);

        [Header("Fall")]
        public float fallAliveAnimationHeight = 0.5f;
        public float fallAliveDestabilizeHeight = 3;
        public float groundStabilizationMaxVelocity = 1;
        public float groundStabilizationMinDuration = 3;

        public bool toogleTPose;

        [Header("Movement")]
        public bool stepEnabled;
        public float stepThreshold = 0.2f;

        public bool turnRelativeToHand = true;
        public float headMinAngle = 30;
        public float headMaxAngle = 80;
        public float handToBodyRotationMaxVelocity = 2;
        public float handToBodyRotationMaxAngle = 30;
        public float turnSpeed = 6;
        public float ikLocomotionSpeedThreshold = 1;
        public float ikLocomotionAngularSpeedThreshold = 30f;

        public AnimationClip dynamicStartReplaceClip;
        public AnimationClip dynamicLoopReplaceClip;
        public AnimationClip dynamicEndReplaceClip;

        public static int hashDynamicOneShot, hashDynamicLoop, hashDynamicLoop3, hashDynamicInterrupt, hashDynamicSpeedMultiplier, hashIsBusy, hashFeminity, hashHeight, hashFalling, hashGetUp, hashTstance, hashStaticIdle;
        public static bool hashInitialized;

        public enum StaggerAnimation
        {
            Default,
            Parry,
            Head,
            Torso,
            Legs,
            FallGround,
        }

        public enum PushType
        {
            Magic,
            Grab,
            Hit,
            Parry,
        }


        protected void Awake()
        {
#if PrivateSDK
            this.gameObject.AddComponent<RagdollTester>();
#endif

            foreach (SkinnedMeshRenderer smr in this.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                smr.updateWhenOffscreen = true;
            }
            if (!lodGroup) lodGroup = this.GetComponentInChildren<LODGroup>();


            ragdoll = this.GetComponentInChildren<Ragdoll>();

            brain = this.GetComponentInChildren<Brain>();
            equipment = this.GetComponentInChildren<Equipment>();
            if (!container) container = this.GetComponentInChildren<Container>();

            locomotion = this.GetComponent<Locomotion>();
            mana = this.GetComponent<Mana>();
            climber = this.GetComponentInChildren<FeetClimber>();

            foreach (RagdollHand hand in this.GetComponentsInChildren<RagdollHand>())
            {
                if (hand.side == Side.Right) handRight = hand;
                if (hand.side == Side.Left) handLeft = hand;
            }
            foreach (RagdollFoot foot in this.GetComponentsInChildren<RagdollFoot>())
            {
                if (foot.side == Side.Right) footRight = foot;
                if (foot.side == Side.Left) footLeft = foot;
            }

            if (!hashInitialized) InitAnimatorHashs();

#if !PrivateSDK
            Rigidbody locomotionRb = locomotion.GetComponent<Rigidbody>();
            locomotionRb.isKinematic = true;

            foreach (RagdollPart ragdollPart in this.GetComponentsInChildren<RagdollPart>())
            {
                Rigidbody ragdollPartRb = ragdollPart.GetComponent<Rigidbody>();
                ragdollPartRb.isKinematic = true;
                ragdollPart.transform.SetParent(ragdollPart.meshBone);
                ragdollPart.SetPositionToBone();
            }
#endif

        }

        protected void InitAnimatorHashs()
        {
            hashFeminity = Animator.StringToHash("Feminity");
            hashHeight = Animator.StringToHash("Height");
            hashFalling = Animator.StringToHash("Falling");
            hashGetUp = Animator.StringToHash("GetUp");
            hashIsBusy = Animator.StringToHash("IsBusy");
            hashTstance = Animator.StringToHash("TStance");
            hashStaticIdle = Animator.StringToHash("StaticIdle");
            hashDynamicOneShot = Animator.StringToHash("DynamicOneShot");
            hashDynamicLoop = Animator.StringToHash("DynamicLoop");
            hashDynamicLoop3 = Animator.StringToHash("DynamicLoop3");
            hashDynamicInterrupt = Animator.StringToHash("DynamicInterrupt");
            hashDynamicSpeedMultiplier = Animator.StringToHash("DynamicSpeedMultiplier");
            hashInitialized = true;
        }

        protected void Update()
        {
        }


        public virtual float GetAnimatorHeightRatio()
        {
            return animator.GetFloat(hashHeight);
        }

#if !PrivateSDK
        [Button]
        protected virtual void SetRagdoll()
        {
            animator.enabled = false;
            foreach (RagdollPart ragdollPart in this.GetComponentsInChildren<RagdollPart>())
            {
                ragdollPart.transform.SetParent(ragdoll.transform, true);
                ragdollPart.meshBone.SetParentOrigin(ragdollPart.transform);
                Rigidbody ragdollPartRb = ragdollPart.GetComponent<Rigidbody>();
                ragdollPartRb.isKinematic = false;
            }
        }
#endif


    }
}

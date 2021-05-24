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
        public LODGroup logGroup;
        public Container container;
        public Transform centerEyes;
        public Vector3 eyeCameraOffset;

        public virtual void OnValidate()
        {
            if (!this.gameObject.activeInHierarchy) return;
            IconManager.SetIcon(this.gameObject, IconManager.LabelIcon.Gray);
        }

        [NonSerialized]
        public Ragdoll ragdoll;
        [NonSerialized]
        public Brain brain;
        [NonSerialized]
        public Locomotion locomotion;
        [NonSerialized]
        public Mana mana;
        [NonSerialized]
        public CreatureSpeak speak;
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

        [Header("Animation")]
        public float animationDampTime = 0.1f;

        [Header("Fall")]
        public float fallAliveAnimationHeight = 0.5f;
        public float fallAliveDestabilizeHeight = 3;
        public float groundStabilizationMaxVelocity = 1;
        public float groundStabilizationMinDuration = 3;

        public bool toogleTPose;

        [Header("Movement")]
        public bool stepEnabled;
        public float stepSpeed = 3f;
        public float stepThreshold = 0.2f;

        public float stationaryVelocityThreshold = 0.01f;

        public bool turnRelativeToHand = true;
        public float headMinAngle = 30;
        public float headMaxAngle = 80;
        public float handToBodyRotationMaxVelocity = 2;
        public float handToBodyRotationMaxAngle = 30;
        public float turnSpeed = 6;
        public float turnAnimSpeed = 0.007f;

        public static int hashIsBusy, hashFeminity, hashHeight, hashStrafe, hashTurn, hashSpeed, hashFalling, hashGetUp, hashTstance, hashStaticIdle;
        public static bool hashInitialized;



        protected void Awake()
        {
#if PrivateSDK
            this.gameObject.AddComponent<RagdollTester>();
#endif

            foreach (SkinnedMeshRenderer smr in this.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                smr.updateWhenOffscreen = true;
            }
            if (!logGroup) logGroup = this.GetComponentInChildren<LODGroup>();
            animator.applyRootMotion = false;
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            animator.enabled = false;
            ragdoll = this.GetComponentInChildren<Ragdoll>();

            brain = this.GetComponentInChildren<Brain>();
            equipment = this.GetComponentInChildren<Equipment>();
            if (!container) container = this.GetComponentInChildren<Container>();

            locomotion = this.GetComponent<Locomotion>();
            mana = this.GetComponent<Mana>();
            climber = this.GetComponentInChildren<FeetClimber>();
            speak = this.GetComponentInChildren<CreatureSpeak>();

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
            hashStrafe = Animator.StringToHash("Strafe");
            hashTurn = Animator.StringToHash("Turn");
            hashSpeed = Animator.StringToHash("Speed");
            hashFalling = Animator.StringToHash("Falling");
            hashGetUp = Animator.StringToHash("GetUp");
            hashIsBusy = Animator.StringToHash("IsBusy");
            hashTstance = Animator.StringToHash("TStance");
            hashStaticIdle = Animator.StringToHash("StaticIdle");
            hashInitialized = true;
        }

        protected void Update()
        {
            UpdateAnimation();
        }


        public virtual void UpdateAnimation()
        {
            // Apply locomotion animations
            if (locomotion.isEnabled && locomotion.isGrounded && (locomotion.horizontalSpeed + Mathf.Abs(locomotion.angularSpeed)) > stationaryVelocityThreshold)
            {
                locomotion.SetCapsuleCollider(this.transform.InverseTransformPoint(ragdoll.headPart.transform.position).y);
                Vector3 stepLocalVelocity = this.transform.InverseTransformDirection(locomotion.velocity);
                animator.SetFloat(hashStrafe, stepLocalVelocity.x, animationDampTime, Time.deltaTime);
                animator.SetFloat(hashTurn, locomotion.angularSpeed * turnAnimSpeed, animationDampTime, Time.deltaTime);
                animator.SetFloat(hashSpeed, stepLocalVelocity.z, animationDampTime, Time.deltaTime);
            }
            else
            {
                animator.SetFloat(hashStrafe, 0, animationDampTime, Time.deltaTime);
                animator.SetFloat(hashTurn, 0, animationDampTime, Time.deltaTime);
                animator.SetFloat(hashSpeed, 0, animationDampTime, Time.deltaTime);
            }
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

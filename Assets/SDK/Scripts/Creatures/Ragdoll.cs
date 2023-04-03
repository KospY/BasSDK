using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Ragdoll")]
    [AddComponentMenu("ThunderRoad/Creatures/Ragdoll")]
    public class Ragdoll : ThunderBehaviour
    {
        public Transform meshRig;
        public Transform meshRootBone;

        [Header("Parts")]
        public RagdollPart headPart;
        public RagdollPart leftUpperArmPart;
        public RagdollPart rightUpperArmPart;
        public RagdollPart targetPart;
        public RagdollPart rootPart;

        [Header("Default forces")]
        public float springPositionForce = 1000;
        public float damperPositionForce = 50;
        public float maxPositionForce = 1000;

        public float springRotationForce = 800;
        public float damperRotationForce = 50;
        public float maxRotationForce = 100;

        [Header("Destabilized")]
        public float destabilizedSpringRotationMultiplier = 0.5f;
        public float destabilizedDamperRotationMultiplier = 0.1f;
        public float destabilizedGroundSpringRotationMultiplier = 0.2f;

        [Header("HipsAttached")]
        public float hipsAttachedSpringPositionMultiplier = 1f;
        public float hipsAttachedDamperPositionMultiplier = 0f;
        public float hipsAttachedSpringRotationMultiplier = 1f;
        public float hipsAttachedDamperRotationMultiplier = 0f;

        [Header("StandUp")]
        public AnimationCurve standUpCurve;
        public float standUpFromGrabDuration = 1.0f;
        public float preStandUpDuration = 3f;
        public float preStandUpRatio = 0.7f;

        [Header("Player arm")]
        public float playerArmPositionSpring = 5000f;
        public float playerArmPositionDamper = 40f;
        public float playerArmRotationSpring = 1000f;
        public float playerArmRotationDamper = 40f;
        public float playerArmMaxPositionForce = 3000f;
        public float playerArmMaxRotationForce = 250f;

        [Header("Collision")]
        public float collisionEffectMinDelay = 0.2f;
        public float collisionMinVelocity = 2.0f;
        [NonSerialized]
        public float lastCollisionEffectTime;

        [Header("Misc")]
        public bool allowSelfDamage;
        public bool grippable = true;

        [NonSerialized]
        public Creature creature;

#if ODIN_INSPECTOR
        [InlineButton("TogglePhysic")]
#endif
        [Header("Physic toggle")]
        public bool physicToggle;
        public float physicTogglePlayerRadius = 5;
        public float physicToggleRagdollRadius = 3;
        public float physicEnabledDuration = 2;
        protected float lastPhysicToggleTime;

        public static bool playerPhysicBody;

        [NonSerialized]
        public Transform animatorRig;
        [NonSerialized]
        public float totalMass;
#if ODIN_INSPECTOR
        [DisableInPlayMode]
#endif
        public State state = State.Disabled;

        public enum State
        {
            Inert,
            Destabilized,
            Frozen,
            Standing,
            Kinematic,
            NoPhysic,
            Disabled,
        }

        public bool hipsAttached;

        public List<RagdollPart> parts;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [NonSerialized]
        public List<Bone> bones = new List<Bone>();


        [NonSerialized]
        public IkController ik;
        [NonSerialized]
        public List<RagdollHand> handlers = new List<RagdollHand>();
        [NonSerialized]
        public List<SpellCaster> tkHandlers = new List<SpellCaster>();

        [NonSerialized]
        public bool isGrabbed;
        [NonSerialized]
        public bool isTkGrabbed;

        [NonSerialized]
        public bool charJointBreakEnabled;

        [NonSerialized]
        protected List<object> physicToggleModifiers = new List<object>();

        [NonSerialized]
        public List<PhysicModifier> physicModifiers = new List<PhysicModifier>();

        public class PhysicModifier
        {
            public object handler;
            public EffectData effectData;
            public PhysicModifier(object handler, EffectData effectData = null)
            {
                this.handler = handler;
                this.effectData = effectData;
            }
        }

        [NonSerialized]
        public bool initialized;

        [NonSerialized]
        public bool standingUp;
        protected Coroutine getUpCoroutine;


        /// <summary> 
        /// Helper class to keep the line of which objects would like to apply a stabilization joint.
        /// </summary>
        private class StabilizationJointQueue
        {
            public GameObject owningObject;
            //public GameObject relativeObject;
            public StabilizationJointSettings settings;

            public StabilizationJointQueue(GameObject owningObject, StabilizationJointSettings settings)
            {
                this.owningObject = owningObject;
                this.settings = settings;
            }
        }

        /// <summary>
        /// Helper class to assign necessary settings to the joint,
        /// nearly identical to the stabilization joint, but additional stuff might be needed sometimes
        /// </summary>
        public class StabilizationJointSettings
        {
            public Vector3 axis = Vector3.zero;
            public ConfigurableJointMotion angularXMotion = ConfigurableJointMotion.Free;
            public ConfigurableJointMotion angularYMotion = ConfigurableJointMotion.Free;
            public ConfigurableJointMotion angularZMotion = ConfigurableJointMotion.Free;
            public bool configuredInWorldSpace = false;
            public bool autoConfigureConnectedAnchor = true;
            public SoftJointLimit angularXLimit;
            public SoftJointLimitSpring angularXLimitSpring;
            public SoftJointLimit angularYLimit;
            public SoftJointLimit angularZLimit;
            public SoftJointLimitSpring angularYZLimitSpring;
            public JointDrive angularYZDrive = new JointDrive();
            public JointDrive angularXDrive = new JointDrive();
            public bool isKinematic = false;
            public GameObject relativeObject = null;
        }

        // The joint required to stabilize the motion of the ragdoll when it's connected to by something and is in the air.
        ConfigurableJoint stabilizationJoint;
        GameObject stabilizationJointFollowObject;
        // The queue (Really a list) to see which objects would like to add a joint to stabilize the ragdoll in the air.
        List<StabilizationJointQueue> stabilizationJointQueue = new List<StabilizationJointQueue>();

        // Gets the stabilization joint for use in any other components. i.e. Telekinesis to rotate character to face the players hand forward
        public ConfigurableJoint StabilizationJoint { get { return stabilizationJoint; } }



        public enum PhysicStateChange
        {
            None,
            ParentingToPhysic,
            PhysicToParenting,
        }


        [System.Serializable]
        public class Bone
        {
            public int[] boneHashes;
            public Transform mesh;
            public Transform animation;
            public Transform meshSplit;
            public ConfigurableJoint animationJoint;
            public FixedJoint fixedJoint;

            public RagdollPart part;
            public Bone parent;
            public List<Bone> childs;
            public bool hasChildAnimationJoint;

            public Vector3 orgLocalPosition;
            public Quaternion orgLocalRotation;

            public Vector3 orgCreatureLocalPosition;
            public Quaternion orgCreatureLocalRotation;

        }

    }
}

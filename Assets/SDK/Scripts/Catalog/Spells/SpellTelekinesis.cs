using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class SpellTelekinesis : SpellData
    {
        public static bool holdGrip;
        public static bool showHighlighter = true;
        [Range(0f, 1f)]
        public float palmDirectionPercent = 0.35f;
        public float maxCatchDistance = 6f;
        public float radius = 1f;
        public float maxAngle = 40;
        [Range(0f, 1f)]
        public float raycastArmInfluence = 0.7f;
        [Range(0f, 1f)]
        public float targetingLookInfluence = 0.2f;
        [Range(0f, 1f)]
        public float antiBrokenBias = 1.0f;
        [Range(0f, 1f)]
        public float lastHeldBias = 10f / 180f;
        [Range(0f, 1f)]
        public float stickyBias = 3f / 180f;
        public LayerMask layerMask;
        public LayerMask lineOfSightMask;
        public LayerMask itemLayerMask;

        [Header("Forces")]
        public float positionSpring = 100;
        public float positionDamper = 10;
        public float positionMaxForce = 1000;

        public float rotationSpring = 10;
        public float rotationDamper = 1;
        public float rotationMaxForce = 1000;

        public float drag = 5;
        public float angularDrag = 0.5f;
        public float reverseAngle = 110;
        public float minFingersWeight = 0f;
        public bool gravity = false;
        public float throwMultiplier = 2;

		[Header("Follow")]
        public AnimationCurve followDefaultSpeedCurve;
        public AnimationCurve followRagdollSpeedCurve;
        protected float catchTime;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")] 
#endif
        public string defaultHandEffectId;
        protected EffectData defaultHandEffectData;


#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")] 
#endif
        public string ragdollHandEffectId;
        protected EffectData ragdollHandEffectData;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")] 
#endif
        public string ragdollGripEffectId;
        protected EffectData ragdollGripEffectData;


#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")] 
#endif
        public string spinEffectId;
        protected EffectData spinEffectData;


        [NonSerialized]
        public float gripDistance;
        [NonSerialized]
        public Rigidbody grip;
        protected ConfigurableJoint joint;
        protected ConstantForce hangingMassForce;
        [NonSerialized]
        public bool justCatched;


        [NonSerialized]
		bool stabilizationReach = false;

		[Header("Pull / Repel")]
        public float repelMaxDistance = 12;
        public float pullAndRepelMaxSpeed = 8;
        public float pullAndRepelNoDragThreshold = 0.8f;
        [NonSerialized]
        public float pullSpeed;
        [NonSerialized]
        public float repelSpeed;
        public bool autoGrab = true;

        [Header("Spin")]
        public bool spinMode;
        public AnimationCurve spinVelocityCurve;
        public bool spinFromCenterOfMass = true;
        [NonSerialized]
        public float spinVelocity;
        protected float spinStartTime;
        protected bool spinToogle;
        public float spinManaDrain = 19f;
        public float spinManaStartConsume = 10f;
        protected float spinIntensity;
        protected float lastSpinHaptic;

        [Header("Push")]
        public float pushDefaultForce = 20;
        public float pushRagdollForce = 30;
        public float pushRagdollOtherPartsForce = 5;
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")] 
#endif
        public string pushEffectId;
        protected EffectData pushEffectData;
        public float minVelocity = 0f;
        public float maxVelocity = 5f;

        [Header("Dismemberment")]
        public bool allowDismemberment = true;
        public float dismembermentBreakForceMultiplier = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Hand pose"), ValueDropdown("GetAllHandPoseID")] 
#endif
        public string closeHandPoseId;
        [NonSerialized]
        public HandPoseData closeHandPoseData;

#if ODIN_INSPECTOR
        [BoxGroup("Hand pose"), ValueDropdown("GetAllHandPoseID")] 
#endif
        public string openHandPoseId;
        [NonSerialized]
        public HandPoseData openHandPoseData;

        protected RaycastHit[] handRayHits;
        protected List<(Collider hitCollider, float value)> sortedHits;
        protected Collider[] touchedColliders;
        protected HashSet<PhysicBody> touchedPBs;

        [NonSerialized]
        public bool grabRagdoll;
        [NonSerialized]
        private bool isPulling;
        [NonSerialized]
        private bool isRepelling;

        [NonSerialized]
		private float bodyTargetAngle;
        [NonSerialized]
		private float bodyCurrentAngle;

        [NonSerialized]
        private bool blockedTargeting = false;
        [NonSerialized]
        private bool updateTargetBlock = true;


#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
        }

        public List<ValueDropdownItem<string>> GetAllHandPoseID()
        {
            return Catalog.GetDropdownAllID(Category.HandPose);
        } 
#endif

        public new SpellTelekinesis Clone()
        {
            return this.MemberwiseClone() as SpellTelekinesis;
        }

    }
}

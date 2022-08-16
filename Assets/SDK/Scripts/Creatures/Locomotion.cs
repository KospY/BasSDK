using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Locomotion")]
    [AddComponentMenu("ThunderRoad/Locomotion")]
    [RequireComponent(typeof(Rigidbody))]
    public class Locomotion : ThunderBehaviour
    {
        [Header("Ground movement")]
        public bool allowMove = true;
        public bool allowTurn = true;
        public bool allowJump = true;
        public bool allowCrouch = true;

        public AnimationCurve moveForceMultiplierByAngleCurve;
        public bool testMove;

        public float forwardSpeed = 0.2f;
        public float backwardSpeed = 0.2f;
        public float strafeSpeed = 0.2f;
        public float runSpeedAdd = 0.1f;
        public float crouchSpeed = 0.1f;

        protected float forwardSpeedMultiplier = 1f;
        protected float backwardSpeedMultiplier = 1f;
        protected float strafeSpeedMultiplier = 1f;
        protected float runSpeedMultiplier = 1f;
        protected float jumpForceMultiplier = 1f;

        public float forwardAngle = 10;
        public float backwardAngle = 10;
        public float runDot = 0.75f;
        public bool runEnabled = true;

        public float crouchHeightRatio = 0.8f;

        public float turnSpeed = 1;
        private float snapTurnTime;
        private bool smoothSnapTurn;

        [Header("Jump / Fall")]
        public float airSpeed = 0.02f;
        public ForceMode airForceMode = ForceMode.VelocityChange;
        public float jumpGroundForce = 0.3f;
        public float jumpClimbVerticalMultiplier = 0.8f;
        public float jumpClimbVerticalMaxVelocityRatio = 20f;
        public float jumpClimbHorizontalMultiplier = 1f;
        public float jumpMaxDuration = 0.6f;

        [Header("Turn")]
        public float turnSmoothDirection;
        public float turnSnapDirection;
        public float turnSmoothSnapDirection;
        public Vector3 moveDirection;
        public ForceMode verticalForceMode = ForceMode.Force;

        [Header("Colliders")]
        public float colliderRadius = 0.3f;
        public float colliderShrinkMinRadius = 0.05f;
        public float colliderGrowDuration = 2f;
        public float colliderHeight = 1f;

        [Header("Ground detection")]
        public float groundDetectionDistance = 0.05f;
        public PhysicMaterial colliderGroundMaterial;
        public float groundDrag = 3f;
        protected float groundDragMultiplier = 1f;
        public PhysicMaterial colliderFlyMaterial;
        public float flyDrag = 1f;
        protected float flyDragMultiplier = 1f;

        [NonSerialized]
        public Rigidbody rb;

        [NonSerialized]
        public CapsuleCollider capsuleCollider;

        protected float orgMass;
        protected float orgDrag;
        protected float orgAngularDrag;
        protected float orgSleepThreshold;
        public float customGravity;

        public enum GroundDetection
        {
            Raycast,
            Collision,
        }

        [NonSerialized]
        public Vector3 prevVelocity;
        [NonSerialized]
        public Vector3 prevPosition;
        [NonSerialized]
        public Quaternion prevRotation;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public Vector3 velocity;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public float horizontalSpeed;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public float verticalSpeed;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public float angularSpeed;

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public bool isCrouched;
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public bool isGrounded;
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public RaycastHit groundHit;

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public float groundAngle;

        public bool locomotionError { get; private set; } = false;


        private Vector3 accelerationCurrentSpeed;

    }
}

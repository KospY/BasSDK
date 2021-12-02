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
    [AddComponentMenu("ThunderRoad/Locomotion")]
    [RequireComponent(typeof(Rigidbody))]
    public class Locomotion : MonoBehaviour
    {
        [Header("Ground movement")]
        public bool allowMove = true;
        public bool allowTurn = true;
        public bool allowJump = true;

        public AnimationCurve moveForceMultiplierByAngleCurve;
        public bool testMove;

        public float speed = 0.13f;
        public float speedMultiplier = 1;
        public float backwardspeedMultiplier = 0.5f;
        public float strafeSpeedMultiplier = 0.6f;
        public float runSpeedMultiplier = 1.5f;

        public float forwardAngle = 10;
        public float backwardAngle = 10;
        public float runAngle = 30;
        public bool runEnabled = true;

        public float crouchSpeedRatio = 0.5f;
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
        public float groundDrag = 3;
        public PhysicMaterial colliderFlyMaterial;
        public float flyDrag = 0.5f;

        [NonSerialized]
        public Rigidbody rb;

        [NonSerialized]
        public CapsuleCollider capsuleCollider;

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


        private Vector3 accelerationCurrentSpeed;

    }
}

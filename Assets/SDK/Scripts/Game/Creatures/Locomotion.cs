using UnityEngine;
using System;

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
        public bool isEnabled = true;

        public float speed = 4;
        public float speedMultiplier = 1;
        public float backwardspeedMultiplier = 0.4f;
        public float strafeSpeedMultiplier = 0.3f;
        public float runSpeedMultiplier = 2f;

        public float forwardAngle = 10;
        public float backwardAngle = 10;
        public float runAngle = 30;
        public bool runEnabled = true;

        public float crouchSpeed = 1f;
        public float crouchHeightRatio = 0.8f;

        public float turnSpeed = 1;
        private float snapTurnTime;
        private bool smoothSnapTurn;

        [Header("Jump / Fall")]
        public AudioClip jumpAudio;
        public float airSpeed = 2;
        public ForceMode airForceMode = ForceMode.Force;
        public float jumpGroundForce = 0.3f;
        public float jumpClimbMultiplier = 0.8f;
        public float jumpMaxDuration = 0.6f;

        [Header("Turn")]
        public float turnSmoothDirection;
        public float turnSnapDirection;
        public float turnSmoothSnapDirection;
        public Vector3 moveDirection;
        public ForceMode verticalForceMode = ForceMode.Force;

        [Header("Colliders")]
        public float colliderRadius = 0.2f;
        public PhysicMaterial colliderGroundMaterial;
        public PhysicMaterial colliderFlyMaterial;

        [Header("Ground detection")]
        public LayerMask groundLayer = 1 << 0;
        public static GroundDetection groundDetection = GroundDetection.Collision;
        public float groundRaycastOffset = 0.001f;
        public float groundMaxAngle = 45f;

        [Header("Ground distance check")]
        public float raycastLenght = 5f;
        public float raycastOffset = 0.01f;

        public enum GroundDetection
        {
            Raycast,
            Collision,
        }

    }
}

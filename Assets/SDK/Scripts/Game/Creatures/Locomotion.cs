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

        public bool testMove;

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
        public float colliderHeight = 1f;
        public PhysicMaterial colliderGroundMaterial;
        public PhysicMaterial colliderFlyMaterial;

        [Header("Ground detection")]
        public static GroundDetection groundDetection = GroundDetection.Collision;
        public float groundRaycastOffset = 0.001f;
        public float groundMaxAngle = 45f;

        [Header("Ground distance check")]
        public float raycastLenght = 5f;
        public float raycastOffset = 0.01f;

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
        public Vector3 velocity;
        [NonSerialized]
        public float horizontalSpeed;
        [NonSerialized]
        public float verticalSpeed;
        [NonSerialized]
        public float angularSpeed;
        [NonSerialized]
        public bool isGrounded;

        private Vector3 accelerationCurrentSpeed;


        protected void Awake()
        {
            rb = this.GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            capsuleCollider = this.gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;
            capsuleCollider.center = new Vector3(0, colliderHeight / 2, 0);
            capsuleCollider.material = colliderGroundMaterial;

            isGrounded = true;
            this.gameObject.layer = LayerMask.NameToLayer(LayerName.BodyLocomotion.ToString());

        }

        protected void Update()
        {

            if (testMove)
            {
                moveDirection = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0) * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
                float axisTurn = Input.GetAxis("Turn");
                if (axisTurn > 0.1f || axisTurn < -0.1f) this.transform.RotateAround(this.transform.position, Vector3.up, axisTurn * turnSpeed * 3);
            }

            // Calculate velocity
            if (rb.isKinematic)
            {
                if (Time.deltaTime > 0) velocity = (this.transform.position - prevPosition) / Time.deltaTime;
                horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;
                verticalSpeed = velocity.y;
                prevPosition = this.transform.position;
            }
            else
            {
                velocity = prevVelocity;
                horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;
                verticalSpeed = velocity.y;
                prevVelocity = rb.velocity;
            }

            // Calculate angular velocity
            if (Time.deltaTime > 0)
            {
                Quaternion deltaRot = this.transform.rotation * Quaternion.Inverse(prevRotation);
                angularSpeed = Mathf.DeltaAngle(0, deltaRot.eulerAngles.y) / Time.deltaTime;
            }
            prevRotation = this.transform.rotation;
        }

        public void SetCapsuleCollider(float height)
        {
            capsuleCollider.height = height;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, (capsuleCollider.height / 2), capsuleCollider.center.z);
        }

        public void MoveWeighted(Vector3 direction, Transform bodyTransform, float heightRatio, float runSpeedRatio = 0, float acceleration = 0)
        {
            if (!isEnabled) return;
            if (isGrounded)
            {
                float bodyAngle = Vector3.Angle(new Vector3(bodyTransform.forward.x, 0, bodyTransform.forward.z), direction);
                float strafingRatio = bodyAngle < 90 ? Mathf.Lerp(1, strafeSpeedMultiplier, Mathf.InverseLerp(forwardAngle, 90, bodyAngle)) : Mathf.Lerp(strafeSpeedMultiplier, 1, Mathf.InverseLerp(90, 180 - forwardAngle, bodyAngle));
                float backwardRatio = Mathf.Lerp(1, backwardspeedMultiplier, Mathf.InverseLerp(90, 180 - backwardAngle, bodyAngle));
                float runRatio = runEnabled ? Mathf.Clamp(bodyAngle < runAngle ? runSpeedMultiplier * runSpeedRatio : 1, 1, Mathf.Infinity) : 1;
                float speed;
                if (heightRatio > crouchHeightRatio)
                {
                    // Standing
                    speed = this.speed * backwardRatio * strafingRatio * runRatio * speedMultiplier;
                }
                else
                {
                    // Crouched
                    speed = crouchSpeed;
                }
                moveDirection.x = direction.x * speed;
                moveDirection.z = direction.z * speed;
            }
            else
            {
                moveDirection.x = direction.x * airSpeed;
                moveDirection.z = direction.z * airSpeed;
            }
            if (acceleration > 0)
            {
                moveDirection = Vector3.SmoothDamp(velocity, moveDirection, ref accelerationCurrentSpeed, acceleration);
            }
        }

        public void MoveStop()
        {
            moveDirection = Vector3.zero;
        }

        protected virtual void FixedUpdate()
        {
            if (!isEnabled) return;
            // Move
            if (!float.IsNaN(moveDirection.x) && !float.IsNaN(moveDirection.z))
            {
                if (moveDirection.x != 0 || moveDirection.z != 0)
                {
                    // Horizontal move
                    if (isGrounded)
                    {
                        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
                    }
                    else
                    {
                        rb.AddForce(new Vector3(moveDirection.x, 0, moveDirection.z), airForceMode);
                    }
                }
                if (moveDirection.y != 0)
                {
                    // Vertical move
                    rb.AddForce(new Vector3(0, moveDirection.y, 0), verticalForceMode);
                }
            }
        }

    }
}

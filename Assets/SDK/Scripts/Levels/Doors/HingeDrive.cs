using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/HingeDrive")]
    public class HingeDrive : ThunderBehaviour
    {
        #region EnumsAndStructs

        // Rotation axis of the hinge
        public enum RotationAxis
        {
            XAxis = 0,
            Yaxis = 1,
            ZAxis = 2
        }

        // State of the hinge, currently thought for doors
        public enum HingeDriveState
        {
            //KeyLocked, // locked with a key       → can't open it at all without the key
            LatchLocked, // locked with the latch → can open it, but requires grabbing the handle
            Unlocked, // neither latched or keyLock
            //Broken, // All hinges are broken
        }

        // Easy and non formal way of knowing how fast is the hinge currently moving since angular velocity
        // is not natural to use
        public enum HingeDriveSpeedState
        {
            NotMoving,
            Slow,
            Fast,
            ReallyFast,
        }

        // Structure used to add more infos to simple hinge joints
        private struct HingeMetaData
        {
            public HingeJoint joint;
            /* --- Breakable hinges code if needed later
            public float health;
            public bool breakable; // Unused for now
            public Collider collider;
            */

            public HingeMetaData(HingeJoint joint /*, float health, bool breakable, Collider collider*/)
            {
                this.joint = joint;
                /* - Breakable hinges code if needed later
                this.health = health;
                this.breakable = breakable;
                this.collider = collider;*/
            }
        }

        // Store inputs as a flag to allow for multiple selection
        [Flags]
        public enum InputType
        {
            None = 0,
            Pinch = 1 << 1,
            Use = 1 << 2,
            AlternateUse = 1 << 3,
            Cast = 1 << 4,
        }

        [Serializable]
        public struct AngleThreshold
        {
            public float Min => center + minOffset;
            public float Max => center + maxOffset;

            public float minOffset;
            public float center;
            public float maxOffset;

            public AngleThreshold(float center, float minOffset, float maxOffset)
            {
                this.minOffset = minOffset;
                this.maxOffset = maxOffset;
                this.center = center;
            }
        }

        #endregion

        #region constantAndAccessors

        private const float UnlockCoolDown = .25f; // When unlocking, prevent the hinge from re-locking immediately
        private const float HingeHitThresholdCooldown = .25f; // Prevents the hinge from SPAMMING events
        private const float GrabSweepTestCooldown = .07f; // Prevents the hinge latch lock teleporting issue


        #endregion

        #region Fields

        [Header("World references")] public Transform hingesHolder;
        public Rigidbody frame; // Transform to link the hinges joints to
        public Transform[] hingesTargets; // Transforms used as targets for defining the hinges anchors in scene
        public Collider[] collidersToIgnore; // Colliders to ignore collision with (for both frame & hinge holder)

        public Handle[] handles; // Item handle to retrieve inputs when grabbed
        //public Collider[] colliders; // The hinge's colliders that should ignore feet 

        // Axis of rotation of the hinge
        [Header("Angles config values")] public RotationAxis rotationAxis = RotationAxis.Yaxis;
        public bool isLimited = true; // If disabled, hinges will spin around a pole
        public float defaultAngle; // Angle at which the hinge will put itself to on start
        public float minAngle = -90f; // Minimum angle where the hinge open to

        public float
            maxAngle = 90f; // Maximum angle the hinge can open to, closed hinge is most of the time at angle = 0

        [Header("Misc config values")] public bool enableCollisionWithFrame;

        [Header("Latch config values")]
        public bool useLatch = true; // The hinge will soft lock (latch) when reaching its resting angle

        public InputType allowedInputsToOpenLatch = InputType.Use; // Inputs that can unlock latch
        public AngleThreshold[] latchAngles; // Angles at which latch will lock
        public bool isLatchBruteForceable = true; // Can the latch be brute forced open ?
        public float forceLatchOpeningImpulseThreshold = 1f; // the hinge will open if hit at this impulse or greater
        public bool isLatchBreakable = true; // Is the latch brute force breakable ?
        public float latchHealth = 100; // Latch's health bar, currently damaged bu raw physic "impulses"

        [Header("Motor config values")] public bool useMotor;
        public float motorTargetVelocity;
        public float motorForce;

        [Header("Auto close config values")]
        [Range(0, 1f)]
        public float angleLimitsBounciness = .2f; // Make the hinge bounce (or not) when it's opened fully

        public float damper = 5; // Prevents the hinge from moving very slow for a long time
        public bool autoCloses; // Allows the hinge to get back to its resting position, like a saloon swinging hinge
        public float restingAngle; // Angle at which the hinge will be pulled toward by the spring
        public float autoCloseSpring = 10; // spring force to pull the hinge with

        /* Breakable hinges code if needed later
        [Header("Interactions config values")] public bool breakableHinges = true; // should the hinges be breakable ?
        public float breakableHingeColliderRadius = .2f; // Radius of the hinges colliders to sense hits
        public float breakableHingeHealth = 100f; // Health value of the hinges, defined by weapon type & swing speed
        */

        [Header("Haptic config values")]
        public bool enableContinuousHapticOnMove; // Should the grabbed handle vibrate on move?

        public bool useSpeedFactor = true; // Should the speed affect the vibration on move?
        public AnimationCurve speedFactor; // Curve that defines how the vibration is affected by the rotation speed
        public float continuousHapticAmplitude = .1f; // Intensity of the continuous vibration
        public bool enableHapticAngleBump; // Should the grabbed handle vibrate each time it reaches a threshold?
        public float angleStepForBumps = 10; // Angle step at which the handle will vibrate (in degrees)
        public float angleStepHapticAmplitude = 5f; // Intensity of the bumps vibration

        [Header("Audio config values")] public int smoothingSamples = 60;

        public FxModule effectAudioHingeMovingPositive; // Hinge is moving
        public FxModule effectAudioHingeMovingNegative; // Hinge is moving
        public FxModule effectAudioSlam; // Fast closing the hinge
        public FxModule effectAudioLatchLock; // Latch is closing
        public FxModule effectAudioLatchUnlock; // Latch is opening
        public FxModule effectAudioLatchBreak; // Latch is breaking
        public FxModule effectAudioWiggle; // Hinge hit min or max angle
        public FxModule effectAudioLatchButtonPress; // Player pressed latch button
        public FxModule effectAudioLatchButtonRelease; // Player released latch button

        [Header("Auto open config values")]
        public float autoOpenFallBackVelocity = 45f; // Calling any auto method without a given velocity will use this

        public float autoOpenFallBackForce = 9999f; // Calling any auto method without a given force will use this
        public bool autoOpenBypassLatch = true; // Calling any auto method without a given force will use this

        [Header("Events")]
        // Triggered when the hinge moves. Parameters are:
        //  - angular velocity
        //  - speed state
        //  - current angle            → [currentMinAngle; currentMaxAngle]
        //  - current normalized angle → [0.0; 1.0]
        public UnityEvent<float, HingeDriveSpeedState, float, float> onHingeMove =
            new UnityEvent<float, HingeDriveSpeedState, float, float>();

        public UnityEvent<float, HingeDriveSpeedState>
            onLatchLock = new UnityEvent<float, HingeDriveSpeedState>(); // Triggered on latch locking

        public UnityEvent<float, HingeDriveSpeedState>
            onLatchUnlock = new UnityEvent<float, HingeDriveSpeedState>(); // Triggered on latch unlocking

        // Triggered on by player pressing a button
        //  - angular velocity
        //  - speed state
        //  - handle from which the player has pressed the button
        public UnityEvent<float, HingeDriveSpeedState, Handle>
            onPlayerPressingLatchButton = new UnityEvent<float, HingeDriveSpeedState, Handle>();

        public UnityEvent<float, HingeDriveSpeedState, Handle>
            onPlayerReleasingLatchButton = new UnityEvent<float, HingeDriveSpeedState, Handle>();

        public UnityEvent<float, HingeDriveSpeedState>
            onLatchBreak = new UnityEvent<float, HingeDriveSpeedState>(); // Triggered on latch breaking

        // first arg is true if the hinge hit min angle
        // second arg is the hinge velocity at the impact
        public UnityEvent<bool, float, HingeDriveSpeedState> onHingeHitThreshold =
            new UnityEvent<bool, float, HingeDriveSpeedState>();

        [Header("Gizmos")][Range(0, 3)] public float gizmosSize = 1f;
        [Range(0, 20)] public int angleSteps = 5;


        private Dictionary<HingeJoint, HingeMetaData> hinges;                                          // used to access hinges custom data


        #endregion

        // Collision handling for latch opening / breaking.
        // Called from a callback of collision event bridge.
        public void CollisionOnHingeHolderEnter(Collision collision)
        {
        }


        #region Gizmos

        private void OnDrawGizmos()
        {
            if (hinges == null || hinges.Count <= 0 && hingesTargets != null)
            {
                foreach (var hingeTarget in hingesTargets)
                    DrawWireArc(hingesHolder, hingeTarget.position - hingesHolder.position, angleSteps, gizmosSize);

                return;
            }

            foreach (var position in hinges.Select(hingePair => hingePair.Value)
                         .Select(hingeMetaData => hingeMetaData.joint.anchor))
                DrawWireArc(hingesHolder, position, angleSteps, gizmosSize);
        }

        private void DrawWireArc(Transform t, Vector3 offset, float maxSteps = 5, float size = 1f)
        {
            var axis = new Vector3(0.0f, 0.0f, 0.0f) { [(int)rotationAxis] = 1.0f };

            var matrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(t.position + offset, t.rotation, Vector3.one);

            Vector3 dir;
            var p = Vector3.zero;
            var previousPos = p;
            for (var i = 0; i <= maxSteps; i++)
            {
                Gizmos.color = new Color(i / maxSteps, .3f, 1 - i / maxSteps);
                dir = Quaternion.AngleAxis(minAngle + (maxAngle - minAngle) * (i / maxSteps), axis) *
                      (Vector3.forward / 2f) * size;
                Gizmos.DrawLine(p, p + dir);
                Gizmos.DrawLine(previousPos, p + dir);

                previousPos = p + dir;
            }

            Gizmos.color = Color.red;
            dir = Quaternion.AngleAxis(defaultAngle, axis) * Vector3.forward * size;
            Gizmos.DrawLine(p, p + dir);
            Gizmos.DrawWireSphere(p + dir, .05f * size);

            Gizmos.color = Color.grey;
            dir = Vector3.forward / 1.5f * size;
            Gizmos.DrawLine(p, p + dir);
            Gizmos.DrawWireSphere(p + dir, .025f * size);

            Gizmos.color = new Color(.1f, 1f, .7f);
            foreach (var latchAngle in latchAngles)
            {
                var min = Quaternion.AngleAxis(latchAngle.Min, axis) * Vector3.forward * .52f * size;
                Gizmos.DrawLine(p, p + min);

                var center = Quaternion.AngleAxis(latchAngle.center, axis) * Vector3.forward * .52f * size;
                Gizmos.DrawLine(p, p + center);

                var max = Quaternion.AngleAxis(latchAngle.Max, axis) * Vector3.forward * .52f * size;
                Gizmos.DrawLine(p, p + max);

                Gizmos.DrawLine(p + min, p + center);
                Gizmos.DrawLine(p + center, p + max);
                Gizmos.DrawWireSphere(p + center, .01f);
            }

            Gizmos.matrix = matrix;
        }

        #endregion

    }
}
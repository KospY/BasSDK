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

        [Header("World references")]
        [Tooltip("This transform is the holder for the Hinge Joint when generated.")]
        public Transform hingesHolder;
        [Tooltip("Transform that the hinge joint links to.")]
        public Rigidbody frame; // Transform to link the hinges joints to
        [Tooltip("References transforms are used as targets for defining the hinge anchors.")]
        public Transform[] hingesTargets; // Transforms used as targets for defining the hinges anchors in scene
        [Tooltip("This object will ignore collision with the referenced colliders")]
        public Collider[] collidersToIgnore; // Colliders to ignore collision with (for both frame & hinge holder)

        [Tooltip("Referenced Handles are used to retrieve inputs from, such as Latches.")]
        public Handle[] handles; // Item handle to retrieve inputs when grabbed
        //public Collider[] colliders; // The hinge's colliders that should ignore feet 

        // Axis of rotation of the hinge
        [Header("Angles config values")]
        [Tooltip("Axis of which the hinge joint rotates in. Gives an accurate gizmo of what axis the hinge rotates.")]
        public RotationAxis rotationAxis = RotationAxis.Yaxis;
        [Tooltip("When disabled, object ignores the min/max axis, and be able to move the full 360 degree angles.")]
        public bool isLimited = true; // If disabled, hinges will spin around a pole
        [Tooltip("The angle of which the hinge will start with when spawned/activated.")]
        public float defaultAngle; // Angle at which the hinge will put itself to on start
        [Tooltip("The minimum angle of which the hinge will open to")]
        public float minAngle = -90f; // Minimum angle where the hinge open to
        [Tooltip("The maximum angle of which the hinge will open to")]
        public float maxAngle = 90f; // Maximum angle the hinge can open to, closed hinge is most of the time at angle = 0

        [Header("Misc config values")]
        [Tooltip("When enabled, the Hinge object will collide with the Frame")]
        public bool enableCollisionWithFrame;

        [Header("Latch config values")]
        [Tooltip("When enabled, the \"Latch\" will be enabled, allowing the hinge to be locked when reaching its resting angle.")]
        public bool useLatch = true; // The hinge will soft lock (latch) when reaching its resting angle

        [Tooltip("The inputs of which can open the latch. This uses the players input, such as \"Cast\", which can open the latch via a spell cast (like Gravity Push)")]
        public InputType allowedInputsToOpenLatch = InputType.Use; // Inputs that can unlock latch
        [Tooltip("The specific angles of which the latch will lock at.")]
        public AngleThreshold[] latchAngles; // Angles at which latch will lock
        [Tooltip("When enabled, Latch can be brute forced, forcing it to open (e.g. when \"slammed\" or pushed)")]
        public bool isLatchBruteForceable = true; // Can the latch be brute forced open ?
        [Tooltip("This threshold determines that the hinge WILL Open if hit at this impulse, or greater than.")]
        public float forceLatchOpeningImpulseThreshold = 1f; // the hinge will open if hit at this impulse or greater
        [Tooltip("When enabled, allows the hinge to be broken")]
        public bool isLatchBreakable = true; // Is the latch brute force breakable ?
        [Tooltip("The health of the hatch of which will be broken if enabled to.")]
        public float latchHealth = 100; // Latch's health bar, currently damaged bu raw physic "impulses"

        [Header("Motor config values")]
        [Tooltip("When enabled, Motor becomes active. The Motor rotates the Hinge in perpetual motion. If the hinge is limited, will automatically rotate from Min to Max. When not limited, it will rotate in perpetual motion, like a wheel.")]
        public bool useMotor;
        [Tooltip("Determines the target velocity of which the motor rotates the hinge.")]
        public float motorTargetVelocity;
        [Tooltip("Determines the force of which the motor rotates the hinge.")]
        public float motorForce;

        [Header("Auto close config values")]
        [Range(0, 1f)]
        [Tooltip("Makes the hinge bounce (or not) when it is opened fully.")]
        public float angleLimitsBounciness = .2f; // Make the hinge bounce (or not) when it's opened fully

        [Tooltip("This damper prevents the hinge from moving very slow for a long time.")]
        public float damper = 5; // Prevents the hinge from moving very slow for a long time
        [Tooltip("Allows the hinge to return to its resting angle.")]
        public bool autoCloses; // Allows the hinge to get back to its resting position, like a saloon swinging hinge
        [Tooltip("Angle of which the hinge will be pulled towards, by a spring")]
        public float restingAngle; // Angle at which the hinge will be pulled toward by the spring
        [Tooltip("Force of the spring that pulls the hinge to the resting position")]
        public float autoCloseSpring = 10; // spring force to pull the hinge with

        /* Breakable hinges code if needed later
        [Header("Interactions config values")] public bool breakableHinges = true; // should the hinges be breakable ?
        public float breakableHingeColliderRadius = .2f; // Radius of the hinges colliders to sense hits
        public float breakableHingeHealth = 100f; // Health value of the hinges, defined by weapon type & swing speed
        */

        [Header("Haptic config values")]
        [Tooltip("When enabled, when grabbed, the hinge handle will vibrate player's controllers when the hinge moves/rotates.")]
        public bool enableContinuousHapticOnMove; // Should the grabbed handle vibrate on move?
        [Tooltip("When enabled, the speed affects the vibration when hinge moves.")]
        public bool useSpeedFactor = true; // Should the speed affect the vibration on move?
        [Tooltip("Curve of which determines the intensity of the vibration on speed factor.")]
        public AnimationCurve speedFactor; // Curve that defines how the vibration is affected by the rotation speed
        [Tooltip("Determines the intensity of the continuous vibration.")]
        public float continuousHapticAmplitude = .1f; // Intensity of the continuous vibration
        [Tooltip("When enabled, the grabbed handle will vibrate each time it reaches a threshold/desired angle.")]
        public bool enableHapticAngleBump; // Should the grabbed handle vibrate each time it reaches a threshold?
        [Tooltip("Angle step at which the handle will vibrate (in degrees)")]
        public float angleStepForBumps = 10; // Angle step at which the handle will vibrate (in degrees)
        [Tooltip("Intensity of the bump vibration.")]
        public float angleStepHapticAmplitude = 5f; // Intensity of the bumps vibration

        [Header("Audio config values")]
        [Tooltip("Determines how smooth the looping sound effects are. The higher the number, the smoother the sound effect.")]
        public int smoothingSamples = 60;

        [Tooltip("FX Plays when Hinge is Moving from the Minimum to Maximum angle")]
        public FxModule effectAudioHingeMovingPositive; // Hinge is moving
        [Tooltip("FX Plays when Hinge is Moving from the Maximum to Minimum angle")]
        public FxModule effectAudioHingeMovingNegative; // Hinge is moving
        [Tooltip("FX Plays when hinge is closed quickly (slam).")]
        public FxModule effectAudioSlam; // Fast closing the hinge
        [Tooltip("FX plays when Latch is Closed.")]
        public FxModule effectAudioLatchLock; // Latch is closing
        [Tooltip("FX plays when Latch is Opened.")]
        public FxModule effectAudioLatchUnlock; // Latch is opening
        [Tooltip("FX plays when Latch is Broken.")]
        public FxModule effectAudioLatchBreak; // Latch is breaking
        [Tooltip("FX plays when hinge hits its minimum/maximum angle.")]
        public FxModule effectAudioWiggle; // Hinge hit min or max angle
        [Tooltip("FX plays when player presses the latch button.")]
        public FxModule effectAudioLatchButtonPress; // Player pressed latch button
        [Tooltip("FX plays when player releases the latch button.")]
        public FxModule effectAudioLatchButtonRelease; // Player released latch button

        [Header("Auto open config values")]
        [Tooltip("Calling any automatic method without a given velocity will fall back to this.")]
        public float autoOpenFallBackVelocity = 45f; // Calling any auto method without a given velocity will use this

        [Tooltip("Calling any automatic method without a given force will fall back to this.")]
        public float autoOpenFallBackForce = 9999f; // Calling any auto method without a given force will use this
        [Tooltip("Calling any automatic method without a given force will bypass the latch to open.")]
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

        [Header("Gizmos")][Range(0, 3)]
        [Tooltip("Determines the size of the gizmo")]
        public float gizmosSize = 1f;
        [Range(0, 20)]
        [Tooltip("Determines how many steps are in the gizmo.")]
        public int angleSteps = 5;


        #endregion

        // Collision handling for latch opening / breaking.
        // Called from a callback of collision event bridge.
        public void CollisionOnHingeHolderEnter(Collision collision)
        {
        }

        public void TryBruteForceLatch() // Unlocks the hinge latch if the latch is brute-forceable
        {
        }

        public void ReleaseLatch() // Unlocks the hinge with the latch
        {
        }

        /// <summary>
        /// Activate a motor on the joint to close it with default parameters specified in editor.
        /// </summary>
        public void AutoClose()
        {
        }

        /// <summary>
        /// Activate a motor on the joint to open it to minimal angle with default parameters specified in editor.
        /// </summary>
        public void AutoOpenMin()
        {
        }

        /// <summary>
        /// Activate a motor on the joint to open it to maximal angle with default parameters specified in editor.
        /// </summary>
        public void AutoOpenMax()
        {
        }

        /// <summary>
        /// Activate a motor on the joint to open it to some desired angle in degrees
        /// with default parameters specified in editor.
        /// </summary>
        /// <param name="targetAngle">Desired angle for the door to open to in degrees.</param>
        public void AutoRotateTo(float targetAngle)
        {
        }

        private void StopAutoRotate()
        {
        }

        /// <summary>
        /// Prevent opening this hinge drive
        /// </summary>
        public void PreventOpening()
        {
        }

        /// <summary>
        /// Allow opening this hinge drive with restored previous allowed inputs
        /// </summary>
        public void AllowOpening()
        {
        }

    }
}
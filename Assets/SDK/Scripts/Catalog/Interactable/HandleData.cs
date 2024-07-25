using System;
using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class HandleData : InteractableData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Handle")]
#endif
        public bool disableOnStart = false;
#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool rotateAroundAxis = false;

#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool forceHoldGripToGrab = false;
#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool forcePlayerPhysicLink = false;

#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool disableHandCollider = false;

#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool alwaysTeleportToHand = false;
#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool disablePinchGrab = false;
#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool setCenterOfMassTohandle = false;
#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public float handlerLocomotionSpeedMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool forceAllowTwoHanded;
#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool allowSlidingWithBothHand;

#if ODIN_INSPECTOR
        [BoxGroup("Handle/Magic")] 
#endif
        public bool allowSpellMenu = false;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Magic")] 
#endif
        public bool allowSpellFire = false;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Magic"), ShowIf("allowSpellFire")] 
#endif
        public Vector3 spellFireMagicOffset;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Magic"), ShowIf("allowSpellFire")] 
#endif
        public bool offsetInHandleSpace;

#if ODIN_INSPECTOR
        [BoxGroup("Handle/UI")] 
#endif
        public bool warnIfNotAllowed = false;

#if ODIN_INSPECTOR
        [BoxGroup("Handle/Audio"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string grabEffectId;
        [NonSerialized]
        public EffectData grabEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Audio")] 
#endif
        public float slideFxMinVelocity = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Audio")] 
#endif
        public float slideFxMaxVelocity = 10;

#if ODIN_INSPECTOR
        [BoxGroup("Handle/Forces")] 
#endif
        public RotationDrive rotationDrive = RotationDrive.Slerp;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Forces")] 
#endif
        public bool useWYZWhenTwoHanded = true;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Forces")] 
#endif
        public float positionSpringMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Forces")] 
#endif
        public float positionDamperMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Forces")] 
#endif
        public float rotationSpringMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Forces")] 
#endif
        public float rotationDamperMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Forces")] 
#endif
        public float rotationSpring2hMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Forces")] 
#endif
        public float rotationDamper2hMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Forces")] 
#endif
        public bool forceClimbing = false;

        public enum RotationDrive
        {
            Slerp,
            X,
            YZ,
            WYZ,
        }

        public static Side dominantHand = Side.Right;
        public static TwoHandedMode twoHandedMode = TwoHandedMode.AutoFront;

#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool dominantWhenTwoHanded = true;
#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool ignoreSnap = false;
#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool disabledOnSnap = false;
#if ODIN_INSPECTOR
        [BoxGroup("Handle")] 
#endif
        public bool useDefaultGripForHolder = true;

#if ODIN_INSPECTOR
        [BoxGroup("Handle/Allowance")] 
#endif
        public bool allowAutoGrab = true;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Allowance")] 
#endif
        public bool allowSteal = true;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Allowance")] 
#endif
        public bool allowSwap = true;

#if ODIN_INSPECTOR
        [BoxGroup("Handle/Telekinesis")] 
#endif
        public bool allowTelekinesis;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Telekinesis")] 
#endif
        public float telekinesisPullRepelMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Telekinesis")] 
#endif
        public float telekinesisMaxDistanceMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Telekinesis")] 
#endif
        public float telekinesisPositionSpringMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Telekinesis")] 
#endif
        public float telekinesisPositionDamperMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Telekinesis")] 
#endif
        public float telekinesisPositionMaxForceMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Telekinesis")] 
#endif
        public float telekinesisRotationSpringMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Telekinesis")] 
#endif
        public float telekinesisRotationDamperMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Telekinesis")] 
#endif
        public float telekinesisRotationMaxForceMultiplier = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Handle/Slide")] 
#endif
        public float slideMinDamper = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Slide")] 
#endif
        public float slideMaxDamper = 300;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Slide")] 
#endif
        public float slideMotorMaxForce = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Slide")] 
#endif
        public float slideMotorAcceleration = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Handle/Slide")] 
#endif
        public SlideMotorDirection slideMotorDir = SlideMotorDirection.Gravity;

        public static string tagTkDefault = "TkDefault";
        public static string tagTkRagdoll = "TkRagdoll";
        public static string tagTkDefaultLeft = "TkDefaultLeft";
        public static string tagTkDefaultRight = "TkDefaultRight";
        public static string tagTkRagdollLeft = "TkRagdollLeft";
        public static string tagTkRagdollRight = "TkRagdollRight";

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

        public enum SlideMotorDirection
        {
            Gravity,
            Up,
            Down,
        }

        public enum AttachType
        {
            Parent,
            Joint,
        }

        public enum TwoHandedMode
        {
            Position,
            Dominant,
            AutoFront,
            AutoRear,
        }

    }
}

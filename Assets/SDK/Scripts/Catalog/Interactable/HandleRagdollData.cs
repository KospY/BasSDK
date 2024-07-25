using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class HandleRagdollData : HandleData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll")] 
#endif
        public Behaviour behaviour = Behaviour.None;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll")] 
#endif
        public float scaleDuringCombat = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll")]
#endif
        public TkCondition tkActiveCondition = TkCondition.Always;

        public enum Behaviour
        {
            None,
            Carry,
            Muffle,
            Choke,
            ChokeAndMuffle
        }

        public enum TkCondition
        {
            Never,
            CreatureDead,
            PartSliced,
            Always,
        }

#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll")] 
#endif
        public List<RagdollPart.Type> activateCollisionOnParts;

#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/IK")] 
#endif
        public bool useIK;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/IK")] 
#endif
        public bool allowRotationIK;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/IK")] 
#endif
        public float IkPositionWeight = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/IK")] 
#endif
        public float IkRotationWeight = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/IK")] 
#endif
        public bool changeHeight;

#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Lift")] 
#endif
        public LiftBehaviour liftBehaviour = LiftBehaviour.None;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Lift")] 
#endif
        public Part liftPartReference = Part.Root;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Lift")] 
#endif
        public ForceLiftCondition forceLiftGrabCondition;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Lift")] 
#endif
        public ForceLiftCondition forceLiftTKCondition;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Lift")] 
#endif
        public float liftOffset = 0.05f;
#if ODIN_INSPECTOR
		[BoxGroup("Ragdoll/Lift")]
#endif
		public bool telekinesisSpinningFix = true; // Fixes the TK spin, just in case it might break stuff, easy to disable
#if ODIN_INSPECTOR
		[BoxGroup("Ragdoll/Lift")]
#endif
		public bool liftSpinningFix = true; // The fix enabling the lifting to lock hips 
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Lift")]
#endif
        public float liftSpinningFacingPower = 10f;// Hips facing slerp power

#if ODIN_INSPECTOR
		[BoxGroup("Ragdoll/Lift")]
#endif
		public float liftSpinningFacingMaxSpeed = 5f; // Maximum slerping speed of the hips
#if ODIN_INSPECTOR
		[BoxGroup("Ragdoll/Lift")]
#endif
		public float liftSpinEnableMaxSpeed = 1.5f; // At which speed does the spin fix get enabled


#if ODIN_INSPECTOR
		[BoxGroup("Handle/StabilizationJoint")]
#endif
		public bool applyStabilizationJoint = true;
#if ODIN_INSPECTOR
		[BoxGroup("Handle/StabilizationJoint")]
#endif
		public UnityEngine.Vector3 telekinesisStabilizationJointAxis;
#if ODIN_INSPECTOR
		[BoxGroup("Handle/StabilizationJoint")]
#endif
		public float stabilizationEnableMinSpeed = 5f;

#if ODIN_INSPECTOR
		[BoxGroup("Handle/StabilizationJoint")]
#endif
		public float stabilizationEnableDot = 0.7f;
#if ODIN_INSPECTOR
		[BoxGroup("Handle/StabilizationJoint")]
#endif
		public float stabilizationDisableDotMultiplier = 0.7f;
#if ODIN_INSPECTOR
		[BoxGroup("Handle/StabilizationJoint")]
#endif
		public float stabilizationPositionSpring = 2000;
#if ODIN_INSPECTOR
		[BoxGroup("Handle/StabilizationJoint")]
#endif
		public float stabilizationPositionDamper = 200;
#if ODIN_INSPECTOR
		[BoxGroup("Handle/StabilizationJoint")]
#endif
		public float stabilizationPositionMaxForce = 1500;

#if ODIN_INSPECTOR
		[BoxGroup("Handle/Telekinesis")]
#endif
		public Vector2 playerFaceForceRange = new Vector2(25f,45f);
#if ODIN_INSPECTOR
		[BoxGroup("Handle/Telekinesis")]
#endif
		public float restingAngle = 20f;

#if ODIN_INSPECTOR
		[BoxGroup("Ragdoll/Move")] 
#endif
        public bool moveStep;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Move")] 
#endif
        public Part stepPartReference = Part.Head;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Move")] 
#endif
        public float stepSpeedMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Move")] 
#endif
        public float stepThresholdMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Move")] 
#endif
        public BodyTurnDirection bodyTurnDirection = BodyTurnDirection.None;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Move"), ShowIf("bodyTurnDirection", BodyTurnDirection.ClosestCardinal)] 
#endif
        public Cardinal cardinal = Cardinal.XZ;

#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Forces")] 
#endif
        public float releaseArmSpring = 0.25f;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Forces")] 
#endif
        public float releaseArmDamper = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Char joint")] 
#endif
        public List<RagdollPart.Type> overrideCharJointLimitsOnParts;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Char joint")] 
#endif
        public float swing1Limit = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Char joint")] 
#endif
        public float lowTwistLimit = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Char joint")] 
#endif
        public float highTwistLimit = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Ragdoll/Char joint")] 
#endif
        public bool invertLowHighOnBack = true;

		[Flags]
        public enum ForceLiftCondition
        {
            IdleOrPatrol = 1,
            Combat = 2,
            Attack = 4,
            Stagger = 8,
            StaggerFull = 16,
            Electrocuted = 32,
        }

        public enum LiftBehaviour
        {
            None,
            Fall,
            Ungrab,
        }

        public enum Part
        {
            Self,
            Root,
            Target,
            Head,
        }

        public enum BodyTurnDirection
        {
            None,
            HandDirection,
            PartDirection,
            ClosestCardinal,
            GrabberPosition,
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllDamagerID()
        {
            return Catalog.GetDropdownAllID(Category.Damager);
        } 
#endif

    }
}

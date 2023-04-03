using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace ThunderRoad
{
	public class BrainModuleDefense : BrainData.Module
    {
        public bool enabled = true;

        public bool parryEnabled = true;
        public float aboutToBeHitTiming = 0.1f;
        public float dangerLevelDecreaseTime = 0.5f;
        public float shieldRiposteChance = 0.3f;

        [Header("Swing defense")]
        public float defenseMaxDistance = 8f;
        public float parryDetectionRadius = 5f;
        public float parryHorizontalMaxAngle = 60;
        public float parryCircleHeight = 0.64f;
        public float parryCircleRadius = 0.66f;
        public float parryAdjustSpeed = 4f;
        public float riposteBlockRequirement = 0.5f;

        [Header("Thrust defense")]
        public float thrustSensitivityVelocity = 0.8f;
        public float thrustSensitivityDirection = 0.8f;
        public float minimumThrustVelocity = -1f;
        public float lookAtIncomingMaxAngle = 45f;
        public float deflectAdjustSpeed = 10f;
        public float deflectTurnSpeedRatio = 2f;
        public float deflectForce = 100f;
        public float deflectLingerForce = 10f;
        public AnimationCurve deflectLingerForceCurve;
        public List<StanceDefenseSettings> stanceSettings = new List<StanceDefenseSettings>();

        public struct StanceDefenseSettings
        {
            public BrainModuleStance.Stance stance;
            public Vector2 minMaxIncomingAttackSpeed;
            public float defenseColliderRotation;
            public ParryPoseMode parryPoseMode;
            public bool extentHorizontalValuesOnTop;
            public bool extentHorizontalValuesOnBottom;
            public bool deflectEnabled;
            public Vector3 preparationStance;
            public float deflectMaxAngle;
            public float deflectAngleOffset;
            public float deflectTurnOffset;
            public float maxDeflectHeight;
            public float deflectHeightOffset;
            public float deflectDistance;
        }

        public enum ParryPoseMode
        {
            NormalizedDirection,
            NormalizedPosition,
        }

        [Header("Dodge")]
        public bool dodgeEnabled = true;
        public AnimationCurve dodgeChanceCurve;
        public float dodgeSpeed = 1f;
        public float dodgeMaxHeight = 0.2f;
        public bool dodgeWhenGrabbed = false;
        public bool dodgeWhenWeaponGrabbed = false;
        public bool sideDodgeEnabled = false;

        [Header("Misc")]
        public float physicsCullParryBias = 10f;
        public float turnSpeedRatio = 1f;
        public float heldDefenseMinRadius = 0.5f;
        public float heldDefenseMaxRadius = 2f;
        public float longDefenseMinRadius = 1f;
        public float longDefenseMaxRadius = 5f;

        [Header("IK")]
        public bool shieldUsesIK = true;
        public float shieldMaxIKWeightPos = 0.2f;
        public float shieldMaxIKWeightRot = 0.8f;
        public float shieldPassiveIKWeightPos = 0f;
        public float shieldPassiveIKWeightRot = 0.5f;
        public bool oneHandUsesIK = true;
        public float oneHandMaxIKWeightPos = 0.1f;
        public float oneHandMaxIKWeightRot = 0.6f;
        public float positionSpeed = 1.7f;
        public float rotationSpeed = 300;
        public float dynamicHeightOffset = 0.1f;
        public float armSpringMultiplier = 1.0f;
        public float armMaxForceMultiplier = 10.0f;

    }
}
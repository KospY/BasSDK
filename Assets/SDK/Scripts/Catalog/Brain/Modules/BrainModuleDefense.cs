using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModuleDefense : BrainData.Module
    {
        public bool enabled = true;

        public bool parryEnabled = true;
#if ODIN_INSPECTOR
        protected void CopyTime()
        {
            foreach (BrainData bd in Catalog.GetDataList<BrainData>())
            {
                if (bd.GetModule<BrainModuleDefense>(false) is BrainModuleDefense defenseModule) defenseModule.aboutToBeHitTiming = this.aboutToBeHitTiming;
            }
        }

        [CustomContextMenu("Copy to other brains", nameof(CopyTime))]
#endif
        public float aboutToBeHitTiming = 0.5f;
        public float dangerLevelDecreaseTime = 0.5f;
        public float shieldRiposteChance = 0.3f;
        public List<StanceDefenseSettings> stanceSettings = new List<StanceDefenseSettings>();

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

        [System.Serializable]
        public struct StanceDefenseSettings
        {
            public BrainModuleStance.Stance stance;
            public Vector2 minMaxIncomingAttackSpeed;
            public float defenseColliderRotation;
            public ParryPoseMode parryPoseMode;
            public bool extentHorizontalValuesOnTop;
            public bool extentHorizontalValuesOnBottom;
        }

        public enum ParryPoseMode
        {
            NormalizedDirection,
            NormalizedPosition,
        }

        [Header("Dodge")]
        public bool dodgeEnabled = true;
#if ODIN_INSPECTOR
        protected void CopyCurveAcrossBrains()
        {
            foreach (BrainData bd in Catalog.GetDataList<BrainData>())
            {
                if (bd.GetModule<BrainModuleDefense>(false) is BrainModuleDefense defenseModule) defenseModule.dodgeChanceCurve = this.dodgeChanceCurve;
            }
        }

        [CustomContextMenu("Copy to other brains", nameof(CopyCurveAcrossBrains))]
#endif
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
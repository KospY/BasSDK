using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill
{
    public class SkillDiveStrike : SkillData
    {
        public delegate void DiveEvent(Creature creature, EventTime eventTime);
        public static event DiveEvent OnDive;

        [Header("General")]
#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float downwardAngle = 5f;

#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float minimumVelocity = 3f;

#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float grabWait = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float headAngleRange = 15f;

        [Header("Jab")]
#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float decelerationRatioThresholdStab = 0.1f;

#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public bool allowPommelDive = false;
        [Header("Swing")]
#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public bool swingDetectionEnabled = false;
#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float decelerationRatioThresholdSlash = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float maxSwingPointDistance = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float minAirborneTimeForPunch = 0.6f;

#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float minAirborneTimeForSlash = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float slashLookBiasAmount = 0.75f;

#if ODIN_INSPECTOR
        [BoxGroup("Dive")]
#endif
        public bool aimAssist = false;

#if ODIN_INSPECTOR
        [BoxGroup("Dive"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string effectId;

        protected EffectData effectData;
#if ODIN_INSPECTOR
        [BoxGroup("Dive")]
#endif
        public float diveSpeedScaleAimAssist = 1.4f;
#if ODIN_INSPECTOR
        [BoxGroup("Dive")]
#endif
        public float currentSpeedScale = 0.75f;
#if ODIN_INSPECTOR
        [BoxGroup("Dive")]
#endif
        public float diveSpeedScale = 2f;
#if ODIN_INSPECTOR
        [BoxGroup("Dive")]
#endif
        public float handVelocityMult = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Dive")]
#endif
        public float maxVelocity = 8f;
#if ODIN_INSPECTOR
        [BoxGroup("Dive")]
#endif
        public float damageMultiplier = 3f;
#if ODIN_INSPECTOR
        [BoxGroup("Dive")]
#endif
        public float groundEndDiveAllowance = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Misc")]
#endif
        public bool endOnHit = false;
#if ODIN_INSPECTOR
        [BoxGroup("Misc")]
#endif
        public float fallDamageScale = 0;

    }

    public enum DiveType
    {
        None,
        Hand,
        Swing,
        Jab,
        FlyDirRef
    }

}

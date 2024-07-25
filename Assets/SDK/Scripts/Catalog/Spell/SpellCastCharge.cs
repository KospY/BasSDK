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
    public class SpellCastCharge : SpellCastData
    {
        public static bool forceOverrideToLocalizedText = false;
#if ODIN_INSPECTOR
        [BoxGroup("Skill UI")]
#endif
        public string castDescription;
#if ODIN_INSPECTOR
        [BoxGroup("Skill UI")]
#endif
        public string imbueDescription;
#if ODIN_INSPECTOR
        [BoxGroup("Skill UI")]
#endif
        public string slamDescription;
        
#if ODIN_INSPECTOR
        [BoxGroup("Charge"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string chargeEffectId;

        [NonSerialized]
        public EffectData chargeEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Charge"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string readyEffectId;

        [NonSerialized]
        public EffectData readyEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Charge"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string readyMinorEffectId = "SpellFlourishMinor";
        protected EffectData readyMinorEffectData;


#if ODIN_INSPECTOR
        [BoxGroup("Charge"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string fingerEffectId;
        protected EffectData fingerEffectData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Charge"), ValueDropdown(nameof(GetAllHandPoseID))]
#endif
        public string closeHandPoseId;
        [NonSerialized]
        public HandPoseData closeHandPoseData;

#if ODIN_INSPECTOR
        [BoxGroup("Charge"), ValueDropdown(nameof(GetAllHandPoseID))]
#endif
        public string openHandPoseId;
        [NonSerialized]
        public HandPoseData openHandPoseData;

#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public bool doReadyHaptic = true;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public AnimationCurve chargeEffectCurve = AnimationCurve.Linear(0, 0, 1, 1);
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public float orbVariationSpeed = 10f;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public float orbVariationAmount = 0.2f;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public float chargeSpeed = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public bool allowStaffBuff = false;
#if ODIN_INSPECTOR
        [BoxGroup("Charge"), ShowIf("allowStaffBuff"), ShowInInspector]
#endif
        public Dictionary<Modifier, float> heldStaffModifiers;
#if ODIN_INSPECTOR
        [BoxGroup("Charge"), LabelText(@"@SkillPassiveLabel(""Charge Speed"", chargeSpeedPerSkill)")]
#endif
        public float chargeSpeedPerSkill = 0.05f;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public float grabbedFireMaxCharge = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public bool allowUnderwater;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public bool endOnGrip = true;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public bool allowCharge = true;

        public bool ShouldSerializeallowUnderwater()
        {
            return false;
        }

#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float chargeMinHaptic = 0.05f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float chargeMaxHaptic = 0.3f;

#if ODIN_INSPECTOR
        [BoxGroup("Hand force")]
#endif
        public float handSpringMultiplier = 0.8f;
#if ODIN_INSPECTOR
        [BoxGroup("Hand force")]
#endif
        public float handLocomotionVelocityCorrectionMultiplier = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public bool allowThrow = true;
#if ODIN_INSPECTOR
        [BoxGroup("Throw"), ValueDropdown(nameof(GetAllEffectID)), ShowIf("allowThrow")]
#endif
        public string throwEffectId;
        [NonSerialized]
        public EffectData throwEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Throw"), ShowIf("allowThrow")]
#endif
        public float throwMinCharge = 0.9f;

#if ODIN_INSPECTOR
        [BoxGroup("Spray")]
#endif
        public bool allowSpray;
#if ODIN_INSPECTOR
        [BoxGroup("Spray"), ValueDropdown(nameof(GetAllHandPoseID)), ShowIf("allowSpray")]
#endif
        public string sprayHandPoseId;
        [NonSerialized]
        public HandPoseData sprayHandPoseData;
#if ODIN_INSPECTOR
        [BoxGroup("Spray"), ShowIf("allowSpray")]
#endif
        public Vector3 sprayMagicOffset;
#if ODIN_INSPECTOR
        [BoxGroup("Spray"), ShowIf("allowSpray")]
#endif
        public float sprayHeadToFireMaxAngle = 45;
#if ODIN_INSPECTOR
        [BoxGroup("Spray"), ShowIf("allowSpray")]
#endif
        public float sprayStopMinCharge = 0.2f;
#if ODIN_INSPECTOR
        [BoxGroup("Spray"), ShowIf("allowSpray")]
#endif
        public float sprayStartMinCharge = 0.5f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Grip Cast"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string gripCastEffectId;
        [NonSerialized]
        public EffectData gripCastEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Grip Cast"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string gripCastStatusEffectId;
#if ODIN_INSPECTOR
        [BoxGroup("Grip Cast")]
#endif
        public float gripCastStatusDuration;
        
        [NonSerialized]
        public bool allowGripCast;

        [NonSerialized]
        protected float lastGripCastTime;
        [NonSerialized]
        public float gripCastDamageInterval;
        [NonSerialized]
        public float gripCastDamageAmount;
        
        [NonSerialized]
        public StatusData gripCastStatusEffect;

        [NonSerialized]
        public bool isGripCasting;

        [NonSerialized]
        public bool isSpraying;

        // [NonSerialized]
        // protected float lastCharge;
        
        [NonSerialized]
        public float currentCharge;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public bool imbueEnabled = true;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public bool imbueAllowMetal = false;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueRate = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueLossMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueRadius = 0.2f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public bool imbueHitUseDamager = false;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue"), HideIf("imbueHitUseDamager")]
#endif
        public float imbueHitMinVelocity = 4f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueWhooshMinSpeed = 4f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueWhooshMaxSpeed = 12f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueWhooshHapticMultiplier = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string imbueMetalEffectId;
        [NonSerialized]
        public EffectData imbueMetalEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string imbueBladeEffectId;
        [NonSerialized]
        public EffectData imbueBladeEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string imbueCrystalEffectId;
        [NonSerialized]
        public EffectData imbueCrystalEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Charge"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string imbueNaaEffectId;

        [NonSerialized]
        public EffectData imbueNaaEffectData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Imbue"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string imbueTransferEffectId;
        [NonSerialized]
        public EffectData imbueTransferEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueHitEnergySteal = 1;


#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamRechargeDelay = 2f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamMinVelocity = 4f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamConsumptionMult = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamMaxVelocity = 15f;

#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string staffSlamTipEffectId;
        [NonSerialized]
        public EffectData staffSlamTipEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string staffSlamCollisionEffectId;

        [NonSerialized]
        public EffectData staffSlamCollisionEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public bool allowEnemySelfSlam = false;



        public new SpellCastCharge Clone()
        {
            return this.MemberwiseClone() as SpellCastCharge;
        }

        public override int GetCurrentVersion()
        {
            return 0;
        }

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            heldStaffModifiers ??= new Dictionary<Modifier, float>
            {
                { Modifier.ChargeSpeed, 1.5f }
            };
            if (forceOverrideToLocalizedText)
            {
                castDescription = $"{{{id}DescriptionCast}}";
                imbueDescription = $"{{{id}DescriptionImbue}}";
                slamDescription = $"{{{id}DescriptionSlam}}";
            }
            chargeEffectData = Catalog.GetData<EffectData>(chargeEffectId);
            readyEffectData = Catalog.GetData<EffectData>(readyEffectId);
            readyMinorEffectData = Catalog.GetData<EffectData>(readyMinorEffectId);
            fingerEffectData = Catalog.GetData<EffectData>(fingerEffectId);
            gripCastEffectData = Catalog.GetData<EffectData>(gripCastEffectId);
            closeHandPoseData = Catalog.GetData<HandPoseData>(closeHandPoseId);
            openHandPoseData = Catalog.GetData<HandPoseData>(openHandPoseId);
            throwEffectData = Catalog.GetData<EffectData>(throwEffectId);
            sprayHandPoseData = Catalog.GetData<HandPoseData>(sprayHandPoseId);

            imbueMetalEffectData = Catalog.GetData<EffectData>(imbueMetalEffectId);
            imbueBladeEffectData = Catalog.GetData<EffectData>(imbueBladeEffectId);
            imbueTransferEffectData = Catalog.GetData<EffectData>(imbueTransferEffectId);
            imbueCrystalEffectData = Catalog.GetData<EffectData>(imbueCrystalEffectId);
            imbueNaaEffectData = Catalog.GetData<EffectData>(imbueNaaEffectId);

            staffSlamCollisionEffectData = Catalog.GetData<EffectData>(staffSlamCollisionEffectId);
            staffSlamTipEffectData = Catalog.GetData<EffectData>(staffSlamTipEffectId);
            gripCastStatusEffect = Catalog.GetData<StatusData>(gripCastStatusEffectId);
        }

        public void InvokeOnGripEndEvent()
        {
        }
    }

    public enum Modifier
    {
        Range,
        Duration,
        Intensity,
        Efficiency,
        Speed,
        ChargeSpeed
    }
}

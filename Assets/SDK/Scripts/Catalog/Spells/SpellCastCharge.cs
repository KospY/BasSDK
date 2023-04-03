using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class SpellCastCharge : SpellCastData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Charge"), ValueDropdown("GetAllEffectID")]
#endif
        public string chargeEffectId;
        protected EffectData chargeEffectData;


#if ODIN_INSPECTOR
        [BoxGroup("Charge"), ValueDropdown("GetAllEffectID")]
#endif
        public string fingerEffectId;
        protected EffectData fingerEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Charge"), ValueDropdown("GetAllHandPoseID")]
#endif
        public string closeHandPoseId;
        [NonSerialized]
        public HandPoseData closeHandPoseData;

#if ODIN_INSPECTOR
        [BoxGroup("Charge"), ValueDropdown("GetAllHandPoseID")]
#endif
        public string openHandPoseId;
        [NonSerialized]
        public HandPoseData openHandPoseData;

#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public float chargeSpeed = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public float stopSpeed = 0.6f;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public bool stopIfManaDepleted = false;
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
        [BoxGroup("Throw"), ValueDropdown("GetAllEffectID"), ShowIf("allowThrow")]
#endif
        public string throwEffectId;
        protected EffectData throwEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Throw"), ShowIf("allowThrow")]
#endif
        public float throwMinCharge = 0.9f;

#if ODIN_INSPECTOR
        [BoxGroup("Spray")]
#endif
        public bool allowSpray;
#if ODIN_INSPECTOR
        [BoxGroup("Spray"), ValueDropdown("GetAllHandPoseID"), ShowIf("allowSpray")]
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
        public float sprayMinCharge = 0.5f;

        [NonSerialized]
        public bool isSpraying;

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
        public float imbueManaConsumption = 0.1f;
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
        [BoxGroup("Imbue"), ValueDropdown("GetAllEffectID")]
#endif
        public string imbueMetalEffectId;
        [NonSerialized]
        public EffectData imbueMetalEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue"), ValueDropdown("GetAllEffectID")]
#endif
        public string imbueBladeEffectId;
        [NonSerialized]
        public EffectData imbueBladeEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue"), ValueDropdown("GetAllEffectID")]
#endif
        public string imbueCrystalEffectId;
        [NonSerialized]
        public EffectData imbueCrystalEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue"), ValueDropdown("GetAllEffectID")]
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
        public float staffSlamConsumption = 30f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamMaxVelocity = 15f;

#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam"), ValueDropdown("GetAllEffectID")]
#endif
        public string staffSlamTipEffectId;
        [NonSerialized]
        public EffectData staffSlamTipEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam"), ValueDropdown("GetAllEffectID")]
#endif
        public string staffSlamCollisionEffectId;

        [NonSerialized]
        public EffectData staffSlamCollisionEffectData;


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
            chargeEffectData = Catalog.GetData<EffectData>(chargeEffectId);
            fingerEffectData = Catalog.GetData<EffectData>(fingerEffectId);
            closeHandPoseData = Catalog.GetData<HandPoseData>(closeHandPoseId);
            openHandPoseData = Catalog.GetData<HandPoseData>(openHandPoseId);
            throwEffectData = Catalog.GetData<EffectData>(throwEffectId);
            sprayHandPoseData = Catalog.GetData<HandPoseData>(sprayHandPoseId);

            imbueMetalEffectData = Catalog.GetData<EffectData>(imbueMetalEffectId);
            imbueBladeEffectData = Catalog.GetData<EffectData>(imbueBladeEffectId);
            imbueTransferEffectData = Catalog.GetData<EffectData>(imbueTransferEffectId);
            imbueCrystalEffectData = Catalog.GetData<EffectData>(imbueCrystalEffectId);

            staffSlamCollisionEffectData = Catalog.GetData<EffectData>(staffSlamCollisionEffectId);
            staffSlamTipEffectData = Catalog.GetData<EffectData>(staffSlamTipEffectId);
        }

    }
}

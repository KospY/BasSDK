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
    public class SpellMergeData : SpellData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Merge"), ValueDropdown(nameof(GetAllSpellID))]
#endif
        [JsonMergeKey]
        public string leftSpellId;

#if ODIN_INSPECTOR
        [BoxGroup("Merge"), ValueDropdown(nameof(GetAllSpellID))]
#endif
        [JsonMergeKey]
        public string rightSpellId;

#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public float chargeSpeed = 0.2f;
#if ODIN_INSPECTOR
        [BoxGroup("Charge"), LabelText(@"@SkillPassiveLabel(""Charge Speed"", chargeSpeedPerSkill)")]
#endif
        public float chargeSpeedPerSkill = 0.05f;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public float chargeStartHandsRatio = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public float stopSpeed = 0.6f;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public bool stopIfManaDepleted = true;
        [NonSerialized]
        public float currentCharge;

#if ODIN_INSPECTOR
        [BoxGroup("Merge")]
#endif
        public float handEnterAngle = 30;
#if ODIN_INSPECTOR
        [BoxGroup("Merge")]
#endif
        public float handEnterDistance = 0.2f;
#if ODIN_INSPECTOR
        [BoxGroup("Merge")]
#endif
        public float handExitAngle = 90;
#if ODIN_INSPECTOR
        [BoxGroup("Merge")]
#endif
        public float handExitDistance = 0.6f;
#if ODIN_INSPECTOR
        [BoxGroup("Merge")]
#endif
        public float handCompletedDistance = 0.001f;
#if ODIN_INSPECTOR
        [BoxGroup("Merge")]
#endif
        public float minCharge = 0.9f;

#if ODIN_INSPECTOR
        [BoxGroup("Merge")]
#endif
        public AnimationCurve hapticIntensityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            // new Utils.CurveBuilder()
            // .WithKey(0, 0)
            // .WithKey(0.83f, 0.4f)
            // .WithKey(0.86f, 0)
            // .WithKey(0.89f, 0)
            // .WithKey(1, 1)
            // .Build();

#if ODIN_INSPECTOR
        [BoxGroup("Merge")]
#endif
        public float hapticCurveModifier = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Hand force")]
#endif
        public AnimationCurve handForceCurve = new AnimationCurve(new Keyframe[3] { new Keyframe(0f, 120f), new Keyframe(0.05f, 180f), new Keyframe(0.1f, 120f) });
#if ODIN_INSPECTOR
        [BoxGroup("Hand force")]
#endif
        public AnimationCurve handForceMultiplierDistanceCurve = new AnimationCurve(new Keyframe[2] { new Keyframe(0f, 1f), new Keyframe(0.2f, 0f) });
#if ODIN_INSPECTOR
        [BoxGroup("Hand force")]
#endif
        public AnimationCurve handForceChargeCurve = new AnimationCurve(new Keyframe[2] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) });
#if ODIN_INSPECTOR
        [BoxGroup("Hand force")]
#endif
        public float handPositionSpringMultiplier = 0.4f;
#if ODIN_INSPECTOR
        [BoxGroup("Hand force")]
#endif
        public float handPositionDamperMultiplier = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Hand force")]
#endif
        public float handLocomotionVelocityCorrectionMultiplier = 0.3f;

        private bool reachedMinCharge = false;

#if ODIN_INSPECTOR
        [BoxGroup("Merge"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string chargeEffectId;
        protected EffectData chargeEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Merge")]
#endif
        [Tooltip("Set to zero for no lerping")]
        public float effectLerpFactor = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Merge"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string fingerEffectId;
        protected EffectData fingerEffectData;
        
        
        [NonSerialized]
        public EffectData overrideFingerEffect;
        
        [NonSerialized]
        public EffectData overrideChargeEffect;

#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public bool allowThrow = false;
#if ODIN_INSPECTOR
        [BoxGroup("Throw"), ValueDropdown(nameof(GetAllEffectID)), ShowIf("allowThrow")]
#endif
        public string throwEffectId;
        protected EffectData throwEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float throwMinHandVelocity = 2f;

#if ODIN_INSPECTOR
        [BoxGroup("Hand pose"), ValueDropdown(nameof(GetAllHandPoseID))]
#endif
        public string closeHandPoseId;
        [NonSerialized]
        public HandPoseData closeHandPoseData;

#if ODIN_INSPECTOR
        [BoxGroup("Hand pose"), ValueDropdown(nameof(GetAllHandPoseID))]
#endif
        public string openHandPoseId;
        [NonSerialized]
        public HandPoseData openHandPoseData;
        


        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            if (chargeEffectId != null && chargeEffectId != "") chargeEffectData = Catalog.GetData<EffectData>(chargeEffectId);
            if (closeHandPoseId != null && closeHandPoseId != "") closeHandPoseData = Catalog.GetData<HandPoseData>(closeHandPoseId);
            if (openHandPoseId != null && openHandPoseId != "") openHandPoseData = Catalog.GetData<HandPoseData>(openHandPoseId);
            if (fingerEffectId != null && fingerEffectId != "") fingerEffectData = Catalog.GetData<EffectData>(fingerEffectId);
            handForceCurve.postWrapMode = WrapMode.Loop;
        }

        public override int GetCurrentVersion()
        {
            return 0;
        }
    }
}

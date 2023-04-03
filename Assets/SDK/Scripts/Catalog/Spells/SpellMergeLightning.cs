using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class SpellMergeLightning : SpellMergeData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Beam"), ValueDropdown("GetAllEffectID")]
#endif
        public string beamEffectId;
        [NonSerialized]
        protected EffectData beamEffectData;


#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public LayerMask beamMask;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamForce = 3f;

#if ODIN_INSPECTOR
        [BoxGroup("Beam"), ValueDropdown("GetAllSpellID")]
#endif
        public string imbueSpellId;
        protected SpellCastCharge imbueSpell;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float imbueAmount = 10;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float damageDelay = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float damageAmount = 10f;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public AnimationCurve beamForceCurve = new AnimationCurve(new Keyframe(0f, 10f), new Keyframe(0.05f, 25f), new Keyframe(0.1f, 10f));

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamHandPositionSpringMultiplier = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamHandPositionDamperMultiplier = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamHandRotationSpringMultiplier = 0.2f;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamHandRotationDamperMultiplier = 0.6f;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamHandLocomotionVelocityCorrectionMultiplier = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamLocomotionPushForce = 4f;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamCastMinHandAngle = 20f;

#if ODIN_INSPECTOR
        [BoxGroup("Beam"), ValueDropdown("GetAllEffectID")]
#endif
        public string beamImpactEffectId;
        protected EffectData beamImpactEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public bool instantBreakBreakables = true;

#if ODIN_INSPECTOR
        [BoxGroup("Chaining")]
#endif
        public float chainRadius = 4;

#if ODIN_INSPECTOR
        [BoxGroup("Chaining")]
#endif
        public float chainDelay = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Chaining"), ValueDropdown("GetAllEffectID")]
#endif
        public string electrocuteEffectId;
        protected EffectData electrocuteEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Chaining"), ValueDropdown("GetAllEffectID")]
#endif
        public string chainEffectId;
        protected EffectData chainEffectData;

        [NonSerialized]
        public bool beamActive;
        [NonSerialized]
        public Ray beamRay;
        [NonSerialized]
        public Transform beamStart;
        [NonSerialized]
        public Transform beamHitPoint;
        protected float lastDamageTick;
        protected float lastChainTick;
        protected Collider[] collidersHit;
        protected HashSet<Creature> creaturesHit;
#if ODIN_INSPECTOR
        [BoxGroup("Hook")]
#endif
        public float beamHookDamper = 150;
#if ODIN_INSPECTOR
        [BoxGroup("Hook")]
#endif
        public float beamHookSpring = 1000;
#if ODIN_INSPECTOR
        [BoxGroup("Hook")]
#endif
        public float beamHookSpeed = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Hook")]
#endif
        public float beamHookMaxAngle = 30;
#if ODIN_INSPECTOR
        [BoxGroup("Charge")]
#endif
        public float zapInterval = 0.7f;

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            beamEffectData = Catalog.GetData<EffectData>(beamEffectId);
            imbueSpell = Catalog.GetData<SpellCastCharge>(imbueSpellId);
            chainEffectData = Catalog.GetData<EffectData>(chainEffectId);
            electrocuteEffectData = Catalog.GetData<EffectData>(electrocuteEffectId);
            beamImpactEffectData = Catalog.GetData<EffectData>(beamImpactEffectId);
            collidersHit = new Collider[20];
            beamForceCurve.postWrapMode = WrapMode.Loop;
            creaturesHit = new HashSet<Creature>();
        }

    }
}

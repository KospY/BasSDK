using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad.Skill.Spell;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThunderRoad.Skill.SpellMerge
{
    public class SpellMergeMoltenBeam : SpellMergeData
    {
        [BoxGroup("General")]
        public float movementSpeedMult = 0.8f;

#if ODIN_INSPECTOR
        [BoxGroup("Beam"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string beamEffectId;
        [NonSerialized]
        protected EffectData beamEffectData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamHitHeatTransferPerSecond = 70;

#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public LayerMask layerMask;

#if ODIN_INSPECTOR
        [BoxGroup("Trail"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string trailEffectId;

        [NonSerialized]
        public EffectData trailEffectData;


#if ODIN_INSPECTOR
        [BoxGroup("Trail")]
#endif
        public float trailMinDelay = 0.4f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Trail")]
#endif
        public float trailMaxDelay = 4f;

#if ODIN_INSPECTOR
        [BoxGroup("Trail")]
#endif
        public float trailMinDistance = 1f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Trail")]
#endif
        public float trailMaxDistance = 1.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Trail")]
#endif
        public float trailDuration = 5;
        
#if ODIN_INSPECTOR
        [BoxGroup("Trail")]
#endif
        public float trailRadius = 2;
        
#if ODIN_INSPECTOR
        [BoxGroup("Trail")]
#endif
        public AnimationCurve trailIntensityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

#if ODIN_INSPECTOR
        [BoxGroup("Trail"), MinMaxSlider(0, 5)]
#endif
        public Vector2Int minMaxFireSpark = new Vector2Int(1, 4);


#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamForce = 3f;

#if ODIN_INSPECTOR
        [BoxGroup("Beam"), ValueDropdown(nameof(GetAllSpellID))]
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
        public AnimationCurve beamForceCurve = new(new Keyframe(0f, 10f), new Keyframe(0.05f, 25f), new Keyframe(0.1f, 10f));

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
        [BoxGroup("Beam")]
#endif
        public bool instantBreakBreakables = true;

#if ODIN_INSPECTOR
        [BoxGroup("Status"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusEffectId = "Burning";
        [NonSerialized]
        public StatusData statusData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Status")]
#endif
        public float statusDuration = 5;
        
#if ODIN_INSPECTOR
        [BoxGroup("Status")]
#endif
        public float statusHeatTransfer = 100;
    }
}
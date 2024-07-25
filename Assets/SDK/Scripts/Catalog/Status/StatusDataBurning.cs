using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using ThunderRoad.Skill;
using ThunderRoad.Skill.Spell;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace ThunderRoad
{
    public class StatusDataBurning : StatusData
    {
        public const string Heat = "Heat";
        public const string HeatGainMult = "HeatGainMult";
        public const string HeatLossMult = "HeatLossMult";
        public const string Char = "CharAmount";

#if ODIN_INSPECTOR
        [BoxGroup("Damage")]
#endif
        public float burnDelay = 0.5f; 
#if ODIN_INSPECTOR
        [BoxGroup("Damage")]
#endif
        public float damagePerTick = 0.75f;
#if ODIN_INSPECTOR
        [BoxGroup("Damage")]
#endif
        public float damagePerTickPlayer = 0.75f;

#if ODIN_INSPECTOR
        [BoxGroup("Heat")]
#endif
        public float heatReductionPerSecond = 1f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Heat")]
#endif
        public float heatReductionPerSecondKilled = 2f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Heat")]
#endif
        public float heatReductionPerSecondPlayer = 4f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Heat"), ValueDropdown(nameof(GetAllAnimationID))] 
#endif
        public string heatAnimationId = "HumanFireDeaths";
        [NonSerialized]
        public AnimationData heatAnimationData;

        [BoxGroup("Heat")]
        public float maxHeat = 100;

#if ODIN_INSPECTOR
        [BoxGroup("Animation")]
#endif
        public float animationStartTimeMax = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Charring")]
#endif
        public float fullCharTime = 10f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Charring")]
#endif
        public float charRevealStep = 0.02f;

#if ODIN_INSPECTOR
        [BoxGroup("Charring")]
#endif
        public AnimationCurve charResistanceCurve = AnimationCurve.Linear(0, 1, 1, 0.2f);
        
#if ODIN_INSPECTOR
        [BoxGroup("Charring")]
#endif
        public bool allowVisualCharring = true;
        
#if ODIN_INSPECTOR
        [BoxGroup("Charring")]
#endif
        public bool freezeOnFullChar = true;
        
#if ODIN_INSPECTOR
        [BoxGroup("Charring")]
#endif
        public bool weakenJointsOnFullChar = true;
        
#if ODIN_INSPECTOR
        [BoxGroup("Charring")]
#endif
        public float fullCharCharJointMultiplier = 1;
    
#if ODIN_INSPECTOR
        [BoxGroup("Spread"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string igniteEffectId = "SpellFireBurningSpread";
        
#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string smokingMainEffectId;

        [NonSerialized]
        public EffectData smokingMainEffectData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string smokingLimbEffectId;

        [NonSerialized]
        public EffectData smokingLimbEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string burningMainEffectId;

        [NonSerialized]
        public EffectData burningMainEffectData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string burningLimbEffectId;

        [NonSerialized]
        public EffectData burningLimbEffectData;

        [NonSerialized]
        public EffectData igniteEffectData;
    }
}

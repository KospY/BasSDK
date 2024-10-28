using System;
using System.Collections.Generic;
using ThunderRoad.Skill.Spell;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill.SpellPower
{
	[Serializable]
    public class SpellPowerSlowTime : SpellPowerData
    {
#if ODIN_INSPECTOR
        [BoxGroup("SlowTime"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string effectId;
        [NonSerialized]
        public EffectData effectData;
#if ODIN_INSPECTOR
        [BoxGroup("SlowTime")] 
#endif
        public bool scaleToMixerParameter;
#if ODIN_INSPECTOR
        [BoxGroup("SlowTime")] 
#endif
        public string scaleToMixerParameterName;
#if ODIN_INSPECTOR
        [BoxGroup("SlowTime")] 
#endif
        public AnimationCurve enterCurve;
#if ODIN_INSPECTOR
        [BoxGroup("SlowTime")] 
#endif
        public AnimationCurve exitCurve;
#if ODIN_INSPECTOR
        [BoxGroup("Focus")] 
#endif
        public float scale = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Focus")] 
#endif
        public float focusConsumptionStart = 5f;
#if ODIN_INSPECTOR
        [BoxGroup("Focus")] 
#endif
        public float focusConsumption = 2.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Focus")] 
#endif
        public float focusMinToUse = 10;
#if ODIN_INSPECTOR
        [BoxGroup("HyperFocus")]
#endif
        public string hyperFocusSkillID = "HyperFocus";
#if ODIN_INSPECTOR
        [BoxGroup("HyperFocus")]
#endif
        protected SkillHyperFocus hyperFocusSkill;

#if ODIN_INSPECTOR
        [BoxGroup("Focus"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string focusDepletedEffectId;
        [NonSerialized]
        public EffectData focusDepletedEffectData;

        public delegate void TimeScaleChangeEvent(SpellPowerSlowTime spell, float scale);

        public static event TimeScaleChangeEvent OnTimeScaleChangeEvent;
        
        public delegate void SlowTimeEvent(SpellPowerSlowTime slowTime, bool hyperFocus);
        public event SlowTimeEvent OnFocusRunOutEvent;

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            effectData = Catalog.GetData<EffectData>(effectId);
            focusDepletedEffectData = Catalog.GetData<EffectData>(focusDepletedEffectId);
        }

    }

}

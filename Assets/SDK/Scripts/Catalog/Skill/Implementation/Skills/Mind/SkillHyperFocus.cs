using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad.Skill.SpellPower;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillHyperFocus : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Time Scale")]
#endif
        public float timeScale = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Time Scale")]
#endif
        public float timeScaleAndroid = 0.15f;

        public float TimeScale => Common.IsAndroid ? timeScaleAndroid : timeScale;

#if ODIN_INSPECTOR
        [BoxGroup("Focus Consumption")]
#endif
        public float focusConsumptionStart = 10f;
#if ODIN_INSPECTOR
        [BoxGroup("Focus Consumption")]
#endif
        public float focusConsumption = 5f;
#if ODIN_INSPECTOR
        [BoxGroup("Focus Consumption")]
#endif
        public float minFocus = 20f;
#if ODIN_INSPECTOR
        [BoxGroup("Activation")]
#endif
        public float buttonHoldThreshold = 0.3f;
#if ODIN_INSPECTOR
        [BoxGroup("Multipliers")]
#endif
        public float jointPosMultiplier = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Multipliers")]
#endif
        public float jointRotMultiplier = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Multipliers")]
#endif
        public float speedMultiplier = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Multipliers")]
#endif
        public float dragMult = 5f;
#if ODIN_INSPECTOR
        [BoxGroup("Multipliers")]
#endif
        public float maxSpeedCap = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Multipliers")]
#endif
        public float mixerMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Multipliers")]
#endif
        public float chargeSpeedMultiplier = 3f;

#if ODIN_INSPECTOR
        [BoxGroup("Effects"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string effectId;

        public EffectData effectData;

#if ODIN_INSPECTOR
        [BoxGroup("Effects"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string handEffectId;
        protected EffectData handEffectData;
        public HashSet<object> handlers;

    }
}

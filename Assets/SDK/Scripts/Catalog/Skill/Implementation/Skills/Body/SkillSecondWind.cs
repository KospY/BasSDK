using System;
using System.Collections;
using UnityEngine;
using ThunderRoad.Skill.SpellPower;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill
{
    public class SkillSecondWind : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Charges")]
#endif
        public int chargesPerLevel = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Healing")]
#endif
        public float healRatio = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string startEffectId;

#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string loopEffectId;

#if ODIN_INSPECTOR
        [BoxGroup("Knockback")]
#endif
        public float knockbackUpwardsForce = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Knockback")]
#endif
        public float knockbackRadius = 4;
#if ODIN_INSPECTOR
        [BoxGroup("Knockback")]
#endif
        public float knockbackForce = 10;

#if ODIN_INSPECTOR
        [BoxGroup("Rage")]
#endif
        public float rageDamageReduction = 0.1f;

#if ODIN_INSPECTOR
        [BoxGroup("Rage")]
#endif
        public float rageDuration = 10f;

#if ODIN_INSPECTOR
        [BoxGroup("Rage")]
#endif
        public float rageStrengthMultiplier = 10;

#if ODIN_INSPECTOR
        [BoxGroup("Rage")]
#endif
        public float rageSpeedMultiplier = 1.3f;

        public delegate void SecondWindEvent(SkillSecondWind skill, EventTime time);
        public event SecondWindEvent OnSecondWindEvent;

    }
}

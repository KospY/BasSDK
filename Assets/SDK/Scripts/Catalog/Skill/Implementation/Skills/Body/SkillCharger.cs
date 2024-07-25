using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill
{
    public class SkillCharger : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float sprintDelay = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float force = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Force"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string linesEffectId;

        public EffectData linesEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Force"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string shoveEffectId;

        public EffectData shoveEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Footstep"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string stepEffectId;

        protected EffectData stepEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Player Modifiers")]
#endif
        public float damageReduction = 0.3f;

#if ODIN_INSPECTOR
        [BoxGroup("Player Modifiers")]
#endif
        public float speedMultiplier = 1.3f;

    }

}
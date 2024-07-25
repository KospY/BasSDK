using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillFanTheFlames : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string effectId;
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusEffectId = "Burning";
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect")]
#endif
        public float duration = 3f;
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect")]
#endif
        public float heatTransferRatio = 0.4f;
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect")]
#endif
        public float radius = 3f;
        [NonSerialized]
        public EffectData effectData;
        protected StatusData status;

    }
}

using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillZapStatusExtender : SpellSkillData
    {
        #if ODIN_INSPECTOR
        [BoxGroup("Status"), ValueDropdown(nameof(GetAllStatusEffectID))]
        #endif
        public string statusId;

        [BoxGroup("Status")]
        public float duration = 3f;

        protected StatusData statusData;
    }
}

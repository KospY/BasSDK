using System.Collections;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using ThunderRoad.Skill.Spell;
using UnityEngine;

namespace ThunderRoad.Skill
{
    public class SkillArcherImbue : AISkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float autoImbueTime = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Spell"), ValueDropdown(nameof(GetAllSpellID))]
#endif
        public string spellId;

#if ODIN_INSPECTOR
        [BoxGroup("Spell"), ValueDropdown(nameof(GetAllSpellID))]
#endif
        public List<string> randomSpells;

    }
}

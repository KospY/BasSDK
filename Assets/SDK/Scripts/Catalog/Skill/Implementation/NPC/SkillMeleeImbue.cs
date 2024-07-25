using System.Collections;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace ThunderRoad.Skill
{
    public class SkillMeleeImbue : AISkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float autoImbueTime = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public bool recharge = true;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        [FormerlySerializedAs("autoImbueDelay")]
        public float rechargeDelay = 5f;

#if ODIN_INSPECTOR
        [BoxGroup("Spell"), ValueDropdown(nameof(GetAllSpellID))]
#endif
        public string spellId;

#if ODIN_INSPECTOR
        [BoxGroup("Spell"), ValueDropdown(nameof(GetAllSpellID))]
#endif
        public List<string> randomSpells;

        protected string chosenSpell;
    }
}

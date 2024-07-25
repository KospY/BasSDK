using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillSapStatusApplier : SpellSkillData
    {
        #if ODIN_INSPECTOR
        [BoxGroup("Imbue"), ValueDropdown(nameof(GetAllSpellID))]
        #endif
        public string spellId;

        [BoxGroup("Imbue")]
        public Gradient sapGradient;

        [BoxGroup("Bolts")]
        public Gradient boltGradient;

        #if ODIN_INSPECTOR
        [BoxGroup("Status Effect"), ValueDropdown(nameof(GetAllStatusEffectID))]
        #endif
        public string statusEffectId;

        [NonSerialized]
        public StatusData statusData;

        [BoxGroup("Status Effect")]
        public float statusDuration = 2f;

        protected int spellHashId;
       
    }
}
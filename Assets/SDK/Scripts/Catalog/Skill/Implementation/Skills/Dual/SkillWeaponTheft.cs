using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using ThunderRoad.Skill.SpellPower;
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillWeaponTheft : SkillData
    {
        [BoxGroup("Steal")]
        public float stealDelay;

        [BoxGroup("Bolts")]
        public Vector2 boltDelay = new(0.5f, 0.1f);

        #if ODIN_INSPECTOR
        [BoxGroup("Bolts"), ValueDropdown(nameof(GetAllEffectID))]
        #endif
        public string boltEffectId;
        protected EffectData boltEffectData;

    }
}
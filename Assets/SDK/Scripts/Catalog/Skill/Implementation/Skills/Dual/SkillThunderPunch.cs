#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillThunderPunch : SkillSpellPunch
    {
#if ODIN_INSPECTOR
        [BoxGroup("Bolt"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string boltEffectId;

        protected EffectData boltEffect;

#if ODIN_INSPECTOR
        [BoxGroup("Bolt")]
#endif
        public float boltRadius = 3;

#if ODIN_INSPECTOR
        [BoxGroup("Bolt")]
#endif
        public float damageShareRatio = 0.5f;
    }
}
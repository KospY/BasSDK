using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill
{
    public class SkillMightyKick : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Kick"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string kickEffectId;

#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float forceMultiplierRagdolls = 1.3f;

#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float forceMultiplierItems = 3f;

    }
}
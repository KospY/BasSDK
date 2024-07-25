#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillSlowingParry : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Status"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusId;

        protected StatusData statusData;

        [BoxGroup("Status")]
        public float duration = 1f;
        [BoxGroup("Status")]
        public float extraSlowMult = 0.3f;
    }
}

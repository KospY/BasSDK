#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillHotPotato : SkillHeldImbue
    {
        [BoxGroup("Status")]
        public float speedMultiplier = 1.5f;
        #if ODIN_INSPECTOR
        [BoxGroup("Status"), ValueDropdown(nameof(GetAllStatusEffectID))]
        #endif
        public string statusEffectId;
        public StatusData statusEffect;

    }
}

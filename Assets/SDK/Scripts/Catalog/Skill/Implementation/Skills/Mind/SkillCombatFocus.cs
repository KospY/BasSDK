#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillCombatFocus : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Focus")]
#endif
        public float killRegen = 15;
#if ODIN_INSPECTOR
        [BoxGroup("Focus")]
#endif
        public float sliceRegen = 30;
#if ODIN_INSPECTOR
        [BoxGroup("Focus")]
#endif
        public float parryRegen = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Focus")]
#endif
        public float deflectRegen = 15;
#if ODIN_INSPECTOR
        [BoxGroup("Focus")]
#endif
        public float rangedHeadshotRegen = 30;
#if ODIN_INSPECTOR
        [BoxGroup("Focus")]
#endif
        public float disarmRegen = 15;

    }
}

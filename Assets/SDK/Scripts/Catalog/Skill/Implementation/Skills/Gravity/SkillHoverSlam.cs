#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillHoverSlam : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Slam")]
        public float slamGravityRatio = 5;
#endif
#if ODIN_INSPECTOR
        [BoxGroup("Slam")]
        public float slamDuration = 2f;
#endif

    }
}

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillMassReduction : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Mass Reduction")]
#endif
        public float massMultiplier = 0.25f;
#if ODIN_INSPECTOR
        [BoxGroup("Joint Modifier")]
#endif
        public float positionSpring = 1.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Joint Modifier")]
#endif
        public float positionDamper = 1.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Joint Modifier")]
#endif
        public float rotationSpring = 1.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Joint Modifier")]
#endif
        public float rotationDamper = 1.5f;
    }
}

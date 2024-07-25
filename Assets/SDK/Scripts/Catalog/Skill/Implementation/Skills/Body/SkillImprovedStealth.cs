#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill
{
    public class SkillImprovedStealth : SkillData
    {
        public float sneakDamageMultiplier = 2f;
        public float footstepIntensityMultiplier = 0.5f;
        public float fovModifier = 0.5f;

    }
}

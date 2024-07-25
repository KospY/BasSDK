using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill
{
    public class SkillFlykick : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Acceleration")]
#endif
        public float speed = 3f;

#if ODIN_INSPECTOR
        [BoxGroup("Acceleration")]
#endif
        public float downwardVelocityCorrection = 0f;

    }
}

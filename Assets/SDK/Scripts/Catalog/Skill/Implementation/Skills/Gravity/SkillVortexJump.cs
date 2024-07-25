using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillVortexJump : BoostSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Boost")]
#endif
        public float maxAngle = 20f;
#if ODIN_INSPECTOR
        [BoxGroup("Vortex")]
#endif
        public float radius = 2f;
#if ODIN_INSPECTOR
        [BoxGroup("Vortex")]
#endif
        public float duration = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Vortex")]
#endif
        public AnimationCurve forceCurve;

    }
}

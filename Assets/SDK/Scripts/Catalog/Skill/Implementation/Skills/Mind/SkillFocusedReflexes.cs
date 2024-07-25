using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillFocusedReflexes : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float radius = 3f;
#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float minimumVelocity = 3f;
#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float velocityThreatAngle = 40;
#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public bool noOwnerIsThreat = false;

    }

    public class ProjectileDetector : ThunderBehaviour
    {
        public delegate void ProjectileCountEvent(ProjectileDetector detector);
        public event ProjectileCountEvent OnAnyProjectiles;
        public event ProjectileCountEvent OnNoProjectiles;

        public Creature creature;
        public SkillFocusedReflexes skill;

        private int lastCount;

    }
}

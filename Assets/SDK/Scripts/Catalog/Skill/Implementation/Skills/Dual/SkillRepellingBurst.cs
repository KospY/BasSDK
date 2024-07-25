using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillRepellingBurst : SkillData
    {
        #if ODIN_INSPECTOR
        [BoxGroup("Dependencies"), ValueDropdown(nameof(GetAllSkillID))]
        #endif
        public string skillRemoteDetonationId = "RemoteDetonation";
        [NonSerialized]
        public SkillRemoteDetonation skillRemoteDetonation;

#if ODIN_INSPECTOR
        [BoxGroup("Explosion Force")]
#endif
        public float force = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion Force")]
#endif
        public float forcePlayer = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion Force")]
#endif
        public float upwardsModifier = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Explosion Force")]
#endif
        public ForceMode forceMode = ForceMode.VelocityChange;

#if ODIN_INSPECTOR
        [BoxGroup("Explosion Force")]
#endif
        public int pushLevel = 2;

    }
}

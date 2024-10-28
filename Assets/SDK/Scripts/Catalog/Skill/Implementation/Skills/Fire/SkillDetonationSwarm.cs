#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThunderRoad.Skill.Spell
{
    public class SkillDetonationSwarm : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Sparks")]
#endif
        public float damageMultiplier = 0.4f;
#if ODIN_INSPECTOR
        [BoxGroup("Sparks")]
#endif
        public float fireVelocity = 3f;
#if ODIN_INSPECTOR
        [BoxGroup("Sparks")]
#endif
        public int baseSparkCount = 3;
#if ODIN_INSPECTOR
        [BoxGroup("Sparks")]
#endif
        public int sparkCountBody = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Sparks")]
#endif
        public float homingDelay = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Sparks")]
#endif
        public bool requireBurningStatus = true;

#if ODIN_INSPECTOR
        [BoxGroup("Dependencies"), ValueDropdown(nameof(GetAllSkillID))]
#endif
        public string remoteDetonationSkillId = "RemoteDetonation";

        [NonSerialized]
        public SkillRemoteDetonation remoteDetonationSkill;
#if ODIN_INSPECTOR
        [BoxGroup("Dependencies"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string burningStatusId = "Burning";
        
        [NonSerialized]
        public StatusDataBurning burning;

        protected SkillRemoteDetonation skillRemoteDetonate;

    }
}

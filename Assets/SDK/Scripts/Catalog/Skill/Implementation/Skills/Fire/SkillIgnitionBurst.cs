#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThunderRoad.Skill.Spell
{
    public class SkillIgnitionBurst : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Ignition")]
#endif
        public float igniteMinRadius;

#if ODIN_INSPECTOR
        [BoxGroup("Ignition")]
#endif
        public float ignitionHeat;
        
#if ODIN_INSPECTOR
        [BoxGroup("Ignition")]
#endif
        public AnimationCurve heatFalloffMult = AnimationCurve.Linear(0, 0, 1, 1);

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
        
    }
}

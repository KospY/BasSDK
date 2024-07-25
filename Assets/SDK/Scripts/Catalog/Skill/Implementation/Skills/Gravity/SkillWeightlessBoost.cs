using System;
using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillWeightlessBoost : BoostSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusId;

        [NonSerialized]
        public StatusData statusData; 
        
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect")]
#endif
        public float duration = 0.3f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Boost")]
#endif
        public float maxAngle = 20f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Boost")]
#endif
        public float boostWindow = 1;

    }
}

using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillWeightlessShove : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Shove Detection")]
#endif
        public float maxDelay = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Status"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusId;
#if ODIN_INSPECTOR
        [BoxGroup("Status")]
#endif
        public float duration = 2f;
        
        protected StatusData statusData;
        protected float lastPush = 0;
        protected Vector3 lastPushDir;

    }
}

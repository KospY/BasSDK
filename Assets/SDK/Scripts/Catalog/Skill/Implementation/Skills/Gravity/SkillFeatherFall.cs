#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{

    public class SkillFeatherFall : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Hover")]
#endif
        public float hoverVelocityThreshold = -0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Hover")]
#endif
        public float hoverForce = 0.46f;
#if ODIN_INSPECTOR
        [BoxGroup("Hover"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string hoverEffectId;
#if ODIN_INSPECTOR
        [BoxGroup("Hover")]
#endif
        public float groundEffectDistance = 3f;
#if ODIN_INSPECTOR
        [BoxGroup("Hover")]
#endif
        public float groundEffectMinDistance = 1.2f;
//#if ODIN_INSPECTOR
//        [BoxGroup("Hover"), ValueDropdown(nameof(GetAllEffectID))]
//#endif
        //public float groundEffectMinVelocity = -2f;

        protected EffectData hoverEffectData;

    }
}

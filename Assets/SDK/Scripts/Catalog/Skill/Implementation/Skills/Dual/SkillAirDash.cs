#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillAirDash : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Jump"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string effectId;
        protected EffectData effectData;
#if ODIN_INSPECTOR
        [BoxGroup("Jump")]
#endif
        public float dashForce = 10;

    }
}

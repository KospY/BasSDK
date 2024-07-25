using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillShockParry : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusId = "Electrocute";
        
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect")]
#endif
        public float duration = 3f;

        [NonSerialized]
        public StatusData statusData;


    }

}

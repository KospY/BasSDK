using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillStaticDisarm : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float disarmChanceNPC = 0.8f;
#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float disarmChancePlayer = 0.3f;

    }
}

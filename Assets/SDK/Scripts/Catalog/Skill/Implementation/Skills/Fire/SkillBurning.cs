
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System.Collections.Generic;

namespace ThunderRoad.Skill.Spell
{
    public class SkillBurning : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusEffectId = "Burning";

        [NonSerialized]
        public StatusData statusEffect;
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect")]
#endif
        public float heatTransfer = 5;

#if ODIN_INSPECTOR
        [BoxGroup("Status Effect"), ValueDropdown(nameof(GetAllMaterialID))]
#endif
        public List<string> ignitionBlockedMaterials;
        

    }
}
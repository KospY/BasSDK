using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillHeldImbue : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Spell"), ValueDropdown(nameof(GetAllSpellID))]
#endif
        public string spellId;


        public virtual void Activate(Creature creature)
        {
        }

        public virtual void Deactivate(Creature creature)
        {
        }
    }
}

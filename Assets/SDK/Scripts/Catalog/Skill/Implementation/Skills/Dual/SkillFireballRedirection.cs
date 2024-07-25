using ThunderRoad.Skill.SpellPower;
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillFireballRedirection : TkSkillData
    {
        public override void OnTkHandSkillLoad(Creature creature, SpellTelekinesis telekinesis)
        {
            base.OnTkHandSkillLoad(creature, telekinesis);

        }

        public override void OnTkHandSkillUnload(Creature creature, SpellTelekinesis telekinesis)
        {
            base.OnTkHandSkillUnload(creature, telekinesis);

        }

    }
}

using ThunderRoad.Skill.SpellPower;

namespace ThunderRoad.Skill.Spell
{
    public class SkillForceChoke : TkSkillData
    {
        public override void OnTkHandSkillLoad(Creature creature, SpellTelekinesis telekinesis)
        {
            base.OnTkHandSkillLoad(creature, telekinesis);
            telekinesis.allowChoke = true;
        }

        public override void OnTkHandSkillUnload(Creature creature, SpellTelekinesis telekinesis)
        {
            base.OnTkHandSkillUnload(creature, telekinesis);
            telekinesis.allowChoke = false;
        }
    }
}

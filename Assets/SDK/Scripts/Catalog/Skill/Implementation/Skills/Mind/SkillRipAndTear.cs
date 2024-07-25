using ThunderRoad.Skill.SpellPower;

namespace ThunderRoad.Skill.Spell
{
    public class SkillRipAndTear : TkSkillData
    {
        public override void OnTkHandSkillLoad(Creature creature, SpellTelekinesis telekinesis)
        {
            base.OnTkHandSkillLoad(creature, telekinesis);
            telekinesis.allowDismemberment = true;
        }

        public override void OnTkHandSkillUnload(Creature creature, SpellTelekinesis telekinesis)
        {
            base.OnTkHandSkillUnload(creature, telekinesis);
            telekinesis.allowDismemberment = false;
        }
    }
}

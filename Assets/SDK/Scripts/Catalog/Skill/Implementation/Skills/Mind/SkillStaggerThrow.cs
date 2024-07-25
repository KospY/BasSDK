using ThunderRoad.Skill.SpellPower;

namespace ThunderRoad.Skill.Spell
{
    public class SkillStaggerThrow : TkSkillData
    {
        public int grabThrowLevel = 1;
        public bool forceDestabilizeOnThrow = false;
        public override void OnTkHandSkillLoad(Creature creature, SpellTelekinesis telekinesis)
        {
            base.OnTkHandSkillLoad(creature, telekinesis);
            telekinesis.grabThrowLevel = grabThrowLevel;
            telekinesis.forceDestabilizeOnThrow = forceDestabilizeOnThrow;
        }

        public override void OnTkHandSkillUnload(Creature creature, SpellTelekinesis telekinesis)
        {
            base.OnTkHandSkillUnload(creature, telekinesis);
            telekinesis.grabThrowLevel = 0;
            telekinesis.forceDestabilizeOnThrow = false;
        }
    }
}

using ThunderRoad.Skill.SpellPower;

namespace ThunderRoad.Skill.Spell
{
    public class SkillTeleSpin : TkSkillData
    {
        public float focusCost = 10f;
        public override void OnTkHandSkillLoad(Creature creature, SpellTelekinesis telekinesis)
        {
            base.OnTkHandSkillLoad(creature, telekinesis);
            telekinesis.allowSpin = true;
        }

        public override void OnTkHandSkillUnload(Creature creature, SpellTelekinesis telekinesis)
        {
            base.OnTkHandSkillUnload(creature, telekinesis);
            telekinesis.allowSpin = false;
        }
    }
}

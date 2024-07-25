using ThunderRoad.Skill.SpellPower;

namespace ThunderRoad.Skill.Spell
{
    public class SkillTemporalThunder : SpellSkillData
    {
        public override void OnSpellLoad(SpellData spell, SpellCaster caster = null)
        {
            base.OnSpellLoad(spell, caster);
            if (spell is not SpellCastLightning lightning) return;
            lightning.speedUpByTimeScale = true;
        }

        public override void OnSpellUnload(SpellData spell, SpellCaster caster = null)
        {
            base.OnSpellUnload(spell, caster);
            if (spell is not SpellCastLightning lightning) return;
            lightning.speedUpByTimeScale = false;
        }
    }
}

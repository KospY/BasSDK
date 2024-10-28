using ThunderRoad.Skill.SpellPower;

namespace ThunderRoad.Skill
{
    public class TkSkillData : SkillData
    {
        public override void OnSkillLoaded(SkillData skillData, Creature creature)
        {
            base.OnSkillLoaded(skillData, creature);
        }

        private void OnSpellUnload(SpellData spellInstance, SpellCaster caster)
        {
        }
        private void OnSpellLoad(SpellData spellInstance, SpellCaster caster)
        {
        }

        public override void OnSkillUnloaded(SkillData skillData, Creature creature)
        {
            base.OnSkillUnloaded(skillData, creature);

        }

        /// <summary>
        /// Called once per hand, given each hand's TK instance
        /// </summary>
        public virtual void OnTkHandSkillLoad(Creature creature, SpellTelekinesis telekinesis)
        {
            
        }
        
        /// <summary>
        /// Called once per hand, given each hand's TK instance
        /// </summary>
        public virtual void OnTkHandSkillUnload(Creature creature, SpellTelekinesis telekinesis)
        {
            
        }
    }
}

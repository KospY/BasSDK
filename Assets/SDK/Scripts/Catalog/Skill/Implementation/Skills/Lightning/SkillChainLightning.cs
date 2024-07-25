#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillChainLightning : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Chaining")]
#endif
        public float chainRangeMultiplier = 3f;
        public override void OnSpellLoad(SpellData spell, SpellCaster caster = null)
        {
            base.OnSpellLoad(spell, caster);
            if (spell is not SpellCastLightning lightning) return;
            lightning.allowChainItems = true;
            lightning.allowChainEnemies = true;
            lightning.AddModifier(this, SpellCastLightning.modifierChainRange, chainRangeMultiplier);
        }

        public override void OnSpellUnload(SpellData spell, SpellCaster caster = null)
        {
            base.OnSpellUnload(spell, caster);
            if (spell is not SpellCastLightning lightning) return;
            lightning.allowChainItems = false;
            lightning.allowChainEnemies = false;
            lightning.RemoveModifier(this, SpellCastLightning.modifierChainRange);
        }
    }
}

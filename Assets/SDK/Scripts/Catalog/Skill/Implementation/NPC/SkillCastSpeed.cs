#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill
{
    public class SkillCastSpeed : AISkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Charge Speed")]
#endif
        public float chargeSpeedMultiplier = 1;

        public override void OnSpellLoad(SpellData spell, SpellCaster caster = null)
        {
            base.OnSpellLoad(spell, caster);
            spell?.AddModifier(this, Modifier.ChargeSpeed, chargeSpeedMultiplier);
        }

        public override void OnSpellUnload(SpellData spell, SpellCaster caster = null)
        {
            base.OnSpellUnload(spell, caster);
            spell?.RemoveModifier(this, Modifier.ChargeSpeed);
        }
    }
}

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class GripCastSkillData : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Spell"), ValueDropdown(nameof(GetAllSpellID))]
#endif
        public string spellId;
        protected SpellCastCharge spellData;
        [BoxGroup("Damage")]
        public float gripCastDamageInterval = 0.5f;
        [BoxGroup("Damage")]
        public float gripCastDamageAmount = 4f;

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            spellData = Catalog.GetData<SpellCastCharge>(spellId);
        }

        public override void OnSpellLoad(SpellData spell, SpellCaster caster = null)
        {
            base.OnSpellLoad(spell, caster);
            if (spellData == null || spell == null || spell.hashId != spellData.hashId || spell is not SpellCastCharge charge) return;
            charge.allowGripCast = true;
            charge.gripCastDamageAmount = gripCastDamageAmount;
            charge.gripCastDamageInterval = gripCastDamageInterval;
        }

        public override void OnSpellUnload(SpellData spell, SpellCaster caster = null)
        {
            base.OnSpellUnload(spell, caster);
            if (spellData == null || spell == null || spell.hashId != spellData.hashId || spell is not SpellCastCharge charge) return;
            charge.allowGripCast = false;
        }
    }
}

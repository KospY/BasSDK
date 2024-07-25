using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillOvercharge : SpellSkillData
    {
        public float overchargeStartMaxCharge = 0.3f;
        public float intensityMultiplier = 10f;
        public float efficiencyMultiplier = 4f;
        public float speedMultiplier = 10f;
        public Gradient gradient;

        public override void OnSpellLoad(SpellData spell, SpellCaster caster = null)
        {
            base.OnSpellLoad(spell, caster);
            if (spell is not SpellCastLightning lightning) return;
            lightning.OnSprayLoopEvent -= OnLoop;
            lightning.OnSprayLoopEvent += OnLoop;
            //lightning.OnChargeSappingEvent += OnSap;
        }

        public override void OnSpellUnload(SpellData spell, SpellCaster caster = null)
        {
            base.OnSpellUnload(spell, caster);
            if (spell is not SpellCastLightning lightning) return;
            lightning.OnSprayLoopEvent -= OnLoop;
            lightning.RemoveModifiers(this);
        }

        private void OnLoop(SpellCastLightning spell)
        {
            spell.RemoveModifiers(this);
            if (spell.currentCharge > overchargeStartMaxCharge || spell.currentCharge < spell.sprayStopMinCharge) return;
            spell.AddModifier(this, Modifier.Intensity, intensityMultiplier);
            spell.AddModifier(this, Modifier.Speed, speedMultiplier);
            spell.AddModifier(this, Modifier.Efficiency, efficiencyMultiplier);
            spell.OverrideBoltColor(gradient);
        }

        // Old code for when it was only for Charge Sapping
        private void OnSap(SpellCastLightning spell, SkillChargeSapping skill, EventTime time, SpellCastCharge other)
        {
            switch (time)
            {
                case EventTime.OnStart:
                    spell.RemoveModifiers(this);
                    break;
                case EventTime.OnEnd when other is SpellCastLightning
                                          && spell.currentCharge > spell.sprayStopMinCharge
                                          && spell.currentCharge < overchargeStartMaxCharge:
                    spell.AddModifier(this, Modifier.Intensity, intensityMultiplier);
                    spell.AddModifier(this, Modifier.Speed, speedMultiplier);
                    spell.AddModifier(this, Modifier.Efficiency, efficiencyMultiplier);
                    spell.OverrideBoltColor(gradient);
                    break;
            }
        }
    }
}

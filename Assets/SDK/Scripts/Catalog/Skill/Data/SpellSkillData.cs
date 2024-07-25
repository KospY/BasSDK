namespace ThunderRoad.Skill
{
    public class SpellSkillData : SkillData
    {
        /// <summary>
        /// Called when any spell is loaded on a creature who owns this skill.
        /// </summary>
        /// <remarks>Use this to enable your skill for a particular spell or to hook into events on that spell.</remarks>
        /// <param name="spell">The loaded spell</param>
        /// <param name="caster">The SpellCaster that loaded the spell - can be null.</param>
        /// <example><code>public virtual void OnSpellLoad(SpellData spell, SpellCaster caster = null) {
        ///     base.OnSpellLoad(spell, caster);
        ///     if (spell is SpellCastProjectile { id: "Fire" } fire) {
        ///         fire.OnFireballHitEvent += OnFireballHit;
        ///     }
        /// }</code></example>
        public virtual void OnSpellLoad(SpellData spell, SpellCaster caster = null)
        {
        }

        /// <summary>
        /// Called when any spell is unloaded from a creature who owns this skill.
        /// </summary>
        /// <remarks>You will usually use this to clean up, checking if the unloaded spell is a certain one.</remarks>
        /// <param name="spell">The unloaded spell</param>
        /// <param name="caster">The caster from which it was unloaded (if any)</param>
        public virtual void OnSpellUnload(SpellData spell, SpellCaster caster = null)
        {
        }

    }
}

using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillChargeSapping : SpellSkillData
    {
        public int maxDrainCount = 6;
        public float damageMult = 3;
        public float rangeMult = 3;
        public float fireRateMult = 2;
        public float drainDelay = 0.3f;
        public float maxDrainRadius = 4;
        public float drainPerSecond = 5;
        public bool allowWhileOtherSpellSpraying = true;

        protected float lastDrain;

        protected int lightningHashId;

    }
}

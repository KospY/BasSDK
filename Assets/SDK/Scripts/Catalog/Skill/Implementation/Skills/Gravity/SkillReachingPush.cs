
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillReachingPush : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Spell Modifiers")]
#endif
        public float rangeMultiplier = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Spell Modifiers")]
#endif
        public float forceMultiplier = 1.5f;

        public string effectId = "SpellGravityPushReach";
        public EffectData effectData;

    }
}

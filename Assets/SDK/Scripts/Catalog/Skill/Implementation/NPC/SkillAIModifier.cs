using UnityEngine;
using UnityEngine.Serialization;

namespace ThunderRoad.Skill
{
    public class SkillAIModifier : AISkillData
    {
        [Header("Modifiers")]
        public float heatGainMult = 1;
        public float heatLossMult = 1;
        public float hitEnvironmentDamageModifier = 1;
        public float healthModifier = 1;

    }
}

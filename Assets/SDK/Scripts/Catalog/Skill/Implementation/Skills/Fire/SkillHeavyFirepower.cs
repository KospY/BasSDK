#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillHeavyFirepower : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Disarm")]
#endif
        public bool disarmPlayer = true;
#if ODIN_INSPECTOR
        [BoxGroup("Disarm")]
#endif
        public bool disarmEnemies = true;
#if ODIN_INSPECTOR
        [BoxGroup("Impact Force")]
#endif
        public float impactForceItem = 6;
#if ODIN_INSPECTOR
        [BoxGroup("Impact Force")]
#endif
        public float impactForceCreature = 30;
#if ODIN_INSPECTOR
        [BoxGroup("Impact Force")]
#endif
        public float impactForcePlayer = 30;
#if ODIN_INSPECTOR
        [BoxGroup("Impact Force")]
#endif
        public ForceMode impactForceMode = ForceMode.VelocityChange;
        
#if ODIN_INSPECTOR
        [BoxGroup("Impact Force")]
#endif
        public int pushLevel = 2;

#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string effectId;

        protected EffectData effectData;
    }
}


using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillImpactPulse : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Hit")]
#endif
        public float minMomentum = 8f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Hit")]
#endif
        public float minVelocity = 5f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Hit"), MinMaxSlider(0, 3, true)]
#endif
        public Vector2 extraForce;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Hit")]
#endif
        public float maxExtraForce = 10f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Hit")]
#endif
        public ForceMode forceMode = ForceMode.Impulse;
        
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Hit")]
#endif
        public float playerMultiplier = 3;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue Hit")]
#endif
        public float imbueEnergyDrainOnHit = 5f;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue Hit")]
#endif
        public int pushLevel = 2;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue Hit"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string effectId;

        protected EffectData effectData;
    }
}

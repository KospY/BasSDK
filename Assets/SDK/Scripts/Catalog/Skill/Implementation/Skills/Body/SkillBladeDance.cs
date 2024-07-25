using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill
{
    public class SkillBladeDance : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string swingBladeEffectId;

        [NonSerialized]
        public EffectData swingBladeEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string swingBluntEffectId;

        [NonSerialized]
        public EffectData swingBluntEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Momentum")]
#endif
        public float minVelocity = 2.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Momentum")]
#endif
        public float swingMomentumGain = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Momentum")]
#endif
        public float lingerDelay = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Momentum")]
#endif
        public float velocityOffset = 0.3f;

#if ODIN_INSPECTOR
        [BoxGroup("Modifiers"), MinMaxSlider(0, 3, true)]
#endif
        public Vector2 sliceAngle = new(0, 2.5f);
#if ODIN_INSPECTOR
        [BoxGroup("Modifiers")]
#endif
        public float force = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Modifiers")]
#endif
        public int pushLevel = 2;

    }

}

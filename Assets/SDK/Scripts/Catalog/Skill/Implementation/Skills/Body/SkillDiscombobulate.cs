using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill
{
    public class SkillDiscombobulate : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Impact Modifiers")]
#endif
        public float impactVelocity = 6f;
#if ODIN_INSPECTOR
        [BoxGroup("Impact Modifiers")]
#endif
        public float headDamageMultiplier = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Impact Modifiers")]
#endif
        public float stunTime = 6f;
#if ODIN_INSPECTOR
        [BoxGroup("Impact Modifiers")]
#endif
        public bool ragdollDuringAttack = false;

#if ODIN_INSPECTOR
        [BoxGroup("Bonk Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string bonkEffectId;

        protected EffectData bonkEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Bonk Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string bonkAltEffectId;

        protected EffectData bonkAltEffectData;

    }
}

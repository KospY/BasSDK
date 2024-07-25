using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using ThunderRoad.Skill.SpellPower;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThunderRoad.Skill
{
    public class SkillRicochetThrow : TkSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Ricochet")]
#endif
        public float maxDistance = 10f;
#if ODIN_INSPECTOR
        [BoxGroup("Ricochet")]
#endif
        public float maxAngle = 50f;
#if ODIN_INSPECTOR
        [BoxGroup("Ricochet")]
#endif
        public int maxBounces = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Ricochet")]
#endif
        public float maxRicochetVelocity = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Ricochet")]
#endif
        public float velocityHitMultiplier = 0.9f;
#if ODIN_INSPECTOR
        [BoxGroup("Ricochet")]
#endif
        public float additionalDamageMult = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Ricochet")]
#endif
        public float nonAimAssistVelocityMult = 0.8f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float throwVelocityMultiplier = 1.3f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float maxThrowDistance = 10f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float maxThrowAngle = 30f;

#if ODIN_INSPECTOR
        [BoxGroup("Return")]
#endif
        public bool returnOnLastBounce = true;

#if ODIN_INSPECTOR
        [BoxGroup("Return"), ShowIf("returnOnLastBounce")]
#endif
        public float returnSpeed = 10f;

#if ODIN_INSPECTOR
        [BoxGroup("Return"), ShowIf("returnOnLastBounce")]
#endif
        public float autoGrabRadius = 2f;

#if ODIN_INSPECTOR
        [BoxGroup("Return"), ShowIf("returnOnLastBounce")]
#endif
        public float grabWindow = 0.6f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Return"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string returnEffectId;

#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string grabEffectId;

        [NonSerialized]
        public EffectData grabEffectData;
        
        [NonSerialized]
        public EffectData returnEffectData;
    }
}

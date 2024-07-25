using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill
{
    public class SkillRangedExpert : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Piercing")]
#endif
        public float penetrationVelocity = 1.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Piercing")]
#endif
        public float penetrationMaxVelocity = 30;
#if ODIN_INSPECTOR
        [BoxGroup("Piercing")]
#endif
        public bool pierceOnlyRigidbodies = true;

#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown("GetAllEffectID")]
#endif
        public string pierceEffectId = "RangedExpertPierce";
        protected EffectData pierceEffectData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Ricochet")]
#endif
        public bool bounceOffTerrain = true;
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
        public float nonAimAssistVelocityMult = 0.8f;
#if ODIN_INSPECTOR
        [BoxGroup("Aim Assist")]
#endif
        public float maxAngle = 40;

#if ODIN_INSPECTOR
        [BoxGroup("Aim Assist")]
#endif
        public float maxDistance = 10;

#if ODIN_INSPECTOR
        [BoxGroup("Aim Assist")]
#endif
        public bool aimAssistOnThrow = false;

#if ODIN_INSPECTOR
        [BoxGroup("Aim Assist"), ShowIf(nameof(aimAssistOnThrow))]
#endif
        public float aimAssistMaxDistance = 5;

#if ODIN_INSPECTOR
        [BoxGroup("Aim Assist"), ShowIf(nameof(aimAssistOnThrow))]
#endif
        public float aimAssistMaxAngle = 20;

    }

}

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillGravityDecapitate : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Push Decapitate")]
#endif
        public float minCharge = 0.95f;
#if ODIN_INSPECTOR
        [BoxGroup("Push Decapitate")]
#endif
        public LayerMask layerMask;
#if ODIN_INSPECTOR
        [BoxGroup("Push Decapitate")]
#endif
        public float maxDistance = 0.2f;

#if ODIN_INSPECTOR
        [BoxGroup("Push Decapitate")]
#endif
        public float pushedPartVelocityMult = 0.4f;
#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string effectId = "SpellGravityPushDecapitate";
        protected EffectData effectData;

#if ODIN_INSPECTOR
        [BoxGroup("Backup Push Force")]
#endif
        public float pushForce = 3f;

#if ODIN_INSPECTOR
        [BoxGroup("Backup Push Force")]
#endif
        public ForceMode pushForceMode = ForceMode.VelocityChange;

    }
}

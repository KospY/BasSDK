using System;
using UnityEngine;
using ThunderRoad.Skill.SpellPower;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill
{
    public class SkillBruteForce : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Limb Tearing")]
#endif
        public float minBreakForceMultiplier = 1.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Karate Chop")]
#endif
        public bool overrideMinSpeed = false;
#if ODIN_INSPECTOR
        [BoxGroup("Karate Chop")]
#endif
        public float chopMinSpeed = 6f;
#if ODIN_INSPECTOR
        [BoxGroup("Karate Chop Damager"), ValueDropdown(nameof(GetAllDamagerID))]
#endif
        public string damagerId;
        protected DamagerData damagerData;
#if ODIN_INSPECTOR
        [BoxGroup("Karate Chop Damager")]
#endif
        public int damagerTier = 3;
#if ODIN_INSPECTOR
        [BoxGroup("Karate Chop Damager")]
#endif
        public bool ignoreArmor = false;
#if ODIN_INSPECTOR
        [BoxGroup("Karate Chop Damager"), ValueDropdown(nameof(GetAllMaterialID))]
#endif
        public string materialId;

#if ODIN_INSPECTOR
        [BoxGroup("Karate Chop"), ValueDropdown(nameof(GetAllHandPoseID))]
#endif
        public string karateChopTargetPoseId;

        [NonSerialized]
        public HandPoseData karateChopTargetPose;

#if ODIN_INSPECTOR
        [BoxGroup("Karate Chop"), ValueDropdown(nameof(GetAllHandPoseID))]
#endif
        public string karateChopDefaultPoseId;

    }

}
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillRemoteDetonation : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Detonation"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string explosionEffectId = "RemoteDetonation";
        [NonSerialized]
        public EffectData explosionEffectData;

        [BoxGroup("Detonation")]
        public float playerDamage = 10f;
        [BoxGroup("Detonation")]
        public float enemyDamage = 50f;
        [BoxGroup("Detonation")]
        public float radius = 2.5f;
        [BoxGroup("Detonation")]
        public float pushMinRadius = 1.5f;
        [BoxGroup("Detonation")]
        public float force = 10f;
        [BoxGroup("Detonation")]
        public float breakForce = 50f;

        [BoxGroup("Detonation")]
        public ForceMode forceMode = ForceMode.Impulse;

        [BoxGroup("Detonation")]
        public AnimationCurve hapticAnimationCurve;
        
        protected SkillBurning ignitionData;
        protected GameData.HapticClip hapticClip;
    }
}

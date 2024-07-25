
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillInnerFlame : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Explosion")]
#endif
        public float explosionRadius = 3;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion")]
#endif
        public LayerMask explosionLayerMask;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string explosionStatusId;

        [NonSerialized]
        public StatusData explosionStatus;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion")]
#endif
        public float explosionStatusDuration = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string explosionEffectId;
        [NonSerialized]
        public EffectData explosionEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion")]
#endif
        public float explosionForce = 10f;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion")]
#endif
        public float explosionPlayerForce = 5f;
    }
}

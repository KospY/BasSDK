
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillRevengeIgnition : SkillData
    {
        public static bool loaded = false;

#if ODIN_INSPECTOR
        [BoxGroup("Conditions")]
#endif
        public bool ignoreMagic = true;
#if ODIN_INSPECTOR
        [BoxGroup("Conditions")]
#endif
        public bool meleeOnly = true;
#if ODIN_INSPECTOR
        [BoxGroup("Conditions")]
#endif
        public float triggerMinDamage = 0f;
#if ODIN_INSPECTOR
        [BoxGroup("Ignition"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string burnEffectID = "Burning";

        public StatusData burnEffect;
#if ODIN_INSPECTOR
        [BoxGroup("Ignition")]
#endif
        public float burnTime = 3f;
#if ODIN_INSPECTOR
        [BoxGroup("Ignition")]
#endif
        public float heatAmount = 20f;
#if ODIN_INSPECTOR
        [BoxGroup("Ignition")]
#endif
        public float burstDamage = 5f;
#if ODIN_INSPECTOR
        [BoxGroup("Ignition"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string burstEffectID;

        [NonSerialized]
        public EffectData burstEffectData;
    }
}

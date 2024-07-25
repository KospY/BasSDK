using System.Collections;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillRiptide : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Grab")]
#endif
        public float initialDelay = 0.1f;

#if ODIN_INSPECTOR
        [BoxGroup("Grab")]
#endif
        public bool includeItems = false;

#if ODIN_INSPECTOR
        [BoxGroup("Grab")]
#endif
        public float gripWindow = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Grab"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string tetherEffectId;

        protected EffectData tetherEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Grab"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string grabEffectId;

        protected EffectData grabEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float throwMultiplier = 3f;

#if ODIN_INSPECTOR
        [BoxGroup("Throw"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string throwEffectId;

        protected EffectData throwEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Status"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusId = "Floating";

        protected StatusData statusData;

    }
}
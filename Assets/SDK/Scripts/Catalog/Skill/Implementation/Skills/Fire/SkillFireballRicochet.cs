#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillFireballRicochet : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public string spellId = "Fire";
        protected int spellHashId;
#if ODIN_INSPECTOR
        [BoxGroup("Aim Assist")]
#endif
        public bool aimAssist = true;
#if ODIN_INSPECTOR
        [BoxGroup("Aim Assist"), ShowIf("aimAssist")]
#endif
        public float maxDistance = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Aim Assist"), ShowIf("aimAssist")]
#endif
        public float maxAngle = 40;
#if ODIN_INSPECTOR
        [BoxGroup("Aim Assist"), ShowIf("aimAssist")]
#endif
        public float targetMoveBias = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Ricochet"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string effectId = "SpellFireballRicochet";
        protected EffectData effectData;
    }
}

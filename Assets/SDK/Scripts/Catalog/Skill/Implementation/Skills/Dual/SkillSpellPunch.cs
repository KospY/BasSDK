#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillSpellPunch : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Punch"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string baseImpactEffectId;

        protected EffectData baseImpactEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Punch"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string spellImpactEffectId;

        protected EffectData spellImpactEffectData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Punch"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string flourishEffectId;

        protected EffectData flourishEffectData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Punch")]
#endif
        public float minPunchVelocity = 3;
#if ODIN_INSPECTOR
        [BoxGroup("Punch")]
#endif
        public float cooldown = 0.3f;
#if ODIN_INSPECTOR
        [BoxGroup("Spell"), ValueDropdown(nameof(GetAllSpellID))]
#endif
        public string spellId;
        protected int spellHashId;
        
#if ODIN_INSPECTOR
        [BoxGroup("Status"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusId;

#if ODIN_INSPECTOR
        [BoxGroup("Status")]
#endif
        public float duration = 0;
        
#if ODIN_INSPECTOR
        [BoxGroup("Status")]
#endif
        public float statusFloatParam;
        protected StatusData statusData;

#if ODIN_INSPECTOR
        [BoxGroup("Effects")]
#endif
        public int pushLevel = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Effects")]
#endif
        public float damage = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Effects")]
#endif
        public float extraForce = 0;
        
#if ODIN_INSPECTOR
        [BoxGroup("Effects")]
#endif
        public bool playerKnockbackAirOnly = true;
#if ODIN_INSPECTOR
        [BoxGroup("Effects")]
#endif
        public float playerKnockbackMultiplier = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Effects")]
#endif
        public float playerKnockbackUpMultiplier = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Effects")]
#endif
        public float maxPlayerKnockback = 10;
        
#if ODIN_INSPECTOR
        [BoxGroup("Effects")]
#endif
        public float playerDashForce = 0;

        protected float lastHit;
    }
}

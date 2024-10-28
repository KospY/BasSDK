#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ThunderRoad.Skill.Spell
{
    public class SkillThunderbolt : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt")]
#endif
        public float chainRadius = 4;
#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt")]
#endif
        public float range = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt")]
#endif
        public float angle = 15;
#if ODIN_INSPECTOR
        [FormerlySerializedAs("damage")]
        [BoxGroup("Thunderbolt")]
#endif
        public float damageNpc = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt")]
#endif
        public float damagePlayer = 10;

#if ODIN_INSPECTOR
        [FormerlySerializedAs("allowBounce")]
        [BoxGroup("Thunderbolt")]
#endif
        public bool allowSurfaceBounce = false;

#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt")]
#endif
        public float itemForce = 10f;
#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt")]
#endif
        public float creatureForce = 10f;

#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt")]
#endif
        public float breakForce = 20f;

#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt")]
#endif
        public LayerMask layerMask;

#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string mainBoltEffectId;

        protected EffectData mainBoltEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string impactEffectId;

#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusEffectId;

        protected StatusData statusData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt")]
#endif
        protected float statusDuration;

#if ODIN_INSPECTOR
        [BoxGroup("Thunderbolt")]
#endif
        public Gradient defaultBoltGradient;

        protected EffectData impactEffectData;

    }
}
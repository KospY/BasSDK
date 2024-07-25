#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillGravityHammer : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string whooshEffectId;
        public EffectData whooshEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float minWhooshVelocity = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float maxWhooshVelocity = 12;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float throwMult = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float jointPositionSpring = 1000;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float jointPositionDamper = 150;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float jointPositionMaxForce = 100000;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float jointRotationSpring = 1000;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float jointRotationDamper = 50;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float jointRotationMaxForce = 10000;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float ragdollMassModifier = 8f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float imbueDrainItem = 3;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float imbueDrainRagdoll = 7;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float minDistance = 1f;

        public float minThrowVelocity = 2;

    }

    public class GravityHammer : ThunderBehaviour
    {
        public ThunderEntity heldEntity;
        public ConfigurableJoint joint;
        public SkillGravityHammer skill;
        public bool orgDisallowDespawn;
        public float heldEntitySize;
        public Rigidbody jointPoint;

    }
}

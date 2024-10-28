using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillHeatwave : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string effectId;
        
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusEffectId = "Burning";
        [NonSerialized]
        public StatusData status;
        
#if ODIN_INSPECTOR
        [BoxGroup("Damage")]
#endif
        public float radius = 1.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Damage")]
#endif
        public int pushLevel = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Damage")]
#endif
        public float force = 10f;

#if ODIN_INSPECTOR
        [BoxGroup("Damage")]
#endif
        public float damage = 10f;

#if ODIN_INSPECTOR
        [BoxGroup("Damage")]
#endif
        public float heat = 40f;

#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float punchStartMinMagnitude = 4;
        [NonSerialized]
        public float punchStartMinSqrMagnitude;

#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float punchStopMaxMagnitude = 0.5f;
        [NonSerialized]
        public float punchStopMaxSqrMagnitude;

#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float punchStopWindow = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Detection")]
#endif
        public float punchDelay = 0.3f;
        
        [NonSerialized]
        public EffectData effectData;


    }

    public class HeatwavePuncher : ThunderBehaviour
    {
        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update;

        public SkillHeatwave skill;
        public SpellCastProjectile fire;
        public RagdollHand hand;

        public bool active;

        public float lastPunchingTime;
        public Vector3 lastVelocity;
        private float lastPunch;

    }
}

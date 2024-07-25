using System;
using System.Collections;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace ThunderRoad.Skill.Spell
{
    public class SkillTwinFlame : SpellSkillData
    {
        #if ODIN_INSPECTOR
        [BoxGroup("Skills"), ValueDropdown(nameof(GetAllSkillID))]
        #endif
        public string remoteDetonationId;

        [BoxGroup("Conditions")]
        public float detonateWindow = 0.1f;

        [BoxGroup("Conditions")]
        public float maxRange = 15f;

        #if ODIN_INSPECTOR
        [BoxGroup("Link"), ValueDropdown(nameof(GetAllEffectID))]
        #endif
        public string linkEffectId;

        [NonSerialized]
        public EffectData linkEffectData;
        
        #if ODIN_INSPECTOR
        [BoxGroup("Wall"), ValueDropdown(nameof(GetAllEffectID))]
        #endif
        public string dropEffectId;

        [NonSerialized]
        public EffectData dropEffectData;
        
        #if ODIN_INSPECTOR
        [BoxGroup("Wall"), ValueDropdown(nameof(GetAllEffectID))]
        #endif
        public string wallEffectId;

        [NonSerialized]
        public EffectData wallEffectData;
        #if ODIN_INSPECTOR
        [BoxGroup("Wall"), ValueDropdown(nameof(GetAllStatusEffectID))]
        #endif
        public string statusId;

        [NonSerialized]
        public StatusData status;

        [BoxGroup("Wall")]
        public float thickness = 0.5f;
        
        [BoxGroup("Wall")]
        public float heatRadius = 1f;
        
        [BoxGroup("Wall")]
        public float height = 1f;

        [BoxGroup("Wall")]
        public float heatPerSecond;

        [BoxGroup("Wall")]
        public float duration = 10;

        [BoxGroup("Wall")]
        public bool allowXZMotion = false;
    }

    public class FlameWall : ThunderBehaviour
    {
        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update;
        public StatusData statusData;
        public float statusDuration;
        public float heatPerSecond;
        public bool ignorePlayer = true;
        
    }
}

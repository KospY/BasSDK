using System;
using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using ThunderRoad.Skill.SpellMerge;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThunderRoad.Skill.Spell
{
    public class SkillDilationBubble : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusId;
        
#if ODIN_INSPECTOR
        [BoxGroup("Velocity")]
#endif
        public float storedVelocityMult = 1.5f;

    }

    public class VelocityStorer : ThunderBehaviour
    {
        public Item item;
        public Vector3 velocity;
        public float multiplier = 1;
        public bool active;
        public bool waitingForThrow;

    }
}
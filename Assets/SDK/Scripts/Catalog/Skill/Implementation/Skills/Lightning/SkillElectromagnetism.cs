#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using ThunderRoad.Skill.SpellPower;
using UnityEngine;
using UnityEngine.UIElements;

namespace ThunderRoad.Skill.Spell
{
    public class SkillElectromagnetism : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Homing")]
#endif
        public float lerpAmount = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Homing")]
#endif
        public float homingForce = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Homing")]
#endif
        public float homingAngle = 15f;
#if ODIN_INSPECTOR
        [BoxGroup("Homing")]
#endif
        public float homingDistance = 5f;
#if ODIN_INSPECTOR
        [BoxGroup("Homing")]
#endif
        public float lerpAmountNpc = 10;

 //ProjectCore
    }

    public class Electromagnet : ThunderBehaviour
    {
        public override ManagedLoops EnabledManagedLoops => ManagedLoops.FixedUpdate | ManagedLoops.Update;
        public Item item;
        public bool attracting;
        public Transform target;
        public SkillElectromagnetism skill;
        public float lastThrow;
        public bool ensureLightningImbue = true;

    }
}
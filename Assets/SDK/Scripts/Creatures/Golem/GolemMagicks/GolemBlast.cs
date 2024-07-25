using System;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Creatures/Golem/Blast config")]
    public class GolemBlast : GolemAbility
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("General")]
#endif
        public GolemController.AttackMotion motion;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Placement")]
#endif
        public Vector3 blastLocalPosition;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Placement")]
#endif
        public bool attachToBone = false;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Placement")]
#endif
        public HumanBodyBones blastLinkedBone = HumanBodyBones.UpperChest;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Placement")]
#endif
        public float blastRadius = 5f;
#if ODIN_INSPECTOR && UNITY_EDITOR
        protected List<ValueDropdownItem<string>> GetAllEffectID() => Catalog.GetDropdownAllID(Category.Effect);

        [BoxGroup("Blast Effects"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string blastEffectID = "";
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Effects")]
#endif
        public Vector3 effectEulers = Vector3.zero;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Damage")]
#endif
        public bool kickPlayerOff = false;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Damage")]
#endif
        public bool damageBreakables = false;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Damage")]
#endif
        public LayerMask blastMask;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Damage")]
#endif
        public float blastDamage = 5f;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Damage")]
#endif
        public float blastForce = 5f;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Damage")]
#endif
        public float blastForceUpwardMult = 1.5f;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Damage")]
#endif
        public ForceMode blastForceMode = ForceMode.VelocityChange;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Blast Damage")]
#endif
        public List<Golem.InflictedStatus> appliedStatuses = new();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [InlineProperty, HideLabel]
        [ShowIf("$type", Value = GolemAbilityType.Melee)]
#endif
        public Golem.AttackRange attackRange;

    }
}

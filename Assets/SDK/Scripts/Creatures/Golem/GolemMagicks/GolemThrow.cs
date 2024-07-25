using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Creatures/Golem/Throw config")]
    public class GolemThrow : GolemAbility
    {
        public static float lastThrowTime;
#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [BoxGroup("Thrown Object")]
        [ValueDropdown(nameof(GetAllEffectID))]
#endif
#endif
        public string summonEffectID;
#if ODIN_INSPECTOR
        private List<ValueDropdownItem<string>> GetAllItemID() => Catalog.GetDropdownAllID(Category.Item);
        private List<ValueDropdownItem<string>> GetAllEffectID() => Catalog.GetDropdownAllID(Category.Effect);

        [BoxGroup("Thrown Object")]
        [ValueDropdown(nameof(GetAllItemID))]
#endif
        public string throwObjectID;
#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [BoxGroup("Thrown Object")]
        [ValueDropdown(nameof(GetAllEffectID))]
#endif
#endif
        public string objectEffectID;
#if ODIN_INSPECTOR
        [BoxGroup("Thrown Object")]
#endif
        public AnimationCurve objectEffectIntensityCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public LayerMask objectSpawnRaycastMask;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float throwVelocity = 5f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float throwCooldownDuration = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float throwMaxDistance = 50;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float throwMaxAngle = 30;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float gravityMultiplier = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public Side grabArmSide = Side.Right;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public Vector3 holdPosition = Vector3.zero;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float holdForce;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float holdDamper;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion")]
#endif
        public float explosionRadius = 10f;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion")]
#endif
        public float explosionDamage = 20;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Explosion")]
#endif
        public List<Golem.InflictedStatus> appliedStatuses = new();
#if ODIN_INSPECTOR
        [BoxGroup("Explosion")]
#endif
        public float explosionForce = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion")]
#endif
        public ForceMode forceMode = ForceMode.Impulse;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion")]
#endif
        public float upwardForceMult = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Explosion")]
#endif
        public LayerMask explosionLayerMask;

#if ODIN_INSPECTOR
        [BoxGroup("Explosion")]
        #if UNITY_EDITOR
        [ValueDropdown(nameof(GetAllEffectID))]
        #endif
#endif
        public string explosionEffectID;

    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/SimpleBreakable.html")]
    public class SimpleBreakable : MonoBehaviour
    {
        [Header("Damage")]
        public DamageType allowedDamageTypes = DamageType.ArticulationOrRigidbody | DamageType.Static | DamageType.Scripts;
        public float maxHealth = 30f;
        [Tooltip("Minimum delay between collisions being registered.")]
        public float collisionDelay = 0.1f;
        [Tooltip("Minimum velocity to deal damage to a crystal.")]
        public float minHitVelocity = 5f;
        [Tooltip("Minimum mass for a rigidbody to deal damage on collision.")]
        public float minHitMass = 0f;
        public List<DamageOverride> damageOverrides = new List<DamageOverride>();
        
        [Header("Damage Fallback")]
        public AnimationCurve velocityCurve = AnimationCurve.Linear(5f, 1f, 15f, 2f);
        public AnimationCurve massCurve = AnimationCurve.Linear(0f, 1f, 5f, 2f);
        public AnimationCurve damageCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("Events")]
        public UnityEvent onRestore;
        public UnityEvent<float> onDamage;
        public UnityEvent onBreak;

        [NonSerialized, ShowInInspector]
        public float health;
        private bool isBroken;
        private float lastCollision;

        [System.Flags]
        public enum DamageType
        {
            None = 0,
            ArticulationOrRigidbody = 1,
            Static = 2,
            ParticleHit = 4,
            Scripts = 8,
        }

        // Do not add things in the middle of this enum, only at the end
        //  (it will break serialization on some unity objects)
        public enum DamageSource
        {
            Unknown,
            Ragdoll,
            Fireball,
            ThrownWeapon,
            HeldWeapon,
            Item,
            Thunderbolt
        }

        [Serializable]
        public struct DamageOverride
        {
            [Tooltip("Type of damage this override will apply to")]
            public DamageSource source;
            [Tooltip("Base amount of damage done by this damage type")]
            public float damage;
            [Tooltip("Multiply damage by this curve, evaluated by the impact velocity of the collision")]
            public AnimationCurve velocityCurve;
            [Tooltip("Multiply damage by this curve, evaluated by the mass of the colliding object")]
            public AnimationCurve massCurve;

            public DamageOverride(DamageSource source, float damage, AnimationCurve velocityCurve, AnimationCurve massCurve)
            {
                this.source = source;
                this.damage = damage;
                this.velocityCurve = velocityCurve;
                this.massCurve = massCurve;
            }
        }

    }
}
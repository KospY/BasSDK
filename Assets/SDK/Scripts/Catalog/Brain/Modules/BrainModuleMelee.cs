using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using ThunderRoad.AI;
using ThunderRoad.AI.Action;
using UnityEngine.AI;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModuleMelee : BrainModuleAttack
    {
        public static float parryMinVelocity = 2f;
        public bool meleeEnabled = true;
        public float attackMaxAngle = 45;
        //public float tooCloseForAttack = 0.6f;
        public float attackTurnSpeedMultiplier = 1;
        public float maxRangeDelta = 0.25f;

        [Header("Recoil and riposte")]
        public bool damageToCancelIsPercentValue = true;
        public float minimumDamageToCancelAttack = 5f;
        public float recoilMinWeaponSpeed = 1;
#if ODIN_INSPECTOR
        [MinMaxSlider(0f, 1f, true)] 
#endif
        public Vector2 recoilNothingRiposteChance = new Vector2(0.25f, 0.75f);
        [Tooltip("This value gets added to the above recoil-nothing-riposte chance, multiplied by the number of consecutive ripostes. This changes the chance to recoil, and chance to riposte.")]
        public Vector2 recoilNothingRiposteModifier = new Vector2(0f, 0f);
        public Vector2 minMaxRiposteDelay = new Vector2(0.75f, 1.5f);
        public float riposteSpeedAdditiveModifier = -0.05f;
        public float physicCullBlockMinimum = 0.1f;

        [Header("Defense settings")]
        public float damagedIgnoreDefenseWindow = 0.75f;

        [Header("Grouping")]
        public int meleeMax = 3;
        public float meleeSafeDistance = 3f;

        // Weapon is rarely totally horizontal when striking
        public float weaponReachRatio = 0.75f;

        public bool updateClipRange = true;

        [Header("Forces")]
        public float armSpringMultiplier = 2.0f;
        public float armDamperMultiplier = 0;
        public float armMaxForceMultiplier = 10.0f;
#if ODIN_INSPECTOR
        [MinMaxSlider(0f, 100f, true)] 
#endif
        public Vector2 minMaxPhysicCullWeaponSpeed = new Vector2(2.5f, 25f);
        public int immediatePushFrameDelay = 2;
        public float maxPushItemMass = 2.99f;
        public float shieldPushMassMult = 1.5f;

        [Header("Animation")]
        public float animationSpeedMultiplier = 1;
        [Range(0f, 1f)]
        public float minimumWeaponMassMultiplier = 0.7f;

        public enum HitType { None, Object, Weapon, Shield, Body }

        [Header("Instance")]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public float lastAttackStartTime;
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly] 
#endif
        [NonSerialized]
        public float lastAttackEndTime;
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly] 
#endif
        [NonSerialized]
        public float nextAttackDelay;
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly] 
#endif
        [NonSerialized]
        public HitType lastHitType;
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly] 
#endif
        [NonSerialized]
        public float lastRiposteTime;
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly] 
#endif
        [NonSerialized]
        public float nextRiposteDelay;
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly] 
#endif
        [NonSerialized]
        public int consecutiveRipostes;

#if ODIN_INSPECTOR
        [LabelText("(Debug) Consecutive parries"), ShowInInspector, PropertyOrder(0), PropertyRange(0, 10)]
        private int debugConsecutive
        {
            get
            {
                debugChanceVisual = new Vector2(Mathf.Clamp01(recoilNothingRiposteChance.x + (_debugConsecutive * recoilNothingRiposteModifier.x)), Mathf.Clamp01(recoilNothingRiposteChance.y + (_debugConsecutive * recoilNothingRiposteModifier.y)));
                return _debugConsecutive;
            }
            set
            {
                debugChanceVisual = new Vector2(Mathf.Clamp01(recoilNothingRiposteChance.x + ((value - 1) * recoilNothingRiposteModifier.x)), Mathf.Clamp01(recoilNothingRiposteChance.y + ((value - 1) * recoilNothingRiposteModifier.y)));
                if (value < 0) _debugConsecutive = 0;
                if (debugChanceVisual.y < 1) _debugConsecutive = value;
                debugChanceVisual = new Vector2(Mathf.Clamp01(recoilNothingRiposteChance.x + (_debugConsecutive * recoilNothingRiposteModifier.x)), Mathf.Clamp01(recoilNothingRiposteChance.y + (_debugConsecutive * recoilNothingRiposteModifier.y)));
            }
        }
        private int _debugConsecutive = 0;
        [LabelText("(Debug) Recoil/Nothing/Riposte Chance Visual"), MinMaxSlider(0f, 1f, true), ShowInInspector, PropertyOrder(1), ReadOnly]
        private Vector2 debugChanceVisual = new Vector2();
#endif

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllAnimationID()
        {
            return Catalog.GetDropdownAllID(Category.Animation);
        } 
#endif

    }
}
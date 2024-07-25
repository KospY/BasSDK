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
	[Serializable]
    public class DamagerData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("General"), ValueDropdown(nameof(GetAllDamagerModifierID))] 
#endif
        public string damageModifierId;
        [NonSerialized]
        public DamageModifierData damageModifierData;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllDamagerModifierID()
        {
            return Catalog.GetDropdownAllID(Category.DamageModifier);
        }

        public List<ValueDropdownItem<string>> GetAllMaterialID()
        {
            return Catalog.GetDropdownAllID(Category.Material);
        } 
#endif

#if ODIN_INSPECTOR
        [TableList(ShowIndexLabels = true)] 
#endif
        public Tier[] tiers;

        [Serializable]
        public class Tier
        {
#if ODIN_INSPECTOR
            [HorizontalGroup("Damage"), LabelText("Mult."), LabelWidth(40)] 
#endif
            public float damageMultiplier = 1;
#if ODIN_INSPECTOR
            [HorizontalGroup("Damage"), LabelText("Player Mult."), LabelWidth(40)]
#endif
            public float playerDamageMultiplier = 1;
#if ODIN_INSPECTOR
            [HorizontalGroup("Damage"), LabelText("V. angle"), LabelWidth(50)] 
#endif
            public float maxVerticalAngle = 80f;
#if ODIN_INSPECTOR
            [HorizontalGroup("Damage"), LabelText("H. angle"), LabelWidth(50)] 
#endif
            public float maxHorizontalAngle = 60f;
#if ODIN_INSPECTOR
            [HorizontalGroup("Damage"), LabelText("Norm. angle"), LabelWidth(70)] 
#endif
            public float maxNormalAngle = 60;
#if ODIN_INSPECTOR
            [HorizontalGroup("Damage"), LabelText("Grip disable chance"), LabelWidth(100)] 
#endif
            public float gripDisableChance = 0f;

#if ODIN_INSPECTOR
            [HorizontalGroup("Dismemberment"), LabelText("Velocity Mult."), LabelWidth(90)] 
#endif
            public float dismembermentMinVelocityMultiplier = 1f;
#if ODIN_INSPECTOR
            [HorizontalGroup("Dismemberment"), LabelText("V. angle"), LabelWidth(50)] 
#endif
            public float dismembermentMaxVerticalAngle = 30;
#if ODIN_INSPECTOR
            [HorizontalGroup("Dismemberment"), LabelText("H. angle"), LabelWidth(50)] 
#endif
            public float dismembermentMaxHorizontalAngle = 30;

        }

        public override int GetCurrentVersion()
        {
            return 1;
        }


#if ODIN_INSPECTOR
        [HorizontalGroup("Split")]
        [VerticalGroup("Split/Left")]
        [BoxGroup("Split/Left/Damage")] 
#endif
        public AnimationCurve velocityDamageCurve = new AnimationCurve(new Keyframe(2, 1), new Keyframe(12, 8));
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Damage")] 
#endif
        public float minSelfVelocity = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Damage")] 
#endif
        public float intensityMinVelocity;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Damage")] 
#endif
        public float intensityMaxVelocity;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Damage")] 
#endif
        public float hitDelayByCollider = 0.1f;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Damage")] 
#endif
        public float playerMinDamage = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Damage")] 
#endif
        public float playerMaxDamage = 15;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Damage")] 
#endif
        public bool selfDamage;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Damage"), ShowIf("selfDamage")] 
#endif
        public AnimationCurve staticVelocityDamageCurve;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Damage"), HideIf("selfDamage")] 
#endif
        public float throwedMultiplier = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Damage")]
#endif
        public bool handleDamager = false;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Bad angle")] 
#endif
        public bool badAngleBluntFallback;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Bad angle"), ShowIf("badAngleBluntFallback")] 
#endif
        public float badAngleDamage = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Bad angle"), ShowIf("badAngleBluntFallback")] 
#endif
        public float badAngleRecoilMultiplier;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Bad angle"), ValueDropdown(nameof(GetAllDamagerModifierID)), ShowIf("badAngleBluntFallback")] 
#endif
        public string badAngleMaterialDamageId;
        [NonSerialized]
        public DamageModifierData badAngleModifierData;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Animation")] 
#endif
        public float dyingAnimationMaxVelocity = 8;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Force")] 
#endif
        public float addForce = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Force"), ShowIf("@this.addForce > 0")] 
#endif
        public ObjectState addForceState = ObjectState.Handled;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Force"), ShowIf("@this.addForce > 0")] 
#endif
        public TargetType addForceTargetType = TargetType.All;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Force"), ShowIf("@this.addForce > 0")] 
#endif
        public ForceMode addForceMode = ForceMode.Impulse;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Force"), ShowIf("@this.addForce > 0")] 
#endif
        public float addForceDuration = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Force"), ShowIf("@this.addForce > 0")] 
#endif
        public float addForceRagdollPartMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Force"), ShowIf("@this.addForce > 0")] 
#endif
        public float addForceRagdollOtherMultiplier = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Force"), ShowIf("@this.addForce > 0")] 
#endif
        public float addForceSlowMoMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Left/Force"), ShowIf("@this.addForce > 0")] 
#endif
        public bool addForceNormalize = false;

#if ODIN_INSPECTOR
        [VerticalGroup("Split/Right")]
        [BoxGroup("Split/Right/Penetration")] 
#endif
        public bool penetrationAllowed;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed")] 
#endif
        public float penetrationDeepDepthMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed")] 
#endif
        public float penetrationDamage = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed")] 
#endif
        public bool penetrationEffect = true;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed")] 
#endif
        public float penetrationInitialVelocityMultiplier = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed")] 
#endif
        public bool penetrationSkewerDetection = false;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed"), ShowIf("penetrationSkewerDetection")] 
#endif
        public float penetrationSkewerDamage = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed")] 
#endif
        public float penetrationHeldDamperIn = 800;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed")] 
#endif
        public float penetrationHeldDamperOut = 1000;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed")] 
#endif
        public float penetrationDamper = 5000;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed")] 
#endif

        public float penetrationShortDepth = 0f;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed")] 
#endif
        public float penetrationShortDepthAngle = 30;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed")] 
#endif
        public bool penetrationAllowSlide = false;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration"), ShowIf("penetrationAllowed"), ShowIf("penetrationAllowSlide")] 
#endif
        public float penetrationSlideDamper = 100;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration Temporary Modifier"), ShowIf("penetrationAllowed")] 
#endif
        public PenetrationTempModifier penetrationTempModifier = PenetrationTempModifier.OnThrow;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration Temporary Modifier"), ShowIf("penetrationAllowed"), ShowIf("@((int)this.penetrationTempModifier) > 0")] 
#endif
        public float penetrationTempModifierDuration = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration Temporary Modifier"), ShowIf("penetrationAllowed"), ShowIf("@((int)this.penetrationTempModifier) > 0")] 
#endif
        public float penetrationTempModifierDamperIn = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Penetration Temporary Modifier"), ShowIf("penetrationAllowed"), ShowIf("@((int)this.penetrationTempModifier) > 0")] 
#endif
        public float penetrationTempModifierDamperOut = 5000;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Pressure")] 
#endif
        public bool penetrationPressureAllowed = false;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Pressure"), ShowIf("penetrationPressureAllowed"), ShowIf("@this.penetrationAllowed && this.penetrationPressureAllowed")] 
#endif
        public AnimationCurve penetrationPressureForceCurve = new AnimationCurve(new Keyframe(0, 5), new Keyframe(2, 0.5f));
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Pressure"), ShowIf("penetrationPressureAllowed"), ShowIf("@this.penetrationAllowed && this.penetrationPressureAllowed")] 
#endif
        public float penetrationPressureMaxDot = 0.2f;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Dismemberment")] 
#endif
        public bool dismembermentAllowed = false;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Dismemberment"), ShowIf("dismembermentAllowed"), ShowIf("@this.penetrationAllowed && this.dismembermentAllowed")] 
#endif
        public float dismembermentMinVelocity = 10f;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Right/Dismemberment"), ShowIf("dismembermentAllowed"), ShowIf("@this.penetrationAllowed && this.dismembermentAllowed")] 
#endif
        public float dismembermentNoPenetrationDuration = 1f;

        [Flags]
        public enum PenetrationTempModifier
        {
            OnHit = 1,
            OnThrow = 2,
        }

        public enum KnockoutCondition
        {
            Disabled,
            BluntOnly,
            Always,
        }

        public enum TargetType
        {
            Object,
            Creature,
            All,
        }

        public enum ObjectState
        {
            Flying,
            Handled,
            All,
        }

    }
}


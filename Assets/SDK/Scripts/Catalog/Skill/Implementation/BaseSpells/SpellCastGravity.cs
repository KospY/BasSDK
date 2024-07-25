using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using ThunderRoad.Skill.SpellMerge;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SpellCastGravity : SpellCastCharge
    {
        public const string LastPushTimeVariable = "LastPushTime";
        public const string LastPushDirection = "LastPushDirection";

#if ODIN_INSPECTOR
        [BoxGroup("Misc")]
#endif
        public bool enableCreatureControl = false;

#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float halfSphereRadius = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Throw"), MinMaxSlider(0, 4f, true)]
#endif
        public Vector2 pushLevel = new(1, 3);
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float pushMaxForce = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float pushForceOnPlayer = 0.3f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float upwardsForceLerpOnPlayer = 0.3f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float playerLocomotionDeactivate = 0.2f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public bool pushUseExplosionForce = true;
#if ODIN_INSPECTOR
        [BoxGroup("Throw"), ShowIf("pushUseExplosionForce")]
#endif
        public float pushExplosionUpwardModifier = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public ForceMode pushForceMode = ForceMode.Impulse;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public LayerMask pushLayerMask;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float minChargeToPushCreature = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw"), LabelText(@"@SkillPassiveLabel(""Push Force"", intensityPerSkill)")]
#endif
        public float intensityPerSkill = 0.05f;

#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float pushStatusExtension = 3f;

#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float pushStatusForce = 1.3f;

#if ODIN_INSPECTOR
        [BoxGroup("Dual Push")]
#endif
        public float dualPushIntensityMultiplier = 0.4f;

#if ODIN_INSPECTOR
        [BoxGroup("Dual Push")]
#endif
        public float dualPushTimeWindow = 0.3f;

#if ODIN_INSPECTOR
        [BoxGroup("Dual Push")]
#endif
        public float dualPushAngleWindow = 50;

#if ODIN_INSPECTOR
        [BoxGroup("Dual Push"), Range(0, 3)]
#endif
        public float dualPushLevelReduction = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Dual Push"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string dualPushEffectId;
        protected EffectData dualPushEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Throw"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string pushEffectId;
        protected EffectData pushEffectData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Push Status Effect"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string pushStatusEffectId = "Floating";
        
        [NonSerialized]
        public StatusData pushStatusEffect;
#if ODIN_INSPECTOR
        [BoxGroup("Push Status Effect")]
#endif
        public float pushStatusDuration = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueGravityRatio = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueDrag = -1;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueAngularDrag = -1;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueGravityFalloffThreshold = 0.3f;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string imbueHitEffectId;
        protected EffectData imbueHitEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string imbueHitItemEffectId;
        protected EffectData imbueHitItemEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string imbueHitRagdollEffectId;
        protected EffectData imbueHitRagdollEffectData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string imbueStatusEffectId = "Floating";
        
        [NonSerialized]
        public StatusData imbueStatusEffect;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit")]
#endif
        public float imbueHitItemZeroGravityDuration = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit")]
#endif
        public float imbueHitRagdollZeroGravityDuration = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit")]
#endif
        public float imbueHitGravityRatio = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit")]
#endif
        public float imbueHitMassRatio = 0.5f;


#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public int staffSlamPushLevel = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamExplosionUpwardModifier = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamWaveSpeed = 12f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamWaveUpdateRate = 0.05f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamMinRadius = 2f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamMaxRadius = 8f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamMinForce = 10f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamMaxForce = 15f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public ForceMode staffSlamForceMode = ForceMode.VelocityChange;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamStatusDuration = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public EffectOnPlayer staffSlamPlayerEffect = EffectOnPlayer.PushWhenJumping;

        [NonSerialized]
        protected float lastImbueFalloffAmount;
        [NonSerialized]
        public GameObject hoverEffectObj;
        [NonSerialized]
        public float hoverIntensity;
        
        public enum EffectOnPlayer {
            None,
            PushWhenJumping,
            PushAlways
        }


        public new SpellCastGravity Clone()
        {
            return MemberwiseClone() as SpellCastGravity;
        }

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            pushEffectData = Catalog.GetData<EffectData>(pushEffectId);
            pushStatusEffect = Catalog.GetData<StatusData>(pushStatusEffectId);
            dualPushEffectData = Catalog.GetData<EffectData>(dualPushEffectId);
            imbueStatusEffect = Catalog.GetData<StatusData>(imbueStatusEffectId);
            imbueHitEffectData = Catalog.GetData<EffectData>(imbueHitEffectId);
            imbueHitItemEffectData = Catalog.GetData<EffectData>(imbueHitItemEffectId);
            imbueHitRagdollEffectData = Catalog.GetData<EffectData>(imbueHitRagdollEffectId);
        }

    }
}
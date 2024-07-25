using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.Mathematics;
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
	public class SpellCastProjectile : SpellCastCharge
    {
#if ODIN_INSPECTOR
        [BoxGroup("Grip Cast")]
#endif
        public float gripCastHeatTransfer = 40;
#if ODIN_INSPECTOR
        [BoxGroup("Cast Heat Zone")]
#endif
        public bool allowCastHeat = true;
#if ODIN_INSPECTOR
        [BoxGroup("Cast Heat Zone"), ShowIf(nameof(allowCastHeat))]
#endif
        public float spellCastHeatPerSecond = 20;
        
#if ODIN_INSPECTOR
        [BoxGroup("Cast Heat Zone"), ShowIf(nameof(allowCastHeat))]
#endif
        public float castHeatRadius = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Projectile"), LabelText(@"@SkillPassiveLabel(""Damage"", intensityPerSkill)")]
#endif
        public float intensityPerSkill = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Projectile"), LabelText(@"@SkillPassiveLabel(""Burn Duration"", durationPerSkill)")]
#endif
        public float durationPerSkill = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Projectile")]
#endif
        public bool destroyInWater = false;
#if ODIN_INSPECTOR
        [BoxGroup("Projectile"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string projectileEffectId;
        [NonSerialized]
        public EffectData projectileEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Projectile"), ValueDropdown(nameof(GetAllItemID))] 
#endif
        public string projectileId;
        [NonSerialized]
        public ItemData projectileData;

#if ODIN_INSPECTOR
        [BoxGroup("Projectile"), ValueDropdown(nameof(GetAllDamagerID))] 
#endif
        public string projectileDamagerId;
        [NonSerialized]
        public DamagerData projectileDamagerData;

#if ODIN_INSPECTOR
        [BoxGroup("Projectile")] 
#endif
        public readonly AnimationCurve damageOverTimeCurve = AnimationCurve.Linear(1, 1, 1, 1);


#if ODIN_INSPECTOR
        [BoxGroup("Projectile")] 
#endif
        public float projectileImbueEnergyTransferred = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Projectile"), UnityEngine.Range(0f, 1f)] 
#endif
        public float throwHeadBias = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Projectile")] 
#endif
        public float projectileVelocity = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Projectile")] 
#endif
        public bool projectilePlayerGuided = true;
#if ODIN_INSPECTOR
        [BoxGroup("Projectile")] 
#endif
        public float projectileGuidanceDelay = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Projectile")] 
#endif
        public bool projectileAllowDeflect = true;
#if ODIN_INSPECTOR
        [BoxGroup("Projectile"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string projectileDeflectEffectId;
        [NonSerialized]
        public EffectData projectileDeflectEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Fire Sparks"), ValueDropdown(nameof(GetAllStatusEffectID))] 
#endif
        public string fireSparkStatusId = "Burning";
        [NonSerialized]
        public StatusData fireSparkStatusEffect;
        
#if ODIN_INSPECTOR
        [BoxGroup("Fire Sparks")] 
#endif
        public float fireSparkStatusDuration = 5;
        
#if ODIN_INSPECTOR
        [BoxGroup("Fire Sparks")] 
#endif
        public float fireSparkHeatTransfer = 10;
        
#if ODIN_INSPECTOR
        [BoxGroup("Imbue"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string imbueHitStatusEffectId = "Burning";
        [NonSerialized]
        protected StatusData imbueHitStatusEffect;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueHitStatusDuration = Mathf.Infinity;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueHitHeat = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbuePenetrateHeatPerSecond = 10;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue use")] 
#endif
        public float imbueUseConsumption = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue use"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string imbueUseEffectId;
        [NonSerialized]
        public EffectData imbueUseEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue use"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string imbueUseProjectileEffectId;
        [NonSerialized]
        public EffectData imbueUseProjectileEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue use")] 
#endif
        public FireMode imbueShootFireMode = FireMode.InheritVelocity;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue use")] 
#endif
        public TriggerType imbueShootTriggerMode = TriggerType.ButtonUp;

#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string imbueHitProjectileEffectId;
        [NonSerialized]
        public EffectData imbueHitProjectileEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")] 
#endif
        protected float staffSlamFireballSpeed = 15f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")] 
#endif
        public int staffSlamNumFireballs = 6;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")] 
#endif
        public float slamOnCrystalDistance = 1f;
        
        protected HashSet<Creature> creaturesHeatedThisFrame;

        public new SpellCastProjectile Clone()
        {
            return this.MemberwiseClone() as SpellCastProjectile;
        }

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();

            projectileData = Catalog.GetData<ItemData>(projectileId);
            projectileDamagerData = Catalog.GetData<DamagerData>(projectileDamagerId);
            projectileEffectData = Catalog.GetData<EffectData>(projectileEffectId);
            projectileDeflectEffectData = Catalog.GetData<EffectData>(projectileDeflectEffectId);
            imbueHitStatusEffect = Catalog.GetData<StatusData>(imbueHitStatusEffectId);
            fireSparkStatusEffect = Catalog.GetData<StatusData>(fireSparkStatusId);
            imbueUseEffectData = Catalog.GetData<EffectData>(imbueUseEffectId);
            imbueHitProjectileEffectData = Catalog.GetData<EffectData>(imbueHitProjectileEffectId);
            imbueUseProjectileEffectData = Catalog.GetData<EffectData>(imbueUseProjectileEffectId);

        }

        public enum FireMode
        {
            Forwards,
            InheritVelocity
        }

        public enum TriggerType
        {
            ButtonDown,
            ButtonUp
        }

    }
}

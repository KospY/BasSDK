using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public class SpellCastProjectile : SpellCastCharge
    {
#if ODIN_INSPECTOR
        [BoxGroup("Projectile"), ValueDropdown("GetAllEffectID")] 
#endif
        public string projectileEffectId;
        [NonSerialized]
        public EffectData projectileEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Projectile"), ValueDropdown("GetAllItemID")] 
#endif
        public string projectileId;
        [NonSerialized]
        public ItemData projectileData;

#if ODIN_INSPECTOR
        [BoxGroup("Projectile"), ValueDropdown("GetAllDamagerID")] 
#endif
        public string projectileDamagerId;
        [NonSerialized]
        public DamagerData projectileDamagerData;

#if ODIN_INSPECTOR
        [BoxGroup("Projectile")] 
#endif
        public float projectileImbueEnergyTransfered = 0;
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
        public bool projectileAllowDeflect = true;
#if ODIN_INSPECTOR
        [BoxGroup("Projectile"), ValueDropdown("GetAllEffectID")] 
#endif
        public string projectileDeflectEffectId;
        [NonSerialized]
        public EffectData projectileDeflectEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue use")] 
#endif
        public float imbueUseConsumption = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue use"), ValueDropdown("GetAllEffectID")] 
#endif
        public string imbueUseEffectId;
        [NonSerialized]
        public EffectData imbueUseEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue use"), ValueDropdown("GetAllEffectID")] 
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
        [BoxGroup("Staff Slam"), ValueDropdown("GetAllEffectID")] 
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
        [BoxGroup("Power Up")] 
#endif
        public bool allowPowerUp;
#if ODIN_INSPECTOR
        [BoxGroup("Power Up"), ShowIf("allowPowerUp")] 
#endif
        public float powerUpChargeTime = 4f;
#if ODIN_INSPECTOR
        [BoxGroup("Power Up"), ShowIf("allowPowerUp")] 
#endif
        public int powerUpNumFireballs = 2;


#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllDamagerID()
        {
            return Catalog.GetDropdownAllID(Category.Damager);
        }

        public List<ValueDropdownItem<string>> GetAllItemID()
        {
            return Catalog.GetDropdownAllID(Category.Item);
        } 
#endif

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

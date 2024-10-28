using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad.Skill.Spell;
using UnityEngine;
using Random = UnityEngine.Random;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill.SpellMerge
{
	[Serializable]
    public class SpellMergeFire : SpellMergeData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Meteor"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string meteorEffectId;

        protected EffectData meteorEffectData;
        [NonSerialized]
        public EffectData overrideMeteorEffect;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string meteorExplosionEffectId;

        protected EffectData meteorExplosionEffectData;
        [NonSerialized]
        public EffectData overrideExplosionEffect;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor"), ValueDropdown(nameof(GetAllItemID))] 
#endif
        public string meteorItemId;

        protected ItemData meteorItemData;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor")] 
#endif
        public float meteorVelocity = 7;
        
#if ODIN_INSPECTOR
        [BoxGroup("Meteor")] 
#endif
        public bool meteorUseGravity = true;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor")] 
#endif
        public float meteorExplosionForce = 20;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor")] 
#endif
        public float meteorExplosionPlayerForce = 5f;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor")] 
#endif
        public float meteorExplosionDamage = 20;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor")] 
#endif
        public float meteorExplosionPlayerDamage = 20;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor")] 
#endif
        public float meteorExplosionRadius = 10;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor")] 
#endif
        public LayerMask explosionLayerMask;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor"), ValueDropdown(nameof(GetAllSpellID))] 
#endif
        public string meteorImbueSpellId;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor")] 
#endif
        public AnimationCurve meteorIntensityCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);
        
        #if ODIN_INSPECTOR
        [BoxGroup("Status Effect"), ValueDropdown(nameof(GetAllStatusEffectID))]
        #endif
        public string statusEffectId;
        [NonSerialized]
        public StatusData statusEffect;
        
        [BoxGroup("Status Effect")]
        public float statusDuration = 5;

        protected SpellCastProjectile fire;

        public delegate void ExplodeEvent(SpellMergeFire fire, Vector3 position, float radius, EventTime time);
        public event ExplodeEvent OnExplodeEvent;

        protected SpellCastCharge meteorImbueSpellData;

        private HashSet<Creature> hitCreatures;
        

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            meteorEffectData = Catalog.GetData<EffectData>(meteorEffectId);
            meteorExplosionEffectData = Catalog.GetData<EffectData>(meteorExplosionEffectId);
            meteorItemData = Catalog.GetData<ItemData>(meteorItemId);
            meteorImbueSpellData = Catalog.GetData<SpellCastCharge>(meteorImbueSpellId);
            statusEffect = Catalog.GetData<StatusData>(statusEffectId);
        }

    }
}

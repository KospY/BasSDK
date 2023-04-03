using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class SpellMergeFire : SpellMergeData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Meteor"), ValueDropdown("GetAllEffectID")] 
#endif
        public string meteorEffectId;

        protected EffectData meteorEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor"), ValueDropdown("GetAllEffectID")] 
#endif
        public string meteorExplosionEffectId;

        protected EffectData meteorExplosionEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor"), ValueDropdown("GetAllItemID")] 
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
        [BoxGroup("Meteor"), ValueDropdown("GetAllSpellID")] 
#endif
        public string meteorImbueSpellId;

#if ODIN_INSPECTOR
        [BoxGroup("Meteor")] 
#endif
        public AnimationCurve meteorIntensityCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);

        protected SpellCastCharge meteorImbueSpellData;

        private HashSet<Creature> hitCreatures;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllItemID() { return Catalog.GetDropdownAllID(Category.Item); } 
#endif


        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            meteorEffectData = Catalog.GetData<EffectData>(meteorEffectId);
            meteorExplosionEffectData = Catalog.GetData<EffectData>(meteorExplosionEffectId);
            meteorItemData = Catalog.GetData<ItemData>(meteorItemId);
            meteorImbueSpellData = Catalog.GetData<SpellCastCharge>(meteorImbueSpellId);
        }

    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class ColliderGroupData : CatalogData
    {
        public CollisionHandling collisionHandling = CollisionHandling.ByGroup;
        public bool allowPenetration = true;

        public List<Modifier> modifiers = new List<Modifier>();

        [FormerlySerializedAs("allowedImbueEffectModules")]
#if ODIN_INSPECTOR
        [ShowInInspector, ValueDropdown(nameof(GetAllEffectModuleTypes))]
#endif
        public Type[] ignoredImbueEffectModules;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<Type>> GetAllEffectModuleTypes()
        {
            var dropdownList = new List<ValueDropdownItem<Type>>();
            foreach (var type in typeof(EffectModule).Assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(EffectModule)))
                    dropdownList.Add(new ValueDropdownItem<Type>(type.Name, type));
            }

            return dropdownList;
        }
#endif

        public bool customSpellEffects = false;
#if ODIN_INSPECTOR
        [ShowInInspector, ShowIf("customSpellEffects")]
#endif
        public List<CustomSpellEffect> customSpellEffectIDs;
#if ODIN_INSPECTOR
        [ShowIf("customSpellEffects")]
#endif
        public Dictionary<int, EffectData> customSpellEffectData;
#if ODIN_INSPECTOR
        [ShowIf("customSpellEffects")]
#endif
        public bool blockPoolSteal = false;

        public enum ImbueType
        {
            None,
            Metal,
            Blade,
            Crystal,
            Custom,
        }

        public enum CollisionHandling
        {
            ByGroup,
            ByCollider,
        }

        [Serializable]
        public class Modifier
        {
            [NonSerialized]
            public ColliderGroupData colliderGroupData;

#if ODIN_INSPECTOR
            [HorizontalGroup("Split", width: 0.25f, LabelWidth = 150)]
            [BoxGroup("Split/Filters")]
#endif
            public TierFilter tierFilter;

            [Flags]
            public enum TierFilter
            {
                Tier0 = 1,
                Tier1 = 2,
                Tier2 = 4,
                Tier3 = 8,
                Tier4 = 16,
                Tier5 = 32,
                Tier6 = 64,
            }


#if ODIN_INSPECTOR
            [InfoBox("@InfoBox")]
            [BoxGroup("Split/Imbue")]
#endif
            public ImbueType imbueType = ImbueType.None;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Imbue"), HideIf("imbueType", ImbueType.None)]
#endif
            public float imbueMax = 100;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Imbue"), HideIf("imbueType", ImbueType.None)]
#endif
            public float imbueRate = 1;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Imbue"), HideIf("imbueType", ImbueType.None)]
#endif
            public float imbueConstantLoss = 3;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Imbue"), HideIf("imbueType", ImbueType.None)]
#endif
            public float staffSlamLoss = 90;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Imbue"), HideIf("imbueType", ImbueType.None)]
#endif
            public float imbueHitLoss = 30;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Imbue"), HideIf("imbueType", ImbueType.None)]
#endif
            public float imbueVelocityLossPerSecond = 0f;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Imbue"), HideIf("imbueType", ImbueType.None)]
#endif
            public float imbueEffectiveness = 1f;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Imbue"), HideIf("imbueType", ImbueType.None)]
#endif
            public float waterLossRateMultiplier = 1f;

#if ODIN_INSPECTOR
            [BoxGroup("Split/Deflect")]
#endif
            public float deflectMaxAngleCollision = 0;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Deflect"), HideIf("deflectMaxAngleCollision", 0f)]
#endif
            public float deflectMaxAngleTarget = 0;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Deflect"), HideIf("deflectMaxAngleCollision", 0f)]
#endif
            public float deflectSpeedMultiplier = 2.0f;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Deflect"), HideIf("deflectMaxAngleCollision", 0f)]
#endif
            public float deflectSpreadHeight = 1; // z rotation
#if ODIN_INSPECTOR
            [BoxGroup("Split/Deflect"), HideIf("deflectMaxAngleCollision", 0f)]
#endif
            public float deflectSpreadWidth = 1; // y rotation
            
            public FilterLogic spellFilterLogic = FilterLogic.AnyExcept;
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllSpellID))] 
#endif
            public List<string> spellIds;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllSpellID()
            {
                return Catalog.GetDropdownAllID<SpellData>();
            }
#endif
        }

        public struct CustomSpellEffect
        {
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllSpellID))]
#endif
            public string spellId;

#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllEffectID))]
#endif
            public string effectId;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllEffectID()
            {
                return Catalog.GetDropdownAllID(Category.Effect);
            }

            public List<ValueDropdownItem<string>> GetAllSpellID()
            {
                return Catalog.GetDropdownAllID<SpellCastCharge>();
            }
#endif
        }

    }
}


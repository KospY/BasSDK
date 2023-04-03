using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class ColliderGroupData : CatalogData
    {
        public CollisionHandling collisionHandling = CollisionHandling.ByGroup;
        public bool allowPenetration = true;

        public List<Modifier> modifiers = new List<Modifier>();

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
            public float imbueHitLoss = 30;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Imbue"), HideIf("imbueType", ImbueType.None)]
#endif
            public float imbueVelocityLoss = 0f;
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
            [ValueDropdown("GetAllSpellID")] 
#endif
            public List<string> spellIds;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllSpellID()
            {
                return Catalog.GetDropdownAllID(Category.Spell);
            } 
#endif            
        }
    }
}


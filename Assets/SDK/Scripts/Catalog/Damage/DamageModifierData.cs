using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class DamageModifierData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public DamageType damageType = DamageType.Blunt;

#if ODIN_INSPECTOR
        [BoxGroup("Collision(s)")]
#endif
        public List<Collision> collisions = new List<Collision>();

        protected Modifier defaultModifier;

        public override int GetCurrentVersion()
        {
            return 1;
        }


        [Serializable]
        public class Collision
        {
            [NonSerialized]
            public DamageModifierData damagerModifierData;

#if ODIN_INSPECTOR
            [HorizontalGroup("Split")]
            [ValueDropdown("GetAllMaterialID", IsUniqueList = true)]
#endif
            public List<string> sourceMaterialIds = new List<string>();
            [NonSerialized]
            public List<MaterialData> sourceMaterials;

#if ODIN_INSPECTOR
            [HorizontalGroup("Split")]
            [ValueDropdown("GetAllMaterialID", IsUniqueList = true)]
#endif
            public List<string> targetMaterialIds = new List<string>();
            [NonSerialized]
            public List<MaterialData> targetMaterials;

            public List<Modifier> modifiers = new List<Modifier>();

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllMaterialID()
            {
                return Catalog.GetDropdownAllID(Category.Material);
            }
#endif

            public void OnCatalogRefresh()
            {
            }

        }

        [Serializable]
        public class Modifier
        {
            [NonSerialized]
            public Collision collision;
            [NonSerialized]
            public DamageModifierData damagerModifierData;

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
            [BoxGroup("Split/Filters")]
#endif
            public FilterLogic imbuesFilterLogic = FilterLogic.AnyExcept;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Filters"), ValueDropdown("GetAllSpellCastChargeID", IsUniqueList = true)]
#endif
            public string[] imbuesFilter = new string[0];

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllSpellCastChargeID()
            {
                return Catalog.GetDropdownAllID<SpellCastCharge>();
            }
#endif

#if ODIN_INSPECTOR
            [BoxGroup("Split/General"), LabelWidth(150)]
#endif
            public float minVelocity = 1;
#if ODIN_INSPECTOR
            [BoxGroup("Split/General"), LabelWidth(150)]
#endif
            public float damageMultiplier = 1;
#if ODIN_INSPECTOR
            [BoxGroup("Split/General"), LabelWidth(150)]
#endif
            public bool allowKnockout = false;
#if ODIN_INSPECTOR
            [BoxGroup("Split/General"), LabelWidth(150)]
#endif
            public bool allowPenetration = false;
#if ODIN_INSPECTOR
            [BoxGroup("Split/General"), LabelWidth(150), EnableIf("@allowPenetration && allowPenetration")]
#endif
            public bool pressureAllowed = false;

#if ODIN_INSPECTOR
            [BoxGroup("Split/Knockout"), LabelWidth(210), LabelText("Allowed min velocity"), EnableIf("allowKnockout")]
#endif
            public float knockoutAllowedMinVelocity = Mathf.Infinity;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Knockout"), LabelWidth(210), LabelText("Forced min velocity"), EnableIf("allowKnockout")]
#endif
            public float knockoutForcedMinVelocity = Mathf.Infinity;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Knockout"), LabelWidth(210), LabelText("Allowed min velocity (Throw-Sliced)"), EnableIf("allowKnockout")]
#endif
            public float knockoutThrowAllowedMinVelocity = Mathf.Infinity;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Knockout"), LabelWidth(210), LabelText("Forced min velocity (Throw-Sliced)"), EnableIf("allowKnockout")]
#endif
            public float knockoutThrowForcedMinVelocity = Mathf.Infinity;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Knockout"), LabelWidth(210), LabelText("Duration multiplier"), EnableIf("allowKnockout")]
#endif
            public float knockoutDurationMultiplier = 1;

#if ODIN_INSPECTOR
            [BoxGroup("Split/Push"), TableList(ShowIndexLabels = true)] 
#endif
            public PushLevel[] pushLevels;

            [Serializable]
            public class PushLevel
            {
                public float hitVelocity = Mathf.Infinity;
                public float throwVelocity = Mathf.Infinity;
            }

#if ODIN_INSPECTOR
                [BoxGroup("Split/Penetration"), LabelWidth(150), LabelText("Min velocity"), EnableIf("allowPenetration")]
#endif
            public float penetrationMinVelocity = 1;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Penetration"), LabelWidth(150), LabelText("Damper multiplier"), EnableIf("allowPenetration")]
#endif
            public float penetrationDamperMultiplier = 1;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Penetration"), LabelWidth(150), LabelText("Velocity max depth"), EnableIf("allowPenetration")]
#endif
            public bool penetrationVelocityMaxDepth;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Penetration"), LabelWidth(150), LabelText("Velocity max depth curve"), EnableIf("@allowPenetration && penetrationVelocityMaxDepth")]
#endif
            public AnimationCurve penetrationVelocityMaxDepthCurve;
#if ODIN_INSPECTOR
            [BoxGroup("Split/Penetration"), LabelWidth(150), EnableIf("@allowPenetration && pressureAllowed")]
#endif
            public float pressureForceMultiplier = 1;
        }
    }
}


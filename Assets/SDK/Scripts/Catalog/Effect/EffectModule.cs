using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor.AddressableAssets.Settings;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	public class EffectModule
    {
#if ODIN_INSPECTOR
        [HorizontalGroup("Split", 0.5f, LabelWidth = 150)]
        [BoxGroup("Split/Module filters")]
#endif
        public Effect.Step step = Effect.Step.Start;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Module filters")]
        [ShowIf("step", Effect.Step.Custom)]
#endif
        public string stepCustomId;
        [NonSerialized]
        public int stepCustomIdHash;
#if ODIN_INSPECTOR
        [BoxGroup("Split/Module filters")]
        [ShowIf("step", Effect.Step.Custom)]
#endif
        public bool loopCustomStep;
        
#if ODIN_INSPECTOR
        [BoxGroup("Split/Module filters")]
#endif
        public PlatformFilter platformFilter = PlatformFilter.Windows | PlatformFilter.Android;

#if ODIN_INSPECTOR
        [Newtonsoft.Json.JsonProperty("isGore")]
        private bool contentSetter
        {
            set
            {
                if (value)
                {
                    sensitiveContent = BuildSettings.ContentFlag.Blood;
                }
                else
                {
                    sensitiveContent = BuildSettings.ContentFlag.None;
                }
            }
        }

        [BoxGroup("Split/Module filters")]
#endif
        public BuildSettings.ContentFlag sensitiveContent = BuildSettings.ContentFlag.None;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Module filters")]
#endif
        public BuildSettings.ContentFlagBehaviour sensitiveFilterBehaviour = BuildSettings.ContentFlagBehaviour.Discard;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Collision filters")]
#endif
        public DamagerFilter damagerStateFilter = DamagerFilter.Inactive | DamagerFilter.Active;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Collision filters")]
#endif
        public DamageTypeFilter damageTypeFilter = DamageTypeFilter.Energy | DamageTypeFilter.Blunt | DamageTypeFilter.Slash | DamageTypeFilter.Pierce;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Collision filters")]
#endif
        public PenetrationFilter penetrationFilter = PenetrationFilter.None | PenetrationFilter.Hit | PenetrationFilter.Pressure;

#if ODIN_INSPECTOR
        [BoxGroup("Split/Collision filters")]
#endif
        public bool reparentWithBreakable = true;

#if ODIN_INSPECTOR
        [HorizontalGroup("Split2", 0.25f, LabelWidth = 150)]
        [BoxGroup("Split2/Imbues filters")]
#endif
        public FilterLogic imbuesFilterLogic = FilterLogic.AnyExcept;

#if ODIN_INSPECTOR
        [BoxGroup("Split2/Imbues filters")]
        [ValueDropdown("GetAllSpellCastChargeID", IsUniqueList = true)]
#endif
        public string[] imbuesFilter = new string[0];
        protected int[] imbueHashes = new int[0];

#if ODIN_INSPECTOR
        [BoxGroup("Split2/Collider group filters")]
#endif
        public FilterLogic colliderGroupFilterLogic = FilterLogic.AnyExcept;

#if ODIN_INSPECTOR
        [BoxGroup("Split2/Collider group filters")]
        [ValueDropdown("GetAllColliderGroupID", IsUniqueList = true)]
#endif
        public string[] colliderGroupsFilter = new string[0];
        protected int[] colliderGroupHashes = new int[0];

#if ODIN_INSPECTOR
        [BoxGroup("Split2/Damagers filters")]
#endif
        public FilterLogic damagersFilterLogic = FilterLogic.AnyExcept;

#if ODIN_INSPECTOR
        [BoxGroup("Split2/Damagers filters")]
        [ValueDropdown("GetAllDamagerID", IsUniqueList = true)]
#endif
        public string[] damagersFilter = new string[0];
        protected int[] damagerHashes = new int[0];

#if ODIN_INSPECTOR
        [HorizontalGroup("Split3")]
        [BoxGroup("Split3/Input modifiers")]
#endif
        public AnimationCurve intensityCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [NonSerialized]
        public EffectData rootData;

        [Flags]
        public enum PlatformFilter
        {
            Windows = 1,
            Android = 2
        }

        [Flags]
        public enum PenetrationFilter
        {
            None = 1,
            Pressure = 2,
            Hit = 4,
            Skewer = 8
        }

        [Flags]
        public enum DamagerFilter
        {
            Inactive = 1,
            Active = 2
        }

        [Flags]
        public enum DamageTypeFilter
        {
            Energy = 1,
            Blunt = 2,
            Slash = 4,
            Pierce = 8
        }


#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllSpellCastChargeID()
        {
            return Catalog.GetDropdownAllID<SpellCastCharge>();
        }
        public List<ValueDropdownItem<string>> GetAllColliderGroupID()
        {
            return Catalog.GetDropdownAllID<ColliderGroupData>();
        }

        public List<ValueDropdownItem<string>> GetAllDamagerID()
        {
            return Catalog.GetDropdownAllID<DamagerData>();
        }
#endif

        public virtual void Clean()
        { }

        public virtual void CopyHDRToNonHDR()
        { }

    }
}

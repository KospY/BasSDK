using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [System.Serializable]
    public class AttackMotion : StanceNode
    {
        // settings for the attack and all related information
#if ODIN_INSPECTOR
        [LabelWidth(75), HorizontalGroup("$prettifiedID/Horiz"), VerticalGroup("$prettifiedID/Horiz/Fields"), HorizontalGroup("$prettifiedID/Horiz/Fields/Row4")]
#endif
        public float skipChance = 0f;
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz", Width = 0.6f)]
        [BoxGroup("$prettifiedID/Horiz/Attack")]
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Motion", Width = 80), HideLabel]
#endif
        public Interactable.HandSide attackSide = Interactable.HandSide.Right;
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Motion", Width = 80), HideLabel]
#endif
        public ItemModuleAI.AttackType attackMotion;
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Motion"), LabelWidth(135)]
#endif
        public bool includeInMeleeRange = true;
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Motion"), LabelWidth(65)]
#endif
        public float minRange;
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Motion", Width = 60), LabelWidth(45)]
#endif
        public bool sweep;
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Motion"), LabelWidth(65)]
#endif
        public float maxRange;
        [JsonProperty("range")]
        protected float range
        {
            set
            {
                minRange = value;
                maxRange = value;
            }
        }
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Motion", Width = 150), LabelWidth(135)]
#endif
        public bool includeWeaponReach = true;
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Motion"), LabelWidth(80)]
#endif
        public float pushForce;
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Timing"), LabelWidth(40)]
#endif
        public float start;
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Timing"), LabelWidth(40)]
#endif
        public float peak;
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Timing"), LabelWidth(40)]
#endif
        public float end;
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Transitions"), LabelWidth(70), ValueDropdown(nameof(GetSourceIdleOptions))]
#endif
        public string sourceIdle = "Any";
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Transitions"), LabelWidth(60), ValueDropdown(nameof(GetEndIdleOptions))]
#endif
        public string endIdle = "Previous";
#if ODIN_INSPECTOR
        [HorizontalGroup("$prettifiedID/Horiz/Attack/Transitions"), LabelWidth(120), ValueDropdown(nameof(GetChainComboOptions))]
#endif
        public string comboChainAttack = "Random";

        [NonSerialized]
        public int? sortedIndex = 0;

        public override bool showDifficulty => true;

        public override bool showWeight => true;

        protected AttackMotion specificChainAttack = null;
        protected List<AttackMotion> leftChainAttacks;
        protected List<AttackMotion> bothChainAttacks;
        protected List<AttackMotion> rightChainAttacks;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetSourceIdleOptions() => GetIdleOptions(true);

        public List<ValueDropdownItem<string>> GetEndIdleOptions() => GetIdleOptions(false);

        public List<ValueDropdownItem<string>> GetIdleOptions(bool source)
        {
            List<ValueDropdownItem<string>> options = new List<ValueDropdownItem<string>>
                {
                    source ? new ValueDropdownItem<string>("Any", "Any") : new ValueDropdownItem<string>("Previous", "Previous"),
                };
            if (!source)
            {
                options.Add(new ValueDropdownItem<string>("RandomInc", "RandomInc"));
                options.Add(new ValueDropdownItem<string>("RandomExc", "RandomExc"));
            }
            if (stanceData is MeleeStanceData meleeStanceData) options.AddRange(GetNodeListOptions(meleeStanceData.idles));
            return options;
        }

        public List<ValueDropdownItem<string>> GetChainComboOptions()
        {
            List<ValueDropdownItem<string>> options = new List<ValueDropdownItem<string>>
            {
                new ValueDropdownItem<string>("None", "None"),
                new ValueDropdownItem<string>("Random", "Random"),
            };
            if (stanceData is MeleeStanceData meleeStanceData) options.AddRange(GetNodeListOptions(meleeStanceData.chainAttacks));
            return options;
        }

        protected List<ValueDropdownItem<string>> GetNodeListOptions<T>(List<T> nodes) where T : StanceNode
        {
            List<ValueDropdownItem<string>> options = new List<ValueDropdownItem<string>>();
            for (int i = 0; i < nodes.Count; i++)
            {
                options.Add(new ValueDropdownItem<string>(nodes[i].id, nodes[i].id));
            }
            return options;
        }
#endif

    }
}

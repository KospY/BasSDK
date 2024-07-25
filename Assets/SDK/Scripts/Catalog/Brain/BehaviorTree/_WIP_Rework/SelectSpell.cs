using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Get
{
    public class SelectSpell : ActionNode
    {
        [Serializable]
        public class Choice
        {
#if ODIN_INSPECTOR
            [HorizontalGroup("Type"), EnumToggleButtons, HideLabel] 
#endif
            public ChoiceType choiceType = ChoiceType.Melee;
#if ODIN_INSPECTOR
            [EnableIf("choiceType", ChoiceType.Magic)]
            [ValueDropdown(nameof(GetAllSpellID))] 
#endif
            [JsonMergeKey]
            public string choiceID = "";
            public float probabilityWeight;
            [NonSerialized]
            public float probabilityCurrentWeight;

#if ODIN_INSPECTOR
            [ShowInInspector, ReadOnly, ProgressBar(0, 100)] 
#endif
            [NonSerialized]
            public float probabilityPercent;

            [NonSerialized]
            public float probabilityRangeFrom;
            [NonSerialized]
            public float probabilityRangeTo;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllSpellID()
            {
                return Catalog.GetDropdownAllID<SpellData>();
            } 
#endif
        }
        public enum ChoiceType
        {
            Melee,
            Magic,
        }
    }
}


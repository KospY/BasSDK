using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class LootTable : LootTableBase, IContainerLoadable<LootTable>
    {

#if ODIN_INSPECTOR
        [ListDrawerSettings(DraggableItems = false, Expanded = true, HideAddButton = true, HideRemoveButton = true, ShowPaging = false), LabelText("Drop Levels")]
#endif
        public List<DropLevel> levelledDrops = new List<DropLevel>();
        private static int maxPickCount = 20;



        [Serializable]
        public class DropLevel
        {

#if ODIN_INSPECTOR
            [HorizontalGroup("$elementName/Top/Row", LabelWidth = 120), PropertyOrder(3), ShowInInspector, InlineButton("AddEntryForEachInCategory", "Add entry for each in category")]
#endif
            [NonSerialized]
            public ItemData.Type itemCategory;
#if ODIN_INSPECTOR
            [Space, TableList(AlwaysExpanded = true), VerticalGroup("$elementName/Bottom"), PropertyOrder(4)]
#endif
            public List<Drop> drops;


            [NonSerialized]
            public LootTable lootTable;
            [NonSerialized]
            public float probabilityTotalWeight;
            [NonSerialized]
            public float minMoneyValue;
            [NonSerialized]
            public float maxMoneyValue;
            [NonSerialized]
            public float minRewardValue;
            [NonSerialized]
            public float maxRewardValue;

        }

        [Serializable]
        public class Drop
        {
            public enum RandMode
            {
                ItemCount,
                MoneyValue,
                RewardValue,
            }

#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllItemOrLootTableID)), PropertyOrder(1)]
#endif
            [JsonMergeKey]
            public string referenceID;

#if ODIN_INSPECTOR
            [HorizontalGroup("Reference"), PropertyOrder(0), EnumToggleButtons, HideLabel]
#endif
            public Reference reference = Reference.Item;

            [NonSerialized]
            public ItemData itemData;
            [NonSerialized]
            public LootTable lootTable;

#if ODIN_INSPECTOR
            [PropertyOrder(1), HideLabel]
#endif
            public RandMode randMode = RandMode.ItemCount;
#if ODIN_INSPECTOR
            [PropertyOrder(2), HideLabel]
#endif
            public Vector2 minMaxRand = Vector2.one;

#if ODIN_INSPECTOR
            [PropertyOrder(3)]
#endif
            public float probabilityWeight;

#if ODIN_INSPECTOR
            [PropertyOrder(4)]
            [ShowInInspector]
            [ReadOnly]
            [ProgressBar(0, 100)]
#endif
            [NonSerialized]
            public float probabilityPercent;

            [NonSerialized]
            public float probabilityRangeFrom;
            [NonSerialized]
            public float probabilityRangeTo;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllItemOrLootTableID()
            {
                if (reference == Reference.Item)
                {
                    return Catalog.GetDropdownAllID(Category.Item);
                }
                else
                {
                    return Catalog.GetDropdownAllID(Category.LootTable);
                }
            }
#endif

            public enum Reference
            {
                Item,
                Table,
            }

        }

        public override int GetCurrentVersion()
        {
            return 1;
        }
        public void OnLoadedFromContainer(Container container)
        { }

        public ContainerContent InstanceContent()
        { return null; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class ShopData : CatalogData
    {
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCreatureIDs))]
#endif
        public string shopKeeperCreatureID;
        public DisplaySorting displaySorting;
        public string tradeCurrency = "Gold";
		public float defaultUsedValueMultiplier = 0f;
        public RoundMethod roundingMethod;
        public int restockDayInterval = 2;

#if ODIN_INSPECTOR
        [TableList(DrawScrollView = false, AlwaysExpanded = true, CellPadding = 2)]
#endif
        public List<ShopStockCategory> shopStockCategories = new List<ShopStockCategory>();

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllCreatureIDs()
        {
            return Catalog.GetDropdownAllID(Category.Creature);
        }

        public List<ValueDropdownItem<string>> GetAllItemIDs()
        {
            return Catalog.GetDropdownAllID(Category.Item);
        }
#endif

        public enum DisplaySorting
        {
            MostValuableWithoutDiscount,
            MostValuableWithDiscount,
            Oldest,
            Newest,
            Random,
        }

        public enum RoundMethod
        {
            RoundNormal,
            RoundUp,
            RoundDown,
        }

        [Serializable]
        public class ShopStockCategory
        {
            [JsonMergeKey]
#if ODIN_INSPECTOR
            [VerticalGroup("Category", PaddingBottom = 2), LabelText("Name"), LabelWidth(40)]
#endif
            public string stockCategory;

#if ODIN_INSPECTOR
            [VerticalGroup("Trade", PaddingBottom = 2), LabelText("\"Used\" Value Mult"), LabelWidth(110), TableColumnWidth(300, resizable: false)]
#endif
            [Range(0f, 1f)]
            public float usedValueMultiplier = 1f;

#if ODIN_INSPECTOR
            [VerticalGroup("Trade"), LabelText("Despawn On Trade"), LabelWidth(115)]
#endif
            public bool despawnOnTrade = false; // Whether to remove this item when traded

#if ODIN_INSPECTOR
            [VerticalGroup("Stocking", PaddingBottom = 2), HorizontalGroup("Stocking/Stock"), ValueDropdown(nameof(GetAllLootTableIDs)), LabelText("Table ID"), LabelWidth(55)]
#endif
            public string restockTableID = "None";
#if ODIN_INSPECTOR
            [VerticalGroup("Stocking"), HorizontalGroup("Stocking/Stock", Width = 200), LabelText("Min/Max"), LabelWidth(60)]
#endif
            public Vector2Int minMaxStockQuantity = new Vector2Int();
#if ODIN_INSPECTOR
            [VerticalGroup("Stocking"), LabelText("Upgrade Checkpoints"), LabelWidth(125)]
#endif
            public string upgradeCheckpoints;

            [NonSerialized]
            public LootTableBase restockTable;
            [NonSerialized]
#if ODIN_INSPECTOR
            [ShowInInspector, ReadOnly, VerticalGroup("Category"), HorizontalGroup("Category/Debug")]
#endif
            public List<string> allAcceptedIDs;

            [NonSerialized]
            public ShopData containingData;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllLootTableIDs()
            {
                return Catalog.GetDropdownAllID(Category.LootTable);
            }
#endif

#if ODIN_INSPECTOR
            [Button(Name = "Load Accepted IDs"), HorizontalGroup("Category/Debug")]
#endif
            public void LoadAcceptedIDs()
            {
                restockTable = Catalog.GetData<LootTableBase>(restockTableID);
                if (restockTable == null)
                {
                    Debug.LogError($"Missing restock table for category {stockCategory}! The shop doesn't know which items belong in this category and they will use the default used value multiplier for buying/selling!");
                    return;
                }
                allAcceptedIDs = new List<string>();
                foreach (ItemData itemData in restockTable.GetAll())
                {
                    if (itemData == null) continue;
                    if (allAcceptedIDs.Contains(itemData.id)) continue;
                    allAcceptedIDs.Add(itemData.id);
                }
            }
        }

        [NonSerialized]
        public Dictionary<string, ShopStockCategory> shopStockDictionary;

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            shopStockDictionary = new Dictionary<string, ShopStockCategory>();
            for (int i = 0; i < shopStockCategories.Count; i++)
            {
                ShopStockCategory shopCategory = shopStockCategories[i];
                shopCategory.containingData = this;
                if (string.IsNullOrEmpty(shopCategory.stockCategory))
                {
                    Debug.LogError($"Shop [{id}] category element #{i} is missing a category name!");
                    continue;
                }
                shopStockDictionary.Add(shopCategory.stockCategory, shopCategory);
                shopCategory.LoadAcceptedIDs();
            }
        }
    }
}

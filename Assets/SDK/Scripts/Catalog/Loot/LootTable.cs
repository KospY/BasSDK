using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class LootTable : CatalogData
    {
#if ODIN_INSPECTOR
        [HorizontalGroup("Data/Split"), LabelWidth(70)]
#endif
        public string groupPath;

#if ODIN_INSPECTOR
        [BoxGroup("Auto-fill"), ShowInInspector, InlineButton("RemoveNonPurchaseable", "Remove non-purchaseable items"), InlineButton("AddEntryForEachInCategory", "Add entry for each in category")]
#endif
        [NonSerialized]
        public ItemData.Type itemCategory;
#if ODIN_INSPECTOR
        [TableList(AlwaysExpanded = true)]
#endif
        public List<Drop> drops;
        private float probabilityTotalWeight;
        private static int maxPickCount = 20;

        private void AddEntryForEachInCategory()
        {
            foreach (ItemData itemData in Catalog.GetDataList<ItemData>())
            {
                if (itemData.type == itemCategory)
                {
                    drops.Add(new Drop()
                    {
                        reference = Drop.Reference.Item,
                        referenceID = itemData.id,
                        probabilityWeight = 1f
                    });
                }
            }
        }

        private void RemoveNonPurchaseable()
        {
            for (int i = drops.Count - 1; i >= 0; i--)
            {
                Drop drop = drops[i];
                if (drop.reference == Drop.Reference.Table) continue;
                drop.itemData = Catalog.GetData<ItemData>(drop.referenceID);
                if (drop.itemData.purchasable) continue;
                drops.RemoveAt(i);
            }
        }

        public override CatalogData Clone()
        {
            LootTable clone = MemberwiseClone() as LootTable;
            clone.drops = clone.drops.Select(item => (Drop)item.Clone()).ToList();
            return clone;
        }

        [Serializable]
        public class Drop
        {
#if ODIN_INSPECTOR
            [ValueDropdown("GetAllItemOrLootTableID"), PropertyOrder(1)]
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
            [PropertyOrder(2)]
#endif
            public float probabilityWeight;

#if ODIN_INSPECTOR
            [PropertyOrder(3)]
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

            public void OnCatalogRefresh(LootTable sourceLootTable)
            {
                if (referenceID != null && referenceID != "")
                {
                    if (reference == Reference.Item)
                    {
                        itemData = Catalog.GetData<ItemData>(referenceID);
                        if (itemData == null)
                        {
                            Debug.LogError("Loot table: " + sourceLootTable.id + " cannot find Item:" + referenceID);
                        }
                    }
                    else if (reference == Reference.Table)
                    {
                        lootTable = Catalog.GetData<LootTable>(referenceID);
                    }
                }
            }

            public Drop Clone()
            {
                return MemberwiseClone() as Drop;
            }
        }

        public override int GetCurrentVersion()
        {
            return 1;
        }

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            CalculateWeight();
            if (drops == null) return;
            foreach (Drop drop in drops)
            {
                drop.OnCatalogRefresh(this);
            }
        }

        public bool DoesLootTableContainItemID(string id, int depth = 0)
        {
            depth++;
            if (depth > maxPickCount)
            {
                Debug.LogError(this.id + " | Max search depth for ID [ " + id + " ] reached! ( " + maxPickCount + ") Please check if there is any loop in the drop table...");
                return false;
            }
            for (int r = 0; r < 2; r++)
            {
                for (int i = 0; i < drops.Count; i++)
                {
                    // skip tables if r == 0, skip items if r == 1
                    if (drops[i].reference == (r == 0 ? Drop.Reference.Table : Drop.Reference.Item)) continue;
                    // return true if the item ID matches or the sub-table (or its sub-tables) contains the ID
                    if ((r == 0 ? id == drops[i].referenceID : Catalog.GetData<LootTable>(drops[i].referenceID).DoesLootTableContainItemID(id, depth))) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Calculates the percentage and asigns the probabilities how many times
        /// the items can be picked. Function used also to validate data when tweaking numbers in editor.
        /// </summary>	
        public void CalculateWeight()
        {
            if (drops == null || drops.Count == 0) return;

            float currentProbabilityWeightMaximum = 0f;
            // Sets the weight ranges of the selected items.
            foreach (Drop line in drops)
            {
                if (line.probabilityWeight < 0f)
                {
                    // Prevent usage of negative weight.
                    line.probabilityWeight = 0f;
                }
                else
                {
                    line.probabilityRangeFrom = currentProbabilityWeightMaximum;
                    currentProbabilityWeightMaximum += line.probabilityWeight;
                    line.probabilityRangeTo = currentProbabilityWeightMaximum;
                }
            }
            probabilityTotalWeight = currentProbabilityWeightMaximum;
            // Calculate percentage of item drop select rate.
            foreach (Drop lootDropItem in drops)
            {
                lootDropItem.probabilityPercent = ((lootDropItem.probabilityWeight) / probabilityTotalWeight) * 100;
            }
        }

        // Can return null
        public ItemData Pick(int pickCount = 0, System.Random randomGen = null)
        {
            CalculateWeight();
            pickCount++;
            if (pickCount > maxPickCount)
            {
                Debug.LogError(id + " | Max pick count reached! ( " + maxPickCount + ") Please check if there is any loop in the drop table...");
                return null;
            }

            float pickedNumber = randomGen != null ? (float)randomGen.NextDouble() * probabilityTotalWeight : UnityEngine.Random.Range(0, probabilityTotalWeight);

            // Find an item whose range contains pickedNumber
            foreach (Drop drop in drops)
            {
                // If the picked number matches the item's range, return item
                if (pickedNumber > drop.probabilityRangeFrom && pickedNumber < drop.probabilityRangeTo)
                {
                    if (drop.referenceID == "" || drop.referenceID == null) return null;
                    if (drop.reference == Drop.Reference.Item)
                    {
                        return drop.itemData;
                    }
                    else
                    {
                        if (drop.lootTable != null)
                        {
                            return drop.lootTable.Pick(pickCount, randomGen);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            Debug.LogError("Drop table id : " + id + " | Item couldn't be picked... Be sure that all of your active loot drop tables have assigned at least one item!");
            return null;
        }

        public List<ItemData> GetAll(int pickCount = 0)
        {
            List<ItemData> itemDataList = new List<ItemData>();
            pickCount++;
            if (pickCount > maxPickCount)
            {
                Debug.LogError(id + " | Max pick count reached! ( " + maxPickCount + ") Please check if there is any loop in the drop table...");
                return itemDataList;
            }

            // Find an item whose range contains pickedNumber
            foreach (Drop drop in drops)
            {
                if (drop.referenceID == "" || drop.referenceID == null) continue;
                if (drop.reference == Drop.Reference.Item)
                {
                    itemDataList.Add(drop.itemData);
                }
                else if (drop.lootTable != null)
                {
                    itemDataList.AddRange(drop.lootTable.GetAll(pickCount));
                }
            }
            return itemDataList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ThunderRoad
{

    /// <summary>
    /// Allows for converting flat containers inventory into a nested category to item record
    /// </summary>
    public class InventoryContent
    {
        /// <summary>
        /// Class to define a complex inventory item content, to keep references
        /// </summary>
        public class InventoryItem
        {
            // Linked item data
            public ItemData itemData;

            // Stacked quantity
            public int quantity;

            // Queued game objects (when adding ti the inventory by ungrabbing)
            public Queue<GameObject> worldObjects;

            // Linked inventory container
            public ContainerData.Content linkedContent;

            // Can we stack more items with it ?
            public bool isStackable => itemData.isStackable;

            // The item max stack quantity, defined in Item data
            public int maxStackQuantity => itemData.maxStackQuantity;

            // Is it fully stacked/overflowing ?
            public bool isStackFull => !itemData.isStackable || quantity >= maxStackQuantity;

            /// <summary>
            /// Converts the item's category path to a full category group
            /// </summary>
            public GameData.CategoryGroup Category => GetCategoryFromPath();

            /// <summary>
            /// Converts the item's category path to a full group
            /// Return the corresponding path to category group
            /// (example: 'xxx' → 'yyy' → 'zzz' → 'Potions' → 'Misc' | in this case it will return "Potions")
            /// </summary>
            /// <returns>Corresponding enum, BackpackHolder.CategoryPath.Invalid if not found</returns>
            private GameData.CategoryGroup GetCategoryFromPath()
            {
                return GameData.InvalidCategoryGroup;
            }

            public InventoryItem(Item item, int quantity, Queue<GameObject> worldObjects,
                ContainerData.Content linkedContent)
            {
                
            }

            public InventoryItem(ItemData itemData, int quantity, ContainerData.Content linkedContent)
            {
            }

            /// <summary>
            /// Stacks items together
            /// </summary>
            /// <param name="item">Item to stack</param>
            /// <param name="updateContent">Should the content quantity be updated ? (Used to prevent double quantity setter)</param>
            public void Add(InventoryItem item, bool updateContent = true)
            {
                
            }

            /// <summary>
            /// Stacks items together with a limited maximum number of items
            /// </summary>
            /// <param name="item">Item to stack</param>
            /// <param name="numberOfItemsToAdd">Maximum number of items to add</param>
            public void Add(InventoryItem item, int numberOfItemsToAdd)
            {
                
            }

            public override string ToString()
            {
                return
                    $"{itemData.displayName} [{quantity}{(isStackable ? "/" + maxStackQuantity : "")} | {worldObjects.Count}]";
            }
        }

        /// <summary>
        /// Class to define a complex inventory category content, to keep references
        /// </summary>
        public class InventoryCategory
        {
            // Linked category group
            public GameData.CategoryGroup categoryGroup;

            // The max slot amount the category can hold
            public int maxSlotAmount = 10;

            // Get the name of the category
            public string Name => categoryGroup.DisplayName();
            public string Id => categoryGroup.id;

            // Items held by the category
            public List<InventoryItem> items;

            public InventoryCategory(GameData.CategoryGroup categoryGroup, List<InventoryItem> items)
            {
                this.categoryGroup = categoryGroup;
                this.items = items;
            }

            public InventoryCategory(GameData.CategoryGroup categoryGroup)
            {
                this.categoryGroup = categoryGroup;
                items = new List<InventoryItem>();
                maxSlotAmount = categoryGroup.inventoryMaxSlotAmount;
            }

            /// <summary>
            /// Check if the category contains at least one item with the same ID.
            /// </summary>
            /// <param name="itemDataId">Item data id.</param>
            /// <returns></returns>
            public bool HasItem(string itemDataId)
            {
                foreach (var inventoryItem in items)
                    if (inventoryItem.itemData.id == itemDataId)
                        return true;

                return false;
            }

            /// <summary>
            /// Converts the a category path to the corresponding categoryGroup class
            /// </summary>
            /// <param name="pathName">Name of the path</param>
            /// <returns></returns>
            public static GameData.CategoryGroup GetCategoryGroupFromName(string pathName)
            {
                return GameData.InvalidCategoryGroup;
            }

            /// <summary>
            /// Adds an item to the category
            /// Stack it if possible, else just add it
            /// </summary>
            /// <param name="item"></param>
            /// <param name="updateContent">Should the content quantity be updated ? (Used to prevent double quantity setter)</param>
            /// <returns>True if the item was added, false if the category is full</returns>
            public (bool, InventoryItem) Add(InventoryItem item, bool updateContent = true)
            {
                if (IsItemOverflowing(item.itemData)) // We are storing too much already
                    return (false, null);

                // If the content is being updated by the building process, bypass the stacking behaviour
                if (updateContent)
                {
                    if (item.isStackable) // If this item is stackable, then check if there's already some of it
                    {
                        foreach (var inventoryItem in items)
                        {
                            if (inventoryItem.itemData.id != item.itemData.id) continue;

                            if (inventoryItem.isStackFull)
                                continue; // If the stack is full, ignore

                            inventoryItem.Add(item); // The item exists and has some room, stack it
                            return (true, inventoryItem);
                        }
                    }
                }

                // if we have slots left, add the item
                if (items.Count < maxSlotAmount)
                {
                    items.Add(item);
                    return (true, null);
                }

                return (false, null);
            }
            /// <summary>
            /// Returns the current quantity of this item
            /// </summary>
            /// <param name="itemData">Item to check for</param>
            /// <returns>The current quantity of this item</returns>
            public int GetItemQuantity(ItemData itemData)
            {
                int sum = 0;

                for (int i = 0; i < items.Count; i++)
                    if (itemData.id == items[i].itemData.id)
                        sum += items[i].quantity;

                return sum;
            }

            /// <summary>
            /// Returns the total quantity of stored items
            /// </summary>
            /// <returns>Total quantity of stored items</returns>
            public int GetTotalItemsQuantity()
            {
                int sum = 0;

                for (int i = 0; i < items.Count; i++)
                    sum += items[i].quantity;

                return sum;
            }

            /// <summary>
            /// Check if item can enter the inventory content
            /// </summary>
            /// <param name="item">the item to check for</param>
            /// <returns></returns>
            public bool CouldBeAdded(Item item)
            {
                return false;
            }

            /// <summary>
            /// Checks if the item-data or category-group max amount limits are reached.
            /// </summary>
            /// <param name="itemData">Item data to check for</param>
            /// <returns>True if we have too much of this item already</returns>
            private bool IsItemOverflowing(ItemData itemData)
            {
                // If the category-group is set up with a max item quantity and we have too much
                // → then it is not allowed
                if (categoryGroup.limitInventoryMaxItemAmount)
                    if (GetTotalItemsQuantity() >= categoryGroup.inventoryMaxItemAmount)
                        return true;

                // If the item is set up with a max quantity limit and we have too much of the same type already
                // → then it is not allowed
                if (itemData.limitMaxStorableQuantity)
                    if (GetItemQuantity(itemData) >= itemData.maxStorableQuantity)
                        return true;
                
                return false;
            }

            /// <summary>
            /// Try to stack items that aren't.
            /// Useful when removing from a full stack when more stacks already exists.
            /// </summary>
            /// <param name="itemId">Item data id</param>
            public void TryToCollapseItems(string itemId)
            {
                
            }
        }

        // Current categories
        public InventoryCategory[] categories;

        // Flat content from the player's inventory
        private List<ContainerData.Content> rawContent;
        
    }

}
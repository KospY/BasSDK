using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThunderRoad
{
    ///<summary>
    /// Allows for storing simple content to display in a two layer Radial menu.
    ///</summary>
    public class DoubleLayeredRadialMenu
    {
        // Content of the radial menu (Categories & Items)
        private RadialMenuContent content;

        // Fast access to content's categories
        public RadialMenuCategory[] Categories => content.categories;

        #region ClassesAndStructs

        /// <summary>
        /// Define a radial menu item, used to display it later
        /// </summary>
        public class RadialMenuItem
        {
            public object value;
            public string id;
            public string displayName;
            public string categoryId;
            public string categoryName;
            public int categorySlotsAmount;
            public int quantity;
            public int maxStackQuantity;
            public bool isStackable;
            public string itemIconAddress;
            public string categoryIconAddress;

            public RadialMenuItem(object value, 
                string id, string displayName, 
                string categoryId, string categoryName, int categorySlotsAmount,
                int quantity, int maxStackQuantity, bool isStackable,
                string categoryIconAddress, string itemIconAddress)
            {
                this.value = value;
                this.id = id;
                this.displayName = displayName;
                this.categoryId = categoryId;
                this.categoryName = categoryName;
                this.categorySlotsAmount = categorySlotsAmount;
                this.quantity = quantity;
                this.maxStackQuantity = maxStackQuantity;
                this.isStackable = isStackable;
                this.categoryIconAddress = categoryIconAddress;
                this.itemIconAddress = itemIconAddress;
            }
        }

        ///<summary>
        /// Custom class to represent the first layer of the menu, contains elements called "Categories":
        ///    - They are used to nest the second elements ("Items"), like tabs would do in a standard UI.
        /// The are simple values, used for display only.
        ///</summary>
        public class RadialMenuCategory
        {
            // Display name of the category.
            public string name;

            // Serves as an ID to get references.
            public string id;
            
            public int slotsNumber;

            // Flat array of items.
            public RadialMenuItem[] items;

            // Look Up Table used to get the amount from the items in the flat array.
            public Dictionary<RadialMenuItem, int> itemsAmountLUT;

            // Address of the category Icon, used to display it in a RawImage UI element later.
            // If unspecified or wrong, icon won't be shown.
            public string iconAddress;

            public RadialMenuCategory(string id, string name, int slotsNumber, Dictionary<RadialMenuItem, int> itemsAndAmountLut,
                string iconAddress = null)
            {
                this.id = id;
                this.name = name;
                this.slotsNumber = slotsNumber;
                this.iconAddress = iconAddress;

                itemsAmountLUT = itemsAndAmountLut;
                items = null;
                BuildItemListFromLUT();
            }

            /// <summary>
            /// Return the item at given index, or bounds on overflow [0; items.Length - 1].
            /// </summary>
            /// <param name="index"></param>
            /// <returns>The RadialMenuItem at index</returns>
            public RadialMenuItem GetItem(int index)
            {
                if (items == null) return null;
                if (items.Length <= 0) return null;

                return items[Mathf.Clamp(index, 0, items.Length - 1)];
            }

            /// <summary>
            /// Return the item quantity, or -1 if not found.
            /// </summary>
            /// <param name="radialMenuItem"></param>
            /// <returns>The quantity of the radialMenuItem</returns>
            public int GetItemQuantity(RadialMenuItem radialMenuItem)
            {
                if (radialMenuItem.value == null) return -1;

                return itemsAmountLUT[radialMenuItem];
            }

            // Look Up Table is a Dictionary, so it doesn't have a proper order.
            // To build the flat array we need to create this order from something → here it's by alphabetical order.
            // Should probably be changed to quantity or by passing some custom comparable if needed.
            public void BuildItemListFromLUT()
            {
                items = itemsAmountLUT.Keys
                    .OrderBy(i => i.displayName)
                    .ThenByDescending(i => i.quantity)
                    .ToArray();
            }
        }

        ///<summary>
        /// Custom structure to allow for converting simple object list → to a double layer menu.
        /// First layer contains elements called "Categories":
        ///    - They are used to nest the second elements, like tabs would do in a standard UI.
        /// Second layer contains elements called "Items":
        ///    - They are the actual objects from a given flat list, holding references for UI events and callbacks
        ///</summary>
        public struct RadialMenuContent
        {
            // Flat array of categories (1st layer)
            public RadialMenuCategory[] categories;

            // Builds the content from a flat list of items.
            public RadialMenuContent(IEnumerable<RadialMenuItem> items,
                GameData.CategoryGroup[] fixedCategories = null)
            {
                // We use a dictionary to collapse all the category occurence from the items.
                var categoriesList = new Dictionary<string, RadialMenuCategory>();

                foreach (var item in items)
                {
                    var category = item.categoryId;
                    var quantity = item.quantity;

                    categoriesList.TryGetValue(category, out var currentCategory);
                    if (currentCategory == null) // If the category doesn't exist yet, create it
                    {
                        currentCategory =
                            new RadialMenuCategory(item.categoryId, item.categoryName, item.categorySlotsAmount,
                                new Dictionary<RadialMenuItem, int>(),
                                item.categoryIconAddress); //retrieve its icon from the current item

                        categoriesList.Add(category, currentCategory);
                    }

                    // Using the Look Up Table, increase quantity if it exists already
                    if (currentCategory.itemsAmountLUT.ContainsKey(item))
                        currentCategory.itemsAmountLUT[item] += quantity;
                    else
                        currentCategory.itemsAmountLUT.Add(item, quantity);

                    // If by any chance the category Icon wasn't in the previous items, try to get it in this one
                    if (string.IsNullOrEmpty(currentCategory.iconAddress))
                        currentCategory.iconAddress = item.categoryIconAddress;
                }

                // If categories can be fixed, add them
                if (fixedCategories != null)
                    foreach (var fixedCategory in fixedCategories)
                    {
                        // We ignore categories that should be hidden
                        if(!fixedCategory.displayWhenEmpty) continue;
                        
                        if (!categoriesList.ContainsKey(fixedCategory.id))
                            categoriesList.Add(fixedCategory.id,
                                new RadialMenuCategory(
                                    fixedCategory.id,
                                    fixedCategory.DisplayName(),
                                    fixedCategory.inventoryMaxSlotAmount,
                                    new Dictionary<RadialMenuItem, int>(),
                                    fixedCategory.iconAddress
                                )
                            );
                    }

                // Same as the items, we need a way of organizing category order
                // If we have fixed categories, use the index
                // If we don't have fixed categories, use the alphabetical order
                categories = fixedCategories != null
                    ? categoriesList.Values
                        .OrderBy(c => Array.FindIndex(fixedCategories,
                            categoryGroup => categoryGroup.id == c.id))
                        .ToArray()
                    : categoriesList.Values.OrderBy(c => c.name).ToArray();


                foreach (var category in categories)
                    category.BuildItemListFromLUT();
            }

            /// <summary>
            /// Return the category at given index, or null on overflow.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public RadialMenuCategory GetCategory(int index)
            {
                if (categories.Length <= index)
                    return null;

                return categories[index];
            }

            /// <summary>
            /// Return the category's index.
            /// </summary>
            /// <param name="category"></param>
            /// <returns></returns>
            public int GetIndex(RadialMenuCategory category)
            {
                return Array.IndexOf(categories, category);
            }
        }

        #endregion

        public DoubleLayeredRadialMenu(IEnumerable<RadialMenuItem> items,
            GameData.CategoryGroup[] fixedCategories = null)
        {
            content = new RadialMenuContent(items, fixedCategories);
        }

        /// <summary>
        /// Return the category at given index, or null on overflow.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public RadialMenuCategory GetCategory(int index)
        {
            return content.GetCategory(index);
        }

        /// <summary>
        /// Return the category's index.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public int GetIndex(RadialMenuCategory category)
        {
            return content.GetIndex(category);
        }

        /// <summary>
        /// Converts an object of type T, to a radial menu item to display
        /// To do this, we use user defined Func (delegates) to get the needed values
        /// </summary>
        /// <param name="t">Object to hold a reference of</param>
        /// <param name="getId">Callback to get an id string from the item instance</param>
        /// <param name="getName">Callback to get a name string from the item instance</param>
        /// <param name="getCategoryId">Callback to get a category id string from the item instance</param>
        /// <param name="getCategoryName">Callback to get a category name string from the item instance</param>
        /// <param name="getCategorySlotsAmount">Callback to get a category max slots amount</param>
        /// <param name="getQuantity">Callback to get a quantity int from the item instance</param>
        /// <param name="getMaxStackQuantity">Callback to get an item max stack quantity</param>
        /// <param name="getIsStackable">Callback to get if an item is stackable</param>
        /// <param name="getCategoryIconAddress">Callback to get an icon string from the item instance</param>
        /// <param name="getItemIconAddress">Callback to get and an icon string from the item instance</param>
        /// <typeparam name="T">Type of the given object, allow for building a menu from anything</typeparam>
        /// <returns>The radial menu item built from the callbacks</returns>
        public static RadialMenuItem ToRadialMenuItem<T>(
            T t,
            Func<T, string> getId,
            Func<T, string> getName,
            Func<T, string> getCategoryId,
            Func<T, string> getCategoryName,
            Func<T, int> getCategorySlotsAmount,
            Func<T, int> getQuantity,
            Func<T, int> getMaxStackQuantity,
            Func<T, bool> getIsStackable,
            Func<T, string> getCategoryIconAddress,
            Func<T, string> getItemIconAddress)
        {
            return new RadialMenuItem(
                t,
                getId(t),
                getName(t),
                getCategoryId(t),
                getCategoryName(t),
                getCategorySlotsAmount(t),
                getQuantity(t),
                getMaxStackQuantity(t),
                getIsStackable(t),
                getCategoryIconAddress(t),
                getItemIconAddress(t)
            );
        }
    }
}
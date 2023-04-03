using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    /// <summary>
    /// Class that stores data for the in-game inventory & backpack
    /// </summary>
    public class GameInventory
    {
        /// <summary>
        /// Data used by the inventory
        /// </summary>
        [Serializable]
        public class Data
        {
            public string inventoryRadialMenuUiPrefabAddress = "Bas.Menu.RadialMenuUI";
            public GameData.AudioContainerAddressAndVolume openSound;
            public GameData.AudioContainerAddressAndVolume closeSound;

            /// <summary>
            /// When the catalog refreshes, clean sound containers and reload them
            /// </summary>
            public void OnCatalogRefresh()
            {
            }
        }

        #region EditorUtilities

        /// <summary>
        /// Class to describe custom data to display
        /// </summary>
#if ODIN_INSPECTOR
        [ReadOnly] 
#endif
        [Serializable]
        public class InventoryItemDebugInfos
        {
#if ODIN_INSPECTOR
            [TableColumnWidth(40, Resizable = false), PreviewField(35, Alignment = ObjectFieldAlignment.Center), ReadOnly] 
#endif
            public Texture icon;

#if ODIN_INSPECTOR
            [VerticalGroup("Category"), HideLabel, GUIColor("GetCategoryGroupColorHash")] 
#endif
            public string categoryGroup;

#if ODIN_INSPECTOR
            [VerticalGroup("Category"), HideLabel, GUIColor("GetCategoryColorHash"), DisplayAsString]  
#endif
            public string category;

#if ODIN_INSPECTOR
            [DisplayAsString] 
#endif
            public string itemName;

#if ODIN_INSPECTOR
            [DisplayAsString] 
#endif
            public int stackAmount;

#if ODIN_INSPECTOR
            [GUIColor("GetStoredAmountLimitColor"), DisplayAsString] 
#endif
            public string storedAmountLimit;

#if ODIN_INSPECTOR
            [GUIColor("GetMaxPossibleAmount"), DisplayAsString]  
#endif
            public int maxPossibleAmountInGame;

#if ODIN_INSPECTOR
            [DisplayAsString, TableColumnWidth(10)] 
#endif
            public string behaviour = "";

#if ODIN_INSPECTOR
            [TableColumnWidth(5), DisplayAsString] 
#endif
            public string sounds = "";

#if ODIN_INSPECTOR
            [TableColumnWidth(40, Resizable = false), PreviewField(35, Alignment = ObjectFieldAlignment.Center), ReadOnly] 
#endif
            public Texture inGameIcon;

            public InventoryItemDebugInfos(ItemData data, GameData.CategoryGroup categoryGroup)
            {
                itemName = data.displayName;
                this.categoryGroup = categoryGroup.id;
                category = data.category;
                stackAmount = data.isStackable ? data.maxStackQuantity : 1;

                // If unlimited, display nothing. If 1, display "Unique". Else display the amount.
                storedAmountLimit = data.limitMaxStorableQuantity
                    ? (data.maxStorableQuantity == 1 ? "Unique" : $"{data.maxStorableQuantity}")
                    : "";

                maxPossibleAmountInGame = categoryGroup.inventoryMaxSlotAmount * stackAmount;

                if (categoryGroup.limitInventoryMaxItemAmount)
                    maxPossibleAmountInGame = Mathf.Min(categoryGroup.inventoryMaxItemAmount, maxPossibleAmountInGame);

                if (data.limitMaxStorableQuantity)
                    maxPossibleAmountInGame = Mathf.Min(data.maxStorableQuantity, maxPossibleAmountInGame);

                Catalog.LoadAssetAsync<Texture>(categoryGroup.iconAddress, texture => icon = texture,
                    "GameInventoryItemDebug");

                Catalog.LoadAssetAsync<Texture>(data.iconAddress, texture => inGameIcon = texture,
                    "GameInventoryItemDebug");

                if (data.modules?.FirstOrDefault(m => m.GetType() == typeof(ItemModuleReturnInInventory)) != null)
                    behaviour = "Always return";

            }

            public override string ToString()
            {
                return $"{categoryGroup} | {itemName} | {category} | {stackAmount} | {maxPossibleAmountInGame}";
            }

            private Color GetMaxPossibleAmount()
            {
#if UNITY_EDITOR && ODIN_INSPECTOR
                Sirenix.Utilities.Editor.GUIHelper.RequestRepaint();
#endif
                return Color.Lerp(Color.white, Color.red, maxPossibleAmountInGame / 100f);
            }

            private Color GetCategoryGroupColorHash()
            {
                var argbString = StringToArgb(categoryGroup);
                ColorUtility.TryParseHtmlString(argbString, out var parsedColor);

                Color.RGBToHSV(parsedColor, out var h, out var s, out var v);
                return Color.HSVToRGB(h, 1, 1);
            }

            private Color GetStoredAmountLimitColor()
            {
                if (storedAmountLimit == "Unique") return new Color(1f, .875f, .1f);

                return Color.white;
            }

            private Color GetCategoryColorHash()
            {
                var argbString = StringToArgb(category);
                ColorUtility.TryParseHtmlString(argbString, out var parsedColor);

                Color.RGBToHSV(parsedColor, out var h, out var s, out var v);
                return Color.HSVToRGB(h, .25f, .9f);
            }

            private string StringToArgb(string text)
            {
                int i = text.GetHashCode();
                return "#" + ((i >> 24) & 0xFF).ToString("X2") + ((i >> 16) & 0xFF).ToString("X2") +
                       ((i >> 8) & 0xFF).ToString("X2") + (i & 0xFF).ToString("X2");
            }
        }

        /// <summary>
        /// Fetches all item data that is flagged as storable in inventory
        /// </summary>
        /// <returns>A flat list of all items that can be stored in inventory</returns>
        public static List<InventoryItemDebugInfos> GetAllCurrentlyStorableItemsDebugInfos()
        {
            if (Catalog.data == null)
                return new List<InventoryItemDebugInfos>();

            try // Avoid error logs from unloaded catalog
            {
                var itemData = Catalog.GetDataList<ItemData>();
                var inventoryCategoryGroups = Catalog.gameData.GetInventoryCategoryGroups();

                return (from data in itemData
                        where data.canBeStoredInPlayerInventory // items that can be stored
                        from categoryGroup in inventoryCategoryGroups
                        where categoryGroup.IsCategoryPathMatching(data.category) // with a correct category path
                        select new InventoryItemDebugInfos(data, categoryGroup))
                    .OrderBy(di => di.categoryGroup)
                    .ThenBy(di => di.category)
                    .ThenBy(di => di.itemName)
                    .ToList();
            }
            catch
            {
                // ignored
            }

            return new List<InventoryItemDebugInfos>();
        }

        #endregion
    }
}
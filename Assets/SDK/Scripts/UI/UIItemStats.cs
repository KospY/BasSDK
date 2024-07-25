using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIItemStats : MonoBehaviour
    {
        public TMP_Text itemName;
        public TMP_Text itemDescription;
        public Image itemTierIcon;
        public List<string> itemIconAddresses = new List<string>() {
            "Bas.Icon.InventorySheet[UI_InventoryIcons_spreadsheet_T0]",
            "Bas.Icon.InventorySheet[UI_InventoryIcons_spreadsheet_T1]",
            "Bas.Icon.InventorySheet[UI_InventoryIcons_spreadsheet_T2]",
            "Bas.Icon.InventorySheet[UI_InventoryIcons_spreadsheet_T3]",
            "Bas.Icon.InventorySheet[UI_InventoryIcons_spreadsheet_T4]",
        };
        public Transform statParent;
        public string itemStatPrefabAddress = "Bas.UI.Inventory.ItemStat";
 // ProjectCore
    }
}

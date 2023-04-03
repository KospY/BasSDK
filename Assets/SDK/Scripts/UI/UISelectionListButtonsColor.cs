using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using TMPro;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UISelectionListButtonsColor : UISelectionListButtons
    {
        public GameObject colorSelectorButtonPrefab;
        public GameObject colorSelectorDeactivatedFillerPrefab;
        
        public int colorSelectorDeactivatedFillerNumber = 30;
        public Transform colorSelectorButtonHolder;
        public UIScrollController colorSelectorScrollController;
        public Transform colorTypeTabs;
        
        public PartColor color = PartColor.Hair;
        public PartColorType colorType = PartColorType.Primary;

        public Color selectedTabColor = Color.white;
        public Color deselectedTabColor = new Color(.7f, .7f, .9f);
        public TextMeshProUGUI hairTab;
        public TextMeshProUGUI skinTab;
        public TextMeshProUGUI eyesTab;
        public TextMeshProUGUI primaryTab;
        public TextMeshProUGUI secondaryTab;

        public bool useSharedColors;

        public enum PartColor
        {
            Hair,
            Eyes,
            Skin,
        }

        public enum PartColorType
        {
            Primary,
            Secondary
        }

    }
}
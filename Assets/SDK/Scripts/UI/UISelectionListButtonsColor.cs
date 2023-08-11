using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
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
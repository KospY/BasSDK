using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        public Toggle primaryToggle;
        public Toggle secondaryToggle;

        public PartColor color = PartColor.Hair;
        public PartColorType colorType = PartColorType.Primary;

        public bool useSharedColors;
        public UICustomisableButton hairToggle;

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
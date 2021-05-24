using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ThunderRoad
{
    public class UISelectionListButtonsColor : UISelectionListButtons
    {
        public PartColor color = PartColor.Hair;

        public enum PartColor
        {
            Hair,
            Eyes,
            Skin,
        }

    }
}
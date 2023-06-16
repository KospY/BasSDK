using System.Collections.Generic;
using ThunderRoad.Modules;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
	public class UISelectionListButtonsMapSelection : UISelectionListButtons
    {
        public RawImage levelImage = null;
        public TextMeshProUGUI levelDescription = null;
        public TextMeshProUGUI levelCount = null;
        public UIMapLevelMode UISelectionLevelMode = null;

    }
}
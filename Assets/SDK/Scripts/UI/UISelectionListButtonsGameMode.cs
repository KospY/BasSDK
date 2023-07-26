using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

namespace ThunderRoad
{
	public class UISelectionListButtonsGameMode : UISelectionListButtons
    {
        public RawImage gameModeImage = null;

        public TextMeshProUGUI gameModeDescription = null;
        public UICustomisableButton selectButton;
        public TextMeshProUGUI gameModeWarning;

    }
}
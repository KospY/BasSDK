using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
	public class UISelectionListButtonsGameMode : UISelectionListButtons
    {
        public RawImage gameModeImage = null;

        public TextMeshProUGUI gameModeDescription = null;
        public UICustomisableButton selectButton;
        public TextMeshProUGUI gameModeWarning;

        public GameObject gameModeConfigPanel;
        public UISelectionListButtonsBool tutorialToggle = null;
        public UISelectionListButtonsGameModeInventoryStart inventoryStartSelectionButton = null;
        public UISelectionListButtonsGameModeDifficultyPreset difficultySelectionButton = null;
    }
}
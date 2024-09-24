using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIWorldMapLevelPresentation : MonoBehaviour
    {
        public UIWorldMapBoard worldMapBoard;

        [Header("Map Infos")]
        public RectTransform levelTitleBox;
        public TextMeshProUGUI levelTitleText;
        public Image mapPreview = null;
        public TextMeshProUGUI modeDescriptionText;

        [Header("Faction Banner")]
        public GameObject enemyBanner;
        public Image bannerlevelIcon;
        public Image enemyBannerIcon;
        public Image enemyBannerBackground;
        public float titleBannerOffset;


        [Header("Level Options selection")]
        public Transform optionsPanel = null;
        public UIMapLevelMode modeSelector = null;
        public GameObject modePrevious;
        public GameObject modeNext;
        public UICustomisableButton nextOptionsButton = null;
        public UICustomisableButton previousOptionsButton = null;
        public TextMeshProUGUI optionsPageText = null;
        public UISelectionListButtonsLevelModeOption LevelOptionPrefab = null;
        public List<UISelectionListButtonsLevelModeOption> levelModeOptions = new();

        [Header("Level instance configuration")]
        public GameObject mapRightPanelConfigurable; // Used to display options category.
        public GameObject mapRightPanelAttrubteDisplay; // Used to display attribute panel category.
        [Space]
        public GameObject attributeDisplayGroupPrefab;
        public Transform attributeDisplayGroupAnchor;
        public float attributeDisplayGroupHeight = 50.0f;


        /// <summary>
        /// Load the level for the selected location and mode
        /// </summary>
        public void Travel()
        {
        }


        /// <summary>
        /// Change to the next mode for the selected map
        /// </summary>
        public void NextMode()
        {
        }

        /// <summary>
        /// Change to the previous mode for the selected map
        /// </summary>
        public void PreviousMode()
        {
        }

        /// <summary>
        /// Change to the next options page for the selected map and selected mode
        /// </summary>
        public void NextOptions()
        {
        }

        /// <summary>
        /// Change to the previous options page for the selected map and selected mode
        /// </summary>
        public void PreviousOptions()
        {
        }

 //ProjectCore
    }
}
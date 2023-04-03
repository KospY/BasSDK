using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class UIWorldMapBoard : MonoBehaviour
    {
        public Canvas worldMapSelectorCanvas;
        public Transform worldMapAnchor;
        public Canvas levelDetails;
        public BoxCollider mapCollider;
        public BoxCollider levelCollider;
        public Transform canvasDetails;
        public bool mapLoops = true;
        public TextMeshProUGUI worldMapLabel;
        public TextMeshProUGUI mapTitleText;
        public TextMeshProUGUI modeDescriptionText;
        public RawImage mapPreview = null;
        public Button travelButton = null;
        public UIWorldMapLocation mapLocationPrefab = null;
        public Transform optionsPanel = null;
        public TextMeshProUGUI descriptionText = null;
        public Button nextOptionsButton = null;
        public Button previousOptionsButton = null;
        public MeshRenderer mapRenderer = null;
        public TextMeshProUGUI pageText = null;
        public UISelectionListButtonsLevelModeOption LevelModeOptionPrefab = null;
        public List<UISelectionListButtonsLevelModeOption> levelModeOptions = new List<UISelectionListButtonsLevelModeOption>();

    }
}
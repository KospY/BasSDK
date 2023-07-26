using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad.Modules;
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
        public TextMeshProUGUI worldMapLabel;
        public TextMeshProUGUI mapTitleText;
        public TextMeshProUGUI modeDescriptionText;
        public RawImage mapPreview = null;
        public UIWorldMapLocation mapLocationPrefab = null;
        public Transform optionsPanel = null;
        public TextMeshProUGUI levelOptionDescriptionText = null;
        public UICustomisableButton nextOptionsButton = null;
        public UICustomisableButton previousOptionsButton = null;
        public MeshRenderer mapRenderer = null;
        public TextMeshProUGUI pageText = null;
        public UISelectionListButtonsLevelModeOption LevelOptionPrefab = null;
        public List<UISelectionListButtonsLevelModeOption> levelModeOptions = new();

    }
}
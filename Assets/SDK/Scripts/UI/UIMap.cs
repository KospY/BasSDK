using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class UIMap : MonoBehaviour
    {
        public string mapId = "Default";
        public Transform locationRoot;
        public ToggleGroup orbGroup;
        public Transform canvasDetails;
        public Transform wholeMap;
        public float visibleRadius = 4;

        public Text gameModTitle;
        public Text mapTitleText;
        public Text modeDescriptionText;

        public RawImage mapSketch = null;

        public Button travelButton = null;

        public Button bulletPointPrefab = null;

        protected int modeIndex = 0;

        public Transform optionsPanel = null;

        public UISelectionListButtonsLevelModeOption LevelModeOptionPrefab = null;
        public List<UISelectionListButtonsLevelModeOption> levelModeOptions = new List<UISelectionListButtonsLevelModeOption>();

        protected float optionSpacingSize = 0.1f;

        protected int currentIndexOption = 0;
        protected int startIndexOption = 0;
        protected int maxPage = 0;
        protected int currentPage = 1;

        public Text descriptionText = null;

        public Button nextOptionsButton = null;
        public Button previousOptionsButton = null;

        protected int currentGameModIndex = 0;

        public Transform mapParent = null;

        protected GameObject currentMap = null;

        public MeshRenderer mapRenderer = null;

        protected UIMapBulletPoint lastSelected = null;

        public Text pageText = null;


        protected void OnDrawGizmos()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 1, 0));
            if (locationRoot != null)
            {
                foreach (Transform child in locationRoot)
                {
                    Gizmos.matrix = child.localToWorldMatrix;
                    Gizmos.DrawRay(Vector3.zero, Vector3.back * 0.1f);
                    Gizmos.DrawWireCube(new Vector3(0, 0, -0.1f - 0.015f), new Vector3(0.07f, 0.07f, 0));
                }
            }
        }
    }
}

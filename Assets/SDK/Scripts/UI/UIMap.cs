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
        public Canvas map;
        public Canvas levelDetails;
        public BoxCollider mapCollider;
        public BoxCollider levelCollider;
        public Transform canvasDetails;
        public bool mapLoops = true;
        public Text gameModTitle;
        public Text mapTitleText;
        public Text modeDescriptionText;
        public RawImage mapSketch = null;
        public Button travelButton = null;
        public UIMapBulletPoint bulletPointPrefab = null;
        public Transform optionsPanel = null;
        public Text descriptionText = null;
        public Button nextOptionsButton = null;
        public Button previousOptionsButton = null;
        public Transform mapParent = null;
        public MeshRenderer mapRenderer = null;
        public Text pageText = null;
        public UISelectionListButtonsLevelModeOption LevelModeOptionPrefab = null;
        public List<UISelectionListButtonsLevelModeOption> levelModeOptions = new List<UISelectionListButtonsLevelModeOption>();

        private Transform locationRoot;
        private ToggleGroup orbGroup;
        private int modeIndex;
        private float optionSpacingSize = 0.1f;
        private int currentIndexOption;
        private int startIndexOption;
        private int maxPage;
        private int currentPage = 1;
        private int mapPageIndex;
        private Canvas currentMap;
        private UIMapBulletPoint lastSelected;


        private void OnDrawGizmos()
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
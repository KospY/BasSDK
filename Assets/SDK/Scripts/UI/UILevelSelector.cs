using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UILevelSelector : MonoBehaviour
    {
        public string mapId = "Default";
        public Transform locationRoot;
        public ToggleGroup orbGroup;
        public Transform canvasDetails;
        public Transform wholeMap;
        public float visibleRadius = 4;

        public Text mapTitleText;
        public Text modeDescriptionText;

        public RawImage mapSketch = null;

        public Button travelButton = null;

        public Button bulletPointPrefab = null;

        protected int modeIndex = 0;

        public Transform optionsPanel = null;

        public UISelectionListButtonsLevelModeOption LevelModeOptionPrefab = null;
        public List<UISelectionListButtonsLevelModeOption> levelModeOptions = new List<UISelectionListButtonsLevelModeOption>();

        protected float optionSpacingSize = 0.09f;

        public Text descriptionText = null;


        protected void OnDrawGizmos()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 1, 0));
            /*if (canvasDetails != null)
            {
                Gizmos.matrix = canvasDetails.localToWorldMatrix;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.402f, 0.29f, 0));
            }*/
            if (locationRoot != null)
            {
                foreach (Transform child in locationRoot)
                {
                    Gizmos.matrix = child.localToWorldMatrix;
                    Gizmos.DrawRay(Vector3.zero, Vector3.back * 0.1f);
                    Gizmos.DrawWireSphere(new Vector3(0, 0, -0.1f - 0.015f), 0.03f);
                }
            }
        }
    }
}

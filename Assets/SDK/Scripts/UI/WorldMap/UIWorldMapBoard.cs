using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad.Modules;
using TMPro;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class UIWorldMapBoard : ThunderBehaviour
    {
        public Canvas worldMapSelectorCanvas;
        public CanvasCuller worldMapCanvasCuller;
        public Transform worldMapAnchor;
        public Transform worldMapCustomInfosAnchor;
        public Canvas levelDetails;
        public BoxCollider mapCollider;
        public BoxCollider levelCollider;
        public Transform canvasDetails;
        public UIWorldMap worldMap;

        [Header("World map Icon")]
        public TextMeshProUGUI worldMapLabel;
        public UIWorldMapLocation mapCardLocationPrefab = null;
        public MeshRenderer mapRenderer = null;

        [Space]
        public UIWorldMapLevelPresentation levelPresentation;

        [Space]
        // small description
        public UIText levelOptionDescriptionText = null;


        [Space]
        public GameObject mapSelectorPrevious;
        public GameObject mapSelectorNext;


        /// <summary>
        /// Switches to the next map page
        /// </summary>
        public void NextWorldMap()
        {
        }

        /// <summary>
        /// Switches to the previous map page
        /// </summary>
        public void PreviousWorldMap()
        {
        }

        /// <summary>
        /// Update the level option description text according to the option being pointed
        /// </summary>
        public void SetLevelOptionDescription(string description)
        {
            levelOptionDescriptionText.text = description;
        }

        /// <summary>
        /// Change to the next location on the currently selected map page
        /// </summary>
        public void SetNextLocation()
        {
        }

        /// <summary>
        /// Change to the previous location on the currently selected map page
        /// </summary>
        public void SetPreviousLocation()
        {
        }

    }
}
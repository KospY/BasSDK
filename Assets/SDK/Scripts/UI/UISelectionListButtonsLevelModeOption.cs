using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class UISelectionListButtonsLevelModeOption : UISelectionListButtons, IPointerEnterHandler, IPointerExitHandler
    {
        public Image starImagePrefab = null;

        public UISelectionListButtonsBool toggle = null;

        public Transform starPlace = null;

        public Color selectedColor = Color.white;
        public Color unselectedColor = new Color(0.5f, 0.5f, 0.5f);

        public string descriptionText = "This is a description test.\n Your text go here.";
        public UIMap uiLevelSelector = null;

        public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
        {
        }

        public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
        {
        }

    }
}
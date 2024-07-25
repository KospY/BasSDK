using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Video;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class UISelectionListButtonsLevelModeOption : UISelectionListButtons, IPointerEnterHandler
    {
        public Image starImagePrefab = null;
        public Sprite starImage = null;
        public Sprite starImageFill = null;

        public UISelectionListButtonsBool toggle = null;
        public Transform starPlace = null;
        public int maxIcon = 5;


        public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
        {
        }
    }
}
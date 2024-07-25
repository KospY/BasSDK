using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldMapLocation : MonoBehaviour
{
    public const int HOVER_CANVAS_SORTED_ORDER = 5;
    public const int SELECTED_CANVAS_SORTED_ORDER = 4;
    public const int UNSELECTED_CANVAS_SORTED_ORDER = 0;

    public Canvas canvas;
    public UICustomisableButton button;
    public TextMeshProUGUI label;
    public RawImage mapIconImage;
    public RawImage highlightedMapIconImage;

    public RectTransform enemyBanner;
    public RawImage enemyIconImage;
    public RawImage enemyBackground;

    public float bannerDeployDuration = 0.1f;
    public float bannerFoldedAnchor = 0.0f;
    public float bannerDeployedAnchor = 1.0f;

    public void OnToogle(bool isSelected)
    {
    }

 // ProjectCore
}
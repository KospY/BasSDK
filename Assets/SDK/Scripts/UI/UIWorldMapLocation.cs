using ThunderRoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PointerEventData = UnityEngine.EventSystems.PointerEventData;
using System;

public class UIWorldMapLocation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    public RawImage image;
    public TextMeshProUGUI label;

    private bool isSelected;

    [NonSerialized]
    public LevelData levelData;
    [NonSerialized]
    public Texture normalTexture;
    [NonSerialized]
    public Texture hoverTexture;

    private void Start()
    {
        image = GetComponent<RawImage>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
        {
            SetHover();
        }
    }

    public void SetHover()
    {
        image.texture = hoverTexture;
    }

    public void SetNormal()
    {
        image.texture = normalTexture;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
        {
            SetNormal();
        }
    }

    public void SetSelected(bool newState)
    {
        isSelected = newState;
        if (isSelected)
        {
            SetHover();
        }
        else
        {
            SetNormal();
        }
    }
}
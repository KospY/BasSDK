using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMapBulletPoint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture normalTexture;
    public Texture hoverTexture;

    private RawImage rawImage = null;

    public bool isSelected = false;

    private void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
            SetHover();
    }

    public void SetHover()
    {
        rawImage.texture = hoverTexture;
    }

    public void SetNormal()
    {
        rawImage.texture = normalTexture;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isSelected)
            SetNormal();
    }

    public void SetSelected(bool newState)
    {
        isSelected = newState;
        if(isSelected)
        {
            SetHover();
        }
        else
        {
            SetNormal();
        }
    }
}

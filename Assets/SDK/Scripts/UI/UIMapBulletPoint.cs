using ThunderRoad;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PointerEventData = UnityEngine.EventSystems.PointerEventData;

public class UIMapBulletPoint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    public RawImage image;
    public Text name;

    private bool isSelected;

    public Texture NormalTexture { set; get; }
    public Texture HoverTexture { set; get; }

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
        image.texture = HoverTexture;
    }

    public void SetNormal()
    {
        image.texture = NormalTexture;
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
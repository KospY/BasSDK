using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonRollhover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite normalButtonSprite = null;
    public Sprite hoverButtonSprite = null;

    [SerializeField] protected Image _image = null;

    private void Start()
    {
        // If image is assigned to another object, do not search for it in self
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.sprite = hoverButtonSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.sprite = normalButtonSprite;
    }

    private void OnDisable()
    {
        if (_image == null) return;

        _image.sprite = normalButtonSprite;
    }
}

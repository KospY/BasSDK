using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIItemSpawnerGridElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected new Text name;
        [SerializeField] protected RawImage icon;
        [SerializeField] protected RawImage frameRollhover;

        public Toggle Toggle { get; private set; }

        public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (Toggle.isOn) return;

            ToggleHoverImage(true);
        }

        public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (Toggle.isOn) return;

            ToggleHoverImage(false);
        }

        private void ToggleHoverImage(bool isVisible)
        {
            frameRollhover.gameObject.SetActive(isVisible);
        }

        private void OnLanguageChanged(string language)
        {
            SetLocalizedFields();
        }

        protected virtual void SetLocalizedFields()
        {

        }
    }
}
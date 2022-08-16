using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIButtonBook : UIButtonRollhover
    {
        private UIText label;
        private Button button;


        public new void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (!button.IsInteractable()) return;

            base.OnPointerEnter(eventData);
        }

        public new void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (!button.IsInteractable()) return;

            base.OnPointerExit(eventData);
        }

    }
}
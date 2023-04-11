using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIItemSpawnerGridElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField] new protected TextMeshProUGUI name;
        [SerializeField] protected RawImage icon;
        [SerializeField] protected RawImage frameRollhover;
  
        public Toggle Toggle { get; private set; }
        public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
        {
        }

        public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
        {
        }

    }
}
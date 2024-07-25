using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIItemSpawnerGridElement : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI elementName;
        [SerializeField] protected Image iconSprite;
        public UICustomisableButton Button { get; private set; }
        
    }
}
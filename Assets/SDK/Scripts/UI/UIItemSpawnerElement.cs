using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIItemSpawnerGridElement : MonoBehaviour
    {
        [SerializeField] protected new TextMeshProUGUI name;
        [SerializeField] protected RawImage icon;

        public UICustomisableButton Button { get; private set; }
        
    }
}
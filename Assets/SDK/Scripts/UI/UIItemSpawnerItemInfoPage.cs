using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIItemSpawnerItemInfoPage : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI blacksmith;
        [SerializeField] private TextMeshProUGUI tier;
        [SerializeField] private TextMeshProUGUI weight;
        [SerializeField] private TextMeshProUGUI size;
        [SerializeField] private TextMeshProUGUI description;

        [SerializeField] private RawImage icon;

        private Texture addressableTexture;

        public ItemData CurrentItem { get; private set; }
    }
}
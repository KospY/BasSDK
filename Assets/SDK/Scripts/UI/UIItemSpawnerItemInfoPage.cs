using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIItemSpawnerItemInfoPage : MonoBehaviour
    {
        [SerializeField] private Text itemName;
        [SerializeField] private Text blacksmith;
        [SerializeField] private Text tier;
        [SerializeField] private Text weight;
        [SerializeField] private Text size;
        [SerializeField] private Text description;

        [SerializeField] private RawImage icon;

    }
}
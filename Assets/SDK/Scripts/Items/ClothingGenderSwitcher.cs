using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/ClothingGenderSwitcher.html")]
    [AddComponentMenu("ThunderRoad/Items/Clothing Gender Switcher")]
    [RequireComponent(typeof(Item))]
    public class ClothingGenderSwitcher : MonoBehaviour
    {
        [Tooltip("Male wearable item (Select item, not model)")]
        public GameObject maleModel;
        [Tooltip("Main left handle of the male item.")]
        public Handle mainMaleHandleRight;
        [Tooltip("Main right handle of item.")]
        public Handle mainMaleHandleLeft;
        [Space]
        [Tooltip("Female wearable item(Select item, not model")]
        public GameObject femaleModel;
        [Tooltip("Main left handle of the female item.")]
        public Handle mainFemaleHandleRight;
        [Tooltip("Main right handle of the female item.")]
        public Handle mainFemaleHandleLeft;

    }
}

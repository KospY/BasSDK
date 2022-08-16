using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ClothingGenderSwitcher")]
    [AddComponentMenu("ThunderRoad/Items/Clothing Gender Switcher")]
    [RequireComponent(typeof(Item))]
    public class ClothingGenderSwitcher : MonoBehaviour
    {
        public GameObject maleModel;
        public Handle mainMaleHandleRight;
        public Handle mainMaleHandleLeft;
        [Space]
        public GameObject femaleModel;
        public Handle mainFemaleHandleRight;
        public Handle mainFemaleHandleLeft;

    }
}

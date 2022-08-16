using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Chabuk.ManikinMono;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Equipment")]
    [AddComponentMenu("ThunderRoad/Creatures/Equipment")]
    public class Equipment : MonoBehaviour
    {
        public delegate void OnArmourEquipped(Wearable slot, Item item);
        public delegate void OnArmourUnEquipped(Wearable slot, Item item);

        public bool canSwapExistingArmour = true;
        public bool equipWardrobesOnLoad = true;
        public bool armourEditModeEnabled = false;

        public enum WardRobeCategory
        {
            Apparel,
            Body
        }

    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Wearable")]
    [AddComponentMenu("ThunderRoad/Wearable")]
    public class Wearable : Interactable
    {
        public delegate void OnItemEquipped(Item item);
        public delegate void OnItemUnEquipped(string layer, Item item);
        public delegate void OnEditModeChanged(bool state);

        public string wardrobeChannel;

    }
}

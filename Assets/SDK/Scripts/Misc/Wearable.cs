using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;
using ThunderRoad.Manikin;

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

#if ODIN_INSPECTOR
        [PropertySpace(0, 10), BoxGroup("WARDROBE", true, true), ValueDropdown("GetAllChannels")]
#endif
        public string wardrobeChannel;

#if ODIN_INSPECTOR
        [BoxGroup("WARDROBE", true, true)]
#endif
        public WearableEntry[] wardrobeLayers;

        // Odin won't display the list without this...
        // Forgive me
        [System.Serializable]
        public class WearableEntry
        {
            public string layer;
        }

    }
}

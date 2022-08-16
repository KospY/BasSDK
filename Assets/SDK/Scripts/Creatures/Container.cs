using System.Collections.Generic;
using UnityEngine;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Container")]
    [AddComponentMenu("ThunderRoad/Container")]
    public class Container : MonoBehaviour
    {
        public LoadContent loadContent = LoadContent.None;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllContainerID")]
#endif
        public string containerID;

        public bool saveOnLevelUnload;
        public string saveToPlayerContainerID;

        public enum LoadContent
        {
            None,
            ContainerID,
            PlayerInventory,
            Purchasable,
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllContainerID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Container);
        }

#endif

    }
}

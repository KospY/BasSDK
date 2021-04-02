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
    [AddComponentMenu("ThunderRoad/Container")]
    public class Container : MonoBehaviour
    {
        public LoadContent loadContent = LoadContent.None;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllContainerID")]
#endif
        public string containerID;

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

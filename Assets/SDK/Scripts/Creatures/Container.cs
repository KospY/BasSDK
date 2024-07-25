using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Creatures/Container.html")]
    [AddComponentMenu("ThunderRoad/Container")]
    public class Container : MonoBehaviour
    {
        public LoadContent loadContent = LoadContent.None;
        public string loadPlayerContainerID;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllContainerID))]
#endif
        public string containerID;
        public bool loadOnStart = true;

        public Item.Owner spawnOwner = Item.Owner.None;
        public List<Holder> linkedHolders = new List<Holder>();

        public enum LoadContent
        {
            None,
            ContainerID,
            PlayerInventory,
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllContainerID()
        {
            return Catalog.GetDropdownAllID(Category.Container);
        }

#endif

    }
}


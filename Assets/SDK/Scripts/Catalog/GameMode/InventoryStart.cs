using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;

namespace ThunderRoad
{
    [Serializable]
    public class InventoryStart
    {
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllContainerID))]
#endif
        public string containerID;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextGroupID))]
#endif
        public string textGroupId;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextId))]
#endif
        public string titleTextID;

        [ShowIf(nameof(titleTextID), null)]
        public string titleText;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllTextId))]
#endif
        public string descriptionTextID;
        
        [ShowIf(nameof(descriptionTextID), null)]
        public string descriptionText;

        public string titleImageAddress;

        [ShowInInspector]
        public Dictionary<string, float> currencies;

        [NonSerialized]
        public Sprite titleImageSprite;
        public event Action<Sprite> onSpriteTitleImageLoade;


#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllContainerID()
        {
            return Catalog.GetDropdownAllID(Category.Container);
        }

        public List<ValueDropdownItem<string>> GetAllTextGroupID()
        {
            return Catalog.GetTextData().GetDropdownAllTextGroups();
        }

        public List<ValueDropdownItem<string>> GetAllTextId()
        {
            return Catalog.GetTextData().GetDropdownAllTexts(textGroupId);
        }
#endif //ODIN_INSPECTOR
    }
}
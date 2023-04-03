using System.Collections.Generic;
using TMPro;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public class ItemModulePotion : ItemModule
    {
        public List<LiquidData.Content> contents;
        public float maxLevel = 50;

        public LayerMask collisionLayer;

        public float flowSpeed = 1;
        public float flowMinAngle = 70;
        public float flowMaxAngle = 100;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")] 
#endif
        public string effectFlowId = "PotionFlow";

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllItemID()
        {
            return Catalog.GetDropdownAllID(Category.Item);
        }

        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
        } 
#endif

    }
}

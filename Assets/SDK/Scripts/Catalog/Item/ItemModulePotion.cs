using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	public class ItemModulePotion : ItemModule
    {
        public List<LiquidData.Content> contents;
        public float maxLevel = 50;
        public bool popCorkOnAltUse;
        public float corkPopForce = 2f;
        public ForceMode corkPopForceMode = ForceMode.Impulse;

        public LayerMask collisionLayer;

        [GradientUsage(true)]
        public Gradient healthIndicatorGradient;
        [GradientUsage(true)]
        public Gradient levelIndicatorGradient;

        public float flowSpeed = 1;
        public float flowMinAngle = 70;
        public float flowMaxAngle = 100;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string effectFlowId = "PotionFlow";

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string healthEffectId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string levelEffectId;
        private EffectData healthEffectData;
        private EffectData levelEffectData;

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

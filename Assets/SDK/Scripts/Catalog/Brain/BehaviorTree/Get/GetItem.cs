using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Get
{
	public class GetItem : ActionNode
    {
        public float searchRadius = 30f;
        public bool checkCanSeeItem = true;
#if ODIN_INSPECTOR
        [HorizontalGroup("Horiz1", MinWidth = 170), LabelWidth(140)] 
#endif
        public bool checkID = false;
#if ODIN_INSPECTOR
        [HorizontalGroup("Horiz1"), EnableIf("checkID"), LabelWidth(120)] 
#endif
        public string itemID = "";
#if ODIN_INSPECTOR
        [HorizontalGroup("Horiz2", MinWidth = 170), LabelWidth(140)] 
#endif
        public bool checkType = true;
#if ODIN_INSPECTOR
        [HorizontalGroup("Horiz2"), EnableIf("checkType"), LabelWidth(120)] 
#endif
        public ItemData.Type type = ItemData.Type.Weapon;
#if ODIN_INSPECTOR
        [HorizontalGroup("Horiz3", MinWidth = 170), LabelWidth(140)] 
#endif
        public bool checkClass = true;
#if ODIN_INSPECTOR
        [HorizontalGroup("Horiz3"), EnableIf("checkClass"), LabelWidth(120)] 
#endif
        public ItemModuleAI.WeaponClass weaponClass = ItemModuleAI.WeaponClass.Melee;
#if ODIN_INSPECTOR
        [HorizontalGroup("Horiz4", MinWidth = 170), LabelWidth(140)] 
#endif
        public bool checkHandling = true;
#if ODIN_INSPECTOR
        [HorizontalGroup("Horiz4"), EnableIf("checkHandling"), LabelWidth(120)] 
#endif
        public ItemModuleAI.WeaponHandling weaponHandling = ItemModuleAI.WeaponHandling.OneHanded;
#if ODIN_INSPECTOR
        [HorizontalGroup("Horiz5", MinWidth = 170), LabelWidth(140)] 
#endif
        public bool checkGrabbableSide = true;
#if ODIN_INSPECTOR
        [HorizontalGroup("Horiz5"), EnableIf("checkGrabbableSide"), LabelWidth(120)] 
#endif
        public Side handleSide = Side.Right;

        public float avoidTargetRadius = 4;

        public bool requirePathAvailable = true;
        public bool useShortestPath = true;

        public string outputItemVariableName = "";
        public string outputItemTransformVariableName = "";
        public string outputItemHandleVariableName = "";
        public string outputItemHandleTransformVariableName = "";

    }
}

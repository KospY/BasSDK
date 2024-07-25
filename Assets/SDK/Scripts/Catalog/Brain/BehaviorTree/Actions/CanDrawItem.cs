using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class CanDrawItem : ActionNode
    {

        public string targetTransformVariableName = "";

#if ODIN_INSPECTOR
        [LabelWidth(130)] 
#endif
        public bool drawRightWeapon;
#if ODIN_INSPECTOR
        [EnableIf("drawRightWeapon"), LabelWidth(130)] 
#endif
        public Equipment.WeaponDrawInfo rightInfo = new Equipment.WeaponDrawInfo();
        //public ItemModuleAI.WeaponClass rightWeaponClass = ItemModuleAI.WeaponClass.None;
        //[HorizontalGroup("Right"), EnableIf("drawRightWeapon"), LabelWidth(150)]
        //public ItemModuleAI.WeaponHandling rightWeaponHandling = ItemModuleAI.WeaponHandling.None;
        //[HorizontalGroup("Right"), EnableIf("drawRightWeapon"), LabelWidth(130)]
        //public bool checkRightAmmo = true;

#if ODIN_INSPECTOR
        [LabelWidth(130)] 
#endif
        public bool drawLeftWeapon;
#if ODIN_INSPECTOR
        [EnableIf("drawLeftWeapon"), LabelWidth(130)] 
#endif
        public Equipment.WeaponDrawInfo leftInfo = new Equipment.WeaponDrawInfo();
        //public ItemModuleAI.WeaponClass leftWeaponClass = ItemModuleAI.WeaponClass.None;
        //[HorizontalGroup("Left"), EnableIf("drawRightWeapon"), LabelWidth(150)]
        //public ItemModuleAI.WeaponHandling leftWeaponHandling = ItemModuleAI.WeaponHandling.None;
        //[HorizontalGroup("Left"), EnableIf("drawRightWeapon"), LabelWidth(130)]
        //public bool checkLeftAmmo = true;

    }
}

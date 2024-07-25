using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI
{
	public class ChildTreeNode : Node
    {
        public enum ChildNodeRef
        {
            ForceID,
            DynamicID,
        }

#if ODIN_INSPECTOR
        [HorizontalGroup("Reference"), EnumToggleButtons, HideLabel] 
#endif
        public ChildNodeRef reference = ChildNodeRef.ForceID;
#if ODIN_INSPECTOR
        [EnableIf("reference", ChildNodeRef.ForceID), ValueDropdown(nameof(GetAllBehaviorTreeID))] 
#endif
        public string childTreeID;
#if ODIN_INSPECTOR
        [EnableIf("reference", ChildNodeRef.DynamicID)] 
#endif
        public string childTreeName;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllBehaviorTreeID()
        {
            return Catalog.GetDropdownAllID(Category.BehaviorTree);
        } 
#endif

    }

}
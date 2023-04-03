using System.Collections.Generic;
using System.Linq;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Get
{
	public class AvailableWeapons
    {
        public Item rightMelee1h;
        public Item leftMelee1h;
        public float meleeDistance1h;

        public Item melee2h;
        public float meleeDistance2h;

        public Item bow;

        public Item shield;
        public SpellData spell;
    }

    public class GetAvailableWeapons : ActionNode
    {
        public string outputAvailableAttacksVariableName = "AvailableWeapons";

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllSubTreesID")] 
#endif
        public List<string> priorityTreeID = new List<string>();

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllSubTreesID()
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            dropdownList.Add(new ValueDropdownItem<string>("None", ""));
            List<string> list2 = Catalog.GetDataList(Category.BehaviorTree).OfType<BehaviorTreeData>().Where(x => x.type == BehaviorTreeData.Type.SubTree).Select(x => x.id).ToList();
            foreach (string id in list2)
            {
                dropdownList.Add(new ValueDropdownItem<string>(id, id));
            }
            return dropdownList;
        } 
#endif

    }
}

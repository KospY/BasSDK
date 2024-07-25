using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class LootConfigData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Data"), ValueDropdown(nameof(GetAllLootTableID))]
#endif
        public string sideRoomLootTableID;

#if ODIN_INSPECTOR
        [BoxGroup("Data"), ValueDropdown(nameof(GetAllLootTableID))]
#endif
        public string enemyDropLootTableID;

#if ODIN_INSPECTOR
        [BoxGroup("Data"), ValueDropdown(nameof(GetAllLootTableID))]
#endif
        public string treasureLootTableID;

#if ODIN_INSPECTOR
        [BoxGroup("Data"), ValueDropdown(nameof(GetAllLootTableID))]
#endif
        public string rewardLootTableID;

#if ODIN_INSPECTOR
        [BoxGroup("Data"), ValueDropdown(nameof(GetAllLootTableID))]
#endif
        public string altSideRoomLootTableID;

#if ODIN_INSPECTOR
        [BoxGroup("Data"), ValueDropdown(nameof(GetAllLootTableID))]
#endif
        public string altTreasureLootTableID;


#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllLootTableID()
        {
            var dropdownList = new List<ValueDropdownItem<string>> {new("None", "")};
            var creatureTableIds = Catalog.GetDataList(Category.LootTable).OfType<LootTableBase>().Select(x => x.id).ToList();
            foreach (var id in creatureTableIds)
            {
                dropdownList.Add(new ValueDropdownItem<string>(id, id));
            }
            return dropdownList;
        }
#endif
    }
}
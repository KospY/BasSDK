using UnityEngine;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace BS
{
    public class ItemSpawner : MonoBehaviour
    {
#if ProjectCore
        [ValueDropdown("GetAllItemID")]
#endif
        public string itemId;
        public bool pooled;
        public bool spawnOnStart = true;

#if ProjectCore
        public List<ValueDropdownItem<string>> GetAllItemID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Item);
        }

        protected void Start()
        {
            if (spawnOnStart) Spawn();
        }

        [Button]
        public Item Spawn()
        {
            if (itemId != "" && itemId != null)
            {
                Item item = Catalog.GetData<ItemData>(itemId).Spawn(pooled);
                item.transform.MoveAlign(item.definition.holderPoint, this.transform);
                return item;
            }
            return null;
        }
#endif
    }
}

using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public abstract class LootTableBase : CatalogData
    {
        public virtual ItemData PickOne(int level = 0, int searchDepth = 0, System.Random randomGen = null)
        {
            return null;
        }

        // Can return null
        public virtual List<ItemData> Pick(int level = 0, int searchDepth = 0, System.Random randomGen = null)
        {
            return null;
        }
        
        public virtual List<ItemData> GetAll(int level = -1, int pickCount = 0)
        {
            List<ItemData> itemDataList = new List<ItemData>();
            return itemDataList;
        }

        public virtual bool DoesLootTableContainItemID(string id, int level = -1, int depth = 0)
        {
	        return false;
        }
        
        public virtual void RenameItem(string itemId, string newName)
        {
	        
        }
    }
}

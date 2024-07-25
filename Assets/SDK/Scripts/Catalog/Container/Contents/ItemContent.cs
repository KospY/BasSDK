using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [System.Serializable]
    public class ItemContent : ItemOrTableContent<ItemData, ItemContent>
    {

        public override ItemContent CloneGeneric()
        {
            //only clone if the list isnt null or empty to avoid overhead
            List<ContentCustomData> contentCustomDatas = customDataList.IsNullOrEmpty() ? null : customDataList.CloneJson();
            ContentState contentState = state?.Clone();
            ItemContent clone = new ItemContent(referenceID, contentState, contentCustomDatas, quantity);
            clone.quantity = quantity; // override quantity because it can cause issue with stackables
            return clone;
        }


        public ItemContent() { }

        public ItemContent(ItemData itemData, ContentState state = null, List<ContentCustomData> customDataList = null, int quantity = 1)
        {
            referenceID = itemData.id;
            this.state = state;
            this.customDataList = customDataList;
        }

        public ItemContent(string referenceID, ContentState state = null, List<ContentCustomData> customDataList = null, int quantity = 1)
        {
            this.referenceID = referenceID;
            this.state = state;
            this.customDataList = customDataList;
        }

    }
}

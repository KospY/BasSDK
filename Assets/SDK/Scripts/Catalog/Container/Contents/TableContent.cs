using System.Collections.Generic;

#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [System.Serializable]
    public class TableContent : ItemOrTableContent<LootTable, TableContent>
    {
        public override TableContent CloneGeneric()
        {
            //only clone if the list isnt null or empty to avoid overhead
            List<ContentCustomData> contentCustomDatas = customDataList.IsNullOrEmpty() ? null : customDataList.CloneJson();
            return new TableContent(referenceID, state?.Clone(), contentCustomDatas, quantity);
        }

        public override ContainerContent GetEndContent()
        {
            ItemData pick = data.PickOne();
            if (pick == null)
                return null;
            //only clone if the list isnt null or empty to avoid overhead
            List<ContentCustomData> contentCustomDatas = customDataList.IsNullOrEmpty() ? null : customDataList.CloneJson();
            return new ItemContent(pick, state?.Clone(), contentCustomDatas, quantity);
        }

        public TableContent() { }

        public TableContent(string referenceID, ContentState state = null, List<ContentCustomData> customDataList = null, int quantity = 1)
        {
            this.referenceID = referenceID;
            this.state = state;
            this.customDataList = customDataList;
        }

        public TableContent(LootTable lootTable, ContentState state = null, List<ContentCustomData> customDataList = null, int quantity = 1)
        {
            this.referenceID = lootTable.id;
            this.state = state;
            this.customDataList = customDataList;
        }

        public override string GetOutput()
        {
            return base.GetOutput();
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections;

namespace ThunderRoad
{
    public class ItemModule 
    {
        [JsonMergeKey, JsonProperty("$type"), JsonIgnore]
        public Type type => GetType();

        [NonSerialized]
        public ItemData itemData;
        protected Item item;

        public virtual void OnItemLoaded(Item item)
        {
            this.item = item;
        }

        public virtual void OnItemDataRefresh(ItemData data)
        {
            itemData = data;
        }

        public virtual IEnumerator LoadAddressableAssetsCoroutine(ItemData data)
        {
            return null;
        }

        public virtual void ReleaseAddressableAssets()
        {

        }
    }
}

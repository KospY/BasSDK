using System;
using System.Collections;
using Newtonsoft.Json;

namespace ThunderRoad
{
	public class ItemModule
    {
        [JsonMergeKey, JsonProperty("$type"), JsonIgnore]
        public Type type
        {
            get { return GetType(); }
            set { }
        }

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
            yield return Yielders.EndOfFrame;
        }

        public virtual void ReleaseAddressableAssets()
        {

        }     
    }
}

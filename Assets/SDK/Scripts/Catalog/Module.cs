using System;
using System.Collections;
using Newtonsoft.Json;

namespace ThunderRoad
{
    public abstract class Module
    {
        [JsonMergeKey, JsonProperty("$type"), JsonIgnore]
        public Type type => GetType();

        public virtual IEnumerator OnLoadCoroutine()
        {
            yield break;
        }

        public virtual void OnErrorThrown(Exception error)
        { 
        
        }

        
        public virtual void Update()
        {

        }

        public virtual void OnUnload()
        {

        }
    }
}

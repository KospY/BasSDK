using Newtonsoft.Json;
using System;
using System.Collections;

namespace ThunderRoad
{
    public class LevelModule
    {
        [NonSerialized]
        public Level level;

        [JsonMergeKey, JsonProperty("$type"), JsonIgnore]
        public Type type
        {
            get { return GetType(); }
            set { }
        }

        public virtual IEnumerator OnLoadCoroutine()
        {
            yield break;
        }

        public virtual void OnErrorThrown(System.Exception error)
        { 
        
        }

        public virtual IEnumerator OnPlayerSpawnCoroutine()
        {
            yield break;
        }

        public virtual void Update()
        {

        }

        public virtual void OnUnload()
        {

        }
    }
}

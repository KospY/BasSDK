namespace ThunderRoad
{
    using Newtonsoft.Json;
    using System;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif //ODIN_INSPECTOR

    public abstract class MusicDynamicModule
    {
        [JsonMergeKey, JsonProperty("$type"), JsonIgnore]
        public Type type
        {
            get { return GetType(); }
            set { }
        }

        public bool isDefault = true;

#if ODIN_INSPECTOR
        [ShowInInspector]
        [ReadOnly]
#endif
        [NonSerialized]
        protected bool _isConnected = false;

        public bool IsConnected => _isConnected;
        public virtual void Connect()
        {
            _isConnected = true;
        }
        public virtual void Disconnect()
        {
            _isConnected = false;
        }
    }
}

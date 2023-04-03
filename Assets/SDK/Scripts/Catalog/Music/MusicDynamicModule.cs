namespace ThunderRoad
{
    using Newtonsoft.Json;
    using System;

    public abstract class MusicDynamicModule
    {
        [JsonMergeKey, JsonProperty("$type"), JsonIgnore]
        public Type type
        {
            get { return GetType(); }
            set { }
        }

        [NonSerialized]
        protected bool _isConnected = false;

        public bool IsConnected() { return _isConnected; }
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

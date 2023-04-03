using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace ThunderRoadVRKBSharedData
{

    public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private List<TKey> keyData = new List<TKey>();
	
        [SerializeField, HideInInspector]
        private List<TValue> valueData = new List<TValue>();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.Clear();
            for (int i = 0; i < this.keyData.Count && i < this.valueData.Count; i++)
            {
                this[this.keyData[i]] = this.valueData[i];
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.keyData.Clear();
            this.valueData.Clear();

            foreach (var item in this)
            {
                this.keyData.Add(item.Key);
                this.valueData.Add(item.Value);
            }
        }
    }
    
    [Serializable]
    public class StringLayerDictionary : UnitySerializedDictionary<string, KeyboardLayerConfiguration> { }
    
    [Serializable]
    public class StringKeyDictionary : UnitySerializedDictionary<string, KeyConfiguration> { }

    
    [Serializable]
    public class KeyboardConfiguration
    {
        public string defaultLayerName = "defaultLayer";
        public KeyProperties KeyProperties = new KeyProperties() {
            fontColor = Color.black,
            fontSize = 6f,
            imageColor = Color.clear,
        };
        public StringLayerDictionary layers;

        public bool TryGetLayerKey(string layer, string keyId, out KeyboardLayerConfiguration keyboardLayerConfiguration, out KeyConfiguration keyConfiguration)
        {
            keyConfiguration = null;
            keyboardLayerConfiguration = null;
            return layers.TryGetValue(layer, out keyboardLayerConfiguration) && keyboardLayerConfiguration.keys.TryGetValue(keyId, out keyConfiguration);
        }
    }

    [Serializable]
    public class KeyboardLayerConfiguration
    {
        public string inheritsFromLayer;
        [ItemCanBeNull]
        public StringKeyDictionary keys;
    }

    [Serializable]
    public class KeyConfiguration
    {
        public string label;
        public KeyActionTypes actionType = KeyActionTypes.OutputLabel;
        public string actionArg;
        public KeyProperties overrideProperties;
    }

    [Serializable]
    public class KeyProperties
    {
        public static readonly KeyProperties DefaultInstance = new KeyProperties();
        public float fontSize;
        public Color fontColor;
        public string imageAddress;
        [NonSerialized]
        public Sprite image;
        public Color imageColor;

        protected bool Equals(KeyProperties other)
        {
            return fontSize.Equals(other.fontSize)
                   && fontColor.Equals(other.fontColor)
                   && imageAddress == other.imageAddress
                   && imageColor.Equals(other.imageColor);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((KeyProperties)obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(fontSize, fontColor, imageAddress, imageColor);
        }
        public static bool operator ==(KeyProperties left, KeyProperties right)
        {
            return Equals(left, right);
        }
        public static bool operator !=(KeyProperties left, KeyProperties right)
        {
            return !Equals(left, right);
        }
    }

    [Serializable]
    public enum KeyActionTypes
    {
        None,
        Backspace,
        OutputLabel,
        Output,
        EnableLayer,
        EnableLayerForNextKey,
        Cancel,
        Confirm
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace ThunderRoadVRKBSharedData
{

    public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
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
            keyboardLayerConfiguration = null;
            keyConfiguration = null;
            return false;
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
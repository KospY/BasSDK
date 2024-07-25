using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using ThunderRoadVRKBSharedData;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Keyboard")]
    [AddComponentMenu("ThunderRoad/Keyboard")]
    public class Keyboard : ThunderBehaviour
    {
        //This is a wrapper around VRKB asset
        #region inner classes

        public struct KeyEvent
        {
            public string keyId;
            public KeyActionTypes actionType;
            public string layer;
            public string label;
            public string arg;
            public bool pressed;
            public float pressStartTime;
            public bool repeating;
            public float prevRepeatTime;
               
        }
     
        [System.Serializable]
        public class OnKeyEvent : UnityEvent<KeyEvent> {}
        [System.Serializable]
        public class OnCancelEvent : UnityEvent<string> {}    
        [System.Serializable]
        public class OnConfirmEvent : UnityEvent<string> {}
        #endregion

        // Events
        public OnKeyEvent OnKey;
        public OnCancelEvent OnCancel;
        public OnConfirmEvent OnConfirm;

        public Material UnpressedKeyMaterial;
        public Material HoverKeyMaterial;
        public Material PressedKeyMaterial;
        
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public KeyboardData data;
        
        [NonSerialized]
        public AsyncOperationHandle<GameObject> addressableHandle;

        public delegate void SpawnEvent(EventTime eventTime);
        public event SpawnEvent OnSpawnEvent;
        public event SpawnEvent OnDespawnEvent;
        

    }
}

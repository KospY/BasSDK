using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
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
        

        public static Keyboard instance;

        private void Awake()
        {
            instance = this;
        }

        protected virtual void Start()
        {
            //Add the keyboard behaviour
        }

        public virtual void Load(KeyboardData keyboardData)
        {
            //most of this is a wrapper for VRKB, so its inside ProjectCore
            data = keyboardData;
            
        }
        
        
        public virtual void InvokeOnSpawnEvent(EventTime eventTime)
        {
            OnSpawnEvent?.Invoke(eventTime);
        }

        public void SetPlaceholderText(string placeholderText)
        {
            
        }

        public void SetTypedText(string typedText)
        {
            
        }


#if UNITY_EDITOR
        [Button]
        public void Load(string keyboardId)
        {
            KeyboardData keyboardData = Catalog.GetData<KeyboardData>(keyboardId);
            if (keyboardData != null)
            {
                Load(keyboardData);
            }
        }
#endif

        /// <summary>
        /// Show the instantiated keyboard.
        /// </summary>
        /// <param name="placeHolderText">The custom placeholder text should already be localized in the selected language</param>
        /// <param name="typedText">Text that will be displayed as already typed. Useful to to show any previously typed characters when the keyboard is re-opened</param>
        public void Show(string placeHolderText = null, string typedText = null)
        {
            if (placeHolderText != null)
            {
                SetPlaceholderText(placeHolderText);
            }

            if (!typedText.IsNullOrEmptyOrWhitespace())
            {
                SetTypedText(typedText);
            }

            gameObject.SetActive(true);
        }

        public void Show(Vector3 position, Quaternion rotation, Transform parent, string placeholderText = null, string typedText = null)
        {
            transform.SetParent(parent, true);
            transform.SetPositionAndRotation(position, rotation);

            Show(placeholderText, typedText);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // DESPAWN
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Keep these method signatures outside so modders can call these events using Unity events
        [Button]
        public void Despawn(float delay)
        {
        }

        [ContextMenu("Despawn")]
        [Button]
        public virtual void Despawn()
        {
        }
    }
}

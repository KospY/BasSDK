using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoadVRKBSharedData;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class KeyboardData : CatalogData
    {
#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllTextId()
        {
            return Catalog.GetDropdownAllID(Category.Text);
        }
#endif
        // PREFAB
#if ODIN_INSPECTOR
        [BoxGroup("Prefab")]
#endif
        public string prefabAddress;
        [NonSerialized]
        public IResourceLocation prefabLocation;
#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [BoxGroup("Prefab"), LabelText("EditorPrefab"), NonSerialized, ShowInInspector, DisableInPlayMode, OnValueChanged("EditorUpdatePrefabAddress"), PreviewField]
#endif
        public GameObject editorPrefab;
#endif
#if ODIN_INSPECTOR
        [BoxGroup("Supported Languages"), ValueDropdown("GetAllTextId")]
#endif
        public HashSet<string> localizationIds;
#if ODIN_INSPECTOR
        [BoxGroup("Keyboard Configuration")]
#endif
        public int RepeatDelayMilliseconds = 500;
#if ODIN_INSPECTOR
        [BoxGroup("Keyboard Configuration")]
#endif
        public int RepeatRatePerSecond = 15;
#if ODIN_INSPECTOR
        [BoxGroup("Keyboard Configuration")]
#endif
        public bool AllowSimultaneousKeyPresses = false;
#if ODIN_INSPECTOR
        [BoxGroup("Keyboard Configuration")]
#endif
        public string placeholderText = "Enter Text..";
#if ODIN_INSPECTOR
        [BoxGroup("Keyboard Configuration")]
#endif
        public bool PasswordMode = false;
#if ODIN_INSPECTOR
        [BoxGroup("Keyboard Configuration")]
#endif
        public KeyboardConfiguration keyboardConfig;
        
        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
#if UNITY_EDITOR
            editorPrefab = Catalog.EditorLoad<GameObject>(prefabAddress);
#endif
        }

        public override IEnumerator OnCatalogRefreshCoroutine()
        {
            yield return Catalog.LoadLocationCoroutine<GameObject>(prefabAddress, value => prefabLocation = value, id);

            if (!string.IsNullOrEmpty(keyboardConfig.KeyProperties.imageAddress))
            {
                yield return Catalog.LoadAssetCoroutine<Sprite>(keyboardConfig.KeyProperties.imageAddress, sprite => keyboardConfig.KeyProperties.image = sprite, id);
            }

            //iterate over keys, spawning any images that are needed and store them on the keys
            
            foreach (var keyboardConfigLayer in keyboardConfig.layers)
            {
                foreach (var key in keyboardConfigLayer.Value.keys)
                {
                    if (string.IsNullOrEmpty(key.Value.overrideProperties.imageAddress)) continue;
                    yield return Catalog.LoadAssetCoroutine<Sprite>(key.Value.overrideProperties.imageAddress, sprite => key.Value.overrideProperties.image = sprite, id);
                }
            }
        }

        public void SpawnAsync(Action<Keyboard> callback, float scale = 1f, string placeHolderText = null, Vector3? position = null, Quaternion? rotation = null, Transform parent = null)
        {
            Keyboard keyboard = null;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GameObject keyboardGo = UnityEditor.PrefabUtility.InstantiatePrefab(Catalog.EditorLoad<GameObject>(prefabAddress), parent) as GameObject;
                if (keyboardGo)
                {
                    keyboard = keyboardGo.GetComponent<Keyboard>();
                    keyboard.transform.SetParent(parent, true);
                    if (position != null) keyboard.transform.position = (Vector3)position;
                    if (rotation != null) keyboard.transform.rotation = (Quaternion)rotation;
                    callback.Invoke(keyboard);
                }
                return;
            }
#endif
            
        }


#if UNITY_EDITOR
        protected void EditorUpdatePrefabAddress()
        {
            if (editorPrefab) prefabAddress = Catalog.GetAddressFromPrefab(editorPrefab);
        }
#endif
    }
}

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
using TriInspector;
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
        [BoxGroup("Prefab"), LabelText("EditorPrefab"), NonSerialized, ShowInInspector, DisableInPlayMode, OnValueChanged(nameof(EditorUpdatePrefabAddress)), PreviewField]
#endif
        public GameObject editorPrefab;
#endif
#if ODIN_INSPECTOR
        [BoxGroup("Supported Languages"), ValueDropdown(nameof(GetAllTextId))]
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
#if UNITY_EDITOR
        public override void CatalogEditorRefresh()
        {
            editorPrefab = Catalog.EditorLoad<GameObject>(prefabAddress);
        }
        protected void EditorUpdatePrefabAddress()
        {
            if (editorPrefab) prefabAddress = Catalog.GetAddressFromPrefab(editorPrefab);
        }

#endif // UNITY_EDITOR
    }
}

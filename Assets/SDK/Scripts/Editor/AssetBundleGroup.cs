using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.AddressableAssets.Settings;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Editor/Asset bundles group")]
    public class AssetBundleGroup : ScriptableObject
    {
        public string folderName;

        [HideInInspector]
        public bool isDefault;

        public List<AddressableAssetGroup> addressableAssetGroups;

        [HideInInspector]
        public bool selected;
        [HideInInspector]
        public bool exportAfterBuild;
    }
}
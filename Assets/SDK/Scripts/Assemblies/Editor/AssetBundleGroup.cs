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

        [Header("Mod manifest")]
        public bool exportModManifest;
#if ODIN_INSPECTOR
        [ShowIf("exportModManifest")]
#endif
        public string modName = "Unknown";
#if ODIN_INSPECTOR
        [ShowIf("exportModManifest")]
#endif
        public string modDescription = "Unknown";
#if ODIN_INSPECTOR
        [ShowIf("exportModManifest")]
#endif
        public string modAuthor = "Unknown";
#if ODIN_INSPECTOR
        [ShowIf("exportModManifest")]
#endif
        public string modVersion = "1.0";
#if ODIN_INSPECTOR
        [ShowIf("exportModManifest")]
#endif
        public string modThumbnail;
    }
}
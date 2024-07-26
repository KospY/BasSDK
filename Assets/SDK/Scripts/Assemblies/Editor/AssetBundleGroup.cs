using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor.AddressableAssets.Settings;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
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

        public bool isMod = false;
        
        [ShowIf("isMod")]
        [Header("Mod manifest")]
        public bool exportModManifest;
        [ShowIf("isMod")]
        public string modName = "Unknown";
        [ShowIf("isMod")]
        public string modDescription = "Unknown";
        [ShowIf("isMod")]
        public string modAuthor = "Unknown";
        [ShowIf("isMod")]
        public string modVersion = "1.0";
        [ShowIf("isMod")]
        public string modThumbnail;
        
        [Button]
        [ShowIf("isMod")]
        public void CreateModCatalogFolder()
        {
            if (!exportModManifest)
            {
                Debug.LogWarning("Mod manifest export is disabled, enable it to create mod catalog folder.");
                return;
            }
            
            string path = Path.Combine(Application.dataPath, "../BuildStaging/Catalogs/Mods/" + folderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string manifestTempFolderPath = AssetBundleBuilderGUI.GenerateManifest(this);
            AssetBundleBuilder.CopyDirectory(manifestTempFolderPath, path);
            Debug.Log("Mod manifest created at " + path);
        }
        
        public bool CheckAddressableLabels(out string message)
        {
            message = string.Empty;
            bool missingLabels = false;
            StringBuilder sb = new StringBuilder();
            foreach (AddressableAssetGroup group in addressableAssetGroups)
            {
                foreach (AddressableAssetEntry entry in group.entries)
                {
                    if (!entry.labels.Contains("Android") && !entry.labels.Contains("Windows"))
                    {
                        Debug.LogWarning($"Entry {entry.address} in group {group.Name} has no Android or Windows label");
                        sb.AppendLine($"[{group.name}][{entry.address}]");
                        missingLabels = true;
                    }
                }
            }
            if (!missingLabels) return true;
            message = sb.ToString();
            return false;
        }
    }
}
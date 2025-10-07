using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEditor.AddressableAssets.Settings;
using TriInspector;
using UnityEditor;

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
        [HideInInspector]
        public AssetBundleBuilderGUI.ExportMode exportMode = AssetBundleBuilderGUI.ExportMode.ToGame;
        
        [HideInInspector]
        public bool isMod => !isDefault;
        
        [ShowIf("isMod")]
        public string modDescription = "Unknown";
        [ShowIf("isMod")]
        public string modAuthor = "Unknown";
        [ShowIf("isMod")]
        public string modVersion = "1.0";
        [ShowIf("isMod")]
        public string modThumbnail;

        public void OnValidate()
        {
            // Make sure folder name is valid
            if (string.IsNullOrEmpty(folderName))
            {
                folderName = this.name;
            }
        }
        
        [Button]
        [ShowIf("isMod")]
        public void CreateModCatalogFolder()
        {
            string path = Path.Combine(Application.dataPath, $"../BuildStaging/Catalogs/Mods/{folderName}");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string manifestTempFolderPath = AssetBundleBuilderGUI.GenerateManifest(this);
            AssetBundleBuilder.CopyDirectory(manifestTempFolderPath, path);
            Debug.Log($"Mod manifest created at {path}");
        }
        [Button]
        [ShowIf("isMod")]
        public void OpenCatalogFolder()
        {
            string path = Path.Combine(Application.dataPath, $"../BuildStaging/Catalogs/Mods/{folderName}");
            if (!Directory.Exists(path))
            {
                //display a dialog to ask if the user wants to create the folder
                if (EditorUtility.DisplayDialog("Folder does not exist", "The folder does not exist. Do you want to create it?", "Yes", "No"))
                {
                    CreateModCatalogFolder();
                }
                else
                {
                    return;
                }
            }
            //open the folder in explorer
            EditorUtility.RevealInFinder(path);
        }
        
        [Button]
        public void CheckLabels()
        {
            CheckAddressableLabels(out string message);
            Debug.Log(message);
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
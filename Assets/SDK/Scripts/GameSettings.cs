using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class GameSettings : ScriptableObject
    {
        private static GameSettings _instance;

        public static GameSettings instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<GameSettings>("GameSettings");

                    if (_instance == null)
                    {
#if UNITY_EDITOR
                        string properPath = Path.Combine(Application.dataPath, "Resources");
                        if (!Directory.Exists(properPath))
                        {
                            UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
                        }
                        string fullPath = Path.Combine(Path.Combine("Assets", "Resources"), "GameSettings.asset");
                        if (!File.Exists(fullPath))
                        {
                            _instance = ScriptableObject.CreateInstance<GameSettings>();
                            UnityEditor.AssetDatabase.CreateAsset(_instance, fullPath);
                        }
#else
                        Debug.LogError("Cannot load GameSettings");
#endif
                    }
                }
                return _instance;
            }
        }

#if ODIN_INSPECTOR
        [BoxGroup("Version"), HorizontalGroup("Version/Default"), LabelText("Major")]
#endif
        public int versionMajor;
#if ODIN_INSPECTOR
        [HorizontalGroup("Version/Default"), LabelText("Minor"), LabelWidth(60)]
#endif
        public int versionMinor;
#if ODIN_INSPECTOR
        [HorizontalGroup("Version/Default"), LabelText("Revision"), LabelWidth(60)]
#endif
        public int versionRevision;
#if ODIN_INSPECTOR
        [HorizontalGroup("Version/Default"), LabelText("Build"), LabelWidth(60)]
#endif
        public int versionBuild;
#if ODIN_INSPECTOR
        [BoxGroup("Version")]
#endif
        public string minModVersion;
#if ODIN_INSPECTOR
        [BoxGroup("Version")]
#endif
        public string exeDescription;


#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public bool pauseOnVRPresence = true;
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public PlatformStoreEnum platformStore = PlatformStoreEnum.None;

        public enum PlatformStoreEnum
        {
            None,
            Steam,
            Oculus,
            HTC,
            Synthesis,
            Pico,
        }

#if ODIN_INSPECTOR
        [ValueDropdown("GetJsonFolders")]
#endif
        public List<string> buildLoadDefaultFolders;
#if ODIN_INSPECTOR
        [ValueDropdown("GetJsonFolders")]
#endif
        public List<string> editorLoadDefaultFolders;

        public static List<string> loadDefaultFolders
        {
            get
            {
                if (Application.isEditor)
                {
                    return instance.editorLoadDefaultFolders;
                }
                else
                {
                    return instance.buildLoadDefaultFolders;
                }
            }
            set
            {
                if (Application.isEditor)
                {
                    instance.editorLoadDefaultFolders = value;
                }
                else
                {
                    instance.buildLoadDefaultFolders = value;
                }
            }
        }

        public bool overrideData = true;

        public string addressableEditorPath = "BuildStaging/AddressableAssets";
        public string catalogsEditorPath = "BuildStaging/Catalogs";

        [System.Flags]
        public enum ContentFlag
        {
            None = 0,
            Blood = (1 << 0),
            Burns = (1 << 1),
            Dismemberment = (1 << 2),
            Desecration = (1 << 3), // Makes bodies despawn after 3 seconds to prevent "misuse" of dead bodies
            Skeleton = (1 << 4),
            Spider = (1 << 5),
            Insect = (1 << 6),
            Snake = (1 << 7), 
            Bird = (1 << 8),
            Fright = (1 << 9), // This should be a catch-all for anything we'd consider to be a "jumpscare" or otherwise potentially scary
        }
        
        public enum ContentFlagBehaviour
        {
            Discard = 0, // Remove the content only if their sensitive content flags matches
            Keep = 1 // Use the content only if their sensitive content flags matches. Acts as a replacement
        }

#if ODIN_INSPECTOR
        [BoxGroup("Content settings")]
#endif
        public ContentFlag activeContent = ~(ContentFlag.None);

        public bool allowJsonMods = true;
        public bool allowScriptedMods = true;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                UnityEditor.PlayerSettings.bundleVersion = GetVersionString(true);
                UnityEditor.PlayerSettings.Android.bundleVersionCode = versionBuild;
            }
        }
#endif

        public static string GetVersionString(bool stripBuild = false)
        {
            if (stripBuild || instance.versionBuild == 0)
            {
                return $"{instance.versionMajor}.{instance.versionMinor}.{instance.versionRevision}";
            }
            else
            {
                return $"{instance.versionMajor}.{instance.versionMinor}.{instance.versionRevision}.{instance.versionBuild}";
            }
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetJsonFolders()
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            foreach (string folderName in FileManager.GetFolderNames(FileManager.Type.JSONCatalog, FileManager.Source.Default))
            {
                if (folderName == ".git") continue;
                dropdownList.Add(new ValueDropdownItem<string>(folderName, folderName));
            }
            return dropdownList;
        }
#endif
    }
}
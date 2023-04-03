using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class GameSettings : ScriptableObject
    {

#if ODIN_INSPECTOR
        [BoxGroup("Version")]
#endif
        public string minModVersion;

#if ODIN_INSPECTOR
        [BoxGroup("Info")]
#endif
        public string productName;

#if ODIN_INSPECTOR
        [BoxGroup("Info")]
#endif
        public string appIdentifier;

#if ODIN_INSPECTOR
        [ValueDropdown("GetJsonFolders")]
#endif
        public List<string> editorLoadDefaultFolders;

        public static List<string> loadDefaultFolders
        {
            get
            {
                return ThunderRoadSettings.current.game.editorLoadDefaultFolders;
            }
            set
            {
                ThunderRoadSettings.current.game.editorLoadDefaultFolders = value;
            }
        }

        [Button("Load all JSON")]
        public void LoadAllJson()
        {
            Catalog.EditorLoadAllJson(true);
        }

        public static string GetVersionString(bool stripBuild = false)
        {
            return $"0.0.0.0";
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
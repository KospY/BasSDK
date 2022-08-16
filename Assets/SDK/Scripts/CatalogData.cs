using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace ThunderRoad
{
    [Serializable]
    public class CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Data")]
        [HorizontalGroup("Data/Split", LabelWidth = 100), JsonProperty(Order = -2)]
#endif
        public string id;

#if ODIN_INSPECTOR
        [HorizontalGroup("Data/Split", LabelWidth = 120), JsonProperty(Order = -2), ValueDropdown("GetJsonFolders")]
#endif
        public string saveFolder = "Bas";
#if ODIN_INSPECTOR
        [HorizontalGroup("Data/Split", LabelWidth = 120), JsonProperty(Order = -2)]
#endif
        public GameSettings.ContentFlag sensitiveContent = GameSettings.ContentFlag.None;
#if ODIN_INSPECTOR
        [HorizontalGroup("Data/Split", LabelWidth = 120), JsonProperty(Order = -2)]
#endif
        public GameSettings.ContentFlagBehaviour sensitiveFilterBehaviour = GameSettings.ContentFlagBehaviour.Discard;
#if ODIN_INSPECTOR
        [HorizontalGroup("Data/Split", LabelWidth = 100), ReadOnly, NonSerialized, ShowInInspector, JsonProperty(Order = -2)]
#endif
        public int hashId;
#if ODIN_INSPECTOR
        [HorizontalGroup("Data/Split", LabelWidth = 100), ReadOnly, JsonProperty(Order = -2)]
#endif
        public int version;

        [NonSerialized]
        public string filePath;

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

        public virtual int GetCurrentVersion()
        {
            return 0;
        }

        public virtual void Init()
        {
            hashId = Animator.StringToHash(id.ToLower());
        }

        public virtual void OnCatalogRefresh()
        {
            version = GetCurrentVersion();
        }


        public virtual void ReleaseAddressableAssets()
        {

        }

        public virtual CatalogData Clone()
        {
            return MemberwiseClone() as CatalogData;
        }

        public virtual void OnLanguageChanged(string language)
        {

        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Data")]
        [HorizontalGroup("Data/Split"), LabelWidth(15), JsonProperty(Order = -2)]
#endif
        public string id;

#if ODIN_INSPECTOR
        [HorizontalGroup("Data/Split"), LabelWidth(105), JsonProperty(Order = -2)]
#endif
        public BuildSettings.ContentFlag sensitiveContent = BuildSettings.ContentFlag.None;
#if ODIN_INSPECTOR
        [HorizontalGroup("Data/Split"), LabelWidth(150), JsonProperty(Order = -2)]
#endif
        public BuildSettings.ContentFlagBehaviour sensitiveFilterBehaviour = BuildSettings.ContentFlagBehaviour.Discard;

#if ODIN_INSPECTOR
        [HorizontalGroup("Data/Split"), LabelWidth(90), ReadOnly, ShowInInspector, JsonProperty(Order = -2)]
#endif
        [NonSerialized]
        public string sourceFolders;

        public string GetLatestOverrideFolder()
        {
            if (string.IsNullOrEmpty(sourceFolders))
            {
                return GameSettings.loadDefaultFolders.Last();
            }
            return sourceFolders.Split('+').Last();
        }

#if ODIN_INSPECTOR
        [HorizontalGroup("Data/Split"), ReadOnly, LabelWidth(45), NonSerialized, ShowInInspector, JsonProperty(Order = -2)]
#endif
        public int hashId;
#if ODIN_INSPECTOR
        [HorizontalGroup("Data/Split"), ReadOnly, LabelWidth(50), JsonProperty(Order = -2)]
#endif
        public int version;

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

        [NonSerialized]
        public string filePath;

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

        public virtual CatalogData Clone()
        {
            return MemberwiseClone() as CatalogData;
        }

        public virtual IEnumerator OnCatalogRefreshCoroutine()
        {
            yield return null;
        }

        public virtual IEnumerator LoadAddressableAssetsCoroutine()
        {
            yield return Yielders.EndOfFrame;
        }

        public virtual void ReleaseAddressableAssets()
        {

        }

        public virtual void OnLanguageChanged(string language)
        {

        }
    }
}
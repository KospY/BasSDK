using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class CatalogData
    {
        [NonSerialized]
        public bool standaloneData = false;
#if ODIN_INSPECTOR
        [ShowIfGroup("standaloneData")]
        [BoxGroup("standaloneData/Data")]
        [HorizontalGroup("standaloneData/Data/Split"), LabelWidth(15), JsonProperty(Order = -2)]
#endif
        public string id;

        public virtual string SortKey() => id;

#if ODIN_INSPECTOR
        [HorizontalGroup("standaloneData/Data/Split"), LabelWidth(105), JsonProperty(Order = -2)]
#endif
        public BuildSettings.ContentFlag sensitiveContent = BuildSettings.ContentFlag.None;
#if ODIN_INSPECTOR
        [HorizontalGroup("standaloneData/Data/Split"), LabelWidth(150), JsonProperty(Order = -2)]
#endif
        public BuildSettings.ContentFlagBehaviour sensitiveFilterBehaviour = BuildSettings.ContentFlagBehaviour.Discard;

#if ODIN_INSPECTOR
        [HorizontalGroup("standaloneData/Data/Split"), LabelWidth(90), ReadOnly, ShowInInspector, JsonProperty(Order = -2)]
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
        [HorizontalGroup("standaloneData/Data/Split"), ReadOnly, LabelWidth(45), NonSerialized, ShowInInspector, JsonProperty(Order = -2)]
#endif
        public int hashId;
#if ODIN_INSPECTOR
        [HorizontalGroup("standaloneData/Data/Split"), ReadOnly, LabelWidth(50), JsonProperty(Order = -2)]
#endif
        public int version;

#if ODIN_INSPECTOR
        private bool showOwnershipFields => standaloneData && ModManager.gameModsLoaded;

        public List<ValueDropdownItem<string>> GetJsonFolders()
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            foreach (string folderName in FileManager.GetFolderNames(FileManager.Type.JSONCatalog, FileManager.Source.Default))
            {
                if (folderName == ".git")
                    continue;
                dropdownList.Add(new ValueDropdownItem<string>(folderName, folderName));
            }
            return dropdownList;
        }
#endif
#if ODIN_INSPECTOR
        [HorizontalGroup("standaloneData/Data/Split"), LabelWidth(70)]
#endif
        public string groupPath;

        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly, BoxGroup("standaloneData/Data"), LabelWidth(60)]
#endif
        public string filePath;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowIfGroup("showOwnershipFields")]
        [BoxGroup("showOwnershipFields/Modding"), ShowInInspector, ReadOnly, ShowIf("showOwnershipFields")]
#endif
        public ModManager.ModData owner;
        [NonSerialized]
#if ODIN_INSPECTOR
        [BoxGroup("showOwnershipFields/Modding"), ShowInInspector, ReadOnly, ShowIf("showOwnershipFields")]
#endif
        public List<ModManager.ModData> changers = new List<ModManager.ModData>();

        public virtual int GetCurrentVersion()
        {
            return 0;
        }

        public virtual void Init()
        {
            hashId = Animator.StringToHash(id.ToLower());
        }

        public virtual void OnEditorSave() { }

        public virtual void OnCatalogRefresh()
        {
            version = GetCurrentVersion();
            standaloneData = true;
        }

        public virtual CatalogData Clone()
        {
            return MemberwiseClone() as CatalogData;
        }

        public virtual IEnumerator OnCatalogRefreshCoroutine()
        {
            return null;
        }

        public virtual IEnumerator LoadAddressableAssetsCoroutine()
        {
            return null;
        }

        public virtual void ReleaseAddressableAssets()
        {

        }

        public virtual void OnLanguageChanged(string language)
        {

        }
#if UNITY_EDITOR
        /// <summary>
        /// Called by odin when the catalog editor is refreshed
        /// </summary>
        public virtual void CatalogEditorRefresh()
        {

        }
#endif        
    }
}
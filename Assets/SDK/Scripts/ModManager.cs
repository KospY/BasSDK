using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using System.Text.RegularExpressions;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    
    public static class ModManager
    {
        
        public const string debugLine = "[ModManager]";
        public static string modFolderName = "Mods";
        public static bool editorLoadAddressableBundles = false;

        public static readonly HashSet<ModData> loadedMods = new HashSet<ModData>();
        public static bool gameModsLoaded;

        
        public delegate void ModLoadEvent(EventTime eventTime, ModLoadEventType eventType, ModData modData = null);
        public static event ModLoadEvent OnModLoad;

#pragma warning disable CS0067 // Not used
        public static event ModLoadEvent OnModUnload;
#pragma warning restore CS0067

        public static bool isGameModsCatalogRefreshed;

        public enum ModLoadEventType
        {
            ModManager,
            Assembly,
            Addressable,
            Catalog,
            ModOption,
            ThunderScript
        }

        public class ModData
        {
            public string Name;
            public string Description;
            public string Author;
            public string ModVersion;
            public string GameVersion;
            public string Thumbnail;

#if ODIN_INSPECTOR
            [ShowInInspector]
#endif
            [NonSerialized]
            public string folderName;

#if ODIN_INSPECTOR
            [ShowInInspector]
#endif
            [NonSerialized]
            public string fullPath;
            
#if ODIN_INSPECTOR
            [ShowInInspector]
#endif
            [NonSerialized]
            public bool Incompatible;
#if ODIN_INSPECTOR
            [ShowInInspector]
#endif
            [NonSerialized]
            public HashSet<Error> errors = new HashSet<Error>();

            public class Error
            {
                public ErrorType type;
                public string description;
                public string innerMessage;
                public string filePath;

                public Error(ErrorType type, string description, string innerMessage, string filePath)
                {
                    this.type = type;
                    this.description = description;
                    this.innerMessage = innerMessage;
                    this.filePath = filePath;
                }

                #region Equality

                protected bool Equals(Error other)
                {
                    return type == other.type
                           && description == other.description
                           && innerMessage == other.innerMessage
                           && filePath == other.filePath;
                }
                public override bool Equals(object obj)
                {
                    if (ReferenceEquals(null, obj)) return false;
                    if (ReferenceEquals(this, obj)) return true;
                    if (obj.GetType() != this.GetType()) return false;
                    return Equals((Error)obj);
                }
                public override int GetHashCode()
                {
                    return HashCode.Combine((int)type, description, innerMessage, filePath);
                }
                public static bool operator ==(Error left, Error right)
                {
                    return Equals(left, right);
                }
                public static bool operator !=(Error left, Error right)
                {
                    return !Equals(left, right);
                }

                #endregion
            }

            public enum ErrorType
            {
                Json,
                Catalog,
                Assembly,
                Manifest,
                Option,
                ThunderScript
            }

            protected bool Equals(ModData other)
            {
                return Name == other.Name
                       && Description == other.Description
                       && Author == other.Author
                       && ModVersion == other.ModVersion
                       && GameVersion == other.GameVersion
                       && Thumbnail == other.Thumbnail;
            }
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((ModData)obj);
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(Name, Description, Author, ModVersion, GameVersion, Thumbnail);
            }
            public static bool operator ==(ModData left, ModData right)
            {
                return Equals(left, right);
            }
            public static bool operator !=(ModData left, ModData right)
            {
                return !Equals(left, right);
            }
        }

        public class LoadOrder
        {
            public List<string> modNames;
        }

        public static IEnumerator LoadAndRefreshCatalog(bool loadJsonOnly = false)
        {
                //Only load mods if we havent tried loading them yet
                if (!gameModsLoaded)
                {
                    yield return LoadCoroutine(loadJsonOnly);
                }
                //Refresh the catalog if mods were loaded
                if (gameModsLoaded && !isGameModsCatalogRefreshed)
                {
                    //refresh catalog and apply mod options
                    float timer = Time.realtimeSinceStartup;
                    Debug.Log("Refresh catalog...");
                    yield return Catalog.RefreshCoroutine();
                    Debug.Log($"------------> Catalog refreshed in {(Time.realtimeSinceStartup - timer):F2} sec");
                    isGameModsCatalogRefreshed = true;
                }
        }

        public static IEnumerator LoadCoroutine(bool loadJsonOnly = false)
        {
            
            List<ModData> orderedMods = GetOrderedMods();
            //Only load mods that arent already loaded and that pass the manifest version check
            orderedMods = orderedMods.Where(m => !loadedMods.Contains(m) && ManifestVersionCheck(m)).ToList();
            

            if (Application.isPlaying)
            {

                yield return LoadModCatalogs(orderedMods);
                
                if (!loadJsonOnly)
                {
                    yield return LoadModOptions(orderedMods);
                    yield return LoadModThunderScripts(orderedMods);
                }
            }
            
            //set mods loaded if any mods successfully loaded, so we know a catalog refresh needs to be done
            if (loadedMods.Any(mod => !mod.Incompatible))
            {
                gameModsLoaded = true;
            }
            yield break;
        }


        public static IEnumerator LoadModCatalogs(List<ModData> modDatas)
        {        
            int modCount = modDatas.Count;
            for (int i = 0; i < modCount; i++)
            {
                ModData mod = modDatas[i];
                if(mod.Incompatible) continue; //mod has an issue so we skip loading this one
                Debug.Log($"{debugLine}[{ModLoadEventType.Catalog}] - Loading Mod: {mod.Name}");
                  

                Catalog.LoadModCatalog(mod);
            
                loadedMods.Add(mod);
                Debug.Log($"{debugLine}[{ModLoadEventType.Catalog}] - Loaded Mod: {mod.Name}");
            }
      
            yield break;
        }
        
        public static IEnumerator LoadModOptions(List<ModData> modDatas)
        {
            
            yield break;
        }
        
        public static IEnumerator LoadModThunderScripts(List<ModData> modDatas)
        {
            
            yield break;
        }
        
        public static void Load(bool loadJsonOnly = false)
        {
            //Call the coroutine method, but iterate the enumerator manually to process it synchronously.
            LoadCoroutine(loadJsonOnly).AsSynchronous();
        }

        public static void ReloadJson()
        {
            foreach (ModData mod in loadedMods)
            {
                Debug.Log($"{debugLine} - Reloading mod json: {mod.Name}");
                //remove json errors
                mod.errors.RemoveWhere(e => e.type == ModData.ErrorType.Json);

                Catalog.LoadModCatalog(mod);

                Debug.Log($"{debugLine} - Mod json: {mod.Name} reloaded");
            }
        }

        static List<ModData> GetOrderedMods()
        {
            bool AddValidMod(string topLevelModFolder, List<ModData> modDatas)
            {
                if (TryReadManifest(topLevelModFolder, out ModData modData))
                {
                    modDatas.Add(modData);
                    Debug.Log($"[ModManager] Added valid mod folder: {topLevelModFolder}. Mod: {modData.Name}");
                    return true;

                }
                return false;
            }

            List<ModData> modList = new List<ModData>();

            //Get all of the folders inside the Mods folder
            string[] topLevelModFolders = FileManager.GetFolderNames(FileManager.Type.JSONCatalog, FileManager.Source.Mods);
            //Order them by the load order.
            topLevelModFolders = GetLoadOrderedMods(topLevelModFolders);

            //Go through each modFolder and search for nested folders which contain a manifest

            for (int i = 0; i < topLevelModFolders.Length; i++)
            {
                string topLevelModFolder = topLevelModFolders[i];
                //Dont load anything in this folder if its disabled
                bool isEnabledFolder = IsEnabledFolder(topLevelModFolder);
                if (!isEnabledFolder) continue;


                // Single top level folder containing mod
                if (AddValidMod(topLevelModFolder, modList)) continue;

                //Nested mod folders (mod.io)
                //Otherwise, get all of the folders inside this path and check if any of them are valid mod folders.
                string fullPathTopLevelModFolder = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Mods, topLevelModFolder);
                string[] dirs = Directory.GetDirectories(fullPathTopLevelModFolder).Select(Path.GetFileName).Where(p => {
                    string nestedModFolder = $"{topLevelModFolder}/{p}";
                    return IsEnabledFolder(p) && ContainsManifest(nestedModFolder);
                }).ToArray();

                //If there is two mod folders inside this though, log error, dont add it to the list, WE DO NOT SUPPORT MULTIPLE MODS INSIDE A SINGLE FOLDER YET
                if (dirs.Length == 0) continue;
                if (dirs.Length > 1)
                {
                    Debug.LogError($"[ModManager] Unable to load mod folder {topLevelModFolder} because it contains multiple nested mod folders. Only one nested mod folder is supported");
                    continue;
                }

                string fullPath = $"{topLevelModFolder}/{dirs[0]}";
                AddValidMod(fullPath, modList);

            }
            return modList;
        }

        /// <summary>
        /// Checks if a given mod is compatible with the current game version
        /// </summary>
        /// <param name="modData"></param>
        /// <returns></returns>
        public static bool ManifestVersionCheck(ModData modData)
        {
            Version minModVersion = new Version(ThunderRoadSettings.current.game.minModVersion);
            Version gameVersion = new Version(modData.GameVersion);

            if (gameVersion.CompareTo(minModVersion) != 0)
            {
                // Version don't match
                Debug.LogWarning($"{debugLine} - Mod {modData.Name} for ({modData.GameVersion}) is not compatible with current minimum mod version {minModVersion}");
                modData.errors.Add(new ModData.Error(ModData.ErrorType.Manifest, "Incompatible with game version", $"Mod {modData.Name} for ({modData.GameVersion}) is not compatible with current minimum mod version {minModVersion}", $"{modData.fullPath}/manifest.json"));
                //add it to the loaded mods, even though we never really loaded it, this is to show it has been processed, but there is an error with it
                loadedMods.Add(modData);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Tries to read the manifest, returns false if it was unable to read it
        /// </summary>
        /// <param name="modFolder"></param>
        /// <param name="modData"></param>
        /// <returns></returns>
        public static bool TryReadManifest(string modFolder, out ModData modData)
        {
            modData = null;
            if (!ContainsManifest(modFolder)) return false;

            try
            {
                string json = FileManager.ReadAllText(FileManager.Type.JSONCatalog, FileManager.Source.Mods, $"{modFolder}/manifest.json");
                modData = JsonConvert.DeserializeObject<ModData>(json, Catalog.GetJsonNetSerializerSettings());
                modData.folderName = modFolder;
                modData.fullPath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Mods, modFolder);
            }
            catch (Exception e)
            {
                Debug.Log($"[ModManager] Unable to read manifest from {modFolder} : {e.Message}");
                return false;
            }

            return true;
        }

        static bool IsEnabledFolder(string modFolder)
        {
            if (Catalog.loadModFolders.Count > 0 && !Catalog.loadModFolders.Contains(modFolder.ToLower())) return true;
            return !modFolder.StartsWith("_");
        }

        static bool ContainsManifest(string modFolder)
        {
            return FileManager.FileExist(FileManager.Type.JSONCatalog, FileManager.Source.Mods, $"{modFolder}/manifest.json");
        }

        /// <summary>
        /// Given a list of modFolders, read the loadorder.json and reorder the array of modFolders
        /// </summary>
        /// <param name="modFolders"></param>
        /// <returns></returns>
        static string[] GetLoadOrderedMods(string[] modFolders)
        {
            string loadOrderPath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Mods, "loadorder.json");

            if (File.Exists(loadOrderPath))
            {
                LoadOrder loadOrder = JsonUtility.FromJson<LoadOrder>(File.ReadAllText(loadOrderPath));
                if (loadOrder != null)
                {
                    List<string> modFoldersOrdered = new List<string>();
                    for (int i = 0; i < modFolders.Length; i++)
                    {
                        if (!loadOrder.modNames.Contains(modFolders[i])) modFoldersOrdered.Add(modFolders[i]);
                    }
                    foreach (string loadOrderModName in loadOrder.modNames)
                    {
                        if (modFolders.Contains(loadOrderModName)) modFoldersOrdered.Add(loadOrderModName);
                    }
                    modFolders = modFoldersOrdered.ToArray();
                }
            }
            return modFolders;
        }

    }
}

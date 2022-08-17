using Ionic.Crc;
using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.UnityConverters.Math;
using Newtonsoft.Json.UnityConverters.Scripting;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Profiling;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace ThunderRoad
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public static class Catalog
    {

        public static List<string> loadModFolders = new List<string>();

        public static List<CatalogData>[] data = new List<CatalogData>[Enum.GetNames(typeof(Category)).Length];
        //Category > HashId > CatalogData
        public static Dictionary<Category, Dictionary<int, CatalogData>> CategoryHashIdLookup = new Dictionary<Category, Dictionary<int, CatalogData>>(Enum.GetNames(typeof(Category)).Length);

        public static GameData gameData;

        public static bool overrideDefaultData = true;
        public static bool loadMods = true;
        public static bool checkFileVersion = true;

        public static JsonSerializerSettings jsonSerializerSettings;

        public enum Category
        {
            Wave,
            Creature,
            CreatureTable,
            LootTable,
            Container,
            Item,
            Spell,
            Brain,
            Effect,
            Material,
            Text,
            Interactable,
            Damager,
            HandPose,
            Expression,
            Level,
            Menu,
            ColliderGroup,
            DamageModifier,
            Skill,
            BehaviorTree,
            EffectGroup,
            Liquid,
            MusicSegment,
            MusicGroup,
            Music,
            Tutorial,
            Animation,
            Voice,
            Custom,
        }

        /// <summary>
        /// A mapping of all of the base types to their catalog Categories
        /// </summary>
        public static (Type Type, Category Category)[] baseTypeCategories = new (Type Type, Category Category)[] {
            (typeof(CreatureData), Category.Creature),
            (typeof(HandPoseData), Category.HandPose),
        };

        // A map of all types and subtypes to their catalog category
        public static Dictionary<Type, Category> typeCategories = new Dictionary<Type, Category>();

        public static Category GetCategory(Type type)
        {
            if (typeCategories.TryGetValue(type, out Category category))
            {
                return category;
            }


            if (IsSameOrSubclass(typeof(CatalogData), type))
            {
                for (int i = 0; i < baseTypeCategories.Length; i++)
                {
                    (Type Type, Category Category) tuple = baseTypeCategories[i];
                    if (IsSameOrSubclass(tuple.Type, type))
                    {
                        typeCategories.Add(type, tuple.Category);
                        return tuple.Category;
                    }
                }
            }
            Debug.LogError($"Type: {type} is not a subclass of CatalogData!");
            return Category.Brain;
        }

        public static bool IsSameOrSubclass(Type baseClass, Type subClass)
        {
            return subClass.IsSubclassOf(baseClass) || subClass == baseClass;
        }

        public static JsonSerializerSettings GetJsonNetSerializerSettings()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
            jsonSerializerSettings.Formatting = Formatting.Indented;
            jsonSerializerSettings.MaxDepth = 50;
            jsonSerializerSettings.ContractResolver = JsonSerializer.CreateDefault().ContractResolver;
            jsonSerializerSettings.Converters.Add(new StringEnumConverter());
            jsonSerializerSettings.Converters.Add(new Vector2Converter());
            jsonSerializerSettings.Converters.Add(new Vector2IntConverter());
            jsonSerializerSettings.Converters.Add(new Vector3Converter());
            jsonSerializerSettings.Converters.Add(new Vector3IntConverter());
            jsonSerializerSettings.Converters.Add(new Vector4Converter());
            jsonSerializerSettings.Converters.Add(new QuaternionConverter());
            jsonSerializerSettings.Converters.Add(new ColorConverter());
            jsonSerializerSettings.Converters.Add(new LayerMaskConverter());
            return jsonSerializerSettings;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LOAD JSON
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool IsJsonLoaded()
        {
            return (gameData != null);
        }

        private static void LoadJsonDbFile(string zipPath, string folderName, bool isMod)
        {
            var fileInfo = new FileInfo(zipPath);
            var localPathFile = $"{fileInfo.Directory.Name}/{fileInfo.Name}";
            Debug.Log($"[JSON][{folderName}] - Loading file: {localPathFile}");
            ZipFile zip = ZipFile.Read(zipPath);
            if (zip == null)
            {
                Debug.LogError($"[JSON][{folderName}] - Error: Can't read JSONDB file: {localPathFile}");
                return;
            }

            int zipCount = zip.Count;
            List<string> inputJsons = new List<string>(zipCount);
            List<string> inputJsonPaths = new List<string>(zipCount);

            for (var i = 0; i < zipCount; i++)
            {
                ZipEntry zipEntry = zip[i];
                if (Path.GetExtension(zipEntry.FileName) == ".json")
                {
                    CrcCalculatorStream stream = zipEntry.OpenReader();
                    StreamReader streamReader = new StreamReader(stream);
                    string jsonText = streamReader.ReadToEnd();
                    var entryName = $"{localPathFile}/{zipEntry.FileName}";
                    if (TryCleanCatalogJson(jsonText, entryName, out var json))
                    {
                        inputJsons.Add(json);
                        inputJsonPaths.Add(entryName);
                    }
                }
            }

            LoadJsonLooseFiles(folderName, inputJsons, inputJsonPaths, isMod);
            Debug.Log($"[JSON][{folderName}] - Loaded file: {localPathFile}");
        }

        private static void LoadJsonLooseFiles(string folderName, List<string> inputJsons, List<string> inputJsonPaths, bool logging)
        {
            object[] deserializedObjects = DeserializeJsons(inputJsons, inputJsonPaths);
            //loop over the objects and load them
            for (int i = 0; i < deserializedObjects.Length; i++)
            {
                var catalogObject = deserializedObjects[i];
                if (catalogObject is null) continue;
                var fileInfo = new FileInfo(inputJsonPaths[i]);
                var localPath = $"{fileInfo.Directory.Name}/{fileInfo.Name}";
                if (logging) Debug.Log($"[JSON][{folderName}] - Loading file: {localPath}");
                if (LoadJson(catalogObject, inputJsons[i], inputJsonPaths[i], folderName))
                {
                    if (logging) Debug.Log($"[JSON][{folderName}] - Loaded file: {localPath}");
                }
                else
                {
                }
            }
        }

        private static void ReadCatalogJsonFiles(FileManager.Source fileSource, string folder, out List<string> inputJsons, out List<string> inputJsonPaths)
        {
            FileManager.ReadFile[] readJsons = FileManager.ReadFiles(FileManager.Type.JSONCatalog, fileSource, folder, "*.json");
            inputJsons = new List<string>(readJsons.Length);
            inputJsonPaths = new List<string>(readJsons.Length);
            for (var i = 0; i < readJsons.Length; i++)
            {
                FileManager.ReadFile readJson = readJsons[i];
                if (TryCleanCatalogJson(readJson.text, readJson.path, out var json))
                {
                    inputJsons.Add(json);
                    inputJsonPaths.Add(readJson.path);
                }
            }
        }

        public static bool TryCleanCatalogJson(string inputJson, string jsonPath, out string outputJson)
        {
            string fileName = Path.GetFileName(jsonPath).ToLower();
            if (fileName.Contains("catalog_") || fileName == "manifest.json")
            {
                outputJson = inputJson;
                return false;
            }
            outputJson = inputJson.Replace("Assembly-CSharp", "ThunderRoad");
            return true;
        }

        private static void LoadDefaultCatalog()
        {
            // Load default catalogs
            Debug.Log($"[JSON] - Loading game catalog");
            gameData = null;
            int defaultFolderCount = GameSettings.loadDefaultFolders?.Count ?? 0;
            // Force the SDK to load the default bas folder.
            if (defaultFolderCount == 0)
            {
                GameSettings.loadDefaultFolders = new List<string>() { "bas" };
                defaultFolderCount = 1;
            }
            for (int i = 0; i < defaultFolderCount; i++)
            {
                string defaultFolder = GameSettings.loadDefaultFolders[i];
                Debug.Log($"[JSON][{defaultFolder}] - Loading catalog game by WarpFrog");
                // Load json archive file if any
                string jsondbPath = FileManager.GetFullPath(FileManager.Type.AddressableAssets, FileManager.Source.Default, $"{defaultFolder}.jsondb");
                if (File.Exists(jsondbPath))
                {
                    LoadJsonDbFile(jsondbPath, defaultFolder, false);
                }
#if UNITY_EDITOR
                // Load json loose files (avoid this, seem that loose file remain after steam update for some people) 
                ReadCatalogJsonFiles(FileManager.Source.Default, defaultFolder, out var inputJsons, out var inputJsonPaths);
                LoadJsonLooseFiles(defaultFolder, inputJsons, inputJsonPaths, false);
#endif
                Debug.Log($"[JSON][{defaultFolder}] - Loaded catalog game by WarpFrog");
            }
            Debug.Log($"[JSON] - Loaded game catalog");
        }

        private static void LoadModCatalog()
        {
            if (!GameSettings.instance.allowJsonMods) return;
            // Load mods
            Debug.Log($"[JSON] - Loading mod catalog");
            List<ModData> mods = FileManager.GetOrderedMods();
            int modsCount = mods.Count;
            for (int i = 0; i < modsCount; i++)
            {
                ModData mod = mods[i];
                Debug.Log($"[JSON][{mod.folderName}] - Loading catalog {mod.Name} by {mod.Author}");

                // Load json archive file if any
                foreach (string jsondbPath in FileManager.GetFullFilePaths(FileManager.Type.JSONCatalog, FileManager.Source.Mods, mod.folderName, "*.jsondb"))
                {
                    LoadJsonDbFile(jsondbPath, mod.folderName, true);
                }

                // Load json loose files
                ReadCatalogJsonFiles(FileManager.Source.Mods, mod.folderName, out var inputJsons, out var inputJsonPaths);
                LoadJsonLooseFiles(mod.folderName, inputJsons, inputJsonPaths, true);
                Debug.Log($"[JSON][{mod.folderName}] - Loaded catalog {mod.Name} by {mod.Author}");
            }
            Debug.Log($"[JSON] - Loaded mod catalog");

        }

        [Button]
        public static void LoadAllJson()
        {

            Debug.Log($"[JSON] - Loading all JSON");
            // Clear data
            CategoryHashIdLookup.Clear();
            IEnumerable<Category> categories = Enum.GetValues(typeof(Category)).Cast<Category>();
            foreach (Category category in categories)
            {
                CategoryHashIdLookup.Add(category, new Dictionary<int, CatalogData>(30));
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == null)
                {
                    data[i] = new List<CatalogData>();
                }
                else
                {
                    data[i].Clear();
                }
            }

            jsonSerializerSettings = GetJsonNetSerializerSettings();

            if (!Application.isPlaying || !Level.master)
            {
                overrideDefaultData = GameSettings.instance.overrideData;
            }
            Profiler.BeginSample("LoadDefaultCatalog");
            LoadDefaultCatalog();
            Profiler.EndSample();
            Profiler.BeginSample("LoadModCatalog");
            LoadModCatalog();
            Profiler.EndSample();

            Debug.Log($"[JSON] - Finished loading all JSON");
        }

        public static object[] DeserializeJsons(List<String> jsons, List<String> jsonPaths)
        {
            var jsonsCount = jsons.Count;
            var outputs = new object[jsonsCount];

            Parallel.For(0, jsonsCount, index =>
            {
                object obj = null;
                try
                {
                    obj = JsonConvert.DeserializeObject(jsons[index], jsonSerializerSettings);
                }
                catch (Exception ex)
                {
                    // Wrapped this to prevent any error spam in the SDK.
                }
                outputs[index] = obj;
            });
            return outputs;

        }

        public static bool LoadJson(object jsonObj, string jsonText, string jsonPath, string folder)
        {
            Profiler.BeginSample("LoadJson");
            object obj = jsonObj;
            if (obj.GetType().IsSubclassOf(typeof(CatalogData)))
            {
                // Load CatalogData
                CatalogData catalogData = obj as CatalogData;
                if (!checkFileVersion || catalogData.version == catalogData.GetCurrentVersion())
                {
                    Category category = GetCategory(catalogData.GetType());
                    //Debug.Log($"[JSON] - loading {catalogData.id} | {catalogData.GetType()} - Category: {category}");
                    CatalogData existingData = GetData(category, catalogData.id, false);
                    if (existingData != null)
                    {
                        if (overrideDefaultData)
                        {
                            Debug.Log($"[JSON][{folder}] - Overriding default data: [{category}][{existingData.GetType()}][{existingData.id}] with: {jsonPath}");
                            JsonConvert.PopulateObject(jsonText, existingData, jsonSerializerSettings);
                        }
                    }
                    else
                    {
                        //Debug.Log("[JSON] - Add data: " + catalogData.id + " | " + catalogData.GetType());
                        data[(int)category].Add(catalogData);

                        if (Application.isEditor)
                        {
                            catalogData.saveFolder = folder;
                            catalogData.filePath = jsonPath;
                        }
                        catalogData.Init();
                        if (CategoryHashIdLookup.TryGetValue(category, out var idMap))
                        {
                            idMap.Add(catalogData.hashId, catalogData);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"[JSON][{folder}] - Version mismatch (file {catalogData.version}, current {catalogData.GetCurrentVersion()}) ignoring file: {jsonPath}");
                    Profiler.EndSample();
                    return false;
                }
            }
            else if (obj.GetType() == typeof(GameData))
            {
                // Load GameData
                GameData gameData = obj as GameData;
                if (Catalog.gameData != null)
                {
                    if (overrideDefaultData)
                    {
                        JsonConvert.PopulateObject(jsonText, Catalog.gameData, jsonSerializerSettings);
                        Debug.Log($"[JSON][{folder}] - Overriding default data: [GameData] with: {jsonPath}");
                    }
                }
                else
                {
                    Catalog.gameData = gameData;
                }
            }
            else
            {
                Debug.LogError($"[JSON][{folder}] - Unsupported JSON: {jsonPath}");
                Profiler.EndSample();
                return false;
            }
            Profiler.EndSample();
            return true;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // SAVE JSON
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void SaveAllJson()
        {
            if (!Application.isPlaying)
            {
                jsonSerializerSettings = GetJsonNetSerializerSettings();
            }

            foreach (Category category in (Category[])Enum.GetValues(typeof(Category)))
            {
                foreach (CatalogData catalogData in GetDataList(category))
                {
                    SaveToJson(catalogData);
                }
            }
            SaveGameData();
            Debug.Log("All Json Saved");
        }

        public static void SaveGameData()
        {
            string defaultGamePath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Default, GameSettings.loadDefaultFolders[0].ToLower());
            string fileName = $"{defaultGamePath}/Game.json";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            File.WriteAllText(fileName, JsonConvert.SerializeObject(gameData, typeof(GameData), GetJsonNetSerializerSettings()));
        }

        public static void SaveToJson(CatalogData catalogData)
        {
            if (!Application.isPlaying)
            {
                if (File.Exists(catalogData.filePath)) File.Delete(catalogData.filePath);
            }
            string savePath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Default, catalogData.saveFolder);
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
            Category category = GetCategory(catalogData.GetType());
            string saveCategoryPath = $"{savePath}/{category}s/";
            if (!Directory.Exists(saveCategoryPath)) Directory.CreateDirectory(saveCategoryPath);
            string fileName = $"{saveCategoryPath}{category}_{catalogData.id}.json";
            File.WriteAllText(fileName, JsonConvert.SerializeObject(catalogData, typeof(CatalogData), jsonSerializerSettings));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // REFRESH
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public static T GetData<T>(string id, bool logError = true) where T : CatalogData
        {
            if (string.IsNullOrEmpty(id)) return null;
            int hashId = Animator.StringToHash(id.ToLower());
            Category category = GetCategory(typeof(T));
            //TODO: Since we know the categories, we could replace the dataCategory map with an array of hashId > catalogDatas
            if (CategoryHashIdLookup.TryGetValue(category, out var idMap))
            {
                if (idMap.TryGetValue(hashId, out CatalogData catalogData))
                {
                    return catalogData as T;
                }
                Debug.LogError($"Data [{id} | {hashId}] of type [{typeof(T)}] is not the correct type");
            }
            if (logError) Debug.LogError($"Data [{id} | {hashId}] of type [{typeof(T)}] cannot be found in catalog");
            return null;
        }

        public static CatalogData GetData(Category category, string id, bool logError = true)
        {
            if (string.IsNullOrEmpty(id)) return null;
            int hashId = Animator.StringToHash(id.ToLower());
            if (CategoryHashIdLookup.TryGetValue(category, out var idMap))
            {
                if (idMap.TryGetValue(hashId, out CatalogData catalogData))
                {
                    return catalogData;
                }
            }

            if (logError) Debug.LogError($"Data [{id} | {hashId}] of type [{category}] cannot be found in catalog");
            return null;
        }

        public static List<CatalogData> GetDataList(Category category)
        {
            return data[(int)category];
        }

        public static List<T> GetDataList<T>() where T : CatalogData
        {
            Category category = GetCategory(typeof(T));
            return (
                from catalogData in GetDataList(category)
                where catalogData is T
                select ((T)catalogData)).ToList();
        }

        public static List<string> GetAllID(Category category)
        {
            return GetDataList(category).Select(x => x.id).ToList();
        }

#if ODIN_INSPECTOR
        public static List<ValueDropdownItem<string>> GetDropdownAllID(Category category, string noneText = "None")
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            dropdownList.Add(new ValueDropdownItem<string>(noneText, ""));
            foreach (string id in GetAllID(category))
            {
                dropdownList.Add(new ValueDropdownItem<string>(id, id));
            }
            return dropdownList;
        }

        public static List<string> GetAllID<T>() where T : CatalogData
        {
            // Get the target category.
            Category category = GetCategory(typeof(T));

            // Try get all data from that category.
            List<CatalogData> dataList = GetDataList(category);


            // Select all data in the list and obtain their IDs.
            List<string> results = new List<string>();
            for (int i = 0; i < dataList.Count; i++)
            {
                results.Add(dataList[i].id);
            }

            return results;
        }

        public static List<ValueDropdownItem<string>> GetDropdownAllID<T>() where T : CatalogData
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            dropdownList.Add(new ValueDropdownItem<string>("None", ""));
            foreach (string id in GetAllID<T>())
            {
                dropdownList.Add(new ValueDropdownItem<string>(id, id));
            }
            return dropdownList;
        }

        public static List<ValueDropdownItem<string>> GetDropdownHolderSlots(string noneText = "None")
        {
            List<ValueDropdownItem<string>> dropdownList = new List<ValueDropdownItem<string>>();
            dropdownList.Add(new ValueDropdownItem<string>(noneText, ""));
            foreach (string id in gameData.holderSlots)
            {
                dropdownList.Add(new ValueDropdownItem<string>(id, id));
            }
            return dropdownList;
        }
#endif


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ASSETS
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}

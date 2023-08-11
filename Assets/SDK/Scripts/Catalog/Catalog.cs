using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json.UnityConverters.Math;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.UnityConverters.Scripting;
using Ionic.Zip;
using Ionic.Crc;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Profiling;
using static ThunderRoad.AddressLocationCache;

#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.Text;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public static class Catalog
    {
        public static List<string> loadModFolders = new List<string>();

        //A lookup for holding (int)Category lookup tables and the catalogData together
        public static CatalogCategory[] data = new CatalogCategory[Enum.GetNames(typeof(Category)).Length];

        public static GameData gameData;

        public static bool loadMods = true;
        public static bool checkFileVersion = true;

        public static JsonSerializerSettings jsonSerializerSettings;
        public static JsonSerializer jsonSerializer;

        /// <summary>
        /// A mapping of all of the base types to their catalog Categories
        /// </summary>
        public static (Type Type, Category Category)[] baseTypeCategories = new (Type Type, Category Category)[] {
            (typeof(WaveData), Category.Wave),
            (typeof(CreatureData), Category.Creature),
            (typeof(ItemData), Category.Item),
            (typeof(SpellData), Category.Spell),
            (typeof(EffectData), Category.Effect),
            (typeof(TextData), Category.Text),
            (typeof(InteractableData), Category.Interactable),
            (typeof(HandPoseData), Category.HandPose),
            (typeof(ExpressionData), Category.Expression),
            (typeof(LevelData), Category.Level),
            (typeof(DamagerData), Category.Damager),
            (typeof(MaterialData), Category.Material),
            (typeof(ColliderGroupData), Category.ColliderGroup),
            (typeof(CreatureTable), Category.CreatureTable),
            (typeof(LootTable), Category.LootTable),
            (typeof(ContainerData), Category.Container),
            (typeof(BrainData), Category.Brain),
            (typeof(SkillData), Category.Skill),
            (typeof(EffectGroupData), Category.EffectGroup),
            (typeof(DamageModifierData), Category.DamageModifier),
            (typeof(LiquidData), Category.Liquid),
            (typeof(MusicGroup), Category.MusicGroup),
            (typeof(Music), Category.Music),
            (typeof(TutorialData), Category.Tutorial),
            (typeof(AnimationData), Category.Animation),
            (typeof(VoiceData), Category.Voice),
            (typeof(AreaConnectionTypeData), Category.AreaConnectionType),
            (typeof(AreaData), Category.Area),
            (typeof(AreaTable), Category.AreaTable),
            (typeof(AreaCollectionData), Category.AreaCollection),
            (typeof(MenuData), Category.Menu),
            (typeof(BehaviorTreeData), Category.BehaviorTree),
            (typeof(CustomData), Category.Custom),
            (typeof(KeyboardData), Category.Keyboard),
            (typeof(GameModeData), Category.GameMode)
        };

        // A map of all types and subtypes to their catalog category
        public static Dictionary<Type, Category> typeCategories = new Dictionary<Type, Category>();

        public static bool TryGetCategory<T>(out Category category)
        {
            category = Category.Custom;
            return TryGetCategory(typeof(T), out category);
        }

        public static bool TryGetCategory(Type type, out Category category)
        {
            if (typeCategories.TryGetValue(type, out category))
            {
                return true;
            }
            if (!IsSameOrSubclass(typeof(CatalogData), type))
            {
                Debug.LogError($"Type: {type} is not a subclass of CatalogData!");
                return false;
            }
            for (int i = 0; i < baseTypeCategories.Length; i++)
            {
                (Type Type, Category Category) tuple = baseTypeCategories[i];
                if (!IsSameOrSubclass(tuple.Type, type)) continue;
                typeCategories.Add(type, tuple.Category);
                category = tuple.Category;
                return true;
            }
            return false;
        }

        public static Category GetCategory(Type type)
        {
            if (typeCategories.TryGetValue(type, out Category category))
            {
                return category;
            }

            if (!IsSameOrSubclass(typeof(CatalogData), type))
            {
                Debug.LogError($"Type: {type} is not a subclass of CatalogData!");
                return Category.Custom;
            }

            for (int i = 0; i < baseTypeCategories.Length; i++)
            {
                (Type Type, Category Category) tuple = baseTypeCategories[i];
                if (!IsSameOrSubclass(tuple.Type, type)) continue;
                typeCategories.Add(type, tuple.Category);
                return tuple.Category;
            }
            //If we dont know what category it is, default to the custom one
            //Really TryGetCategory should be used
            return Category.Custom;
        }

        public static bool IsSameOrSubclass(Type baseClass, Type subClass)
        {
            return subClass.IsSubclassOf(baseClass) || subClass == baseClass;
        }

        #region JSON

        //create a reusable serializer
        public static JsonSerializer GetJsonNetSerializer() => jsonSerializer ??= JsonSerializer.CreateDefault(GetJsonNetSerializerSettings());

        public static JsonSerializerSettings GetJsonNetSerializerSettings()
        {
            if (jsonSerializerSettings != null) return jsonSerializerSettings;
            jsonSerializerSettings = new JsonSerializerSettings();
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
            jsonSerializerSettings.Converters.Add(new KeyedListMergeConverter(jsonSerializerSettings.ContractResolver));
            return jsonSerializerSettings;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LOAD JSON
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool IsJsonLoaded()
        {
            return gameData != null;
        }

        private static void LoadJsonDbFile(string zipPath, string folderName, ModManager.ModData modData = null)
        {
            string logTag = modData != null ? $"[ModManager][Catalog][{folderName}]" : $"[Catalog][{folderName}]";
            FileInfo fileInfo = new FileInfo(zipPath);
            string localPathFile = $"{fileInfo.Directory.Name}/{fileInfo.Name}";
            Debug.Log($"{logTag} Loading file: {localPathFile}");

            ZipFile zip = null;
            try
            {
                zip = ZipFile.Read(zipPath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"{logTag} Error: Exception reading .jsondb file: {localPathFile}");
                if (modData != null)
                {
                    modData.errors.Add(new ModManager.ModData.Error(ModManager.ModData.ErrorType.Json, "Exception reading .jsondb file", ex.Message, localPathFile));
                    //add it to the loaded mods, even though we never really loaded it, this is to show it has been processed, but there is an error with it
                    ModManager.loadedMods.Add(modData);
                }
                return;
            }

            if (zip == null)
            {
                Debug.LogError($"{logTag} Error: Can't read .jsondb file: {localPathFile}");
                if (modData != null)
                {
                    modData.errors.Add(new ModManager.ModData.Error(ModManager.ModData.ErrorType.Json, "Can't read .jsondb file", "", localPathFile));
                    //add it to the loaded mods, even though we never really loaded it, this is to show it has been processed, but there is an error with it
                    ModManager.loadedMods.Add(modData);
                }
                return;
            }

            int zipCount = zip.Count;
            List<string> inputJsons = new List<string>(zipCount);
            List<string> inputJsonPaths = new List<string>(zipCount);

            for (int i = 0; i < zipCount; i++)
            {
                ZipEntry zipEntry = zip[i];
                if (Path.GetExtension(zipEntry.FileName) == ".json")
                {
                    CrcCalculatorStream stream = zipEntry.OpenReader();
                    StreamReader streamReader = new StreamReader(stream);
                    string jsonText = streamReader.ReadToEnd();
                    string entryName = $"{localPathFile}/{zipEntry.FileName}";
                    if (TryCleanCatalogJson(jsonText, entryName, out string json))
                    {
                        inputJsons.Add(json);
                        inputJsonPaths.Add(entryName);
                    }
                }
            }

            LoadJsonLooseFiles(folderName, inputJsons, inputJsonPaths, modData);
            Debug.Log($"{logTag}- Loaded file: {localPathFile}");
        }

        private static void LoadJsonLooseFiles(string folderName, List<string> inputJsons, List<string> inputJsonPaths, ModManager.ModData modData = null)
        {
            string basePath = modData != null ? FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Mods) : FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Default);
            basePath = Path.Combine(basePath, folderName);
            string logTag = modData != null ? $"[ModManager][Catalog][{folderName}]" : $"[Catalog][{folderName}]";
            object[] deserializedObjects = DeserializeJsons(inputJsons, inputJsonPaths, modData);
            //loop over the objects and load them
            for (int i = 0; i < deserializedObjects.Length; i++)
            {
                object catalogObject = deserializedObjects[i];
                if (catalogObject is null) continue;
                // skip if the file isnt a catalog or game data
                if (catalogObject is not CatalogData && catalogObject is not GameData) continue;
  
                FileInfo fileInfo = new FileInfo(inputJsonPaths[i]);
                if (fileInfo.Directory == null)
                {
                    Debug.LogError($"{logTag} Failed to load file: {inputJsonPaths[i]}");
                    continue;
                }

                string localPath = Path.GetRelativePath(basePath, inputJsonPaths[i]);
                if (!LoadJson(catalogObject, inputJsons[i], inputJsonPaths[i], folderName, modData))
                {
                    Debug.LogError($"{logTag} Failed to load file: {localPath}");
                }
                //     if (modData != null)
                //     {
                //         Debug.Log($"{logTag} Loaded file: {localPath}");
                //     }
                //     continue;
                
                

            }
        }

        private static void ReadCatalogJsonFiles(FileManager.Source fileSource, string folder, out List<string> inputJsons, out List<string> inputJsonPaths)
        {
            FileManager.ReadFile[] readJsons = FileManager.ReadFiles(FileManager.Type.JSONCatalog, fileSource, folder, "*.json");
            inputJsons = new List<string>(readJsons.Length);
            inputJsonPaths = new List<string>(readJsons.Length);
            for (int i = 0; i < readJsons.Length; i++)
            {
                FileManager.ReadFile readJson = readJsons[i];
                if (TryCleanCatalogJson(readJson.text, readJson.path, out string json))
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


        public static bool LoadJson(object jsonObj, string jsonText, string jsonPath, string folder, ModManager.ModData modData = null)
        {
            string logTag = modData != null ? $"[ModManager][Catalog][{folder}]" : $"[Catalog][{folder}]";
            Profiler.BeginSample("LoadJson");
            switch (jsonObj)
            {
                // Load CatalogData
                case CatalogData catalogData when !DoesCatalogVersionMatch(catalogData): {
                    Debug.LogWarning($"{logTag} Version mismatch (file {catalogData.version}, current {catalogData.GetCurrentVersion()}) ignoring file: {jsonPath}");
                    if (modData != null)
                    {
                        modData.errors.Add(new ModManager.ModData.Error(ModManager.ModData.ErrorType.Json, "Catalog version incompatible", $"[{folder}] Version mismatch (file {catalogData.version}, current {catalogData.GetCurrentVersion()}) ignoring file: {jsonPath}", jsonPath));
                        //add it to the loaded mods, even though we never really loaded it, this is to show it has been processed, but there is an error with it
                        ModManager.loadedMods.Add(modData);
                    }
                    Profiler.EndSample();
                    return false;
                }
                case CatalogData catalogData:
                    bool result = LoadCatalogData(catalogData, jsonText, jsonPath, folder, modData);
                    Profiler.EndSample();
                    return result;
                case GameData newGameData:
                    // Load GameData
                    LoadGameData(newGameData, jsonText, jsonPath, folder, modData);
                    Profiler.EndSample();
                    return true;
                default: {
                    Debug.LogWarning($"{logTag} Custom Json File or malformed CatalogData skipped: {jsonPath}");
                    Profiler.EndSample();
                    return false;
                }
            }

        }

        public static void LoadGameData(GameData newGameData, string jsonText, string jsonPath, string folder, ModManager.ModData modData = null)
        {
            // Load GameData
            if (gameData != null)
            {
                if (!ThunderRoadSettings.current.overrideData) return;
                JsonConvert.PopulateObject(jsonText, gameData, jsonSerializerSettings);
                if (modData != null)
                {
                    string relativePath = jsonPath.Substring(jsonPath.IndexOf(folder, StringComparison.Ordinal));
                    Debug.Log($"[ModManager][Catalog][{folder}] Overriding: [GameData] with: {relativePath}");
                }
                if (Application.isEditor) gameData.sourceFolders += $"+{folder}";
            }
            else
            {
                gameData = newGameData;
                if (Application.isEditor) gameData.sourceFolders = folder;
            }
        }

        public static bool LoadCatalogData(CatalogData catalogData, string jsonText, string jsonPath, string folder, ModManager.ModData modData = null)
        {
            string logTag = modData != null ? $"[ModManager][Catalog][{folder}]" : $"[Catalog][{folder}]";
            if (!TryGetCategory(catalogData.GetType(), out Category category))
            {
                Debug.LogError($"{logTag} CatalogData: [{catalogData.GetType()}][{catalogData.id}] JSON: {jsonPath} does not map to a valid category. Please subclass CustomData to extend catalog data types");
                return false;
            }

            CatalogData existingData = GetData(category, catalogData.id, false);
            if (existingData != null)
            {
                if (!ThunderRoadSettings.current.overrideData) return false;
                if (modData != null)
                {
                    string relativePath = jsonPath.Substring(jsonPath.IndexOf(folder, StringComparison.Ordinal));
                    Debug.Log($"{logTag} Overriding: [{category.ToString()}][{existingData.GetType()}][{existingData.id}] with: {relativePath}");
                }
                JsonConvert.PopulateObject(jsonText, existingData, jsonSerializerSettings);
                if (Application.isEditor) existingData.sourceFolders += $"+{folder}";
            }
            else
            {
                //Debug.Log("[Catalog] Add data: " + catalogData.id + " | " + catalogData.GetType());

                if (Application.isEditor)
                {
                    catalogData.sourceFolders = folder;
                    catalogData.filePath = jsonPath;
                }
                catalogData.Init();

                int categoryIndex = (int)category;
                data[categoryIndex] ??= new CatalogCategory(category);

                //Add the catalog data to the category data
                data[categoryIndex].AddCatalogData(catalogData);

            }
            return true;
            //Debug.Log($"[Catalog] loading {catalogData.id} | {catalogData.GetType()} - Category: {category}");
        }

        public static object[] DeserializeJsons(List<string> jsons, List<string> jsonPaths, ModManager.ModData modData = null)
        {
            int jsonsCount = jsons.Count;
            object[] outputs = new object[jsonsCount];
            object lockObj = new object();
            Parallel.For(0, jsonsCount, index => {
                object obj = null;
                try
                {
                    obj = JsonConvert.DeserializeObject(jsons[index], jsonSerializerSettings);
                }
                catch (Exception ex)
                {
                    bool isExperimentalJSON = false;
#if UNITY_EDITOR
                    if (modData == null)
                    {
                        if (ex.InnerException?.Message.Contains("Could not find type") == true)
                        {
                            string notFoundType = ex.InnerException.Message.Split("Could not find type '")[1].Split('\'')[0];
                            if (jsons[index].Contains($"\"{notFoundType}, ThunderRoad.Experimental\"") && ex.InnerException.Message.Contains("ThunderRoad.Experimental"))
                            {
                                string nonExperimentalJSON = jsons[index].Replace($"\"{notFoundType}, ThunderRoad.Experimental\"", $"\"{notFoundType}, ThunderRoad\"");
                                try
                                {
                                    obj = JsonConvert.DeserializeObject(nonExperimentalJSON, jsonSerializerSettings);
                                    isExperimentalJSON = true;
                                }
                                catch (Exception newEx)
                                {
                                    ex = newEx;
                                }
                            }
                            if (!isExperimentalJSON)
                            {
                                string experimentalJSON = jsons[index];
                                while (ex.InnerException?.Message.Contains("Could not find type") == true)
                                {
                                    notFoundType = ex.InnerException.Message.Split("Could not find type '")[1].Split('\'')[0];
                                    if (experimentalJSON.Contains($"\"{notFoundType}, ThunderRoad.Experimental\""))
                                    {
                                        Debug.LogError($"LoadJson EDITOR ONLY : Could not convert [ {jsonPaths[index]} ] to experimental JSON, must be wrong type, not wrong assembly:\n{experimentalJSON}");
                                        break;
                                    }
                                    try
                                    {
                                        experimentalJSON = experimentalJSON.Replace($"\"{notFoundType}, ThunderRoad\"", $"\"{notFoundType}, ThunderRoad.Experimental\"");
                                        obj = JsonConvert.DeserializeObject(experimentalJSON, jsonSerializerSettings);
                                        isExperimentalJSON = true;
                                        break;
                                    }
                                    catch (Exception newEx)
                                    {
                                        ex = newEx;
                                    }
                                }
                            }
                        }
                    }
#endif
                    if (!isExperimentalJSON)
                    {
                        Debug.LogError($"LoadJson : Cannot read json file {jsonPaths[index]} ({ex.Message}) \n ({ex.InnerException?.Message})");
                    }
                    if (modData != null)
                    {
                        lock (lockObj)
                        {
                            modData.errors.Add(new ModManager.ModData.Error(ModManager.ModData.ErrorType.Json, $"Cannot read json file: {ex.Message}", ex.InnerException?.Message, jsonPaths[index]));
                            //add it to the loaded mods, even though we never really loaded it, this is to show it has been processed, but there is an error with it
                            ModManager.loadedMods.Add(modData);
                        }
                    }
                }
                outputs[index] = obj;
            });
            return outputs;
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
            string defaultGamePath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Default, gameData.GetLatestOverrideFolder());
            string fileName = $"{defaultGamePath}/Game.json";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            File.WriteAllText(fileName, JsonConvert.SerializeObject(gameData, typeof(GameData), GetJsonNetSerializerSettings()));
        }

        public static void SaveToJson(CatalogData catalogData)
        {
            if (!Application.isPlaying && File.Exists(catalogData.filePath))
            {
                File.Delete(catalogData.filePath);
            }
            string savePath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Default, catalogData.GetLatestOverrideFolder());
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
            if (TryGetCategory(catalogData.GetType(), out Category category))
            {
                string saveCategoryPath = $"{savePath}/{category}s/";
                string type = catalogData.GetType().ToString();
                string[] splitType = type.Split('.');
                type = splitType[splitType.Length - 1];
                if (category == Category.AreaCollection) saveCategoryPath = $"{savePath}/{type}s/"; // AreaCollection can be different type
                if (!Directory.Exists(saveCategoryPath)) Directory.CreateDirectory(saveCategoryPath);
                string fileName = $"{saveCategoryPath}{category}_{catalogData.id}.json";
                if (catalogData is ItemData) fileName = $"{savePath}/{category}s/{category}_{(catalogData as ItemData).type.ToString() + "_" + catalogData.id}.json";
                File.WriteAllText(fileName, JsonConvert.SerializeObject(catalogData, typeof(CatalogData), jsonSerializerSettings));
            }
            else
            {
                Debug.Log($"Cannot get valid category for CatalogData: [{catalogData.id}][{catalogData.GetType()}]. Unable to save to json");
            }
        }

        #endregion

        [Button]
        public static void Clear()
        {
            data ??= new CatalogCategory[Enum.GetNames(typeof(Category)).Length];

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == null)
                {
                    data[i] = new CatalogCategory((Category)i);
                }
                else
                {
                    data[i].Clear();
                }
            }
            gameData = null;
            jsonSerializerSettings = GetJsonNetSerializerSettings();
        }

        #region LOAD CATALOG

        public static void EditorLoadAllJson(bool requiresRefresh = false, bool force = false)
        {
            if (gameData != null && !force) return;
            LoadDefaultCatalogs();
            //Need to load mods twice, once for the core mods, then once for game mods
            ModManager.Load(true);
            if (requiresRefresh)
            {
                Refresh();
            }
        }

        public static void LoadDefaultCatalogs()
        {
            Clear();

            Profiler.BeginSample("LoadDefaultCatalog");

            // Load default catalogs
            Debug.Log($"Loading default catalogs");
            gameData = null;
            int defaultFolderCount = GameSettings.loadDefaultFolders.Count;
            for (int i = 0; i < defaultFolderCount; i++)
            {
                string defaultFolder = GameSettings.loadDefaultFolders[i];
                Debug.Log($"Loading default catalog in folder: {defaultFolder}");
                // Load json archive file if any
                string jsondbPath = FileManager.GetFullPath(FileManager.Type.AddressableAssets, FileManager.Source.Default, $"{defaultFolder}.jsondb");
                if (File.Exists(jsondbPath))
                {
                    LoadJsonDbFile(jsondbPath, defaultFolder);
                }
#if UNITY_EDITOR
                // Load json loose files (avoid this, seem that loose file remain after steam update for some people) 
                ReadCatalogJsonFiles(FileManager.Source.Default, defaultFolder, out List<string> inputJsons, out List<string> inputJsonPaths);
                LoadJsonLooseFiles(defaultFolder, inputJsons, inputJsonPaths);
#endif
                Debug.Log($"Loaded catalog in folder: {defaultFolder}");
            }
            Debug.Log($"Finished loading default catalogs");

            Profiler.EndSample();
        }

        public static void LoadModCatalog(ModManager.ModData mod)
        {
            if (mod.Incompatible) return;
            // Load json archive file if any
            foreach (string jsondbPath in FileManager.GetFullFilePaths(FileManager.Type.JSONCatalog, FileManager.Source.Mods, mod.folderName, "*.jsondb"))
            {
                LoadJsonDbFile(jsondbPath, mod.folderName, mod);
            }
            // Load json loose files
            ReadCatalogJsonFiles(FileManager.Source.Mods, mod.folderName, out List<string> inputJsons, out List<string> inputJsonPaths);
            LoadJsonLooseFiles(mod.folderName, inputJsons, inputJsonPaths, mod);
        }

        #endregion

        public static bool DoesCatalogVersionMatch(CatalogData catalogData)
        {
            return !checkFileVersion || catalogData.version == catalogData.GetCurrentVersion();
        }

        #region REFRESH

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // REFRESH
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        
        public static void Refresh()
        {
            RefreshCoroutine().AsSynchronous();
        }

        public static IEnumerator RefreshCoroutine()
        {
            IEnumerator BatchYield(List<Coroutine> list, int batchCount)
            {
                for (int j = 0; j < batchCount; j++)
                {
                    yield return list[j];
                }
                list.Clear();
            }
            float timer = Time.realtimeSinceStartup;
            gameData.OnCatalogRefresh();

            yield return gameData.OnCatalogRefreshCoroutine();
#if UNITY_EDITOR
            Debug.Log($"[Catalog] Game data refreshed in {Time.realtimeSinceStartup - timer:F2} sec");
#endif
            //Batch the coroutines so we dont create too many at once
            //As it can cause a stack overflow in unity
            int batchSize = 100;
            int batchCount = 0;

            List<Coroutine> coroutines = new List<Coroutine>();
            for (int i = 0; i < data.Length; i++)
            {
                timer = Time.realtimeSinceStartup;
                //get the category
                Category category = (Category)i;
                List<CatalogData> catalogDatas = data[i].catalogDatas;
                if (catalogDatas == null) continue;


                int catalogDatasCount = catalogDatas.Count;
                batchCount = 0; //reset batch count for each category
                for (int d = 0; d < catalogDatasCount; d++)
                {
                    CatalogData catalogData = catalogDatas[d];
                    catalogData.OnCatalogRefresh();
                    catalogData.OnCatalogRefreshCoroutine().AsSynchronous();
                }

#if UNITY_EDITOR
                Debug.Log($"[Catalog] {Enum.GetName(typeof(Category), i)} refreshed {catalogDatasCount} entries in {(Time.realtimeSinceStartup - timer):F2} sec");
#endif
            }
        }

        #endregion

        #region ADDRESSABLES


        #endregion

        #region TEXT

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // TEXTS
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method to update data when the language changes
        /// </summary>
        /// <param name="language">Language key</param>
        public static void OnLanguageChanged(string language)
        {
            gameData.OnLanguageChanged(language);
            for (int i = 0; i < data.Length; i++)
            {
                CatalogCategory catalogCategory = data[i];
                if (catalogCategory == null) continue;
                int catalogDatasCount = catalogCategory.catalogDatas.Count;
                for (int j = 0; j < catalogDatasCount; j++)
                {
                    catalogCategory.catalogDatas[j].OnLanguageChanged(language);
                }
            }
        }
        public static TextData GetTextData()
        {
            return GetData<TextData>(SystemLanguage.English.ToString());
        }


        #endregion
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // GET
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool TryGetCategoryData(Category category, out CatalogCategory catalogCategory)
        {
            catalogCategory = GetCategoryData(category);
            return catalogCategory != null;
        }

        public static CatalogCategory GetCategoryData(Category category) => data?[(int)category];

        public static bool TryGetData<T>(string id, out T outputData, bool logError = true) where T : CatalogData
        {
            outputData = null;
            if (string.IsNullOrEmpty(id)) return false;
            outputData = GetData<T>(id, logError);
            return outputData != null;
        }

        public static T GetData<T>(string id, bool logError = true) where T : CatalogData
        {
            if (string.IsNullOrEmpty(id)) return null;
            if (TryGetCategory<T>(out Category category))
            {
                CatalogData catalogData = GetData(category, id, logError);
                if (catalogData is T)
                {
                    return catalogData as T;
                }
            }
            if (logError) Debug.LogError($"Data [{id} | {Animator.StringToHash(id.ToLower())}] of type [{typeof(T)} | {category}] cannot be found in catalog or is not the correct type");
            return null;
        }

        public static CatalogData GetData(Category category, string id, bool logError = true)
        {
            if (string.IsNullOrEmpty(id)) return null;
            if (TryGetCategoryData(category, out CatalogCategory categoryData) && categoryData.TryGetCatalogData(id, out CatalogData catalogData))
            {
                return catalogData;
            }
            if (logError) Debug.LogError($"Data [{id} | {Animator.StringToHash(id.ToLower())}] of type [{category}] cannot be found in catalog");
            return null;
        }

        public static List<CatalogData> GetDataList(Category category)
        {
            if (!TryGetCategoryData(category, out CatalogCategory categoryData)) return new List<CatalogData>();
            return categoryData.catalogDatas;
        }

        public static List<T> GetDataList<T>() where T : CatalogData
        {
            if (!TryGetCategory<T>(out Category category)) return new List<T>(0);
            return (
                from catalogData in GetDataList(category)
                where catalogData is T
                select (T)catalogData).ToList();
        }

        public static IEnumerable<T> GetDataEnumerable<T>() where T : CatalogData
        {
            if (!TryGetCategory<T>(out Category category)) return new T[0];
            return from catalogData in GetDataList(category)
                where catalogData is T
                select (T)catalogData;
        }


#if ODIN_INSPECTOR
        #region ODIN

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

        #endregion
#endif

        public static List<string> GetAllID(Category category)
        {
            return GetDataList(category).Select(x => x.id).ToList();
        }

        public static List<string> GetAllID<T>() where T : CatalogData
        {
            // Get the target category.
            if (!TryGetCategory<T>(out Category category)) return new List<string>(0);

            // Try get all data from that category.
            List<CatalogData> dataList = GetDataList(category);

            // Does the category have any data?
            if (dataList.IsNullOrEmpty())
            {
                // No, return empty.
                return new List<string>(0);
            }

            // Select all data in the list and obtain their IDs.
            List<string> results = new List<string>();
            for (int i = 0; i < dataList.Count; i++)
            {
                results.Add(dataList[i].id);
            }

            return results;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // ASSETS
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Load single location
        public static void LoadLocationAsync<T>(string address, Action<IResourceLocation> result, string requestName)
        {
            var operation = LoadResourceLocationsAsync<T>(address);
            if (operation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Addressable operation failed load location for address: {address} - Please check the bundles have been built correctly. {operation.OperationException}");
                if (operation.OperationException == null) Catalog.ReleaseAsset(operation);
            }
            operation.Completed += operationHandle => OnLoadResourceLocations<T>(address, result, operationHandle, requestName);
        }

        public static IEnumerator LoadLocationCoroutine<T>(string address, Action<IResourceLocation> result, string requestName)
        {
            if (!Application.isPlaying || string.IsNullOrEmpty(address))
            {
                result?.Invoke(null);
                yield break;
            }
            //try to get the result from the cache first
            if (TryGetAddressLocation<T>(address, out IResourceLocation location))
            {
                result?.Invoke(location);
                yield break;
            }
            AsyncOperationHandle<IList<IResourceLocation>> handle = LoadResourceLocationsAsync<T>(address);
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Addressable operation failed load location for address: {address} - Please check the bundles have been built correctly. {handle.OperationException}");
                if (handle.OperationException == null) Catalog.ReleaseAsset(handle);
                result?.Invoke(null);
                yield break;
            }
            if (!handle.IsDone)
            {
                yield return handle;
            }
            OnLoadResourceLocations<T>(address, result, handle, requestName);
        }

        //Load possibly multiple locations
        private static AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsAsync<T>(string address)
        {
            return LoadResourceLocationsAsync(address, typeof(T));
        }

        private static AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsAsync(string address, Type type = null)
        {

            GetAddressAndSubAddress(address, out string addressOnly, out string subAddress);
            bool hasSubAddress = subAddress != null;

#if UNITY_EDITOR
            if (type == null)
            {
                Debug.LogWarning($"LoadResourceLocationAsync called with null type for address {address}, this could affect performance in loading");
                return Addressables.LoadResourceLocationsAsync(address);
            }
            //This is fast in editor and doesnt load all assets.
            //But multiple resource locations could return, especially for different platforms. Player prefabs for android vs windows is an example of this.
            return Addressables.LoadResourceLocationsAsync(address, type);
#else
            if (hasSubAddress)
            {
                // Sadly it seem not possible to filter a label while using an address with sub-asset. Question has been asked on Unity forum:
                // https://forum.unity.com/threads/get-location-with-a-specific-address-sub-object-and-label.1162751/
                return Addressables.LoadResourceLocationsAsync(address, type);
            }
            else
            {
                var labels = new string[] { address, Common.GetPlatform().ToString() };
                IEnumerable enumerable = labels;
                //when using "Use asset database" for addressables, When this is called the very first time, it is incredibly slow and loads everything in the editor when:
                //  filtering by multiple keys or  with one key but a null type 
                //But it is perfectly fine in the build, thanks Unity
                return Addressables.LoadResourceLocationsAsync(enumerable, Addressables.MergeMode.Intersection, type);
            }
#endif
        }

        //Helper method
        private static void GetAddressAndSubAddress(string address, out string addressOnly, out string subAddress)
        {
            if (address.Contains("["))
            {
                string[] split = address.Split('[', ']');
                addressOnly = split[0];
                subAddress = split[1];
            }
            else
            {
                addressOnly = address;
                subAddress = null;
            }
        }

        //Callbacks to handle filtering resource locations
#if UNITY_EDITOR
        private static List<AddressableAssetEntry> AddressableAssetEntries = new List<AddressableAssetEntry>();
#endif
        private static void OnLoadResourceLocations<T>(string address, Action<IResourceLocation> callback, AsyncOperationHandle<IList<IResourceLocation>> handle, string requestName)
        {
            OnLoadResourceLocations(address, callback, handle, requestName, typeof(T));
        }

        private static void OnLoadResourceLocations(string address, Action<IResourceLocation> callback, AsyncOperationHandle<IList<IResourceLocation>> handle, string requestName, Type type)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result.Count > 0)
            {
                IResourceLocation resultLocation = null;
                IList<IResourceLocation> locations = handle.Result;
                resultLocation = handle.Result[0];

                if (locations.Count > 1)
                {
#if UNITY_EDITOR
                    //There are multiple locations for this address, which means either labels could not be provided, such as in editor
                    //Or there are duplicate addresses somewhere.
                    //In the build, we just have to take the first one because there is not much else we can do to determine the correct one
                    //In editor we can lookup the AddressableGroupData to find the address, then check labels to see if we can get the platform specific one
                    if (AddressableAssetEntries.IsNullOrEmpty())
                    {
                        List<AddressableAssetEntry> entriesList = new List<AddressableAssetEntry>();
                        AddressableAssetSettingsDefaultObject.Settings.GetAllAssets(entriesList, true);
                        string platformLabel = Common.GetPlatform().ToString();

                        //filter out the ones which have the wrong platform
                        foreach (AddressableAssetEntry addressableAssetEntry in entriesList)
                        {
                            if (addressableAssetEntry.labels.Contains(platformLabel))
                            {
                                AddressableAssetEntries.Add(addressableAssetEntry);
                            }
                        }

                    }
                    List<IResourceLocation> found = new List<IResourceLocation>();
                    foreach (AddressableAssetEntry addressableAssetEntry in AddressableAssetEntries)
                    {
                        //find the ones with the matching prefab address
                        if (addressableAssetEntry.address.Equals(address))
                        {
                            //then check if the assetPath matches
                            foreach (IResourceLocation resourceLocation in locations)
                            {
                                if (addressableAssetEntry.AssetPath.Equals(resourceLocation.InternalId))
                                {
                                    found.Add(resourceLocation);
                                }
                            }
                        }
                    }
                    if (found.Count == 1)
                    {
                        resultLocation = found[0];
                    }
                    //if we found more than 1, then there must be a duplicate address, with the same label AND prefab, seems unlikely
                    if (found.Count > 1)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (IResourceLocation resourceLocation in found)
                        {
                            sb.Append(resourceLocation.PrimaryKey).Append("@").Append(resourceLocation.InternalId).Append(", ");
                        }
                        Debug.LogWarning($"Was unable to find a single matching addressable location for this address: {address} with platform: {Common.GetPlatform().ToString()}. We think its one of these and will default to the first one: {sb}");
                        resultLocation = found[0];
                    }

                    //Hopefully this never happens, because unity found one already, so we should find it, unless our checks are wrong
                    if (found.Count == 0)
                    {
                        Debug.LogWarning($"Was unable to find a matching addressable location for this address: {address} with platform: {Common.GetPlatform().ToString()}. Unity thinks {resultLocation.InternalId} is the right one");
                    }

#else
                    //multiple locations, if its a sub address, try to filter on that one
                    GetAddressAndSubAddress(address, out string addressOnly, out string subAddress);
                    if (!string.IsNullOrEmpty(subAddress))
                    {
                        resultLocation = locations.First(location => location.InternalId.Contains(subAddress));
                    }
                    else
                    {
                        Debug.LogWarning($"Multiple addressable locations found for address: {address}. There could be duplicate assets with the same address. Using first one: {resultLocation.InternalId}");
                    }
#endif
                }

                TryAddAddressLocation(address, type, resultLocation);
                callback?.Invoke(resultLocation);
            }
            else
            {
                Debug.LogWarning($"Address [{address}] not found for [{requestName}]");
                Addressables.Release(handle);
                callback?.Invoke(null);
            }
        }


        //Load Asset
        public static void LoadAssetAsync<T>(string address, Action<T> callback, string handlerName) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                callback.Invoke(EditorLoad<T>(address));
                return;
            }
#endif
            if (!Application.isPlaying || string.IsNullOrEmpty(address))
            {
                callback?.Invoke(null);
                return;
            }

            //try to get the result from the cache first
            if (TryGetAddressLocation<T>(address, out IResourceLocation cachedLocation))
            {
                LoadAssetAsync<T>(cachedLocation, callback, handlerName);
                return;
            }
            //Otherwise load the location
            LoadLocationAsync<T>(address, location => LoadAssetAsync<T>(location, callback, handlerName), handlerName);
        }

        public static void LoadAssetAsync<T>(object location, Action<T> callback, string handlerName) where T : UnityEngine.Object
        {
            if (!Application.isPlaying || location == null)
            {
                callback?.Invoke(null);
                return;
            }
            AsyncOperationHandle<T> handle;
            //This is to support calling the appropriate Addressables API for loading a location vs a key
            IResourceLocation resourceLocation = null;
            if (location is IResourceLocation)
            {
                resourceLocation = location as IResourceLocation;
                handle = Addressables.LoadAssetAsync<T>(resourceLocation);
            }
            else
            {
                handle = Addressables.LoadAssetAsync<T>(location);
            }
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Addressable operation failed load location for handlerName: {handlerName} - Please check the bundles have been built correctly. {handle.OperationException}");
                if (handle.OperationException == null) Catalog.ReleaseAsset(handle);
                callback?.Invoke(null);
                return;
            }
            handle.Completed += (operationHandle) => {
                if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    callback?.Invoke(operationHandle.Result);
                    return;
                }
                Debug.LogWarning($"Unable to find asset at resource location [{typeof(T).Name}][{resourceLocation?.ResourceType}][{resourceLocation?.PrimaryKey}] for object [{handlerName}]");
                if (handle.OperationException == null) Catalog.ReleaseAsset(operationHandle);
                callback?.Invoke(null);
            };
        }

        public static IEnumerator LoadAssetCoroutine<T>(string address, Action<T> callback, string handlerName) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                callback.Invoke(EditorLoad<T>(address));
                yield break;
            }
#endif
            if (!Application.isPlaying || string.IsNullOrEmpty(address))
            {
                callback?.Invoke(null);
                yield break;
            }

            IResourceLocation location = null;
            yield return LoadLocationCoroutine<T>(address, value => location = value, handlerName);
            yield return LoadAssetCoroutine<T>(location, callback, handlerName);

        }

        public static IEnumerator LoadAssetCoroutine<T>(IResourceLocation location, Action<T> callback, string handlerName) where T : UnityEngine.Object
        {
            if (location == null)
            {
                callback?.Invoke(null);
                yield break;
            }

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(location);
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Addressable operation failed load location for handlerName: {handlerName} - Please check the bundles have been built correctly. {handle.OperationException}");
                if (handle.OperationException == null) Catalog.ReleaseAsset(handle);
                callback?.Invoke(null);
                yield break;
            }
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                callback.Invoke(handle.Result);
                yield break;
            }

            Debug.LogWarning($"Unable to find asset at resource location [{typeof(T).Name}][{location.ResourceType}][{location.PrimaryKey}] for object [{handlerName}]");
            if (handle.OperationException == null) Catalog.ReleaseAsset(handle);
            callback?.Invoke(null);
        }

        //Instantiate Asset
        public static void InstantiateAsync(string address, Vector3 position, Quaternion rotation, Transform parent, Action<GameObject> callback, string handlerName)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GameObject instance = UnityEditor.PrefabUtility.InstantiatePrefab(EditorLoad<GameObject>(address), parent) as GameObject;
                if (instance != null)
                {
                    instance.transform.SetPositionAndRotation(position, rotation);
                }
                else
                {
                    Debug.LogError($"Unable to instantiate prefab: {address}");
                }
                callback.Invoke(instance);
                return;
            }
#endif

            if (!Application.isPlaying || string.IsNullOrEmpty(address))
            {
                callback?.Invoke(null);
                return;
            }

            //try to get the result from the cache first
            if (TryGetAddressLocation<GameObject>(address, out IResourceLocation cachedLocation))
            {
                InstantiateAsync(cachedLocation, position, rotation, parent, callback, handlerName);
                return;
            }
            //Otherwise load the location
            LoadLocationAsync<GameObject>(address, location => InstantiateAsync(location, position, rotation, parent, callback, handlerName), handlerName);
        }

        public static void InstantiateAsync(object location, Vector3 position, Quaternion rotation, Transform parent, Action<GameObject> callback, string handlerName)
        {
            if (!Application.isPlaying || location == null)
            {
#if !UNITY_EDITOR
                Debug.LogWarning($"Prefab location is null, unable to instantiate gameobject for object {handlerName}");
#endif
                callback?.Invoke(null);
                return;
            }

            AsyncOperationHandle<GameObject> handle;
            //This is to support calling the appropriate Addressables API for loading a location vs a key
            IResourceLocation resourceLocation = null;
            if (location is IResourceLocation)
            {
                resourceLocation = location as IResourceLocation;
                handle = Addressables.InstantiateAsync(resourceLocation, position, rotation, parent);
            }
            else
            {
                handle = Addressables.InstantiateAsync(location, position, rotation, parent);
            }
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Addressable operation failed InstantiateAsync for handlerName: {handlerName} - Please check the bundles have been built correctly. {handle.OperationException}");
                if (handle.OperationException == null) Catalog.ReleaseAsset(handle);
                callback?.Invoke(null);
                return;
            }
            handle.Completed += (operationHandle) => {
                if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    callback?.Invoke(operationHandle.Result);
                }
                else
                {
                    Debug.LogWarning($"Unable to instantiate gameObject from location {resourceLocation?.PrimaryKey} for object {handlerName}");
                    if (handle.OperationException == null) Addressables.ReleaseInstance(operationHandle);
                    callback?.Invoke(null);
                }
            };

        }

        public static IEnumerator InstantiateCoroutine<T>(string address, Action<T> callback, string handlerName) where T : UnityEngine.Object
        {
            if (!Application.isPlaying || string.IsNullOrEmpty(address))
            {
                callback?.Invoke(null);
                yield break;
            }

            IResourceLocation location = null;
            //Load location type here is always a gameobject, since were instantiating a prefab instance
            yield return LoadLocationCoroutine<GameObject>(address, value => location = value, handlerName);
            yield return InstantiateCoroutine<T>(location, callback, handlerName);
        }

        /// <summary>
        /// Instantiates a prefab and returns either the prefab itself if the Type param is GameObject or a component on the gameobject of type T
        /// </summary>
        /// <param name="location"></param>
        /// <param name="callback"></param>
        /// <param name="handlerName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerator InstantiateCoroutine<T>(object location, Action<T> callback, string handlerName) where T : UnityEngine.Object
        {
            if (location == null)
            {
                callback?.Invoke(null);
                yield break;
            }

            AsyncOperationHandle<GameObject> handle;
            //This is to support calling the appropriate Addressables API for loading a location vs a key
            IResourceLocation resourceLocation = null;
            if (location is IResourceLocation)
            {
                resourceLocation = location as IResourceLocation;
                handle = Addressables.InstantiateAsync(resourceLocation);
            }
            else
            {
                handle = Addressables.InstantiateAsync(location);
            }
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Addressable operation failed InstantiateAsync for handlerName: {handlerName} - Please check the bundles have been built correctly. {handle.OperationException}");
                if (handle.OperationException == null) Catalog.ReleaseAsset(handle);
                callback?.Invoke(null);
                yield break;
            }
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (typeof(T) == typeof(GameObject))
                {
                    callback?.Invoke(handle.Result as T);
                }
                else
                {
                    callback?.Invoke(handle.Result.GetComponent<T>());
                }
                yield break;
            }

            Debug.LogWarning($"Unable to find asset at location [{resourceLocation?.PrimaryKey}] of type [{typeof(T).Name}] for object [{handlerName}]");
            Addressables.Release(handle);
            callback?.Invoke(null);
        }

        //Release Asset
        public static void ReleaseAsset<TObject>(AsyncOperationHandle<TObject> handle)
        {
#if UNITY_EDITOR
            // Prevents errors being thrown while in editor and not playing
            if (!Application.isPlaying)
                return;
#endif
            Addressables.Release(handle);
        }

        public static void ReleaseAsset<TObject>(TObject obj)
        {
#if UNITY_EDITOR
            // Prevents errors being thrown while in editor and not playing
            if (!Application.isPlaying)
                return;
#endif
            Addressables.Release(obj);
        }

        public static void ReleaseAsset(AsyncOperationHandle handle)
        {
#if UNITY_EDITOR
            // Prevents errors being thrown while in editor and not playing
            if (!Application.isPlaying)
                return;
#endif
            Addressables.Release(handle);
        }


        public static T EditorLoad<T>(string address) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(address)) return null;
#if UNITY_EDITOR

            GetAddressAndSubAddress(address, out address, out string subAddress);

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            List<AddressableAssetEntry> allEntries = new List<AddressableAssetEntry>(settings.groups.SelectMany(g => g.entries)).FindAll(e => e.address == address);

            foreach (AddressableAssetEntry entry in allEntries)
            {
                if (entry.labels.Contains(Common.GetPlatform().ToString()))
                {
                    if (subAddress != null)
                    {
                        UnityEngine.Object[] objects = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(entry.AssetPath); //loads all sub-assets from one asset

                        if (objects.Length > 0)
                        {
                            for (int i = 0; i < objects.Length; i++) // loop on all sub-assets loaded
                            {
                                if (objects[i].name == subAddress && objects[i].GetType() == typeof(T)) // if the name AND the type match we found it
                                {
                                    return objects[i] as T;
                                }
                            }
                        }
                    }
                    else
                    {
                        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(entry.AssetPath);
                    }
                }
            }

            return null;
#else
            Debug.LogError("Can't use Catalog.EditorLoad() when game is played!");
            return null;
#endif
        }

        public static bool EditorExist<T>(string address)
        {
            if (string.IsNullOrEmpty(address)) return false;
#if UNITY_EDITOR
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            List<AddressableAssetEntry> allEntries = new List<AddressableAssetEntry>(settings.groups.SelectMany(g => g.entries)).FindAll(e => e.address == address);

            foreach (AddressableAssetEntry entry in allEntries)
            {
                if (entry.labels.Contains(Common.GetPlatform().ToString()))
                {
                    return true;
                }
            }

            return false;
#else
            Debug.LogError("Can't use Catalog.EditorExist() when game is played!");
            return false;
#endif
        }

        public static string GetAddressFromPrefab(UnityEngine.Object target)
        {
#if UNITY_EDITOR
            string path = UnityEditor.AssetDatabase.GetAssetPath(target);
            string guid = UnityEditor.AssetDatabase.AssetPathToGUID(path);
            AddressableAssetEntry assetEntry = AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(guid);
            if (assetEntry == null)
            {
                Debug.LogError($"Asset {target.name} can't be found in addressable");
                return null;
            }
            UnityEngine.Object[] objects = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
            if (objects.Length > 0)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    if (objects[i] == target)
                    {
                        return assetEntry.address + "[" + objects[i].name + "]";
                    }
                }
            }

            return assetEntry.address;
#else
             Debug.LogError("Can't use Catalog.GetAddressFromPrefab() when game is played!");
            return null;
#endif
        }

    }
}

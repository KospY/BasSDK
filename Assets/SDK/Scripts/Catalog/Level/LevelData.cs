using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine.ResourceManagement.ResourceProviders;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class LevelData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Level")]
#endif
        public string name;
#if ODIN_INSPECTOR
        [BoxGroup("Level")]
#endif
        [TextArea]
        public string description;
#if ODIN_INSPECTOR
        [BoxGroup("Level")]
#endif
        public string descriptionLocalizationId;
#if ODIN_INSPECTOR
        [BoxGroup("Level")]
#endif
        public string sceneAddress;
        [NonSerialized]
        public IResourceLocation sceneLocation;
#if ODIN_INSPECTOR
        [BoxGroup("Level")]
#endif
        public bool showOnlyDevMode = false;

#if ODIN_INSPECTOR
        [BoxGroup("Level")]
#endif
        public bool showInLevelSelection = true;
#if ODIN_INSPECTOR
        [BoxGroup("Level")]
#endif
        public bool savePlayerInventoryOnUnload;

#if ODIN_INSPECTOR
        [BoxGroup("Map")]
#endif
        public string worldmapPrefabAddress = "Bas.Image.Map.Default";
        [NonSerialized]
        public IResourceLocation worldmapPrefabLocation;
        [NonSerialized]
        public GameObject worldmapPrefab;
        [NonSerialized]
        public int worldmapHash;

#if ODIN_INSPECTOR
        [BoxGroup("Map location")]
#endif
        public int mapLocationIndex;

#if ODIN_INSPECTOR
        [BoxGroup("Map location")]
#endif
        public bool showOnMap = true;

#if ODIN_INSPECTOR
        [BoxGroup("Map location")]
#endif
        public bool hideOnAndroid;

#if ODIN_INSPECTOR
        [BoxGroup("Map location")]
#endif
        public string mapLocationIconAddress = "Bas.Icon.Location.Default";
        [NonSerialized]
        public IResourceLocation mapLocationIconLocation;
        [NonSerialized]
        public Texture2D mapLocationIcon;

#if ODIN_INSPECTOR
        [BoxGroup("Map location")]
#endif
        public string mapLocationIconHoverAddress = "Bas.Icon.Location.DefaultHover";
        [NonSerialized]
        public IResourceLocation mapLocationIconHoverLocation;
        [NonSerialized]
        public Texture2D mapLocationIconHover;

#if ODIN_INSPECTOR
        [BoxGroup("Map location")]
#endif
        public string mapPreviewImageAddress;
        [NonSerialized]
        public IResourceLocation mapPreviewImageLocation;
        [NonSerialized]
        public Texture2D mapPreviewImage;

#if ODIN_INSPECTOR
        [BoxGroup("Modes"), ShowInInspector]
#endif
        public List<Mode> modes;

#if ODIN_INSPECTOR
        [BoxGroup("Modes")]
#endif
        public int modePickCountPerRank = 2;

#if ODIN_INSPECTOR
        [BoxGroup("Cameras")]
#endif
        public List<CustomCameras> customCameras;

#if ODIN_INSPECTOR
        [BoxGroup("AI")]
#endif
        public ThrowableReference throwableRefType = ThrowableReference.Item;
#if ODIN_INSPECTOR
        [BoxGroup("AI")]
#endif
        public string improvisedThrowableID = null;

        public enum ThrowableReference
        {
            Item,
            Table
        }

        [JsonIgnore]
        public ItemData throwableData
        {
            get
            {
                if (throwableItem != null)
                {
                    return throwableItem;
                }
                if (throwableTable != null)
                {
                    return throwableTable.Pick();
                }
                if (!string.IsNullOrEmpty(improvisedThrowableID))
                {
                    switch (throwableRefType)
                    {
                        case ThrowableReference.Item:
                            throwableItem = Catalog.GetData<ItemData>(improvisedThrowableID);
                            break;
                        case ThrowableReference.Table:
                            throwableTable = Catalog.GetData<LootTable>(improvisedThrowableID);
                            break;
                    }
                    return throwableData;
                }
                Debug.LogError("Could not find any improvised throwable data: Level_Master.json may have data overrides which clear improvised throwable by mistake.");
                return null;
            }
        }

        private ItemData throwableItem;
        private LootTable throwableTable;

        [Serializable]
        public struct CustomCameras
        {
            public Vector3 position;
            public Quaternion rotation;
        }

        [Serializable]
        public class Option

        {
            public string name;
            public string displayName;
            public string description;
            public Type type;
            public int minValue;
            public int maxValue;
            public int defaultValue;

            public List<string> valueList;

            public enum Type
            {
                Stars,
                Toggle,
                StringList,
            }

            [JsonIgnore]
            public Category catalogType;
            [Button]
            public void AddAllValueFromCatalogType()
            {
                if (valueList == null) valueList = new List<string>();
                valueList.AddRange(Catalog.GetAllID(catalogType));
            }
        }

        [Serializable]
        public class Mode
        {
            [JsonMergeKey]
            public string name = "Default";
            public string displayName = "{Default}";
            [TextArea]
            public string description = "{NoDescription}";

            public List<string> allowGameModes;

            public int mapOrder;

#if ODIN_INSPECTOR
            [TableList(AlwaysExpanded = true, ShowIndexLabels = true)]
#endif
            public DifficultyMultipliers[] difficultyMultipliers;

            [Serializable]
            public class DifficultyMultipliers
            {
                [Tooltip("Max health multiplier of the player")]
                public float playerHealthMultiplier = 1;
                [Tooltip("Multiplier for damage done by player to creatures (ignoring deep penetration)")]
                public float playerDamageMultiplier = 1;
                [Tooltip("Multiplier for damage done by creatures to player")]
                public float creatureDamageMultiplier = 1;
            }

            public bool playerCanDie = true;
            public string deathLossAddress = "Bas.Loss.Death";

#if ODIN_INSPECTOR
            [ShowInInspector]
#endif
            public List<LevelModule> modules = new List<LevelModule>();

            public List<Option> availableOptions = new List<Option>();
            
            public bool HasOption(string optionName)
            {
                if (availableOptions.IsNullOrEmpty()) return false;
                int availableOptionsCount = availableOptions.Count;
                for (int i = 0; i < availableOptionsCount; i++)
                {
                    Option availableOption = availableOptions[i];
                    if (availableOption.name.Equals(optionName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }

            public T GetModule<T>() where T : LevelModule
            {
                int modulesCount = modules.Count;
                for (int i = 0; i < modulesCount; i++)
                {
                    LevelModule levelModule = modules[i];
                    if (levelModule is T)
                    {
                        return levelModule as T;
                    }
                }
                return null;
            }

            public LevelModule GetModule(Type type)
            {
                int modulesCount = modules.Count;
                for (int i = 0; i < modulesCount; i++)
                {
                    LevelModule levelModule = modules[i];
                    if (levelModule.GetType() == type)
                    {
                        return levelModule;
                    }
                }
                return null;
            }

            public bool HasModule<T>() where T : LevelModule
            {
                T module = GetModule<T>();
                return module != null;
            }

            public bool HasModule(Type type)
            {
                LevelModule module = GetModule(type);
                return module != null;
            }

            public bool TryGetModule(Type type, out LevelModule module)
            {
                module = GetModule(type);
                return module != null;
            }
            
            public bool TryGetModule<T>(out T module) where T : LevelModule
            {
                module = GetModule<T>();
                return module != null;
            }
        }

        public enum MusicType
        {
            Background,
            Combat,
        }

        public override int GetCurrentVersion()
        {
            return 3;
        }

    }
}

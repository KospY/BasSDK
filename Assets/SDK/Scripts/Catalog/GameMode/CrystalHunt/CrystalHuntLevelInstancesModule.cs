using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThunderRoad.Modules
{
    /// <summary>
    /// Handles creating and updating the level instances for the Crystal Hunt game mode
    /// </summary>
    [Serializable]
    public class CrystalHuntLevelInstancesModule : LevelInstancesModule
    {
        //this dungeon type describes if this is a loot heavy or shard heavy dungeon
        // this enum will be used to get the correct multiplier for the end reward
        public enum LootType
        {
            Loot,
            Shard,
            Arena,
            Dalgarian,
        }

        public static string CrystalHuntGameModeId = "CrystalHunt";

        public string mapInfoPrefabAddress;

        public string difficultyIcon;
        public Color difficultyIconColor;

        public int minPinsToGeneratePerDay = 2;
        public int maxPinsToGeneratePerDay = 4;

        [Header("Arenas")]
        public List<LevelInfo> arenasLevelInfo;
        public int minNumberOfArenasAllowedPerDay = 1;
        public int maxNumberOfArenasAllowedPerDay = 1;

        [Header("Dalgarian Dungeon")]
        public LevelInfo dalgarianDungeonLevelInfo;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEnemyConfigs))]
#endif
        public List<string> dalgarianEnemyConfigs;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllLootConfigs))]
#endif
        public List<string> dalgarianLootConfigs;
        public int dalgarianMapLocationRandomNearest = 3;

        [Header("Tutorial")]
        public LevelInfo tutorialDungeonLevelInfo;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEnemyConfigs))]
#endif
        public string tutorialEnemyConfig;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllLootConfigs))]
#endif
        public string tutorialLootConfig;

        [Header("Outpost Dungeon")]
        public LevelInfo outpostDungeonLevelInfo;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEnemyConfigs))]
#endif
        public List<string> outpostEnemyConfigs;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllLootConfigs))]
#endif
        public List<string> outpostLootConfigs;
        public int outpostMapLocationRandomNearest = 5;

        [Header("Shop & Persistent levels")]
        public string shopID;
        public List<LevelInfo> persistentLevels;

 // ProjectCore
        
        [System.Serializable]
        public class LevelInfo
        {
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllLevelId))]
#endif
            public string levelId;
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllLevelMode))]
#endif
            public string modeId;

            [Newtonsoft.Json.JsonProperty("modId")]
            private string modeIdSetter
            {
                set
                {
                    modeId = value;
                }
            }

 // ProjectCore
#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllLevelId()
            {
                return Catalog.GetDropdownAllID<LevelData>();
            }

            public List<ValueDropdownItem<string>> GetAllLevelMode()
            {
                List<ValueDropdownItem<string>> result = new List<ValueDropdownItem<string>>();
                LevelData level = Catalog.GetData<LevelData>(levelId);
                if (level == null) return result;

                List<LevelData.Mode> levelModes = level.modes;
                int count = levelModes.Count;
                for (int i = 0; i < count; i++)
                {
                    result.Add(new ValueDropdownItem<string>(levelModes[i].name, levelModes[i].name));
                }
                return result;
            }
#endif //ODIN_INSPECTOR
        }


        #region ScriptableObjects

        public string endRewardBalanceAddress;
        public string dungeonLengthBalanceAddress;
        public string dungeonLootMultiplierBalanceAddress;
        public string dungeonTypeBalanceAddress;
        public string outpostFactionTierBalanceAddress;
        public string dalgarianFactionTierBalanceAddress;
 // ProjectCore
        #endregion


#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEnemyConfigs()
        {
            return Catalog.GetDropdownAllID<EnemyConfig>();
        }

        public List<ValueDropdownItem<string>> GetAllLootConfigs()
        {
            return Catalog.GetDropdownAllID<LootConfigData>();
        }
#endif
    }
}

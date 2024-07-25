using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Areas/ItemSpawnerLimiter.html")]
    public class ItemSpawnerLimiter : MonoBehaviour
    {
        #region Public Fields
        [Tooltip("Maximum amount of items that can spawn in a room")]
        public int maxSpawn = 10;
        [Tooltip("The maximum amount of child items in a room (items that have parent spawners)")]
        public int maxChildSpawn = 6;

        [Tooltip("Maximum amount of items that can spawn in a room on the Android Variant")]
        public int androidMaxSpawn = 4;
        [Tooltip("Maximum amount of child items that can spawn in a room on the Android Variant")]
        public int androidMaxChildSpawn = 2;

        [Tooltip("When set to true, the items will spawn on start.")]
        public bool spawnOnStart = true;
        [Tooltip("When set to true, the items will spawn on level load.")]
        public bool spawnOnLevelLoad = false;

        [NonSerialized]
        public int spawnCount;
        [NonSerialized]
        public int spawnChildCount;
        [NonSerialized]
        public ItemSpawner[] itemSpawners;

        [NonSerialized]
        public System.Random randomGen;

        #endregion

        #region Private Fields

        private int treasureSpawnersToActivate = 0; 
        private int activeTreasureSpawners = 0;  //additional loot inside chest, scales off of dungeon length
        private int sideRoomLootSpawnersToActivate = 0;
        private int activeSideRoomLootSpawners = 0; //additional side rooms, loose loot around chest. Random amount from 1 - maxSideRoomLootSpawners
        private int totalSideRoomLootSpawners = 0; //total number of side room loot spawners in the area, whether active or not
        private List<ItemSpawner> sideRoomSpawners = new List<ItemSpawner>();
        private List<ItemSpawner> treasureSpawners = new List<ItemSpawner>();
        private List<ItemSpawner> rewardSpawners = new List<ItemSpawner>();

        #endregion

    }
}

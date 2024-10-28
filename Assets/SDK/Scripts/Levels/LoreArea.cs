#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif //ODIN_INSPECTOR
using System.Collections.Generic;
using System.Linq;
using ThunderRoad.Modules;
using UnityEngine;

namespace ThunderRoad {
    public class LoreArea : MonoBehaviour 
    {
        #region Public Fields
        public List<LoreSpawner> loreSpawners = new List<LoreSpawner>();
        public string levelID = "";
        public string roomID = "";
        #endregion

        #region Private Fields
        private LoreModule loreModule;
        #endregion


        public void HandleLevelLoaded(LevelData levelData, LevelData.Mode mode, EventTime eventTime)
        {
        }

        public void SetLevelId(string levelId)
        {
            this.levelID = levelId;

            // Add levelId and RoomId to lore spawner condition parameters
            int spawnCount = loreSpawners.Count;
            if (!string.IsNullOrEmpty(levelId))
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    if (!loreSpawners[i].loreConditionOptionalParameters.Contains(levelID))
                    {
                        loreSpawners[i].loreConditionOptionalParameters.Add(levelId);
                    }
                }
            }
        }

        public void SetRoomId(string roomId)
        {
            this.roomID = roomId;
            SetLoreSpawnerRoomId();
        }

        private void SetLoreSpawnerRoomId()
        {
            // Add levelId and RoomId to lore spawner condition parameters
            int spawnCount = loreSpawners.Count;
            if (!string.IsNullOrEmpty(roomID))
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    loreSpawners[i].loreConditionOptionalParameters.Add(roomID);
                }
            }
        }

    }
}


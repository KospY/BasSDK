#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif //ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class LoreManager : MonoBehaviour
    {
        public static Action onLoreCollected;
        
        public List<LoreArea> loreAreas = new List<LoreArea>();
        public List<LoreSpawner> activeLoreSpawners = new List<LoreSpawner>();
      
        private float delayTime = 2f;
        private System.Random rng;
        public static LoreManager instance = null;


        public void HandleLevelLoaded(LevelData levelData, LevelData.Mode mode, EventTime eventTime)
        {
            if(eventTime == EventTime.OnEnd)
            {
                PopulateLoreAreas();
            }
        }

        public void PopulateLoreAreas()
        {
            //aggregate all active lore spawners from all lore areas 
            foreach (LoreArea loreArea in loreAreas) 
            {
                activeLoreSpawners.AddRange(loreArea.loreSpawners);
            }

            //shuffle the lore spawners to evenly distrubute throughout areas
            Shuffle(activeLoreSpawners, rng);

            //populate lore on shuffled active spawners
            foreach (LoreSpawner loreSpawner in activeLoreSpawners)
            {
                loreSpawner.Init();
                loreSpawner.PopulateLore();
            }
        }

        public void Shuffle<T>(IList<T> list, System.Random randomGen)
        {
            
        }

        [Button]
        public void MarkAllLoreRead()
        {
            int count = 0;
            foreach (LoreSpawner loreSpawner in activeLoreSpawners)
            {
                count += loreSpawner.loreItems.Count;
                loreSpawner.MarkAsRead(false);
            }
            Debug.Log($"collected <color=red>{count}</color>");
        }
    }
}

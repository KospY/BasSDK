using System;
using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class ItemSpawnerLimiter : MonoBehaviour
    {
        public int maxSpawn = 10;
        public int maxChildSpawn = 6;

        public int androidMaxSpawn = 4;
        public int androidMaxChildSpawn = 2;

        public bool spawnOnStart = true;

        [NonSerialized]
        public int spawnCount;
        [NonSerialized]
        public int spawnChildCount;
        [NonSerialized]
        public ItemSpawner[] itemSpawners;

        [NonSerialized]
        public System.Random randomGen;

        private void Start()
        {
            if (!this.enabled) return;
            if (spawnOnStart)
            {
                randomGen = new System.Random(Level.seed);
                SpawnAll();
            }
        }

        [Button]
        public void SpawnAll()
        {
            spawnCount = 0;
            spawnChildCount = 0;
            itemSpawners = this.GetComponentsInChildren<ItemSpawner>();
            Shuffle(itemSpawners);
            int countMax = (Common.GetPlatform() == Platform.Android ? androidMaxSpawn : maxSpawn);
            foreach (ItemSpawner itemSpawner in itemSpawners)
            {
                // Mandatory Item will spawn on its own, limiter should not spawn them
                if (itemSpawner.spawnOnStart)
                {
                    if(itemSpawner.priority != ItemSpawner.Priority.Mandatory)
                    {
                        if (spawnCount >= countMax) continue;
                        
                        int max = countMax - spawnCount;
                        if (max > 1) max = randomGen.Next(1, max + 1);
                        spawnCount += itemSpawner.Spawn(checkSpawnLimiter: false, randomGen: randomGen, maxItemCount: max);
                    }
                    else
                    {
                        itemSpawner.Spawn(checkSpawnLimiter: false, randomGen: randomGen);
                    }
                }
            }
        }

        public void SpawnChild(ItemSpawner itemSpawner, bool allowDuplicates)
        {
            if (itemSpawner.priority != ItemSpawner.Priority.Mandatory)
            {
                int countMax = (Common.GetPlatform() == Platform.Android ? androidMaxChildSpawn : maxChildSpawn);
                if (spawnChildCount >= countMax) return;

                int max = countMax - spawnChildCount;
                if (max > 1) max = randomGen.Next(1, max + 1);
                spawnChildCount += itemSpawner.Spawn(checkParentSpawner: false, checkSpawnLimiter: false,
                    randomGen: randomGen, maxItemCount: max, allowDuplicates: allowDuplicates);
            }
            else
            {
                itemSpawner.Spawn(checkParentSpawner: false, checkSpawnLimiter: false, randomGen: randomGen, allowDuplicates:allowDuplicates);
            }
        }

        public void Shuffle<T>(IList<T> list)
        {            
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = randomGen.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public bool IsItemSpawning()
        {
            if (itemSpawners == null) return false;
            for (int i = 0; i < itemSpawners.Length; i++)
            {
                if (itemSpawners[i].IsCurrentlySpawning())
                {
                    return true;
                }
            }

            return false;
        }
    }
}

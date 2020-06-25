using UnityEngine;
using System.Collections.Generic;
using System.Collections;
#if ProjectCore
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class CreatureSpawner : MonoBehaviour
    {
#if ProjectCore
        [ValueDropdown("GetAllCreatureID")]
        public string creatureId;
        [ValueDropdown("GetAllContainerID")]
        public string containerID;
        [ValueDropdown("GetAllBrainID")]
        public string brainId;
        [ValueDropdown("GetAllFactionID")]
        public int factionId;
#else
        public string creatureId;
        public string containerID;
        public string brainId;
        public int factionId;
#endif

        public bool pooled = true;
        public float spawnDelay;
        public int spawnCount = 1;

        public int rowCount = 10;
        public float rowSpace = 1;
        public float delayBetweenSpawn = 0.5f;
        public bool spawnOnStart = true;

#if ProjectCore
        public List<ValueDropdownItem<string>> GetAllCreatureID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Creature);
        }

        public List<ValueDropdownItem<string>> GetAllContainerID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Container);
        }

        public List<ValueDropdownItem<string>> GetAllBrainID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Brain);
        }

        public List<ValueDropdownItem<int>> GetAllFactionID()
        {
            return Catalog.gameData.GetFactions();
        }

        protected void Start()
        {
            if (spawnOnStart)
            {
                if (spawnDelay > 0)
                {
                    Invoke("Spawn", spawnDelay);
                }
                else
                {
                    Spawn();
                }
            }
        }

        [Button]
        public void Spawn()
        {
            if (creatureId != "" && creatureId != null)
            {
                StartCoroutine(SpawnCoroutine());
            }
        }

        IEnumerator SpawnCoroutine()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Spawn(this.transform.position + (this.transform.right * rowSpace * (i % rowCount)) + (this.transform.forward * rowSpace * Mathf.FloorToInt((i / rowCount))));
                yield return new WaitForSeconds(delayBetweenSpawn);
            }
        }

        public void Spawn(Vector3 position)
        {
            CreatureData creatureData = Catalog.GetData<CreatureData>(creatureId).Clone() as CreatureData;
            creatureData.factionId = factionId;
            if (brainId != "" && brainId != null && brainId != "None") creatureData.brainId = brainId;
            if (containerID != "" && containerID != null && containerID != "None") creatureData.containerID = containerID;
            creatureData.Spawn(position, this.transform.rotation, pooled);
        }
#endif
    }
}

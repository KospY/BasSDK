using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class CreatureSpawner : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllCreatureID")]
#endif
        public string creatureId;
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllContainerID")]
#endif
        public string containerID;
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllBrainID")]
#endif
        public string brainId;
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllFactionID")]
#endif
        public int factionId;

        public bool pooled = true;
        public float spawnDelay;
        public int spawnCount = 1;

        public int rowCount = 10;
        public float rowSpace = 1;
        public float delayBetweenSpawn = 0.5f;
        public bool spawnOnStart = true;
        public bool spawnOnNavMesh = true;

        public Transform waypoints;

#if PrivateSDK
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
#endif

        private void OnValidate()
        {
            IconManager.SetIcon(this.gameObject, IconManager.LabelIcon.Red);
        }

    }
}

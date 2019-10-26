using UnityEngine;
using System.Collections.Generic;
#if ProjectCore
using Sirenix.OdinInspector;
#endif

namespace BS
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

        public bool pooled;
        public bool spawnOnStart = true;

#if ProjectCore
        public List<ValueDropdownItem<string>> GetAllCreatureID()
        {
            return Catalog.current.GetDropdownAllID(Catalog.Category.Creature);
        }

        public List<ValueDropdownItem<string>> GetAllContainerID()
        {
            return Catalog.current.GetDropdownAllID(Catalog.Category.Container);
        }

        public List<ValueDropdownItem<string>> GetAllBrainID()
        {
            return Catalog.current.GetDropdownAllID(Catalog.Category.Brain);
        }

        public List<ValueDropdownItem<int>> GetAllFactionID()
        {
            return Catalog.current.gameData.GetFactions();
        }

        protected void Start()
        {
            if (spawnOnStart) Spawn();
        }

        [Button]
        public Creature Spawn()
        {
            if (creatureId != "" && creatureId != null)
            {
                CreatureData creatureData = Catalog.current.GetData<CreatureData>(creatureId).Clone() as CreatureData;
                creatureData.factionId = factionId;
                if (brainId != "" && brainId != null && brainId != "None") creatureData.brainId = brainId;
                if (containerID != "" && containerID != null && containerID != "None") creatureData.containerID = containerID;
                return creatureData.Spawn(this.transform.position, this.transform.rotation, pooled);
            }
            return null;
        }
#endif
    }
}

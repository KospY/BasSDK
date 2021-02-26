using UnityEngine;
using System.Collections.Generic;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class CreatureTableSpawner : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllCreatureTableID")]
#endif
        public string creatureTableID;

        public bool pooled;
        public bool spawnOnStart = true;
        public float spawnDelay;
        public GameObject waypoints;

#if PrivateSDK
        public List<ValueDropdownItem<string>> GetAllCreatureTableID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.CreatureTable);
        }
#endif
    }
}

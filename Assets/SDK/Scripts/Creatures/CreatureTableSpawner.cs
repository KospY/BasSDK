using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

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
        public bool spawnOnNavMesh = true;

        public bool spawnAtRandomWaypoint = true;

        public Transform waypoints;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllCreatureTableID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.CreatureTable);
        }
#endif

        private void OnValidate()
        {
            IconManager.SetIcon(this.gameObject, IconManager.LabelIcon.Orange);
        }

    }
}

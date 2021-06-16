using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.Events;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Spawners/CreatureTable Spawner")]
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
        public bool ignoreRoomMaxNPC;
        public bool spawnAtRandomWaypoint = true;
        public Transform waypointsRoot;

        [System.Serializable]
        public class CreatureEvent : UnityEvent<UnityEngine.Object> { }

        [Header("Event")]
        public CreatureEvent OnCombatState = new CreatureEvent();
        public UnityEvent OnAlertState = new UnityEvent();
        public UnityEvent OnKill = new UnityEvent();
        public UnityEvent OnDespawn = new UnityEvent();

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllCreatureTableID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.CreatureTable);
        }
#endif

        private void OnValidate()
        {
            IconManager.SetIcon(this.gameObject, null);
        }

    }
}

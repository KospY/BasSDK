using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.Events;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Levels/Spawners/CreatureTable Spawner")]
    public class CreatureSpawner : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllCreatureTableID")]
#endif
        public string creatureTableID;

        public bool pooled;
        public bool spawnOnStart = true;
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

        [NonSerialized]
        public bool spawning;

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

        [Button]
        public void Spawn()
        {
        }
    }
}

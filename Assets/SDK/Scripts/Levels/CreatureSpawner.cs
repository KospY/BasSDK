using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.Events;
using System;
using ThunderRoad.Modules;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/CreatureSpawner.html")]
    [AddComponentMenu("ThunderRoad/Levels/Spawners/CreatureTable Spawner")]
    public class CreatureSpawner : MonoBehaviour
    {
        public enum State
        {
            Init,
            Spawning,
            Spawned
        }

        public enum ReferenceType
        {
            None,
            EnemyConfig
        }

        public enum EnemyConfigType
        {
            PatrolMix,
            PatrolMelee,
            PatrolRanged,
            AlertMix,
            AlertMelee,
            AlertRanged,
            RareMix,
            RareMelee,
            RareRanged
        }
        
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllCreatureTableID))]
        [ShowIf("referenceType", Value = ReferenceType.None)]
#endif
        public string creatureTableID;
        public ReferenceType referenceType = ReferenceType.EnemyConfig;
#if ODIN_INSPECTOR
        [ShowIf("referenceType", Value = ReferenceType.EnemyConfig)]
#endif
        public EnemyConfigType enemyConfigType;
        
        [Tooltip("Spawns creatures in faster/immediately, but can break some of them. If your creatures are behaving weirdly, uncheck this box.")]
        public bool asyncSpawn = true;
        [Tooltip("The creature spawner is ignored by areas(?)")]
        public bool ignoredByAreas = false;
        [Tooltip("When set to true, the creature will spawn on start")]
        public bool spawnOnStart = true;
        [Tooltip("When enabled, creature will respawn when the killed.")]
        public bool respawnOnDeath = false;
        #if ODIN_INSPECTOR
        [ShowIf(nameof(respawnOnDeath))]
#endif
        [Tooltip("Sets a delay for the respawn.")]
        public float respawnDelay = 1f;
        [Tooltip("If set to true, if this spawner is called on Level load, it'll delay the end of the loading screen until the creature is done spawning.")]
        public bool blockLoad = false;
        [Tooltip("Spawns the creature on a navmesh if it isn't already.")]
        public bool spawnOnNavMesh = true;
        [Tooltip("When enabled, will ignore the Dungeon rooms' max NPC count.")]
        public bool ignoreRoomMaxNPC;
        [Tooltip("When enabled, will spawn the creature on a random waypoint.")]
        public bool spawnAtRandomWaypoint = true;
        [Tooltip("Specify the waypoint the creature spawns on.")]
        public Transform waypointsRoot;

        [System.Serializable]
        public class CreatureEvent : UnityEvent<UnityEngine.Object> { }

        [System.Serializable]
        public class BrainStateEvent
        {
            public Brain.State triggerState = Brain.State.Combat;
            public float timeDelay = 0f;
            [Tooltip("This should be toggled on if this brain state change event starts a wave")]
            public bool addCreatureTargetShareDelay = false;
#if ODIN_INSPECTOR
            [BoxGroup("Post-delay checks")]
#endif
            public bool checkAlive = true;
#if ODIN_INSPECTOR
            [BoxGroup("Post-delay checks")]
#endif
            public bool checkNotMuffled = true;
#if ODIN_INSPECTOR
            [BoxGroup("Post-delay checks")]
#endif
            public bool checkSameState = true;
            public CreatureEvent onChange;
        }

        [Header("Events")]
        public List<BrainStateEvent> brainStateChangeEvents = new();
        public UnityEvent OnStart = new();
        public UnityEvent OnKill = new();
        public UnityEvent<Creature> OnSpawn = new();
        public UnityEvent OnDespawn = new();

        public State CurrentState { get; private set; }

        private CreatureTable currentLoadedData;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllCreatureTableID()
        {
            return Catalog.GetDropdownAllID(Category.CreatureTable);
        }
#endif

        [Button]
        public void Spawn()
        {
            Spawn(null);
        }

        [Button]
        public void Spawn(Action completeCallback = null, System.Random randomGen = null)
        {
        }


        public void SpawnCreature(CreatureData creatureData, Action completeCallback = null)
        {
        }

        /// <summary>
        /// Returns the creature data linked to this ID in the catalog.
        /// </summary>
        /// <returns>The creature data in the catalog, null if not found</returns>
        private CreatureTable GetCreatureTableData()
        {
            return currentLoadedData;
        }

        [Button]
        public void ResetSpawner()
        {
        }

        [Button]
        public void SetCreaturesToWaveNPCS()
        {
        }

    }
}

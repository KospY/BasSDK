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
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/CreatureSpawner")]
    [AddComponentMenu("ThunderRoad/Levels/Spawners/CreatureTable Spawner")]
    public class CreatureSpawner : MonoBehaviour
    {
        public enum State
        {
            Init,
            Spawning,
            Spawned
        }

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllCreatureTableID")]
#endif
        public string creatureTableID;

        public bool pooled;
        [Tooltip("Spawns creatures in faster/immediately, but can break some of them. If your creatures are behaving weirdly, uncheck this box.")]
        public bool asyncSpawn = true;
        public bool spawnOnStart = true;
        [Tooltip("If set to true, if this spawner is called on Level load, it'll delay the end of the loading screen until the creature is done spawning.")]
        public bool blockLoad = false;
        public bool spawnOnNavMesh = true;
        public bool ignoreRoomMaxNPC;
        public bool spawnAtRandomWaypoint = true;
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
        public List<BrainStateEvent> brainStateChangeEvents = new List<BrainStateEvent>();
        public UnityEvent OnStart = new UnityEvent();
        public UnityEvent OnKill = new UnityEvent();
        public UnityEvent OnDespawn = new UnityEvent();

        public State CurrentState { get; private set; }

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

        public void Spawn()
        {
            Spawn(null);
        }

        [Button]
        public void Spawn(Action completeCallback = null)
        {
        }


        private IEnumerator ResetSpawnerCoroutine()
        {
            // Despawn any alive and dead creatures
            yield return DespawnCreaturesCoroutine(true);

            CurrentState = State.Init;

            // Spawn the creatures again
            Spawn();
        }

        /// <summary>
        /// Despawn creatures
        /// </summary>
        /// <param name="allCreatures">True if the dead creatures should also be despawned</param>
        /// <returns></returns>
        private IEnumerator DespawnCreaturesCoroutine(bool allCreatures)
        {
            yield break;
        }

    }
}

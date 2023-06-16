using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/WaveSpawner")]
    [AddComponentMenu("ThunderRoad/Levels/Spawners/Waves Spawner")]
    public class WaveSpawner : MonoBehaviour
    {
        public static List<WaveSpawner> instances = new List<WaveSpawner>();

        [Tooltip("List of spawn points, where NPC will spawn during a wave start.")]
        public List<Transform> spawns = new List<Transform>();
        [Tooltip("Adds FleePoint to the Spawn Point locations. When wave ends, alive NPC will move to these spots to despawn.")]
        public bool addAsFleepointOnStart = true;

        [Tooltip("Begins wave apon loading in to the level.")]
        public bool beginWaveOnStart;
        [Tooltip("Delay of Wave activation on start.")]
        public float beginWaveOnStartDelay;
        [Tooltip("ID of wave that begins on start")]
        public string startWaveId;
        [Tooltip("When ticked, the WaveSpawner will use creatures from the pool.")]
        public bool pooled = true;
        [Tooltip("When ticked, items and dead bodies will despawn when the wave starts. When disabled, bodies and items do not despawn on start.")]
        public bool cleanBodiesAndItemsOnWaveStart = true;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllFactionID")]
#endif
        [Tooltip("ID of the faction that is ignored by the NPCs of this wave.")]
        public int ignoredFactionId = -1;
        [Tooltip("Determines how often the spawner checks to see if it can spawn a new creature.")]
        public float updateRate = 2;
        [Tooltip("Determines the delay between spawning creatures at the same spawn point.")]
        public float sameSpawnDelay = 3;
        [Tooltip("Delay between NPC spawns.")]
        public float spawnDelay = 2;
        
        [Header("Event")]
        public UnityEvent OnWaveBeginEvent = new UnityEvent();
        public UnityEvent OnWaveAnyEndEvent = new UnityEvent();
        public UnityEvent OnWaveWinEvent = new UnityEvent();
        public UnityEvent OnWaveLossEvent = new UnityEvent();
        public UnityEvent OnWaveCancelEvent = new UnityEvent();
        public UnityEvent OnWaveLoopEvent = new UnityEvent();
        public static UnityEvent<WaveSpawner> OnWaveSpawnerEnabledEvent = new UnityEvent<WaveSpawner>();
        public static UnityEvent<WaveSpawner> OnWaveSpawnerDisabledEvent = new UnityEvent<WaveSpawner>();
        public static UnityEvent<WaveSpawner> OnWaveSpawnerStartRunningEvent = new UnityEvent<WaveSpawner>();
        public static UnityEvent<WaveSpawner> OnWaveSpawnerStopRunningEvent = new UnityEvent<WaveSpawner>();

    }
}
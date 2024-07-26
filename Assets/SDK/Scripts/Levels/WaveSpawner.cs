using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/WaveSpawner.html")]
    [AddComponentMenu("ThunderRoad/Levels/Spawners/Waves Spawner")]
    public class WaveSpawner : MonoBehaviour
    {
        public enum ReferenceType
        {
            None,
            EnemyConfig
        }

        public enum EnemyConfigType
        {
            MeleeOnlyStd,
            MeleeFocusedStd,
            MixedStd,
            RangedFocusedStd,
            MeleeOnlyArena,
            MeleeFocusedArena,
            MixedArena,
            RangedFocusedArena,
            MeleeOnlyEnd,
            MeleeFocusedEnd,
            MixedEnd,
            RangedFocusedEnd
        }

        public static List<WaveSpawner> instances = new List<WaveSpawner>();

        public ReferenceType referenceType = ReferenceType.EnemyConfig;
#if ODIN_INSPECTOR
        [ShowIf("referenceType", Value = ReferenceType.EnemyConfig)]
#endif
        public EnemyConfigType enemyConfigType;

        [Tooltip("When ticked, the wave spawner will ignore the dungeon limits and spawn NPCs regardless of the dungeon limit. This is useful for playing a dungeon room in sandbox mode")]
        public bool ignoreDungeonLimits = false;
        [Tooltip("List of spawn points, where NPC will spawn during a wave start.")]
        public List<Transform> spawns = new List<Transform>();
        [Tooltip("Adds FleePoint to the Spawn Point locations. When wave ends, alive NPC will move to these spots to despawn.")]
        public bool addAsFleepointOnStart = true;

        [Tooltip("Begins wave apon loading in to the level.")]
        public bool beginWaveOnStart;
        [Tooltip("Delay of Wave activation on start.")]
        public float beginWaveOnStartDelay;
        [Tooltip("ID of wave that begins on start")]
#if ODIN_INSPECTOR        
        [ShowIf("referenceType", true, Value = ReferenceType.None)]
#endif        
        public string startWaveId;
        [Tooltip("When ticked, items and dead bodies will despawn when the wave starts. When disabled, bodies and items do not despawn on start.")]
        public bool cleanBodiesAndItemsOnWaveStart = true;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllFactionID))]
#endif
        [Tooltip("ID of the faction that is ignored by the NPCs of this wave.")]
        public int ignoredFactionId = -1;
        [Tooltip("Determines how often the spawner checks to see if it can spawn a new creature.")]
        public float updateRate = 2;
        [Tooltip("Determines the delay between spawning creatures at the same spawn point.")]
        public float sameSpawnDelay = 3;
        [Tooltip("Delay between NPC spawns.")]
        public float spawnDelay = 2;
        [Tooltip("Automatically end the wave if no action after a specific duration.")]
        public float inactivityEndTimer = 60f;
        
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

        #region Exposed SDK methods
        public void StartWave(string waveId)
        {
        }

        public void CancelWave()
        {
        }

        public void StopWave(bool success)
        {
        }

        public void Clean()
        {
        }
        #endregion

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<int>> GetAllFactionID()
        {
            if (Catalog.gameData == null) return null;
            return Catalog.gameData.GetFactions();
        }

        public List<ValueDropdownItem<string>> GetAllWaveID()
        {
            return Catalog.GetDropdownAllID(Category.Wave);
        }
#endif
    }
}
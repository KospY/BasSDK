using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine.Events;
using UnityEngine.Profiling;
using UnityEngine.Serialization;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class GolemSpawner : ThunderBehaviour
    {
        public string golemAddress;
        public WeakPointRandomizer arenaCrystalRandomizer;
        public bool spawnOnStart = true;
        public SpawnAction actionOnSpawn = SpawnAction.None;
        
        [Header("Events")]
        public UnityEvent onStartWakeFull;
        public UnityEvent onStartWakeShortA;
        public UnityEvent onStartWakeShortB;
        public UnityEvent onGolemAwaken;
        public UnityEvent onGolemDefeat;
        public UnityEvent onGolemKill;
        public UnityEvent onGolemStun;
        public UnityEvent onCrystalGrabbed;
        public UnityEvent onCrystalUnGrabbed;

        [NonSerialized]
        public Golem golem;

        public enum SpawnAction
        {
            None,
            Disable,
            Wake,
        }

        private void Start()
        {
            if (spawnOnStart) SpawnGolem();
        }

        public void SpawnGolem() => SpawnGolem(golemAddress, actionOnSpawn, transform.position, transform.rotation, transform, arenaCrystalRandomizer, this);

        public static void SpawnGolem(string address, SpawnAction spawnAction, Vector3 position, Quaternion rotation, Transform parent,
            WeakPointRandomizer arenaCrystalRandomizer, GolemSpawner spawner)
        {
        }

        [Button]
        public void EnableGolem()
        {
        }

        [Button]
        public void WakeGolem()
        {
        }

        [Button]
        public void StunGolem()
        {
        }

        [Button]
        public void StunGolem(float time = 0f)
        {
        }

        [Button]
        public void DefeatGolem()
        {
        }

        public void StartWakeSequence(int num)
        {
        }
    }
}

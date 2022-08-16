using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/PlayerSpawner")]
    public class PlayerSpawner : MonoBehaviour
    {
        public static List<PlayerSpawner> all = new List<PlayerSpawner>();
        public static List<PlayerSpawner> allActive = new List<PlayerSpawner>();
        public static PlayerSpawner current;

        public string id = "default";
        [Tooltip("-1 will use the default spawning chances, please note spawners with -1 will by default be 50% when used with weighted spawners.")]
        public int spawnWeight = -1;
        public bool spawnBody = true;
        [Tooltip("Should the player forcefully spawn here if they are a new character (hasn't completed the tutorial)?")]
        public bool forceSpawnHereIfNewPlayer;

        public UnityEvent playerSpawnEvent;

        public enum Type
        {
            DefaultStart,
            AltnernateStart,
            Stage,
        }


        public IEnumerator SpawnCoroutine(Action callback = null)
        {
            yield break;
        }
    }
}

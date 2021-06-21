using UnityEngine;
using System.Collections.Generic;

namespace ThunderRoad
{
    public class PlayerSpawner : MonoBehaviour
    {
        public static List<PlayerSpawner> all = new List<PlayerSpawner>();
        public static List<PlayerSpawner> allActive = new List<PlayerSpawner>();

        private void Awake()
        {
            all.Add(this);
        }

        private void OnDestroy()
        {
            all.Remove(this);
        }

        private void OnEnable()
        {
            allActive.Add(this);
        }

        private void OnDisable()
        {
            allActive.Remove(this);
        }

        public static PlayerSpawner Get()
        {
            if (allActive.Count == 0) return null;
            PlayerSpawner randomSpawner = allActive[UnityEngine.Random.Range(0, allActive.Count)];
            return randomSpawner;
        }
    }
}

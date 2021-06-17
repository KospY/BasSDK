using UnityEngine;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Levels/Spawners/Item Spawner")]
    public class ItemSpawner : MonoBehaviour
    {
        public string itemId;
        public bool pooled;
        public bool spawnOnStart = true;
        public bool disallowDespawn;

        public Transform spawnPoint = null;

    }
}

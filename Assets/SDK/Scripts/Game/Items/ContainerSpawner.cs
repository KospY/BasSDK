using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class ContainerSpawner : MonoBehaviour
    {
        public string containerId;
        public bool pooled;
        public bool spawnOnStart = true;
        public bool disallowDespawn;

        public List<Transform> spawnPoints = new List<Transform>();

    }
}

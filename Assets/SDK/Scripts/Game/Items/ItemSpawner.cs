using UnityEngine;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class ItemSpawner : MonoBehaviour
    {
        public bool pooled;
        public bool spawnOnStart = true;

        public List<Transform> spawnPoints;
        public List<Holder> holders;

    }
}

using UnityEngine;
using System;
using System.Collections.Generic;

namespace BS
{
    public class LevelDefinition : MonoBehaviour
    {
        public static LevelDefinition current;
        public Transform playerStart;
        public List<Transform> spawnPoints;
        public List<CustomReference> customReferences;
        public bool loadDefaultCharIfNeeded = true;

        [Serializable]
        public class CustomReference
        {
            public string name;
            public List<Transform> transforms;
        }
    }
}

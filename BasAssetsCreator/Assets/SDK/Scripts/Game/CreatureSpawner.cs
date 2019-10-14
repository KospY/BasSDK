using System.Collections.Generic;
using UnityEngine;

namespace BS
{
    public class CreatureSpawner : MonoBehaviour
    {
        public string creatureId;
        public string containerID;
        public string brainId;
        public int factionId;

        public bool pooled;
        public bool spawnOnStart = true;
    }
}

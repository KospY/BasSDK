using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace ThunderRoad
{
    public class CreatureSpawner : MonoBehaviour
    {
        public string creatureId;
        public string containerID;
        public string brainId;
        public int factionId;

        public bool pooled = true;
        public float spawnDelay;
        public int spawnCount = 1;

        public int rowCount = 10;
        public float rowSpace = 1;
        public float delayBetweenSpawn = 0.5f;
        public bool spawnOnStart = true;

    }
}

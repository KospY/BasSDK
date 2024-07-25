using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class LevelModuleCheckpoints : LevelModule
    {
        public List<Checkpoint> checkpoints = new List<Checkpoint>();

        [Serializable]
        public class Checkpoint
        {
            public string checkpointZoneName = "";
            public string checkpointSpawnerName = "";
        }

    }
}

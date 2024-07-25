#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class LoreGazeReceiver : MonoBehaviour
    {
        public LoreSpawner loreSpawner;
        public Vector3 dir;
        private bool looking = false;

        [NonSerialized]
        public float distanceThreshold = 1f;
        [NonSerialized]
        public float lookDuration;
        [NonSerialized]
        public float lookThreshold;
        private Item item;

    }
}
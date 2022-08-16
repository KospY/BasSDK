using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class WaterHandler
    {

        [NonSerialized]
        public bool inWater;
        [NonSerialized]
        public float submergedRatio;
        [NonSerialized]
        public Vector3 waterSurfacePosition;
        [NonSerialized]
        public Vector3 waterSurfaceNormal;
        [NonSerialized]
        public Vector3 sampledPosition;

        public delegate void SimpleDelegate();
        public event SimpleDelegate OnWaterEnter;
        public event SimpleDelegate OnWaterExit;

    }
}

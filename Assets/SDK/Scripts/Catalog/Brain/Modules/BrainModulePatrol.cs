using UnityEngine;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	public class BrainModulePatrol : BrainData.Module
    {
        public float distanceChangePoint = 1;
        public float viewRadius;
        public float viewAngle;
        public LayerMask targetMask;
        public LayerMask obstacleMask;
        public float runSpeedRatio;
#if ODIN_INSPECTOR
        [NonSerialized, ShowInInspector, ReadOnly]
#endif
        public int targetWaypointIndex;
#if ODIN_INSPECTOR
        [NonSerialized, ShowInInspector, ReadOnly]
#endif
        public WayPoint[] waypoints;
    }
}
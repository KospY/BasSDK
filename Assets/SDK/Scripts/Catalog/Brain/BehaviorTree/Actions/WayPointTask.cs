using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
    public class WayPointTask : ActionNode
    {
        public string inputTransformVariableName = "Waypoint";
        public string outputTargetVariableName = "WaypointTarget";
        public bool resetInteruptAnimation;
    }
}

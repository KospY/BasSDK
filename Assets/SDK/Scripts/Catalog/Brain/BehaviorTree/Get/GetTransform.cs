using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Get
{
    public class GetTransform : ActionNode
    {
        public Target target = Target.NextWaypoint;
        public bool prioritizeShortestPath = true;
        public string outputTransformVariableName = "NavigationTarget";

        public enum Target
        {
            NextWaypoint,
            ClosestFleePoint,
            ClosestDungeonExit,
            RandomWayPoint,
        }

    }
}

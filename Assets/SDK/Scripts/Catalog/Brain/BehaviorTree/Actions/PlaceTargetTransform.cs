using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
    public class PlaceTargetTransform : ActionNode
    {
        public string vectorPositionName = "Position";
        public string transformVariableName = "PositionTargetTransform";

    }
}

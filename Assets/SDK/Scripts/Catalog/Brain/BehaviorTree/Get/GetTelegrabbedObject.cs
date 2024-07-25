using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Get
{
    public class GetTelegrabbedObject : ActionNode
    {
        public bool ignoreFriendlyTK = true;
        public float offsetHeightRatioWhenOnFloor = 1;
        public float raycastOffset = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float maxDistance = 0;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public bool debugLines;

#if ODIN_INSPECTOR
        [BoxGroup("Output")] 
#endif
        public string outputPositionVariableName = "TKGrabPosition";
#if ODIN_INSPECTOR
        [BoxGroup("Output")] 
#endif
        public string outputOffsetPositionVariableName = "TKGrabOffsetPosition";

    }
}

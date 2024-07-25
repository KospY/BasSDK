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
	public class GetLightPosition : ActionNode
    {
        public float offsetHeightRatioWhenOnFloor = 1;
        public float raycastOffset = 0.1f;

#if ODIN_INSPECTOR
        [BoxGroup("Output")] 
#endif
        public string outputPositionVariableName = "LightPosition";
#if ODIN_INSPECTOR
        [BoxGroup("Output")] 
#endif
        public string outputOffsetPositionVariableName = "LightOffsetPosition";

    }
}

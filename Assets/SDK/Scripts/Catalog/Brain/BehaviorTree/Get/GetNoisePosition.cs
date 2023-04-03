using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Get
{
	public class GetNoisePosition : ActionNode
    {
        public float offsetHeightRatioWhenOnFloor = 1;
        public float raycastOffset = 0.1f;

#if ODIN_INSPECTOR
        [BoxGroup("Output")] 
#endif
        public string outputPositionVariableName = "NoisePosition";
#if ODIN_INSPECTOR
        [BoxGroup("Output")] 
#endif
        public string outputOffsetPositionVariableName = "NoiseOffsetPosition";

    }
}

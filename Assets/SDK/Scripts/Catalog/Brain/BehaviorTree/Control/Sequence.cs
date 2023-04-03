using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Control
{
	[System.Serializable]
    public class Sequence : ControlNode
    {
#if ODIN_INSPECTOR
        [PropertyOrder(0)] 
#endif
        public bool evaluateAllNodesOnCycle;

    }
}

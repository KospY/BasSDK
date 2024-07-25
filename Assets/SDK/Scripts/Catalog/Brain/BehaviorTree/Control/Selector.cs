#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Control
{
	public class Selector : ControlNode
    {
#if ODIN_INSPECTOR
        [PropertyOrder(0)] 
#endif
        public bool evaluateAllNodesOnCycle;

    }
}
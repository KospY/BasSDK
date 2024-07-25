using System.Collections;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI
{
	public class ControlNode : Node
    {
#if ODIN_INSPECTOR
        [ListDrawerSettings(Expanded = true), PropertyOrder(10)] 
#endif
        public List<Node> childs = new List<Node>();
    }
}
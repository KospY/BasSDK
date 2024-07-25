using System.Collections;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Control
{
	public class Parallel : ControlNode
    {
#if ODIN_INSPECTOR
        [PropertyOrder(0)] 
#endif
        public bool stopOnFirstSuccess;
#if ODIN_INSPECTOR
        [PropertyOrder(0)] 
#endif
        public bool stopOnFirstFailure;

    }
}


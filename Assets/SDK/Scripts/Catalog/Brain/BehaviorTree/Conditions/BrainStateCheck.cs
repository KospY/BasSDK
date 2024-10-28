using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Condition
{
    public class BrainStateCheck : ConditionNode
    {
        public bool invert;
        public List<Brain.State> successStates = new List<Brain.State>();

    }
}

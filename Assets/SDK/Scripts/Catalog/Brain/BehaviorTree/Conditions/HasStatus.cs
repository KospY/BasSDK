#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Condition
{
    public class HasStatus : ConditionNode
    {
        public bool any = false;
        public string statusID;

    }
}

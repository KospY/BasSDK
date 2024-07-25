#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
    public class InterruptIdle : ActionNode
    {
        public bool alwaysFullInterrupt = false;
        public bool changeBrainStateBeforeInterrupt = false;
#if ODIN_INSPECTOR
        [ShowIf("changeBrainStateBeforeInterrupt")]
#endif
        public Brain.State brainState = Brain.State.Alert;

    }
}

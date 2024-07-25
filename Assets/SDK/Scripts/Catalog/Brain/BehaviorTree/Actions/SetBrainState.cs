#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class SetBrainState : ActionNode
    {
        public Brain.State brainState = Brain.State.Idle;
    }
}

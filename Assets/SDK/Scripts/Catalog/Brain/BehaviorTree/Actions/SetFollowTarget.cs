#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
{
	public class SetFollowTarget : ActionNode
    {
        public enum Action
        {
            Set,
            Clear,
        }
        public Action action = Action.Set;
        public string creatureVariableName = "";

    }
}
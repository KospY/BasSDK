
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class SetStance : ActionNode
    {
        public BrainModuleStance.Stance stance = BrainModuleStance.Stance.Idle;
        public bool onlyOnce = false;

#if ODIN_INSPECTOR
#endif

    }
}

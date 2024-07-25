#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
    public class StopMove : ActionNode
    {
        public bool stopMovement;
        public bool stopTurning;

        protected BrainModuleMove moduleMove;

        public override void Init(Creature p_creature, Blackboard p_blackboard)
        {
            base.Init(p_creature, p_blackboard);
        }

        public override State Evaluate()
        {
            return State.SUCCESS;
        }
    }
}

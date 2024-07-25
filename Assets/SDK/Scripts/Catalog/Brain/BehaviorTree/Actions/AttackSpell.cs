
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class AttackSpell : ActionNode
    {
        public Target target = Target.CurrentTarget;
#if ODIN_INSPECTOR
        [HideIf("target", optionalValue: Target.CurrentTarget)] 
#endif
        public string inputTransformVariableName = "";

        public enum Target
        {
            CurrentTarget,
            InputTransform,
        }

        public bool castLeft = true;
        public bool castRight = false;

        public enum CastSide
        {
            None,
            CastRight,
            CastLeft,
            CastBoth,
        }

    }
}


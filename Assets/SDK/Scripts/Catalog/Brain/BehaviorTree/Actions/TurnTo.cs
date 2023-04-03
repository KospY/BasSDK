using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
{
	public class TurnTo : ActionNode
    {
        public Target target = Target.CurrentTarget;
#if ODIN_INSPECTOR
        [HideIf("target", optionalValue: Target.CurrentTarget)] 
#endif
        public string inputVariableName = "TurnTarget";

        public bool successIfAngleReached;
        public float successAngle = 0;

        public bool becomeExclusiveHandler = false;

        public enum Target
        {
            CurrentTarget,
            InputTransform,
            InputPosition,
        }

    }
}

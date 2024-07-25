
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class SetAnimator : ActionNode
    {
        public string parameterID;
        public Parameter parameterType = Parameter.Trigger;

        public enum Parameter
        {
            Trigger,
            Bool,
            Float,
        }

#if ODIN_INSPECTOR
        [EnableIf("parameterType", Parameter.Bool)] 
#endif
        public bool boolValue;
#if ODIN_INSPECTOR
        [EnableIf("parameterType", Parameter.Float)] 
#endif
        public float floatValue;

    }
}

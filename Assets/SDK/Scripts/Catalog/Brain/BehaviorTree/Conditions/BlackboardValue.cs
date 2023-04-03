
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Condition
{
	public class BlackboardValue : ConditionNode
    {
        public string variableName;
        public VariableType variableType;
#if ODIN_INSPECTOR
        [EnableIf("variableType", optionalValue: VariableType.Float)]
#endif
        public Comparator variableComparator;
        public float comparedValue;

        public enum Comparator
        {
            Equal,
            GreaterThan,
            GreaterThanOrEqual,
            LowerThan,
            LowerThanOrEqual,
        }

        public enum VariableType
        {
            Bool,
            Float,
        }

    }
}
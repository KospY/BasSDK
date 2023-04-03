using UnityEngine;

namespace ThunderRoad.AI.Condition
{
    public class BlackboardTimeValue : ConditionNode
    {
        public string variableName;
        public BlackboardValue.Comparator variableComparator;
        public float timeDiff;

    }
}
using System.Collections;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
{
    public class PauseIf : ActionNode
    {
#if ODIN_INSPECTOR
        [PropertyOrder(0)]
#endif
        public Operation operation = Operation.AND;

        public enum Operation
        {
            AND,
            OR,
        }

#if ODIN_INSPECTOR
        [ListDrawerSettings(Expanded = true), PropertyOrder(0)]
#endif
        public List<ConditionNode> ifConditions = new List<ConditionNode>();
#if ODIN_INSPECTOR
        [ListDrawerSettings(Expanded = true), PropertyOrder(0)]
#endif
        public List<ConditionNode> ifNotConditions = new List<ConditionNode>();

    }
}

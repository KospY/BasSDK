using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Condition
{
    public class AnimatorValue : ConditionNode
    {
        public string parameterName;
        public AnimatorControllerParameterType parameterType = AnimatorControllerParameterType.Bool;
#if ODIN_INSPECTOR
        [HideIf("parameterType", optionalValue: AnimatorControllerParameterType.Bool)]
#endif
        public Comparator variableComparator;
#if ODIN_INSPECTOR
        [HideIf("parameterType", optionalValue: AnimatorControllerParameterType.Bool)]
#endif
        public float comparedValue;

        public enum Comparator
        {
            Equal,
            GreaterThan,
            GreaterThanOrEqual,
            LowerThan,
            LowerThanOrEqual,
        }

    }
}
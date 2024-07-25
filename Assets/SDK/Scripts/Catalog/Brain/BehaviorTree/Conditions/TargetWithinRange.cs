using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Condition
{
	public class TargetWithinRange : ConditionNode
    {
#if ODIN_INSPECTOR
        [BoxGroup("Inputs")] 
#endif
        public bool flatDistance = false;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), LabelWidth(100)] 
#endif
        public Target target = Target.CurrentTarget;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), ShowIf("target", optionalValue: Target.InputTransform), LabelWidth(200)] 
#endif
        public string inputTransformVariableName = "";

        public enum Target
        {
            CurrentTarget,
            InputTransform,
            None,
        }

        public enum CompareType
        {
            LessThanOrEqual,
            LessThan,
            CloseTo,
            GreaterThan,
            GreaterOrEqual
        }

        public enum Range
        {
            Fixed,
            MinMeleeRange,
            MaxMeleeRange,
            Blackboard,
        }

#if ODIN_INSPECTOR
        [BoxGroup("Inputs")] 
#endif
        public CompareType comparison;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs")]
#endif
        public Range range = Range.Fixed;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), ShowIf("range", optionalValue: Range.Fixed)]
#endif
        public float distance;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), ShowIf("range", optionalValue: Range.Blackboard)]
#endif
        public string valueName = "TargetWithinRange";
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HideIf("range", optionalValue: Range.Fixed)]
#endif
        public float addedDistance;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), EnableIf("comparison", optionalValue: CompareType.CloseTo)]
#endif
        public float maxDiff;

    }
}

using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Condition
{
    public class TargetHasMoved : ConditionNode
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
#if ODIN_INSPECTOR
        [BoxGroup("Inputs")]
#endif
        public float distance = 3;

    }
}

using UnityEngine;
using UnityEngine.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Get
{
	public class GetRandomPositionAround : ActionNode
    {
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), LabelWidth(100)] 
#endif
        public Target target = Target.CurrentTarget;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), DisableIf("target", optionalValue: Target.CurrentTarget), LabelWidth(200)] 
#endif
        public string inputTargetVariableName = "";
        public bool scaleMinMaxByHeightDiff = false;
        public float maxHeightDiff = 10f;

        public enum Target
        {
            CurrentTarget,
            InputTransform,
            InputCreature,
            InputPosition,
        }

#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), LabelWidth(100)] 
#endif
        public float minRadius = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), LabelWidth(100)] 
#endif
        public float maxRadius = 3;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), LabelWidth(100)] 
#endif
        public int attempts = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Output")] 
#endif
        public string outputPositionVariableName = "RandomPosition";

    }
}

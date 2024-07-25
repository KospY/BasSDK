using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Get
{
	public class GetCreaturePart : ActionNode
    {
        public Target target = Target.CurrentTarget;
#if ODIN_INSPECTOR
        [ShowIf("target", optionalValue: Target.InputCreature)] 
#endif
        public string inputCreatureVariableName = "";

        public enum Target
        {
            CurrentTarget,
            InputCreature,
        }

        public TargetPart targetPart = TargetPart.CreatureRoot;
        public string outputTransformVariableName = "CreatureTransform";

        public enum TargetPart
        {
            CreatureRoot,
            RagdollRoot,
            RagdollTarget,
            RagdollHead,
            RagdollEyes,
            RagdollHandRight,
            RagdollHandLeft,
        }

    }
}

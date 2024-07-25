using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
    public class SetLookAt : ActionNode
    {
        public bool enabled = true;
#if ODIN_INSPECTOR
        [ShowIf("enabled")] 
#endif
        public BrainModuleLookAt.BodyBehaviour bodyBehaviour = BrainModuleLookAt.BodyBehaviour.BodyUpright;
#if ODIN_INSPECTOR
        [ShowIf("enabled")] 
#endif
        public Target target = Target.CurrentTarget;
#if ODIN_INSPECTOR
        [ShowIf("enabled"), HideIf("target", optionalValue: Target.CurrentTarget)] 
#endif
        public string inputVariableName = "LookTarget";
#if ODIN_INSPECTOR
        [ShowIf("enabled")] 
#endif
        public Vector3 localOffset = new Vector3();

        public enum Target
        {
            CurrentTarget,
            InputTransform,
            InputPosition,
        }

    }
}

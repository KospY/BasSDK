using UnityEngine;

namespace ThunderRoad.AI.Action
{
    public class SaveFacingAngleToBlackboard : ActionNode
    {
        public bool flatAngle = true;
        public string inputTransformVariableName = "InputTransform";
        public string outputAngleVariableName = "FacingAngle";

        public override State Evaluate()
        {
            
            return State.SUCCESS;
            
        }
    }
}

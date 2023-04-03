using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
{
	public class PickSpell : ActionNode
    {
        public Side side = Side.Left;
        public SelectionMode selectionMode = SelectionMode.ByDistance;
#if ODIN_INSPECTOR
        [ShowIf("selectionMode", Value = SelectionMode.ByID)] 
#endif
        public string spellID = "";
#if ODIN_INSPECTOR
        [ShowIf("selectionMode", Value = SelectionMode.ByDistance)] 
#endif
        public Target target = Target.CurrentTarget;
#if ODIN_INSPECTOR
        [ShowIf("selectionMode", Value = SelectionMode.ByDistance), DisableIf("target", optionalValue: Target.CurrentTarget)] 
#endif
        public string inputTransformVariableName = "";

        public enum SelectionMode
        {
            ByDistance,
            ByID,
            Random,
        }

        public enum Target
        {
            CurrentTarget,
            InputTransform,
        }

    }
}

using UnityEngine;
using UnityEngine.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Get
{
	public class GetShootPosition : ActionNode
    {
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), LabelWidth(100)] 
#endif
        public Target target = Target.CurrentTarget;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), DisableIf("target", optionalValue: Target.CurrentTarget), HorizontalGroup("Inputs/Horiz"), LabelWidth(200)] 
#endif
        public string inputVariableName = "";

        public enum Target
        {
            CurrentTarget,
            InputCreature,
        }

#if ODIN_INSPECTOR
        [BoxGroup("Selection"), LabelWidth(200)] 
#endif
        public bool checkShortestPath = false;

#if ODIN_INSPECTOR
        [BoxGroup("Output")] 
#endif
        public string outputPositionVariableName = "ShootPosition";

        public enum Sight
        {
            Nothing,
            Creature,
            Target,
        }

    }
}

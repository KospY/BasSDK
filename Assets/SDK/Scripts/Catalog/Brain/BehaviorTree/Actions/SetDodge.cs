#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
{
	public class SetDodge : ActionNode
    {
        public BrainModuleDodge.DodgeBehaviour dodgeBehaviour = BrainModuleDodge.DodgeBehaviour.None;

#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), LabelWidth(100), HideIf("dodgeBehaviour", BrainModuleDodge.DodgeBehaviour.None)] 
#endif
        public Target target = Target.CurrentTarget;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), DisableIf("target", optionalValue: Target.CurrentTarget), HorizontalGroup("Inputs/Horiz"), LabelWidth(200), HideIf("dodgeBehaviour", BrainModuleDodge.DodgeBehaviour.None)] 
#endif
        public string inputCreatureVariableName = "";

        public enum Target
        {
            CurrentTarget,
            InputCreature,
        }

    }
}

#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class SetDefenseState : ActionNode
    {
        public bool parry;
        public bool dodge;

        /*[BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), LabelWidth(100)]
        public Target target = Target.CurrentTarget;
        [BoxGroup("Inputs"), DisableIf("target", optionalValue: Target.CurrentTarget), HorizontalGroup("Inputs/Horiz"), LabelWidth(200)]
        public string inputCreatureVariableName = "";

        public enum Target
        {
            CurrentTarget,
            InputCreature,
        }*/


    }
}

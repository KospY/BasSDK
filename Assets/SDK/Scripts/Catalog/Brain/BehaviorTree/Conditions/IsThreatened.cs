using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.AI.Condition
{
	public class IsThreatened : ConditionNode
    {
        public Target target = Target.CurrentTarget;
#if ODIN_INSPECTOR
        [ShowIf("target", Target.InputCreature)]
#endif
        public string inputCreatureVariableName = "";

        public enum Target
        {
            CurrentTarget,
            InputCreature,
        }

        public float threatMinSpeed = 2f;
        public float threatDuration = 2f;

    }
}
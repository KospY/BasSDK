using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class MoveInDirection : ActionNode
    {
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), LabelWidth(100)] 
#endif
        public Vector2 moveDirection = new Vector2();
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), LabelWidth(100)] 
#endif
        public float moveSpeedRatio = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), LabelWidth(100)] 
#endif
        public float turnSpeed = 0f;
#if ODIN_INSPECTOR
        [BoxGroup("Inputs"), HorizontalGroup("Inputs/Horiz"), LabelWidth(100)] 
#endif
        public float moveMinDuration = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("StuckDetection"), HorizontalGroup("StuckDetection/Horiz"), LabelWidth(100)] 
#endif
        public float stuckRadius = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("StuckDetection"), HorizontalGroup("StuckDetection/Horiz"), LabelWidth(150)] 
#endif
        public float stuckMaxDuration = 5f;

    }
}

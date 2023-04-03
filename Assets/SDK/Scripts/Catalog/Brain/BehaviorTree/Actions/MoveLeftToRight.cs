using UnityEngine;
using System.Collections;

#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
{
	public class MoveLeftToRight : ActionNode
    {
        public bool startActionOnDisarm = true;
        public Vector2 disarmedActionStartDelay = new Vector2(0.1f, 0.25f);
    }
}

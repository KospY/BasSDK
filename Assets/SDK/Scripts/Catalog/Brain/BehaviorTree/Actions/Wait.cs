using UnityEngine;

#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
{
	public class Wait : ActionNode
    {
        public bool startWaiting = true;
        public float minDuration = 1;
        public float maxDuration = 5;

    }
}

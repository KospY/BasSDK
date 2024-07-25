using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class AttackBow : AttackRanged
    {
#if ODIN_INSPECTOR
        [BoxGroup("Overrides")] 
#endif
        public bool overrideMaxShootAngle = false;
#if ODIN_INSPECTOR
        [BoxGroup("Overrides"), EnableIf("overrideMaxShootAngle")] 
#endif
        public float maxShootAngleOverride = 0f;

    }
}

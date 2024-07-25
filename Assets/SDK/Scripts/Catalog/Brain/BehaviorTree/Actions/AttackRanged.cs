using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public abstract class AttackRanged : ActionNode
    {
#if ODIN_INSPECTOR
        [BoxGroup("Inputs")] 
#endif
        public string inputTransformVariableName = "";
#if ODIN_INSPECTOR
        [BoxGroup("Inputs")] 
#endif
        public bool useCurrentTargetIfNullTransform = true;
#if ODIN_INSPECTOR
        [BoxGroup("Overrides")] 
#endif
        public bool overrideSpread = false;
#if ODIN_INSPECTOR
        [BoxGroup("Overrides"), EnableIf("overrideSpread")] 
#endif
        public Vector2 spreadOverride = Vector2.zero;
#if ODIN_INSPECTOR
        [BoxGroup("Overrides")] 
#endif
        public bool overrideWeaponRotationX = false;
#if ODIN_INSPECTOR
        [BoxGroup("Overrides")] 
#endif
        public bool overrideWeaponRotationY = false;
#if ODIN_INSPECTOR
        [BoxGroup("Overrides")] 
#endif
        public bool overrideWeaponRotationZ = false;
#if ODIN_INSPECTOR
        [BoxGroup("Overrides")] 
#endif
        public Vector3 weaponRotationOverride = Vector3.zero;

    }
}

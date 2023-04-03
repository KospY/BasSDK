using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Action
{
	public class BlackboardFloatAdd : ActionNode
    {
        public struct Additive
        {
            public bool useOtherBlackboardValue;
#if ODIN_INSPECTOR
            [HideIf("useOtherBlackboardValue")] 
#endif
            public float value;
#if ODIN_INSPECTOR
            [ShowIf("useOtherBlackboardValue")] 
#endif
            public string otherValueName;
        }

        public string blackboardValue = "";
        public bool scaleWithCycleSpeed = true;
        public List<Additive> toAdd = new List<Additive>();

#if ODIN_INSPECTOR
        [HideLabel, HorizontalGroup("Horiz", MinWidth = 10)] 
#endif
        public bool clampLower = false;
#if ODIN_INSPECTOR
        [EnableIf("clampLower"), HorizontalGroup("Horiz", MinWidth = 100), LabelWidth(60)] 
#endif
        public float minValue = 0f;
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly, HorizontalGroup("Horiz", MinWidth = 100), LabelWidth(60)] 
#endif
        protected float value = 0f;
#if ODIN_INSPECTOR
        [HideLabel, HorizontalGroup("Horiz", MinWidth = 10)] 
#endif
        public bool clampUpper = false;
#if ODIN_INSPECTOR
        [EnableIf("clampUpper"), HorizontalGroup("Horiz", MinWidth = 100), LabelWidth(60)] 
#endif
        public float maxValue = 10f;

    }
}

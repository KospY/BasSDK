using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Condition
{
	public class IsTKGrabbed : ConditionNode
    {
#if ODIN_INSPECTOR
        [BoxGroup("Options"), HorizontalGroup("Options/Horiz"), LabelWidth(200)] 
#endif
        public bool anyHandle = true;
#if ODIN_INSPECTOR
        [BoxGroup("Options"), HorizontalGroup("Options/Horiz2"), LabelWidth(200), DisableIf("anyHandle", optionalValue: true)] 
#endif
        public TKOptions handleSpecifiers = TKOptions.SpecificHandle;
#if ODIN_INSPECTOR
        [BoxGroup("Options"), HorizontalGroup("Options/Horiz3"), LabelWidth(200), DisableIf("anyHandle", optionalValue: true), EnableIf("handleSpecifiers", optionalValue: TKOptions.SpecificHandle)] 
#endif
        public string handleName = "None";

        [Flags]
        public enum TKOptions
        {
            SpecificHandle = 0,
            CarryHandle = 1,
            ChokeHandle = 2
        }

    }
}
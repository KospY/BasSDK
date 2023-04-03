using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad.AI.Condition
{
	public class IsGrabbed : ConditionNode
    {
        [Flags]
        public enum HandleOptions
        {
            SpecificHandle = 0,
            CarryHandle = 1,
            MuffleHandle = 2,
            ChokeHandle = 4
        }

        public enum FromDirection
        {
            Either,
            FrontOnly,
            BackOnly
        }

#if ODIN_INSPECTOR
        [BoxGroup("Options"), HorizontalGroup("Options/Horiz"), LabelWidth(200)] 
#endif
        public bool anyHandle = true;
#if ODIN_INSPECTOR
        [BoxGroup("Options"), HorizontalGroup("Options/Horiz2"), LabelWidth(200), DisableIf("anyHandle", optionalValue: true)] 
#endif
        public HandleOptions handleSpecifiers = HandleOptions.SpecificHandle;
#if ODIN_INSPECTOR
        [BoxGroup("Options"), HorizontalGroup("Options/Horiz3"), LabelWidth(200), DisableIf("anyHandle", optionalValue: true), EnableIf("handleSpecifiers", optionalValue: HandleOptions.SpecificHandle)] 
#endif
        public string handleName = "None";
#if ODIN_INSPECTOR
        [BoxGroup("Options"), HorizontalGroup("Options/Horiz4"), LabelWidth(200)] 
#endif
        public FromDirection fromDirection = FromDirection.Either;

    }
}
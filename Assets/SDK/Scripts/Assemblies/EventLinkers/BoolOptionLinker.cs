using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BoolOptionLinker : ModOptionLinker<bool, BoolChecker>
    {
        public override void UnsubscribeNamedMethods() { }
    }

    [System.Serializable]
    public class BoolChecker : UpdateChecker<bool>
    {
        public override bool Check(bool value)
        {
            switch (comparison)
            {
                case Comparison.NotEquals:
                    return value != comparedValue;
                case Comparison.Lesser:
                    PrintMessage($"Can't use a lesser comparison on two bools! Check your Bool Option Linkers to confirm they aren't set incorrectly!", true);
                    return !value && comparedValue;
                case Comparison.LesserOrEqual:
                    PrintMessage($"Can't use a lesser or equal comparison on two bools! Check your Bool Option Linkers to confirm they aren't set incorrectly!", true);
                    return value == comparedValue || (!value && comparedValue);
                case Comparison.Equals:
                    return value == comparedValue;
                case Comparison.GreaterOrEqual:
                    PrintMessage($"Can't use a greater or equal comparison on two bools! Check your Bool Option Linkers to confirm they aren't set incorrectly!", true);
                    return value == comparedValue || (!comparedValue && value);
                case Comparison.Greater:
                    PrintMessage($"Can't use a greater comparison on two bools! Check your Bool Option Linkers to confirm they aren't set incorrectly!", true);
                    return !comparedValue && value;
            }
            return false;
        }
    }
}

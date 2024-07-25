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
    public class IntOptionLinker : ModOptionLinker<int, IntChecker>
    {
        public override void UnsubscribeNamedMethods() { }
    }

    [System.Serializable]
    public class IntChecker : UpdateChecker<int>
    {
        public override bool Check(int value)
        {
            switch (comparison)
            {
                case Comparison.NotEquals:
                    return value != comparedValue;
                case Comparison.Lesser:
                    return value < comparedValue;
                case Comparison.LesserOrEqual:
                    return value <= comparedValue;
                case Comparison.Equals:
                    return value == comparedValue;
                case Comparison.GreaterOrEqual:
                    return value >= comparedValue;
                case Comparison.Greater:
                    return value > comparedValue;
            }
            return false;
        }
    }
}

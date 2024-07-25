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
    public class FloatOptionLinker : ModOptionLinker<float, FloatChecker>
    {
        public override void UnsubscribeNamedMethods() { }
    }

    [System.Serializable]
    public class FloatChecker : UpdateChecker<float>
    {
        public override bool Check(float value)
        {
            switch (comparison)
            {
                case Comparison.NotEquals:
                    return !value.IsApproximately(comparedValue);
                case Comparison.Lesser:
                    return value < comparedValue;
                case Comparison.LesserOrEqual:
                    return value <= comparedValue;
                case Comparison.Equals:
                    return value.IsApproximately(comparedValue);
                case Comparison.GreaterOrEqual:
                    return value >= comparedValue;
                case Comparison.Greater:
                    return value > comparedValue;
            }
            return false;
        }
    }
}

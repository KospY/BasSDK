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
    public class StringOptionLinker : ModOptionLinker<string, StringChecker>
    {
        public override void UnsubscribeNamedMethods() { }
    }

    [System.Serializable]
    public class StringChecker : UpdateChecker<string>
    {
        public override bool Check(string value)
        {
            switch (comparison)
            {
                case Comparison.NotEquals:
                    return value != comparedValue;
                case Comparison.Lesser:
                    PrintMessage("A string option linker is using a lesser than comparison for strings! When using this comparison, the return is a comparison of the string lengths", false);
                    return value.Length < comparedValue.Length;
                case Comparison.LesserOrEqual:
                    PrintMessage("A string option linker is using a lesser than or equal comparison for strings! When using this comparison, the return is a comparison of the string lengths", false);
                    return value.Length <= comparedValue.Length;
                case Comparison.Equals:
                    return value == comparedValue;
                case Comparison.GreaterOrEqual:
                    PrintMessage("A string option linker is using a greater than or equal comparison for strings! When using this comparison, the return is a comparison of the string lengths", false);
                    return value.Length >= comparedValue.Length;
                case Comparison.Greater:
                    PrintMessage("A string option linker is using a greater than comparison for strings! When using this comparison, the return is a comparison of the string lengths", false);
                    return value.Length > comparedValue.Length;
            }
            return false;
        }
    }
}

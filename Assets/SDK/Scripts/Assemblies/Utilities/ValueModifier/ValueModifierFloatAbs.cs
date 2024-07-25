using UnityEngine;

namespace ThunderRoad.Utilities
{
    public class ValueModifierFloatAbs : ValueModifierBase
    {
        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Float;
        
        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Float;
        
        public override string description => "Return the absolute value of the input.";

        public override object ProcessFloat(float chainedValue)
        {
            return Mathf.Abs(chainedValue);
        }
    }
}
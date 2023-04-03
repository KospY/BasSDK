using System;

namespace ThunderRoad.Utilities
{
    public class ValueModifierFloatEquals : ValueModifierBase
    {
        public float target;
        public float tolerance = 0.001f;
        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Float;

        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Boolean;

        public override string description => "True if the input is equal to the target using a tolerance check.";
        
        public override object ProcessFloat(float chainedValue)
        {
            return Math.Abs(chainedValue - target) < tolerance;
        }
    }
}
using UnityEngine;

namespace ThunderRoad.Utilities
{
    public class ValueModifierFloatCurve : ValueModifierBase
    {
        public AnimationCurve curve = AnimationCurve.Constant(0, 1, 1);

        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Float;
        
        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Float;

        public override string description => "Sample the input with the given curve.";
        
        public override object ProcessFloat(float chainedValue)
        {
            return curve.Evaluate(chainedValue);
        }
    }
}
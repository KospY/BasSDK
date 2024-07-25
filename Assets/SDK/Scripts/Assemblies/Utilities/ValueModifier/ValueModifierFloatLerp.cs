using UnityEngine;

namespace ThunderRoad.Utilities
{
    public class ValueModifierFloatLerp : ValueModifierBase
    {
        public float a;
        public float b = 1f;

        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Float;
        
        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Float;

        public override string description =>
            "Linear interpolation between [a; b] sampled with the input as the time parameter";

        public override object ProcessFloat(float chainedValue)
        {
            return Mathf.Lerp(a, b, chainedValue);
        }
    }
}
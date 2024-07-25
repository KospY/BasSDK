using UnityEngine;

namespace ThunderRoad.Utilities
{
    public class ValueModifierFloatClamp01 : ValueModifierBase
    {
        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Float;
        
        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Float;
        
        public override string description => "Clamps the input between [0; 1].";

        public override object ProcessFloat(float chainedValue)
        {
            return Mathf.Clamp01(chainedValue);
        }
    }
}
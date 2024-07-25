using UnityEngine;

namespace ThunderRoad.Utilities
{
    public class ValueModifierVector2Normalize : ValueModifierBase
    {
        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Vector2;
        
        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Vector2;

        public override string description => "Return the normalized vector of the input.";

        public override object ProcessVector2(Vector2 chainedValue)
        {
            return chainedValue.normalized;
        }
    }
}
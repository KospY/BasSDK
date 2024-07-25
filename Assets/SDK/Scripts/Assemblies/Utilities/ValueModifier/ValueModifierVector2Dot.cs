using UnityEngine;

namespace ThunderRoad.Utilities
{
    public class ValueModifierVector2Dot : ValueModifierBase
    {
        public Vector2 target;
        
        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Vector2;
        
        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Float;

        public override string description => "Return the dot product with the target.";

        public override object ProcessVector2(Vector2 chainedValue)
        {
            return Vector2.Dot(chainedValue, target);
        }
    }
}
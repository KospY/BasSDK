using UnityEngine;

namespace ThunderRoad.Utilities
{
    public class ValueModifierVector3Dot : ValueModifierBase
    {
        public Vector3 target;
        
        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Vector3;
        
        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Float;

        public override string description => "Return the dot product with the target.";

        public override object ProcessVector3(Vector3 chainedValue)
        {
            return Vector3.Dot(chainedValue, target);
        }
    }
}
using UnityEngine;

namespace ThunderRoad.Utilities
{
    public class ValueModifierVector3Normalize : ValueModifierBase
    {
        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Vector3;
            
        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Vector3;
        
        public override string description => "Return the normalized vector of the input.";

        public override object ProcessVector3(Vector3 chainedValue)
        {
            return chainedValue.normalized;
        }
    }
}
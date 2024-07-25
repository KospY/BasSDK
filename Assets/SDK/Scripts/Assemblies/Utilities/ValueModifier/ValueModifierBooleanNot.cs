using UnityEngine;

namespace ThunderRoad.Utilities
{
    public class ValueModifierBooleanNot : ValueModifierBase
    {
        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Boolean;
        
        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Boolean;

        public override string description => "Inverts the input.";
        
        public override object ProcessBoolean(bool chainedValue)
        {
            return !chainedValue;
        }
    }
}
using UnityEngine;

namespace ThunderRoad.Utilities
{
    public class ValueModifierFloatDampen : ValueModifierBase
    {
        public float minValue = 5;
        public float maxValue = 12;
        public float dampening = .1f;

        private float dampenedValue;

        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Float;

        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Float;

        public override string description =>
            "Dampen the given value between [minValue; maxValue] with a dampening factor. Return a [0; 1] value.";

        public override object ProcessFloat(float chainedValue)
        {
            dampenedValue = Mathf.Lerp(dampenedValue, Mathf.InverseLerp(minValue, maxValue, chainedValue), dampening);
            return dampenedValue;
        }
    }
}
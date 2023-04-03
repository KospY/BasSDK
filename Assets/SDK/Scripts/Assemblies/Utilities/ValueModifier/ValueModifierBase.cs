using System;
using UnityEngine;

namespace ThunderRoad.Utilities
{
    [ExecuteInEditMode, RequireComponent(typeof(ValueModifierSequence))]
    public abstract class ValueModifierBase : MonoBehaviour, IComparable<ValueModifierBase>
    {
        [NonSerialized] public ValueModifierSequence sequence;

        public virtual ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Float;
        
        public virtual ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Float;

        public virtual string description => "";

        public string MenuName => GetType().Name.Remove(0, 13 + inputType.ToString().Length);
        public string MenuPath => $"{inputType} → {outputType}/{MenuName}";
        
        [HideInInspector] public int order = -1;

        protected virtual void OnEnable()
        {
            hideFlags = HideFlags.HideInInspector;
            
            if (!sequence) return;

            if (!sequence.modifiers.Contains(this))
                sequence.modifiers.Add(this);
        }

        protected virtual void OnDisable()
        {
            if (!sequence) return;

            sequence.modifiers.Remove(this);
        }

        public object ProcessValue(object value)
        {
            switch (value)
            {
                case float f:
                    return ProcessFloat(f);
                case int i:
                    return ProcessInteger(i);
                case bool b:
                    return ProcessBoolean(b);
                case Vector2 v2:
                    return ProcessVector2(v2);
                case Vector3 v3:
                    return ProcessVector3(v3);
                case Color c:
                    return ProcessColor(c);
            }

            return value;
        }

        public virtual object ProcessFloat(float chainedValue)
        {
            return chainedValue;
        }
        
        public virtual object ProcessInteger(int chainedValue)
        {
            return chainedValue;
        }

        public virtual object ProcessBoolean(bool chainedValue)
        {
            return chainedValue;
        }

        public virtual object ProcessVector2(Vector2 chainedValue)
        {
            return chainedValue;
        }

        public virtual object ProcessVector3(Vector3 chainedValue)
        {
            return chainedValue;
        }

        public virtual object ProcessColor(Color chainedValue)
        {
            return chainedValue;
        }

        public int CompareTo(ValueModifierBase other)
        {
            return order - other.order;
        }

        /// <summary>
        /// Process the value from the given sequence-output.
        /// </summary>
        /// <param name="sequenceOutput">Sequence-output to read the value from.</param>
        /// <returns>The sequence-output after setting its value to the processed one</returns>
        public ValueModifierSequence.ValueModifierSequenceOutput ProcessValue(ValueModifierSequence.ValueModifierSequenceOutput sequenceOutput)
        {
            return sequenceOutput.SetValue(ProcessValue(sequenceOutput.GetValue(inputType)));
        }
    }

    public static class ValueModifier
    {
        public static bool HasFlagNoGC(this ValueModifierSequence.ValueModifierType flags, ValueModifierSequence.ValueModifierType value)
        {
            return ((flags & value) > 0);
        }
    }
}
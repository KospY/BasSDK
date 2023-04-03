using System;
using UnityEngine;

namespace ThunderRoad.Utilities
{
    public class ValueModifierFloatSampleAverage : ValueModifierBase
    {
        [Min(1)] public int sampleCount = 1;
        public float initialValues;
        
        private float[] sampleBuffer;
        private float sum;
        private int sampleIndex;

        public override ValueModifierSequence.ValueModifierType inputType =>
            ValueModifierSequence.ValueModifierType.Float;

        public override ValueModifierSequence.ValueModifierType outputType =>
            ValueModifierSequence.ValueModifierType.Float;

        public override string description =>
            "Each time an input is sent, it gets stored in a buffer of size \"sample count\".\nThe output value is an average of all the values stored in that buffer";

        private void Awake()
        {
            sampleBuffer ??= new float[sampleCount];
            sampleIndex = 0;
            
            ResetBuffer(initialValues);
        }

        public override object ProcessFloat(float chainedValue)
        {
            Push(chainedValue);
            return GetSmoothedValue();
        }

        public void Push(float value)
        {
            if(sampleBuffer == null) ResetBuffer(initialValues);

            // Inspector value has changed, we resize the buffer to match it
            if (sampleBuffer.Length != sampleCount)
            {
                Array.Resize(ref sampleBuffer, sampleCount);
                sampleIndex %= sampleBuffer.Length; // avoid overflow
            }

            sum -= sampleBuffer[sampleIndex];
            sum += value;
            sampleBuffer[sampleIndex] = value;

            // Advance the index in a circular way
            sampleIndex  = (sampleIndex + 1) % sampleBuffer.Length;;
        }

        public float GetSmoothedValue()
        {
            return sum / sampleBuffer.Length;
        }

        public void ResetBuffer(float value)
        {
            sum = value * sampleBuffer.Length;

            for (int i = 0; i < sampleBuffer.Length; ++i)
                sampleBuffer[i] = value;
        }
    }
}
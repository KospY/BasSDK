using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad.Utilities
{
    public class ValueModifierSequence : MonoBehaviour
    {
        [Flags]
        public enum ValueModifierType
        {
            Undefined = 0,
            Float = 1 << 1,
            Integer = 1 << 2,
            Boolean = 1 << 3,
            Vector2 = 1 << 4,
            Vector3 = 1 << 5,
            Color = 1 << 6,
        }

        /// <summary>
        /// Class used to hold the different values from the sequence, to allow multiple type branching in parallel
        /// </summary>
        [Serializable]
        public class ValueModifierSequenceOutput
        {
            // Output values
            public float floatOutput;
            public int integerOutput;
            public bool booleanOutput;
            public Vector2 vector2Output;
            public Vector3 vector3Output;
            public Color colorOutput;

            // Boolean flags to know which values are used
            public bool isSetFloat;
            public bool isSetInteger;
            public bool isSetBoolean;
            public bool isSetVector2;
            public bool isSetVector3;
            public bool isSetColor;

            /// <summary>
            /// History stack to debug
            /// </summary>
            public List<object> valueHistory = new List<object>();

            /// <summary>
            /// Current used output types (flags)
            /// </summary>
            public ValueModifierType outputTypes;

            public ValueModifierSequenceOutput SetValue(object value)
            {
                valueHistory.Add(value);
                
                switch (value)
                {
                    case float f:
                        SetValue(f);
                        break;
                    case int i:
                        SetValue(i);
                        break;
                    case bool b:
                        SetValue(b);
                        break;
                    case Vector2 v2:
                        SetValue(v2);
                        break;
                    case Vector3 v3:
                        SetValue(v3);
                        break;
                    case Color c:
                        SetValue(c);
                        break;
                }

                return this;
            }

            public void SetValue(float value)
            {
                floatOutput = value;
                isSetFloat = true;

                outputTypes |= ValueModifierType.Float;
            }

            public void SetValue(int value)
            {
                integerOutput = value;
                isSetInteger = true;

                outputTypes |= ValueModifierType.Integer;
            }

            public void SetValue(bool value)
            {
                booleanOutput = value;
                isSetBoolean = true;

                outputTypes |= ValueModifierType.Boolean;
            }

            public void SetValue(Vector2 value)
            {
                vector2Output = value;
                isSetVector2 = true;

                outputTypes |= ValueModifierType.Vector2;
            }

            public void SetValue(Vector3 value)
            {
                vector3Output = value;
                isSetVector3 = true;

                outputTypes |= ValueModifierType.Vector3;
            }

            public void SetValue(Color value)
            {
                colorOutput = value;
                isSetColor = true;

                outputTypes |= ValueModifierType.Color;
            }

            public object GetValue(ValueModifierType outputType)
            {
                if(outputType.HasFlagNoGC(ValueModifierType.Float))
                    return floatOutput;
                
                if(outputType.HasFlagNoGC(ValueModifierType.Integer))
                    return integerOutput;
                
                if(outputType.HasFlagNoGC(ValueModifierType.Boolean))
                    return booleanOutput;
                
                if(outputType.HasFlagNoGC(ValueModifierType.Vector2))
                    return vector2Output;
                
                if(outputType.HasFlagNoGC(ValueModifierType.Vector3))
                    return vector3Output;
                
                if(outputType.HasFlagNoGC(ValueModifierType.Color))
                    return colorOutput;
                
                return null;
            }

            public override string ToString()
            {
                var str = new StringBuilder();

                if (isSetFloat)
                    str.Append($"Float → {floatOutput}\n");

                if (isSetInteger)
                    str.Append($"Integer → {integerOutput}\n");

                if (isSetBoolean)
                    str.Append($"Boolean → {booleanOutput}\n");

                if (isSetVector2)
                    str.Append($"Vector2 → {vector2Output}\n");

                if (isSetVector3)
                    str.Append($"Vector3 → {vector3Output}\n");

                if (isSetColor)
                    str.Append($"Color → {colorOutput}\n");

                return str.ToString();
            }
        }

        [Header("Detected Modules")] public List<ValueModifierBase> modifiers;

        [Header("Float")] public UnityEvent<float> onProcessFloat;
        [Header("Integer")] public UnityEvent<int> onProcessInteger;
        [Header("Boolean")] public UnityEvent<bool> onProcessBoolean;
        [Header("Vector2")] public UnityEvent<Vector2> onProcessVector2;
        [Header("Vector3")] public UnityEvent<Vector3> onProcessVector3;
        [Header("Color")] public UnityEvent<Color> onProcessColor;

        private bool initialized;

        [NonSerialized] public ValueModifierSequenceOutput lastOutput;

#if UNITY_EDITOR
        private void OnValidate()
        {
            Refresh();
        }
#endif

        /// <summary>
        /// Retrieves the modifiers and order them from their "order" serialized field
        /// </summary>
        public void RetrieveModifiers()
        {
            modifiers = new List<ValueModifierBase>(GetComponentsInChildren<ValueModifierBase>());
            modifiers.Sort((a, b) => a.CompareTo(b));

            for (var i = 0; i < modifiers.Count; i++)
            {
                var modifier = modifiers[i];
                modifier.sequence = this;
                modifier.order = i;
            }
        }

        public void Refresh()
        {
            RetrieveModifiers();
        }

        private void Awake()
        {
            Refresh();
        }

        public void ProcessValue(float value)
        {
            RunSequence(value);
        }

        public void ProcessValue(int value)
        {
            RunSequence(value);
        }

        public void ProcessValue(bool value)
        {
            RunSequence(value);
        }

        public void ProcessValue(Vector2 value)
        {
            RunSequence(value);
        }

        public void ProcessValue(Vector3 value)
        {
            RunSequence(value);
        }

        public void ProcessValue(Color value)
        {
            RunSequence(value);
        }

        /// <summary>
        /// Runs the sequence from input value through all modifiers
        /// </summary>
        /// <param name="value"></param>
        private void RunSequence(object value)
        {
            var sequenceOutput = new ValueModifierSequenceOutput();
            sequenceOutput.SetValue(value);
            for (int i = 0; i < modifiers.Count; i++)
            {
                var modifier = modifiers[i];
                if (sequenceOutput.outputTypes.HasFlagNoGC(modifier.inputType))
                    modifier.ProcessValue(sequenceOutput);
            }

            ProcessOutput(sequenceOutput);
            lastOutput = sequenceOutput;

        }

        /// <summary>
        /// Invoke the different events from the sequence outputs
        /// </summary>
        /// <param name="sequenceOutput">Output from the previously ran sequence</param>
        private void ProcessOutput(ValueModifierSequenceOutput sequenceOutput)
        {
            if (sequenceOutput.isSetFloat)
                onProcessFloat?.Invoke(sequenceOutput.floatOutput);

            if (sequenceOutput.isSetInteger)
                onProcessInteger?.Invoke(sequenceOutput.integerOutput);

            if (sequenceOutput.isSetBoolean)
                onProcessBoolean?.Invoke(sequenceOutput.booleanOutput);

            if (sequenceOutput.isSetVector2)
                onProcessVector2?.Invoke(sequenceOutput.vector2Output);

            if (sequenceOutput.isSetVector3)
                onProcessVector3?.Invoke(sequenceOutput.vector3Output);

            if (sequenceOutput.isSetColor)
                onProcessColor?.Invoke(sequenceOutput.colorOutput);
        }
    }
}
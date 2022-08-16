using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class BlendValues : MonoBehaviour
    {
        public BlendMode blendMode = BlendMode.Average;
        [Range(2, 5)]
        public int valueCount = 2;
        public UnityEvent<float> output = new UnityEvent<float>();

        protected float value1;
        protected float value2;
        protected float value3;
        protected float value4;
        protected float value5;

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        protected float outputValue;

        protected bool changeDetected;

        public void SetValue1(float value)
        {
            value1 = value;
            changeDetected = true;
        }

        public void SetValue2(float value)
        {
            value2 = value;
            changeDetected = true;
        }

        public void SetValue3(float value)
        {
            value3 = value;
            changeDetected = true;
        }

        public void SetValue4(float value)
        {
            value4 = value;
            changeDetected = true;
        }

        public void SetValue5(float value)
        {
            value5 = value;
            changeDetected = true;
        }

        private void LateUpdate()
        {
            if (changeDetected)
            {
                Refresh();
                changeDetected = false;
            }
        }

        public void Refresh()
        {
            if (blendMode == BlendMode.Min)
            {
                if (valueCount == 2)
                {
                    outputValue = Mathf.Min(value1, value2);
                }
                else if (valueCount == 3)
                {
                    outputValue = Mathf.Min(value1, value2, value3);
                }
                else if (valueCount == 4)
                {
                    outputValue = Mathf.Min(value1, value2, value3, value4);
                }
                else if (valueCount == 5)
                {
                    outputValue = Mathf.Min(value1, value2, value3, value4, value5);
                }
            }
            else if (blendMode == BlendMode.Max)
            {
                if (valueCount == 2)
                {
                    outputValue = Mathf.Max(value1, value2);
                }
                else if (valueCount == 3)
                {
                    outputValue = Mathf.Max(value1, value2, value3);
                }
                else if (valueCount == 4)
                {
                    outputValue = Mathf.Max(value1, value2, value3, value4);
                }
                else if (valueCount == 5)
                {
                    outputValue = Mathf.Max(value1, value2, value3, value4, value5);
                }
            }
            else if (blendMode == BlendMode.Average)
            {
                if (valueCount == 2)
                {
                    outputValue = (value1 + value2) / 2;
                }
                else if (valueCount == 3)
                {
                    outputValue = (value1 + value2 + value3) / 3;
                }
                else if (valueCount == 4)
                {
                    outputValue = (value1 + value2 + value3 + value4) / 4;
                }
                else if (valueCount == 5)
                {
                    outputValue = (value1 + value2 + value3 + value4 + value5) / 5;
                }
            }
            else if (blendMode == BlendMode.Multiply)
            {
                if (valueCount == 2)
                {
                    outputValue = value1 * value2;
                }
                else if (valueCount == 3)
                {
                    outputValue = value1 * value2 * value3;
                }
                else if (valueCount == 4)
                {
                    outputValue = value1 * value2 * value3 * value4;
                }
                else if (valueCount == 5)
                {
                    outputValue = value1 * value2 * value3 * value4 * value5;
                }
            }
            output?.Invoke(outputValue);
        }
    }
}

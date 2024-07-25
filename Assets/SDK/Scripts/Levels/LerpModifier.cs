using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    public class LerpModifier : MonoBehaviour
    {
        public float inputMin = 0;
        public float inputMax = 1;

        public bool invertOutputRatio;

        public float outputMin = 0;
        public float outputMax = 1;

        public UnityEvent<float> output = new UnityEvent<float>();

        public void SetValue(float value)
        {
            float newValue = Mathf.Lerp(outputMin, outputMax, Mathf.InverseLerp(inputMin, inputMax, value));
            if (invertOutputRatio) newValue = (1 - newValue);
            output?.Invoke(newValue);
        }
    }
}

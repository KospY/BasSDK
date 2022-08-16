using UnityEngine;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class FxBlendCurves
    {
        public bool useIntensityCurve;
#if ODIN_INSPECTOR
        [ShowIf("useIntensityCurve")]
#endif
        public AnimationCurve intensityCurve;

        public bool useSpeedCurve;
#if ODIN_INSPECTOR
        [ShowIf("useSpeedCurve")]
#endif
        public AnimationCurve speedCurve;

#if ODIN_INSPECTOR
        [ShowIf("useIntensityCurve"), ShowIf("useSpeedCurve")]
#endif
        public BlendMode blendMode = BlendMode.Min;

        public FxBlendCurves()
        {
            this.intensityCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
            this.speedCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        }

        public FxBlendCurves(AnimationCurve curve)
        {
            this.intensityCurve = curve;
            this.speedCurve = curve;
        }

        public bool IsUsed()
        {
            return (useIntensityCurve || useSpeedCurve);
        }

        public bool TryGetValue(float intensity, float speed, out float value)
        {
            if (useIntensityCurve && useSpeedCurve)
            {
                switch (blendMode)
                {
                    case BlendMode.Min:
                        value = Mathf.Min(intensityCurve.Evaluate(intensity), speedCurve.Evaluate(speed));
                        return true;
                    case BlendMode.Max:
                        value = Mathf.Max(intensityCurve.Evaluate(intensity), speedCurve.Evaluate(speed));
                        return true;
                    case BlendMode.Average:
                        value = Mathf.Lerp(intensityCurve.Evaluate(intensity), speedCurve.Evaluate(speed), 0.5f);
                        return true;
                    case BlendMode.Multiply:
                        value = intensityCurve.Evaluate(intensity) * speedCurve.Evaluate(speed);
                        return true;
                }
            }
            else if (useIntensityCurve)
            {
                value = intensityCurve.Evaluate(intensity);
                return true;
            }
            else if (useSpeedCurve)
            {
                value = speedCurve.Evaluate(speed);
                return true;
            }
            value = 0;
            return false;
        }
    }
}

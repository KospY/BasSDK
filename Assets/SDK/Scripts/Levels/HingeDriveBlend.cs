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
    public class HingeDriveBlend : MonoBehaviour
    {
        public HingeDrive hingeDrive;
        public float referenceAngle;
        public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        public DisableBehavior disableBehavior = DisableBehavior.OutputLastCurveValue;
        public UnityEvent<float> output = new UnityEvent<float>();

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        private float hingeAngleRatio;

        public enum DisableBehavior
        {
            None,
            OutputLastCurveValue,
            OutputFirstCurveValue,
        }

        private void OnDisable()
        {
            if (disableBehavior == DisableBehavior.OutputLastCurveValue)
            {
                output?.Invoke(curve.GetLastValue());
            }
            else if (disableBehavior == DisableBehavior.OutputFirstCurveValue)
            {
                output?.Invoke(curve.GetFirstValue());
            }
        }

    }
}

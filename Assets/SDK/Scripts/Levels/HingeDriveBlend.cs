using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
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

        public enum DisableBehavior
        {
            None,
            OutputLastCurveValue,
            OutputFirstCurveValue,
        }

    }
}

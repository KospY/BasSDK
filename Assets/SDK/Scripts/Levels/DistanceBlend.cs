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
    public class DistanceBlend : MonoBehaviour
    {
        public bool usePlayerHeadAsReference;
        public Transform reference;
        public float radius = 2;
        public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        public DisableBehavior disableBehavior = DisableBehavior.OutputLastCurveValue;
        public UnityEvent<float> output = new UnityEvent<float>();

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


        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
    }
}

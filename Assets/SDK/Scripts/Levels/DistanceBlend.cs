using UnityEngine;
using UnityEngine.Events;

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


        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
    }
}

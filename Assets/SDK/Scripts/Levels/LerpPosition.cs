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
    public class LerpPosition : MonoBehaviour
    {
        public Transform reference;
        public Transform target;
        public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        [Range(0, 1)]
        public float ratio;



        protected Vector3 orgPosition;
        protected Quaternion orgRotation;

        private void OnEnable()
        {
            orgPosition = reference.position;
            orgRotation = reference.rotation;
        }

        public void SetRatio(float ratio)
        {
            this.ratio = ratio;
        }

        private void Update()
        {
            if (reference && target)
            {
                reference.position = Vector3.Lerp(orgPosition, target.transform.position, curve.Evaluate(ratio));
                reference.rotation = Quaternion.Lerp(orgRotation, target.transform.rotation, curve.Evaluate(ratio));
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (reference && target)
            {
                Gizmos.DrawLine(orgPosition, target.transform.position);
            }
        }
    }
}

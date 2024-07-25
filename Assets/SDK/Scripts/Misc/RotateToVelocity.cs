using UnityEngine;

#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
	public class RotateToVelocity : MonoBehaviour
    {
#pragma warning disable 0109 // unity this.rigidbody is obsolete, we can override this
        public new Rigidbody rigidbody;
#pragma warning restore 0109
        public Vector3 defaultDirection = Vector3.up;
        public Vector3 upwards = Vector3.up;
        public float minSpeed = 0f;
        public float maxSpeed = 3f;
        public bool revert = false;

        private void Update()
        {
            Vector3 velocityAtPoint = rigidbody.GetPointVelocity(this.transform.position);
            float velocityRatio = Mathf.InverseLerp(minSpeed, maxSpeed, velocityAtPoint.magnitude);
            this.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(defaultDirection, upwards), Quaternion.LookRotation(revert ? -velocityAtPoint.normalized : velocityAtPoint.normalized, upwards), velocityRatio);
        }
    }
}

using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class VelocityTracker : ThunderBehaviour
    {
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public Vector3 velocity;
        
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public Vector3 angularVelocity;
        private Vector3 lastPos;
        private Vector3 lastRot;
        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update;

        public Vector3 position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Quaternion rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public static implicit operator Transform(VelocityTracker tracker) => tracker.transform;
        
        protected override void ManagedOnEnable()
        {
            base.ManagedOnEnable();
            lastPos = transform.position;
            lastRot = transform.eulerAngles;
        }

        protected internal override void ManagedUpdate()
        {
            base.ManagedUpdate();

            velocity = (transform.position - lastPos) / Time.fixedDeltaTime;
            angularVelocity = (transform.eulerAngles - lastRot) / Time.fixedDeltaTime;

            lastPos = transform.position;
            lastRot = transform.eulerAngles;
        }
    }

    public static class KinematicVelocityTrackerExtensions
    {
        public static VelocityTracker GetVelocityTracker(this Rigidbody rigidbody)
            => rigidbody.gameObject.TryGetOrAddComponent(out VelocityTracker velocityTracker)
                ? velocityTracker
                : null;
    }
}

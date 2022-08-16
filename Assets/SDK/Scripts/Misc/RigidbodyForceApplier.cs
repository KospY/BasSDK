using System;
using UnityEngine;

namespace ThunderRoad
{
    class RigidbodyForceApplier : ThunderBehaviour
    {
        public enum ForceApplyMode
        {
            WorldSpace,
            LocalSpace,
            OffsetSpace,
        }

        public Rigidbody rb;
        [Header("Linear force")]
        public bool linearForceActive;
        public ForceApplyMode linearForceApplyMode;
        public ForceMode linearForceType;
        public Vector3 linearForce;
        [Header("Angular force")]
        public bool angularForceActive;
        public bool localAngularForce = false;
        public ForceMode angularForceType;
        public Vector3 angularForce;

        public void ToggleLinearForce() => linearForceActive = !linearForceActive;

        public void ToggleAngularForce() => angularForceActive = !angularForceActive;

        public void SetLinearForceActive(bool active) => linearForceActive = active;

        public void SetAngularForceActive(bool active) => angularForceActive = active;

        private void OnValidate()
        {
            rb ??= GetComponent<Rigidbody>();
        }

    }
}

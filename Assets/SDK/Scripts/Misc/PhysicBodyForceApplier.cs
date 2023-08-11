﻿using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    class PhysicBodyForceApplier : ThunderBehaviour, IToolControllable
    {
        public enum ForceApplyMode
        {
            WorldSpace,
            LocalSpace,
            OffsetSpace,
        }

        public Rigidbody rb;
        public ArticulationBody ab;

        protected PhysicBody physicBody;

        [Header("Linear force")]
        public bool linearForceActive;
        public float linearForceDuration;
        protected float linearForceRemainingDuration;

        public ForceApplyMode linearForceApplyMode;
        public ForceMode linearForceType;
        public Vector3 linearForce;


        [Header("Angular force")]
        public bool angularForceActive;
        public float angularForceDuration;
        protected float angularForceRemainingDuration;

        public bool localAngularForce = false;
        public ForceMode angularForceType;
        public Vector3 angularForce;

        public void ToggleLinearForce() => linearForceActive = !linearForceActive;

        public void ToggleAngularForce() => angularForceActive = !angularForceActive;

        public void SetLinearForceActive(bool active) => linearForceActive = active;

        public void SetAngularForceActive(bool active) => angularForceActive = active;

        private void OnValidate()
        {
            if (!gameObject.activeInHierarchy) return;
            TryAssignBody();
        }

        private void TryAssignBody()
        {
            rb ??= GetComponent<Rigidbody>();
            ab ??= GetComponent<ArticulationBody>();
        }

        [Button]
        public void StartAngularForce()
        {
            angularForceRemainingDuration = angularForceDuration;
        }

        [Button]
        public void StartLinearForce()
        {
            linearForceRemainingDuration = linearForceDuration;
        }

        public void CopyFrom(IToolControllable original)
        {
            var originalApplier = original as PhysicBodyForceApplier;
            linearForceActive = originalApplier.linearForceActive;
            linearForceDuration = originalApplier.linearForceDuration;
            linearForceApplyMode = originalApplier.linearForceApplyMode;
            linearForceType = originalApplier.linearForceType;
            linearForce = originalApplier.linearForce;
            angularForceActive = originalApplier.angularForceActive;
            angularForceDuration = originalApplier.angularForceDuration;
            localAngularForce = originalApplier.localAngularForce;
            angularForceType = originalApplier.angularForceType;
            angularForce = originalApplier.angularForce;
        }

        public void Remove()
        {
            Destroy(this);
        }

    }
}

using System.Collections.Generic;
using UnityEngine;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/FxModifierPhysic")]
    public class FxModifierPhysic : ThunderBehaviour
    {
        [Header("References")]
        public FxController fxController;
        public Rigidbody rb;

        [Header("Velocity")]
        public Link velocityLink = Link.None;
        public Transform velocityPointTransform;
        public Vector2 velocityRange = new Vector2(0, 5);
        public float velocityDampening = 1f;

        [Header("Torque")]
        public Link torqueLink = Link.None;
        public Vector2 torqueRange = new Vector2(2, 8);
        public float torqueDampening = 1f;

        public enum Link
        {
            None,
            Intensity,
            Speed,
        }

        protected float dampenedVelocity;
        protected float dampenedTorque;

        protected override ManagedLoops ManagedLoops => ManagedLoops.Update;

        private void OnValidate()
        {
            if (fxController == null) fxController = this.GetComponent<FxController>();
        }

        private void Awake()
        {
            if (fxController == null)
            {
                Debug.LogError("FxModifierPhysic have no fxController set!");
                this.enabled = false;
            }
            if (rb == null)
            {
                Debug.LogError("FxModifierPhysic have no rigidbody set!");
                this.enabled = false;
            }
        }

        protected internal override void ManagedUpdate()
        {
            if (velocityLink != Link.None)
            {
                if (!rb.isKinematic && !rb.IsSleeping())
                {
                    Vector3 pointVelocity = rb.GetPointVelocity(velocityPointTransform.position);
                    dampenedVelocity = Mathf.Lerp(dampenedVelocity, Mathf.InverseLerp(velocityRange.x, velocityRange.y, pointVelocity.magnitude), velocityDampening);
                    if (velocityLink == Link.Intensity) fxController.SetIntensity(dampenedVelocity);
                    if (velocityLink == Link.Speed) fxController.SetSpeed(dampenedVelocity);
                }
                else if (dampenedVelocity > 0)
                {
                    dampenedVelocity = Mathf.Lerp(dampenedVelocity, 0, velocityDampening);
                }
            }
            if (torqueLink != Link.None)
            {
                if (!rb.isKinematic && !rb.IsSleeping())
                {
                    dampenedTorque = Mathf.Lerp(dampenedTorque, Mathf.InverseLerp(torqueRange.x, torqueRange.y, rb.angularVelocity.magnitude), torqueDampening);
                    if (torqueLink == Link.Intensity) fxController.SetIntensity(dampenedTorque);
                    if (torqueLink == Link.Speed) fxController.SetSpeed(dampenedTorque);
                }
                else if (dampenedTorque > 0)
                {
                    dampenedTorque = Mathf.Lerp(dampenedTorque, 0, torqueDampening);
                }
            }
        }
    }
}

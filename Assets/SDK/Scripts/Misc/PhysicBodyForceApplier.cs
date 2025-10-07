using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class PhysicBodyForceApplier : ThunderBehaviour, IToolControllable
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
        protected float linearForceRemainingDuration = -1;

        public ForceApplyMode linearForceApplyMode;
        public Transform offsetTransform = null;
        public ForceMode linearForceType;
        public Vector3 linearForce;


        [Header("Angular force")]
        public bool angularForceActive;
        public float angularForceDuration;
        protected float angularForceRemainingDuration = -1;

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
        public void StartAngularForce() => angularForceRemainingDuration = angularForceDuration;

        [Button]
        public void StartLinearForce() => linearForceRemainingDuration = linearForceDuration;

        public bool IsCopyable() => true;

        public void CopyTo(UnityEngine.Object other)
        {
            ((IToolControllable)this).CopyControllableTo(other);
        }

        public void CopyFrom(IToolControllable original)
        {
            if (original is PhysicBodyForceApplier orgForceApplier)
            {
                linearForceActive = orgForceApplier.linearForceActive;
                linearForceDuration = orgForceApplier.linearForceDuration;
                linearForceApplyMode = orgForceApplier.linearForceApplyMode;
                if (orgForceApplier.linearForceApplyMode == ForceApplyMode.OffsetSpace && orgForceApplier.offsetTransform != null && orgForceApplier.offsetTransform.parent == orgForceApplier.transform)
                {
                    Transform copyOffset = new GameObject(orgForceApplier.offsetTransform.name).transform;
                    copyOffset.SetParentOrigin(transform, orgForceApplier.offsetTransform.localPosition, orgForceApplier.offsetTransform.localRotation, orgForceApplier.offsetTransform.localScale);
                    offsetTransform = copyOffset;
                }
                else
                {
                    offsetTransform = orgForceApplier.offsetTransform;
                }
                linearForceType = orgForceApplier.linearForceType;
                linearForce = orgForceApplier.linearForce;
                angularForceActive = orgForceApplier.angularForceActive;
                angularForceDuration = orgForceApplier.angularForceDuration;
                localAngularForce = orgForceApplier.localAngularForce;
                angularForceType = orgForceApplier.angularForceType;
                angularForce = orgForceApplier.angularForce;
            }
        }

        public void ReparentAlign(Component other) => ((IToolControllable)this).ReparentAlignTransform(other);

        public void Remove() => Destroy(this);

        public Transform GetTransform() => transform;

    }
}

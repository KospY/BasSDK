using System;
using System.Collections.Generic;
using UnityEngine;
using static ThunderRoad.PhysicBody;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class PhysicBody
    {
        public Rigidbody rigidBody;
        public Transform transform;
        public GameObject gameObject;

        private bool _isForcedFreeze;
        public RigidbodyConstraints _unfreezeConstraint;
        public Vector3 _unfreezInertiaTensor;
        public Quaternion _unfreezInertiaTensorRotation;

        public Action onMassChanged;
        public Action onCenterOfMassChanged;
        public Action onGravityChanged;
        public Action onInertiaTensorChanged;

        public PhysicBody(Rigidbody rigidbody)
        {
            this.rigidBody = rigidbody;
            this.transform = rigidbody.transform;
            this.gameObject = rigidbody.gameObject;
        }

        public PhysicBody(Transform transform)
        {
            this.transform = transform;
            this.gameObject = transform.gameObject;
            this.rigidBody = transform.GetComponent<Rigidbody>();
        }
        
        private Vector3 _centerOfMass;
        private bool comInitialSet = false;

        public Vector3 centerOfMass
        {
            get
            {
                //if this is the first time this is called, we want to cache the result
                if (!comInitialSet)
                {
                    _centerOfMass = rigidBody.centerOfMass;
                    comInitialSet = true;
                }

                return _centerOfMass;
            }
            set
            {

                rigidBody.centerOfMass = value;

                _centerOfMass = value;
                onCenterOfMassChanged?.Invoke();
            }
        }

        public Vector3 worldCenterOfMass => rigidBody.worldCenterOfMass;

        private int velocityLastUpdate;
        private Vector3 _velocity;

        public Vector3 velocity
        {
            get
            {
                return _velocity;
            }
            set { rigidBody.velocity = _velocity = value; }
        }

        private int angularVelocityLastUpdate;
        private Vector3 _angularVelocity;

        public Vector3 angularVelocity
        {
            get
            {
                return _angularVelocity;
            }
            set { rigidBody.angularVelocity = _angularVelocity = value; }
        }

        public Vector3 inertiaTensor
        {
            get => rigidBody.inertiaTensor;
            set
            {

                if (_isForcedFreeze)
                {
                    _unfreezInertiaTensor = value;
                }
                else
                {
                    rigidBody.inertiaTensor = value;
                }

                onInertiaTensorChanged?.Invoke();
            }
        }

        public Quaternion inertiaTensorRotation
        {
            get => rigidBody.inertiaTensorRotation;
            set
            {

                if (_isForcedFreeze)
                {
                    _unfreezInertiaTensorRotation = value;
                }
                else
                {
                    rigidBody.inertiaTensorRotation = value;
                }

                onInertiaTensorChanged?.Invoke();
            }
        }

        public float drag
        {
            get => rigidBody.drag;
            set => rigidBody.drag = value;
        }

        public float angularDrag
        {
            get => rigidBody.angularDrag;
            set => rigidBody.angularDrag = value;
        }

        public float sleepThreshold
        {
            get => rigidBody.sleepThreshold;
            set => rigidBody.sleepThreshold = value;
        }

        public float mass
        {
            get => rigidBody.mass;
            set
            {
                rigidBody.mass = value;

                onMassChanged?.Invoke();
            }
        }

        public bool useGravity
        {
            get => rigidBody.useGravity;
            set
            {

                rigidBody.useGravity = value;

                onGravityChanged?.Invoke();
            }
        }

        public RigidbodyInterpolation interpolation
        {
            get => rigidBody.interpolation;
            set => rigidBody.interpolation = value;
        }
        
        public bool freezeRotation
        {
            get => rigidBody.freezeRotation;
            set => rigidBody.freezeRotation = value;
        }

        public bool isEnabled
        {
            get => !isKinematic;
            set => isKinematic = !value;
        }
        
        private int isKinematicLastUpdate;
        private bool _isKinematic;

        public bool isKinematic
        {
            get
            {

                return _isKinematic;

            }
            set { rigidBody.isKinematic = _isKinematic = value; }
        }

        public CollisionDetectionMode collisionDetectionMode
        {
            get => rigidBody.collisionDetectionMode;
            set => rigidBody.collisionDetectionMode = value;
        }

        public RigidbodyConstraints constraints
        {
            get
            {
                if (_isForcedFreeze) return _unfreezeConstraint;

                return rigidBody.constraints;
            }
            set
            {

                if (_isForcedFreeze)
                {
                    _unfreezeConstraint = value;
                    return;
                }

                rigidBody.constraints = value;
            }
        }

        public void ForceFreeze()
        {
            if (_isForcedFreeze) return;
            _unfreezeConstraint = constraints;
            _unfreezInertiaTensorRotation = rigidBody.inertiaTensorRotation;
            _unfreezInertiaTensor = rigidBody.inertiaTensor;
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            _isForcedFreeze = true;

        }

        public void UnFreeze()
        {
            if (!_isForcedFreeze) return;
            rigidBody.constraints = constraints;
            rigidBody.inertiaTensorRotation = _unfreezInertiaTensorRotation;
            rigidBody.inertiaTensor = _unfreezInertiaTensor;
            _isForcedFreeze = false;
        }

        public void Teleport(Vector3 position, Quaternion rotation)
        {

            rigidBody.transform.SetPositionAndRotation(position, rotation);

        }

        public void MovePosition(Vector3 position)
        {

            rigidBody.MovePosition(position);

        }

        public void MoveRotation(Quaternion rotation)
        {

            rigidBody.MoveRotation(rotation);

        }

        public Vector3 GetPointVelocity(Vector3 worldPoint)
        {
            return rigidBody.GetPointVelocity(worldPoint);
        }

        private int sleepingLastUpdate;
        private bool _isSleeping;
        public bool IsSleeping()
        {
            return _isSleeping;
        }

        private int meaningfulVelocityLastUpdate;
        private bool _hasMeaningfulVelocity;
        public bool HasMeaningfulVelocity()
        {
            return _hasMeaningfulVelocity;
        }

        public Vector3 NaiveFuturePosition(float time) => NaiveFuturePosition(transform.position, time);

        public Vector3 NaiveFuturePosition(Vector3 position, float time)
        {
            Vector3 timeRotation = angularVelocity * time;
            return position.RotateAroundPivot(transform.TransformPoint(centerOfMass), Quaternion.Euler(timeRotation)) + (velocity * time);
        }

        public void WakeUp()
        {
            rigidBody.WakeUp();
            _isSleeping = false;
        }

        public void ResetInertiaTensor()
        {
            rigidBody.ResetInertiaTensor();
            onInertiaTensorChanged?.Invoke();
        }

        public void ResetCenterOfMass()
        {
            rigidBody.ResetCenterOfMass();
            onCenterOfMassChanged?.Invoke();
        }

        public void AddRadialForce(float force, Vector3 position, float upwardsModifier, ForceMode mode)
        {
            var closestPoint = rigidBody.ClosestPointOnBounds(position);
            var modifiedTarget = closestPoint + Vector3.up * upwardsModifier;
            var toTarget = modifiedTarget - position;
            this.AddForceAtPosition(toTarget.normalized * force, modifiedTarget, mode);

        }

        public void AddExplosionForce(float force, Vector3 position, float radius, float upwardsModifier, ForceMode mode)
        {

            rigidBody.AddExplosionForce(force, position, radius, upwardsModifier, mode);

        }
        public void AddForce(Vector3 force, ForceMode mode)
        {
            rigidBody.AddForce(force, mode);

        }

        public void AddRelativeForce(Vector3 force, ForceMode mode)
        {

            rigidBody.AddRelativeForce(force, mode);

        }

        public void AddForceAtPosition(Vector3 force, Vector3 position, ForceMode mode)
        {

            rigidBody.AddForceAtPosition(force, position, mode);

        }

        public void AddTorque(Vector3 force, ForceMode mode)
        {

            rigidBody.AddTorque(force, mode);

        }

        public void AddRelativeTorque(Vector3 force, ForceMode mode)
        {

            rigidBody.AddRelativeTorque(force, mode);

        }

        public static bool operator ==(PhysicBody lhs, PhysicBody rhs)
        {
            if (lhs is null) return rhs is null;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(PhysicBody lhs, PhysicBody rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (!(other is PhysicBody pb)) return false;

            return Equals(pb);
        }

        protected bool Equals(PhysicBody other)
        {
            if (other is null) return false;

            return Equals(rigidBody, other.rigidBody) && Equals(transform, other.transform) && Equals(gameObject, other.gameObject);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (rigidBody != null ? rigidBody.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (transform != null ? transform.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (gameObject != null ? gameObject.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static implicit operator bool(PhysicBody pb)
        {
            if (pb is null) return false;

            return pb.rigidBody;
        }
    }
}

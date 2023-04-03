using System;
using UnityEngine;

namespace ThunderRoad
{
    public class PhysicBody
    {
        public Rigidbody rigidBody;
        public ArticulationBody articulationBody;
        public bool isArticulationBody;
        public Transform transform;
        public GameObject gameObject;

        private bool _isForcedFreeze;
        public RigidbodyConstraints _unfreezeConstraint;
        public Vector3 _unfreezInertiaTensor;
        public Quaternion _unfreezInertiaTensorRotation;

        public PhysicBody(Rigidbody rigidbody)
        {
            this.rigidBody = rigidbody;
            this.isArticulationBody = false;
            this.transform = rigidbody.transform;
            this.gameObject = rigidbody.gameObject;
        }

        public PhysicBody(ArticulationBody articulationBody)
        {
            this.articulationBody = articulationBody;
            this.isArticulationBody = true;
            this.transform = articulationBody.transform;
            this.gameObject = articulationBody.gameObject;
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
                    _centerOfMass = isArticulationBody ? articulationBody.centerOfMass : rigidBody.centerOfMass;
                    comInitialSet = true;
                }

                return _centerOfMass;
            }
            set
            {
                if (isArticulationBody)
                {
                    articulationBody.centerOfMass = value;
                }
                else
                {
                    rigidBody.centerOfMass = value;
                }
                _centerOfMass = value;
            }
        }

        public Vector3 worldCenterOfMass => isArticulationBody ? articulationBody.worldCenterOfMass : rigidBody.worldCenterOfMass;

        private int velocityLastUpdate;
        private Vector3 _velocity;
        public Vector3 velocity
        {
            get
            {
                
                return _velocity;
            }
            set
            {
                if (isArticulationBody)
                {
                    articulationBody.velocity = _velocity = value;
                }
                else
                {
                    rigidBody.velocity = _velocity = value;
                }
            }
        }

        private int angularVelocityLastUpdate;
        private Vector3 _angularVelocity;
        public Vector3 angularVelocity
        {
            get
            {
                
                return _angularVelocity;
            }
            set
            {
                if (isArticulationBody)
                {
                    articulationBody.angularVelocity = _angularVelocity = value;
                }
                else
                {
                    rigidBody.angularVelocity = _angularVelocity = value;
                }
            }
        }

        public Vector3 inertiaTensor
        {
            get => isArticulationBody ? articulationBody.inertiaTensor : rigidBody.inertiaTensor;
            set
            {
                if (isArticulationBody)
                {
                    articulationBody.inertiaTensor = value;
                }
                else
                {
                    if (_isForcedFreeze)
                    {
                        _unfreezInertiaTensor = value;
                    }
                    else
                    {
                        rigidBody.inertiaTensor = value;
                    }
                }
            }
        }

        public Quaternion inertiaTensorRotation
        {
            get => isArticulationBody ? articulationBody.inertiaTensorRotation : rigidBody.inertiaTensorRotation;
            set
            {
                if (isArticulationBody)
                {
                    articulationBody.inertiaTensorRotation = value;
                }
                else
                {
                    if (_isForcedFreeze)
                    {
                        _unfreezInertiaTensorRotation = value;
                    }
                    else
                    {
                        rigidBody.inertiaTensorRotation = value;
                    }
                }
            }
        }

        public float drag
        {
            get => isArticulationBody ? articulationBody.linearDamping : rigidBody.drag;
            set
            {
                if (isArticulationBody)
                {
                    articulationBody.linearDamping = value;
                }
                else
                {
                    rigidBody.drag = value;
                }
            }
        }

        public float angularDrag
        {
            get => isArticulationBody ? articulationBody.angularDamping : rigidBody.angularDrag;
            set
            {
                if (isArticulationBody)
                {
                    articulationBody.angularDamping = value;
                }
                else
                {
                    rigidBody.angularDrag = value;
                }
            }
        }

        public float sleepThreshold
        {
            get => isArticulationBody ? articulationBody.sleepThreshold : rigidBody.sleepThreshold;
            set
            {
                if (isArticulationBody)
                {
                    articulationBody.sleepThreshold = value;
                }
                else
                {
                    rigidBody.sleepThreshold = value;
                }
            }
        }

        public float mass
        {
            get => isArticulationBody ? articulationBody.mass : rigidBody.mass;
            set
            {
                if (isArticulationBody)
                {
                    articulationBody.mass = value;
                }
                else
                {
                    rigidBody.mass = value;
                }
            }
        }

        public bool useGravity
        {
            get => isArticulationBody ? articulationBody.useGravity : rigidBody.useGravity;
            set
            {
                if (isArticulationBody)
                {
                    articulationBody.useGravity = value;
                }
                else
                {
                    rigidBody.useGravity = value;
                }
            }
        }

        private int isKinematicLastUpdate;
        private bool _isKinematic;
        public bool isKinematic
        {
            get
            {
                
                return _isKinematic;
            }
            set
            {
                if (isArticulationBody)
                {
                    articulationBody.immovable = _isKinematic = value;
                }
                else
                {
                    rigidBody.isKinematic = _isKinematic = value;
                }
            }
        }

        public CollisionDetectionMode collisionDetectionMode
        {
            get => isArticulationBody ? articulationBody.collisionDetectionMode : rigidBody.collisionDetectionMode;
            set
            {
                if (isArticulationBody)
                {
                    articulationBody.collisionDetectionMode = value;
                }
                else
                {
                    rigidBody.collisionDetectionMode = value;
                }
            }
        }

        public RigidbodyConstraints constraints
        {
            get
            {
                if (isArticulationBody) return RigidbodyConstraints.None;

                if (_isForcedFreeze) return _unfreezeConstraint;

                return rigidBody.constraints;
            }
            set
            {
                if (isArticulationBody)
                {
                    if (constraints != RigidbodyConstraints.None)
                        Debug.LogError("unable to set constraints on " + articulationBody.name +
                                       ", ArticulationBody don't support constraints");

                    return;
                }

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

        public Vector3 GetPointVelocity(Vector3 worldPoint)
        {
            return isArticulationBody ? articulationBody.GetPointVelocity(worldPoint) : rigidBody.GetPointVelocity(worldPoint);
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

        public void WakeUp()
        {
            if (isArticulationBody)
            {
                articulationBody.WakeUp();
            }
            else
            {
                rigidBody.WakeUp();
            }
            _isSleeping = false;
        }

        public void ResetInertiaTensor()
        {
            if (isArticulationBody)
            {
                articulationBody.ResetInertiaTensor();
            }
            else
            {
                rigidBody.ResetInertiaTensor();
            }
        }

        public void ResetCenterOfMass()
        {
            if (isArticulationBody)
            {
                articulationBody.ResetCenterOfMass();
            }
            else
            {
                rigidBody.ResetCenterOfMass();
            }
        }

        public void AddForce(Vector3 force, ForceMode mode)
        {
            if (isArticulationBody)
            {
                articulationBody.AddForce(force, mode);
            }
            else
            {
                rigidBody.AddForce(force, mode);
            }
        }

        public void AddRelativeForce(Vector3 force, ForceMode mode)
        {
            if (isArticulationBody)
            {
                articulationBody.AddRelativeForce(force, mode);
            }
            else
            {
                rigidBody.AddRelativeForce(force, mode);
            }
        }   

        public void AddForceAtPosition(Vector3 force, Vector3 position, ForceMode mode)
        {
            if (isArticulationBody)
            {
                articulationBody.AddForceAtPosition(force, position);
            }
            else
            {
                rigidBody.AddForceAtPosition(force, position, mode);
            }
        }

        public void AddTorque(Vector3 force, ForceMode mode)
        {
            if (isArticulationBody)
            {
                articulationBody.AddTorque(force, mode);
            }
            else
            {
                rigidBody.AddTorque(force, mode);
            }
        }

        public void AddRelativeTorque(Vector3 force, ForceMode mode)
        {
            if (isArticulationBody)
            {
                articulationBody.AddRelativeTorque(force, mode);
            }
            else
            {
                rigidBody.AddRelativeTorque(force, mode);
            }
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

            return Equals(rigidBody, other.rigidBody) && Equals(articulationBody, other.articulationBody) &&
                   isArticulationBody == other.isArticulationBody && Equals(transform, other.transform) &&
                   Equals(gameObject, other.gameObject);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (rigidBody != null ? rigidBody.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (articulationBody != null ? articulationBody.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ isArticulationBody.GetHashCode();
                hashCode = (hashCode * 397) ^ (transform != null ? transform.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (gameObject != null ? gameObject.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static implicit operator bool(PhysicBody pb)
        {
            if (pb is null) return false;

            return pb.rigidBody || pb.articulationBody;
        }
    }
}
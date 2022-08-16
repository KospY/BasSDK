using UnityEngine;
using UnityEngine.XR;

namespace ThunderRoad
{

	public class PlayerTestHand : MonoBehaviour
    {
        public XRNode xrNode;
        protected Rigidbody rb;
        protected FixedJoint fixedJoint;
        protected Rigidbody grabbedRb;
        protected bool orgKinematic;
        protected InputDevice device;

        protected bool primaryPressState;
        protected bool secondaryPressState;
        protected bool gripPressState;

        private void Awake()
        {
            rb = this.GetComponent<Rigidbody>();
            if (!rb) rb = this.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }

        void Update()
        {
            device = InputDevices.GetDeviceAtXRNode(xrNode);
#if DUNGEN
            if (xrNode == XRNode.LeftHand)
            {
                if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryPressed))
                {
                    if (primaryPressed)
                    {
                        if (!primaryPressState)
                        {
                            PlayerTest playerControllerTest = this.GetComponentInParent<PlayerTest>();

                            DunGen.AdjacentRoomCulling adjacentRoomCulling = GameObject.FindObjectOfType<DunGen.AdjacentRoomCulling>();
                            if (adjacentRoomCulling) adjacentRoomCulling.enabled = !adjacentRoomCulling.enabled;
                            primaryPressState = true;
                        }
                    }
                    else
                    {
                        if (primaryPressState)
                        {
                            primaryPressState = false;
                        }
                    }
                }
                if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryPressed))
                {
                    if (secondaryPressed)
                    {
                        if (!secondaryPressState)
                        {
                            Level level = GameObject.FindObjectOfType<Level>();
                            if (level && level.dungeon)
                            {
                                level.dungeon.GenerateDungeon();
                            }
                            secondaryPressState = true;
                        }
                    }
                    else
                    {
                        if (secondaryPressState)
                        {
                            secondaryPressState = false;
                        }
                    }
                }
            }
#endif
            if (device.TryGetFeatureValue(CommonUsages.gripButton, out bool gripPressed))
            {
                if (gripPressed)
                {
                    if (!gripPressState)
                    {
                        if (!grabbedRb)
                        {
                            Collider[] colliders = Physics.OverlapSphere(this.transform.position, 0.05f);
                            float closestDistanceSqr = Mathf.Infinity;
                            Collider nearestCollider = null;
                            foreach (Collider collider in colliders)
                            {
                                if (collider.attachedRigidbody)
                                {
                                    float dSqrToTarget = (collider.ClosestPoint(this.transform.position) - this.transform.position).sqrMagnitude;
                                    if (dSqrToTarget < closestDistanceSqr)
                                    {
                                        closestDistanceSqr = dSqrToTarget;
                                        nearestCollider = collider;
                                    }
                                }
                            }
                            if (nearestCollider)
                            {
                                if (nearestCollider.attachedRigidbody)
                                {
                                    //orgKinematic = nearestCollider.attachedRigidbody.isKinematic;
                                    grabbedRb = nearestCollider.attachedRigidbody;
                                    fixedJoint = rb.gameObject.AddComponent<FixedJoint>();
                                    fixedJoint.connectedBody = grabbedRb;
                                    this.GetComponent<MeshRenderer>().enabled = false;
                                }
                            }
                        }
                        gripPressState = true;
                    }
                }
                else
                {
                    if (gripPressState)
                    {
                        if (grabbedRb)
                        {
                            if (fixedJoint) Destroy(fixedJoint);
                            grabbedRb = null;
                            this.GetComponent<MeshRenderer>().enabled = true;
                        }
                        gripPressState = false;
                    }
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerGrabTest : MonoBehaviour
{
    public XRNode xrNode;
    protected Transform grabbedTransform;
    protected Rigidbody grabbedRb;
    protected bool orgKinematic;
    protected InputDevice device;
    protected bool pressState;

    private void Start()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(xrNode, devices);

        if (devices.Count == 1)
        {
            device = devices[0];
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", device.name, device.role.ToString()));
        }
        else if (devices.Count > 1)
        {
            Debug.Log("Found more than one left hand!");
        }
    }

    void Update()
    {
        bool pressed;
        if (device.TryGetFeatureValue(CommonUsages.gripButton, out pressed))
        {
            if (pressed)
            {
                if (!pressState)
                {
                    if (!grabbedTransform)
                    {
                        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 0.05f);
                        float closestDistanceSqr = Mathf.Infinity;
                        Collider nearestCollider = null;
                        foreach (Collider collider in colliders)
                        {
                            float dSqrToTarget = (collider.ClosestPoint(this.transform.position) - this.transform.position).sqrMagnitude;
                            if (dSqrToTarget < closestDistanceSqr)
                            {
                                closestDistanceSqr = dSqrToTarget;
                                nearestCollider = collider;
                            }
                        }
                        if (nearestCollider)
                        {
                            if (nearestCollider.attachedRigidbody)
                            {
                                orgKinematic = nearestCollider.attachedRigidbody.isKinematic;
                                grabbedTransform = nearestCollider.attachedRigidbody.transform;
                            }
                            else
                            {
                                grabbedTransform = nearestCollider.transform.root;
                            }       
                            grabbedTransform.SetParent(this.transform);
                            this.GetComponent<MeshRenderer>().enabled = false;
                        }
                    }
                    pressState = true;
                }
            }
            else
            {
                if (pressState)
                {
                    if (grabbedTransform)
                    {
                        if (grabbedRb) grabbedRb.isKinematic = orgKinematic;
                        grabbedTransform.SetParent(null);
                        grabbedTransform = null;
                        this.GetComponent<MeshRenderer>().enabled = true;
                    }
                    pressState = false;
                }
            }
        }
    }
}

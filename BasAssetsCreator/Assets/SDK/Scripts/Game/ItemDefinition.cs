using System;
using System.Collections.Generic;
using UnityEngine;

namespace BS
{
    [RequireComponent(typeof(Rigidbody))]
    public class ItemDefinition : MonoBehaviour
    {
        public string itemId;
        public Transform holderPoint;
        public Transform parryPoint;
        public HandleDefinition mainHandleRight;
        public HandleDefinition mainHandleLeft;
        public Transform flyDirRef;
        public Preview preview;
        private WhooshPoint whoosh;
        public bool useCustomCenterOfMass;
        public Vector3 customCenterOfMass;
        public bool customInertiaTensor;
        public CapsuleCollider customInertiaTensorCollider;
        public List<CustomReference> customReferences;
    

        [NonSerialized]
        public List<Renderer> renderers;
        [NonSerialized]
        public List<ColliderGroup> colliderGroups;
        [NonSerialized]
        public List<WhooshPoint> whooshPoints;

        protected virtual void OnValidate()
        {
            if (!gameObject.activeInHierarchy) return;
            holderPoint = transform.Find("HolderPoint");
            if (!holderPoint)
            {
                holderPoint = new GameObject("HolderPoint").transform;
                holderPoint.SetParent(transform, false);
            }
            parryPoint = transform.Find("ParryPoint");
            if (!parryPoint)
            {
                parryPoint = new GameObject("ParryPoint").transform;
                parryPoint.SetParent(transform, false);
            }
            preview = GetComponentInChildren<Preview>();
            if (!preview && transform.Find("Preview")) preview = transform.Find("Preview").gameObject.AddComponent<Preview>();
            if (!preview)
            {
                preview = new GameObject("Preview").AddComponent<Preview>();  
                preview.transform.SetParent(transform, false);
            }
            whoosh = GetComponentInChildren<WhooshPoint>();
            if (!whoosh && transform.Find("Whoosh")) whoosh = transform.Find("Whoosh").gameObject.AddComponent<WhooshPoint>();
            if (!whoosh)
            { 
                whoosh = new GameObject("Whoosh").AddComponent<WhooshPoint>();
                whoosh.transform.SetParent(transform, false); 
            }

            foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
            {
                if (!mesh.GetComponent<Paintable>())
                {
                    mesh.gameObject.AddComponent<Paintable>();
                }
            }

            if (!mainHandleRight)
            {
                foreach (HandleDefinition handleDefinition in GetComponentsInChildren<HandleDefinition>())
                {
                    if (handleDefinition.IsAllowed(Side.Right))
                    {
                        mainHandleRight = handleDefinition;
                        break;
                    }
                }
            }
            if (!mainHandleLeft)
            {
                foreach (HandleDefinition handleDefinition in GetComponentsInChildren<HandleDefinition>())
                {
                    if (handleDefinition.IsAllowed(Side.Left))
                    {
                        mainHandleLeft = handleDefinition;
                        break;
                    }
                }
            }
            if (!mainHandleRight)
            {
                mainHandleRight = GetComponentInChildren<HandleDefinition>();
            }
            if (useCustomCenterOfMass)
            {
                GetComponent<Rigidbody>().centerOfMass = customCenterOfMass;
            }
            else
            {
                GetComponent<Rigidbody>().ResetCenterOfMass();
            }
            if (customInertiaTensor)
            {
                if (customInertiaTensorCollider == null)
                {
                    Transform foundInertiaTensor = GetComponentInParent<ItemDefinition>().transform.Find("InertiaTensorCollider");
                    if (foundInertiaTensor)
                    {
                        customInertiaTensorCollider = foundInertiaTensor.GetComponent<CapsuleCollider>();
                    }
                }
                if (customInertiaTensorCollider == null)
                {
                    customInertiaTensorCollider = new GameObject("InertiaTensorCollider").AddComponent<CapsuleCollider>();
                    customInertiaTensorCollider.transform.SetParent(transform, false);
                    customInertiaTensorCollider.radius = 0.05f; 
                    customInertiaTensorCollider.direction = 2; 
                }
                customInertiaTensorCollider.enabled = false;
                customInertiaTensorCollider.isTrigger = true;
                customInertiaTensorCollider.gameObject.layer = 2;
            }
            else
            {
                customInertiaTensorCollider = null;
            }
        }

        public static void DrawGizmoArrow(Vector3 pos, Vector3 direction, Vector3 upwards, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
            Vector3 right = Quaternion.LookRotation(direction, upwards) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction, upwards) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.TransformPoint(GetComponent<Rigidbody>().centerOfMass), 0.01f);
            Gizmos.matrix = holderPoint.transform.localToWorldMatrix;
            DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f, Vector3.up, Common.HueColourValue(HueColorNames.Purple), 0.1f, 10);
            DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f, Vector3.up, Common.HueColourValue(HueColorNames.Green), 0.05f);
        }
    }
}

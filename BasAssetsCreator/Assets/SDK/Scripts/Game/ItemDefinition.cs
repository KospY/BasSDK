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
            if (!this.gameObject.activeInHierarchy) return;
            holderPoint = this.transform.Find("HolderPoint");
            if (!holderPoint)
            {
                holderPoint = new GameObject("HolderPoint").transform;
                holderPoint.SetParent(this.transform, false);
            }
            parryPoint = this.transform.Find("ParryPoint");
            if (!parryPoint)
            {
                parryPoint = new GameObject("ParryPoint").transform;
                parryPoint.SetParent(this.transform, false);
            }
            preview = this.GetComponentInChildren<Preview>();
            if (!preview && this.transform.Find("Preview")) preview = this.transform.Find("Preview").gameObject.AddComponent<Preview>();
            if (!preview)
            {
                preview = new GameObject("Preview").AddComponent<Preview>();
                preview.transform.SetParent(this.transform, false);
            }
            Transform whoosh = this.transform.Find("Whoosh");
            if (whoosh && !whoosh.GetComponent<WhooshPoint>())
            {
                whoosh.gameObject.AddComponent<WhooshPoint>();
            }

            if (!mainHandleRight)
            {
                foreach (HandleDefinition handleDefinition in this.GetComponentsInChildren<HandleDefinition>())
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
                foreach (HandleDefinition handleDefinition in this.GetComponentsInChildren<HandleDefinition>())
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
                mainHandleRight = this.GetComponentInChildren<HandleDefinition>();
            }
            if (useCustomCenterOfMass)
            {
                this.GetComponent<Rigidbody>().centerOfMass = customCenterOfMass;
            }
            else
            {
                this.GetComponent<Rigidbody>().ResetCenterOfMass();
            }
            if (customInertiaTensor)
            {
                if (customInertiaTensorCollider == null)
                {
                    customInertiaTensorCollider = new GameObject("InertiaTensorCollider").AddComponent<CapsuleCollider>();
                    customInertiaTensorCollider.transform.SetParent(this.transform, false);
                    customInertiaTensorCollider.radius = 0.05f;
                    customInertiaTensorCollider.direction = 2;
                }
                customInertiaTensorCollider.enabled = false;
                customInertiaTensorCollider.isTrigger = true;
                customInertiaTensorCollider.gameObject.layer = 2;
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
            Gizmos.DrawWireSphere(this.transform.TransformPoint(this.GetComponent<Rigidbody>().centerOfMass), 0.01f);
            Gizmos.matrix = holderPoint.transform.localToWorldMatrix;
            DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f, Vector3.up, Common.HueColourValue(HueColorNames.Purple), 0.1f, 10);
            DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f, Vector3.up, Common.HueColourValue(HueColorNames.Green), 0.05f);
        }
    }
}

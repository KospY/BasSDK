using UnityEngine;
using System.Collections.Generic;


namespace ThunderRoad
{
    public class InteractableDefinition : MonoBehaviour
    {
        public string interactableId;

        public float axisLength = 0;
        public float touchRadius = 0.1f;
        public Vector3 touchCenter;

        protected virtual void Awake()
        {
        }
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            if (axisLength > 0)
            {
                Gizmos.color = Color.white;
                Common.DrawGizmoArrow(Vector3.zero, Vector3.up * (axisLength / 2), Common.HueColourValue(HueColorName.White), 0.05f);
                Common.DrawGizmoArrow(Vector3.zero, -Vector3.up * (axisLength / 2), Common.HueColourValue(HueColorName.White), 0.05f);
                Common.DrawGizmoCapsule(Vector3.zero, Vector3.up * (axisLength / 2), Common.HueColourValue(HueColorName.White), axisLength / 2, touchRadius);
                Common.DrawGizmoCapsule(Vector3.zero, -Vector3.up * (axisLength / 2), Common.HueColourValue(HueColorName.White), axisLength / 2, touchRadius);
            }
            else
            {
                Gizmos.matrix = this.transform.localToWorldMatrix;
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(touchCenter, touchRadius);
            }
        }
    }
}

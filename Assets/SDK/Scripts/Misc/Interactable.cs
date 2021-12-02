using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.AddressableAssets;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class Interactable : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllInteractableID")]
#endif
        public string interactableId;

        public HandSide allowedHandSide = HandSide.Both;

        public enum HandSide
        {
            Both,
            Right,
            Left,
        }

        public float axisLength = 0;
        public float touchRadius = 0.1f;
        public Vector3 touchCenter;

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

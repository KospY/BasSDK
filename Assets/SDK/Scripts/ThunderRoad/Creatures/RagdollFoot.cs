using UnityEngine;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Ragdoll foot")]
    public class RagdollFoot : RagdollPart
    {
        public Side side = Side.Right;
        public Transform grip;

        [NonSerialized]
        public Transform toesAnchor;
        [NonSerialized]
        public Transform upperLegBone;


        protected override void OnValidate()
        {
            base.OnValidate();
            if (!this.gameObject.activeInHierarchy) return;
            IconManager.SetIcon(this.gameObject, IconManager.LabelIcon.Red);
            grip = this.transform.Find("Grip");
            if (!grip)
            {
                grip = new GameObject("Grip").transform;
                grip.SetParent(this.transform);
            }
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            if (grip)
            {
                Gizmos.matrix = grip.transform.localToWorldMatrix;
                Common.DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.05f, Common.HueColourValue(HueColorName.Purple), 0.05f, 10);
                Common.DrawGizmoArrow(Vector3.zero, Vector3.up * 0.025f, Common.HueColourValue(HueColorName.Green), 0.025f);
            }
        }

    }
}
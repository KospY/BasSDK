using UnityEngine;
using System;

#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/RagdollFoot")]
	[AddComponentMenu("ThunderRoad/Creatures/Ragdoll foot")]
    public class RagdollFoot : RagdollPart
    {
        public Side side = Side.Right;
        public Transform grip;

        [Header("Bones (non-humanoid creature only)")]
        [Tooltip("detected automatically for humanoid")]
        public Transform upperLegBone;
        [Tooltip("detected automatically for humanoid")]
        public Transform lowerLegBone;
        [Tooltip("detected automatically for humanoid")]
        public Transform toesBone;

        [NonSerialized]
        public Transform toesAnchor;


        protected override void OnValidate()
        {
            base.OnValidate();
            IconManager.SetIcon(this.gameObject, null);
            if (!this.gameObject.activeInHierarchy) return;
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
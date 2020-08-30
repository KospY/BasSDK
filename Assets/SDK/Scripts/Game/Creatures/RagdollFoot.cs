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
    public class RagdollFoot : MonoBehaviour
    {
        public Side side = Side.Right;

        [NonSerialized]
        public Creature creature;
        [NonSerialized]
        public Transform grip;
        [NonSerialized]
        public Transform toesBone;
        [NonSerialized]
        public Transform ankleBone;
        [NonSerialized]
        public Transform lowerLegBone;
        [NonSerialized]
        public Transform upperLegBone;


        public virtual void OnValidate()
        {
            if (!this.gameObject.activeInHierarchy) return;
            creature = this.GetComponentInParent<Creature>();
            if (creature)
            {
                grip = this.transform.Find("Grip");
                if (!grip)
                {
                    grip = CreateDefaultGrip();
                }
            }
            IconManager.SetIcon(this.gameObject, IconManager.LabelIcon.Red);
        }

        public RagdollFoot GetOtherFoot()
        {
            if (!creature) creature = this.GetComponentInParent<Creature>();
            return side == Side.Left ? creature.footRight : creature.footLeft;
        }

        [Button]
        public virtual void MirrorConfigToOtherHand()
        {
            creature = this.GetComponentInParent<Creature>();

            // Destroy all childrens of other hand
            while (GetOtherFoot().transform.childCount > 0)
            {
                DestroyImmediate(GetOtherFoot().transform.GetChild(0).gameObject);
            }
            // Copy childs to other hand and mirror X
            foreach (Transform childTransform in this.transform)
            {
                Transform copyTransform = Instantiate(childTransform);
                copyTransform.name = childTransform.name;
                copyTransform.SetParent(GetOtherFoot().transform);
                copyTransform.localPosition = childTransform.localPosition;
                copyTransform.localRotation = childTransform.localRotation;
                copyTransform.localScale = childTransform.localScale;
            }
            GetOtherFoot().transform.MirrorChilds(new Vector3(-1, 1, 1));
            GetOtherFoot().OnValidate();
        }

        [Button]
        public virtual void SetGripToDefaultPosition()
        {
            grip = this.transform.Find("Grip");
            if (grip) DestroyImmediate(grip.gameObject);
            grip = CreateDefaultGrip();
        }

        public virtual Transform CreateDefaultGrip()
        {
            Transform newGrip = new GameObject("Grip").transform;
            newGrip.SetParent(this.transform);
            newGrip.transform.localScale = Vector3.one;
            if (side == Side.Left)
            {
                newGrip.transform.localPosition = new Vector3(-0.04f, -0.03f, 0);
                newGrip.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
            if (side == Side.Right)
            {
                newGrip.transform.localPosition = new Vector3(0.04f, -0.03f, 0);
                newGrip.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            return newGrip;
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector3(0, 0, 0.045f), new Vector3(0.05f, 0.01f, 0.05f)); // foot
            Gizmos.DrawWireCube(new Vector3(side == Side.Left ? 0.017f : -0.017f, 0, 0.085f), new Vector3(0.01f, 0.01f, 0.02f)); // big
            Gizmos.DrawWireCube(new Vector3(side == Side.Left ? 0.002f : -0.002f, 0, 0.0825f), new Vector3(0.01f, 0.01f, 0.015f)); // middle
            Gizmos.DrawWireCube(new Vector3(side == Side.Left ? -0.014f : 0.014f, 0, 0.08f), new Vector3(0.01f, 0.01f, 0.01f)); // Little

            if (grip)
            {
                Gizmos.matrix = grip.transform.localToWorldMatrix;
                Common.DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.05f, Common.HueColourValue(HueColorName.Purple), 0.05f, 10);
                Common.DrawGizmoArrow(Vector3.zero, Vector3.up * 0.025f, Common.HueColourValue(HueColorName.Green), 0.025f);
            }
        }

    }
}
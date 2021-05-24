using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Holder")]
    [RequireComponent(typeof(Rigidbody))]
    public class Holder : Interactable
    {
        public static bool showHudHighlighter = true;
        public List<Transform> slots = new List<Transform>();
        public List<Item> startObjects = new List<Item>();
        public List<Collider> ignoredColliders = new List<Collider>();
        public string editorTargetAnchor;


        protected virtual void OnValidate()
        {
            if (slots.Count == 0) slots.Add(this.transform);
        }


        [Button("Align start object")]
        public void AlignObject(Item item)
        {
            Item.HolderPoint hp = item.GetHolderPoint(editorTargetAnchor);
            item.transform.MoveAlign(hp != null ? hp.anchor : item.transform, slots[0].transform);
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            if (slots == null) return;
            if (slots.Count == 0)
            {
                Gizmos.matrix = this.transform.localToWorldMatrix;
                Common.DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f, Common.HueColourValue(HueColorName.Purple), 0.1f, 10);
                Common.DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f, Common.HueColourValue(HueColorName.Green), 0.05f);
            }
            else
            {
                foreach (Transform slot in slots)
                {
                    Gizmos.matrix = slot.localToWorldMatrix;
                    Common.DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f / 2, Common.HueColourValue(HueColorName.Purple), 0.1f / 2, 10 / 2);
                    Common.DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f / 2, Common.HueColourValue(HueColorName.Green), 0.05f / 2);
                }
            }
        }


    }
}

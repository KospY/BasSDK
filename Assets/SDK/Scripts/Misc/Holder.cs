using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/Holder.html")]
    [AddComponentMenu("ThunderRoad/Holder")]
    public class Holder : Interactable
    {
        [Tooltip("If enabled, when the hand is near the holder, it will display a HUD Highlighter")]
        public static bool showHudHighlighter = true;
        [Tooltip("If set to true, this holder will ignore the JSON item limit, and spawn an infinite supply of the item")]
        public static bool infiniteSupply = false;
        [Tooltip("This is the position of the holder, assuming this holder has been placed on a creature.")]
        public DrawSlot drawSlot = DrawSlot.None;
        [Tooltip("If useAnchor is set to true, the holder will only align the items to the first slot.")]
        public bool useAnchor = true;
        [Tooltip("Determines the amount of slots in the holder. The number of items inside the holder can bypass the amount of slots, but it will not make new slots for the items to sit in.")]
        public List<Transform> slots = new List<Transform>();
        [Tooltip("This is a list of items that the holder will start with. For this to take effect, the item must exist in the scene that the holder is in (for example, an item rack)")]
        public List<Item> startObjects = new List<Item>();
        [Tooltip("GameObjects placed in this holder will not have any physics interactions with any colliders in this list, when the item is taken out (For example, an arrow will not collide with its' quiver)")]
        public List<Collider> ignoredColliders = new List<Collider>();
        [Tooltip("This value is used to define the target anchor in editor only. This is ignored in game, and is a testing tool only.")]
        public string editorTargetAnchor;
        [NonSerialized]
        public bool spawningItem;

        public enum DrawSlot
        {
            None,
            BackRight,
            BackLeft,
            HipsRight,
            HipsLeft,
        }

        [Serializable]
        public class HolderSize
        {
#if ODIN_INSPECTOR
            [ValueDropdown(nameof(GetAllHolderSlots))]
#endif
            public string slot;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllHolderSlots()
            {
                return Catalog.GetDropdownHolderSlots();
            }
#endif
        }

        [NonSerialized]
        public List<Item> items = new List<Item>();


        protected virtual void OnValidate()
        {
            if (!gameObject.activeInHierarchy) return;
            if (slots.Count == 0) slots.Add(this.transform);
        }

        [Button("Refill holder")]
        public void FillWithDefault()
        {
        }

        [Button("Align start object")]
        public virtual void AlignObject(Item item)
        {
	        string targetAnchor = editorTargetAnchor;

            Item.HolderPoint hp = item.GetHolderPoint(targetAnchor);
            
            ItemData.CustomSnap cs = item.GetCustomSnap(name);
            Transform itemHolderPoint = hp != null ? hp.anchor : item.transform;
            Transform alignmentPoint = useAnchor ? slots[0].transform : slots[items.IndexOf(item)].transform;
            Quaternion holderStartLocal = itemHolderPoint.localRotation;
            itemHolderPoint.LocalEulerRotation(cs != null ? -cs.localRotation : Vector3.zero, item.transform);
            Vector3 transformedHolderPoint = item.transform.TransformPoint(item.transform.InverseTransformPoint(itemHolderPoint.position) + (cs != null ? cs.localPosition : Vector3.zero));
            Vector3 resultPoint = alignmentPoint.TransformPoint(itemHolderPoint.InverseTransformPoint(transformedHolderPoint));
            item.transform.MoveAlign(itemHolderPoint, resultPoint, alignmentPoint.rotation, alignmentPoint);
            itemHolderPoint.localRotation = holderStartLocal;
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
	                if(slot == null) continue;
                    Gizmos.matrix = slot.localToWorldMatrix;
                    Common.DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f / 2, Common.HueColourValue(HueColorName.Purple), 0.1f / 2, 10 / 2);
                    Common.DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f / 2, Common.HueColourValue(HueColorName.Green), 0.05f / 2);
                }
            }
        }

        private void OnDrawGizmos()
        {
        }


        public void UnSnapOneItem(bool destroy)
        {
        }


        public virtual void UnSnapAll()
        {
        }
    }
}
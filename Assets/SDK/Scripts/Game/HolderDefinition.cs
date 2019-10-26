using UnityEngine;
using System.Collections.Generic;
using System.Linq;
#if ProjectCore
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace BS
{
    public class HolderDefinition : InteractableDefinition
    {
        public List<Transform> slots = new List<Transform>();
        public List<ItemDefinition> startObjects = new List<ItemDefinition>();
        public List<Collider> ignoredColliders = new List<Collider>();

#if ProjectCore
        public new List<ValueDropdownItem<string>> GetAllInteractableID()
        {
            return Catalog.GetDropdownAllID<InteractableHolder>();
        }
#endif

        protected virtual void OnValidate()
        {
            if (slots.Count == 0) slots.Add(this.transform);
        }

        protected override void Awake()
        {
            base.Awake();
            if (slots.Count == 0) slots.Add(this.transform);
        }

        [Button("Align start object")]
        public void AlignObject()
        {
            foreach (ItemDefinition startObject in startObjects)
            {
                if (slots.ElementAtOrDefault(startObjects.IndexOf(startObject)) != null) startObject.transform.MoveAlign(startObject.holderPoint ? startObject.holderPoint : startObject.transform, slots[startObjects.IndexOf(startObject)].transform);
                else Debug.LogError("Slot " + startObjects.IndexOf(startObject) + " do not exist!!");
            }
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            if (slots == null) return;
            if (slots.Count == 0)
            {
                Gizmos.matrix = this.transform.localToWorldMatrix;
                Common.DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f, Common.HueColourValue(HueColorNames.Purple), 0.1f, 10);
                Common.DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f, Common.HueColourValue(HueColorNames.Green), 0.05f);
            }
            else
            {
                foreach (Transform slot in slots)
                {
                    Gizmos.matrix = slot.localToWorldMatrix;
                    Common.DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f / 2, Common.HueColourValue(HueColorNames.Purple), 0.1f / 2, 10 / 2);
                    Common.DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f / 2, Common.HueColourValue(HueColorNames.Green), 0.05f / 2);
                }
            }
        }
    }
}

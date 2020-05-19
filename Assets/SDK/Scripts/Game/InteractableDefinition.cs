using UnityEngine;
using System.Collections.Generic;

#if ProjectCore
using Sirenix.OdinInspector;
#endif

namespace BS
{
    public class InteractableDefinition : MonoBehaviour
    {
#if ProjectCore
        [ValueDropdown("GetAllInteractableID")]
#endif
        public string interactableId;

        public float axisLength = 0;
        public float touchRadius = 0.1f;
        public Vector3 touchCenter;

#if ProjectCore
        public List<ValueDropdownItem<string>> GetAllInteractableID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Interactable);
        }
#endif
        protected virtual void Awake()
        {
#if ProjectCore
            if (interactableId != null && interactableId != "" && interactableId != "None")
            {
                InteractableData interactableData = Catalog.GetData<InteractableData>(interactableId);
                if (interactableData != null) Load(interactableData.Clone() as InteractableData);
            }
#endif
        }
#if ProjectCore
        public virtual void Load(InteractableData interactableData)
        {
            foreach (Interactable interactable in this.GetComponents<Interactable>()) Destroy(interactable);
            interactableId = interactableData.id;
            interactableData.CreateComponent(this.gameObject);
        }
#endif
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

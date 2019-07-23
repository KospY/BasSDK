using UnityEngine;
using System.Collections.Generic;

#if FULLGAME
using Sirenix.OdinInspector;
#endif

namespace BS
{
    public class InteractableDefinition : MonoBehaviour
    {
#if FULLGAME
        [ValueDropdown("GetAllInteractableID")]
#endif
        public string interactableId;

        public float axisLength = 0;
        public float touchRadius = 0.1f;
        public Vector3 touchCenter;

#if FULLGAME
        public List<ValueDropdownItem<string>> GetAllInteractableID()
        {
            return Catalog.current.GetDropdownAllID(Catalog.Category.Interactable);
        }
#endif


        protected virtual void Awake()
        {
#if FULLGAME
            if (interactableId != null && interactableId != "" && interactableId != "None")
            {
                Load(Catalog.current.GetData<InteractableData>(interactableId).Clone() as InteractableData);
            }
#endif
        }
#if FULLGAME
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
                Common.DrawGizmoArrow(Vector3.zero, Vector3.up * (axisLength / 2), Common.HueColourValue(HueColorNames.White), 0.05f);
                Common.DrawGizmoArrow(Vector3.zero, -Vector3.up * (axisLength / 2), Common.HueColourValue(HueColorNames.White), 0.05f);
                Common.DrawGizmoCapsule(Vector3.zero, Vector3.up * (axisLength / 2), Common.HueColourValue(HueColorNames.White), axisLength / 2, touchRadius);
                Common.DrawGizmoCapsule(Vector3.zero, -Vector3.up * (axisLength / 2), Common.HueColourValue(HueColorNames.White), axisLength / 2, touchRadius);
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

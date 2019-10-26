using UnityEngine;
using System.Collections.Generic;

#if ProjectCore
using Sirenix.OdinInspector;
#endif

namespace BS
{
    public class RopeDefinition : HandleDefinition
    {
        public Transform ropeStart;
        public Transform ropeTarget;
        public float ropeRadius = 0.03f;
        public Material ropeMaterial;
        public bool ropeUseCollider;
        public int ropeLayer;
        public PhysicMaterial ropePhysicMaterial;

#if ProjectCore

        protected Interactable interactable;

        public new List<ValueDropdownItem<string>> GetAllInteractableID()
        {
            return Catalog.current.GetDropdownAllID<InteractableHandle>();
        }

        protected override void Awake()
        {
            axisLength = Vector3.Distance(ropeStart.position, ropeTarget.position);
            base.Awake();
            //interactable = this.GetComponent<Interactable>();
            TubeBuilder tubeBuilder = ropeStart.gameObject.AddComponent<TubeBuilder>();
            tubeBuilder.radius = ropeRadius;
            tubeBuilder.material = ropeMaterial;
            tubeBuilder.target = ropeTarget;
            //tubeBuilder.continuousUpdate = continuousUpdate;
            tubeBuilder.useCollider = ropeUseCollider;
            tubeBuilder.physicMaterial = ropePhysicMaterial;
            tubeBuilder.layer = ropeLayer;
            tubeBuilder.Generate();
            UpdateRope();
        }

        protected virtual void UpdateRope()
        {
            axisLength = Vector3.Distance(ropeStart.position, ropeTarget.position);
            //if (interactable.touchCollider) (interactable.touchCollider as CapsuleCollider).height = axisLength;
            this.transform.position = Vector3.Lerp(ropeStart.position, ropeTarget.position, 0.5f);
            this.transform.rotation = Utils.LookRotation(ropeTarget.position - ropeStart.position, Vector3.up, Vector3.up);
        }

#endif

        protected virtual void OnDrawGizmos()
        {
            if (ropeTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(ropeStart.position, ropeTarget.position);
            }
        }
    }
}

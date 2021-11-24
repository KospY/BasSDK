using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class RopeSimple : MonoBehaviour
    {
        public Transform targetAnchor;

        [Header("Spring joint")]
        public float spring = 10000f;
        public float damper = 0;
        public float minDistance = 0;
        public float maxDistance = 0;
        public Rigidbody connectedBody;

        [Header("Rope mesh")]
        public float radius = 0.015f;
        public float tilingOffset = 10;
        public Material material;

        [Header("Audio")]
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")]
#endif
        public string effectId;
        public float audioMinForce = 400;
        public float audioMaxForce = 1000;
        public float audioMinSpeed = 0.25f;
        public float audioMaxSpeed = 2;

        private Rigidbody rb;
        private MeshRenderer mesh;
        private MaterialPropertyBlock materialPropertyBlock;
        private SpringJoint springJoint;
#if PrivateSDK
        private EffectInstance effectInstance;
#endif

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Effect);
        }
#endif

        public void Awake()
        {
            rb = this.GetComponentInParent<Rigidbody>();
            if (!rb)
            {
                Debug.LogError("RopeSimple component need to have a parent rigidbody");
                this.enabled = false;
                return;
            }

            if (!targetAnchor)
            {
                Debug.LogError("RopeSimple component doesn't have any target anchor set!");
                this.enabled = false;
                return;
            }

            // Add springjoint
            springJoint = rb.gameObject.AddComponent<SpringJoint>();
            springJoint.spring = spring;
            springJoint.damper = damper;
            springJoint.minDistance = minDistance;
            springJoint.maxDistance = maxDistance <= 0 ? Vector3.Distance(this.transform.position, targetAnchor.position) : maxDistance;
            springJoint.anchor = rb.transform.InverseTransformPoint(this.transform.position);
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.connectedAnchor = targetAnchor.position;
            springJoint.connectedBody = connectedBody;

            // Generate rope mesh
            mesh = GameObject.CreatePrimitive(PrimitiveType.Cylinder).GetComponent<MeshRenderer>();
            Collider collider = mesh.GetComponent<Collider>();
            DestroyImmediate(collider);
            mesh.transform.SetParent(this.transform);
            mesh.name = "RopeMesh";
            mesh.material = material;

            materialPropertyBlock = new MaterialPropertyBlock();
            mesh.GetPropertyBlock(materialPropertyBlock);

        }


        protected void LateUpdate()
        {
            if (rb.IsSleeping())
            {
                return;
            }

            // Update rope mesh
            mesh.transform.position = Vector3.Lerp(this.transform.position, targetAnchor.transform.position, 0.5f);
            mesh.transform.rotation = Quaternion.FromToRotation(mesh.transform.TransformDirection(Vector3.up), targetAnchor.position - this.transform.position) * mesh.transform.rotation;
            float distance = Vector3.Distance(this.transform.position, targetAnchor.position);
            mesh.transform.localScale = new Vector3(radius, distance / 2, radius);
            materialPropertyBlock.SetVector("_BaseMap_ST", new Vector4(1, distance * tilingOffset, 0, 0));
            mesh.SetPropertyBlock(materialPropertyBlock);

        }

        protected void OnDrawGizmos()
        {
            if (targetAnchor)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(this.transform.position, targetAnchor.position);
            }
        }
    }
}
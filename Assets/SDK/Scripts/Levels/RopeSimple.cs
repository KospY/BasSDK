using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/RopeSimple.html")]
    public class RopeSimple : MonoBehaviour
    {
        [Tooltip("Depicts what connects to the rope. It connects from the origin point of the transform of the Rope Simple script, to the object." +
            "\n\nFor example, the RopeSimple script is attached to a chandelier, of which the target anchor is a point on a ceiling that the chandelier is connected to.")]
        public Transform targetAnchor;

        [Header("Spring joint")]
        [Tooltip("The spring modifier of the rope. Depicts how springy the rope is")]
        public float spring = 10000f;
        [Tooltip("The damper modifier of the rope. Depicts how much of a dampener the rope has")]
        public float damper = 0;
        [Tooltip("The minimum distance of the rope. 0 means default length between the Rope Simple transform and the target anchor.")]
        public float minDistance = 0;
        [Tooltip("The maximum distance of the rope. 0 means default length between the Rope Simple transform and the target anchor.")]
        public float maxDistance = 0;
        [Tooltip("Instead of using the transform of Rope Simple, you can instead reference a specific rigidbody/item.")]
        public Rigidbody connectedBody;

        [Header("Rope mesh")]
        [Tooltip("Depicts the radius of the rope mesh.")]
        public float radius = 0.015f;
        [Tooltip("Depicts the material tiling offset.")]
        public float tilingOffset = 10;
        [Tooltip("Depicts the material that the rope uses.")]
        public Material material;

        [Header("Audio")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllEffectID))]
#endif
        [Tooltip("Depicts the Effect ID from JSON to make sounds when the rope moves")]
        public string effectId;
        [Tooltip("The minimum force required to play the Effect sound")]
        public float audioMinForce = 400;
        [Tooltip("The maximum sound as of which plays the maximum volume of the audio")]
        public float audioMaxForce = 1000;
        [Tooltip("Depicts the minimum speed the rope needs to go to play the audio")]
        public float audioMinSpeed = 0.25f;
        [Tooltip("Depicts the maximum speed the rope needs to go to play the audio at its' max volume")]
        public float audioMaxSpeed = 2;

        private Rigidbody rb;
        private MeshRenderer mesh;
        private SpringJoint springJoint;
        private Transform meshTransform;

        private static readonly int BaseMapSt = Shader.PropertyToID("_BaseMap_ST");
        private Vector4 ropeScaling = Vector4.zero;

        private LightVolumeReceiver lightVolumeReceiver;


#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Category.Effect);
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
                Debug.LogError("RopeSimple component doesn't have any target anchor set!", this);
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
            meshTransform = mesh.transform;
            meshTransform.SetParent(targetAnchor);
            mesh.name = "RopeMesh";
            mesh.material = material;

            lightVolumeReceiver = mesh.gameObject.AddComponent<LightVolumeReceiver>();
            lightVolumeReceiver.volumeDetection = LightVolumeReceiver.VolumeDetection.StaticPerMesh;

        }


        protected void LateUpdate()
        {
            if (rb.IsSleeping())
            {
                return;
            }

            // Update rope mesh
            var thisTransform = this.transform;
            var position = thisTransform.position;
            var targetAnchorPosition = targetAnchor.position;
            meshTransform.position = Vector3.Lerp(position, targetAnchorPosition, 0.5f);
            meshTransform.rotation = Quaternion.FromToRotation(mesh.transform.TransformDirection(Vector3.up), targetAnchorPosition - position) * mesh.transform.rotation;
            float distance = Vector3.Distance(position, targetAnchorPosition);
            mesh.transform.localScale = new Vector3(radius, distance * 0.5f, radius);


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
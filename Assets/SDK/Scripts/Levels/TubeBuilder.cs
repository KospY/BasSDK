using System;
using System.Collections;
using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/TubeBuilder")]
    public class TubeBuilder : MonoBehaviour
    {
        public Transform target;
        public float radius = 0.015f;
        public float tilingOffset = 10;
        public Material material;
        public int layer;

        public bool useCollider;
        public PhysicMaterial physicMaterial;

        public bool preGenerate;
        public bool continuousUpdate;

        [NonSerialized]
        public MeshRenderer tube;
        //private MaterialPropertyBlock block; //Use SRP batcher instead
        private Material materialInstance;

        protected virtual void OnValidate()
        {
            if (preGenerate) Generate();
            else ClearTube();
        }

        public virtual void ClearTube()
        {
            Transform tubeTransform = this.transform.Find("Tube");
            if (tubeTransform)
            {
                if (Application.isEditor) StartCoroutine(DestroyCoroutine(tubeTransform.gameObject));
                else Destroy(tubeTransform.gameObject);
            }
        }

        IEnumerator DestroyCoroutine(UnityEngine.Object obj)
        {
                yield return new WaitForEndOfFrame();
            DestroyImmediate(obj);
        }

        public virtual void Awake()
        {
            Generate();
        }

        public virtual void Generate()
        {
            if (!target || !material) return;
            ClearTube();
            tube = GameObject.CreatePrimitive(PrimitiveType.Cylinder).GetComponent<MeshRenderer>();
            tube.transform.SetParent(this.transform);
            tube.name = "Tube";
            tube.material = material;
            tube.gameObject.layer = layer;
            //block = new MaterialPropertyBlock(); //Use SRP batcher instead
            //tube.GetPropertyBlock(block);
            materialInstance = tube.material;

            Collider collider = tube.GetComponent<Collider>();      
            if (useCollider)
            {
                collider.material = physicMaterial;
            }
            else
            {
                StartCoroutine(DestroyCoroutine(collider));
            }

            UpdateTube();
        }

        protected virtual void Update()
        {
            if (continuousUpdate)
            {
                UpdateTube();
            }
        }

        protected virtual void UpdateTube()
        {
            if (!target) return;
            tube.transform.position = Vector3.Lerp(this.transform.position, target.transform.position, 0.5f);
            tube.transform.rotation = Quaternion.FromToRotation(tube.transform.TransformDirection(Vector3.up), target.position - this.transform.position) * tube.transform.rotation;
            float distance = Vector3.Distance(this.transform.position, target.position);
            tube.transform.localScale = new Vector3(radius, distance / 2, radius);
            //block.SetVector("_BaseMap_ST", new Vector4(1, distance * tilingOffset, 0, 0)); //Use SRP batcher instead
            //tube.SetPropertyBlock(block);
            materialInstance.SetVector("_BaseMap_ST", new Vector4(1, distance * tilingOffset, 0, 0));
        }

        protected virtual void OnDrawGizmos()
        {
            if (!preGenerate && target)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(this.transform.position, target.position);
            }
        }
    }
}
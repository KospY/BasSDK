using System;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace BS
{
    public class ColliderGroup : MonoBehaviour
    {
        [Tooltip("Set if the colliders are imbuable like a blade or crystal")]
        public ImbueMagic imbueMagic = ImbueMagic.None;
        [Tooltip("(Optional) Use a mesh instead of collider(s) to apply imbue vfx and particles effects")]
        public Renderer imbueRenderer;
        [Tooltip("(Optional) Set a renderer to use to apply imbue shader emissive effects")]
        public Renderer imbueEmissionRenderer;
        [Tooltip("Create a collision event for each collider hit (true) or for the whole group (false)")]
        public bool checkIndependently;
        [NonSerialized]
        public List<Collider> colliders;

        public enum ImbueMagic
        {
            None,
            Blade,
            Crystal,
        }

#if ProjectCore
        [NonSerialized]
        public Imbue imbue;

        protected void Awake()
        {
            colliders = new List<Collider>(this.GetComponentsInChildren<Collider>());
            if (imbueMagic != ImbueMagic.None)
            {
                imbue = this.gameObject.AddComponent<Imbue>();
            }
        }
#endif

        [Button("Generate imbue mesh")]
        public void GenerateImbueMesh()
        {
            colliders = new List<Collider>(this.GetComponentsInChildren<Collider>());

            List<CombineInstance> combines = new List<CombineInstance>();
            List<Vector3> orgScales = new List<Vector3>();

            foreach (Collider collider in colliders)
            {
                orgScales.Add(collider.transform.localScale);
                Vector3 scale = Vector3.one;
                if (collider is BoxCollider)
                {
                    scale = (collider as BoxCollider).size;
                }
                else if (collider is CapsuleCollider)
                {
                    float height = (collider as CapsuleCollider).height;
                    float radius = (collider as CapsuleCollider).radius;
                    if ((collider as CapsuleCollider).direction == 0)
                    {
                        scale = new Vector3(height, radius, radius);
                        collider.transform.localScale = new Vector3(collider.transform.localScale.x, Mathf.Max(collider.transform.localScale.y, collider.transform.localScale.z), Mathf.Max(collider.transform.localScale.y, collider.transform.localScale.z));
                    }
                    if ((collider as CapsuleCollider).direction == 1)
                    {
                        scale = new Vector3(radius, height, radius);
                        collider.transform.localScale = new Vector3(Mathf.Max(collider.transform.localScale.x, collider.transform.localScale.z), collider.transform.localScale.y, Mathf.Max(collider.transform.localScale.x, collider.transform.localScale.z));
                    }
                    if ((collider as CapsuleCollider).direction == 2)
                    {
                        scale = new Vector3(radius, radius, height);
                        collider.transform.localScale = new Vector3(Mathf.Max(collider.transform.localScale.x, collider.transform.localScale.y), Mathf.Max(collider.transform.localScale.x, collider.transform.localScale.y), collider.transform.localScale.z);
                    }
                }
                else if (collider is SphereCollider)
                {
                    float radius = (collider as SphereCollider).radius;
                    scale = new Vector3(radius, radius, radius);
                    float maxSize = Mathf.Max(collider.transform.localScale.x, collider.transform.localScale.y, collider.transform.localScale.z);
                    collider.transform.localScale = new Vector3(maxSize, maxSize, maxSize);
                }
                CombineInstance combineInstance = new CombineInstance();
                combineInstance.mesh = collider is MeshCollider ? (collider as MeshCollider).sharedMesh : GenerateCubeMesh(scale);
                combineInstance.transform = this.transform.worldToLocalMatrix * collider.transform.localToWorldMatrix;
                combines.Add(combineInstance);
            }
            Mesh imbueMesh = new Mesh();
            imbueMesh.name = "GeneratedMesh";
            imbueMesh.CombineMeshes(combines.ToArray());
            int i = 0;
            foreach (Collider collider in colliders)
            {
                collider.transform.localScale = orgScales[i];
                i++;
            }
            MeshFilter meshFilter = new GameObject("ImbueGeneratedMesh").AddComponent<MeshFilter>();
            meshFilter.transform.SetParent(this.transform);
            meshFilter.sharedMesh = imbueMesh;
            imbueRenderer = meshFilter.gameObject.AddComponent<MeshRenderer>();
        }

        private Mesh GenerateCubeMesh(Vector3 size)
        {
            Vector3 p0 = new Vector3(-size.x * .5f, -size.y * .5f, size.z * .5f);
            Vector3 p1 = new Vector3(size.x * .5f, -size.y * .5f, size.z * .5f);
            Vector3 p2 = new Vector3(size.x * .5f, -size.y * .5f, -size.z * .5f);
            Vector3 p3 = new Vector3(-size.x * .5f, -size.y * .5f, -size.z * .5f);

            Vector3 p4 = new Vector3(-size.x * .5f, size.y * .5f, size.z * .5f);
            Vector3 p5 = new Vector3(size.x * .5f, size.y * .5f, size.z * .5f);
            Vector3 p6 = new Vector3(size.x * .5f, size.y * .5f, -size.z * .5f);
            Vector3 p7 = new Vector3(-size.x * .5f, size.y * .5f, -size.z * .5f);

            Vector3[] vertices = new Vector3[]
            {
	        // Bottom
	        p0, p1, p2, p3,
	        // Left
	        p7, p4, p0, p3,
	        // Front
	        p4, p5, p1, p0,
	        // Back
	        p6, p7, p3, p2,
	        // Right
	        p5, p6, p2, p1,
	        // Top
	        p7, p6, p5, p4
            };

            int[] triangles = new int[]
            {
	        // Bottom
	        3, 1, 0,
            3, 2, 1,			
	        // Left
	        3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
            3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
	        // Front
	        3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
            3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
	        // Back
	        3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
            3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
	        // Right
	        3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
            3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
	        // Top
	        3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
            3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
            };

            Mesh mesh = new Mesh();
            mesh.name = "GeneratedCubeMesh";
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.Optimize();
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}
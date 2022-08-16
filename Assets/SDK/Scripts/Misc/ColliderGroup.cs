using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ColliderGroup")]
    public class ColliderGroup : MonoBehaviour
    {
        [Tooltip("(Optional) Use a mesh instead of collider(s) to apply imbue vfx and particles effects")]
        public Renderer imbueEffectRenderer;
        [Tooltip("(Optional) Set a renderer to use to apply imbue shader emissive effects")]
        public Renderer imbueEmissionRenderer;
        [Tooltip("Spawn position and direction of projectiles/spells")]
        public Transform imbueShoot;
        [Tooltip("Position used to calculate speed for whoosh effects")]
        public Transform whooshPoint;

        [Tooltip("Custom imbue effect")]
        public FxController imbueCustomFxController;
        [Tooltip("Allowed spell ID for custom imbue effect")]
        public string imbueCustomSpellID;

        [NonSerialized]
        public List<Collider> colliders;
        [NonSerialized]
        public HashSet<Collider> colliderSet;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
#if PrivateSDK
        [NonSerialized]
        public ColliderGroupData data;
#endif
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [NonSerialized]
        public CollisionHandler collisionHandler;
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
#if PrivateSDK
        [NonSerialized]
        public ColliderGroupData.Modifier modifier;
#endif

        [Button("Generate imbue mesh")]
        public void GenerateImbueMesh()
        {
            colliders = new List<Collider>(this.GetComponentsInChildren<Collider>());
            colliderSet = new HashSet<Collider>(colliders);
            List<CombineInstance> combines = new List<CombineInstance>();
            List<Vector3> orgScales = new List<Vector3>();

            foreach (Collider collider in colliders)
            {
                CombineInstance combineInstance = new CombineInstance();
                orgScales.Add(collider.transform.localScale);
                if (collider is BoxCollider)
                {
                    combineInstance.mesh = GenerateCubeMesh((collider as BoxCollider).size);
                    combineInstance.transform = this.transform.worldToLocalMatrix * (collider.transform.localToWorldMatrix * Matrix4x4.Translate((collider as BoxCollider).center));
                }
                else if (collider is CapsuleCollider)
                {
                    float height = (collider as CapsuleCollider).height;
                    float radius = (collider as CapsuleCollider).radius;
                    Vector3 scale = Vector3.one;
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
                    combineInstance.mesh = GenerateCubeMesh(scale);
                    combineInstance.transform = this.transform.worldToLocalMatrix * (collider.transform.localToWorldMatrix * Matrix4x4.Translate((collider as CapsuleCollider).center));
                }
                else if (collider is SphereCollider)
                {
                    float maxSize = Mathf.Max(collider.transform.localScale.x, collider.transform.localScale.y, collider.transform.localScale.z);
                    collider.transform.localScale = new Vector3(maxSize, maxSize, maxSize);
                    combineInstance.mesh = GenerateIcoSphereMesh(4, (collider as SphereCollider).radius);
                    Matrix4x4 transformMatrix = Matrix4x4.Translate((collider as SphereCollider).center) * Matrix4x4.Scale(new Vector3(1 / collider.transform.lossyScale.x, 1 / collider.transform.lossyScale.y, 1 / collider.transform.lossyScale.z));
                    combineInstance.transform = this.transform.worldToLocalMatrix * (collider.transform.localToWorldMatrix * transformMatrix);
                }
                else if (collider is MeshCollider)
                {
                    combineInstance.mesh = (collider as MeshCollider).sharedMesh;
                    combineInstance.transform = this.transform.worldToLocalMatrix * collider.transform.localToWorldMatrix;
                }
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
            meshFilter.transform.localPosition = Vector3.zero;
            meshFilter.transform.localRotation = Quaternion.identity;
            meshFilter.transform.localScale = Vector3.one;
            meshFilter.sharedMesh = imbueMesh;
            //creates a directory and saves the mesh as an asset
            int meshNumber = 1;

#if (UNITY_EDITOR)
            if (!Application.isPlaying)
            {
                System.IO.Directory.CreateDirectory("Assets/Private/Generated Meshes");
                string path = Application.dataPath + "/Private/Generated Meshes/ImbueGeneratedMesh" + name + meshNumber;
                if (System.IO.File.Exists(path))
                {
                    Debug.Log("File ImbueGeneratedMesh" + name + " already exists. Renaming current ImbueGeneratedMesh.");
                    while (System.IO.File.Exists(Application.dataPath + "/Private/Generated Meshes/ImbueGeneratedMesh" + name + meshNumber))
                    {
                        meshNumber++;
                    }
                }

                AssetDatabase.CreateAsset(imbueMesh, "Assets/Private/Generated Meshes/ImbueGeneratedMesh" + name + meshNumber);
            }
#endif
            imbueEffectRenderer = meshFilter.gameObject.AddComponent<MeshRenderer>();
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

        public static Mesh GenerateIcoSphereMesh(int n, float radius)
        {
            int nn = n * 4;
            int vertexNum = (nn * nn / 16) * 24;
            Vector3[] vertices = new Vector3[vertexNum];
            int[] triangles = new int[vertexNum];
            Vector2[] uv = new Vector2[vertexNum];

            Quaternion[] init_vectors = new Quaternion[24];
            // 0
            init_vectors[0] = new Quaternion(0, 1, 0, 0);   //the triangle vertical to (1,1,1)
            init_vectors[1] = new Quaternion(0, 0, 1, 0);
            init_vectors[2] = new Quaternion(1, 0, 0, 0);
            // 1
            init_vectors[3] = new Quaternion(0, -1, 0, 0);  //to (1,-1,1)
            init_vectors[4] = new Quaternion(1, 0, 0, 0);
            init_vectors[5] = new Quaternion(0, 0, 1, 0);
            // 2
            init_vectors[6] = new Quaternion(0, 1, 0, 0);   //to (-1,1,1)
            init_vectors[7] = new Quaternion(-1, 0, 0, 0);
            init_vectors[8] = new Quaternion(0, 0, 1, 0);
            // 3
            init_vectors[9] = new Quaternion(0, -1, 0, 0);  //to (-1,-1,1)
            init_vectors[10] = new Quaternion(0, 0, 1, 0);
            init_vectors[11] = new Quaternion(-1, 0, 0, 0);
            // 4
            init_vectors[12] = new Quaternion(0, 1, 0, 0);  //to (1,1,-1)
            init_vectors[13] = new Quaternion(1, 0, 0, 0);
            init_vectors[14] = new Quaternion(0, 0, -1, 0);
            // 5
            init_vectors[15] = new Quaternion(0, 1, 0, 0); //to (-1,1,-1)
            init_vectors[16] = new Quaternion(0, 0, -1, 0);
            init_vectors[17] = new Quaternion(-1, 0, 0, 0);
            // 6
            init_vectors[18] = new Quaternion(0, -1, 0, 0); //to (-1,-1,-1)
            init_vectors[19] = new Quaternion(-1, 0, 0, 0);
            init_vectors[20] = new Quaternion(0, 0, -1, 0);
            // 7
            init_vectors[21] = new Quaternion(0, -1, 0, 0);  //to (1,-1,-1)
            init_vectors[22] = new Quaternion(0, 0, -1, 0);
            init_vectors[23] = new Quaternion(1, 0, 0, 0);

            int j = 0;  //index on vectors[]

            for (int i = 0; i < 24; i += 3)
            {
                /*
                 *                   c _________d
                 *    ^ /\           /\        /
                 *   / /  \         /  \      /
                 *  p /    \       /    \    /
                 *   /      \     /      \  /
                 *  /________\   /________\/
                 *     q->       a         b
                 */
                for (int p = 0; p < n; p++)
                {
                    //edge index 1
                    Quaternion edge_p1 = Quaternion.Lerp(init_vectors[i], init_vectors[i + 2], (float)p / n);
                    Quaternion edge_p2 = Quaternion.Lerp(init_vectors[i + 1], init_vectors[i + 2], (float)p / n);
                    Quaternion edge_p3 = Quaternion.Lerp(init_vectors[i], init_vectors[i + 2], (float)(p + 1) / n);
                    Quaternion edge_p4 = Quaternion.Lerp(init_vectors[i + 1], init_vectors[i + 2], (float)(p + 1) / n);

                    for (int q = 0; q < (n - p); q++)
                    {
                        //edge index 2
                        Quaternion a = Quaternion.Lerp(edge_p1, edge_p2, (float)q / (n - p));
                        Quaternion b = Quaternion.Lerp(edge_p1, edge_p2, (float)(q + 1) / (n - p));
                        Quaternion c, d;
                        if (edge_p3 == edge_p4)
                        {
                            c = edge_p3;
                            d = edge_p3;
                        }
                        else
                        {
                            c = Quaternion.Lerp(edge_p3, edge_p4, (float)q / (n - p - 1));
                            d = Quaternion.Lerp(edge_p3, edge_p4, (float)(q + 1) / (n - p - 1));
                        }

                        triangles[j] = j;
                        vertices[j++] = new Vector3(a.x, a.y, a.z);
                        triangles[j] = j;
                        vertices[j++] = new Vector3(b.x, b.y, b.z);
                        triangles[j] = j;
                        vertices[j++] = new Vector3(c.x, c.y, c.z);
                        if (q < n - p - 1)
                        {
                            triangles[j] = j;
                            vertices[j++] = new Vector3(c.x, c.y, c.z);
                            triangles[j] = j;
                            vertices[j++] = new Vector3(b.x, b.y, b.z);
                            triangles[j] = j;
                            vertices[j++] = new Vector3(d.x, d.y, d.z);
                        }
                    }
                }
            }
            Mesh mesh = new Mesh();
            mesh.name = "IcoSphere";

            for (int i = 0; i < vertexNum; i++)
            {
                vertices[i] *= radius;
            }
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}
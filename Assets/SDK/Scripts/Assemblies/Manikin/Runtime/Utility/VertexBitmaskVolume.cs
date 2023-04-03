#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    public class VertexBitmaskVolume : MonoBehaviour
    {
        public enum UVChannel
        {
            UV0,
            UV1,
            UV2,
            UV3
        }
        public UVChannel uvChannel = UVChannel.UV1;

        public enum VectorChannel
        {
            X,
            Y,
            Z,
            W
        }
        public VectorChannel vectorChannel;

        public enum VolumeType
        {
            Sphere,
            Box
        }
        public VolumeType volumeType;

        [Range(0.01f, 3f)]
        public float radius;

        public Vector3 position;
        public Vector3 rotation;

        public Vector3 size;

        public int bitmaskValue;

        private Vector3 zero = Vector3.zero;
        private Vector3 one = Vector3.one;

        private void OnDrawGizmos()
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.TransformPoint(position), Quaternion.Euler(rotation), one);
            switch (volumeType)
            {
                case VolumeType.Sphere:
                    Gizmos.DrawWireSphere(zero, radius);
                    break;
                case VolumeType.Box:
                    Gizmos.DrawWireCube(zero, size);
                    break;
            }
        }

        public bool ContainsPoint(Vector3 point)
        {
            switch (volumeType)
            {
                case VolumeType.Sphere:
                    Vector3 len = transform.TransformPoint(position) - point;
                    return len.sqrMagnitude < (radius * radius);
                case VolumeType.Box:
                    point = transform.InverseTransformPoint(point);

                    Matrix4x4 mat = Matrix4x4.TRS(position, Quaternion.Euler(rotation), one);
                    mat = mat.inverse;
                    point = mat.MultiplyPoint(point);
                    

                    if(!(point.x >= (-size.x * 0.5f) && point.x <= (size.x * 0.5f)))
                    {
                        return false;
                    }
                    if (!(point.y >= (-size.y * 0.5f) && point.y <= (size.y * 0.5f)))
                    {
                        return false;
                    }
                    if (!(point.z >= (-size.z * 0.5f) && point.z <= (size.z * 0.5f)))
                    {
                        return false;
                    }

                    return true;
            }

            return false;
        }

        [UnityEditor.MenuItem("GameObject/Manikin/UpdateAllBitmaskVolumes")]
        public static void UpdateAllBitmaskVolumes()
        {
            if(UnityEditor.Selection.activeGameObject != null)
            {
                if (!UnityEditor.EditorUtility.DisplayDialog("Warning", "This will overwrite existing UV data. Are you sure?", "OK", "Cancel"))
                    return;

                UnityEditor.EditorUtility.DisplayProgressBar("Updating data", "test", 0f);
                VertexBitmaskVolume[] volumes = UnityEditor.Selection.activeGameObject.GetComponentsInChildren<VertexBitmaskVolume>();
                if (volumes == null)
                    return;

                bool[] uvCounter = new bool[4];
                int uvChannel = 1;
                bool[] vectorCounter = new bool[4];
                VectorChannel vectorChannel = VectorChannel.X;
                foreach (VertexBitmaskVolume volume in volumes)
                {
                    switch (volume.uvChannel)
                    {
                        case UVChannel.UV0:
                            uvCounter[0] = true;
                            uvChannel = 0;
                            if (uvCounter[1] || uvCounter[2] || uvCounter[3])
                            {
                                Debug.LogError("Multiple different UV Channel found!");
                                return;
                            }
                            break;
                        case UVChannel.UV1:
                            uvCounter[1] = true;
                            uvChannel = 1;
                            if (uvCounter[0] || uvCounter[2] || uvCounter[3])
                            {
                                Debug.LogError("Multiple different UV Channel found!");
                                return;
                            }
                            break;
                        case UVChannel.UV2:
                            uvCounter[2] = true;
                            uvChannel = 2;
                            if (uvCounter[0] || uvCounter[1] || uvCounter[3])
                            {
                                Debug.LogError("Multiple different UV Channel found!");
                                return;
                            }
                            break;
                        case UVChannel.UV3:
                            uvCounter[3] = true;
                            uvChannel = 3;
                            if (uvCounter[0] || uvCounter[1] || uvCounter[2])
                            {
                                Debug.LogError("Multiple different UV Channel found!");
                                return;
                            }
                            break;
                    }

                    switch(volume.vectorChannel)
                    {
                        case VectorChannel.X:
                            vectorCounter[0] = true;
                            vectorChannel = VectorChannel.X;
                            if (vectorCounter[1] || vectorCounter[2] || vectorCounter[3])
                            {
                                Debug.LogError("Multiple different Vector Channel found!");
                                return;
                            }
                            break;
                        case VectorChannel.Y:
                            vectorCounter[1] = true;
                            vectorChannel = VectorChannel.Y;
                            if (vectorCounter[0] || vectorCounter[2] || vectorCounter[3])
                            {
                                Debug.LogError("Multiple different Vector Channel found!");
                                return;
                            }
                            break;
                        case VectorChannel.Z:
                            vectorCounter[2] = true;
                            vectorChannel = VectorChannel.Z;
                            if (vectorCounter[0] || vectorCounter[1] || vectorCounter[3])
                            {
                                Debug.LogError("Multiple different Vector Channel found!");
                                return;
                            }
                            break;
                        case VectorChannel.W:
                            vectorCounter[3] = true;
                            vectorChannel = VectorChannel.W;
                            if (vectorCounter[0] || vectorCounter[1] || vectorCounter[2])
                            {
                                Debug.LogError("Multiple different Vector Channel found!");
                                return;
                            }
                            break;
                    }
                }

                MeshFilter[] filters = UnityEditor.Selection.activeGameObject.GetComponentsInChildren<MeshFilter>();
                SkinnedMeshRenderer[] smrs = UnityEditor.Selection.activeGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

                Mesh[] filterMeshes = new Mesh[filters.Length];
                for(int i = 0; i < filters.Length; i++)
                {
                    filterMeshes[i] = Instantiate<Mesh>(filters[i].sharedMesh);
                    Vector3[] verts = filterMeshes[i].vertices;
                    for(int v = 0; v < verts.Length; v++)
                    {
                        verts[v] = filters[i].transform.TransformPoint(verts[v]);
                    }
                    filterMeshes[i].vertices = verts;
                }

                Mesh[] smrMeshes = new Mesh[smrs.Length];
                for(int i = 0; i < smrs.Length; i++)
                {
                    smrMeshes[i] = new Mesh();
                    smrs[i].BakeMesh(smrMeshes[i]);
                    Vector3[] smrVerts = smrMeshes[i].vertices;
                    for(int v = 0; v < smrVerts.Length; v++)
                    {
                        smrVerts[v] = smrs[i].transform.TransformPoint(smrVerts[v]);
                    }
                    smrMeshes[i].vertices = smrVerts;
                }

                int totalMeshes = filters.Length + smrs.Length;
                for (int index = 0; index < filterMeshes.Length; index++)
                {
                    UnityEditor.EditorUtility.DisplayProgressBar("Updating data", filterMeshes[index].name, index / totalMeshes);
                    Vector4[] uvData = CalculateUVData(filterMeshes[index], volumes, vectorChannel);

                    if (uvData.Length > 0)
                    {
                        filters[index].sharedMesh.SetUVs(uvChannel, uvData);
                    }
                }

                for(int index = 0; index < smrMeshes.Length; index++)
                {
                    UnityEditor.EditorUtility.DisplayProgressBar("Updating data", smrMeshes[index].name, (index + filterMeshes.Length) / totalMeshes);
                    Vector4[] uvData = CalculateUVData(smrMeshes[index], volumes, vectorChannel);

                    if (uvData.Length > 0)
                    {
                        smrs[index].sharedMesh.SetUVs(uvChannel, uvData);
                    }
                }

                UnityEditor.EditorUtility.ClearProgressBar();
                Debug.Log("Vertex bitmask update Complete");
            }
        }

        private static Vector4[] CalculateUVData(Mesh mesh, VertexBitmaskVolume[] volumes, VectorChannel vectorChannel)
        {
            Vector3[] vertices = mesh.vertices;
            Vector4[] uvData = new Vector4[vertices.Length];

            foreach (VertexBitmaskVolume volume in volumes)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    if (volume.ContainsPoint(vertices[i]))
                    {
                        switch(vectorChannel)
                        {
                            case VectorChannel.X:
                                uvData[i].x = (int)uvData[i].x | volume.bitmaskValue;
                                break;
                            case VectorChannel.Y:
                                uvData[i].y = (int)uvData[i].y | volume.bitmaskValue;
                                break;
                            case VectorChannel.Z:
                                uvData[i].z = (int)uvData[i].z | volume.bitmaskValue;
                                break;
                            case VectorChannel.W:
                                uvData[i].w = (int)uvData[i].w | volume.bitmaskValue;
                                break;
                        }
                    }
                }
            }

            if (uvData.Length > 0)
            {
                return uvData;
            }
            return null;
        }
    }
}
#endif

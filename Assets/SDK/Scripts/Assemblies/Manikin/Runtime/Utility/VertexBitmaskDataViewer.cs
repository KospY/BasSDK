#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Manikin
{
    /// <summary>
    /// This editor only component can be used to view the vertices on an attached renderer.
    /// It will highlight any vertices in red that have a corresponding bit set from the supplied bitmasks on this component.
    /// It'll look for that bitmask data in the selected UV channel.
    /// </summary>
    public class VertexBitmaskDataViewer : MonoBehaviour
    {
        public enum VectorChannel
        {
            X,
            Y,
            Z,
            W
        }

        public enum UVChannel
        {
            UV0,
            UV1,
            UV2,
            UV3
        }

        public bool showVertices = true;
        public int bitmask;
        [Range(0.001f, 0.05f)]
        public float size = 0.01f;

        [Tooltip("This is the UV channel to for bitmask data to compare with the supplied bitmask on this component.")]
        public UVChannel uvChannel = UVChannel.UV1;

        public VectorChannel vectorChannel = VectorChannel.X;

        private MeshFilter meshFilter;
        [HideInInspector, NonSerialized]
        public SkinnedMeshRenderer skinnedMeshRenderer;
        [HideInInspector, NonSerialized]
        public Vector3[] vertices;
        [HideInInspector, NonSerialized]
        public List<Vector4> uvs = new List<Vector4>();

        private void OnValidate()
        {
            if(meshFilter == null) { meshFilter = GetComponent<MeshFilter>(); }
            if(skinnedMeshRenderer == null ) { skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>(); }

            if(meshFilter != null)
            {
                vertices = meshFilter.sharedMesh.vertices;
                meshFilter.sharedMesh.GetUVs((int)uvChannel, uvs);
            }
            else
            {
                if(skinnedMeshRenderer != null)
                {
                    Mesh mesh = new Mesh();
                    skinnedMeshRenderer.BakeMesh(mesh);//.sharedMesh.vertices;
                    vertices = mesh.vertices;
                    float scale = 1f / skinnedMeshRenderer.transform.localScale.y;
                    for(int i = 0; i < vertices.Length; i++)
                    {
                        vertices[i] *= scale;
                    }

                    skinnedMeshRenderer.sharedMesh.GetUVs((int)uvChannel, uvs);

                    DestroyImmediate(mesh, false);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (vertices == null)
            {
                return;
            }

            if (showVertices)
            {
                Gizmos.color = Color.white;
                Vector3 cubeSize = new Vector3(size, size, size);

                for (int i = 0; i < vertices.Length; i++)
                {
                    if (uvs != null && uvs.Count > 0)
                    {
                        Gizmos.color = Color.white;
                        int value = 0;
                        switch (vectorChannel)
                        {
                            case VectorChannel.X:
                                value = (int)uvs[i].x;
                                break;
                            case VectorChannel.Y:
                                value = (int)uvs[i].y;
                                break;
                            case VectorChannel.Z:
                                value = (int)uvs[i].z;
                                break;
                            case VectorChannel.W:
                                value = (int)uvs[i].w;
                                break;
                        }

                        if ((value & bitmask) != 0)
                        {
                            Gizmos.color = Color.red;
                        }
                    }
                    //Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), sphereRadius);
                    Gizmos.DrawCube(transform.TransformPoint(vertices[i]), cubeSize); //drawing cubes is much faster than spheres.
                }
            }
        }
    }
}
#endif
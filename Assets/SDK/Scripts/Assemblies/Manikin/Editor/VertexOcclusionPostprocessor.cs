using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace ThunderRoad.Manikin
{
	
    public class VertexOcclusionPostprocessor : AssetPostprocessor
    {
        Dictionary<Material, int> bitmaskMaterials = new Dictionary<Material, int>();
        List<string> propertyNames = new List<string>();

        public void OnPreprocessMaterialDescription(MaterialDescription description, Material material, AnimationClip[] materialAnimation)
        {
            Debug.Log("AP: VertexOcclusionPostprocessor: OnPreprocessMaterialDescription");
            string propertyName = "";
            propertyNames.Clear();
            description.GetFloatPropertyNames(propertyNames);
            foreach (string n in propertyNames)
            {
                if(n.ToLower().Equals("bitmask"))
                {
                    propertyName = n;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(propertyName))
            {
                if (description.TryGetProperty(propertyName, out float val))
                {
                    Debug.Log("Material with Bitmask found: " + description.materialName + " " + val);
                    bitmaskMaterials.Add(material, (int)val);
                }
            }
        }

        void OnPostprocessModel(GameObject root)
        {
            Debug.Log("AP: VertexOcclusionPostprocessor: OnPostprocessModel");
            if (bitmaskMaterials.Count > 0)
            {
                var settings = ManikinSettingsProvider.GetSettings();
                if (settings == null)
                {
                    Debug.LogError("No Manikin settings found!");
                    return;
                }

                SkinnedMeshRenderer[] renderers = root.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (SkinnedMeshRenderer renderer in renderers)
                {
                    bool hasBitmask = false;
                    Material[] materials = renderer.sharedMaterials;
                    List<Material> finalMaterials = new List<Material>();
                    for (int submeshIndex = 0; submeshIndex < materials.Length; submeshIndex++)
                    {
                        if (bitmaskMaterials.ContainsKey(materials[submeshIndex]))
                        {
                            hasBitmask = true;
                        }
                        else
                        {
                            finalMaterials.Add(materials[submeshIndex]);
                        }
                    }

                    if (!hasBitmask)
                        continue;

                    List<Vector2> data2 = new List<Vector2>();
                    List<Vector3> data3 = new List<Vector3>();
                    List<Vector4> data4 = new List<Vector4>();
                    List<Color> colorData = new List<Color>();

                    if (settings.vertexOcclusionVertexChannel == ManikinSettingsProvider.VertexChannel.Color)
                    {
                        renderer.sharedMesh.GetColors(colorData);
                        if (colorData == null)
                        {
                            colorData = new List<Color>(renderer.sharedMesh.vertexCount);
                        }
                        colorData.AddRange(new Color[renderer.sharedMesh.vertexCount]);
                    }
                    else
                    {
                        switch (settings.vertexOcclusionVectorSize)
                        {
                            case ManikinSettingsProvider.VectorSize.Vector2:
                                renderer.sharedMesh.GetUVs((int)settings.vertexOcclusionVertexChannel, data2);
                                if (data2 == null)
                                {
                                    data2 = new List<Vector2>(renderer.sharedMesh.vertexCount);
                                }
                                data2.AddRange(new Vector2[renderer.sharedMesh.vertexCount]);
                                break;
                            case ManikinSettingsProvider.VectorSize.Vector3:
                                renderer.sharedMesh.GetUVs((int)settings.vertexOcclusionVertexChannel, data3);
                                if (data3 == null)
                                {
                                    data3 = new List<Vector3>(renderer.sharedMesh.vertexCount);
                                }
                                data3.AddRange(new Vector3[renderer.sharedMesh.vertexCount]);
                                break;
                            case ManikinSettingsProvider.VectorSize.Vector4:
                                renderer.sharedMesh.GetUVs((int)settings.vertexOcclusionVertexChannel, data4);
                                if (data4 == null)
                                {
                                    data4 = new List<Vector4>(renderer.sharedMesh.vertexCount);
                                }
                                data4.AddRange(new Vector4[renderer.sharedMesh.vertexCount]);
                                break;
                        }
                    }

                    List<int> allBitmaskTris = new List<int>();
                    List<List<int>> regularTris = new List<List<int>>();
                    int bitmaskSubmeshCount = 0;
                    for (int submeshIndex = 0; submeshIndex < renderer.sharedMesh.subMeshCount; submeshIndex++)
                    {
                        if (bitmaskMaterials.TryGetValue(materials[submeshIndex], out int bitmask))
                        {
                            int[] tris = renderer.sharedMesh.GetTriangles(submeshIndex);
                            allBitmaskTris.AddRange(tris);
                            bitmaskSubmeshCount++;

                            for (int triIndex = 0; triIndex < tris.Length; triIndex++)
                            {
                                Color color = Color.clear;
                                Vector2 v2 = Vector2.zero;
                                Vector3 v3 = Vector3.zero;
                                Vector4 v4 = Vector4.zero;

                                switch (settings.vertexOcclusionVectorChannel)
                                {
                                    case ManikinSettingsProvider.VectorChannel.X:
                                        if (settings.vertexOcclusionVertexChannel == ManikinSettingsProvider.VertexChannel.Color)
                                        {
                                            color.r = (int)(colorData[tris[triIndex]].r) | bitmask;
                                            colorData[tris[triIndex]] = color;
                                        }
                                        else
                                        {
                                            switch (settings.vertexOcclusionVectorSize)
                                            {
                                                case ManikinSettingsProvider.VectorSize.Vector2:
                                                    v2.x = (int)(data2[tris[triIndex]].x) | bitmask;
                                                    data2[tris[triIndex]] = v2;
                                                    break;
                                                case ManikinSettingsProvider.VectorSize.Vector3:
                                                    v2.x = (int)(data2[tris[triIndex]].x) | bitmask;
                                                    data3[tris[triIndex]] = v2;
                                                    break;
                                                case ManikinSettingsProvider.VectorSize.Vector4:
                                                    v2.x = (int)(data2[tris[triIndex]].x) | bitmask;
                                                    data4[tris[triIndex]] = v2;
                                                    break;
                                            }
                                        }
                                        break;
                                    case ManikinSettingsProvider.VectorChannel.Y:
                                        if (settings.vertexOcclusionVertexChannel == ManikinSettingsProvider.VertexChannel.Color)
                                        {
                                            color.g = (int)(colorData[tris[triIndex]].g) | bitmask;
                                            colorData[tris[triIndex]] = color;
                                        }
                                        else
                                        {
                                            switch (settings.vertexOcclusionVectorSize)
                                            {
                                                case ManikinSettingsProvider.VectorSize.Vector2:
                                                    v2.y = (int)(data2[tris[triIndex]].y) | bitmask;
                                                    data2[tris[triIndex]] = v2;
                                                    break;
                                                case ManikinSettingsProvider.VectorSize.Vector3:
                                                    v2.y = (int)(data2[tris[triIndex]].y) | bitmask;
                                                    data3[tris[triIndex]] = v2;
                                                    break;
                                                case ManikinSettingsProvider.VectorSize.Vector4:
                                                    v2.y = (int)(data2[tris[triIndex]].y) | bitmask;
                                                    data4[tris[triIndex]] = v2;
                                                    break;
                                            }
                                        }
                                        break;
                                    case ManikinSettingsProvider.VectorChannel.Z:
                                        if (settings.vertexOcclusionVertexChannel == ManikinSettingsProvider.VertexChannel.Color)
                                        {
                                            color.b = (int)(colorData[tris[triIndex]].b) | bitmask;
                                            colorData[tris[triIndex]] = color;
                                        }
                                        else
                                        {
                                            switch (settings.vertexOcclusionVectorSize)
                                            {
                                                case ManikinSettingsProvider.VectorSize.Vector2:
                                                    Debug.LogError("Undefined settings");
                                                    break;
                                                case ManikinSettingsProvider.VectorSize.Vector3:
                                                    v3.z = (int)(data3[tris[triIndex]].z) | bitmask;
                                                    data3[tris[triIndex]] = v3;
                                                    break;
                                                case ManikinSettingsProvider.VectorSize.Vector4:
                                                    v3.z = (int)(data3[tris[triIndex]].z) | bitmask;
                                                    data4[tris[triIndex]] = v3;
                                                    break;
                                            }
                                        }
                                        break;
                                    case ManikinSettingsProvider.VectorChannel.W:
                                        if (settings.vertexOcclusionVertexChannel == ManikinSettingsProvider.VertexChannel.Color)
                                        {
                                            color.a = (int)(colorData[tris[triIndex]].a) | bitmask;
                                            colorData[tris[triIndex]] = color;
                                        }
                                        else
                                        {
                                            switch (settings.vertexOcclusionVectorSize)
                                            {
                                                case ManikinSettingsProvider.VectorSize.Vector2:
                                                    Debug.LogError("Undefined settings");
                                                    break;
                                                case ManikinSettingsProvider.VectorSize.Vector3:
                                                    Debug.LogError("Undefined settings");
                                                    break;
                                                case ManikinSettingsProvider.VectorSize.Vector4:
                                                    v4.w = (int)(data4[tris[triIndex]].w) | bitmask;
                                                    data4[tris[triIndex]] = v4;
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            regularTris.Add(new List<int>());
                            regularTris[regularTris.Count - 1].AddRange(renderer.sharedMesh.GetTriangles(submeshIndex));
                        }
                    }

                    if (settings.vertexOcclusionVertexChannel == ManikinSettingsProvider.VertexChannel.Color)
                    {
                        renderer.sharedMesh.SetColors(colorData);
                    }
                    else
                    {
                        switch (settings.vertexOcclusionVectorSize)
                        {
                            case ManikinSettingsProvider.VectorSize.Vector2:
                                renderer.sharedMesh.SetUVs((int)settings.vertexOcclusionVertexChannel, data2);
                                break;
                            case ManikinSettingsProvider.VectorSize.Vector3:
                                renderer.sharedMesh.SetUVs((int)settings.vertexOcclusionVertexChannel, data3);
                                break;
                            case ManikinSettingsProvider.VectorSize.Vector4:
                                renderer.sharedMesh.SetUVs((int)settings.vertexOcclusionVertexChannel, data4);
                                break;
                        }
                    }

                    if (regularTris.Count > 0)
                    {
                        regularTris[0].AddRange(allBitmaskTris);
                        for (int i = 0; i < regularTris.Count; i++)
                        {
                            renderer.sharedMesh.SetTriangles(regularTris[i], i);
                        }
                        renderer.sharedMesh.subMeshCount = regularTris.Count;
                        renderer.sharedMaterials = finalMaterials.ToArray();
                    }

                }
            }
        }
    }
}
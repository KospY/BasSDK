using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThunderRoad
{
    public class MeshBatcher
    {

        // Calculate the Z-order curve (Morton code) from the bounds center
        public static int CalculateMortonCodeFromBounds(Bounds bounds, float scale)
        {
            Vector3 center = bounds.center * scale;

            // Convert the bounds center to integer values
            int x = Mathf.RoundToInt(center.x);
            int y = Mathf.RoundToInt(center.y);
            int z = Mathf.RoundToInt(center.z);

            // Calculate the Morton code using bit interleaving
            return Morton3D(x, y, z);
        }

        // Interleave the bits for the Morton Code (Z-order curve)
        public static int Morton3D(int x, int y, int z)
        {
            return (Part1By2(x) << 2) + (Part1By2(y) << 1) + Part1By2(z);
        }

        // Helper function to interleave bits of an integer
        public static int Part1By2(int n)
        {
            n = (n | (n << 16)) & 0x030000FF;
            n = (n | (n << 8)) & 0x0300F00F;
            n = (n | (n << 4)) & 0x030C30C3;
            n = (n | (n << 2)) & 0x09249249;
            return n;
        }
        
        public static float CalculateScaleFactor(Bounds worldBounds, float targetRange = 10f)
        {
            // Calculate the size of the world in each dimension
            float worldSizeX = worldBounds.size.x;
            float worldSizeY = worldBounds.size.y;
            float worldSizeZ = worldBounds.size.z;
    
            // Find the largest dimension of the world
            float maxWorldSize = Mathf.Max(worldSizeX, worldSizeY, worldSizeZ);
    
            // Calculate the scale factor to fit the largest world dimension into the target range
            float scaleFactor = targetRange / maxWorldSize;
    
            return scaleFactor;
        }
        
        public static List<MeshInfo> CreateStaticBatchGroup(MeshRenderer[] meshRenderers)
        {
            List<MeshInfo> meshInfos = new List<MeshInfo>();
#if UNITY_EDITOR            
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                MeshRenderer meshRenderer = meshRenderers[i];
                if (meshRenderer == null) continue;
                GameObject go = meshRenderer.gameObject;
                //dont static batch things we want to skip
                if(meshRenderer.gameObject.CompareTag("NoRoomStaticBatching")) continue;
                //dont static batch books/uis
                if(meshRenderer.gameObject.CompareTag("PointerActive")) continue;
                //static batch if its marked static, baked, or has the tag
                if (GameObjectUtility.AreStaticEditorFlagsSet(go, StaticEditorFlags.BatchingStatic) || go.CompareTag("RoomStaticBatch"))
                {
                    if (!go.TryGetComponent(out MeshFilter meshFilter)) continue;
                    if (meshFilter.sharedMesh == null) continue;
                    if (meshRenderer.sharedMaterials.Length == 0) continue;
                    if(meshRenderer.sharedMaterial == null) continue;
                    if(meshRenderer.sharedMaterial.shader == null) continue;
                    MeshInfo meshInfo = new MeshInfo(meshRenderer, meshFilter);
                    meshInfos.Add(meshInfo);
                }
            }
#endif               
            return meshInfos;
        }
        public class StaticBatchGroup
        {
            public int vertexCount;
            public List<MeshInfo> meshInfos = new List<MeshInfo>();
            private List<GameObject> gameObjects = new List<GameObject>();
            public bool AddMeshInfo(MeshInfo meshInfo)
            {
                if (vertexCount + meshInfo.VertexCount > 64000) return false;
                meshInfos.Add(meshInfo);
                gameObjects.Add(meshInfo.Renderer.gameObject);
                vertexCount += meshInfo.VertexCount;
                return true;
            }
            public void StaticBatch()
            {
#if UNITY_EDITOR
//                Debug.Log($"[ThunderBatcher] Static batching [Obj:{gameObjects.Count}][Vert:{vertexCount}]");
#endif
                GameObject[] gameObjectsArray = gameObjects.ToArray();
                StaticBatchingUtility.Combine(gameObjectsArray, gameObjectsArray[0]);
            }

            public void RemoveStaticBatchFlag()
            {
#if UNITY_EDITOR
                foreach (GameObject go in gameObjects)
                {
                    GameObjectUtility.SetStaticEditorFlags(go, GameObjectUtility.GetStaticEditorFlags(go) & ~StaticEditorFlags.BatchingStatic);
                }
#endif
            }
        }
        public static Dictionary<int, List<StaticBatchGroup>> CreateStaticBatchGroups(List<MeshInfo> meshInfos)
        {
            //group the meshinfos together by hashcode, up to a max of 64k vertices
            Dictionary<int, List<StaticBatchGroup>> meshGroups = new();
            int meshInfoCount = meshInfos.Count;
            for (int i = 0; i < meshInfoCount; i++)
            {
                var meshInfo = meshInfos[i];
                //get the hashcode for the meshinfo
                int hashCode = meshInfo.GetHashCode();
                if (!meshGroups.TryGetValue(hashCode, out List<StaticBatchGroup> groups))
                {
                    groups = new List<StaticBatchGroup>();
                    meshGroups.Add(hashCode, groups);
                }
                //get the last group otherwise create a new one
                StaticBatchGroup group = groups.Count > 0 ? groups[^1] : null;
                //if we cant add the meshinfo to the group, create a new one and add it
                if (group == null || !group.AddMeshInfo(meshInfo))
                {
                    group = new StaticBatchGroup();
                    group.AddMeshInfo(meshInfo);
                    groups.Add(group);
                }
            }
            return meshGroups;
        }
        public static List<MeshInfo> SortMeshInfo(List<MeshInfo> meshInfos, Bounds worldBounds, float scale = 10f)
        {
            float scaleFactor = CalculateScaleFactor(worldBounds, scale);
            for (int i = 0; i < meshInfos.Count; i++)
            {
                //we need to update the lightmapindex. since in dungeons its set at runtime
                meshInfos[i].LightmapIndex = meshInfos[i].Renderer.lightmapIndex;
                //calculate the z-order curve from the bounds center
                meshInfos[i].ZOrderCurveIndex = CalculateMortonCodeFromBounds(meshInfos[i].Renderer.bounds, scaleFactor);
            }
            meshInfos.Sort();
            return meshInfos;
        }
    
        [Serializable]
        public class MeshInfo : IComparable<MeshInfo>
        {
            public MeshRenderer Renderer;
            public MeshFilter MeshFilter;
            public int NumberOfMaterials;
            public int ShaderHash;
            public int ShaderKeywordsHash;
            public int MaterialHash;
            public int LightmapIndex;
            public int ZOrderCurveIndex;
            public int VertexCount;

            public MeshInfo(MeshRenderer renderer, MeshFilter meshFilter)
            {
                Renderer = renderer;
                MeshFilter = meshFilter;
                NumberOfMaterials = renderer.sharedMaterials.Length;
                // store hashsets of all the shaders, materials and keywords
                HashSet<Shader> shaders = new HashSet<Shader>();
                HashSet<string> keywords = new HashSet<string>();
                HashSet<Material> materials = new HashSet<Material>();
                for (int i = 0; i < NumberOfMaterials; i++)
                {
                    Material material = renderer.sharedMaterials[i];
                    if (material == null) continue;
                    shaders.Add(material.shader);
                    materials.Add(material);
                    string[] materialKeywords = material.shaderKeywords;
                    for (int j = 0; j < materialKeywords.Length; j++)
                    {
                        keywords.Add(materialKeywords[j]);
                    }
                }
                // calculate the hashcodes for the shaders, materials and keywords
                if(shaders.Count != 0) ShaderHash = shaders.Select(s => s.GetHashCode()).Aggregate((a, b) => a ^ b);
                if(materials.Count != 0) MaterialHash = materials.Select(m => m.GetHashCode()).Aggregate((a, b) => a ^ b);
                if(keywords.Count != 0) ShaderKeywordsHash = keywords.Select(k => k.GetHashCode()).Aggregate((a, b) => a ^ b);
                
                LightmapIndex = renderer.lightmapIndex;
                ZOrderCurveIndex = 0;
                VertexCount = meshFilter.sharedMesh.vertexCount;
            }

            public int CompareTo(MeshInfo other)
            {
                if (NumberOfMaterials != other.NumberOfMaterials) return NumberOfMaterials > other.NumberOfMaterials ? 1 : -1;
                if (ShaderHash != other.ShaderHash) return ShaderHash > other.ShaderHash ? 1 : -1;
                if (ShaderKeywordsHash != other.ShaderKeywordsHash) return ShaderKeywordsHash > other.ShaderKeywordsHash ? 1 : -1;
                if (MaterialHash != other.MaterialHash) return MaterialHash > other.MaterialHash ? 1 : -1;
                if (LightmapIndex != other.LightmapIndex) return LightmapIndex > other.LightmapIndex ? 1 : -1;
                if (VertexCount != other.VertexCount) return VertexCount > other.VertexCount ? 1 : -1;
                if (ZOrderCurveIndex != other.ZOrderCurveIndex) return ZOrderCurveIndex > other.ZOrderCurveIndex ? 1 : -1;
                return 0;
            }
            
            public override int GetHashCode()
            {
                return NumberOfMaterials ^ ShaderHash ^ ShaderKeywordsHash ^ MaterialHash;
            }
        }
    }
}

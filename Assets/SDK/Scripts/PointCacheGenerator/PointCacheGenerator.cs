using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public static class PointCacheGenerator
{
    // Merge between the legacy Unity PointCacheBakeTool and SMRVFX (As PointCacheBakeTool doesn't output textures and SMRVFX doesn't bake properly some mesh)
    
    //Point cache generates the cache each time on imbue
    //Prefix to return already computed PCache, otherwise return and let the normal method deal with it
    //Then Postfix with storing the result
    public static Dictionary<PCacheSettings, PointCacheGenerator.PCache> pointCaches = new Dictionary<PCacheSettings, PointCacheGenerator.PCache>();
    
    
    public static ComputeShader computeShader;

    public enum MeshBakeMode
    {
        Vertex,
        Triangle
    }

    public enum Distribution
    {
        Sequential,
        Random,
        RandomUniformArea
    }

    public class PCache
    {
        public RenderTexture positionMap;
        public RenderTexture normalMap;
        public Mesh mesh;
        public int mapSize;
        public ComputeBuffer positionBuffer;
        public ComputeBuffer normalBuffer;
        public List<Vector3> positions;
        public List<Vector3> normals;
        
        
        public void Update(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            skinnedMeshRenderer.BakeMesh(mesh);
            mesh.GetVertices(positions);
            mesh.GetNormals(normals);
            int vcount = positions.Count;
            int vcount_x3 = vcount * 3;

            if (positionBuffer.count != vcount_x3)
            {
                positionBuffer.Dispose();
                normalBuffer.Dispose();
                positionBuffer = new ComputeBuffer(vcount_x3, sizeof(float));
                normalBuffer = new ComputeBuffer(vcount_x3, sizeof(float));
            }

            computeShader.SetInt("VertexCount", vcount);

            positionBuffer.SetData(positions);
            normalBuffer.SetData(normals);

            computeShader.SetBuffer(0, "PositionBuffer", positionBuffer);
            computeShader.SetBuffer(0, "NormalBuffer", normalBuffer);

            computeShader.SetTexture(0, "PositionMap", positionMap);
            computeShader.SetTexture(0, "NormalMap", normalMap);

            computeShader.Dispatch(0, mapSize / 8, mapSize / 8, 1);
        }

        public void Dispose()
        {
            if (positionBuffer != null)
            {
                positionBuffer.Dispose();
                normalBuffer.Dispose();
                positionBuffer = null;
                normalBuffer = null;
            }
        }
    }

    public static PCache ComputePCacheFromMesh(Mesh mesh, int mapSize = 512, int pointCount = 4096, int seed = 0, Distribution distribution = Distribution.RandomUniformArea, MeshBakeMode meshBakeMode = MeshBakeMode.Triangle)
    {
        if (mapSize % 8 != 0)
        {
            Debug.LogError("Position map dimensions should be a multiple of 8.");
            return null;
        }
        
        PCacheSettings pCacheSettings = new PCacheSettings(mesh.name, mapSize, pointCount, seed, distribution, meshBakeMode);
        
        //Return a cached version if it exists
        if(pointCaches.TryGetValue(pCacheSettings, out PointCacheGenerator.PCache pCache))
        {
            return pCache;
        }
        
        if (computeShader == null)
        {
            computeShader = computeShader = Resources.Load<ComputeShader>("ComputeMeshToMap");
        }

        pCache = new PCache();

        // From PointCacheBakeTool
        MeshData meshCache = ComputeDataCache(mesh);

        Picker picker = null;
        if (distribution == Distribution.Sequential)
        {
            if (meshBakeMode == MeshBakeMode.Vertex)
            {
                picker = new SequentialPickerVertex(meshCache);
            }
            else if (meshBakeMode == MeshBakeMode.Triangle)
            {
                picker = new SequentialPickerTriangle(meshCache);
            }
        }
        else if (distribution == Distribution.Random)
        {
            if (meshBakeMode == MeshBakeMode.Vertex)
            {
                picker = new RandomPickerVertex(meshCache, seed);
            }
            else if (meshBakeMode == MeshBakeMode.Triangle)
            {
                picker = new RandomPickerTriangle(meshCache, seed);
            }
        }
        else if (distribution == Distribution.RandomUniformArea)
        {
            picker = new RandomPickerUniformArea(meshCache, seed);
        }
        if (picker == null) throw new InvalidOperationException("Unable to find picker");

        pCache.mesh = mesh;
        pCache.mapSize = mapSize;
        //Size the lists first so we avoid resizing later
        pCache.positions = new List<Vector3>(pointCount);
        pCache.normals = new List<Vector3>(pointCount);

        for (int i = 0; i < pointCount; ++i)
        {
            MeshData.Vertex vertex = picker.GetNext();
            pCache.positions.Add(vertex.position);
            pCache.normals.Add(vertex.normal);
        }

        // From SMRVFX
        pCache.positionMap = new RenderTexture(mapSize, mapSize, 0, RenderTextureFormat.ARGBFloat);
        pCache.positionMap.enableRandomWrite = true;
        pCache.positionMap.Create();
        pCache.normalMap = new RenderTexture(mapSize, mapSize, 0, RenderTextureFormat.ARGBFloat);
        pCache.normalMap.enableRandomWrite = true;
        pCache.normalMap.Create();

        // Transfer data
        int vcount = pCache.positions.Count;
        int vcount_x3 = vcount * 3;

        pCache.positionBuffer = new ComputeBuffer(vcount_x3, sizeof(float));
        pCache.normalBuffer = new ComputeBuffer(vcount_x3, sizeof(float));

        computeShader.SetInt("VertexCount", vcount);

        pCache.positionBuffer.SetData(pCache.positions);
        pCache.normalBuffer.SetData(pCache.normals);

        computeShader.SetBuffer(0, "PositionBuffer", pCache.positionBuffer);
        computeShader.SetBuffer(0, "NormalBuffer", pCache.normalBuffer);

        computeShader.SetTexture(0, "PositionMap", pCache.positionMap);
        computeShader.SetTexture(0, "NormalMap", pCache.normalMap);

        computeShader.Dispatch(0, pCache.mapSize / 8, pCache.mapSize / 8, 1);
        
        //Add to the cache
        pointCaches.Add(pCacheSettings, pCache);
        return pCache;
    }

    static MeshData ComputeDataCache(Mesh input)
    {
        Vector3[] positions = input.vertices;
        Vector3[] normals = input.normals;
        Vector4[] tangents = input.tangents;
        Color[] colors = input.colors;
        List<Vector4[]> uvs = new List<Vector4[]>();

        normals = normals.Length == input.vertexCount ? normals : null;
        tangents = tangents.Length == input.vertexCount ? tangents : null;
        colors = colors.Length == input.vertexCount ? colors : null;

        for (int i = 0; i < 8; ++i)
        {
            List<Vector4> uv = new List<Vector4>();
            input.GetUVs(i, uv);
            if (uv.Count == input.vertexCount)
            {
                uvs.Add(uv.ToArray());
            }
            else
            {
                break;
            }
        }

        MeshData meshData = new MeshData();
        meshData.vertices = new MeshData.Vertex[input.vertexCount];
        for (int i = 0; i < input.vertexCount; ++i)
        {
            meshData.vertices[i] = new MeshData.Vertex()
            {
                position = positions[i],
                color = colors != null ? colors[i] : Color.white,
                normal = normals != null ? normals[i] : Vector3.up,
                tangent = tangents != null ? tangents[i] : Vector4.one,
                uvs = Enumerable.Range(0, uvs.Count).Select(c => uvs[c][i]).ToArray()
            };
        }

        meshData.triangles = new MeshData.Triangle[input.triangles.Length / 3];
        int[] triangles = input.triangles;
        for (int i = 0; i < meshData.triangles.Length; ++i)
        {
            meshData.triangles[i] = new MeshData.Triangle()
            {
                a = triangles[i * 3 + 0],
                b = triangles[i * 3 + 1],
                c = triangles[i * 3 + 2],
            };
        }
        return meshData;
    }

    class MeshData
    {
        public struct Vertex
        {
            public Vector3 position;
            public Color color;
            public Vector3 normal;
            public Vector4 tangent;
            public Vector4[] uvs;
            
            public static Vertex operator +(Vertex a, Vertex b)
            {
                if (a.uvs.Length != b.uvs.Length) throw new InvalidOperationException("Adding compatible vertex");

                Vertex r = new Vertex()
                {
                    position = a.position + b.position,
                    color = a.color + b.color,
                    normal = a.normal + b.normal,
                    tangent = a.tangent + b.tangent,
                    uvs = new Vector4[a.uvs.Length]
                };

                for (int i = 0; i < a.uvs.Length; ++i)
                {
                    r.uvs[i] = a.uvs[i] + b.uvs[i];
                }

                return r;
            }

            public static Vertex operator *(float a, Vertex b)
            {
                Vertex r = new Vertex()
                {
                    position = a * b.position,
                    color = a * b.color,
                    normal = a * b.normal,
                    tangent = a * b.tangent,
                    uvs = new Vector4[b.uvs.Length]
                };

                for (int i = 0; i < b.uvs.Length; ++i)
                {
                    r.uvs[i] = a * b.uvs[i];
                }

                return r;
            }
        };

        public struct Triangle
        {
            public int a, b, c;
        };

        public Vertex[] vertices;
        public Triangle[] triangles;
    }

    abstract class Picker
    {
        public abstract MeshData.Vertex GetNext();

        protected Picker(MeshData data)
        {
            m_cacheData = data;
        }

        //See http://inis.jinr.ru/sl/vol1/CMC/Graphics_Gems_1,ed_A.Glassner.pdf (p24) uniform distribution from two numbers in triangle generating barycentric coordinate
        protected readonly static Vector2 center_of_sampling = new Vector2(4.0f / 9.0f, 3.0f / 4.0f);
        protected MeshData.Vertex Interpolate(MeshData.Triangle triangle, Vector2 p)
        {
            return Interpolate(m_cacheData.vertices[triangle.a], m_cacheData.vertices[triangle.b], m_cacheData.vertices[triangle.c], p);
        }

        protected static MeshData.Vertex Interpolate(MeshData.Vertex A, MeshData.Vertex B, MeshData.Vertex C, Vector2 p)
        {
            if (A.uvs.Length != B.uvs.Length) throw new InvalidOperationException("Adding compatible vertex");
            if (B.uvs.Length != C.uvs.Length) throw new InvalidOperationException("Adding compatible vertex");
            
            float s = p.x;
            float t = Mathf.Sqrt(p.y);
            float a = 1.0f - t;
            float b = (1 - s) * t;
            float c = s * t;

            MeshData.Vertex r = new MeshData.Vertex();  
            // a * A + b * B + c * C
            r.position.x = a * A.position.x + b * B.position.x + c * C.position.x;
            r.position.y = a * A.position.y + b * B.position.y + c * C.position.y;
            r.position.z = a * A.position.z + b * B.position.z + c * C.position.z;
            
            r.color.a = a * A.color.a + b * B.color.a + c * C.color.a;
            r.color.r = a * A.color.r + b * B.color.r + c * C.color.r;
            r.color.g = a * A.color.g + b * B.color.g + c * C.color.g;
            r.color.b = a * A.color.b + b * B.color.b + c * C.color.b;
            
            r.normal.x = a * A.normal.x + b * B.normal.x + c * C.normal.x;
            r.normal.y = a * A.normal.y + b * B.normal.y + c * C.normal.y;
            r.normal.z = a * A.normal.z + b * B.normal.z + c * C.normal.z;
            
            r.tangent.x = a * A.tangent.x + b * B.tangent.x + c * C.tangent.x;
            r.tangent.y = a * A.tangent.y + b * B.tangent.y + c * C.tangent.y;
            r.tangent.z = a * A.tangent.z + b * B.tangent.z + c * C.tangent.z;
            r.tangent.w = a * A.tangent.w + b * B.tangent.w + c * C.tangent.w;
            
            r.uvs = new Vector4[A.uvs.Length];
            for (int i = 0; i < r.uvs.Length; ++i)
            {
                r.uvs[i].x = a * A.uvs[i].x + b * B.uvs[i].x + c * C.uvs[i].x; 
                r.uvs[i].y = a * A.uvs[i].y + b * B.uvs[i].y + c * C.uvs[i].y;
                r.uvs[i].z = a * A.uvs[i].z + b * B.uvs[i].z + c * C.uvs[i].z;
                r.uvs[i].w = a * A.uvs[i].w + b * B.uvs[i].w + c * C.uvs[i].w;

            }
            
            r.normal = r.normal.normalized;
            Vector3 tangent = new Vector3(r.tangent.x, r.tangent.y, r.tangent.z).normalized;
            r.tangent.x = tangent.x;
            r.tangent.y = tangent.y;
            r.tangent.z = tangent.z;
            r.tangent.w = r.tangent.w > 0.0f ? 1.0f : -1.0f;
            return r;
        }

        protected MeshData m_cacheData;
    }

    abstract class RandomPicker : Picker
    {
        protected RandomPicker(MeshData data, int seed) : base(data)
        {
            m_Rand = new System.Random(seed);
        }

        protected float GetNextRandFloat()
        {
            return (float)m_Rand.NextDouble(); //[0; 1[
        }

        protected System.Random m_Rand;
    }

    class RandomPickerVertex : RandomPicker
    {
        public RandomPickerVertex(MeshData data, int seed) : base(data, seed)
        {
        }

        public override sealed MeshData.Vertex GetNext()
        {
            int randomIndex = m_Rand.Next(0, m_cacheData.vertices.Length);
            return m_cacheData.vertices[randomIndex];
        }
    }

    class SequentialPickerVertex : Picker
    {
        private uint m_Index = 0;
        public SequentialPickerVertex(MeshData data) : base(data)
        {
        }

        public override sealed MeshData.Vertex GetNext()
        {
            MeshData.Vertex r = m_cacheData.vertices[m_Index];
            m_Index++;
            if (m_Index >= m_cacheData.vertices.Length)
                m_Index = 0;
            return r;
        }
    }

    class RandomPickerTriangle : RandomPicker
    {
        public RandomPickerTriangle(MeshData data, int seed) : base(data, seed)
        {
        }

        public override sealed MeshData.Vertex GetNext()
        {
            int index = m_Rand.Next(0, m_cacheData.triangles.Length);
            Vector2 rand = new Vector2(GetNextRandFloat(), GetNextRandFloat());
            return Interpolate(m_cacheData.triangles[index], rand);
        }
    }

    class RandomPickerUniformArea : RandomPicker
    {
        private double[] m_accumulatedAreaTriangles;

        private double ComputeTriangleArea(MeshData.Triangle t)
        {
            Vector3 A = m_cacheData.vertices[t.a].position;
            Vector3 B = m_cacheData.vertices[t.b].position;
            Vector3 C = m_cacheData.vertices[t.c].position;
            return 0.5f * Vector3.Cross(B - A, C - A).magnitude;
        }

        public RandomPickerUniformArea(MeshData data, int seed) : base(data, seed)
        {
            m_accumulatedAreaTriangles = new double[data.triangles.Length];
            m_accumulatedAreaTriangles[0] = ComputeTriangleArea(data.triangles[0]);
            for (int i = 1; i < data.triangles.Length; ++i)
            {
                m_accumulatedAreaTriangles[i] = m_accumulatedAreaTriangles[i - 1] + ComputeTriangleArea(data.triangles[i]);
            }
        }

        private uint FindIndexOfArea(double area)
        {
            uint min = 0;
            uint max = (uint)m_accumulatedAreaTriangles.Length - 1;
            uint mid = max >> 1;
            while (max >= min)
            {
                if (mid > m_accumulatedAreaTriangles.Length)
                    throw new InvalidOperationException("Cannot Find FindIndexOfArea");

                if (m_accumulatedAreaTriangles[mid] >= area &&
                    (mid == 0 || (m_accumulatedAreaTriangles[mid - 1] < area)))
                {
                    return mid;
                }
                else if (area < m_accumulatedAreaTriangles[mid])
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
                mid = (min + max) >> 1;
            }
            throw new InvalidOperationException("Cannot FindIndexOfArea");
        }

        public override sealed MeshData.Vertex GetNext()
        {
            double areaPosition = m_Rand.NextDouble() * m_accumulatedAreaTriangles.Last();
            uint areaIndex = FindIndexOfArea(areaPosition);

            Vector2 rand = new Vector2(GetNextRandFloat(), GetNextRandFloat());
            return Interpolate(m_cacheData.triangles[areaIndex], rand);
        }
    }

    class SequentialPickerTriangle : Picker
    {
        private uint m_Index = 0;
        public SequentialPickerTriangle(MeshData data) : base(data)
        {
        }

        public override sealed MeshData.Vertex GetNext()
        {
            MeshData.Triangle t = m_cacheData.triangles[m_Index];
            m_Index++;
            if (m_Index >= m_cacheData.triangles.Length)
                m_Index = 0;
            return Interpolate(t, center_of_sampling);
        }
    }
    
    public struct PCacheSettings : IEquatable<PCacheSettings>
    {
        private string meshName;
        private int pointCacheMapSize;
        private int pointCachePointCount;
        private int pointCacheSeed;
        private PointCacheGenerator.Distribution pointCacheDistribution;
        private PointCacheGenerator.MeshBakeMode pointCacheBakeMode;

        public PCacheSettings(string meshName, int pointCacheMapSize, int pointCachePointCount, int pointCacheSeed, PointCacheGenerator.Distribution pointCacheDistribution, PointCacheGenerator.MeshBakeMode pointCacheBakeMode) {
            this.meshName = meshName;
            this.pointCacheMapSize = pointCacheMapSize;
            this.pointCachePointCount = pointCachePointCount;
            this.pointCacheSeed = pointCacheSeed;
            this.pointCacheDistribution = pointCacheDistribution;
            this.pointCacheBakeMode = pointCacheBakeMode;
        }

        public bool Equals(PCacheSettings other)
        {
            return meshName == other.meshName
                   && pointCacheMapSize == other.pointCacheMapSize
                   && pointCachePointCount == other.pointCachePointCount
                   && pointCacheSeed == other.pointCacheSeed
                   && pointCacheDistribution == other.pointCacheDistribution
                   && pointCacheBakeMode == other.pointCacheBakeMode;
        }
        public override bool Equals(object obj)
        {
            return obj is PCacheSettings other && Equals(other);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = meshName.GetHashCode();
                hashCode = (hashCode * 397) ^ pointCacheMapSize;
                hashCode = (hashCode * 397) ^ pointCachePointCount;
                hashCode = (hashCode * 397) ^ pointCacheSeed;
                hashCode = (hashCode * 397) ^ (int)pointCacheDistribution;
                hashCode = (hashCode * 397) ^ (int)pointCacheBakeMode;
                return hashCode;
            }
        }
        public static bool operator ==(PCacheSettings left, PCacheSettings right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(PCacheSettings left, PCacheSettings right)
        {
            return !left.Equals(right);
        }
    }
}

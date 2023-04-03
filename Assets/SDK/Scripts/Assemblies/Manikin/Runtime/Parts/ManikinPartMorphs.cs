using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace ThunderRoad.Manikin
{
    [RequireComponent(typeof(ManikinPart), typeof(Renderer))]
    public class ManikinPartMorphs : MonoBehaviour, ISerializationCallbackReceiver
    {
        public List<ManikinMorphAsset> morphAssets;

        public bool IsDirty { get { return isDirty; } }
        [SerializeField, HideInInspector]
        private bool isDirty;

        private Dictionary<int, float> currentMorphs = new Dictionary<int, float>();

        [SerializeField, HideInInspector]
        private List<int> morphKeys = new List<int>();
        [SerializeField, HideInInspector]
        private List<float> morphValues = new List<float>();

        [SerializeField, HideInInspector]
        private ManikinPart part;
        [SerializeField, HideInInspector]
        private Mesh defaultMesh;

        public SkinnedMeshRenderer SkinnedMeshRenderer { get { return smr; } }

        [SerializeField, HideInInspector]
        private SkinnedMeshRenderer smr;

        public bool UpdateMorphValue(int hash, float value)
        {
            if(currentMorphs.TryGetValue(hash, out float morphValue))
            {
                if(morphValue != value)
                {
                    currentMorphs[hash] = value;
                    isDirty = true;
                }
                //else
                //{
                //    Debug.LogWarning("Same value morph, no need to update.");
                //}
                return true;
            }
            else
            {
                foreach(var morphAsset in morphAssets)
                {
                    if(morphAsset.MorphHash == hash)
                    {
                        currentMorphs.Add(hash, value);
                        isDirty = true;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TryGetMeshNeedUpdate(out Mesh mesh)
        {
            if (isDirty && smr != null && morphAssets != null && morphAssets.Count > 0)
            {
                if (ReferenceEquals(smr.sharedMesh, defaultMesh))
                {
                    smr.sharedMesh = Instantiate(defaultMesh);
                }
                mesh = defaultMesh;

                foreach (ManikinMorphAsset morphAsset in morphAssets)
                {
                    if(!currentMorphs.ContainsKey(morphAsset.MorphHash))
                    {
                        currentMorphs.Add(morphAsset.MorphHash, 0f);
                    }
                }

                return true;
            }

            mesh = null;
            return false;
        }

        public JobHandle UpdateMesh(NativeArray<Vector3> vertices, NativeArray<Vector3> normals, NativeArray<Vector4> tangents)
        {
            if (morphAssets == null)
                return default;

            NativeArray<JobHandle> handles = new NativeArray<JobHandle>(morphAssets.Count, Allocator.Temp);

            for(int i = 0; i < morphAssets.Count; i++)
            {
                float morphValue = 0;
                if (currentMorphs.TryGetValue(morphAssets[i].MorphHash, out float value))
                {
                    morphValue = value;
                }

                var jobHandle = new MorphJob
                {
                    vertices = vertices,
                    normals = normals,
                    tangents = tangents,
                    deltaVertices = new NativeArray<Vector3>(morphAssets[i].deltaVertices, Allocator.TempJob),
                    deltaNormals = new NativeArray<Vector3>(morphAssets[i].deltaNormals, Allocator.TempJob),
                    deltaTangents = new NativeArray<Vector3>(morphAssets[i].deltaTangents, Allocator.TempJob),
                    value = morphValue
                };

                handles[i] = jobHandle.Schedule(vertices.Length, 64);
            }
            isDirty = false;

            return JobHandle.CombineDependencies(handles);
        }

        [BurstCompile]
        public struct MorphJob : IJobParallelFor
        {
            public NativeArray<Vector3> vertices;
            public NativeArray<Vector3> normals;
            public NativeArray<Vector4> tangents;

            [ReadOnly, DeallocateOnJobCompletion]public NativeArray<Vector3> deltaVertices;
            [ReadOnly, DeallocateOnJobCompletion]public NativeArray<Vector3> deltaNormals;
            [ReadOnly, DeallocateOnJobCompletion]public NativeArray<Vector3> deltaTangents;

            public float value;

            public void Execute(int index)
            {
                vertices[index] = vertices[index] + deltaVertices[index] * value;
                normals[index] = normals[index] + deltaNormals[index] * value;
                tangents[index] = tangents[index] + (Vector4)(deltaTangents[index] * value);
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            part = GetComponent<ManikinPart>();
            smr = GetComponent<SkinnedMeshRenderer>();
            if (smr != null)
            {
                defaultMesh = smr.sharedMesh;
            }

            if (morphAssets != null)
            {
                currentMorphs.Clear();
                foreach (ManikinMorphAsset morphAsset in morphAssets)
                {
                    if(morphAsset != null && !currentMorphs.ContainsKey(morphAsset.MorphHash))
                    {
                        currentMorphs.Add(morphAsset.MorphHash, 0f);
                    }
                }
            }
        }

        public List<KeyValuePair<int, float>> GetMorphKeyValuePairs()
        {
            List<KeyValuePair<int, float>> pairs = new List<KeyValuePair<int, float>>();
            foreach (var kvp in currentMorphs)
            {
                pairs.Add(kvp);
            }
            return pairs;
        }
#endif

        public void OnBeforeSerialize()
        {
            morphKeys.Clear();
            morphValues.Clear();

            foreach (var kvp in currentMorphs)
            {
                morphKeys.Add(kvp.Key);
                morphValues.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            currentMorphs.Clear();
            for (int i = 0; i != Math.Min(morphKeys.Count, morphValues.Count); i++)
                currentMorphs.Add(morphKeys[i], morphValues[i]);
        }
    }
}

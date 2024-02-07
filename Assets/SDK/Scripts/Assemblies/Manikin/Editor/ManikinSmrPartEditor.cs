using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Linq;

namespace ThunderRoad.Manikin
{
    [CustomEditor(typeof(ManikinSmrPart))]
    public class ManikinSmrPartEditor : Editor
    {
        private bool showSingleBone;
        private SerializedProperty singleBone;

        private bool showTransferBones;
        private SerializedProperty fromSmrs;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            if (GUILayout.Button("Initialize"))
            {
                (target as ManikinSmrPart).Initialize();
            }

            if (GUILayout.Button("Set Smr bones from hashes"))
            {
                (target as ManikinSmrPart).SetSmrBonesFromHashes();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Helper Tools", EditorStyles.boldLabel);

            showSingleBone = EditorGUILayout.BeginFoldoutHeaderGroup(showSingleBone, "Single Bone");
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (showSingleBone)
            {
                singleBone = serializedObject.FindProperty("singleBone");
                EditorGUILayout.PropertyField(singleBone, new GUIContent("Bone"));

                if (GUILayout.Button("Setup Single Bone"))
                {
                    SetupSingleBone();
                }
            }

            EditorGUILayout.Space();

            showTransferBones = EditorGUILayout.BeginFoldoutHeaderGroup(showTransferBones, "Transfer Bones");
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (showTransferBones)
            {
                fromSmrs = serializedObject.FindProperty("fromSmrs");
                EditorGUILayout.PropertyField(fromSmrs, new GUIContent("From SMRs"));

                if (GUILayout.Button("Transfer Bones"))
                {
                    TransferBones();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private bool TryGetReadableMesh(out Mesh mesh)
        {
            var part = target as ManikinSmrPart;

            mesh = null;
            if (part.TryGetComponent<MeshFilter>(out var meshFilter))
                mesh = meshFilter.sharedMesh;
            else if (part.TryGetComponent<SkinnedMeshRenderer>(out var smr))
            {
                mesh = new Mesh();
                mesh.name = smr.sharedMesh.name;
                smr.BakeMesh(mesh, true);
            }

            if (!(mesh?.isReadable ?? true))
                mesh = MakeReadableMeshCopy(mesh);

            return mesh != null;
        }

        private static Mesh MakeReadableMeshCopy(Mesh nonReadableMesh)
        {
            var meshCopy = new Mesh();
            meshCopy.name = nonReadableMesh.name;
            meshCopy.indexFormat = nonReadableMesh.indexFormat;

            // Handle vertices
            var verticesBuffer = nonReadableMesh.GetVertexBuffer(0);
            var totalSize = verticesBuffer.stride * verticesBuffer.count;
            var data = new byte[totalSize];
            verticesBuffer.GetData(data);
            meshCopy.SetVertexBufferParams(nonReadableMesh.vertexCount, nonReadableMesh.GetVertexAttributes());
            meshCopy.SetVertexBufferData(data, 0, 0, totalSize);
            verticesBuffer.Release();

            // Handle triangles
            meshCopy.subMeshCount = nonReadableMesh.subMeshCount;
            var indexesBuffer = nonReadableMesh.GetIndexBuffer();
            var tot = indexesBuffer.stride * indexesBuffer.count;
            var indexesData = new byte[tot];
            indexesBuffer.GetData(indexesData);
            meshCopy.SetIndexBufferParams(indexesBuffer.count, nonReadableMesh.indexFormat);
            meshCopy.SetIndexBufferData(indexesData, 0, 0, tot);
            indexesBuffer.Release();

            // Restore submesh structure
            var currentIndexOffset = 0u;
            for (var i = 0; i < meshCopy.subMeshCount; i++)
            {
                var subMeshIndexCount = nonReadableMesh.GetIndexCount(i);
                meshCopy.SetSubMesh(i, new SubMeshDescriptor((int)currentIndexOffset, (int)subMeshIndexCount));
                currentIndexOffset += subMeshIndexCount;
            }

            // Recalculate normals and bounds
            meshCopy.RecalculateNormals();
            meshCopy.RecalculateBounds();

            return meshCopy;
        }

        private static bool SaveMesh(ref Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
        {
            var path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "mesh");
            if (string.IsNullOrEmpty(path))
                return false;

            path = FileUtil.GetProjectRelativePath(path);

            if (makeNewInstance)
                mesh = Object.Instantiate(mesh);

            if (optimizeMesh)
                MeshUtility.Optimize(mesh);

            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.SaveAssets();
            return true;
        }

        private void SetupSingleBone()
        {
            var part = target as ManikinSmrPart;

            if (!TryGetReadableMesh(out var mesh))
            {
                Debug.LogError("No mesh was found on this GameObject! Make sure you have a MeshFilter OR SkinnedMeshRenderer with a mesh present.");
                return;
            }

            // Setup bone weights
            var weights = new BoneWeight[mesh.vertexCount];
            for (var i = 0; i < mesh.vertexCount; i++)
            {
                weights[i].boneIndex0 = 0;
                weights[i].weight0 = 1;
            }
            // Save mesh
            mesh.boneWeights = weights;
            mesh.bindposes = new[] { part.singleBone.worldToLocalMatrix * part.transform.localToWorldMatrix };
            if (!SaveMesh(ref mesh, mesh.name, true, false))
            {
                Debug.Log("Cancelled saving mesh.");
                return;
            }

            // SMR
            if (!part.gameObject.TryGetComponent(out SkinnedMeshRenderer smr))
                smr = part.gameObject.AddComponent<SkinnedMeshRenderer>();
            smr.rootBone = part.singleBone;
            smr.bones = new[] { part.singleBone };
            smr.sharedMesh = mesh;

            part.OnValidate();
            part.Initialize();
        }

        private void TransferBones()
        {
            var stopWatch = Stopwatch.StartNew();
            var part = target as ManikinSmrPart;

            if (!TryGetReadableMesh(out var mesh))
            {
                Debug.LogError("No mesh was found on this GameObject! Make sure you have a MeshFilter OR SkinnedMeshRenderer with a mesh present.");
                return;
            }
            if (part.fromSmrs == null || part.fromSmrs.Length == 0)
            {
                Debug.LogError("Empty FromSMRs! Please provide at least one SkinnedMeshRenderer to transfer the bones from.");
                return;
            }

            var smrCount = part.fromSmrs.Length;
            var bakedMeshes = new Mesh[smrCount];
            var fromVertices = new Vector3[smrCount][];
            var bones = new HashSet<Transform>();
            var indexMap = new int[smrCount][];
            for (var i = 0; i < smrCount; i++)
            {
                bakedMeshes[i] = new Mesh();
                part.fromSmrs[i].BakeMesh(bakedMeshes[i], true);
                fromVertices[i] = new Vector3[bakedMeshes[i].vertexCount];
                bones.UnionWith(part.fromSmrs[i].bones);

                indexMap[i] = new int[part.fromSmrs[i].bones.Length];
                for (var j = 0; j < part.fromSmrs[i].bones.Length; j++)
                    indexMap[i][j] = GetIndex(part.fromSmrs[i].bones[j]);

                int GetIndex(Transform value)
                {
                    var i = 0;
                    foreach (var v in bones)
                    {
                        if (v == value)
                            return i;
                        i++;
                    }

                    return -1;
                }
            }

            // Setup bone weights
            var weights = new BoneWeight[mesh.vertexCount];
            for (var i = 0; i < mesh.vertexCount; i++)
            {
                var toPos = part.transform.TransformPoint(mesh.vertices[i]);

                var minDistance = float.MaxValue;
                int minJ = 0, minK = 0;
                for (var j = 0; j < smrCount; j++)
                {
                    for (var k = 0; k < bakedMeshes[j].vertexCount; k++)
                    {
                        var fromPos = fromVertices[j][k];
                        if (fromPos == Vector3.zero)
                            fromPos = fromVertices[j][k] = part.fromSmrs[j].transform.TransformPoint(bakedMeshes[j].vertices[k]);
                        var d = (fromPos - toPos).sqrMagnitude;
                        if (minDistance <= d)
                            continue;
                        minDistance = d;
                        minJ = j;
                        minK = k;
                    }
                }

                var indices = indexMap[minJ];
                weights[i] = part.fromSmrs[minJ].sharedMesh.boneWeights[minK];
                if (weights[i].weight0 > 0)
                {
                    weights[i].boneIndex0 = indices[weights[i].boneIndex0];
                }
                if (weights[i].weight1 > 0)
                {
                    weights[i].boneIndex1 = indices[weights[i].boneIndex1];
                }
                if (weights[i].weight2 > 0)
                {
                    weights[i].boneIndex2 = indices[weights[i].boneIndex2];
                }
                if (weights[i].weight3 > 0)
                {
                    weights[i].boneIndex3 = indices[weights[i].boneIndex3];
                }
            }
            mesh.boneWeights = weights;

            // Setup bind poses
            var bindPoses = new Matrix4x4[bones.Count];
            var index = 0;
            foreach (var bone in bones)
                bindPoses[index++] = bone.worldToLocalMatrix * part.transform.localToWorldMatrix;
            mesh.bindposes = bindPoses;

            stopWatch.Stop();
            Debug.Log("Finished in " + stopWatch.Elapsed.ToString(@"hh\:mm\:ss\:fff"));

            // Save mesh
            if (!SaveMesh(ref mesh, mesh.name, true, false))
            {
                Debug.Log("Cancelled saving mesh.");
                return;
            }

            // SMR
            if (!part.gameObject.TryGetComponent(out SkinnedMeshRenderer smr))
                smr = part.gameObject.AddComponent<SkinnedMeshRenderer>();
            smr.rootBone = part.fromSmrs[0].rootBone;
            smr.bones = bones.ToArray();
            smr.sharedMesh = mesh;

            part.OnValidate();
            part.Initialize();
        }
    }
}

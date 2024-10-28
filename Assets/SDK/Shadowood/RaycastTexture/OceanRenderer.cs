using System;
using System.Collections.Generic;
using System.Linq;
using Shadowood.RaycastTexture;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Shadowood.RaycastTexture
{
    [ExecuteInEditMode]
    public class OceanRenderer : MonoBehaviour
    {
        public bool debug;
        public bool showSplineDebug;
        public int showSplineDebugCount = 256;

        public bool receiveShadows = false;
        public ShadowCastingMode shadowCastingMode = ShadowCastingMode.Off;
        public LightProbeUsage lightProbeUsage = LightProbeUsage.Off;

        [Tooltip("Enable the LIGHTMAP_ON keyword, needed for shadowmask support")]
        public bool lightmapON = true;

        public Transform targetTransform;

        public Mesh lowMesh, highMesh;

        //public Mesh meshCopy;
        public Mesh horizonMesh;

        [SerializeField] private Mesh horizonMeshCopy;

        //public RaycastTexture raycastTexture;
/*
        public enum eMethod
        {
            none,
            //regularMeshLow,
            //regularMeshHigh,
            drawMesh,
            drawMeshInstanced,
            //drawMeshInstancedIndirect // not implemented
        }
*/
        public bool drawToHorizon = true;

        [SingleLayer] public int layer = 4;

        //public eMethod currentMethod;
        //public eMethod methodDesktop = eMethod.drawMeshInstanced;
        //public eMethod methodMobile = eMethod.drawMeshInstanced;

        //public AssetSorceryPlatformRuntime.ePlatformAS chosenPlatform = AssetSorceryPlatformRuntime.ePlatformAS.Desktop;
        //private AssetSorceryPlatformRuntime.ePlatformAS chosenPlatformLast;

        [Space] [Header("drawMeshInstanced settings")] [Range(1, 31)]
        public int instancedGridNum = 16;

        [Tooltip("Mask spline to hide water surface")] [InlineButton("FindThunderSpline")]
        public ThunderSplineWithin thunderSplineWithin;

        void FindThunderSpline()
        {
            if (thunderSplineWithin == null) thunderSplineWithin = FindObjectOfType<ThunderSplineWithin>(false);
        }

        [Button, ContextMenu("RunWithChosenPlatform")]
        public void RunWithChosenPlatform()
        {
            //Debug.Log("OceanRenderer: RunWithChosenPlatform");

            MethodSetup();
            /*
            //var platform = AssetSorceryPlatform.AssetSorceryGetPlatformCalculated();
            var platform = chosenPlatform; //AssetSorceryPlatformRuntime.AssetSorceryGetBuildPlatform();

            switch (platform)
            {
                case AssetSorceryPlatformRuntime.ePlatformAS.Desktop:
                    MethodSetup(methodDesktop);
                    break;
                case AssetSorceryPlatformRuntime.ePlatformAS.Mobile:
                    MethodSetup(methodMobile);
                    break;
            }*/
        }

        private Matrix4x4 lastMatrix;

        public void OnValidate()
        {
            if (!isActiveAndEnabled) return;
            /*
            if (chosenPlatformLast != chosenPlatform)
            {
                chosenPlatformLast = chosenPlatform;
                RunWithChosenPlatform();
            }*/
            RunWithChosenPlatform();
            //var newMatrix = transform.localToWorldMatrix;
            //if (lastMatrix != newMatrix)
            //{
            //    lastMatrix = newMatrix;
            //    RunWithChosenPlatform();
            //}
        }

        public void Update()
        {
            var newMatrix = targetTransform.localToWorldMatrix;
            if (lastMatrix != newMatrix)
            {
                lastMatrix = newMatrix;
                RunWithChosenPlatform();
            }
        }

#if UNITY_EDITOR
        public void Reset()
        {
            name = "OceanRenderer";
            transform.localScale = new Vector3(80, 1, 80);
            targetTransform = transform;
            FindMeshAssets();
            MethodSetup();
            layer = LayerMask.NameToLayer("Water");
        }

        public void FindMeshAssets()
        {
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("OceanPlane_2Tri").First());
            lowMesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>(path);

            var path2 = UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("OceanPlane_SubD_50").First());
            highMesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>(path2);

            var path3 = UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("Infinite_Plane").First());
            horizonMesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>(path3);
        }

#else
        public void Reset()
        {
        }

#endif
        /*
        [Button]
        public void MethodSetupMobile()
        {
            MethodSetup(methodMobile);
        }

        [Button]
        public void MethodSetupDesktop()
        {
            MethodSetup(methodDesktop);
        }*/

        public void SetMaterial(Material matIn)
        {
            mat = matIn;
        }

        public void SetMatProp(MaterialPropertyBlock matPropIn)
        {
            matProp = matPropIn;
            //if (rendererItem) rendererItem.SetPropertyBlock(matProp);
        }

        private MaterialPropertyBlock matProp;

        public Material mat;


        //public Renderer rendererItem;
        //public MeshFilter meshFilterItem;
        public float maxDistance = 100;

        public void UpdateKeyword()
        {
            if (lightmapON)
            {
                mat.EnableKeyword("LIGHTMAP_ON");
            }
            else
            {
                mat.DisableKeyword("LIGHTMAP_ON");
            }
        }

        public void MethodSetup()
        {
            UpdateKeyword();

            // SplineUtility is editor only, so cache this in 'matrices' to avoid doing at runtime
            if (horizonMesh && horizonMeshCopy == null)
            {
                float bound = 100000f;
                horizonMeshCopy = new Mesh();
                Copy(horizonMesh, horizonMeshCopy);
                horizonMeshCopy.bounds = new Bounds(Vector3.zero, new Vector3(bound, 1, bound));
            }

#if UNITY_EDITOR
            if (!Application.isPlaying) MethodSetupSplines();
#endif

            if (matricesCached == null || matricesCached.Count == 0)
            {
#if UNITY_EDITOR
                //Debug.LogError("OceanRenderer: Matrices not cached", this);
                MethodSetupMatrices();
#else
                //Debug.LogWarning("OceanRenderer: Matrices not cached - creating default grid with no spline mask");
                //MethodSetupMatricesNoSpline(); // Fill matrices up, no testing against the spline mask as this is a runtime fallback to avoid ocean being entirely missing
                MethodSetupMatrices();
#endif
            }

#if UNITY_EDITOR
            if (!Application.isPlaying) MethodSetupMatrices();
#endif

            TransformCachedMatrices();
        }


#if UNITY_EDITOR
        //[Button]
        public void UpdateMatrixCache()
        {
            MethodSetupMatrices();
            ;
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(prefabStage.scene);
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.EditorUtility.SetDirty(prefabStage.prefabContentsRoot);
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(prefabStage.prefabContentsRoot);
            }
        }
#endif


//#if UNITY_EDITOR

        /// <summary>
        /// Matrix cache purely for debug view
        /// </summary>
        public void MethodSetupSplines()
        {
            if (showSplineDebugCount > 0 && showSplineDebug)
            {
                matricesSpline = new List<Matrix4x4>();

                var gn = showSplineDebugCount; //raycastTexture.resolutionColor;
                var unit = 1.0f / (float) gn;
                var lm = targetTransform.localToWorldMatrix;

                int i = 0;
                for (int x = 0; x < gn; x++)
                {
                    for (int y = 0; y < gn; y++)
                    {
                        var pos = new Vector3((unit * (float) x) - (unit * (float) gn / 2f) + unit / 2.0f, 0, (unit * (float) y) - (unit * (float) gn / 2f) + unit / 2.0f);
                        var matCalc = lm * Matrix4x4.TRS(pos, Quaternion.identity, new Vector3(unit, 1, unit) * 1.001f); // 1.001 to add a tiny overlap to prevent missing pixels causing firefly

                        if (thunderSplineWithin && thunderSplineWithin.isActiveAndEnabled)
                        {
                            // Check if all of the 4 corners are within the spline, if so skip rendering this instance
                            var a = ThunderSplineWithin.IsInsideSpline(thunderSplineWithin.thunderSpline.transform.InverseTransformPoint(lm.MultiplyPoint(pos - new Vector3(-unit / 2, 0, -unit / 2))), thunderSplineWithin.thunderSpline, maxDistance);
                            var b = ThunderSplineWithin.IsInsideSpline(thunderSplineWithin.thunderSpline.transform.InverseTransformPoint(lm.MultiplyPoint(pos - new Vector3(-unit / 2, 0, unit / 2))), thunderSplineWithin.thunderSpline, maxDistance);
                            var c = ThunderSplineWithin.IsInsideSpline(thunderSplineWithin.thunderSpline.transform.InverseTransformPoint(lm.MultiplyPoint(pos - new Vector3(unit / 2, 0, unit / 2))), thunderSplineWithin.thunderSpline, maxDistance);
                            var d = ThunderSplineWithin.IsInsideSpline(thunderSplineWithin.thunderSpline.transform.InverseTransformPoint(lm.MultiplyPoint(pos - new Vector3(unit / 2, 0, -unit / 2))), thunderSplineWithin.thunderSpline, maxDistance);
                            if (a && b && c && d) continue;
                        }

                        matricesSpline.Add(matCalc);

                        i++;
                    }
                }
            }
            else
            {
                matricesSpline.Clear();
            }
        }


        /// <summary>
        /// Matrix cache for device runtime, caches spline mask hiding parts of the grid
        /// </summary>
        [ContextMenu("MethodSetupMatrices")]
        public void MethodSetupMatrices()
        {
            matricesCached = new List<Matrix4x4>();

            var unit = 1.0f / (float) instancedGridNum;
            var lm = targetTransform.localToWorldMatrix;

            int i = 0;
            for (int x = 0; x < instancedGridNum; x++)
            {
                for (int y = 0; y < instancedGridNum; y++)
                {
                    var pos = new Vector3((unit * (float) x) - (unit * (float) instancedGridNum / 2f) + unit / 2.0f, 0, (unit * (float) y) - (unit * (float) instancedGridNum / 2f) + unit / 2.0f);
                    //var pos = new Vector3((unit * (float) x), 0, (unit * (float) y));


                    //var matCalc = lm * Matrix4x4.TRS(pos, Quaternion.identity, new Vector3(unit, 1, unit) * 1.001f); // 1.001 to add a tiny overlap to prevent missing pixels causing firefly
                    var matCalc = Matrix4x4.TRS(pos, Quaternion.identity, new Vector3(unit, 1, unit) * 1.001f); // 1.001 to add a tiny overlap to prevent missing pixels causing firefly

                    if (thunderSplineWithin && thunderSplineWithin.isActiveAndEnabled)
                    {
                        // Check if all of the 4 corners are within the spline, if so skip rendering this instance
                        var a = ThunderSplineWithin.IsInsideSpline(thunderSplineWithin.thunderSpline.transform.InverseTransformPoint(lm.MultiplyPoint(pos - new Vector3(-unit / 2, 0, -unit / 2))), thunderSplineWithin.thunderSpline, maxDistance);
                        var b = ThunderSplineWithin.IsInsideSpline(thunderSplineWithin.thunderSpline.transform.InverseTransformPoint(lm.MultiplyPoint(pos - new Vector3(-unit / 2, 0, unit / 2))), thunderSplineWithin.thunderSpline, maxDistance);
                        var c = ThunderSplineWithin.IsInsideSpline(thunderSplineWithin.thunderSpline.transform.InverseTransformPoint(lm.MultiplyPoint(pos - new Vector3(unit / 2, 0, unit / 2))), thunderSplineWithin.thunderSpline, maxDistance);
                        var d = ThunderSplineWithin.IsInsideSpline(thunderSplineWithin.thunderSpline.transform.InverseTransformPoint(lm.MultiplyPoint(pos - new Vector3(unit / 2, 0, -unit / 2))), thunderSplineWithin.thunderSpline, maxDistance);
                        if (a && b && c && d) continue;
                    }

                    matricesCached.Add(matCalc);

                    i++;
                }
            }
        }


//#endif

        /// <summary>
        /// Create matrix list for grid rendering, but with no use of spline mask, fall back for if it fails at runtime / is missing the cache
        /// </summary>
        public void MethodSetupMatricesNoSpline()
        {
            matricesCached = new List<Matrix4x4>();
            var unit = 1.0f / (float) instancedGridNum;
            int i = 0;
            for (int x = 0; x < instancedGridNum; x++)
            {
                for (int y = 0; y < instancedGridNum; y++)
                {
                    var pos = new Vector3((unit * (float) x) - (unit * (float) instancedGridNum / 2f) + unit / 2.0f, 0, (unit * (float) y) - (unit * (float) instancedGridNum / 2f) + unit / 2.0f);
                    var matCalc = Matrix4x4.TRS(pos, Quaternion.identity, new Vector3(unit, 1, unit) * 1.001f); // 1.001 to add a tiny overlap to prevent missing pixels causing firefly
                    matricesCached.Add(matCalc);
                    i++;
                }
            }
        }

        // TODO move to utility class
        public static void Copy(Mesh src, Mesh dst)
        {
            if (dst == null || src == null) return;

            if (!src.isReadable) Debug.LogError("OceanRenderer: Mesh needs to be set to Read/Write in import settings: " + src.name);

            dst.Clear();
            dst.vertices = src.vertices;

            List<Vector4> uvs = new List<Vector4>();

            src.GetUVs(0, uvs);
            dst.SetUVs(0, uvs);
            //src.GetUVs(1, uvs); dst.SetUVs(1, uvs);
            //src.GetUVs(2, uvs); dst.SetUVs(2, uvs);
            //src.GetUVs(3, uvs); dst.SetUVs(3, uvs);

            dst.normals = src.normals;
            dst.tangents = src.tangents;
            //dst.boneWeights = src.boneWeights;
            //dst.colors = src.colors;
            //dst.bindposes = src.bindposes;

            dst.subMeshCount = src.subMeshCount;
            dst.indexFormat = src.indexFormat;
            dst.bounds = src.bounds;

            for (var i = 0; i < src.subMeshCount; i++) dst.SetIndices(src.GetIndices(i), src.GetTopology(i), i);

            dst.name = src.name + "-Copy";
        }

        private Mesh quadMesh;

        void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering -= DrawNow;
            RenderPipelineManager.beginCameraRendering += DrawNow;

            RunWithChosenPlatform();
        }

        [ContextMenu("TransformCachedmatrices")]
        public void TransformCachedMatrices()
        {
            matricesRotated = new List<Matrix4x4>();
            var lm = targetTransform.localToWorldMatrix;

            foreach (var m in matricesCached)
            {
                matricesRotated.Add(lm * m);
            }
        }

        void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= DrawNow;

            if (matProp != null) matProp.Clear();

            //if (argsBuffer != null) argsBuffer.Release();
            //argsBuffer = null;
//
            //if (positionBuffer != null) positionBuffer.Release();
            //positionBuffer = null;
        }

#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = targetTransform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 0, 1));

            //if (!debug) return;
            //if (currentMethod == eMethod.drawMeshInstanced) ;// || currentMethod == eMethod.drawMeshInstancedIndirect)
            {
                Gizmos.color = new Color(1f, 1f, 1f, 0.02f);
                for (int i = 0; i < matricesLow.Count; i++)
                {
                    Gizmos.matrix = matricesLow[i];
                    Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 0, 1));
                }

                Gizmos.color = new Color(1f, 1f, 1f, 1f);
                for (int i = 0; i < matricesHigh.Count; i++)
                {
                    Gizmos.matrix = matricesHigh[i];
                    Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 0.01f, 1));
                }
            }

            if (showSplineDebugCount > 0 && showSplineDebug && matricesSpline.Count > 0)
            {
                Gizmos.color = new Color(0f, 1f, 0f, 1f);
                for (int i = 0; i < matricesSpline.Count; i++)
                {
                    Gizmos.matrix = matricesSpline[i];
                    Gizmos.DrawRay(Vector3.zero, Vector3.up);
                    //Gizmos.DrawLine(Vector3.zero, Vector3.one);
                    //Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 0.02f, 1));
                }
            }
        }
#else
        public void OnDrawGizmosSelected()
        {
        }
#endif

        //private ComputeBuffer argsBuffer;
        //private ComputeBuffer positionBuffer;

        public float maxDist = 10;

        //[SerializeField]
        internal List<Matrix4x4> matricesCached = new List<Matrix4x4>();
        private List<Matrix4x4> matricesRotated = new List<Matrix4x4>();

        [NonSerialized] private List<Matrix4x4> matricesHigh = new List<Matrix4x4>();
        [NonSerialized] private List<Matrix4x4> matricesLow = new List<Matrix4x4>();

//#if UNITY_EDITOR
        [NonSerialized] public List<Matrix4x4> matricesSpline = new List<Matrix4x4>();
//#endif

        [ShowInInspector] private int highCount;

        [ShowInInspector] private int lowCount;

        public bool frustumCulling = true;
        public float adjustFrustumFOV = 0;

        private Plane[] planes = new Plane[6];

        public void SplineChanged()
        {
            if (debug) Debug.Log("SplineChanged", this);
            RunWithChosenPlatform();
        }


        void DrawNow(ScriptableRenderContext context, Camera cameraIn)
        {
            if (mat == null)
            {
                Debug.LogError("OceanRenderer: Material missing", this);
                return;
            }

            if (matProp == null) matProp = new MaterialPropertyBlock();
            matProp.SetFloat("_TessNum", 1);

            if (drawToHorizon && horizonMeshCopy)
            {
                Graphics.DrawMesh(horizonMeshCopy, targetTransform.localToWorldMatrix, mat, 0, Camera.current, 0, matProp, shadowCastingMode, receiveShadows, null, lightProbeUsage, null);
            }

            matProp.SetFloat("_TessNum", mat.GetFloat("_TessNum"));

            if (matricesRotated == null || matricesRotated.Count == 0) return;

/*
            if (currentMethod == eMethod.drawMesh && meshCopy)
            {
                if (cameraIn.cameraType != CameraType.Game && cameraIn.cameraType != CameraType.SceneView) return;
                if (debug) Debug.LogError("DrawNow: " + cameraIn.name, cameraIn);
                //if (quadMesh == null) quadMesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");

                //Graphics.DrawMesh(meshCopy, transform.localToWorldMatrix, mat, 0, Camera.current, 0, matProp, ShadowCastingMode.Off, false);

                var renderParams = new RenderParams
                {
                    material = mat,
                    camera = cameraIn,
                    layer = layer,
                    lightProbeProxyVolume = null,
                    lightProbeUsage = LightProbeUsage.Off,
                    matProps = matProp,
                    receiveShadows = false,
                    reflectionProbeUsage = ReflectionProbeUsage.Off,
                };

                Graphics.RenderMesh(renderParams, meshCopy, 0, targetTransform.localToWorldMatrix);
            }
*/
            //if (currentMethod == eMethod.drawMeshInstanced)
            {
                matricesHigh.Clear();
                matricesLow.Clear();

                var ty = targetTransform.position.y;

                var camPos = cameraIn.transform.position;
                camPos.y = ty;
                //var tileSize = matricesRotated[0].lossyScale.x / 2.0f;

                float storeFOV = 0;
                if (adjustFrustumFOV != 0)
                {
                    storeFOV = cameraIn.fieldOfView;
                    cameraIn.fieldOfView += adjustFrustumFOV;
                }

                if (frustumCulling) GeometryUtility.CalculateFrustumPlanes(cameraIn, planes);
                if (adjustFrustumFOV != 0) cameraIn.fieldOfView = storeFOV;

                int i = 0;
                foreach (var m in matricesRotated)
                {
                    i++;

                    var c1 = m.MultiplyPoint(new Vector3(0.5f, 0, -0.5f));
                    var c2 = m.MultiplyPoint(new Vector3(0.5f, 0, 0.5f));
                    var c3 = m.MultiplyPoint(new Vector3(-0.5f, 0, 0.5f));
                    var c4 = m.MultiplyPoint(new Vector3(-0.5f, 0, -0.5f));

                    var d1 = Vector3.Distance(camPos, c1) < maxDist;
                    var d2 = Vector3.Distance(camPos, c2) < maxDist;
                    var d3 = Vector3.Distance(camPos, c3) < maxDist;
                    var d4 = Vector3.Distance(camPos, c4) < maxDist;

                    if (frustumCulling)
                    {
                        var bounds = new Bounds(Vector3.zero, Vector3.one * 2).Transform(m);
                        var isVisible = GeometryUtility.TestPlanesAABB(planes, bounds);
                        if (!isVisible) continue;
                    }

                    /*
                    Random.InitState(i);
                    var col = Random.ColorHSV(0, 1, 1, 1, 1, 1);
                    Debug.DrawLine(c1, c2, col);
                    Debug.DrawLine(c2, c3, col);
                    Debug.DrawLine(c3, c4, col);
                    Debug.DrawLine(c4, c1, col);

                    col *= 0.1f;
                    Debug.DrawLine(c1, camPos, col);
                    Debug.DrawLine(c2, camPos, col);
                    Debug.DrawLine(c3, camPos, col);
                    Debug.DrawLine(c4, camPos, col);
                    */

                    // If any of the 4 corners of the quad are less than the maxDist from camera then put in the matricesHigh for higher LOD
                    if (d1 || d2 || d3 || d4)
                    {
                        matricesHigh.Add(m);
                    }
                    else
                    {
                        matricesLow.Add(m);
                    }
                }

                highCount = matricesHigh.Count;
                lowCount = matricesLow.Count;

                // Max instance count of 1023
                if (matricesHigh.Count > 0 && highMesh) Graphics.DrawMeshInstanced(highMesh, 0, mat, matricesHigh, matProp, shadowCastingMode, receiveShadows, layer, cameraIn, lightProbeUsage, null);
                if (matricesLow.Count > 0 && lowMesh) Graphics.DrawMeshInstanced(lowMesh, 0, mat, matricesLow, matProp, shadowCastingMode, receiveShadows, layer, cameraIn, lightProbeUsage, null);
            }

/*
            if (currentMethod == eMethod.drawMeshInstancedIndirect)
            {
                // https://docs.unity3d.com/ScriptReference/Graphics.DrawMeshInstancedIndirect.html
                if (argsBuffer == null) InitializeBuffers();
                Graphics.DrawMeshInstancedIndirect(highMesh, 0, mat, new Bounds(Vector3.zero, new Vector3(10000, 10000, 10000)), argsBuffer);
            }*/
        }

/*
        private void InitializeBuffers()
        {
            var population = matrices.Length;
            // Argument buffer used by DrawMeshInstancedIndirect.
            uint[] args = new uint[5] {0, 0, 0, 0, 0};
            // Arguments for drawing mesh.
            // 0 == number of triangle indices, 1 == population, others are only relevant if drawing submeshes.
            args[0] = (uint) highMesh.GetIndexCount(0);
            args[1] = (uint) population;
            args[2] = (uint) highMesh.GetIndexStart(0);
            args[3] = (uint) highMesh.GetBaseVertex(0);

            argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
            argsBuffer.SetData(args);

            //

            if (positionBuffer != null) positionBuffer.Release();
            positionBuffer = new ComputeBuffer(population, 16);
            Vector4[] positions = new Vector4[population];
            for (int i = 0; i < population; i++)
            {
                positions[i] = matrices[i].GetPosition();
            }

            positionBuffer.SetData(positions);
            mat.SetBuffer("positionBuffer", positionBuffer);
        }*/
    }
}

#if UNITY_EDITOR

namespace UnityEditor
{
    [CustomEditor(typeof(OceanRenderer)), CanEditMultipleObjects]
    public class OceanRendererEditor : Editor
    {
        private MaterialEditor materialEditor;

        public override void OnInspectorGUI()
        {
            var targ = target as OceanRenderer;
            DrawDefaultInspector();

            if (targ.mat) ExtensionsMisc.DrawMaterialEditor(targ.mat, serializedObject, ref materialEditor);
        }

        protected void OnDisable()
        {
            if (materialEditor != null) DestroyImmediate(materialEditor); // Free the memory used by default MaterialEditor.
        }
    }
}
#endif

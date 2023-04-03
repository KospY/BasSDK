using System;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [ExecuteInEditMode]
    public class BakedLODGroup : MonoBehaviour, ICheckAsset
    {
#if ODIN_INSPECTOR
        [BoxGroup("Source")]
#endif
        public MeshRenderer meshRenderer;

#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [BoxGroup("Source")]
#endif
        public bool applySourceMaterialOnTargets;
#endif

#if ODIN_INSPECTOR
        [BoxGroup("Baking")]
#endif
        public bool applySourceLightmapOnTarget;
#if ODIN_INSPECTOR
        [BoxGroup("Baking"), ShowIf("applySourceLightmapOnTarget")]
#endif
        public MeshRenderer[] targetMeshRenderers = new MeshRenderer[0];

        protected LightingGroup lightingGroup;
        protected float orgLightmapScale = 0;

        private void Awake()
        {
            lightingGroup = this.GetComponentInParent<LightingGroup>();
            foreach (MeshRenderer targetMeshRenderer in targetMeshRenderers)
            {
                if (meshRenderer && targetMeshRenderer && !targetMeshRenderer.isPartOfStaticBatch)
                {
                    // ScaleOffset need to be set in awake to run before staticBatching
                    targetMeshRenderer.lightmapScaleOffset = meshRenderer.lightmapScaleOffset;
                }
            }
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (Lightmapping.isRunning) return;

            if (!applySourceLightmapOnTarget && !applySourceLightmapOnTarget && !useHighPoly)
            {
                Debug.LogWarningFormat(this, "Baked lodgroup is not configured with any options, the component is probably useless");
            }
#endif
            if (!lightingGroup)
            {
                // Apply Lightmaps need to be done OnStart, probably because it need to be after staticBatching
                ApplyLightmaps();
            }
        }

        [Button]
        public void ApplyLightmaps()
        {
            if (applySourceLightmapOnTarget)
            {
                if (meshRenderer == null)
                {
                    Debug.LogWarningFormat(this, "BakeLODGroup - Can't apply lightmap because meshRenderer is null");
                    return;
                }

                foreach (MeshRenderer targetMeshRenderer in targetMeshRenderers)
                {
                    if (targetMeshRenderer == null)
                    {
                        Debug.LogWarningFormat(this, $"BakeLODGroup - Can't apply lightmap to a targetMeshRenderer because it's null");
                        continue;
                    }
                    targetMeshRenderer.lightmapIndex = meshRenderer.lightmapIndex;
                    targetMeshRenderer.realtimeLightmapIndex = meshRenderer.realtimeLightmapIndex;
                    targetMeshRenderer.lightProbeUsage = meshRenderer.lightProbeUsage;
                    targetMeshRenderer.realtimeLightmapScaleOffset = meshRenderer.realtimeLightmapScaleOffset;
                    if (!targetMeshRenderer.isPartOfStaticBatch)
                    {
                        targetMeshRenderer.lightmapScaleOffset = meshRenderer.lightmapScaleOffset;
                    }
#if UNITY_EDITOR
                    targetMeshRenderer.scaleInLightmap = meshRenderer.scaleInLightmap;
                    StaticEditorFlags meshStaticEditorFlags = GameObjectUtility.GetStaticEditorFlags(targetMeshRenderer.gameObject);
                    if (!meshStaticEditorFlags.HasFlag(StaticEditorFlags.ContributeGI))
                    {
                        meshStaticEditorFlags = meshStaticEditorFlags | StaticEditorFlags.ContributeGI;
                        GameObjectUtility.SetStaticEditorFlags(targetMeshRenderer.gameObject, meshStaticEditorFlags);
                    }
                    targetMeshRenderer.receiveGI = ReceiveGI.Lightmaps;
#endif
                }
            }
        }

#if UNITY_EDITOR
        public static bool allowHighPolyBake = true;

#if ODIN_INSPECTOR
        [BoxGroup("Baking")]
#endif
        public bool useHighPoly;
#if ODIN_INSPECTOR
        [BoxGroup("Baking"), ShowInInspector, OnValueChanged("EditorUpdateHighPolyMesh"), InlineButton("EditorClearHighPolyMesh", "Clear"), InlineButton("EditorUnloadCachedHighPolyMesh", "Unload"), InlineButton("EditorLoadCachedHighPolyMesh", "Load"), ShowIf("useHighPoly")]
#endif
        [NonSerialized]
        public Mesh highPolyMeshEditorReference;
#if ODIN_INSPECTORODIN_INSPECTOR
        [BoxGroup("Cached"), ReadOnly] 
#endif
        public string highPolyMeshName;
#if ODIN_INSPECTOR
        [BoxGroup("Cached"), ReadOnly]
#endif
        public string highPolyMeshGUID;

        protected void EditorUpdateHighPolyMesh()
        {
            if (highPolyMeshEditorReference) highPolyMeshName = highPolyMeshEditorReference.name;
            if (highPolyMeshEditorReference) highPolyMeshGUID = UnityEditor.AssetDatabase.GUIDFromAssetPath(UnityEditor.AssetDatabase.GetAssetPath(highPolyMeshEditorReference)).ToString();
        }

        private void EditorLoadCachedHighPolyMesh()
        {
            if (allowHighPolyBake && useHighPoly && highPolyMeshGUID != null && highPolyMeshGUID != "")
            {
                UnityEngine.Object[] objects = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(highPolyMeshGUID));
                //Debug.LogFormat(this, "Load highPoly Mesh EditorReference:" + highPolyMeshName);
                for (int i = 0; i < objects.Length; i++) // loop on all sub-assets loaded
                {
                    if (objects[i].name == highPolyMeshName && objects[i].GetType() == typeof(Mesh)) // if the name AND the type match we found it
                    {
                        highPolyMeshEditorReference = objects[i] as Mesh;
                    }
                }
            }
        }

        protected void EditorUnloadCachedHighPolyMesh()
        {
            highPolyMeshEditorReference = null;
            EditorUtility.UnloadUnusedAssetsImmediate();
        }

        protected void EditorClearHighPolyMesh()
        {
            highPolyMeshName = null;
            highPolyMeshGUID = null;
        }

        private void OnValidate()
        {
            if (!this.gameObject.activeInHierarchy || Application.isPlaying) return;

            if (applySourceMaterialOnTargets && meshRenderer)
            {
                foreach (MeshRenderer targetMeshRenderer in targetMeshRenderers)
                {
                    targetMeshRenderer.sharedMaterials = meshRenderer.sharedMaterials;
                }
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }

        private void OnEnable()
        {
            LightmapBakeHelper.onBakeStarted += OnBakeStarted;
            LightmapBakeHelper.onBakeCompleted += OnBakeCompleted;
        }

        private void OnDisable()
        {
            LightmapBakeHelper.onBakeStarted -= OnBakeStarted;
            LightmapBakeHelper.onBakeCompleted -= OnBakeCompleted;
        }

        Mesh orgSourceMesh;

        private void OnBakeStarted()
        {
            EditorLoadCachedHighPolyMesh();

            if (meshRenderer)
            {
                StaticEditorFlags meshStaticEditorFlags = GameObjectUtility.GetStaticEditorFlags(meshRenderer.gameObject);
                if (!meshStaticEditorFlags.HasFlag(StaticEditorFlags.ContributeGI))
                {
                    meshStaticEditorFlags = meshStaticEditorFlags | StaticEditorFlags.ContributeGI;
                    GameObjectUtility.SetStaticEditorFlags(meshRenderer.gameObject, meshStaticEditorFlags);
                }
                meshRenderer.receiveGI = ReceiveGI.Lightmaps;
            }

            // Reset targets to no lightmaps
            foreach (MeshRenderer targetMeshRenderer in targetMeshRenderers)
            {
                if (targetMeshRenderer == null) continue;
                targetMeshRenderer.lightmapIndex = -1;
                targetMeshRenderer.realtimeLightmapIndex = -1;
                StaticEditorFlags staticEditorFlags = GameObjectUtility.GetStaticEditorFlags(targetMeshRenderer.gameObject);
                if (staticEditorFlags.HasFlag(StaticEditorFlags.ContributeGI))
                {
                    staticEditorFlags = staticEditorFlags & ~(StaticEditorFlags.ContributeGI);
                    GameObjectUtility.SetStaticEditorFlags(targetMeshRenderer.gameObject, staticEditorFlags);
                }
                targetMeshRenderer.receiveGI = ReceiveGI.Lightmaps;
            }

            if (meshRenderer)
            {
                MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
                if (allowHighPolyBake && useHighPoly && highPolyMeshEditorReference)
                {
                    if (orgSourceMesh != meshFilter.sharedMesh)
                    {
                        orgSourceMesh = meshFilter.sharedMesh;
                    }
                    meshFilter.sharedMesh = highPolyMeshEditorReference;
                }
                else if (orgSourceMesh)
                {

                    meshFilter.sharedMesh = orgSourceMesh;
                    orgSourceMesh = null;
                }
            }
        }

        protected void OnBakeCompleted(LightmapBakeHelper.BakeResult bakeResult)
        {
            ResetOrg();
            highPolyMeshEditorReference = null;
            if (bakeResult == LightmapBakeHelper.BakeResult.Successfull) ApplyLightmaps();
        }

        protected void ResetOrg()
        {
            // Return 
            if (orgSourceMesh)
            {
                MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
                meshFilter.sharedMesh = orgSourceMesh;
                orgSourceMesh = null;
            }
            if (orgLightmapScale > 0)
            {
                meshRenderer.scaleInLightmap = orgLightmapScale;
                orgLightmapScale = 0;
            }
        }

        public List<Issue> GetIssues(bool includeLongCheck, bool experimental)
        {
            List<Issue> issues = new List<Issue>();
            if (!meshRenderer)
            {
                issues.Add(new Issue(this, "Source MeshRenderer is null", true));
            }
            else
            {
                MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
                if (meshFilter.sharedMesh)
                {
                    if (meshFilter.sharedMesh.name == highPolyMeshName)
                    {
                        issues.Add(new Issue(this, "Source MeshRenderer seem to still use the highpoly mesh", true));
                    }
                }
                else
                {
                    issues.Add(new Issue(this, "Source MeshRenderer have no mesh", true));
                }
            }
            foreach (MeshRenderer targetMeshRenderer in targetMeshRenderers)
            {
                if (targetMeshRenderer == null)
                {
                    issues.Add(new Issue(this, "Some targetMeshRenderer are null", true));
                }
            }
            return issues;
        }
#endif
    }
}

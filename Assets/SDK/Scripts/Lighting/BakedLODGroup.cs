using System;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/BakedLodGroup.html")]
    [ExecuteInEditMode]
    public class BakedLODGroup : MonoBehaviour, ICheckAsset
    {
#if ODIN_INSPECTOR
        [BoxGroup("Source")]
#endif
        [Tooltip("This will be the head mesh renderer, which will be used for the lightmap, especially if you share lightmaps with other LODs.")]
        public MeshRenderer meshRenderer;
#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [BoxGroup("Source")]
        [Tooltip("If true, the lightmap scale will be scaled by the object's scale")]
#endif
        public bool scaleByObjectScale = true;
#if ODIN_INSPECTOR
        [BoxGroup("Source")]
#endif
        [Tooltip("The minimum lightmap scale of this object when far away from a Player Lightmap Volume")]
        public float minLightMapScale;
#if ODIN_INSPECTOR
        [BoxGroup("Source")]
        [ReadOnly]
        [ShowInInspector]
        [Tooltip("The min lightmap scale scaled by the object's scale")]
#endif        
        private float scaledMinLightMapScale;
#if ODIN_INSPECTOR
        [BoxGroup("Source")]
#endif
        [Tooltip("The maximum lightmap scale of this object when inside a Player Lightmap Volume")]
        public float maxLightMapScale;
#if ODIN_INSPECTOR
        [BoxGroup("Source")]
        [ReadOnly]
        [ShowInInspector]
        [Tooltip("The max lightmap scale scaled by the object's scale")]
#endif        
        private float scaledMaxLightMapScale;
#if ODIN_INSPECTOR
        [BoxGroup("Source")]
#endif
        [Tooltip("The default lightmap scale which was set in the meshRenderer in the prefab")]
        public float defaultLightMapScale;

#if ODIN_INSPECTOR
        [BoxGroup("Source")]
        [ReadOnly]
        [Tooltip("The calculated lightmap scale")]
#endif
        [NonSerialized]
        public float calculatedLightMapScale;

#if ODIN_INSPECTOR
        [BoxGroup("Source")]
#endif
        [Tooltip("When enabled, the material of the LOD0 will share between LODs")]
        public bool applySourceMaterialOnTargets;

#if ODIN_INSPECTOR
        [BoxGroup("Source")]
        [ReadOnly]
        [ShowInInspector]
        [Tooltip("The global scale of the object in the world")]
#endif        
        private Vector3 objectLossyScale;
#if ODIN_INSPECTOR
        [BoxGroup("Source")]
        [ReadOnly]
        [ShowInInspector]
        [Tooltip("The ratio of how much the object is scaled")]
#endif        
        private float objectScaleRatio;


#endif

#if ODIN_INSPECTOR
        [BoxGroup("Baking")]
#endif
        [Tooltip("To save lightmap space, you can put other LODs of this object here, a recommendation if they share the same lightmap UVs.")]
        public bool applySourceLightmapOnTarget;
#if ODIN_INSPECTOR
        [BoxGroup("Baking"), ShowIf("applySourceLightmapOnTarget")]
#endif
        [Tooltip("Put LODs you want to share lightmap scales with here.")]
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

#endif
            if (!lightingGroup)
            {
                // Apply Lightmaps need to be done OnStart, probably because it need to be after staticBatching
                ApplyLightmaps();
            }
        }

#if UNITY_EDITOR
        [Button]
        public void GetMeshRenderer()
        {
            //Variables you only want checked when in a Scene
            if (meshRenderer == null) meshRenderer = this.GetComponent<MeshRenderer>();
            UpdateLightmapScale();
            if (applySourceMaterialOnTargets && meshRenderer)
            {
                foreach (MeshRenderer targetMeshRenderer in targetMeshRenderers)
                {
                    targetMeshRenderer.sharedMaterials = meshRenderer.sharedMaterials;
                }
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif        
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
                    LightmapBakeHelper.SetEditorContributeGI(targetMeshRenderer, true);
#endif
                }
            }
        }

#if UNITY_EDITOR
        public static bool allowHighPolyBake = true;

#if ODIN_INSPECTOR
        [BoxGroup("Baking")]
#endif
        [Tooltip("When baking, the renderer will switch to this model to get a high poly bake, then switch back when baking ends.")]
        public bool useHighPoly;
#if ODIN_INSPECTOR
        [BoxGroup("Baking"), ShowInInspector, OnValueChanged(nameof(EditorUpdateHighPolyMesh)), InlineButton("EditorClearHighPolyMesh", "Clear"), InlineButton("EditorUnloadCachedHighPolyMesh", "Unload"), InlineButton("EditorLoadCachedHighPolyMesh", "Load"), ShowIf("useHighPoly")]
#endif
        [NonSerialized]
        public Mesh highPolyMeshEditorReference;
#if ODIN_INSPECTOR
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

        [UnityEditor.MenuItem("GameObject/BakedLODGroup/Clear min max scale overrides")]
        public static void ClearMinMaxScaleOverrides()
        {
            //get the active selection gameobjects
            GameObject[] selectedObjects = Selection.gameObjects;
            HashSet<BakedLODGroup> bakedLODGroups = new HashSet<BakedLODGroup>();

            for (var i = 0; i < selectedObjects.Length; i++)
            {
                GameObject selectedObject = selectedObjects[i];
                //get all the BakedLODGroups on the selected object and its children
                BakedLODGroup[] selectedBakedLODGroups = selectedObject.GetComponentsInChildren<BakedLODGroup>();
                //add them to the hashset
                foreach (BakedLODGroup selectedBakedLODGroup in selectedBakedLODGroups)
                {
                    bakedLODGroups.Add(selectedBakedLODGroup);
                }
            }
            //loop over the hashset 
            foreach (BakedLODGroup bakedLODGroup in bakedLODGroups)
            {
                GameObject gameObject = bakedLODGroup.gameObject;
                string pathToPrefab = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
                if (!string.IsNullOrEmpty(pathToPrefab))
                {
                    var prefabObject = PrefabUtility.GetCorrespondingObjectFromSourceAtPath(gameObject, pathToPrefab);
                    if (prefabObject != null && prefabObject.TryGetComponentInChildren(out BakedLODGroup prefabBakedLODGroup) && prefabBakedLODGroup.meshRenderer)
                    {
                        Debug.Log($"Setting BakedLODGroup on {gameObject.name} to use the min max scale overrides from the prefab");
                        bakedLODGroup.minLightMapScale = prefabBakedLODGroup.minLightMapScale;
                        bakedLODGroup.maxLightMapScale = prefabBakedLODGroup.maxLightMapScale;
                        //mark the object as dirty so the changes are saved
                        UnityEditor.EditorUtility.SetDirty(bakedLODGroup);
                    }
                }
            }

        }


        private void OnValidate()
        {
            if (!this.gameObject.activeInHierarchy || Application.isPlaying) return;
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
            bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(this.gameObject) == PrefabInstanceStatus.Connected;
            if (!isValidPrefabStage && prefabConnected)
            {
                GetMeshRenderer();
            }

        }

        [Button]
        public void UpdateLightmapScale()
        {
            if(Application.isPlaying) return;
            bool inPrefabStage = PrefabStageUtility.GetCurrentPrefabStage() != null;
            if (!meshRenderer) return;
            //clamp the min and max lightmap scales so they are always positive
            if (minLightMapScale < 0) minLightMapScale = 0;
            if (maxLightMapScale < 0) maxLightMapScale = 0;
            if (defaultLightMapScale < 0) defaultLightMapScale = 0;
            //if the default is zero, get the original prefabs values
            if (defaultLightMapScale == 0 || minLightMapScale == 0 || maxLightMapScale == 0)
            {
                if (inPrefabStage)
                {
                    //we are in the prefab stage, so if there are any invalid values, we need to reset these, since we ARE the prefab
                    defaultLightMapScale = meshRenderer.scaleInLightmap;
                    minLightMapScale = 0.1f;
                    maxLightMapScale = 1;
                }
                else
                {
                    //we are not in the prefab stage, so this is a object on the scene, so we need to get the values from the prefab
                    string pathToPrefab = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(this.gameObject);
                    if (!string.IsNullOrEmpty(pathToPrefab))
                    {
                        var prefabObject = PrefabUtility.GetCorrespondingObjectFromSourceAtPath(this.gameObject, pathToPrefab);
                        if (prefabObject != null && prefabObject.TryGetComponentInChildren(out BakedLODGroup prefabBakedLODGroup) && prefabBakedLODGroup.meshRenderer)
                        {
                            if (defaultLightMapScale == 0) defaultLightMapScale = prefabBakedLODGroup.meshRenderer.scaleInLightmap;
                            if (minLightMapScale == 0) minLightMapScale = prefabBakedLODGroup.minLightMapScale;
                            if (maxLightMapScale == 0) maxLightMapScale = prefabBakedLODGroup.maxLightMapScale;
                        }
                    }
                }
            }
            //if the lightmap scales havent been set yet, set them to 1 to be very safe as the mesh renderer could have been blown out of proportion
            if (defaultLightMapScale == 0) defaultLightMapScale = 1;
            if (calculatedLightMapScale == 0) calculatedLightMapScale = 1;
            if (minLightMapScale == 0) minLightMapScale = 0.1f;
            if (maxLightMapScale == 0) maxLightMapScale = 1;
            //clamp the min so it is always smaller than the max
            minLightMapScale = Mathf.Min(minLightMapScale, maxLightMapScale);
            //clamp the max so it is always larger than the min
            maxLightMapScale = Mathf.Max(minLightMapScale, maxLightMapScale);

            if (inPrefabStage)
            {
                meshRenderer.scaleInLightmap = Mathf.Clamp(meshRenderer.scaleInLightmap, minLightMapScale, maxLightMapScale);
                calculatedLightMapScale = 1;
                scaledMinLightMapScale = 1;
                scaledMaxLightMapScale = 1;
                objectScaleRatio = 1;
            }
            else
            {
                //clamp the calculated scale so it is always between the min and max
                CalculateLightMapScale();
                meshRenderer.scaleInLightmap = calculatedLightMapScale;
            }
            //mark the object as dirty so the changes are saved
            UnityEditor.EditorUtility.SetDirty(meshRenderer);
            UnityEditor.EditorUtility.SetDirty(this);
        }

        // Just used for debugging and gizmo drawing
#if UNITY_EDITOR
        [NonSerialized]
        public PlayerLightMapVolume closestLightMapVolume = null;
        [NonSerialized]
        Vector3 closestPositionMesh = Vector3.zero;
        [NonSerialized]
        Vector3 closestPositionOnVolume = Vector3.zero;
        public static bool MinLightMapScaleBake = false;
#endif
        public void CalculateLightMapScale()
        {
            if (meshRenderer == null) return;
            if (!meshRenderer.TryGetComponent(out MeshFilter meshFilter)) return;

            //if the asset manager is set to do a min lightmap scale bake then we just use the minimum lightmap scale
            if (MinLightMapScaleBake)
            {
                calculatedLightMapScale = minLightMapScale;
                return;
            }
            Transform meshRendererTransform = meshRenderer.transform;
            //get the PlayerLightMapVolumes
            var lightMapVolumes = PlayerLightMapVolume.playerLightMapVolumes;
            Bounds meshRendererBounds = meshRenderer.bounds;
            //find the closest LightMapVolume from the mesh renders bounds
            closestPositionMesh = Vector3.zero;
            closestPositionOnVolume = Vector3.zero;
            float closestDistance = float.MaxValue;
            closestLightMapVolume = null;
            foreach (var lightMapVolume in lightMapVolumes)
            {
                if (!lightMapVolume.gameObject.activeInHierarchy) continue;
                if (!lightMapVolume.boxCollider) continue;

                //if the gameobjects bounds is entirely inside the collider, the distance is 0
                float distance;
                Vector3 closestPointOnMesh = meshRendererBounds.center;
                Vector3 closestPointOnVolume = lightMapVolume.boxCollider.bounds.center;

                if (!lightMapVolume.boxCollider.bounds.Contains(meshRendererBounds.min) || !lightMapVolume.boxCollider.bounds.Contains(meshRendererBounds.max))
                {
                    //otherwise calculate the closest point on the mesh renders bounds to the light map volumes collider

                    //get the closestpoint on the meshrenders bounds to the light map volumes collider

                    //this is a two phased approach, we find the closest point based on their centers.
                    closestPointOnMesh = meshRendererBounds.ClosestPoint(closestPointOnVolume);
                    closestPointOnVolume = lightMapVolume.boxCollider.ClosestPoint(closestPointOnMesh);
                    //then we do it again but using the point on the bounds
                    closestPointOnMesh = meshRendererBounds.ClosestPoint(closestPointOnVolume);
                    closestPointOnVolume = lightMapVolume.boxCollider.ClosestPoint(closestPointOnMesh);

                    distance = Vector3.Distance(closestPointOnVolume, closestPointOnMesh);
                }
                else
                {
                    distance = 0;
                }

                if (distance <= closestDistance)
                {
                    //if the distances are  the same, check which has the higher priority
                    if (closestLightMapVolume != null && Math.Abs(distance - closestDistance) < 0.0001f)
                    {
                        if (lightMapVolume.priority > closestLightMapVolume.priority)
                        {
                            closestDistance = distance;
                            closestPositionMesh = closestPointOnMesh;
                            closestPositionOnVolume = closestPointOnVolume;
                            closestLightMapVolume = lightMapVolume;
                        }
                    }
                    else
                    {
                        closestDistance = distance;
                        closestPositionMesh = closestPointOnMesh;
                        closestPositionOnVolume = closestPointOnVolume;
                        closestLightMapVolume = lightMapVolume;
                    }
                }
            }

            calculatedLightMapScale = defaultLightMapScale;
            //calculate the falloff for the lightmap scale multiplier, it should reduce as closest distance increases
            if (closestLightMapVolume != null)
            {
                float falloff = 1 - Mathf.Clamp01(closestDistance / closestLightMapVolume.lightMapFalloffDistance);
                var calculatedMultiplier = closestLightMapVolume.lightMapScaleMultiplier * falloff;
                if (calculatedMultiplier <= 0) calculatedMultiplier = Mathf.Epsilon;
                calculatedLightMapScale *= calculatedMultiplier;
            }

            objectScaleRatio = 1.0f;
            if (scaleByObjectScale)
            {
                //if an object's min lightmap scale is 1, but the artist scales the object itself up 2x in the scene, then the min lightmap scale can safely be changed to 0.5
                //Take into account the objects scale, so the lightmap scale is relative to the objects scale
                objectLossyScale = meshRendererTransform.lossyScale;
                objectScaleRatio = CalculateObjectScaleRatio(meshRendererTransform);
            }
            //we want to scale the min and max lightmap scales by the objects scale
            scaledMinLightMapScale = minLightMapScale * objectScaleRatio;
            scaledMaxLightMapScale = maxLightMapScale * objectScaleRatio;

            // //the object could be far away from a PLV so the actual calculatedLightMapScale could be zero, in that case, we need to clamp it to the min/max first
            // calculatedLightMapScale = Mathf.Clamp(scaledCalculatedLightMapScale, minLightMapScale, maxLightMapScale);
            //
            // the calculated lightmap scale is inversely proportional to the scale of the object
            float scaledCalculatedLightMapScale = calculatedLightMapScale * objectScaleRatio;

            // then we can clamp it to the scaled min/max
            scaledCalculatedLightMapScale = Mathf.Clamp(scaledCalculatedLightMapScale, scaledMinLightMapScale, scaledMaxLightMapScale);

            calculatedLightMapScale = scaledCalculatedLightMapScale;

        }

        //A method which will take the lossy scale of an object, and calculate the ratio of how much it is under or overscaled from 1,1,1
        private float CalculateObjectScaleRatio(Transform objectsTransform)
        {
            Vector3 transformLossyScale = objectsTransform.lossyScale;
            transformLossyScale.x = 1.0f / transformLossyScale.x;
            transformLossyScale.y = 1.0f / transformLossyScale.y;
            transformLossyScale.z = 1.0f / transformLossyScale.z;
            //get the average of the components, retain float precision
            float averageScale = (transformLossyScale.x + transformLossyScale.y + transformLossyScale.z + Mathf.Epsilon) / 3.0f;
            if (averageScale <= 0) averageScale = Mathf.Epsilon;
            return averageScale;
        }

        private void OnEnable()
        {
            if(Application.isPlaying) return;
            LightmapBakeHelper.onBakeStarted += OnBakeStarted;
            LightmapBakeHelper.onBakeCompleted += OnBakeCompleted;
            UpdateLightmapScale();
        }

        private void OnDisable()
        {
            if(Application.isPlaying) return;
            LightmapBakeHelper.onBakeStarted -= OnBakeStarted;
            LightmapBakeHelper.onBakeCompleted -= OnBakeCompleted;
        }

        Mesh orgSourceMesh;

        private void OnBakeStarted()
        {
            EditorLoadCachedHighPolyMesh();

            if (meshRenderer)
            {
                LightmapBakeHelper.SetEditorContributeGI(meshRenderer, true);
            }

            // Reset targets to no lightmaps
            foreach (MeshRenderer targetMeshRenderer in targetMeshRenderers)
            {
                if (targetMeshRenderer == null) continue;
                targetMeshRenderer.lightmapIndex = -1;
                targetMeshRenderer.realtimeLightmapIndex = -1;
                LightmapBakeHelper.SetEditorContributeGI(targetMeshRenderer, false);
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

        [NonSerialized]
        int lastFrameDrawnGizmos = -1;
        [NonSerialized]
        MeshFilter cachedMeshFilter;
        [NonSerialized]
        Mesh cachedMesh;
        [NonSerialized]
        Transform cachedMeshTransform;
        
#if UNITY_EDITOR
        Color red = Color.red;
        Color green = Color.green;
        public void OnDrawGizmosSelected()
        {

            if (PlayerLightMapVolume.drawLinesToVolumes)
            {
                Gizmos.color = Color.magenta;
                // Gizmos.DrawSphere(closestPositionMesh, 0.1f);
                // Gizmos.DrawSphere(closestPositionOnVolume, 0.1f);
                Gizmos.DrawLine(closestPositionMesh, closestPositionOnVolume);
            }
            
            if (PlayerLightMapVolume.drawLightmapScaleColors)
            {
                if (meshRenderer is null) return;
                if (cachedMeshFilter is null && !meshRenderer.TryGetComponent(out cachedMeshFilter)) return;
                if (cachedMesh is null) cachedMesh = cachedMeshFilter.sharedMesh;
                if (cachedMesh is null) return;
                if (cachedMeshTransform is null) cachedMeshTransform = meshRenderer.transform;
                if (cachedMeshTransform is null) return;
                //set the colour based on the mesh renderer's scale in lightmap
                Color gizmoColor = Color.Lerp(red, green, calculatedLightMapScale);
                Gizmos.color = gizmoColor;
                Gizmos.DrawMesh(cachedMesh, cachedMeshTransform.position, cachedMeshTransform.rotation, cachedMeshTransform.lossyScale);
            }
            // Bounds meshRendererBounds = meshRenderer.bounds;
            // Gizmos.DrawWireCube(meshRendererBounds.center, meshRendererBounds.size);
            
            //     Vector3 labelPosition = meshRendererBounds.center;
            //     labelPosition.y += meshRendererBounds.extents.y;
            //     Handles.Label(labelPosition, $"LightMapVolume: {this.name}\nDefault Scale:{defaultLightMapScale}\nCalculated Scale: {calculatedLightMapScale}");
            //
            
            if (PlayerLightMapVolume.updateSelectedLightmapScale)
            {
                int frameCount = Time.frameCount;
                if (lastFrameDrawnGizmos == frameCount) return;
                lastFrameDrawnGizmos = frameCount;
                UpdateLightmapScale();
            }
        }
#endif


#endif
    }
}

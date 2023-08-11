using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

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
    public class LightingGroup : MonoBehaviour, ICheckAsset
    {
        public static List<LightingGroup> allActive = new List<LightingGroup>();

        public static LightingGroup sceneSettingsMaster;

        private static Action _updateRenderSettingEvent;
        public static event Action UpdateRenderSettingEvent
        {
            remove
            {
                _updateRenderSettingEvent -= value;
            }

            add
            {
                _updateRenderSettingEvent -= value;
                _updateRenderSettingEvent += value;
            }
        }

#if ODIN_INSPECTOR
        [TableList, InlineButton("UpdateReferencesAndSaveAll", "Save all")]
#endif
        public LightingPreset lightingPreset;

        public bool changeLightmapFormatForAndroid;

#if ODIN_INSPECTOR
        [TableList, InlineButton("UpdateReferences")]
#endif
        public List<MeshRendererReference> meshRendererReferences = new List<MeshRendererReference>();

        [Serializable]
        public class MeshRendererReference
        {
            public string guid;
            public MeshRenderer meshRenderer;
            public MeshRendererReference(MeshRenderer meshRenderer)
            {
                this.guid = Guid.NewGuid().ToString();
                this.meshRenderer = meshRenderer;
            }
        }

#if ODIN_INSPECTOR
        [TableList]
#endif
        public List<LightReference> lightReferences = new List<LightReference>();

        [Serializable]
        public class LightReference
        {
            public string guid;
            public Light light;
            public LightReference(Light light)
            {
                this.guid = Guid.NewGuid().ToString();
                this.light = light;
            }
        }

#if ODIN_INSPECTOR
        [TableList, InlineButton("BakeProbeVolumes", "Bake probe volumes")]
#endif
        public List<LightProbeVolumeReference> lightProbeVolumeReferences = new List<LightProbeVolumeReference>();

        [Serializable]
        public class LightProbeVolumeReference
        {
            public string guid;
            public LightProbeVolume lightProbeVolume;
            public LightProbeVolumeReference(LightProbeVolume lightProbeVolume)
            {
                this.guid = Guid.NewGuid().ToString();
                this.lightProbeVolume = lightProbeVolume;
            }
        }

#if ODIN_INSPECTOR
        [TableList]
#endif
        public List<ReflectionProbeReference> reflectionProbeReferences = new List<ReflectionProbeReference>();

        [Serializable]
        public class ReflectionProbeReference
        {
            public string guid;
            public ReflectionProbe reflectionProbe;
            public ReflectionProbeReference(ReflectionProbe reflectionProbe)
            {
                this.guid = Guid.NewGuid().ToString();
                this.reflectionProbe = reflectionProbe;
            }
        }

        protected LightingPreset currentLightingPreset;
        protected List<BakedLODGroup> bakedLODGroups;

        private bool isStarted = false;

        [NonSerialized]
        public bool initialized;

        public bool initializedDebug;
        private Dictionary<string, MeshRenderer> _meshRendererByGuid = null;

        public class LightmapIndexHelper
        {
            private List<LightmapData> _lightmaps = null;
            private Queue<int> _freeIndex = null;
            private Dictionary<int, int> _lightmapIndexCountMapping = null;
            private LightmapData _defaultEmptyLightmap = null;
            public LightmapIndexHelper()
            {
                _freeIndex = new Queue<int>();
                _lightmapIndexCountMapping = new Dictionary<int, int>();
                _defaultEmptyLightmap = new LightmapData();

                bool allLightmapNull = true;
                LightmapData[] lightmaps = LightmapSettings.lightmaps;
                for (int i = 0; i < lightmaps.Length; i++)
                {
                    if (lightmaps[i].lightmapColor == null
                        && lightmaps[i].lightmapDir == null
                        && lightmaps[i].shadowMask == null)
                    {
                        lightmaps[i] = _defaultEmptyLightmap;
                        _freeIndex.Enqueue(i);
                    }
                    else
                    {
                        allLightmapNull = false;
                    }
                }

                if (allLightmapNull)
                {
                    _freeIndex.Clear();
                    LightmapSettings.lightmaps = null;
                }
                else
                {
                    LightmapSettings.lightmaps = lightmaps;
                }
            }

            public void InitFromLightmap()
            {
                _lightmaps = new List<LightmapData>(LightmapSettings.lightmaps);
            }

            public void SetLightmap()
            {
                if (_lightmaps == null) return;
                LightmapSettings.lightmaps = _lightmaps.ToArray();

                _lightmaps = null;
            }

            public void SetMeshLightmapIndex(List<MeshRenderer> meshList, LightmapData lightmapData)
            {

                if (_lightmaps == null) return;

                int meshCount = meshList.Count;

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    // Search if lightmap already exist
                    for (int i = 0; i < _lightmaps.Count; i++)
                    {
                        if (_lightmaps[i].lightmapColor == lightmapData.lightmapColor)
                        {
                            for (int indexMesh = 0; indexMesh < meshCount; indexMesh++)
                            {
                                meshList[indexMesh].lightmapIndex = i;
                            }

                            return;
                        }
                    }

                    _lightmaps.Add(lightmapData);

                    for (int indexMesh = 0; indexMesh < meshCount; indexMesh++)
                    {
                        meshList[indexMesh].lightmapIndex = _lightmaps.Count - 1;
                    }

                    return;
                }
#endif //UNITY_EDITOR

                // Search if lightmap already exist
                for (int i = 0; i < _lightmaps.Count; i++)
                {
                    if (_lightmaps[i] == _defaultEmptyLightmap) continue;
                    if (!_lightmapIndexCountMapping.ContainsKey(i)) continue;

                    if (_lightmaps[i].lightmapColor == lightmapData.lightmapColor)
                    {
                        for (int indexMesh = 0; indexMesh < meshCount; indexMesh++)
                        {
                            meshList[indexMesh].lightmapIndex = i;
                            _lightmapIndexCountMapping[i]++;
                        }

                        return;
                    }
                }

                // Need to add lightmap
                int index = _lightmaps.Count;
                // search if there is an empty slot
                while (_freeIndex.Count > 0)
                {
                    // FreeSpace
                    index = _freeIndex.Dequeue();
                    if (index < 0) continue;
                    if (index >= _lightmaps.Count) continue;

                    if (_lightmaps[index] != _defaultEmptyLightmap) continue;

                    _lightmaps[index] = lightmapData;
                    _lightmapIndexCountMapping.Add(index, 0);

                    for (int indexMesh = 0; indexMesh < meshCount; indexMesh++)
                    {
                        meshList[indexMesh].lightmapIndex = index;
                        _lightmapIndexCountMapping[index]++;
                    }

                    return;
                }

                index = _lightmaps.Count;
                _lightmaps.Add(lightmapData);
                _lightmapIndexCountMapping.Add(index, 0);

                for (int indexMesh = 0; indexMesh < meshCount; indexMesh++)
                {
                    meshList[indexMesh].lightmapIndex = index;
                    _lightmapIndexCountMapping[index]++;
                }
            }

            public void RemoveLightmap(MeshRenderer mesh)
            {
                if (_lightmaps == null) return;

                int index = mesh.lightmapIndex;
                if (_lightmapIndexCountMapping.ContainsKey(index))
                {
                    _lightmapIndexCountMapping[mesh.lightmapIndex]--;
                    if (_lightmapIndexCountMapping[index] <= 0)
                    {
                        // release
                        _lightmaps[index].lightmapColor = null;
                        _lightmaps[index].lightmapDir = null;
                        _lightmaps[index].shadowMask = null;
                        _lightmaps[index] = _defaultEmptyLightmap;
                        _freeIndex.Enqueue(index);
                        _lightmapIndexCountMapping.Remove(index);

                        if (_lightmapIndexCountMapping.Count == 0)
                        {
                            _freeIndex.Clear();
                            _lightmaps.Clear();
                        }
                    }
                }
            }
        }

        private static LightmapIndexHelper lightmapIndexHelper;
        public static void FreeLightmapIndexHelper()
        {
            lightmapIndexHelper = null;
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;
#if UNITY_EDITOR            
            if (UnityEditor.BuildPipeline.isBuildingPlayer) return;
#endif            
            if (this.InPrefabScene()) return;
            if (currentLightingPreset != lightingPreset)
            {
                if (currentLightingPreset)
                {
                    ClearAll(currentLightingPreset);
                }
                if (lightingPreset)
                {
                    bakedLODGroups = new List<BakedLODGroup>(this.GetComponentsInChildren<BakedLODGroup>());
                    ApplyAll();
                }
            }
        }

        private void Awake()
        {
            GetBakedLODGroups();
            if (Application.isPlaying && lightingPreset)
            {
                foreach (LightingPreset.MeshRendererData meshRendererData in lightingPreset.rendererDataListForLightmaps)
                {
                    if (TryGetMeshRenderer(meshRendererData.meshRendererGuid, out MeshRenderer meshRenderer))
                    {
                        if (meshRenderer == null) continue;
                        if (!meshRenderer.isPartOfStaticBatch)
                        {
                            // ScaleOffset need to be set in awake to run before staticBatching
                            meshRenderer.lightmapScaleOffset = meshRendererData.offsetScale;
                        }
                    }
                }
            }
        }

        public void Start()
        {
            if (lightingPreset != null)
            {
                ApplyAll();
            }

            isStarted = true;
        }

        private void OnEnable()
        {
            if (!isStarted) return;
            if (lightingPreset != null)
            {
                ApplyAll();
            }
#if UNITY_EDITOR
            LightmapBakeHelper.onBakeStarted += OnBakeStarted;
#endif
        }

        private void OnDisable()
        {
            allActive.Remove(this);
            ClearAll();
#if UNITY_EDITOR
            LightmapBakeHelper.onBakeStarted -= OnBakeStarted;
#endif
        }

        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                ClearAll();
            }
        }

        private void GetBakedLODGroups()
        {
            //only get the components if its empty
            bakedLODGroups ??= new List<BakedLODGroup>();
            if (bakedLODGroups.Count == 0)
            {
                this.GetComponentsInChildren<BakedLODGroup>(bakedLODGroups);
            }
        }

        [Button, ContextMenu("ApplyAll")]
        public void ApplyAll()
        {
#if UNITY_EDITOR
            if (Application.isPlaying && initialized) return;
#else
            if (initialized) return;
#endif //UNITY_EDITOR

            if (allActive != null) allActive.Add(this);
            ApplyLightmaps(false);
            ApplyLightProbeVolumes(false);
            ApplySceneSettings(false);
            if (lightingPreset) Debug.LogFormat(this, "Apply lightmaps, lightProbe and Scene settings from " + lightingPreset.name + " on lightingGroup " + this.name);
            initialized = true;
        }

        public void ApplyPresetWithoutSceneSettings(LightingPreset preset)
        {
            if (lightingPreset != null) return;
            lightingPreset = preset;
            ApplyLightmaps();
            ApplyLightProbeVolumes();
        }

        public void ApplyLightmaps(bool showDebugLine = true)
        {
        }

        public void ApplyLightProbeVolumes(bool showDebugLine = true)
        {
            if (lightingPreset == null)
            {
                Debug.LogWarningFormat(this, "Cannot apply lighting data as LightingPreset field is null");
                return;
            }

            if (showDebugLine) Debug.LogFormat(this, "Apply lightProbeVolumes from " + lightingPreset.name + " on lightingGroup " + this.name);

            currentLightingPreset = lightingPreset;

            foreach (LightingPreset.LightmapLightData lightmapLightData in lightingPreset.lightmapLights)
            {
                if (TryGetLight(lightmapLightData.lightGuid, out Light light))
                {
                    if (light == null) continue;
                    LightBakingOutput bakingOutput = new LightBakingOutput();
                    bakingOutput.isBaked = true;
                    bakingOutput.lightmapBakeType = (LightmapBakeType)lightmapLightData.baketype;
                    bakingOutput.mixedLightingMode = (MixedLightingMode)lightmapLightData.mixedLightingMode;
                    bakingOutput.probeOcclusionLightIndex = lightmapLightData.probeOcclusionLightIndex;
                    bakingOutput.occlusionMaskChannel = lightmapLightData.occlusionMaskChannel;
                    light.bakingOutput = bakingOutput;
                }
            }

            foreach (LightingPreset.ReflectionProbeData reflectionProbeData in lightingPreset.reflectionProbes)
            {
                if (TryGetReflectionProbe(reflectionProbeData.reflectionProbeGuid, out ReflectionProbe reflectionProbe))
                {
                    if (reflectionProbe == null) continue;
                    reflectionProbe.bakedTexture = reflectionProbeData.texture;
                }
            }

            foreach (LightingPreset.LightProbeVolumeData lightProbeVolumeData in lightingPreset.lightProbeVolumes)
            {
                if (TryGetLightProbeVolume(lightProbeVolumeData.lightProbeVolumeGuid, out LightProbeVolume lightProbeVolume))
                {
                    if (lightProbeVolume != null)
                    {
                        lightProbeVolume.SetTexture(lightProbeVolumeData.SHAr, lightProbeVolumeData.SHAg, lightProbeVolumeData.SHAb, lightProbeVolumeData.occ);
                    }
                }
            }
        }

        /// <summary>
        /// Apply only scene settings
        /// </summary>
        [Button("Apply scene settings only")]
        public void ApplySceneSettings(bool showDebugLine = true, bool applySun = true)
        {
            ApplySceneSettings(lightingPreset, showDebugLine);
        }

        /// <summary>
        /// Apply only scene settings
        /// </summary>
        /// <param name="lightingPreset">Reference of the lightingPreset asset where the settings should retrieved</param>
        public void ApplySceneSettings(LightingPreset lightingPreset, bool showDebugLine = true, bool reset = true, bool applySun = true)
        {
            if (lightingPreset == null)
            {
                Debug.LogWarningFormat(this, "Cannot apply scene settings, no lightingPreset is referenced");
                return;
            }

            if (reset) ResetSceneSettings();

            if (showDebugLine) Debug.LogFormat(this, "Apply Scene Settings from " + lightingPreset.name);

            LightingPreset copyLevelCurrentPreset = null;
            if (Application.isPlaying && Level.current != null)
            {
                if (Level.current.currentLightingPreset == null)
                {
                    copyLevelCurrentPreset = ScriptableObject.CreateInstance<LightingPreset>();
                    Level.current.currentLightingPreset = copyLevelCurrentPreset;
                    copyLevelCurrentPreset.applyAtRuntime = false;
                    copyLevelCurrentPreset.fog = LightingPreset.State.Disabled;
                    copyLevelCurrentPreset.skybox = LightingPreset.State.Disabled;
                    copyLevelCurrentPreset.clouds = LightingPreset.State.Disabled;
                }
                else
                {
                    copyLevelCurrentPreset = Level.current.currentLightingPreset;
                }
            }
            else
            {
                // create a dummy to avoid compile error
                copyLevelCurrentPreset = ScriptableObject.CreateInstance<LightingPreset>();
            }



            RenderSettings.ambientIntensity = lightingPreset.ambientIntensity;
            copyLevelCurrentPreset.ambientIntensity = lightingPreset.ambientIntensity;
            RenderSettings.subtractiveShadowColor = lightingPreset.shadowColor;
            copyLevelCurrentPreset.shadowColor = lightingPreset.shadowColor;

#if UNITY_EDITOR
            Lightmapping.lightingSettings.indirectScale = lightingPreset.indirectIntensity;
            copyLevelCurrentPreset.indirectIntensity = lightingPreset.indirectIntensity;

            Lightmapping.lightingSettings.aoExponentIndirect = lightingPreset.AOIndirectContribution;
            copyLevelCurrentPreset.AOIndirectContribution = lightingPreset.AOIndirectContribution;

            Lightmapping.lightingSettings.aoExponentDirect = lightingPreset.AODirectContribution;
            copyLevelCurrentPreset.AODirectContribution = lightingPreset.AODirectContribution;
#endif

            if (applySun
                && !Application.isPlaying || lightingPreset.applyAtRuntime)
            {
                if (RenderSettings.sun)
                {
                    copyLevelCurrentPreset.applyAtRuntime = true;
                    RenderSettings.sun.color = lightingPreset.dirLightColor;
                    copyLevelCurrentPreset.dirLightColor = lightingPreset.dirLightColor;

                    RenderSettings.sun.intensity = lightingPreset.dirLightIntensity;
                    copyLevelCurrentPreset.dirLightIntensity = lightingPreset.dirLightIntensity;

                    RenderSettings.sun.bounceIntensity = lightingPreset.dirLightIndirectMultiplier;
                    copyLevelCurrentPreset.dirLightIndirectMultiplier = lightingPreset.dirLightIndirectMultiplier;

                    RenderSettings.sun.transform.rotation = this.transform.TransformRotation(lightingPreset.directionalLightLocalRotation);
                    copyLevelCurrentPreset.directionalLightLocalRotation = this.transform.TransformRotation(lightingPreset.directionalLightLocalRotation);
                }
                else
                {
                    Debug.LogError("Scene have no sun source set in lighting parameters");
                }
            }

            if (lightingPreset.fog == LightingPreset.State.Enabled)
            {
                lightingPreset.ValidateFogParameters();

                RenderSettings.fog = true;
                copyLevelCurrentPreset.fog = LightingPreset.State.Enabled;

                RenderSettings.fogColor = lightingPreset.fogColor;
                copyLevelCurrentPreset.fogColor = lightingPreset.fogColor;

                RenderSettings.fogStartDistance = lightingPreset.fogStartDistance;
                copyLevelCurrentPreset.fogStartDistance = lightingPreset.fogStartDistance;

                RenderSettings.fogEndDistance = lightingPreset.fogEndDistance;
                copyLevelCurrentPreset.fogEndDistance = lightingPreset.fogEndDistance;
            }
            else if (lightingPreset.fog == LightingPreset.State.Disabled)
            {
                RenderSettings.fog = false;
                copyLevelCurrentPreset.fog = LightingPreset.State.Disabled;
            }

            if (lightingPreset.skybox == LightingPreset.State.Enabled)
            {
                copyLevelCurrentPreset.skyBoxMaterial = lightingPreset.skyBoxMaterial;
                if (lightingPreset.skyBoxMaterial) RenderSettings.skybox = lightingPreset.skyBoxMaterial;
                if (Clouds.instance)
                {
                    copyLevelCurrentPreset.skybox = LightingPreset.State.Enabled;

                    MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                    Clouds.instance.meshRenderer.GetPropertyBlock(materialPropertyBlock);

                    materialPropertyBlock.SetFloat("_SunSize", lightingPreset.skyBoxSunSize);
                    copyLevelCurrentPreset.skyBoxSunSize = lightingPreset.skyBoxSunSize;

                    materialPropertyBlock.SetFloat("_SunSizeConvergence", lightingPreset.skyBoxSunConvergence);
                    copyLevelCurrentPreset.skyBoxSunConvergence = lightingPreset.skyBoxSunConvergence;

                    materialPropertyBlock.SetFloat("_AtmosphereThickness", lightingPreset.skyBoxAtmosphereThickness);
                    copyLevelCurrentPreset.skyBoxAtmosphereThickness = lightingPreset.skyBoxAtmosphereThickness;

                    materialPropertyBlock.SetColor("_SkyTint", lightingPreset.skyBoxSkyTint);
                    copyLevelCurrentPreset.skyBoxSkyTint = lightingPreset.skyBoxSkyTint;

                    materialPropertyBlock.SetColor("_GroundColor", lightingPreset.skyBoxGroundTint);
                    copyLevelCurrentPreset.skyBoxGroundTint = lightingPreset.skyBoxGroundTint;

                    materialPropertyBlock.SetFloat("_Exposure", lightingPreset.skyBoxExposure);
                    copyLevelCurrentPreset.skyBoxExposure = lightingPreset.skyBoxExposure;

                    Clouds.instance.meshRenderer.SetPropertyBlock(materialPropertyBlock);
                }
            }
            else if (lightingPreset.skybox == LightingPreset.State.Disabled)
            {
                RenderSettings.skybox = null;
                copyLevelCurrentPreset.skybox = LightingPreset.State.Disabled;
            }

            if (Clouds.instance && Clouds.instance.meshRenderer)
            {
                if (lightingPreset.clouds == LightingPreset.State.Enabled)
                {
                    copyLevelCurrentPreset.clouds = LightingPreset.State.Enabled;

                    Clouds.instance.meshRenderer.enabled = true;
                    MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                    Clouds.instance.meshRenderer.GetPropertyBlock(materialPropertyBlock);

                    materialPropertyBlock.SetFloat("_CloudSoftness", lightingPreset.cloudsSoftness);
                    copyLevelCurrentPreset.cloudsSoftness = lightingPreset.cloudsSoftness;

                    materialPropertyBlock.SetFloat("_Speed", lightingPreset.cloudsSpeed);
                    copyLevelCurrentPreset.cloudsSpeed = lightingPreset.cloudsSpeed;

                    materialPropertyBlock.SetFloat("_Size", lightingPreset.cloudsSize);
                    copyLevelCurrentPreset.cloudsSize = lightingPreset.cloudsSize;

                    materialPropertyBlock.SetColor("_Color", lightingPreset.cloudsColor);
                    copyLevelCurrentPreset.cloudsColor = lightingPreset.cloudsColor;

                    materialPropertyBlock.SetFloat("_Alpha", lightingPreset.cloudsAlpha);
                    copyLevelCurrentPreset.cloudsAlpha = lightingPreset.cloudsAlpha;

                    Clouds.instance.meshRenderer.SetPropertyBlock(materialPropertyBlock);
                }
                else if (lightingPreset.clouds == LightingPreset.State.Disabled)
                {
                    Clouds.instance.meshRenderer.enabled = false;
                    copyLevelCurrentPreset.clouds = LightingPreset.State.Disabled;
                }
            }
            sceneSettingsMaster = this;

            if (_updateRenderSettingEvent != null) _updateRenderSettingEvent.Invoke();
        }

        /// <summary>
        /// Apply only scene settings
        /// </summary>
        /// <param name="t">When t is 0 current level preset is fully apply, when t is 1 this lighting preset is fully apply</param>
        public void BlendSceneSettingsWithCurrent(float t, bool showDebugLine = true, bool applySun = true)
        {
            // Check its not the current group (we don't lerp between same group)
            if (sceneSettingsMaster == this) return;

            if (Level.current == null)
            {
                Debug.LogWarningFormat(this, "Cannot blend scene settings, no current level found");
                return;
            }

            LightingPreset currentLightingPreset = Level.current.currentLightingPreset;
            if (currentLightingPreset == null)
            {
                Debug.LogWarningFormat(this, "Cannot blend scene settings, level currentLightingPreset is not set");
                return;
            }

            if (lightingPreset == null)
            {
                Debug.LogWarningFormat(this, "Cannot blend scene settings, no lightingPreset is referenced");
                return;
            }
#if UNITY_EDITOR
            if (showDebugLine) Debug.LogFormat(this, "Blend current Scene Settings with " + lightingPreset.name + " with t = " + t);
#endif
            // We only blend fog for now
            if (lightingPreset.fog != LightingPreset.State.NoChange)
            {
                if (currentLightingPreset.fog == LightingPreset.State.Disabled
                    && lightingPreset.fog == LightingPreset.State.Disabled)
                {
                    RenderSettings.fog = false;
                }
                else
                {
                    Color fogColor = currentLightingPreset.fogColor;
                    float fogStartDistance = currentLightingPreset.fogStartDistance;
                    float fogEndDistance = currentLightingPreset.fogEndDistance;

                    if (currentLightingPreset.fog == LightingPreset.State.Disabled)
                    {
                        fogColor = Color.white;
                        fogStartDistance = Camera.main.farClipPlane;
                        fogEndDistance = Camera.main.farClipPlane;
                    }

                    if (lightingPreset.fog == LightingPreset.State.Enabled)
                    {
                        fogColor = Color.Lerp(fogColor, lightingPreset.fogColor, t);
                        fogStartDistance = FogDistanceSmoothStep(fogStartDistance, lightingPreset.fogStartDistance, t);
                        fogEndDistance = FogDistanceSmoothStep(fogEndDistance, lightingPreset.fogEndDistance, t);
                    }
                    else
                    {
                        Color tempColor = Color.white;
                        fogColor = Color.Lerp(fogColor, tempColor, t);
                        fogStartDistance = FogDistanceSmoothStep(fogStartDistance, Camera.main.farClipPlane, t);
                        fogEndDistance = FogDistanceSmoothStep(fogEndDistance, Camera.main.farClipPlane, t);
                    }

                    if (fogStartDistance > fogEndDistance)
                    {
                        fogStartDistance = fogEndDistance - 0.01f;
                    }

                    RenderSettings.fog = true;
                    RenderSettings.fogColor = fogColor;
                    RenderSettings.fogStartDistance = fogStartDistance;
                    RenderSettings.fogEndDistance = fogEndDistance;
                }


                if (_updateRenderSettingEvent != null) _updateRenderSettingEvent.Invoke();
            }
        }

        private float FogDistanceSmoothStep(float from, float to, float t)
        {
            float smoothRatioStopValue = 500.0f;
            float min = from;
            float tSmooth = t;
            if (to < min)
            {
                min = to;
                tSmooth = 1.0f - t;
            }

            float smoothRatio = 1.0f;
            if (min < smoothRatioStopValue)
            {
                smoothRatio = Mathf.Lerp(min, smoothRatioStopValue, tSmooth) / smoothRatioStopValue;
                smoothRatio *= smoothRatio;
            }

            float factor = (to - from);
            float value = from + (t * smoothRatio * factor);
            return value;
        }

        /// <summary>
        /// Clear all lightingPreset settings except scene settings
        /// </summary>
        [Button]
        public void ClearAll()
        {
            if (!initialized) return;
            ClearAll(lightingPreset);
            initialized = false;
        }

        public void ClearLightingPreset()
        {
            if (!initialized) return;
            if (lightingPreset == null) return;
            ClearLightmaps(lightingPreset);
            ClearLightProbeVolumes(lightingPreset);
            lightingPreset = null;
            currentLightingPreset = null;
        }

        public void ClearAll(LightingPreset lightingPreset)
        {
            ClearLightmaps(lightingPreset);
            ClearLightProbeVolumes(lightingPreset);

            ClearSceneSettings();
        }

        public void ClearLightmaps(LightingPreset lightingPreset)
        {
            // Clear mesh lightmaps
            var lightingPresetName = lightingPreset != null ? lightingPreset.name : "Unknown - Lightning Preset already Destroyed";
            Debug.LogFormat(this, "Clear lightmaps from {0}", lightingPresetName);
            lightmapIndexHelper.InitFromLightmap();
            foreach (MeshRendererReference meshRendererReference in meshRendererReferences)
            {
                if (meshRendererReference.meshRenderer == null || meshRendererReference.meshRenderer.lightmapIndex < 0) continue;
                lightmapIndexHelper.RemoveLightmap(meshRendererReference.meshRenderer);
            }
            lightmapIndexHelper.SetLightmap();
        }

        public void ClearLightProbeVolumes(LightingPreset lightingPreset)
        {
            int count = lightProbeVolumeReferences.Count;
            for (int i = 0; i < count; i++)
            {
                LightProbeVolumeReference lightProbeVolumeReference = lightProbeVolumeReferences[i];
                LightProbeVolume lightProbeVolume = lightProbeVolumeReference.lightProbeVolume;

                if (lightProbeVolume != null)
                {
                    lightProbeVolume.SetTexture(null, null, null, null);
                }
            }
        }

        public void ClearSceneSettings()
        {
            if (sceneSettingsMaster != this)
            {
                return;
            }

            Debug.LogFormat(this, "Clear scene settings");

            ResetSceneSettings();

            sceneSettingsMaster = null;

            if (allActive.Count > 0)
            {
                allActive[0].ApplySceneSettings();
            }
        }

        protected void ResetSceneSettings()
        {
            if (Application.isPlaying)
            {
                if (Level.current && Level.current.defaultLightingPreset)
                {
                    ApplySceneSettings(Level.current.defaultLightingPreset, reset: false);
                }
            }
            else
            {
                Level level = GameObject.FindObjectOfType<Level>();
                if (level && level.defaultLightingPreset)
                {
                    ApplySceneSettings(level.defaultLightingPreset, reset: false);
                }
            }
        }

#if UNITY_EDITOR

        private void OnBakeStarted()
        {
            if (allActive.Count > 1)
            {
                Debug.LogError("Baking multiple lightingGroup at the same time is not supported");
                LightmapBakeHelper.CancelBake();
            }
        }

        [ContextMenu("BakeProbeVolumes")]
        public void BakeProbeVolumes()
        {
            LightProbeVolumeGenerator[] lightProbeVolumeGenerators = this.gameObject.GetComponentsInChildren<LightProbeVolumeGenerator>();
            for (int i = 0; i < lightProbeVolumeGenerators.Length; i++)
            {
                EditorUtility.DisplayProgressBar("Generating Baked Lighting", "Processing lightProbeVolumes", (float)i / (float)lightProbeVolumeGenerators.Length);
                lightProbeVolumeGenerators[i].Generate3dTextures();
                EditorUtility.ClearProgressBar();
            }
        }

        [ContextMenu("UpdateReferencesAndSaveAll")]
        public void UpdateReferencesAndSaveAll()
        {
            UpdateReferencesAndSaveAll(true);
        }

        public void UpdateReferencesAndSaveAll(bool saveBakedReflectionProbe = true)
        {
            if (LightmapSettings.lightmaps == null || LightmapSettings.lightmaps.Length == 0)
            {
                Debug.LogErrorFormat(this, "Cannot save lighting data as no lightmaps are loaded on this scene");
                return;
            }

            if (lightingPreset == null) lightingPreset = LightingPreset.Create(this);

            UpdateReferences();

            lightingPreset.SaveLightingGroup(this, GetPresetFolder(), false, saveBakedReflectionProbe);

            GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(this.gameObject);
            if (sourcePrefab) PrefabUtility.ApplyPrefabInstance(this.gameObject, InteractionMode.AutomatedAction);

            Debug.LogFormat(this, "Saved Lighting Data To Preset");
        }

        [ContextMenu("SaveLightProbeVolumesOnly")]
        public void SaveLightProbeVolumesOnly(bool saveBakedReflectionProbe = true)
        {
            if (lightingPreset == null) lightingPreset = LightingPreset.Create(this);
            lightingPreset.SaveLightingGroup(this, GetPresetFolder(), true, saveBakedReflectionProbe);
        }

        protected string GetPresetFolder()
        {
            string lightingPresetPath = AssetDatabase.GetAssetPath(lightingPreset);
            string lightingPresetFolderPath = Path.GetDirectoryName(lightingPresetPath);
            string lightingPresetFileName = Path.GetFileNameWithoutExtension(lightingPresetPath);
            string lightingPresetAssetsFolderPath = Path.Combine(lightingPresetFolderPath, lightingPresetFileName);
            if (!AssetDatabase.IsValidFolder(lightingPresetAssetsFolderPath))
            {
                AssetDatabase.CreateFolder(lightingPresetFolderPath, lightingPresetFileName);
            }
            return lightingPresetAssetsFolderPath;
        }

        public void UpdateReferences()
        {
            // Get all baked meshRenderers
            List<MeshRenderer> bakedMeshRenderers = new List<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in this.gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                if (!meshRenderer.enabled) continue;
                if (meshRenderer.receiveGI != ReceiveGI.Lightmaps) continue;
                if (!GameObjectUtility.AreStaticEditorFlagsSet(meshRenderer.gameObject, StaticEditorFlags.ContributeGI)) continue;
                bakedMeshRenderers.Add(meshRenderer);
            }
            // Clear deleted meshRenderers
            for (int i = meshRendererReferences.Count - 1; i >= 0; i--)
            {
                if (meshRendererReferences[i].meshRenderer == null || !bakedMeshRenderers.Contains(meshRendererReferences[i].meshRenderer))
                {
                    meshRendererReferences.RemoveAt(i);
                }
            }
            // Add new meshRenderers
            foreach (MeshRenderer meshRenderer in bakedMeshRenderers)
            {
                if (!HasMeshRendererReference(meshRenderer))
                {
                    meshRendererReferences.Add(new MeshRendererReference(meshRenderer));
                }
            }

            // Get all baked lights
            List<Light> bakedLights = new List<Light>();
            foreach (Light light in this.gameObject.GetComponentsInChildren<Light>())
            {
                if (light.enabled)
                {
                    bakedLights.Add(light);
                }
            }
            // Clear deleted lights
            for (int i = lightReferences.Count - 1; i >= 0; i--)
            {
                if (lightReferences[i].light == null || !bakedLights.Contains(lightReferences[i].light))
                {
                    lightReferences.RemoveAt(i);
                }
            }
            // Add new lights
            foreach (Light light in bakedLights)
            {
                if (!HasLightReference(light))
                {
                    lightReferences.Add(new LightReference(light));
                }
            }

            // Get all reflection probes
            List<ReflectionProbe> reflectionProbes = new List<ReflectionProbe>();
            foreach (ReflectionProbe reflectionProbe in this.gameObject.GetComponentsInChildren<ReflectionProbe>())
            {
                if (reflectionProbe.enabled)
                {
                    reflectionProbes.Add(reflectionProbe);
                }
            }
            // Clear deleted reflection probes
            for (int i = reflectionProbeReferences.Count - 1; i >= 0; i--)
            {
                if (reflectionProbeReferences[i].reflectionProbe == null || !reflectionProbes.Contains(reflectionProbeReferences[i].reflectionProbe))
                {
                    reflectionProbeReferences.RemoveAt(i);
                }
            }
            // Add new reflection probes
            foreach (ReflectionProbe reflectionProbe in reflectionProbes)
            {
                if (!HasReflectionProbeReference(reflectionProbe))
                {
                    reflectionProbeReferences.Add(new ReflectionProbeReference(reflectionProbe));
                }
            }

            // Get all baked lightProbeVolumes
            List<LightProbeVolume> bakedLightProbeVolumes = new List<LightProbeVolume>();
            foreach (LightProbeVolume lightProbeVolume in this.gameObject.GetComponentsInChildren<LightProbeVolume>())
            {
                if (lightProbeVolume.enabled)
                {
                    bakedLightProbeVolumes.Add(lightProbeVolume);
                }
            }
            // Clear deleted lightProbeVolumes
            for (int i = lightProbeVolumeReferences.Count - 1; i >= 0; i--)
            {
                if (lightProbeVolumeReferences[i].lightProbeVolume == null || !bakedLightProbeVolumes.Contains(lightProbeVolumeReferences[i].lightProbeVolume))
                {
                    lightProbeVolumeReferences.RemoveAt(i);
                }
            }
            // Add new lightProbeVolumes
            foreach (LightProbeVolume lightProbeVolume in bakedLightProbeVolumes)
            {
                if (!HasLightProbeVolumeReference(lightProbeVolume))
                {
                    lightProbeVolumeReferences.Add(new LightProbeVolumeReference(lightProbeVolume));
                }
            }

            EditorUtility.SetDirty(this);
        }
#endif

        public bool HasMeshRendererReference(MeshRenderer meshRenderer)
        {
            foreach (MeshRendererReference meshRendererReference in meshRendererReferences)
            {
                if (meshRendererReference.meshRenderer == meshRenderer)
                {
                    return true;
                }
            }
            return false;
        }

        [Button]
        public bool TryGetMeshRendererGuid(MeshRenderer meshRenderer, out string meshRendererGuid)
        {
            foreach (MeshRendererReference meshRendererReference in meshRendererReferences)
            {
                if (meshRendererReference.meshRenderer == meshRenderer)
                {
                    meshRendererGuid = meshRendererReference.guid;
                    return true;
                }
            }
            meshRendererGuid = null;
            return false;
        }

        [Button]
        public bool TryGetMeshRenderer(string meshRendererGuid, out MeshRenderer meshRenderer)
        {
            if (_meshRendererByGuid == null)
            {
                int count = meshRendererReferences.Count;
                _meshRendererByGuid = new Dictionary<string, MeshRenderer>(count);
                for (var i = 0; i < count; i++)
                {
                    MeshRendererReference meshRendererReference = meshRendererReferences[i];
                    if (meshRendererReference.meshRenderer == null)
                    {
                        //Debug.LogError("mesh is null : " + meshRendererReference.guid + " in lighting group :" + gameObject.name);
                        continue;
                    }

                    _meshRendererByGuid.Add(meshRendererReference.guid, meshRendererReference.meshRenderer);
                }
            }

            return _meshRendererByGuid.TryGetValue(meshRendererGuid, out meshRenderer);
        }

        public bool HasLightReference(Light light)
        {
            foreach (LightReference lightReference in lightReferences)
            {
                if (lightReference.light == light)
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryGetLight(string lightGuid, out Light light)
        {
            foreach (LightReference lightReference in lightReferences)
            {
                if (lightReference.guid == lightGuid)
                {
                    light = lightReference.light;
                    return true;
                }
            }
            light = null;
            return false;
        }

        public bool HasLightProbeVolumeReference(LightProbeVolume lightProbeVolume)
        {
            foreach (LightProbeVolumeReference lightProbeVolumeReference in lightProbeVolumeReferences)
            {
                if (lightProbeVolumeReference.lightProbeVolume == lightProbeVolume)
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryGetLightProbeVolume(string lightGuid, out LightProbeVolume lightProbeVolume)
        {
            foreach (LightProbeVolumeReference lightProbeVolumeReference in lightProbeVolumeReferences)
            {
                if (lightProbeVolumeReference.guid == lightGuid)
                {
                    lightProbeVolume = lightProbeVolumeReference.lightProbeVolume;
                    return true;
                }
            }
            lightProbeVolume = null;
            return false;
        }

        public bool HasReflectionProbeReference(ReflectionProbe reflectionProbe)
        {
            foreach (ReflectionProbeReference reflectionProbeReference in reflectionProbeReferences)
            {
                if (reflectionProbeReference.reflectionProbe == reflectionProbe)
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryGetReflectionProbe(string reflectionProbeGuid, out ReflectionProbe reflectionProbe)
        {
            foreach (ReflectionProbeReference reflectionProbeReference in reflectionProbeReferences)
            {
                if (reflectionProbeReference.guid == reflectionProbeGuid)
                {
                    reflectionProbe = reflectionProbeReference.reflectionProbe;
                    return true;
                }
            }
            reflectionProbe = null;
            return false;
        }

        public bool TryGetLightmapIndex(Texture2D lightmapColor, List<LightmapData> lightmaps, out int lightmapIndex)
        {
            for (int i = 0; i < lightmaps.Count; i++)
            {
                if (lightmaps[i].lightmapColor == lightmapColor)
                {
                    lightmapIndex = i;
                    return true;
                }
            }
            lightmapIndex = -1;
            return false;
        }

#if UNITY_EDITOR
        public List<Issue> GetIssues(bool includeLongCheck, bool experimental)
        {
            List<Issue> issues = new List<Issue>();

            if (lightingPreset == null)
            {
                issues.Add(new Issue(this, "No lighting preset set", true));
            }

            if (lightingPreset.serializedLightmaps.Count == 0)
            {
                issues.Add(new Issue(this, "No lightmaps set", true));
            }

            foreach (var serializedLightmapData in lightingPreset.serializedLightmaps)
            {
                if (serializedLightmapData.color == null)
                {
                    issues.Add(new Issue(this, "Some lightmap texture are missing or null", true));
                }
            }

            // Check meshRenderer with no lightmaps
            List<MeshRenderer> allMeshRenderers = new List<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in this.GetComponentsInChildren<MeshRenderer>())
            {
                if (meshRenderer.enabled && meshRenderer.gameObject.ActiveInPrefabHierarchy() && GameObjectUtility.AreStaticEditorFlagsSet(meshRenderer.gameObject, StaticEditorFlags.ContributeGI) && meshRenderer.receiveGI == ReceiveGI.Lightmaps)
                {
                    allMeshRenderers.Add(meshRenderer);
                }
            }


            int meshCount = lightingPreset.rendererDataListForLightmaps.Count;
            int indexLightmap = -1;
            int indexNextLightmapMesh = 0;
            while (indexNextLightmapMesh == 0 && indexLightmap < lightingPreset.indexLightmapsRendererMeshCount.Count - 1)
            {
                indexLightmap++;
                indexNextLightmapMesh = lightingPreset.indexLightmapsRendererMeshCount[indexLightmap];
            }

            List<MeshRenderer> meshList = new List<MeshRenderer>();
            for (int indexMesh = 0; indexMesh < meshCount; ++indexMesh)
            {
                while (indexNextLightmapMesh == indexMesh && indexLightmap < lightingPreset.indexLightmapsRendererMeshCount.Count - 1)
                {
                    indexLightmap++;
                    indexNextLightmapMesh = lightingPreset.indexLightmapsRendererMeshCount[indexLightmap];
                }

                if (TryGetMeshRenderer(lightingPreset.rendererDataListForLightmaps[indexMesh].meshRendererGuid, out MeshRenderer meshRenderer))
                {
                    if (meshRenderer)
                    {
                        MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();
                        if (meshFilter)
                        {
                            if (meshFilter.sharedMesh)
                            {
                                string assetPath = AssetDatabase.GetAssetPath(meshFilter.sharedMesh);
                                if (!string.IsNullOrEmpty(assetPath))
                                {
                                    AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
                                    if (assetImporter && assetImporter is ModelImporter)
                                    {
                                        if (lightingPreset.rendererDataListForLightmaps[indexMesh].generateSecondaryUV != (assetImporter as ModelImporter).generateSecondaryUV)
                                        {
                                            issues.Add(new Issue(meshRenderer, "Generate Lightmap UVs has been changed in mesh importer settings from last bake, it may break some lightmaps", true));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (indexLightmap >= 0 && indexLightmap < lightingPreset.serializedLightmaps.Count)
                    {
                        if (lightingPreset.serializedLightmaps[indexLightmap].color == null)
                        {
                            issues.Add(new Issue(meshRenderer, "Main lightmap texture is missing", true));
                        }
                    }
                    else
                    {
                        issues.Add(new Issue(lightingPreset, "No lightmap for meshes index : " + indexLightmap, true));
                    }

                    allMeshRenderers.Remove(meshRenderer);
                }

            }

            foreach (MeshRenderer meshRenderer in allMeshRenderers)
            {
                issues.Add(new Issue(meshRenderer, "MeshRenderer is set to static but have no lightmaps", true));
            }

            Area area = this.GetComponent<Area>();
            if (area)
            {
                foreach (var reflectionProbeData in lightingPreset.reflectionProbes)
                {
                    if (reflectionProbeData.texture)
                    {
                        issues.Add(new Issue(area, "Reflection probe texture are saved in preset but it's an area", false));
                    }
                }
            }

            // Check lightprobeVolumes with no texture3D
            List<LightProbeVolume> allLightProbeVolumes = new List<LightProbeVolume>();
            foreach (LightProbeVolume lightProbeVolume in this.GetComponentsInChildren<LightProbeVolume>())
            {
                if (lightProbeVolume.enabled && lightProbeVolume.gameObject.ActiveInPrefabHierarchy())
                {
                    allLightProbeVolumes.Add(lightProbeVolume);
                }
            }

            foreach (var lightProbeVolumeData in lightingPreset.lightProbeVolumes)
            {
                if (TryGetLightProbeVolume(lightProbeVolumeData.lightProbeVolumeGuid, out LightProbeVolume lightProbeVolume))
                {
                    if (lightProbeVolumeData.SHAb == null || lightProbeVolumeData.SHAg == null || lightProbeVolumeData.SHAr == null || lightProbeVolumeData.occ == null)
                    {
                        issues.Add(new Issue(lightProbeVolume, "Some 3DTexture are missing", true));
                        continue;
                    }
                    allLightProbeVolumes.Remove(lightProbeVolume);
                }
            }

            foreach (LightProbeVolume lightProbeVolume in allLightProbeVolumes)
            {
                issues.Add(new Issue(lightProbeVolume, "LightProbeVolume have no 3DTexture saved in LightingPreset", true));
            }

            return issues;
        }
#endif
    }
}
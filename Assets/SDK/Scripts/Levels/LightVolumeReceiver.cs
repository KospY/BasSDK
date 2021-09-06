using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using ThunderRoad.Plugins;

#if PrivateSDK
using RainyReignGames.PrefabBakedLighting;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class LightVolumeReceiver : MonoBehaviour
    {
        public Method method = Method.SRPBatching;

        public enum Method
        {
            GPUInstancing,
            SRPBatching,
        }

        public VolumeDetection volumeDetection = VolumeDetection.DynamicTrigger;

        public enum VolumeDetection
        {
            StaticPerMesh,
            DynamicTrigger,
        }

        public bool initRenderersOnStart = true;
        public bool addMaterialInstances = true;

#if PrivateSDK
        public LightProbeVolume currentLightProbeVolume;
        public List<MaterialInstance> materialInstances = new List<MaterialInstance>();
        public List<LightProbeVolume> lightProbeVolumes = new List<LightProbeVolume>();
        public List<Renderer> renderers = new List<Renderer>();
        protected MaterialPropertyBlock materialPropertyBlock;

        public delegate void OnVolumeChangeDelegate(LightProbeVolume lightProbeVolume);
        public event OnVolumeChangeDelegate OnVolumeChangeEvent;

#if UNITY_EDITOR
        protected List<Renderer> noVolumeRenderers = new List<Renderer>();
#endif

        private void Awake()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
        }


        void Start()
        {
            if (initRenderersOnStart)
            {
                SetRenderers(new List<Renderer>(this.gameObject.GetComponentsInChildren<Renderer>(true)), addMaterialInstances);
            }
            if (volumeDetection == VolumeDetection.StaticPerMesh)
            {
                if (Level.current.dungeon)
                {
                    if (Level.current.dungeon.initialized)
                    {
                        UpdateRenderers();
                    }
                    else
                    {
                        Level.current.dungeon.onDungeonGenerated += OnDungeonGenerated;
                    }
                }
                else
                {
                    UpdateRenderers();
                }
            }
        }

        protected void OnDungeonGenerated(EventTime eventTime)
        {
            UpdateRenderers();
        }

        [Button]
        public void SetRenderers(List<Renderer> renderers, bool addMaterialInstances = true)
        {
            materialInstances.Clear();
            this.renderers.Clear();
            foreach (Renderer renderer in renderers)
            {
                if (method == Method.SRPBatching)
                {
                    if (renderer.gameObject.TryGetComponent<MaterialInstance>(out MaterialInstance materialInstance))
                    {
                        materialInstances.Add(materialInstance);
                    }
                    else if (addMaterialInstances)
                    {
                        if (!renderer.gameObject.GetComponent<Chabuk.ManikinMono.ManikinProperties>())
                        {
                            materialInstance = renderer.gameObject.AddComponent<MaterialInstance>();
                            materialInstances.Add(materialInstance);
                        }
                    }
                }
                else if (method == Method.GPUInstancing)
                {
                    this.renderers.Add(renderer);
                }
            }
        }

        protected void OnRootHolderVolumeChange(LightProbeVolume lightProbeVolume)
        {
            currentLightProbeVolume = lightProbeVolume;
            UpdateRenderers();
        }
        protected void OnDespawn(EventTime eventTime)
        {
            if (eventTime == EventTime.OnStart)
            {
                lightProbeVolumes.Clear();
                currentLightProbeVolume = null;
                RestoreRenderers();
            }
        }

        [Button]
        public void UpdateRenderers()
        {
            if (volumeDetection == VolumeDetection.StaticPerMesh)
            {
#if UNITY_EDITOR
                noVolumeRenderers.Clear();
#endif
                currentLightProbeVolume = null;
                lightProbeVolumes.Clear();

                if (method == Method.SRPBatching)
                {
                    foreach (MaterialInstance materialInstance in materialInstances)
                    {
                        LightProbeVolume lightProbeVolume = GetVolumeFromPosition(materialInstance.CachedRenderer.bounds.center);
                        if (lightProbeVolume)
                        {
                            materialInstance.CachedRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                            foreach (Material material in materialInstance.materials)
                            {
                                lightProbeVolume.UpdateMaterialProperties(material);

                            }
                            if (!lightProbeVolumes.Contains(lightProbeVolume)) lightProbeVolumes.Add(lightProbeVolume);
                        }
                        else
                        {
#if UNITY_EDITOR
                            Debug.LogWarning("Cannot find probe volume for " + materialInstance.CachedRenderer.name);
                            if (!noVolumeRenderers.Contains(materialInstance.CachedRenderer)) noVolumeRenderers.Add(materialInstance.CachedRenderer);
#endif
                        }
                        if (materialInstance.CachedRenderer.bounds.center == Vector3.zero) Debug.LogError("renderer " + materialInstance.CachedRenderer.name + " have bounds.center at 0,0,0");
                    }
                }
                else if (method == Method.GPUInstancing)
                {
                    foreach (Renderer renderer in renderers)
                    {
                        LightProbeVolume lightProbeVolume = ApplyProbeVolume(renderer, materialPropertyBlock);
                        if (lightProbeVolume)
                        {
                            if (!lightProbeVolumes.Contains(lightProbeVolume)) lightProbeVolumes.Add(lightProbeVolume);
                        }
#if UNITY_EDITOR
                        else
                        {
                            Debug.LogWarning("Cannot find probe volume for " + renderer.name);
                            if (!noVolumeRenderers.Contains(renderer)) noVolumeRenderers.Add(renderer);
                        }
#endif
                    }
                }
            }
            else if (volumeDetection == VolumeDetection.DynamicTrigger && currentLightProbeVolume)
            {
                if (method == Method.SRPBatching)
                {
                    foreach (MaterialInstance materialInstance in materialInstances)
                    {
                        materialInstance.CachedRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                        foreach (Material material in materialInstance.materials)
                        {
                            currentLightProbeVolume.UpdateMaterialProperties(material);
                        }
                    }
                }
                else if (method == Method.GPUInstancing)
                {
                    foreach (Renderer renderer in renderers)
                    {
                        ApplyProbeVolume(renderer, materialPropertyBlock);
                    }
                }
            }
        }

        [Button]
        public void RestoreRenderers()
        {
            if (method == Method.SRPBatching)
            {
                foreach (MaterialInstance materialInstance in materialInstances)
                {
                    materialInstance.CachedRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
                    materialInstance.RestoreRenderer();
                }
            }
            else if (method == Method.GPUInstancing)
            {
                foreach (Renderer renderer in renderers)
                {
                    renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
                    renderer.ClearMaterialPropertyBlocks();
                }
            }
        }

        public static LightProbeVolume GetVolumeFromPosition(Vector3 position)
        {
            foreach (LightProbeVolume lightProbeVolume in LightProbeVolume.list)
            {
                BoxCollider boxCollider = lightProbeVolume.GetComponent<BoxCollider>();
                if (boxCollider && boxCollider.bounds.Contains(position))
                {
                    return lightProbeVolume;
                }
            }
            return null;
        }

        public static LightProbeVolume ApplyProbeVolume(Renderer renderer, MaterialPropertyBlock materialPropertyBlock)
        {
            Vector3 position = renderer is ParticleSystemRenderer ? renderer.transform.position : renderer.bounds.center;
            if (position == Vector3.zero) Debug.LogError("renderer " + renderer.name + " have position at 0,0,0");
            LightProbeVolume lightProbeVolume = GetVolumeFromPosition(position);
            if (lightProbeVolume)
            {
                renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.CustomProvided;
                renderer.GetPropertyBlock(materialPropertyBlock);
                lightProbeVolume.UpdateMaterialPropertyBlock(materialPropertyBlock, position);
                /*
                // Dirty fix for probe occlusion not working with particle LIT shaders...
                Vector4 lightData = materialPropertyBlock.GetVector("unity_LightData");
                Vector4 probeOcclusion = materialPropertyBlock.GetVector("unity_ProbesOcclusion");
                materialPropertyBlock.SetVector("unity_LightData", new Vector4(lightData.x, lightData.y, probeOcclusion.x, lightData.w));
                */
                renderer.SetPropertyBlock(materialPropertyBlock);
            }
            return lightProbeVolume;
        }

        public static void DisableProbeVolume(Renderer renderer)
        {
            renderer.SetPropertyBlock(null);
            renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (LightProbeVolume.list.Count == 0) return;
            if (volumeDetection == VolumeDetection.DynamicTrigger)
            {
                if (other.gameObject.layer == Common.lightProbeVolumeLayer && other.TryGetComponent<LightProbeVolume>(out LightProbeVolume lightProbeVolume) && !lightProbeVolumes.Contains(lightProbeVolume))
                {
                    lightProbeVolumes.Add(lightProbeVolume);
                    currentLightProbeVolume = lightProbeVolume;
                    UpdateRenderers();
                    if (OnVolumeChangeEvent != null) OnVolumeChangeEvent.Invoke(currentLightProbeVolume);
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (LightProbeVolume.list.Count == 0) return;
            if (volumeDetection == VolumeDetection.DynamicTrigger)
            {
                if (other.gameObject.layer == Common.lightProbeVolumeLayer && other.TryGetComponent<LightProbeVolume>(out LightProbeVolume lightProbeVolume) && lightProbeVolumes.Contains(lightProbeVolume))
                {
                    lightProbeVolumes.Remove(lightProbeVolume);
                    if (lightProbeVolumes.Count > 0)
                    {
                        currentLightProbeVolume = lightProbeVolumes[0];
                        UpdateRenderers();
                    }
                    else
                    {
                        lightProbeVolumes.Clear();
                        currentLightProbeVolume = null;
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (Renderer renderer in noVolumeRenderers)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(renderer.bounds.center, renderer.bounds.size);
                Gizmos.DrawSphere(renderer.bounds.center, 0.1f);
            }
        }
#endif

#endif
    }
}
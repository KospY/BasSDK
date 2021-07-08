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
        public List<Renderer> meshRenderers = new List<Renderer>();
        protected MaterialPropertyBlock materialPropertyBlock;

        public delegate void OnVolumeChangeDelegate(LightProbeVolume lightProbeVolume);
        public event OnVolumeChangeDelegate OnVolumeChangeEvent;

        private void Awake()
        {
            if (method == Method.GPUInstancing) materialPropertyBlock = new MaterialPropertyBlock();
        }


        void Start()
        {
            if (initRenderersOnStart)
            {
                SetRenderers(new List<Renderer>(this.gameObject.GetComponentsInChildren<Renderer>()), addMaterialInstances);
            }
            if (volumeDetection == VolumeDetection.StaticPerMesh)
            {
                UpdateRenderers();
            }
        }

        [Button]
        public void SetRenderers(List<Renderer> renderers, bool addMaterialInstances = true)
        {
            materialInstances.Clear();
            meshRenderers.Clear();
            foreach (Renderer renderer in renderers)
            {
                if (renderer is SkinnedMeshRenderer || renderer is MeshRenderer)
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
                        meshRenderers.Add(renderer);
                    }
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
                currentLightProbeVolume = null;
                lightProbeVolumes.Clear();

                if (method == Method.SRPBatching)
                {
                    foreach (MaterialInstance materialInstance in materialInstances)
                    {
                        LightProbeVolume lightProbeVolume = GetVolumeFromPosition(materialInstance.CachedRenderer.transform.position);
                        if (lightProbeVolume)
                        {
                            materialInstance.CachedRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                            foreach (Material material in materialInstance.materials)
                            {
                                lightProbeVolume.UpdateMaterialProperties(material);

                            }
                            if (!lightProbeVolumes.Contains(lightProbeVolume)) lightProbeVolumes.Add(lightProbeVolume);
                        }
                    }
                }
                else if (method == Method.GPUInstancing)
                {
                    foreach (Renderer renderer in meshRenderers)
                    {
                        LightProbeVolume lightProbeVolume = GetVolumeFromPosition(renderer.transform.position);
                        if (lightProbeVolume)
                        {
                            renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                            renderer.GetPropertyBlock(materialPropertyBlock);
                            lightProbeVolume.UpdateMaterialPropertyBlock(materialPropertyBlock, renderer.transform.position);
                            renderer.SetPropertyBlock(materialPropertyBlock);
                            if (!lightProbeVolumes.Contains(lightProbeVolume)) lightProbeVolumes.Add(lightProbeVolume);
                        }

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
                    foreach (Renderer renderer in meshRenderers)
                    {
                        renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                        renderer.GetPropertyBlock(materialPropertyBlock);
                        currentLightProbeVolume.UpdateMaterialPropertyBlock(materialPropertyBlock, renderer.transform.position);
                        renderer.SetPropertyBlock(materialPropertyBlock);
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
                foreach (Renderer renderer in meshRenderers)
                {
                    renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
                    renderer.ClearMaterialPropertyBlocks();
                }
            }
        }

        protected LightProbeVolume GetVolumeFromPosition(Vector3 position)
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

        private void OnTriggerEnter(Collider other)
        {
            if (LightProbeVolume.list.Count == 0) return;
            if (volumeDetection == VolumeDetection.DynamicTrigger)
            {
                if (other.gameObject.layer == Common.roomAndVolumeLayer && other.TryGetComponent<LightProbeVolume>(out LightProbeVolume lightProbeVolume) && !lightProbeVolumes.Contains(lightProbeVolume))
                {
                    lightProbeVolumes.Add(lightProbeVolume);
                    currentLightProbeVolume = lightProbeVolume;
                    UpdateRenderers();
                    if (OnVolumeChangeEvent != null) OnVolumeChangeEvent.Invoke(currentLightProbeVolume);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (LightProbeVolume.list.Count == 0) return;
            if (volumeDetection == VolumeDetection.DynamicTrigger)
            {
                if (other.gameObject.layer == Common.roomAndVolumeLayer && other.TryGetComponent<LightProbeVolume>(out LightProbeVolume lightProbeVolume) && lightProbeVolumes.Contains(lightProbeVolume))
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
#endif
    }
}
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
    public class LightProbeVolumeReceiver : MonoBehaviour
    {
        public bool updateMaterialInstancesOnStart = true;
        public bool addMaterialInstances = true;
#if PrivateSDK
        public LightProbeVolume currentLightProbeVolumes;
        public List<MaterialInstance> materialInstances = new List<MaterialInstance>();
        public List<LightProbeVolume> lightProbeVolumes = new List<LightProbeVolume>();

        void Start()
        {
            if (updateMaterialInstancesOnStart) UpdateMaterialInstances(new List<Renderer>(this.gameObject.GetComponentsInChildren<Renderer>()), addMaterialInstances);
            RefreshMaterials();
        }

        private void OnDisable()
        {
            foreach (MaterialInstance materialInstance in materialInstances)
            {
                materialInstance.CachedRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
                materialInstance.RestoreRenderer();
            }
            lightProbeVolumes.Clear();
            currentLightProbeVolumes = null;
        }

        [Button]
        public void UpdateMaterialInstances(List<Renderer> renderers, bool addMaterialInstances = true)
        {
            materialInstances.Clear();
            foreach (Renderer renderer in renderers)
            {
                if (renderer is SkinnedMeshRenderer || renderer is MeshRenderer)
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
            }
        }

        [Button]
        public void RefreshMaterials()
        {
            if (currentLightProbeVolumes)
            {
                foreach (MaterialInstance materialInstance in materialInstances)
                {
                    foreach (Material material in materialInstance.materials)
                    {
                        currentLightProbeVolumes.UpdateMaterialProperties(material);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == Common.roomAndVolumeLayer && other.TryGetComponent<LightProbeVolume>(out LightProbeVolume lightProbeVolume) && !lightProbeVolumes.Contains(lightProbeVolume))
            {
                lightProbeVolumes.Add(lightProbeVolume);
                foreach (MaterialInstance materialInstance in materialInstances)
                {
                    materialInstance.CachedRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                    foreach (Material material in materialInstance.materials)
                    {
                        lightProbeVolume.UpdateMaterialProperties(material);
                    }
                }
                currentLightProbeVolumes = lightProbeVolume;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == Common.roomAndVolumeLayer && other.TryGetComponent<LightProbeVolume>(out LightProbeVolume lightProbeVolume) && lightProbeVolumes.Contains(lightProbeVolume))
            {
                lightProbeVolumes.Remove(lightProbeVolume);
                if (lightProbeVolumes.Count > 0)
                {
                    foreach (MaterialInstance materialInstance in materialInstances)
                    {
                        foreach (Material material in materialInstance.materials)
                        {
                            lightProbeVolumes[0].UpdateMaterialProperties(material);
                        }
                    }
                    currentLightProbeVolumes = lightProbeVolumes[0];
                }
                else
                {
                    foreach (MaterialInstance materialInstance in materialInstances)
                    {
                        materialInstance.CachedRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
                        materialInstance.RestoreRenderer();
                    }
                    lightProbeVolumes.Clear();
                    currentLightProbeVolumes = null;
                }
            }
        }
#endif
    }
}
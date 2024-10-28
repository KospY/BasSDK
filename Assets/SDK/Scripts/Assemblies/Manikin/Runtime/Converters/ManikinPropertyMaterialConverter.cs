using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ThunderRoad.Manikin
{
    [CreateAssetMenu(menuName = "Manikin/Converters/Material Converter")]
    public class ManikinPropertyMaterialConverter : ManikinPropertyConverterBase
    {
        private static readonly int OcclusionBitmaskPropertyID = Shader.PropertyToID("_Bitmask");
        //private static readonly int UseVertexOcclusionPropertyID = Shader.PropertyToID("_UseVertexOcclusion"); //vertex occlusion is always enabled now as of 1.0.5 (nomad release)
        private static readonly int UseProbeVolumePropertyID = Shader.PropertyToID("_UseProbeVolume");
        private static readonly int ProbeVolumeOccPropertyID = Shader.PropertyToID("_ProbeVolumeOcc");
        private static readonly int ProbeVolumeShBPropertyID = Shader.PropertyToID("_ProbeVolumeShB");
        private static readonly int ProbeVolumeShGPropertyID = Shader.PropertyToID("_ProbeVolumeShG");
        private static readonly int ProbeVolumeShRPropertyID = Shader.PropertyToID("_ProbeVolumeShR");
        private static readonly int ProbeWorldToTexture = Shader.PropertyToID("_ProbeWorldToTexture");
        private static readonly int VolumeMin = Shader.PropertyToID("_ProbeVolumeMin");
        private static readonly int ProbeVolumeSizeInvPropertyID = Shader.PropertyToID("_ProbeVolumeSizeInv");

        public override void ApplyProperty(GameObject obj, float[] values, bool useSRPBatcher = false, Renderer renderer = null,
            int materialIndex = 0, object payload = null)
        {
            if(!renderer) return;
            if(!(payload is Material m)) return;

            renderer.TryGetMaterialInstance(out var currentMaterial);
            
            // To prevents overriding dynamic values:
            // We cache them, change material, then reapply. Used for occlusion and probes
            var occlusionBitmask = currentMaterial.GetFloat(OcclusionBitmaskPropertyID);
            //var useVertexOcclusion = currentMaterial.GetFloat(UseVertexOcclusionPropertyID);

            var useProbeVolume = currentMaterial.GetFloat(UseProbeVolumePropertyID);
            bool isProbeVolumeKeyWord = currentMaterial.IsKeywordEnabled("_PROBEVOLUME_ON");
            bool hasProbeWorldTexture = currentMaterial.HasMatrix(ProbeWorldToTexture);
            var probeWorldTexture = hasProbeWorldTexture ? currentMaterial.GetMatrix(ProbeWorldToTexture) : Matrix4x4.identity;
            var volumeMin = currentMaterial.GetVector(VolumeMin);
            var probeVolumeSizeInv = currentMaterial.GetVector(ProbeVolumeSizeInvPropertyID);

            var probeVolumeOcc = currentMaterial.GetTexture(ProbeVolumeOccPropertyID);
            var probeVolumeShB = currentMaterial.GetTexture(ProbeVolumeShBPropertyID);
            var probeVolumeShG = currentMaterial.GetTexture(ProbeVolumeShGPropertyID);
            var probeVolumeShR = currentMaterial.GetTexture(ProbeVolumeShRPropertyID);
            
            renderer.CopyPropertiesFromMaterial(m);

            // reapply previous values for occlusion and probes.
            currentMaterial.SetFloat(OcclusionBitmaskPropertyID, occlusionBitmask);
            //currentMaterial.SetFloat(UseVertexOcclusionPropertyID, useVertexOcclusion);

            currentMaterial.SetFloat(UseProbeVolumePropertyID, useProbeVolume);
            if (isProbeVolumeKeyWord) currentMaterial.EnableKeyword("_PROBEVOLUME_ON");
            if(hasProbeWorldTexture) currentMaterial.SetMatrix(ProbeWorldToTexture, probeWorldTexture);
            currentMaterial.SetVector(VolumeMin, volumeMin);
            currentMaterial.SetVector(ProbeVolumeSizeInvPropertyID, probeVolumeSizeInv);
            currentMaterial.SetTexture(ProbeVolumeOccPropertyID, probeVolumeOcc);
            currentMaterial.SetTexture(ProbeVolumeShBPropertyID, probeVolumeShB);
            currentMaterial.SetTexture(ProbeVolumeShGPropertyID, probeVolumeShG);
            currentMaterial.SetTexture(ProbeVolumeShRPropertyID, probeVolumeShR);
        }
    }
}
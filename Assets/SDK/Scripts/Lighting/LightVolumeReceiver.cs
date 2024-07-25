using UnityEngine;
using System.Collections.Generic;
using System;
using ThunderRoad.Manikin;
using UnityEditor;
using UnityEngine.Rendering;
using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Areas/LightProbeVolumeReceiver.html")]
    public class LightVolumeReceiver : ThunderBehaviour
    {
        [Tooltip("The Method of which the Receiver uses.\n\nGPU Instancing: Uses the GPU instancing to apply the volume.\nSRPBatching: Uses SRPBatching to apply the volume.")]
        public Method method = Method.SRPBatching;

        public enum Method
        {
            GPUInstancing,
            SRPBatching,
        }

        [Tooltip("Depicts how the 3D volume is detected.\n\nStatic Per Mesh: Detects it per mesh to apply the volume. Recommended.\nDynamic Trigger: Detects the volume dynamicaly, either through events or code.")]
        public VolumeDetection volumeDetection = VolumeDetection.DynamicTrigger;

        public enum VolumeDetection
        {
            StaticPerMesh,
            DynamicTrigger,
        }

        [Tooltip("Initialize the light probe volumes on to the renderers on Start")]
        public bool initRenderersOnStart = true;
        [Tooltip("Add a material instance on to the objects. This is recommended to allow 3D volumes to be added to materials without affecting other ones.")]
        public bool addMaterialInstances = true;

    }
}

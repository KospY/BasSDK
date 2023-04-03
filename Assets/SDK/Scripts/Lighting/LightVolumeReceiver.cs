using UnityEngine;
using System.Collections.Generic;
using System;
using ThunderRoad.Manikin;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/LightVolumeReceiver")]
    public class LightVolumeReceiver : ThunderBehaviour
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

    }
}
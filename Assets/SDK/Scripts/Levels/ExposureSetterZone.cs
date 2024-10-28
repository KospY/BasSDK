using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ExposureTransitionZone")]
    [RequireComponent(typeof(BoxCollider))]
    public class ExposureSetterZone : ThunderBehaviour
    {
        /// <summary>
        /// Is a zone currently active?
        /// </summary>
        public static bool isBusy;

        /// <summary>
        /// The current or last active zone the player was in.
        /// </summary>
        public static ExposureSetterZone activeZone;


        public static readonly int TonemappingSettings = Shader.PropertyToID("_TonemappingSettings");
        public enum ExposureAdjustMode
        {
            PostProcessingVolume,
            ShaderToneMapping
        }
        [Header("Portal")]
        public Vector3 size = Vector3.one;
        [Header("Exposure")]
        [Tooltip("The exposure adjust mode to use.")]
        public ExposureAdjustMode exposureAdjustMode = ExposureAdjustMode.PostProcessingVolume;
        [Tooltip("The post processing volume to use for exposure adjustment.")]
        public Volume localPostProcessingVolume;
        [Tooltip("The exposure rate of change when transitioning to a brighter zone")]
        public float speedUp = 3;
        [Tooltip("The exposure rate of change  when transitioning to a darker zone")]
        public float speedDown = 1;
        [Tooltip("The exposure to set when the player enters zone via green portal.")]
        public float exposureOnEnter = 1f;
 
        
    }
}

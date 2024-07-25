using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThunderRoad.AssetSorcery
{
    [Serializable]
    public class AssetSorceryShaderSettings
    {
        [Tooltip("Rename the shader using this string")]
        public string customShaderName;

        public string outputShaderFName;
        //public string outputShaderFilename;

        public AssetSorceryPlatformRuntime.ePlatformAS PlatformAS = AssetSorceryPlatformRuntime.ePlatformAS.Desktop;

        public bool tessellationDesktop; // Toggles the define for SW_TESSELLATION
        public bool tessellationMobile; // Toggles the define for SW_TESSELLATION
        
        //public bool forceOpaqueDesktop; // Toggles the depth queue between OPAQUE and TRANSPARENT
        //public bool forceOpaqueMobile; // Toggles the depth queue between OPAQUE and TRANSPARENT

        
        #if ODIN_INSPECTOR
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "displayName")]
#endif
        public List<PassItem> passItems = new List<PassItem>();
        
#if ODIN_INSPECTOR
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "displayName")]
#endif
        public List<AssetSorceryShaderKeyword> shaderKeywords = new List<AssetSorceryShaderKeyword>();
    }

  

    [Serializable]
    public class PassItem
    {
        [HideInInspector] public string displayName;

        public string passName;

        [HideInInspector] 
#if ODIN_INSPECTOR
        [ReadOnly] 
#endif
        public bool present;

        [HideInInspector] public int indexSubShader;

        public bool enabledDesktop;
        public bool enabledMobile;

        //

        public void DisplayNameCalc()
        {
            var prefix = (present ? "" : " -- ") + (enabledDesktop ? "ON - " : "OFF - ") + (enabledMobile ? "ON - " : "OFF - ");
            displayName = prefix + passName;
        }

        public ShaderData.Subshader GetPassSubshader(Shader shader)
        {
            var shaderData = ShaderUtil.GetShaderData(shader);
            var subShader = shaderData.GetSerializedSubshader(indexSubShader);
            return subShader;
        }
    }
}

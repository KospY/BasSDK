using System;
using UnityEngine;
using UnityEngine.Rendering;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace Shadowood
{
    [ExecuteInEditMode]
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public class Tonemapping : MonoBehaviour
    {
        private static readonly int GlobalSkyBlur = Shader.PropertyToID("GlobslSkyBlur"); // TODO typo!
        public static readonly int TonemappingMasterBlend = Shader.PropertyToID("_TonemappingMasterBlend");
        private static readonly int Tonemapping_Settings = Shader.PropertyToID("_TonemappingSettings");

#if DYNAMICBLITPASS
        public DynamicBlitPass.DynamicBlitPass dynamicBlitPass;
#endif
        
        public TonemappingSettings settings = new TonemappingSettings();

        private static TonemappingSettings settingsGlobal = new TonemappingSettings();

        [Serializable]
        public class TonemappingSettings
        {
            //public bool tonemapInShader;

            public bool tonemapGlobal = true;

            [Range(-20, 20)] public float exposure;
            [Range(1, 1.5f)] public float contrast;
            [Range(0, 2)] public float saturation;
            [Range(0, 1)] public float blend;

            public TonemappingSettings(float exposure = 0, float contrast = 1, float saturation = 1, float blend = 1)
            {
                //this.tonemapInShader = true;
                this.exposure = exposure;
                this.contrast = contrast;
                this.saturation = saturation;
                this.blend = blend;
            }
        }

/*

        public void GlobalEnable()
        {
            //var skyBlurStore = Shader.GetGlobalFloat("GlobslSkyBlur");
            //Shader.SetGlobalFloat("GlobslSkyBlur", skyBlur);

            //var masterBlend = Shader.GetGlobalFloat("_TonemappingMasterBlend");
            Shader.SetGlobalFloat(TonemappingMasterBlend, 1);
            Shader.SetKeyword(new GlobalKeyword("UseTonemapping"),true );
        }
*/
        public void GlobalDisable()
        {
            //Shader.EnableKeyword("_TONEMAPPING_ON");
            var keyword = GlobalKeyword.Create("_TONEMAPPING_ON");
            Shader.SetKeyword(keyword, false);

            var keyword2 = GlobalKeyword.Create("GLOBALTONEMAPPING");
            Shader.SetKeyword(keyword2, false);


            //var skyBlurStore = Shader.GetGlobalFloat("GlobslSkyBlur");
            //Shader.SetGlobalFloat("GlobslSkyBlur", skyBlur);

            //var masterBlend = Shader.GetGlobalFloat("_TonemappingMasterBlend");
            Shader.SetGlobalFloat(TonemappingMasterBlend, 0);
            //Shader.SetKeyword(new GlobalKeyword("UseTonemapping"),false );
        }

#if UNITY_EDITOR
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            UpdateLater();
            Run(settingsGlobal);
        }

        static Tonemapping()
        {
            //SetDefaults(); // SetGlobalVectorImpl is not allowed to be called from a MonoBehaviour constructor (or instance field initializer)
            UnityEditor.EditorApplication.update += UpdateLater;
        }

        private static void UpdateLater()
        {
            UnityEditor.EditorApplication.update -= UpdateLater;
            SetDefaults();
        }
#endif

        private static bool brieflyDisabled = false;
        private static float masterBlendStore = -1;

        public static void TonemappingBrieflyDisable(Action
            actionIn)
        {
            masterBlendStore = Shader.GetGlobalFloat(TonemappingMasterBlend);
            Shader.SetGlobalFloat(TonemappingMasterBlend, 0.0f);
            actionIn.Invoke();
            Shader.SetGlobalFloat(TonemappingMasterBlend, masterBlendStore);
            masterBlendStore = -1;
        }

        public static void TonemappingBrieflyDisable()
        {
            if (brieflyDisabled) return;
            //if (masterBlendStore == 0) return;
            masterBlendStore = Shader.GetGlobalFloat(TonemappingMasterBlend);
            Shader.SetGlobalFloat(TonemappingMasterBlend, 0.0f);
            brieflyDisabled = true;
        }

        public static void TonemappingBrieflyReEnable()
        {
            if (!brieflyDisabled) return;
            //if (masterBlendStore == -1) return;
            Shader.SetGlobalFloat(TonemappingMasterBlend, masterBlendStore);
            brieflyDisabled = false;
            //masterBlendStore = -1;
        }
        
        [ContextMenu("SetDefaults")]
        public static void SetDefaults()
        {
            Debug.Log("Tonemapping: set defaults");
            Run(new TonemappingSettings());
            Shader.SetGlobalVector("OceanWaterTint_RGB", new Vector4(0, 1, 0, 1));
        }


        public void OnValidate()
        {
            if (!isActiveAndEnabled) return;
            //if (Application.isPlaying) return;
            //settings.exposure = Mathf.Max(0, settings.exposure);
            settings.contrast = Mathf.Max(0, settings.contrast);
            settings.saturation = Mathf.Max(0, settings.saturation);
            settings.blend = Mathf.Max(0, settings.blend);

            settingsGlobal = settings;
            Run(settingsGlobal);
        }

        private void OnEnable()
        {
            settingsGlobal = settings;
            Run(settingsGlobal);
            //GlobalEnable();

            //UnityEngine.Rendering.RenderPipelineManager.beginCameraRendering -= SRPRenderReflection;
            //UnityEngine.Rendering.RenderPipelineManager.beginCameraRendering += SRPRenderReflection;
        }

        private void OnDisable()
        {
            GlobalDisable();

#if UNITY_EDITOR
            DestroyTempCam();
#endif
            //UnityEngine.Rendering.RenderPipelineManager.beginCameraRendering -= SRPRenderReflection;
        }

        public void SetExposure(float valIn)
        {
            settings.exposure = valIn;
            Run(settingsGlobal);
        }

        public void SetBlend(float valIn)
        {
            settings.blend = valIn;
            Run(settingsGlobal);
        }

        public void SetMasterBlend(float valIn)
        {
            Shader.SetGlobalFloat(TonemappingMasterBlend, valIn);
        }

        //private void SRPRenderReflection(ScriptableRenderContext arg1, Camera arg2)
        //{
        //    Run(settingsGlobal);
        //}

        private void Reset()
        {
            gameObject.name = "Tonemapping";
        }

        public static void SetExposureStatic(float exposure)
        {
            settingsGlobal.exposure = exposure;
            Run(settingsGlobal);
        }
        
        public static void SetSaturationStatic(float saturation)
        {
            settingsGlobal.saturation = saturation;
            Run(settingsGlobal);
        }

        private static void Run(TonemappingSettings settingsIn)
        {
            //Debug.Log("Tonemapping Run A: " + settingsIn);

            //Shader.EnableKeyword("_TONEMAPPING_ON");
            //var keyword = GlobalKeyword.Create("_TONEMAPPING_ON");
            //Shader.SetKeyword(keyword, settingsIn.tonemapGlobal);
            //if(!settingsIn.tonemapGlobal) Shader.DisableKeyword("_TONEMAPPING_ON"); // TODO Remove

            Shader.EnableKeyword("GLOBALTONEMAPPING");
            var keyword2 = GlobalKeyword.Create("GLOBALTONEMAPPING");
            Shader.SetKeyword(keyword2, settingsIn.tonemapGlobal);
            //if(!settingsIn.tonemapGlobal) Shader.DisableKeyword("GLOBALTONEMAPPING"); // TODO Remove

            var postExposureLinear = Mathf.Pow(2f, settingsIn.exposure);

            var settings = new Vector4(postExposureLinear, settingsIn.contrast, settingsIn.saturation, settingsIn.blend);

            //Debug.Log("Tonemapping Run B: " + settings);

            Shader.SetGlobalVector(Tonemapping_Settings, settings);
            Shader.SetGlobalFloat(TonemappingMasterBlend, 1);

            //Shader.SetKeyword(new GlobalKeyword("_TONEMAPPING"),settingsIn.tonemapInShader );

            //Shader.SetGlobalFloat("_TonemappingExposure", settingsIn.exposure);
            //Shader.SetGlobalFloat("_TonemappingContrast", settingsIn.contrast);
            //Shader.SetGlobalFloat("_TonemappingSaturation", settingsIn.saturation);
            //Shader.SetGlobalFloat("_TonemappingBlend", settingsIn.blend);
        }

#if UNITY_EDITOR

        public bool sceneHDRFix = true;
        private Camera tempCam;

        public void DestroyTempCam()
        {
            if (tempCam != null)
            {
                DestroyImmediate(tempCam.gameObject);
            }
        }

        public void Update()
        {
            if (Application.isPlaying) return;

            if (tempCam != null) tempCam.tag = "Untagged";

            if (Camera.main == null || Camera.main == tempCam)
            {
                if (sceneHDRFix)
                {
                    if (tempCam == null)
                    {
                        Debug.Log("Tonemapping: Create Temp Camera");
                        tempCam = new GameObject().AddComponent<Camera>().GetComponent<Camera>();
                        tempCam.name = "TempCamera-Tonemapping";
                        tempCam.gameObject.hideFlags = HideFlags.HideAndDontSave;
                        
                        tempCam.cullingMask = 0;
                        tempCam.useOcclusionCulling = false;
                        tempCam.backgroundColor = Color.black;
                        tempCam.clearFlags = CameraClearFlags.Nothing;
                        tempCam.tag = "MainCamera";
                        //var acd = tempCam.GetComponent<UniversalAdditionalCameraData>();
                        //acd.renderShadows = false;
                    }
                    tempCam.tag = "MainCamera";
                }
                else
                {
                    DestroyTempCam();
                }
            }
            else
            {
                DestroyTempCam();
            }
        }
#endif
    }
}

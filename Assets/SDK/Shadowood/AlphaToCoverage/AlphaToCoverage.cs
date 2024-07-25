using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Shadowood
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class AlphaToCoverage
    {
        private static readonly int ATC = Shader.PropertyToID("AlphaToCoverage");

#if UNITY_EDITOR
        static AlphaToCoverage()
        {
            RenderPipelineManager.beginFrameRendering -= Begin;
            RenderPipelineManager.beginFrameRendering += Begin;
        }

        private static float sceneViewPresent;

        private static void Begin(ScriptableRenderContext arg1, Camera[] cameras)
        {
            int count = 0;

            for (int i = 0; i < cameras.Length; i++)
            {
                count++;
                if (cameras[i].cameraType == CameraType.SceneView)
                {
                    sceneViewPresent = Time.timeSinceLevelLoad;
                    break;
                }
            }

            var atc = QualitySettings.antiAliasing > 1 ? 1 : 0;

            var present = (Time.timeSinceLevelLoad - sceneViewPresent) < 1f / 60f ? 0 : atc;
            //Debug.Log("ARG: " + present +":"+(Time.timeSinceLevelLoad - sceneViewPresent));
            Shader.SetGlobalInt(ATC, present);
        }


        ~AlphaToCoverage()
        {
            RenderPipelineManager.beginFrameRendering -= Begin;
        }

#endif

        /// <summary>
        /// Enables ATC if MSAA is enabled
        /// </summary>
        public static void EnableAlphaToCoverage()
        {
            if (QualitySettings.antiAliasing > 1)
            {
                Debug.Log("EnableAlphaToCoverage: True");
                Shader.SetGlobalInt(ATC, 1);
            }
            else
            {
                Debug.Log("EnableAlphaToCoverage: False MSAA is disabled");
                Shader.SetGlobalInt(ATC, 0);
            }
        }

        public static void DisableAlphaToCoverage()
        {
            Shader.SetGlobalInt(ATC, 0);
        }
    }
}

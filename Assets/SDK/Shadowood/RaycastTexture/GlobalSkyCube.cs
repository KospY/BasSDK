#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;
using UnityEngine.Rendering;

namespace Shadowood
{
    [ExecuteInEditMode]
    public class GlobalSkyCube : MonoBehaviour
    {
        [GUIColor("@skyCube ? Color.white : Color.red")] [InlineButton("Reset", "Find Sky")]
        public Texture skyCube;

        public RaycastTexture.RaycastTexture raycastTexture;

        public ReflectionProbe reflectionProbe;

        [Range(0, 360)] public float rotateCubemaps;

        public void OnValidate()
        {
            if (!isActiveAndEnabled) return;
            rotateCubemaps = rotateCubemaps.Wrap(360); // Wrap around value
            SetCubemapRotation(rotateCubemaps);
            SetGlobals();
        }


        void Reset()
        {
            gameObject.name = "GlobalSkyCube";

            if (raycastTexture != null)
            {
                skyCube = raycastTexture.reflectionCubemap;
            }

            if (reflectionProbe != null)
            {
                switch (reflectionProbe.mode)
                {
                    case ReflectionProbeMode.Baked:
                        skyCube = reflectionProbe.bakedTexture;
                        break;
                    case ReflectionProbeMode.Custom:
                        skyCube = reflectionProbe.customBakedTexture;
                        break;
                }
            }

            //if (RenderSettings.skybox)
            //{
            //    skyCube = RenderSettings.skybox.GetTexture("_Tex");
            //    if (!skyCube) skyCube = RenderSettings.skybox.mainTexture;
            //}
        }

        public void OnEnable()
        {
            SetCubemapRotation(rotateCubemaps);
            SetGlobals();
        }

        public void OnDisable()
        {
            SetDefaults();
        }

        void SetGlobals()
        {
            if (!skyCube) return;
            Shader.SetGlobalTexture("GlobalSkyCube", skyCube);
        }

        public void SetCubemap(Texture reflectionCubemap)
        {
            skyCube = reflectionCubemap;
            SetGlobals();
        }

        public static void SetDefaults()
        {
            Shader.SetGlobalFloat("GlobalSkyRotation", 0);
        }

        /// <summary>
        ///  Values is wrapped to stay within 0-360
        /// Sets rotation of the cubemap for reflection on surface and the cubemap you see faking a grabpass from under surface.
        /// </summary>
        public static void SetCubemapRotation(float valIn)
        {
            valIn = valIn.Wrap(360); // Wrap around value
            Shader.SetGlobalFloat("GlobalSkyRotation", valIn);
        }
    }
}

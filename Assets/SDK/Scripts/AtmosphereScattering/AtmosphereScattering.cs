using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;
using System.Reflection;
using System.Linq;
using ThunderRoad;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

[ExecuteInEditMode]
public class AtmosphereScattering : MonoBehaviour
{
    public static AtmosphereScattering instance;

    public bool active;

    public Color skyColor;
    public Color groundColor;
    public float scatteringIntensity = 1.0f;
    public float skyExposure = 1.0f;
    public float farPoint = 16000.0f;
    public float startDistance = 0.0f;
    public float endDistance = 16000.0f;

    public float sunSize = 1.0f;
    [Range(0.0f, 1.0f)]
    public float sunBrightness = 1.0f;

    [Range(0.0f, 3.0f)]
    public float atmosphereThickness = 1.0f;

    [Range(0.0f, 3.0f)]
    public float maxScatteringValue = 1.0f;

    [Range(-1000.0f, 1000.0f)]
    public float horizonPoint = 0.0f;

    [Range(0.0f, 1000.0f)]
    public float horizonTransitionSmoothness = 100.0f;

    public Material atmosphereMaterial;

    protected RenderObjects rendererFeature;

    private void Awake()
    {
        if (Common.GetPlatform() == Platform.Android)
        {
            this.gameObject.SetActive(false);
            return;
        }

        if (instance == null)
        {
            // Probably master or SDK
            instance = this;
            rendererFeature = GetRendererFeature();
            Refresh();
        }
        else
        {
            // Copy parameter to the master
            instance.active = active;
            instance.skyColor = skyColor;
            instance.groundColor = groundColor;
            instance.scatteringIntensity = scatteringIntensity;
            instance.skyExposure = skyExposure;
            instance.farPoint = farPoint;
            instance.startDistance = startDistance;
            instance.endDistance = endDistance;
            instance.sunSize = sunSize;
            instance.sunBrightness = sunBrightness;
            instance.atmosphereThickness = atmosphereThickness;
            instance.maxScatteringValue = maxScatteringValue;
            instance.horizonPoint = horizonPoint;
            instance.horizonTransitionSmoothness = horizonTransitionSmoothness;
            instance.Refresh();
            this.enabled = false;
            Destroy(this);
        }
    }

    protected RenderObjects GetRendererFeature()
    {
        UniversalRenderPipelineAsset pipeline = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
        FieldInfo fieldInfo = pipeline.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
        ScriptableRendererData scriptableRendererData = ((ScriptableRendererData[])fieldInfo?.GetValue(pipeline))?[0];
        foreach (RenderObjects renderObjects in scriptableRendererData.rendererFeatures.OfType<RenderObjects>())
        {
            if (renderObjects.settings.overrideMaterial == atmosphereMaterial)
            {
                return renderObjects;
            }
        }
        Debug.LogError("Atmosphere scattering renderer feature has not been added to Forward Renderer asset!");
        return null;
    }

    [Button]
    public void Refresh()
    {
        if (!rendererFeature) rendererFeature = GetRendererFeature();
        if (rendererFeature) rendererFeature.SetActive(active);
        atmosphereMaterial.SetFloat("_AtmoScatteringIntensity", scatteringIntensity);
        atmosphereMaterial.SetFloat("_AtmoScatterFarPoint", farPoint);
        atmosphereMaterial.SetFloat("_AtmoStartDist", startDistance);
        atmosphereMaterial.SetFloat("_AtmoEndDist", endDistance);
        atmosphereMaterial.SetFloat("_SunSize", sunSize);
        atmosphereMaterial.SetFloat("_SkyExposure", skyExposure);
        atmosphereMaterial.SetColor("_SkyTint", skyColor);
        atmosphereMaterial.SetColor("_GroundColor", groundColor);
        atmosphereMaterial.SetFloat("_SunBrightness", sunBrightness);
        atmosphereMaterial.SetFloat("_MaxScatteringValue", maxScatteringValue);
        atmosphereMaterial.SetFloat("_AtmosphereThickness", atmosphereThickness);
        atmosphereMaterial.SetFloat("_HorizonPoint", horizonPoint);
        atmosphereMaterial.SetFloat("_HorizonTransitionSmoothness", horizonTransitionSmoothness);
    }
}

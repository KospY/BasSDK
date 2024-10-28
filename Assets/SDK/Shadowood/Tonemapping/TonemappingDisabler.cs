using Shadowood;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class TonemappingDisabler : MonoBehaviour
{
    public void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
        RenderPipelineManager.beginCameraRendering += BeginCameraRendering;

        RenderPipelineManager.endCameraRendering -= EndCameraRendering;
        RenderPipelineManager.endCameraRendering += EndCameraRendering;
    }

    private void EndCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        Tonemapping.TonemappingBrieflyReEnable();
    }

    private void BeginCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        if (arg2.cameraType == CameraType.Reflection)
        {
            Tonemapping.TonemappingBrieflyDisable();
        }
    }

    public void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= EndCameraRendering;
    }
}

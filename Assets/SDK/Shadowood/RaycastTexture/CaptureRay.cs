using System;
using Shadowood.RaycastTexture;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;
using Tonemapping = Shadowood.Tonemapping;

[Serializable]
public class CaptureRay
{
    public const string camName = "tempCamera-CaptureTexture";

    public Camera tempCamera;

    public Texture2D texPixel;
    public RenderTexture rt;

    //public RenderTexture depthTexture;

    //public RenderTexture depthCopy;
    //public Texture2D depthPixel;

    public int res = 1;

    //
    public Color Capture(Vector3 pos, Vector3 dir, float maxDistance = 0)
    {
        if (tempCamera == null) tempCamera = GameObject.Find(camName)?.GetComponent<Camera>();
        if (tempCamera == null)
        {
            tempCamera = new GameObject(camName, typeof(Camera)).GetComponent<Camera>();
            tempCamera.gameObject.hideFlags = HideFlags.DontSave;
        }

        //rt = RenderTexture.GetTemporary(1, 1, 0);
        if (rt == null || rt.width != res) rt = new RenderTexture(res, res, 32, DefaultFormat.HDR);

        RenderTexture.active = rt;

        Tonemapping.TonemappingBrieflyDisable();

        //Shadowood.OceanFogController.SetDefaults();
        //Shadowood.Caustics.SetDefaults();

        //Shadowood.GlobalSkyCube.SetDefaults();

        //Shadowood.AlphaToCoverage.EnableAlphaToCoverage();

        //if (Common.GetQualityLevel() == QualityLevel.Android)
        //{
        //    Shadowood.Tonemapping.SetDefaults();
        //}

        tempCamera.clearFlags = CameraClearFlags.SolidColor;
        tempCamera.backgroundColor = new Color(0, 0, 0, 0);
        tempCamera.targetTexture = RenderTexture.active;
        tempCamera.orthographic = true;
        tempCamera.enabled = true;
        tempCamera.gameObject.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(dir));
        tempCamera.nearClipPlane = 0.00001f;
        tempCamera.farClipPlane = 10000;
        if (maxDistance > 0) tempCamera.farClipPlane = maxDistance;
        tempCamera.orthographicSize = 0.01f;
        tempCamera.useOcclusionCulling = false;

        tempCamera.depthTextureMode = DepthTextureMode.Depth;
        var hd = tempCamera.gameObject.GetOrAddComponent<UniversalAdditionalCameraData>();
        hd.renderPostProcessing = false;

        //RenderPipelineManager.beginCameraRendering -= DrawNow;
        //RenderPipelineManager.beginCameraRendering += DrawNow;
        //
        //RenderPipelineManager.endCameraRendering -= PostRender;
        //RenderPipelineManager.endCameraRendering += PostRender;

        tempCamera.Render();

        //

        //depthTexture = Shader.GetGlobalTexture("_CameraDepthTexture") as RenderTexture;

        //var depthTexture2 = Shader.GetGlobalTexture("_LastCameraDepthTexture ") as RenderTexture;

        //if(depthTexture)Debug.Log("depthTex: " + depthTexture.width+":"+depthTexture.name+" f:"+depthTexture.format+" dsf:"+depthTexture.depthStencilFormat+" gf:"+depthTexture.graphicsFormat);
        //if(depthTexture2)Debug.Log("depthTexLast: " + depthTexture2.width+":"+depthTexture2.name);
        //if (!depthTexture || depthTexture.width != _camDepthTexture.width) depthTexture = new Texture2D(_camDepthTexture.width, _camDepthTexture.height, TextureFormat.RFloat, false);
        //Graphics.CopyTexture(_camDepthTexture, depthTexture);

        //

        if (texPixel == null || texPixel.width != res) texPixel = new Texture2D(res, res, TextureFormat.RGBAFloat, false, true);
        Color result = new Color(0, 0, 0, 0);
#if UNITY_EDITOR //fix build compile errors
        result = rt.ToTexture2D(ref texPixel).GetPixel(0, 0);
#endif
        //if (depthPixel == null || depthPixel.width != res) depthPixel = new Texture2D(res, res, TextureFormat.RFloat, false);

        //if(depthCopy==null || depthCopy.width != depthTexture.width)depthCopy = new RenderTexture(res,res, 32, depthTexture.format);
        //Graphics.Blit(depthTexture,depthCopy);

        //result.a = depthTexture.ToTexture2D(ref depthPixel, TextureFormat.RFloat).GetPixel(0, 0).r;

        //RenderTexture.ReleaseTemporary(rt);

        //RenderPipelineManager.beginCameraRendering -= DrawNow;
        //RenderPipelineManager.endCameraRendering -= PostRender;

        Tonemapping.TonemappingBrieflyReEnable();

        tempCamera.enabled = false;

        return result;
    }

    public void CaptureFinished()
    {
        if (tempCamera != null) Object.DestroyImmediate(tempCamera.gameObject);
        //if (rt != null) rt.Release();
        rt = null;
        texPixel = null;

        //RenderPipelineManager.beginCameraRendering -= DrawNow;
        //RenderPipelineManager.endCameraRendering += PostRender;
    }

    /*
    private void DrawNow(ScriptableRenderContext src, Camera camera)
    {
        if (camera.orthographic == false || camera.name != camName) return;

        //Debug.Log("Hello", camera);
        UniversalRenderPipeline.RenderSingleCamera(src, camera);
        
        var depthTexture2 = Shader.GetGlobalTexture("_CameraDepthTexture") as RenderTexture;
        Debug.Log("depthTex2: " + depthTexture2.width +":"+depthTexture2.name);
    }
    
    private void PostRender(ScriptableRenderContext src, Camera camera)
    {
        if (camera.orthographic == false || camera.name != camName) return;
        
        depthTexture = Shader.GetGlobalTexture("_CameraDepthTexture") as RenderTexture;
        Debug.Log("depthTex3: " + depthTexture.width +":"+depthTexture.name);
    }
    */
}


[Serializable]
public struct RayData
{
    public Color col;
    public Vector3 pos;
    public Vector3 dir;

    public RayData(Vector3 pos, Vector3 dir, Color col)
    {
        this.pos = pos;
        this.dir = dir;
        this.col = col;
    }
}

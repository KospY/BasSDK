#if UNITY_EDITOR
using System;
using Needle.ShaderGraphMarkdown;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class HistogramTest : MonoBehaviour
{
    public HistogramTexture histogramTexture = new HistogramTexture();

    public Texture texture;

    public Texture2D result;

    public ComputeShader computeShader;

    private void OnEnable()
    {
        if(histogramTexture==null) histogramTexture = new HistogramTexture();
        result = histogramTexture.Run(texture,result, ColorWriteMask.All, computeShader);
    }

    private void OnDisable()
    {
        histogramTexture = null;
    }
}
#endif
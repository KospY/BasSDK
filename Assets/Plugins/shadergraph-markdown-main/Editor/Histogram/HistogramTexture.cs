#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Needle.ShaderGraphMarkdown
{
    public class HistogramTexture
    {
        public ComputeShader shader;

        //public Texture2D inputTexture;   
        public uint[] histogramData;

        ComputeBuffer histogramBuffer;
        int handleMain;
        int handleInitialize;


        public int height = 30;


        public RenderTexture textureResult;
        //public Texture2D output;

        ~HistogramTexture()
        {
            if (histogramBuffer != null)
            {
                histogramBuffer.Release();
                histogramBuffer = null;
            }
        }

        public Texture2D Run(Texture inputTexture, Texture2D output = null, ColorWriteMask mask = ColorWriteMask.All, ComputeShader computeShader = null)
        {
            if (computeShader != null) shader = computeShader;

            //if (outputIn) output = outputIn;

            if (computeShader == null)
            {
                var guid = AssetDatabase.FindAssets("Histogram t:ComputeShader", null).First();
                var path = AssetDatabase.GUIDToAssetPath(guid);
                shader = AssetDatabase.LoadAssetAtPath<ComputeShader>(path);
            }

            if (histogramBuffer != null)
            {
                histogramBuffer.Release();
                histogramBuffer = null;
            }


            // TODO add find extension for shader Shader.Find()

            Setup(inputTexture);

            if (shader == null || inputTexture == null || 0 > handleInitialize || 0 > handleMain || histogramBuffer == null || histogramData == null)
            {
                Debug.Log("Cannot compute histogram");
                return null;
            }

            if (textureResult == null)
            {
                //textureResult = new RenderTexture();
            }

            var width = histogramData.Length / 4;

            if (output == null || output.height != height | output.width != width)
            {
                Debug.Log("New texture");
                output = new Texture2D(width, height)
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }

            //shader.SetTexture(0,"TextureOutput", textureResult);
            shader.Dispatch(handleInitialize, 256 / 64, 1, 1);
            // divided by 64 in x because of [numthreads(64,1,1)] in the compute shader code
            shader.Dispatch(handleMain, (inputTexture.width + 7) / 8, (inputTexture.height + 7) / 8, 1);
            // divided by 8 in x and y because of [numthreads(8,8,1)] in the compute shader code

            histogramBuffer.GetData(histogramData);

            Color maxCol = new Color(0,0,0,0);
            float maxVal = 0;
            for (int x = 1; x < width - 1; x++)
            {
                var col2 = new Color(histogramData[(4 * x)], histogramData[(4 * x) + 1], histogramData[(4 * x) + 2], histogramData[(4 * x) + 3]);

                if (col2.r > maxCol.r) maxCol.r = col2.r;
                if (col2.g > maxCol.g) maxCol.g = col2.g;
                if (col2.b > maxCol.b) maxCol.b = col2.b;
                if (col2.a > maxCol.a) maxCol.a = col2.a;


                //if (col2.grayscale > maxVal) maxVal = col2.grayscale;

                uint val = 0;
                
                switch (mask)
                {
                    case ColorWriteMask.Red:
                        val = histogramData[(4 * x) + 0];
                        break;
                    case ColorWriteMask.Green:
                        val = histogramData[(4 * x) + 1];
                        break;
                    case ColorWriteMask.Blue:
                        val = histogramData[(4 * x) + 2];
                        break;
                    case ColorWriteMask.Alpha:
                        val = histogramData[(4 * x) + 3];
                        break;
                    case ColorWriteMask.All:
                        val += histogramData[(4 * x) + 0];
                        val += histogramData[(4 * x) + 1];
                        val += histogramData[(4 * x) + 2];
                        val += histogramData[(4 * x) + 3];
                        break;
                }

                if (val > maxVal) maxVal = val;
                
            }

            for (int x = 0; x < width; x++)
            {
                var col2 = new Color(histogramData[(4 * x)], histogramData[(4 * x) + 1], histogramData[(4 * x) + 2], histogramData[(4 * x) + 3]);
                if (x <= 0) col2 = new Color(0, 0, 0, 0);
                if (x >= width - 1) col2 = new Color(0, 0, 0, 0);
                //col2 /= maxVal;
                col2 = new Color(col2.r / maxCol.r, col2.g / maxCol.g, col2.b / maxCol.b, col2.a / maxCol.a);
                //col2 *= maxCol;
                //col2 = new Color(col2.grayscale,col2.grayscale,col2.grayscale,1);
                //col2 = new Color(0.1f,0,0,0);

                for (int y = 0; y < height; y++)
                {
                    var col = new Color(1,1,1,1);

                    var norm = (float) y / height;

                    bool mirrored = true;

                    if (mirrored)
                    {
                        norm *= 2;
                        norm -= 1;
                        norm = Mathf.Abs(norm);
                    }

                    col = new Color(norm >= col2.r ? 0 : 1, norm >= col2.g ? 0 : 1, norm >= col2.b ? 0 : 1, norm >= col2.a ? 0 : 1);

                    switch (mask)
                    {
                        case ColorWriteMask.Red:
                            col = new Color(col.r, col.r, col.r, col.r);
                            break;
                        case ColorWriteMask.Green:
                            col = new Color(col.g, col.g, col.g, col.g);
                            break;
                        case ColorWriteMask.Blue:
                            col = new Color(col.b, col.b, col.b, col.b );
                            break;
                        case ColorWriteMask.Alpha:
                            col = new Color(col.a, col.a, col.a, col.a);
                            break;
                        case ColorWriteMask.All:
                            float alpha = (norm >= col2.r ? 0 : 1) + (norm >= col2.g ? 0 : 1) + (norm >= col2.b ? 0 : 1) + (norm >= col2.a ? 0 : 1);
                            alpha = Mathf.Clamp01(alpha);
                            col.a = alpha;
                            break;
                    }

                   
                    

                    col.a *= col.grayscale + 0.3f;
                    //col.a = 1;
                    col.a *= 0.5f;
                    
                    //col = Color.red;
                    //col = Color.Lerp(col, Color.black, 0);
                    output.SetPixel(x, y, col);
                }
            }

            output.Apply();
            return output;
        }

        void Setup(Texture inputTexture)
        {
            if (null == shader)
            {
                Debug.Log("Shader or input texture missing.");
                return;
            }

            handleInitialize = shader.FindKernel("HistogramInitialize");
            handleMain = shader.FindKernel("HistogramMain");
            histogramBuffer = new ComputeBuffer(256, sizeof(uint) * 4);
            histogramData = new uint[256 * 4];

            if (handleInitialize < 0 || handleMain < 0 ||
                null == histogramBuffer || null == histogramData)
            {
                Debug.Log("Initialization failed.");
                return;
            }

            shader.SetTexture(handleMain, "InputTexture", inputTexture);
            shader.SetBuffer(handleMain, "HistogramBuffer", histogramBuffer);
            shader.SetBuffer(handleInitialize, "HistogramBuffer", histogramBuffer);
        }
    }
}
#endif
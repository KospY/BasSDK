using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
#pragma warning disable IDE0090 // Use 'new(...)'

namespace ThunderRoad
{
    //    Instead of having a window that selects the folder
    //Have a right click function that has "Convert to TR/Lit" for materials
    //So it replaces the material and it overall makes it easier to manage rather than selecting the entire folder
    public class MOESConvertWindow
    {
        static Vector2 windowScrollPos;

        static List<MoesTexture> materials = new List<MoesTexture>();
        static string materialPath;
        static bool updateExistingMaterials;
        static bool copyMaterialProperties = true;
        static bool fixColorSpace = true;

        static Shader shaderMoesConvert;
 

        static GUIStyle fontAdd = new GUIStyle();
        static GUIStyle fontSkip = new GUIStyle();
        static GUIStyle fontError = new GUIStyle();

        public static void Initialize()
        {
            shaderMoesConvert = Shader.Find("Hidden/MOESConvert");

            fontAdd.normal.textColor = Color.green;
            fontSkip.normal.textColor = Color.yellow;
            fontError.normal.textColor = Color.red;
        }
  
        public static void CreateTexture(MoesTexture moes)
        {
            Material _material = new Material(shaderMoesConvert)
            {
                hideFlags = HideFlags.HideAndDontSave
            };

            int width = 1;
            int height = 1;
            if (moes.metallic != null)
            {
                width = Mathf.Max(width, moes.metallic.width);
                height = Mathf.Max(height, moes.metallic.height);
            }
            else if (moes.occlusion != null)
            {
                width = Mathf.Max(width, moes.occlusion.width);
                height = Mathf.Max(height, moes.occlusion.height);
            }
            else if (moes.emission != null)
            {
                width = Mathf.Max(width, moes.emission.width);
                height = Mathf.Max(height, moes.emission.height);
            }
            else if (moes.smoothness != null)
            {
                width = Mathf.Max(width, moes.smoothness.width);
                height = Mathf.Max(height, moes.smoothness.height);
            }

            _material.SetTexture("_MetallicGlossMap", moes.metallic);
            _material.SetTexture("_OcclusionMap", moes.occlusion);
            _material.SetTexture("_EmissionMap", moes.emission);
            _material.SetTexture("_SpecGlossMap", moes.smoothness);
            _material.SetInt("_SmoothnessIsAlpha", moes.smoothnessIsAlpha ? 1 : 0);
            _material.SetInt("_InvertSmoothness", moes.isAutodesk ? 1 : 0);
            _material.SetInt("_FixColorSpace", fixColorSpace ? 1 : 0);

            var previous = RenderTexture.active;

            RenderTexture tempRT = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
            tempRT.Create();
            Graphics.Blit(null, tempRT, _material);

            Texture2D output = new Texture2D(tempRT.width, tempRT.height, TextureFormat.ARGB32, true, true);
            RenderTexture.active = tempRT;

            output.ReadPixels(new Rect(0, 0, tempRT.width, tempRT.height), 0, 0);
            output.Apply();
            output.filterMode = FilterMode.Bilinear;

            RenderTexture.active = previous;
            tempRT.Release();

            var outputPath = moes.path + "_MOES.png";
            File.WriteAllBytes(outputPath, output.EncodeToPNG());
            Debug.Log("Created " + outputPath);
            AssetDatabase.ImportAsset(outputPath);
            Debug.LogWarning("ImportAsset: MOESConvert: " + outputPath);
        }
    }
    public class MoesTexture
    {
        public string path;
        public Material material;
        public Texture2D metallic;
        public Texture2D occlusion;
        public Texture2D emission;
        public Texture2D smoothness;
        public bool smoothnessIsAlpha;
        public bool isAlphaClip;
        public bool isAutodesk;
        public bool moesExists;
    }
}
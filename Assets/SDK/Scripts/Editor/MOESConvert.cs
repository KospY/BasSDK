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
        static string windowTitle = "Convert materials to MOES";
        static Vector2 windowScrollPos;

        static List<MoesTexture> materials = new List<MoesTexture>();
        static string materialPath;
        static bool updateExistingMaterials;
        static bool copyMaterialProperties = true;
        static bool fixColorSpace = true;

        static Shader shaderMoesConvert;
        static Shader shaderThunderRoadLit;

        static GUIStyle fontAdd = new GUIStyle();
        static GUIStyle fontSkip = new GUIStyle();
        static GUIStyle fontError = new GUIStyle();

        public static void Initialize()
        {
            shaderMoesConvert = Shader.Find("Hidden/MOESConvert");
            shaderThunderRoadLit = Shader.Find("ThunderRoad/Lit");

            fontAdd.normal.textColor = Color.green;
            fontSkip.normal.textColor = Color.yellow;
            fontError.normal.textColor = Color.red;
        }
        public static void ImportMaterialsRightClick()
        {
            materials = new List<MoesTexture>();

            foreach (Object o in Selection.objects)
            {
                if (o.GetType() == typeof(Material))
                {
                    try
                    {
                        var path = AssetDatabase.GetAssetPath(o);
                        if (path.EndsWith("_MOES.mat")) continue;

                        var material = AssetDatabase.LoadAssetAtPath<Material>(path);

                        var item = new MoesTexture()
                        {
                            path = path.Substring(0, path.Length - 4),
                            material = material,
                            metallic = (Texture2D)material.GetTexture("_MetallicGlossMap"),
                            occlusion = (Texture2D)material.GetTexture("_OcclusionMap"),
                            emission = (Texture2D)material.GetTexture("_EmissionMap"),
                            smoothness = (Texture2D)material.GetTexture("_SpecGlossMap"),
                            isAlphaClip = material.GetInt("_AlphaClip") == 1,
                            isAutodesk = material.shader.name.Contains("Autodesk"),
                            moesExists = AssetDatabase.LoadAssetAtPath<Material>(path.Replace(".mat", "_MOES.mat")) != null
                        };
                        if (!item.smoothness)
                        {
                            item.smoothness = item.material.GetInt("_SmoothnessTextureChannel") == 0 ? (Texture2D)material.GetTexture("_MetallicGlossMap") : (Texture2D)material.GetTexture("_BaseMap");
                            item.smoothnessIsAlpha = true;
                        }

                        if (item.metallic || item.occlusion || item.emission || item.smoothness) materials.Add(item);
                    }
                    catch { }
                }
            }
        }


        [MenuItem("Assets/ThunderRoad/Convert MOES")]
        public static void ConvertMOES()
        {
            Initialize();

            ImportMaterialsRightClick();

            foreach (MoesTexture moes in materials)
            {
                if (!moes.moesExists || updateExistingMaterials)
                {
                    CreateTexture(moes);
                    ModifyMaterial(moes);
                }
            }
            AssetDatabase.Refresh();
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
        }
        public static void CreateMaterial(MoesTexture moes)
        {
            var material = new Material(shaderThunderRoadLit);

            // Copying material properties can sometimes prevent BaseMap and BaseColor from being set
            if (copyMaterialProperties) material.CopyPropertiesFromMaterial(moes.material);

            var mainTex = moes.material.GetTexture("_MainTex");
            material.SetColor("_BaseColor", mainTex ? Color.white : moes.isAutodesk ? moes.material.GetColor("_Color") : moes.material.GetColor("_BaseColor"));
            if (moes.isAutodesk) material.SetTexture("_BaseMap", mainTex ?? moes.material.mainTexture ?? moes.material.GetTexture("_BaseMap"));
            else material.SetTexture("_BaseMap", moes.material.GetTexture("_BaseMap") ?? mainTex ?? moes.material.mainTexture);
            material.SetTexture("_MetallicGlossMap", AssetDatabase.LoadAssetAtPath<Texture2D>(moes.path + "_MOES.png"));
            material.SetTexture("_BumpMap", moes.material.GetTexture("_BumpMap"));

            material.SetTexture("_OcclusionMap", null);
            material.SetTexture("_SpecGlossMap", null);

            if (moes.emission) material.EnableKeyword("_EMISSIONMAP_ON");
            if (moes.isAutodesk) material.SetFloat("_Smoothness", 1f);

            material.renderQueue = -1;

            //Debug.Log(material.GetTexture("_BaseMap"));
            //Debug.Log(material.GetColor("_BaseColor"));
            Debug.Log(material.name);
            Debug.Log(material.shader.name);

            var outputPath = moes.path + "_MOES.mat";
            AssetDatabase.CreateAsset(material, outputPath);
            Debug.Log("Created " + outputPath);
            AssetDatabase.ImportAsset(outputPath);
        }
        public static void ModifyMaterial(MoesTexture moes)
        {
            moes.material.shader = shaderThunderRoadLit;

            var mainTex = moes.material.GetTexture("_MainTex");
            moes.material.SetColor("_BaseColor", mainTex ? Color.white : moes.isAutodesk ? moes.material.GetColor("_Color") : moes.material.GetColor("_BaseColor"));
            if (moes.isAutodesk) moes.material.SetTexture("_BaseMap", mainTex ?? moes.material.mainTexture ?? moes.material.GetTexture("_BaseMap"));
            else moes.material.SetTexture("_BaseMap", moes.material.GetTexture("_BaseMap") ?? mainTex ?? moes.material.mainTexture);
            moes.material.SetTexture("_MetallicGlossMap", AssetDatabase.LoadAssetAtPath<Texture2D>(moes.path + "_MOES.png"));
            moes.material.SetTexture("_BumpMap", moes.material.GetTexture("_BumpMap"));

            moes.material.SetTexture("_OcclusionMap", null);
            moes.material.SetTexture("_SpecGlossMap", null);

            if (moes.emission) moes.material.EnableKeyword("_EMISSIONMAP_ON");
            if (moes.isAutodesk) moes.material.SetFloat("_Smoothness", 1f);

            moes.material.renderQueue = -1;

            Debug.Log(moes.material.GetTexture("_BaseMap"));
            Debug.Log(moes.material.GetColor("_BaseColor"));

            AssetDatabase.SaveAssets();
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
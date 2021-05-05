using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
#pragma warning disable IDE0090 // Use 'new(...)'

namespace ThunderRoad {
    public class MOESConvertWindow : EditorWindow {
        string windowTitle = "Convert materials to MOES";
        Vector2 windowScrollPos;

        List<MoesTexture> materials = new List<MoesTexture>();
        string materialPath;
        bool updateExistingMaterials;
        bool copyMaterialProperties = true;
        bool fixColorSpace = true;

        Shader shaderMoesConvert;
        Shader shaderThunderRoadLit;
        Shader shaderThunderRoadLitCutoff;

        GUIStyle fontAdd = new GUIStyle();
        GUIStyle fontSkip = new GUIStyle();
        GUIStyle fontError = new GUIStyle();

        [MenuItem("ThunderRoad (SDK)/Convert materials to MOES")]
        static void Open() {
            MOESConvertWindow window = GetWindow<MOESConvertWindow>();
            window.Initialize();
        }

        public void Initialize() {
            titleContent = new GUIContent(windowTitle);
            shaderMoesConvert = Shader.Find("Hidden/MOESConvert");
            shaderThunderRoadLit = Shader.Find("ThunderRoad/Lit");
            shaderThunderRoadLitCutoff = Shader.Find("ThunderRoad/Lit_Cutoff");

            fontAdd.normal.textColor = Color.green;
            fontSkip.normal.textColor = Color.yellow;
            fontError.normal.textColor = Color.red;
        }

        private void OnGUI() {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent("Materials folder"), new GUIStyle("BoldLabel"), GUILayout.Width(150));
            if (GUILayout.Button(materialPath, new GUIStyle("textField"))) {
                materialPath = EditorUtility.OpenFolderPanel("Select materials folder to convert", materialPath ?? "Assets/", "");
                ImportMaterials();
            }

            if (GUILayout.Button("Refresh")) {
                ImportMaterials();
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            updateExistingMaterials = GUILayout.Toggle(updateExistingMaterials, "Update existing MOES materials");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            copyMaterialProperties = GUILayout.Toggle(copyMaterialProperties, "Copy all material properties (disable if Albedo isn't transferring)");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            fixColorSpace = GUILayout.Toggle(fixColorSpace, "Fix color space conversion (Autodesk only)");
            GUILayout.EndHorizontal();

            if (materials.Count > 0) {

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                GUILayout.Label("Materials", EditorStyles.boldLabel);

                windowScrollPos = EditorGUILayout.BeginScrollView(windowScrollPos, false, false);

                Vector2 previewSize = new Vector2(32, 32);
                foreach (MoesTexture moes in materials) {
                    GUILayout.BeginHorizontal();

                    if (moes.moesExists) {
                        if (updateExistingMaterials) GUILayout.Label("UPDATE: " + moes.path, fontAdd);
                        else GUILayout.Label("EXISTS: " + moes.path, fontSkip);
                    } else {
                        GUILayout.Label("ADD: " + moes.path, fontAdd);
                    }
                    GUILayout.Box(new GUIContent(AssetPreview.GetMiniThumbnail(moes.metallic), "Metallic"), GUILayout.Width(previewSize.x), GUILayout.Height(previewSize.y));
                    GUILayout.Box(new GUIContent(AssetPreview.GetMiniThumbnail(moes.occlusion), "Occlusion"), GUILayout.Width(previewSize.x), GUILayout.Height(previewSize.y));
                    GUILayout.Box(new GUIContent(AssetPreview.GetMiniThumbnail(moes.emission), "Emission"), GUILayout.Width(previewSize.x), GUILayout.Height(previewSize.y));
                    GUILayout.Box(new GUIContent(AssetPreview.GetMiniThumbnail(moes.smoothness), "Smoothness" + (moes.smoothnessIsAlpha ? " (from Alpha Channel)" : "")), GUILayout.Width(previewSize.x), GUILayout.Height(previewSize.y));

                    GUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Generate Textures")) {
                    foreach (MoesTexture moes in materials) {
                        if (!moes.moesExists || updateExistingMaterials) {
                            CreateTexture(moes);
                            CreateMaterial(moes);
                        }
                    }
                    AssetDatabase.Refresh();
                }

            }
        }

        public void ImportMaterials() {
            if (!string.IsNullOrEmpty(materialPath)) {
                materialPath = materialPath.Substring(materialPath.LastIndexOf("Assets"), materialPath.Length - materialPath.LastIndexOf("Assets"));
                materials = new List<MoesTexture>();
                var assets = AssetDatabase.FindAssets("t:Material", new[] { materialPath });
                foreach (var guid in assets) {
                    try {
                        var path = AssetDatabase.GUIDToAssetPath(guid);
                        if (path.EndsWith("_MOES.mat")) continue;

                        var material = AssetDatabase.LoadAssetAtPath<Material>(path);

                        var item = new MoesTexture() {
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
                        if (!item.smoothness) {
                            item.smoothness = item.material.GetInt("_SmoothnessTextureChannel") == 0 ? (Texture2D)material.GetTexture("_MetallicGlossMap") : (Texture2D)material.GetTexture("_BaseMap");
                            item.smoothnessIsAlpha = true;
                        }

                        if (item.metallic || item.occlusion || item.emission || item.smoothness) materials.Add(item);
                    }
                    catch { }
                }
            }
        }

        public void CreateTexture(MoesTexture moes) {
            Material _material = new Material(shaderMoesConvert) {
                hideFlags = HideFlags.HideAndDontSave
            };

            int width = 1;
            int height = 1;
            if (moes.metallic != null) {
                width = Mathf.Max(width, moes.metallic.width);
                height = Mathf.Max(height, moes.metallic.height);
            } else if (moes.occlusion != null) {
                width = Mathf.Max(width, moes.occlusion.width);
                height = Mathf.Max(height, moes.occlusion.height);
            } else if (moes.emission != null) {
                width = Mathf.Max(width, moes.emission.width);
                height = Mathf.Max(height, moes.emission.height);
            } else if (moes.smoothness != null) {
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

        public void CreateMaterial(MoesTexture moes) {
            var material = new Material(moes.isAlphaClip ? shaderThunderRoadLitCutoff : shaderThunderRoadLit);
            
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

            Debug.Log(material.GetTexture("_BaseMap"));
            Debug.Log(material.GetColor("_BaseColor"));

            var outputPath = moes.path + "_MOES.mat";
            AssetDatabase.CreateAsset(material, outputPath);
            Debug.Log("Created " + outputPath);
            AssetDatabase.ImportAsset(outputPath);
        }
    }

    public class MoesTexture {
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
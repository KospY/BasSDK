using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Needle.ShaderGraphMarkdown
{
    public class Gradient2GeneratorDrawer : MarkdownMaterialPropertyDrawer
    {
        private const string DefaultTexturePropertyName = "_RampTexture";
        public string texturePropertyName = DefaultTexturePropertyName;
        private Dictionary<Material, Dictionary<string, GradientUserData>> mappedGradients;
        [NonSerialized] private static Texture2D gradientTextureStore;

        
        
        

        public override void OnDrawerGUI(MaterialEditor materialEditor, MaterialProperty[] properties, DrawerParameters parameters)
        {
            var targetProperty = parameters.Get(0, properties);
            var targetPropertyName = texturePropertyName;
            if (targetProperty != null && targetProperty.type == MaterialProperty.PropType.Texture)
            {
                targetPropertyName = targetProperty.name;
                if (string.IsNullOrEmpty(targetPropertyName)) targetPropertyName = texturePropertyName;
            }

            var targetMat = materialEditor.target as Material;
            if (!targetMat) return;

            if (mappedGradients == null) mappedGradients = new Dictionary<Material, Dictionary<string, GradientUserData>>();

            // already cached
            bool gradientWasFound = mappedGradients.ContainsKey(targetMat) && mappedGradients[targetMat].ContainsKey(targetPropertyName);

            void AddToCache(Gradient gradient)
            {
                if (!mappedGradients.ContainsKey(targetMat)) mappedGradients.Add(targetMat, new Dictionary<string, GradientUserData>());
                mappedGradients[targetMat][targetPropertyName] = new GradientUserData(gradient);
                gradientWasFound = true;
            }

            if (targetPropertyName == DefaultTexturePropertyName) return;


            // check for user data in importer
            if (!gradientWasFound && targetProperty?.textureValue is Texture2D tex)
            {
                if (AssetDatabase.Contains(tex))
                {
                    var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tex));
                    var userData = importer.userData;
                    if (!string.IsNullOrEmpty(userData))
                    {
                        var metaData = new GradientUserData(new Gradient());
                        JsonUtility.FromJsonOverwrite(userData, metaData);
                        if (metaData.isSet && metaData.gradient != null)
                            AddToCache(metaData.gradient);
                    }
                }
            }


            // fallback: generate gradient from texture
            if (!gradientWasFound && targetProperty?.textureValue is Texture2D texture)
            {
                var gradient = GenerateFromTexture(texture);
                if (gradient != null)
                    AddToCache(gradient);
            }

            
            
            EditorGUILayout.BeginHorizontal();
            var displayName = targetProperty != null ? targetProperty.displayName : "Ramp";
            // strip condition
            var lastIndex = displayName.LastIndexOf('[');
            if (lastIndex > 0)
                displayName = displayName.Substring(0, lastIndex);
            
            

            EditorGUI.BeginChangeCheck();
            var existingGradient = gradientWasFound ? mappedGradients[targetMat][targetPropertyName] : new GradientUserData(new Gradient());
            var newGradient = EditorGUILayout.GradientField(new GUIContent(parameters.ShowPropertyNames ? targetPropertyName : displayName, parameters.Tooltip), existingGradient.gradient, true);
            if (EditorGUI.EndChangeCheck())
                AddToCache(newGradient);

            var placeholderContent = new GUIContent(targetProperty?.textureValue ? "ApplyMM" : "CreateMM");

            var buttonRect = GUILayoutUtility.GetRect(placeholderContent, GUI.skin.button);
            // draw texture picker next to button
            // TODO renders incorrectly on 2019.4 and prevents clicking the actual button.
            // var haveFixedOverlayBug = false;
            // if(haveFixedOverlayBug && targetProperty != null && buttonRect.width > 46 + 18)
            // {
            //     var controlRect = buttonRect;
            //     controlRect.height = 16;
            //     controlRect.y += 2;
            //     var newObj = (Texture2D) EditorGUI.ObjectField(controlRect, targetProperty.textureValue, typeof(Texture2D), true);
            //     if(newObj != targetProperty.textureValue)
            //     {
            //         Undo.RecordObjects(new Object[] { this, targetMat }, "Applied Gradient");
            //         targetProperty.textureValue = newObj;
            //         mappedGradients[targetMat].Remove(targetPropertyName);
            //         GUIUtility.ExitGUI(); // reset GUI loop so the gradient can be picked up properly
            //     }
            //     buttonRect.xMax -= 18;
            // }

            float timeLimit = 1;
            var time = Time.realtimeSinceStartup;
            if (targetProperty?.textureValue && existingGradient.willApply && ((time - existingGradient.lastApplied) > 1f / timeLimit))
            {
                //Debug.Log("BLA:" + time + ":" + existingGradient.lastApplied + ":" + 1f / timeLimit + " - " + existingGradient.willApply);
                existingGradient.willApply = false;
                existingGradient.lastApplied = time;
                ApplyRampTexture(targetMat, parameters.Get(0, targetPropertyName));
            }

            //if (!targetProperty?.textureValue)
                if (GUI.Button(buttonRect, targetProperty?.textureValue ? "Apply" : "Create"))
                    WillApplyRampTexture(targetMat, parameters.Get(0, targetPropertyName));

            var curHash = existingGradient.GetHashCode();
            if (targetProperty?.textureValue && existingGradient.lastHash != curHash)
            {
                existingGradient.lastHash = curHash;
                //Debug.Log("Gradient changed: " + curHash);
                WillApplyRampTexture(targetMat, parameters.Get(0, targetPropertyName));
            }


            EditorGUILayout.EndHorizontal();

            // show gradient fallback when no gradient was found - 
            // this shouldn't happen, ever, since we're generating the gradient from the texture
            if (!gradientWasFound && targetProperty != null)
            {
                if (targetProperty.textureValue)
                {
                    var rect = EditorGUILayout.GetControlRect(true, 8);
                    rect.xMin += EditorGUIUtility.labelWidth + 3;
                    rect.height -= 2;
                    var existingTexture = targetProperty.textureValue;
                    GUI.DrawTexture(rect, existingTexture);
                }
                else
                {
                    EditorGUILayout.LabelField(" ", "None Assigned", EditorStyles.miniLabel);
                }
            }
        }

        public override IEnumerable<MaterialProperty> GetReferencedProperties(MaterialEditor materialEditor, MaterialProperty[] properties, DrawerParameters parameters)
        {
            var textureProperty = parameters.Get(0, properties);
            if (textureProperty != null && textureProperty.type == MaterialProperty.PropType.Texture)
                yield return textureProperty;
        }

   

        private void ApplyRampTexture(Material targetMat, string propertyName)
        {
            //Debug.Log("ApplyRampTextures: " + propertyName);

            var gradientUserData = mappedGradients[targetMat][propertyName];

            if (1==1 ||gradientTextureStore == null || gradientTextureStore.width != GradientTexturePixelWidth || gradientTextureStore.height != GradientTexturePixelHeight)
            {
                //Debug.Log("new Texture");
                gradientTextureStore = new Texture2D(GradientTexturePixelWidth, GradientTexturePixelHeight, TextureFormat.RGBAFloat, false, true)
                {
                    name = propertyName + "_Gradient",
                    filterMode = FilterMode.Bilinear,
                    wrapMode = TextureWrapMode.Clamp,
                    alphaIsTransparency = true,
                }; 
                // TODO add pixel/bilinear and hdr control to inspector
            }

            var tex = CreateGradientTexture(targetMat, gradientUserData, propertyName, gradientTextureStore);

            gradientUserData.willSave = true;
            targetMat.SetTexture(propertyName, tex);

            if (gradientUserData.willSave)
            {
                //Debug.Log("Saving Now");
                
                gradientUserData.willSave = false;
                var returnTexture = SaveAndGetTexture(targetMat, gradientTextureStore);
                targetMat.SetTexture(propertyName, returnTexture);
                
                // store gradient meta-info as user data
                var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(returnTexture));
                //importer.userData = JsonUtility.ToJson(new GradientUserData(gradientUserData.gradient) { isSet = true }, false);
                gradientUserData.isSet = true;
                importer.userData = JsonUtility.ToJson(gradientUserData, false);
                if (importer is TextureImporter textureImporter) textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
                EditorUtility.SetDirty(returnTexture);
                importer.SaveAndReimport();
            }
        }

        private void WillApplyRampTexture(Material targetMat, string propertyName)
        {
            var gradientUserData = mappedGradients[targetMat][propertyName];
            gradientUserData.willApply = true;
           //gradientUserData.lastApplied = Time.realtimeSinceStartup;
            //gradientUserData.targetMat = targetMat;
            //gradientUserData.propertyName = propertyName;
        }

        private Gradient GenerateFromTexture(Texture2D texture)
        {
            try
            {
                // sample texture in 8 places and make a gradient from that as fallback.
                var rt = new RenderTexture(texture.width, texture.height, 0, DefaultFormat.HDR);
                var prev = RenderTexture.active;
                RenderTexture.active = rt;
                var tex2 = new Texture2D(texture.width, texture.height, TextureFormat.RGBAFloat, false);
                Graphics.Blit(texture, rt);
                tex2.wrapMode = TextureWrapMode.Clamp;
                tex2.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
                tex2.Apply();
                RenderTexture.active = prev;
                rt.Release();

                var grad = new Gradient();
                var colorKeys = new GradientColorKey[8];
                var alphaKeys = new GradientAlphaKey[8];

                for (int i = 0; i < 8; i++)
                {
                    var x = i / (8.0f - 1);
                    var color = tex2.GetPixelBilinear(x, 0.5f, 0);
                    colorKeys[i] = new GradientColorKey(color, x);
                    alphaKeys[i] = new GradientAlphaKey(color.a, x);
                }

                grad.SetKeys(colorKeys, alphaKeys);
                return grad;
            }
            catch
            {
                return null;
            }
        }

        private const int GradientTexturePixelWidth = 32; // TODO add resolution control to inspector
        private const int GradientTexturePixelHeight = 4; // needs to be multiple of 4 for DXT1 format compression

        [System.Serializable]
        private class GradientUserData
        {
            public Gradient gradient = new Gradient();
            public bool isSet;
            public int lastHash;

             public bool willApply;

            [NonSerialized]public float lastApplied;

            public bool willSave;
            //[NonSerialized] public Material targetMat;
            //[NonSerialized] public string propertyName;

            //public GradientUserData()
            //{
            //}

            public GradientUserData(Gradient gradient)
            {
                this.gradient = gradient;
            }
        }

        static Color[] _colors = new Color[0];

        private static Texture2D CreateGradientTexture(Material targetMaterial, GradientUserData gradientUserData, string propertyName, Texture2D gradientTexture)
        {

            //Debug.Log("CreateGradientTexture As");

            //for (int j = 0; j < GradientTexturePixelHeight; j++)
            //{
            //    for (int i = 0; i < GradientTexturePixelWidth; i++)
            //        gradientTexture.SetPixel(i, j, gradientUserData.gradient.Evaluate((float) i / GradientTexturePixelWidth));
            //}

            
            /*
            
            //Debug.Log("CreateGradientTexture Ass: " + GradientTexturePixelWidth + "x" + GradientTexturePixelHeight +":"+(gradientTexture.width +":"+gradientTexture.height));
            if (_colors.Length != GradientTexturePixelHeight * GradientTexturePixelWidth) _colors = new Color[GradientTexturePixelHeight * GradientTexturePixelWidth];

            int index = 0;
            for (int x = 0; x < GradientTexturePixelWidth; x++)
            {
                for (int y = 0; y < GradientTexturePixelHeight; y++)
                {
                    //Debug.Log(x+"x"+y);
                    _colors[index] = gradientUserData.gradient.Evaluate((float) x / GradientTexturePixelWidth);
                    //gradientTexture.SetPixel(x, y, gradientUserData.gradient.Evaluate((float) x / GradientTexturePixelWidth)); // Bad slow!
                    index++;
                }
            }*/

            if (_colors.Length != GradientTexturePixelWidth) _colors = new Color[GradientTexturePixelWidth];

            for (int x = 0; x < GradientTexturePixelWidth; x++)
            {
                _colors[x] = gradientUserData.gradient.Evaluate((float) x / GradientTexturePixelWidth);
            }

            //int y = 0;
            for (int y = 0; y < GradientTexturePixelHeight; y++)
            {
                gradientTexture.SetPixels(0,y, GradientTexturePixelWidth, 1, _colors);
            }
           
            
   
            gradientTexture.Apply(false);
            gradientTexture.name = propertyName + "_Gradient";
            
            //var returnTexture = SaveAndGetTexture(targetMaterial, gradientTexture);
            //Debug.Log("CreateGradientTexture Bs");
            
            //returnTexture.name = propertyName + "_Gradient";

            
          

            return gradientTexture;
            
        }

        private static Texture2D SaveAndGetTexture(Material targetMaterial, Texture2D sourceTexture)
        {
            string targetFolder = AssetDatabase.GetAssetPath(targetMaterial);
            targetFolder = targetFolder.Replace(targetMaterial.name + ".mat", string.Empty);

            targetFolder += "Gradient Textures/";

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
                AssetDatabase.Refresh();
            }

            bool hdr = true;

            string path = "";
            if (hdr)
            {
                path = targetFolder + targetMaterial.name + sourceTexture.name + ".exr";
                File.WriteAllBytes(path, sourceTexture.EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat));
            }
            else
            {
                path = targetFolder + targetMaterial.name + sourceTexture.name + ".png";
                File.WriteAllBytes(path, sourceTexture.EncodeToPNG());
            }

            //Debug.Log("Writing Gradient Texture: " + path);


           
            //AssetDatabase.asset
            //AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
           // AssetDatabase.ImportAsset(path, ImportAssetOptions.Default);
            sourceTexture = (Texture2D) AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));

            return sourceTexture;
        }
    }
}

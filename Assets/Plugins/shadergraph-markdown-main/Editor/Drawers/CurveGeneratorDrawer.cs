using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Object = UnityEngine.Object;

namespace Needle.ShaderGraphMarkdown
{
    public static class CurveExtension
    {
        public static int CalculateHash(this AnimationCurve[] curve)
        {
            if (curve == null) return 0;

            int hashRes = 8;
            int hash = 0;

            for (int ii = 0; ii < curve.Length; ii++)
            {
                if (curve[ii] == null) continue;
                for (int i = 0; i < hashRes; i++)
                {
                    hash += curve[ii].Evaluate((float) i / (float) hashRes).GetHashCode();
                }
            }


            return hash;
        }

        public static int CalculateHash(this AnimationCurve curve)
        {
            if (curve == null) return 0;
            int hashRes = 8;
            int hash = 1;
            for (int i = 0; i < hashRes; i++)
            {
                hash += curve.Evaluate((float) i / (float) hashRes).GetHashCode();
            }


            return hash;
        }
    }

    public class CurveGeneratorDrawer : MarkdownMaterialPropertyDrawer
    {
        private const string DefaultTexturePropertyName = "_RampTexture";
        public string texturePropertyName = DefaultTexturePropertyName;
        private Dictionary<Material, Dictionary<string, ItemUserData>> mappedItems = new Dictionary<Material, Dictionary<string, ItemUserData>>();
        [NonSerialized] private static Texture2D textureStore;



        private static void TextureField(string name, Texture2D texture)
        {
            GUILayout.BeginVertical();
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.UpperCenter;
            style.fixedWidth = 70;

            // TODO remove this nonsense
            if (name != "") GUILayout.Label(name, style);
            var placeholderContent = new GUIContent("ApplyMM");
            var rect = GUILayoutUtility.GetRect(placeholderContent, GUI.skin.button);

            EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y, rect.width, 18), texture);
            //var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(130), GUILayout.Height(20),GUILayoutOption);
            GUILayout.EndVertical();
            //return result;
        }

        public enum eCurveCount
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4
        }


        public override void OnDrawerGUI(MaterialEditor materialEditor, MaterialProperty[] properties, DrawerParameters parameters)
        {
            var targetProperty = parameters.Get(0, properties);
            var targetPropertyName = texturePropertyName;
            if (targetProperty != null && targetProperty.type == MaterialProperty.PropType.Texture)
            {
                targetPropertyName = targetProperty.name;
                if (string.IsNullOrEmpty(targetPropertyName)) targetPropertyName = texturePropertyName;
            }

            //Debug.Log(targetPropertyName);

            var targetMat = materialEditor.target as Material;
            if (!targetMat) return;

            if (mappedItems == null) mappedItems = new Dictionary<Material, Dictionary<string, ItemUserData>>();

            // already cached
            bool gradientWasFound = mappedItems.ContainsKey(targetMat) && mappedItems[targetMat].ContainsKey(targetPropertyName);

            //Debug.Log("gradientWasFound: " + gradientWasFound);

            void AddToCache(ItemUserData itemIn)
            {
                if (!mappedItems.ContainsKey(targetMat))
                {
                    Debug.Log("Add new key: " + targetMat.name);
                    mappedItems.Add(targetMat, new Dictionary<string, ItemUserData>());
                }

                Debug.Log("Add key:" + mappedItems[targetMat].ContainsKey(targetPropertyName));

                if (!mappedItems[targetMat].ContainsKey(targetPropertyName)) mappedItems[targetMat].Add(targetPropertyName, itemIn);
                mappedItems[targetMat][targetPropertyName] = itemIn;
                gradientWasFound = true;
            }


            // check for user data in importer, meta data exists on the texture itself
            if (!gradientWasFound && targetProperty?.textureValue is Texture2D tex)
            {
                if (AssetDatabase.Contains(tex))
                {
                    var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tex));
                    var userData = importer.userData;
                    if (!string.IsNullOrEmpty(userData))
                    {
                        var metaData = new ItemUserData();
                        JsonUtility.FromJsonOverwrite(userData, metaData);
                        if (metaData.isSet && metaData.curve != null)
                        {
                            AddToCache(metaData);
                            Debug.Log("Loaded:");
                        }
                    }
                }
            }
            
            if (targetPropertyName == DefaultTexturePropertyName) return;


            // fallback: generate gradient from texture
            if (!gradientWasFound && targetProperty?.textureValue is Texture2D texture)
            {
                Debug.LogError("Not implemented");
                return;
                //var curve = GenerateFromTexture(texture);
                //if (curve != null)
                //    AddToCache(curve);
            }


            EditorGUILayout.BeginVertical();
            var displayName = targetProperty != null ? targetProperty.displayName : "Ramp";
            // strip condition
            var lastIndex = displayName.LastIndexOf('[');
            if (lastIndex > 0)
                displayName = displayName.Substring(0, lastIndex);

            EditorGUI.BeginChangeCheck();
            var existingGradient = gradientWasFound
                ? mappedItems[targetMat][targetPropertyName]
                : new ItemUserData()
                {
                    curveCount = 1
                };

            var enumlength = Enum.GetValues(typeof(eCurveCount)).Length;
            if (existingGradient.curve == null || existingGradient.curve.Length != enumlength) existingGradient.curve = new AnimationCurve[enumlength];
            for (int i = 0; i < existingGradient.curveCount; i++)
            {
                if (existingGradient.curve[i] == null) existingGradient.curve[i] = AnimationCurve.Linear(0, 0, 1, 1);
            }
            //mappedItems[targetMat][targetPropertyName] = existingGradient;

            if (existingGradient.curveCount <= 0) existingGradient.curveCount = 1;
            eCurveCount display = (eCurveCount) existingGradient.curveCount;

            
            
           


            var lastrect = GUILayoutUtility.GetRect(new GUIContent(targetProperty?.textureValue ? "ApplyMM" : "CreateMM"), GUI.skin.button);
            if (targetProperty?.textureValue)
            {
                display = (eCurveCount) EditorGUI.EnumPopup(new Rect(lastrect.x, lastrect.y, 120, 15), "", display);


                if (targetProperty?.textureValue && existingGradient.curveCount != (int) display)
                {
                    Debug.Log("Param: " + parameters.Get(0, targetPropertyName) + " : " + targetPropertyName);
                    existingGradient.curveCount = (int) display;
                    WillApplyRampTexture(targetMat, parameters.Get(0, targetPropertyName));
                }

                var newGradients = new AnimationCurve[(int) display];
                for (int i = 0; i < (int) display; i++)
                {
                    if (existingGradient.curve == null || existingGradient.curve.Length != enumlength) existingGradient.curve = new AnimationCurve[enumlength];
                    if (existingGradient.curve[i] == null) existingGradient.curve[i] = AnimationCurve.Linear(0, 0, 1, 1);
                    var nameSuffix = " ";
                    if (i == 0) nameSuffix += "(R)";
                    if (i == 1) nameSuffix += "(G)";
                    if (i == 2) nameSuffix += "(B)";
                    if (i == 3) nameSuffix += "(A)";
                    newGradients[i] =
                        EditorGUILayout.CurveField(parameters.ShowPropertyNames ? targetPropertyName + nameSuffix : displayName + nameSuffix,  existingGradient.curve[i]);
                }
            }


            if(targetProperty?.textureValue&&textureStore)TextureField("", (Texture2D) targetProperty.textureValue);
            
            var placeholderContent2 = new GUIContent("ApplyMM");
            var rect2 = GUILayoutUtility.GetRect(placeholderContent2, GUI.skin.button);
            
            // draw texture picker
            if(targetProperty != null)
            {
                var controlRect = rect2;
                controlRect.height = 16;
                controlRect.y += 2;
                //controlRect.x = buttonRect.x + buttonRect.width + 0;
                //controlRect.width = 10;
                var newObj = (Texture2D) EditorGUI.ObjectField(controlRect, targetProperty.textureValue, typeof(Texture2D), true);
                if(newObj != targetProperty.textureValue)
                {
                    Undo.RecordObjects(new Object[] { this, targetMat }, "Applied Gradient");
                    targetProperty.textureValue = newObj;
                    mappedItems[targetMat].Remove(targetPropertyName);
                    GUIUtility.ExitGUI(); // reset GUI loop so the gradient can be picked up properly
                }
                //buttonRect.xMax -= 18;
            }

            if (EditorGUI.EndChangeCheck())
            {
                AddToCache(existingGradient);
            }


            var placeholderContent = new GUIContent(targetProperty?.textureValue ? "ApplyMM" : "CreateMM");

            var buttonRect = GUILayoutUtility.GetRect(placeholderContent, GUI.skin.button);
            
           

          

            //if (!targetProperty?.textureValue)
            {
                if (GUI.Button(buttonRect, targetProperty?.textureValue ? "Apply" : "Create"))
                {
                    existingGradient.lastHash = 0;
                    AddToCache(existingGradient);
                    Debug.Log("Param: " + parameters.Get(0, targetPropertyName) + " : " + targetPropertyName);

                    WillApplyRampTexture(targetMat, parameters.Get(0, targetPropertyName));
                }
            }

            
           

            var curHash = existingGradient.curve.CalculateHash();

            if (targetProperty?.textureValue && existingGradient.lastHash != curHash)
            {
                existingGradient.lastHash = curHash;
                Debug.Log("Gradient changed: " + curHash);
                WillApplyRampTexture(targetMat, parameters.Get(0, targetPropertyName));
            }


            EditorGUILayout.EndVertical();
            
            float timeLimit = 1;
            var time = Time.realtimeSinceStartup;
            //Debug.Log("targetProperty?.textureValue:"+targetProperty?.textureValue);
            if ( existingGradient.willApply )//&& ((time - existingGradient.lastApplied) > 1f / timeLimit))
            {
                Debug.Log("BLA:" + time + ":" + existingGradient.lastApplied + ":" + 1f / timeLimit + " - " + existingGradient.willApply);
                existingGradient.willApply = false;
                existingGradient.lastApplied = time;
                ApplyRampTexture(targetMat, parameters.Get(0, targetPropertyName));
            }


            // show gradient fallback when no gradient was found - 
            // this shouldn't happen, ever, since we're generating the gradient from the texture
            if (!gradientWasFound && targetProperty != null)
            {
                if (targetProperty?.textureValue)
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
            Debug.Log("ApplyRampTextures: " + propertyName);

            var gradientUserData = mappedItems[targetMat][propertyName];

            if ( textureStore == null || textureStore.width != GradientTexturePixelWidth || textureStore.height != GradientTexturePixelHeight)
            {
                Debug.Log("new Texture");
                textureStore = new Texture2D(GradientTexturePixelWidth, GradientTexturePixelHeight, TextureFormat.RGBAFloat, false, true)
                {
                    name = propertyName + "_Gradient",
                    filterMode = FilterMode.Bilinear,
                    wrapMode = TextureWrapMode.Clamp,
                    alphaIsTransparency = true,
                };
                // TODO add pixel/bilinear and hdr control to inspector
            }

            var tex = CreateGradientTexture(targetMat, gradientUserData, propertyName, textureStore);

            gradientUserData.willSave = true;
            targetMat.SetTexture(propertyName, tex);

            if (gradientUserData.willSave)
            {
                Debug.Log("Saving Now");

                gradientUserData.willSave = false;
                var returnTexture = SaveAndGetTexture(targetMat, textureStore);
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
            Debug.Log("targetMat:" + targetMat.name + ":" + propertyName + ":" + (mappedItems.ContainsKey(targetMat)) + ":" + mappedItems[targetMat].ContainsKey(propertyName));
            if (!mappedItems.ContainsKey(targetMat))
            {
                Debug.LogError("Badness");
                return;
            }

            var gradientUserData = mappedItems[targetMat][propertyName];
            gradientUserData.willApply = true;

            //gradientUserData.lastApplied = Time.realtimeSinceStartup;
            //gradientUserData.targetMat = targetMat;
            //gradientUserData.propertyName = propertyName;
        }
/*
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
        }*/

        private const int GradientTexturePixelWidth = 32; // TODO add resolution control to inspector
        private const int GradientTexturePixelHeight = 4; // needs to be multiple of 4 for DXT1 format compression

        [System.Serializable]
        private class ItemUserData
        {
            public AnimationCurve[] curve = new AnimationCurve[]
            {
                AnimationCurve.Linear(0, 0, 1, 1),
                AnimationCurve.Linear(0, 0, 1, 1),
                AnimationCurve.Linear(0, 0, 1, 1),
                AnimationCurve.Linear(0, 0, 1, 1),
            }; // = AnimationCurve.Linear(0, 0, 1, 1);
            public bool isSet;
            public int lastHash;

            [NonSerialized]public bool willApply;

            [NonSerialized] public float lastApplied;

            [NonSerialized]public bool willSave;

            public int curveCount = 1;
            //[NonSerialized] public Material targetMat;
            //[NonSerialized] public string propertyName;

            public ItemUserData()
            {
            }

            public ItemUserData(AnimationCurve[] curve)
            {
                this.curve = curve;
            }
        }

        static Color[] _colors = new Color[0];

        private static Texture2D CreateGradientTexture(Material targetMaterial, ItemUserData itemUserData, string propertyName, Texture2D gradientTexture)
        {
            Debug.Log("CreateGradientTexture As");

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
                _colors[x] = new Color(0, 0, 0, 0);
                for (int i = 0; i < itemUserData.curveCount; i++)
                {
                    if (i == 0) _colors[x] += new Color(1, 0, 0, 0) * itemUserData.curve[i].Evaluate((float) x / GradientTexturePixelWidth);
                    if (i == 1) _colors[x] += new Color(0, 1, 0, 0) * itemUserData.curve[i].Evaluate((float) x / GradientTexturePixelWidth);
                    if (i == 2) _colors[x] += new Color(0, 0, 1, 0) * itemUserData.curve[i].Evaluate((float) x / GradientTexturePixelWidth);
                    if (i == 3) _colors[x] += new Color(0, 0, 0, 1) * itemUserData.curve[i].Evaluate((float) x / GradientTexturePixelWidth);
                }

                //_colors[x] = Color.white * itemUserData.curve.Evaluate((float) x / GradientTexturePixelWidth);
            }

            //int y = 0;
            for (int y = 0; y < GradientTexturePixelHeight; y++)
            {
                gradientTexture.SetPixels(0, y, GradientTexturePixelWidth, 1, _colors);
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

            targetFolder += "Curve Textures/";

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

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace Needle.ShaderGraphMarkdown
{
    public class PackedDrawer : MarkdownMaterialPropertyDrawer
    {
        private const string DefaultTexturePropertyName = "_PackedDefault";
        private string texturePropertyName = DefaultTexturePropertyName;

        private static Shader blitShaderPackedRemap;
        private static Shader blitShaderRGBA;

        private static Material blitMaterialPackedRemap;
        private static Material blitMaterialRGBA;

        private const int TextureSize = 64;

        private HistogramTexture histogramTexture = new HistogramTexture();

        // TODO catch: Error in Custom Drawer "Packed": Exception of type 'UnityEngine.ExitGUIException' was thrown.

        // TODO storage needs to be destroyed?
        //private Dictionary<string, RenderTexture> texStorage = new Dictionary<string, RenderTexture>();

        private Dictionary<string, Texture2D> texStorage2 = new Dictionary<string, Texture2D>();

        //

        private static void TextureField(string name, Texture texture, float width, float height, bool transparent = false, Material mat = null, ColorWriteMask mask = ColorWriteMask.All, Rect rectIn = new Rect())
        {
            //GUILayout.BeginVertical();

            //var lastRect = GUILayoutUtility.GetLastRect();
            var placeholderContent = new GUIContent("");
            var style = new GUIStyle(GUI.skin.button);
            style.fixedHeight = height;
            style.fixedWidth = width;
            //style.border = new RectOffset(0,0,0,0);
            //style.margin = new RectOffset(0,0,0,0);

            var rect = GUILayoutUtility.GetRect(placeholderContent, style);
            if (rectIn != new Rect()) rect = rectIn;
            style.alignment = TextAnchor.UpperLeft;


            //placeholderContent = new GUIContent("");
            style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.UpperLeft;
            //var recttext = GUILayoutUtility.GetRect(placeholderContent, style);
            var recttext = GUILayoutUtility.GetLastRect();

            if (transparent)
            {
                Color guiColor = GUI.color; // Save the current GUI color
                GUI.color = Color.clear; // This does the magic

                EditorGUI.DrawTextureTransparent(rect, texture);

                GUI.color = guiColor; // Get back to previous GUI color
            }
            else
            {
                EditorGUI.DrawPreviewTexture(rect, texture, mat); // ScaleMode.ScaleToFit, width / height, 0, mask
            }

            //E
            var tc = new GUIContent(name);
            tc.text = name;

            if (name != "") EditorGUI.LabelField(recttext, tc);
            //var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(130), GUILayout.Height(20),GUILayoutOption);
            //GUILayout.EndVertical();
            //return result;
        }


        public enum eMode
        {
            none,
            textureDrop,
            sliders,
        }

        public override bool ReplaceOriginal()
        {
            return false;
        }

        public enum eRemapModes
        {
            ZeroToOne, // Positive 0-1
            MinusOneToOne, // Negative 1 to 1, Special red/blue debug mode for -1 to 1 values
        }

        public override void OnDrawerGUI(MaterialEditor materialEditor, MaterialProperty[] properties, DrawerParameters parameters)
        {
            //return;

            bool changed = false;

            var targetProperty = parameters.Get(0, properties);
            var targetPropertyName = texturePropertyName;
            if (targetProperty != null && targetProperty.type == MaterialProperty.PropType.Texture)
            {
                targetPropertyName = targetProperty.name;
                if (string.IsNullOrEmpty(targetPropertyName)) targetPropertyName = texturePropertyName;
            }

            var targetMat = materialEditor.target as Material;
            if (!targetMat) return;

            if (targetPropertyName == DefaultTexturePropertyName) return;


            var displayName = targetProperty != null ? targetProperty.displayName : "Preview";

            // strip condition
            var lastIndex = displayName.LastIndexOf('[');
            if (lastIndex > 0) displayName = displayName.Substring(0, lastIndex);


            // Get options, eg: !DRAWER Packed _DetailMapPacked B _DetailMapPackedRemap [_DetailMapPacking&&_DETAIL_ON]
            var titleName = parameters.Get(1, "Remapper");
            titleName = titleName.Replace("_", " ");

            var propOptChannel = parameters.Get(2, "");
            if (propOptChannel.Contains("Channel_")) propOptChannel = propOptChannel.Replace("Channel_", "");
            var mode = eMode.textureDrop;
            if (!string.IsNullOrEmpty(propOptChannel)) mode = eMode.sliders;

            eRemapModes remapMap = eRemapModes.ZeroToOne;
            var propMode = parameters.Get(3, "Mode_0");
            if (propMode.Contains("Mode_")) propMode = propMode = propMode.Replace("Mode_", "");
            if (propMode == "Detail" || propMode == "Positive") remapMap = eRemapModes.ZeroToOne;
            if (propMode == "Smoothness" || propMode == "Negative") remapMap = eRemapModes.MinusOneToOne;


            // Name of the Vec4 property to apply set on the material to the value of the sliders
            var vecPropName = parameters.Get(4, targetPropertyName + "Remap");

            //var storageName = targetMat.GetHashCode() + vecPropName;

            //var propertyName = parameters.Get(0, targetPropertyName);


            //if (!texStorage.ContainsKey(storageName))
            //{
            //    texStorage.Add(storageName, null);
            //    changed = true;
            //}
//
            //var textureStore = texStorage[storageName];

            // textureStore = RenderTexture.GetTemporary(TextureSize, TextureSize, 0, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Linear);

            //if (textureStore == null || textureStore.width != TextureSize || textureStore.height != TextureSize)
            //{
            //    Debug.Log("PackedDrawer: New Texture: " + storageName);
            //    textureStore = new RenderTexture(TextureSize, TextureSize, 0, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Linear)
            //    {
            //        name = propertyName + "_Preview",
            //        filterMode = FilterMode.Bilinear,
            //        wrapMode = TextureWrapMode.Clamp,
            //        hideFlags = HideFlags.HideAndDontSave,
            //    };
            //    changed = true;
            //}

            //texStorage[storageName] = textureStore;

            Vector4 remapProperty = Vector4.zero;

            //EditorGUILayout.BeginVertical();

            if (mode == eMode.sliders && targetProperty?.textureValue)
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();

                {
                    EditorGUILayout.BeginVertical();

                    EditorGUI.BeginChangeCheck();
                    if (blitShaderPackedRemap == null) blitShaderPackedRemap = Shader.Find("Hidden/PackedBlit");
                    if (blitShaderPackedRemap == null) Debug.LogError("Shader missing");
                    if (blitMaterialPackedRemap == null) blitMaterialPackedRemap = new Material(blitShaderPackedRemap) {hideFlags = HideFlags.HideAndDontSave, name = "blitMaterial"};


                    Texture2D histogramTex = null;
                    {
                        var kLabelFloatMaxW = (float) ((double) EditorGUIUtility.labelWidth + (double) EditorGUIUtility.fieldWidth + 5.0);
                        //var aaa = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, (float) ((double) kLabelFloatMaxW + 5.0 + 100.0), 18f, 18f, GUI.skin.horizontalSlider, null);


                        var storageName2 = targetProperty.textureValue.GetHashCode().ToString() + targetProperty.textureValue.imageContentsHash + "" + propOptChannel;
                        if (!texStorage2.ContainsKey(storageName2)) texStorage2.Add(storageName2, null);
                        var hist = texStorage2[storageName2];

                        //var storageName3 = targetProperty.textureValue.GetHashCode().ToString() + "b2" + propOptChannel;
                        //if (!texStorage2.ContainsKey(storageName3)) texStorage2.Add(storageName3, null);
                        //var hist2 = texStorage2[storageName3];

                        if (hist == null)
                        {
                            ColorWriteMask colMask = ColorWriteMask.All;
                            if (propOptChannel == "R") colMask = ColorWriteMask.Red;
                            if (propOptChannel == "G") colMask = ColorWriteMask.Green;
                            if (propOptChannel == "B") colMask = ColorWriteMask.Blue;
                            if (propOptChannel == "A") colMask = ColorWriteMask.Alpha;
                            Debug.Log("Run: " + propOptChannel);
                            hist = histogramTexture.Run(targetProperty.textureValue, texStorage2[storageName2], colMask);
                        }

                        //if (hist2 == null)
                        //{
                        //    Debug.Log("Run");
                        //    hist2 = histogramTexture.Run(targetProperty.textureValue, texStorage2[storageName3]);
                        //}

                        if (hist)
                        {
                            /*
                            var lastRect = aaa; //GUILayoutUtility.GetLastRect();
                            lastRect = GUILayoutUtility.GetLastRect();
                            lastRect.x += 10;
                            lastRect.y -= lastRect.height;
                            lastRect.width -= 10;
                            //lastRect.x += GUILayoutUtility.GetRect(new GUIContent("a"),  new GUIStyle(GUI.skin.label) ).width;

                            var newrect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, (float) ((double) kLabelFloatMaxW + 5.0 + 100.0), 18f, 18f, GUI.skin.horizontalSlider, null);
                            //newrect.width *= 0f;
                            newrect.x = 0;
                            newrect.y = 0;
                            //GUI.BeginClip(new Rect(0,0,1100,870));
                            //GUI.depth -= 110;
                            //GUI.FocusControl(""); // force update
                            //lastRect.y -= 10;

                            lastRect.height *= 1.5f;
                            lastRect.y += -6;
*/
                            //TextureField("", hist, hist.width, hist.height, transparent: true, mask: ColorWriteMask.All, rectIn: lastRect);
                            //if (titleName.Contains("Detail")) TextureField("", hist, hist.width, hist.height, transparent:true, mask: ColorWriteMask.Red,rectIn:lastRect);
                            //if (titleName.Contains("Smoothness")) TextureField("", hist2, lastRect.width, hist.height, transparent:true,mask: ColorWriteMask.Blue,rectIn:lastRect);
                            //GUI.EndClip();
                        }

                        histogramTex = hist;
                        if (hist) texStorage2[storageName2] = hist;
                        //if (hist2) texStorage2[storageName3] = hist2;
                    }

                    //
                    //
                    //EditorGUILayout.PrefixLabel("title");
                    EditorGUILayout.LabelField(titleName, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                    //EditorGUILayout.TextArea("Hello: " + titleName);

                    remapProperty = targetMat.GetVector(vecPropName);
                    //var style2 = new GUIStyle(GUI.skin.button);
                    //style.fixedHeight = 30;
                    //style.fixedWidth = 400;
                    //var placeholderContent3 = new GUIContent("ApplyMM");
                    //var rect3 = GUILayoutUtility.GetRect(placeholderContent3, style2);
                    //remapProperty = EditorGUI.Vector4Field(rect3, "Hello", remapProperty);
                    //GUILayoutUtility.GetRect(0, 30);


                    //GUI.depth += 111;
                    float minVal = remapProperty.x;
                    float maxValue = remapProperty.y;
                    //GUILayoutOption[] options = new GUILayoutOption[]{  GUILayout.Width (295)  };
                    //bool remapInvert = targetMat.GetInteger(vecPropName + "_Invert") == 1;


                    //EditorGUILayout.ToggleLeft("boo", remapInvert);
                    //EditorGUILayout.MinMaxSlider(ref minVal, ref maxValue, 0, 1);
                    GUILayoutUtility.GetRect(0, -20);
                    EditorGUIExt2.MinMaxSlider(ref remapProperty.x, ref remapProperty.y, 0, 1, histogramTex);

                    //EditorGUILayout.LabelField(minVal + " -" + maxValue);
             
                    
                    //minVal = EditorGUILayout.FloatField("x", minVal);
                    //maxValue = EditorGUILayout.FloatField("y", maxValue);
                    //GUILayout.EndHorizontal();
                    //if (EditorGUI.EndChangeCheck())
                    //{
                    //    targetMat.SetVector(vecPropName, new Vector4(minVal,maxValue, remapProperty.y, remapProperty.z));
                    //}


                    //GUILayoutUtility.GetRect(0, -20);
                    //switch (remapMap)
                    //{
                    //    case eRemapModes.ZeroToOne:
                    //        EditorGUILayout.MinMaxSlider(ref minVal, ref maxValue, 0, 1);
                    //        break;
                    //    case eRemapModes.MinusOneToOne:
                    //        EditorGUILayout.MinMaxSlider(ref minVal, ref maxValue, -1, 1);
                    //        break;
                    //    default:
                    //        throw new ArgumentOutOfRangeException();
                    //}


                    //EditorGUILayout.LabelField(minVal + " -" + maxValue);

                    //EditorGUI.MinMaxSlider(EditorGUILayout.s_LastRect = EditorGUILayout.GetSliderRect(false, null), ref minVal, ref maxValue, 0, 1);


                    //remapProperty.x = minVal;
                    //remapProperty.y = maxValue;


                    //float minVal2 = remapProperty.z;
                    //float maxValue2 = remapProperty.w;

                    //GUILayoutUtility.GetRect(0, -TextureSize - 10);

                    //EditorGUILayout.MinMaxSlider(ref minVal2, ref maxValue2, 0, 1);

                    switch (remapMap)
                    {
                        case eRemapModes.ZeroToOne:
                            EditorGUILayout.MinMaxSlider(ref remapProperty.z, ref remapProperty.w, 0, 1);
                            break;
                        case eRemapModes.MinusOneToOne:
                            EditorGUILayout.MinMaxSlider(ref remapProperty.z, ref remapProperty.w, -1, 1);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    //EditorGUILayout.LabelField(minVal2 + " -" + maxValue2);
                    
                    EditorGUI.BeginChangeCheck();
                    //GUILayout.BeginHorizontal();
                    remapProperty = EditorGUILayout.Vector4Field("", remapProperty);
                  
                    
                    //minVal = EditorGUILayout.FloatField("x", minVal);
                    //maxValue = EditorGUILayout.FloatField("y", maxValue);
                    //GUILayout.EndHorizontal();
                   

                    //remapProperty.z = minVal2;
                    //remapProperty.w = maxValue2;

                    if (EditorGUI.EndChangeCheck())
                    {
                        changed = true;
                        targetMat.SetVector(vecPropName, remapProperty);
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space(10);

                {
                    if (targetProperty?.textureValue && blitMaterialPackedRemap)
                    {
                        targetMat.SetVector(vecPropName, remapProperty);

                        blitMaterialPackedRemap.SetTexture("_MainTex", targetProperty.textureValue);
                        blitMaterialPackedRemap.SetVector("_Remap", remapProperty);
                        var channelVec = Vector4.zero;
                        if (propOptChannel == "R") channelVec = new Vector4(1, 0, 0, 0);
                        if (propOptChannel == "G") channelVec = new Vector4(0, 1, 0, 0);
                        if (propOptChannel == "B") channelVec = new Vector4(0, 0, 1, 0);
                        if (propOptChannel == "A") channelVec = new Vector4(0, 0, 0, 1);

                        //blitMaterialPackedRemap.SetInt("_Mode", 0); // Default mode 0-1 range;
                        //if (titleName.Contains("Detail")) blitMaterialPackedRemap.SetInt("_Mode", 0);
                        //if (titleName.Contains("Smoothness")) blitMaterialPackedRemap.SetInt("_Mode", 1); // Special red/blue debug mode for -1 to 1 values

                        blitMaterialPackedRemap.SetInt("_Mode", (int) remapMap);

                        blitMaterialPackedRemap.SetVector("_Channel", channelVec);

                        //if(changed)Graphics.Blit(null, textureStore, blitMaterialPackedRemap);
                    }

                    //TextureField("", textureStore, TextureSize);
                    TextureField("", targetProperty.textureValue, TextureSize, TextureSize, transparent: false, blitMaterialPackedRemap);
                }

                EditorGUILayout.Space(0);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(10);
            }


            if (mode == eMode.textureDrop)
            {
                EditorGUILayout.Space(10);
                //EditorGUILayout.PrefixLabel("title");
                EditorGUILayout.LabelField(displayName, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                // draw texture picker


                var placeholderContent = new GUIContent("");
                var style = new GUIStyle(GUI.skin.button);
                style.fixedHeight = TextureSize;
                style.fixedWidth = TextureSize;
                //style.border = new RectOffset(0,0,0,0);
                //style.margin = new RectOffset(0,0,0,0);
                //style.padding = new RectOffset(0,0,0,0);

                var rect = GUILayoutUtility.GetRect(placeholderContent, style);
                var val = (Texture2D) EditorGUI.ObjectField(rect, targetProperty.textureValue, typeof(Texture2D), true);
                if (targetProperty.textureValue != val)
                {
                    targetProperty.textureValue = val;
                    changed = true;
                }

                if (EditorGUI.EndChangeCheck()) changed = true;

                //

                if (1 == 1)
                {
                    if (blitShaderRGBA == null) blitShaderRGBA = Shader.Find("Hidden/RGBABlit");
                    if (blitShaderRGBA == null)
                    {
                        Debug.LogError("Shader missing: blitShaderRGBA");
                    }
                    else
                    {
                        if (blitMaterialRGBA == null) blitMaterialRGBA = new Material(blitShaderRGBA) {hideFlags = HideFlags.HideAndDontSave, name = "blitMaterial"};

                        for (int i = 0; i < 5; i++)
                        {
                            string col = "";
                            if (i == 0) col = "R";
                            if (i == 1) col = "G";
                            if (i == 2) col = "B";
                            if (i == 3) col = "A";
                            if (i == 4) col = "N";

                            if (i == 3)
                            {
                                if (!GraphicsFormatUtility.HasAlphaChannel(targetProperty.textureValue.graphicsFormat)) continue;
                                var asset = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(targetProperty.textureValue));
                                if (asset != null && asset is TextureImporter)
                                {
                                    TextureImporter footprintTextureImporter = (TextureImporter) asset;
                                    if (footprintTextureImporter.alphaSource != TextureImporterAlphaSource.FromGrayScale && !footprintTextureImporter.DoesSourceTextureHaveAlpha()) continue;
                                }
                            }

                            //if (!texStorage.ContainsKey(storageName + col))
                            //{
                            //    texStorage.Add(storageName + col, null);
                            //    changed = true;
                            //}
//
                            //var textureStoreC = texStorage[storageName + col];
                            int tsize = TextureSize; //Mathf.FloorToInt((TextureSize - 3f) / 2f);
                            tsize *= 1;

                            /*
                            if (textureStoreC == null || textureStoreC.width != tsize || textureStoreC.height != tsize)
                            {
                                Debug.Log("PackedDrawer: New Texture: " + storageName + col);
                                textureStoreC = new RenderTexture(tsize, tsize, 0, RenderTextureFormat.DefaultHDR, RenderTextureReadWrite.Linear)
                                {
                                    name = propertyName + "_Preview" + col,
                                    filterMode = FilterMode.Bilinear,
                                    wrapMode = TextureWrapMode.Clamp,
                                    hideFlags = HideFlags.HideAndDontSave,
                                };
                                changed = true;
                            }*/


                            {
                                if (i == 0) blitMaterialRGBA.SetVector("_Channel", new Vector4(1, 0, 0, 0));
                                if (i == 1) blitMaterialRGBA.SetVector("_Channel", new Vector4(0, 1, 0, 0));
                                if (i == 2) blitMaterialRGBA.SetVector("_Channel", new Vector4(0, 0, 1, 0));
                                if (i == 3) blitMaterialRGBA.SetVector("_Channel", new Vector4(0, 0, 0, 1));


                                blitMaterialRGBA.SetInt("_Mode", 0);
                                if (i == 4) blitMaterialRGBA.SetInt("_Mode", 1);

                                blitMaterialRGBA.SetTexture("_MainTex", targetProperty.textureValue);

                                /*
                                if (changed || textureStoreC == null || textureStoreC.width != tsize || textureStoreC.height != tsize || !textureStoreC.IsCreated())
                                {
                                    Graphics.Blit(null, textureStoreC, blitMaterialRGBA);

                                    texStorage[storageName + col] = textureStoreC;
                                }*/
                            }

                            //if (i == 1) continue;
                            //if (i == 3) continue;
                            TextureField(col, targetProperty.textureValue, tsize, tsize, transparent: false, blitMaterialRGBA);
                            //if (textureStoreC) TextureField(col, textureStoreC, tsize);
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }


            //EditorGUILayout.EndVertical();
        }

        public override IEnumerable<MaterialProperty> GetReferencedProperties(MaterialEditor materialEditor, MaterialProperty[] properties, DrawerParameters parameters)
        {
            var textureProperty = parameters.Get(0, properties);
            if (textureProperty != null && textureProperty.type == MaterialProperty.PropType.Texture) yield return textureProperty;
        }
    }
}

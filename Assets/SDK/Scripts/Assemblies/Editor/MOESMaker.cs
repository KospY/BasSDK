using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThunderRoad
{
    public class MoesMaker : EditorWindow
    {
        private Texture2D textureBase;

        // take 4 textures and combine them into one on each channel
        private Texture2D textureMetallic;
        private SourceChannel metallicChannel;

        private Texture2D textureOcclusion;
        private SourceChannel occlusionChannel;

        private Texture2D textureEmission;
        private Texture2D textureEmissionUser;
        private EmissionSource emissionSource;
        private SourceChannel emissionChannel;

        private Texture2D textureSmoothness;
        private SourceChannel smoothnessChannel;
        private SmoothnessSource smoothnessSource;
        private SmoothnessInvert invertSmoothness;

        private Texture2D textureCombined;

        private Texture2D textureMOES;

        private enum EmissionSource
        {
            Texture,
            Black,
            White,
        }

        private enum SmoothnessSource
        {
            BaseAlpha,
            MetallicAlpha,
        }

        private enum SmoothnessInvert
        {
            Normal,
            Invert,
        }

        private enum SourceChannel
        {
            None,
            Red,
            Green,
            Blue,
            Alpha,
        }

        [MenuItem("Asset/Invert Color")]
        public static void InvertColor()
        {
            foreach (Object o in Selection.objects)
            {
                if (o is Texture2D)
                {
                    string path = AssetDatabase.GetAssetPath(o);
                    //mark the texture as read/write
                    var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (textureImporter == null) continue;
                    bool currentReadable = textureImporter.isReadable;
                    textureImporter.isReadable = true;
                    TextureImporterFormat format = textureImporter.GetAutomaticFormat("Default");
                    TextureImporterPlatformSettings platformSettings = textureImporter.GetDefaultPlatformTextureSettings();
                    platformSettings.format = format;
                    textureImporter.SaveAndReimport();
                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                    Color[] pixels = texture.GetPixels();
                    for (int i = 0; i < pixels.Length; i++)
                    {
                        pixels[i] = InvertColor(pixels[i]);
                    }
                    texture.SetPixels(pixels);
                    texture.Apply();
                    if (!currentReadable)
                    {
                        textureImporter.isReadable = false;
                        textureImporter.SaveAndReimport();
                    }
                }
            }
        }

        [MenuItem("ThunderRoad (SDK)/Tools/MoesMaker")]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow(typeof(MoesMaker));
            window.minSize = new Vector2(640f, 340f);
            window.maxSize = window.minSize;
        }

        bool TextureOverridden(SourceChannel channel)
        {
            return textureCombined && channel != SourceChannel.None;
        }

        void OnGUI()
        {
            EditorGUIUtility.labelWidth = 300f;
            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.Space();
                //heading 
                EditorGUILayout.LabelField("MOES Texture Maker", EditorStyles.boldLabel);
                //line
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                string baseLabel = "Base";
                string metallicLabel = "Metallic";
                string occlusionLabel = "Occlusion";
                string emissionLabel = "Emission";
                string smoothnessLabel = "Smoothness";
                string combinedLabel = "Combined";

                var labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.alignment = TextAnchor.UpperCenter;

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    using (new GUILayout.VerticalScope(GUILayout.Width(100)))
                    {
                        EditorGUILayout.LabelField(baseLabel, labelStyle, GUILayout.Width(100));
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            textureBase = (Texture2D)EditorGUILayout.ObjectField(textureBase, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
                            string path = textureBase != null ? AssetDatabase.GetAssetPath(textureBase) : "None";
                            GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", path));
                            GUILayout.FlexibleSpace();
                        }
                    }
                    using (new GUILayout.VerticalScope(GUILayout.Width(100)))
                    {
                        EditorGUILayout.LabelField(metallicLabel, labelStyle, GUILayout.Width(100));
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            textureMetallic = (Texture2D)EditorGUILayout.ObjectField(textureMetallic, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
                            string path = textureMetallic != null ? AssetDatabase.GetAssetPath(textureMetallic) : "None";
                            GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", path));
                            GUILayout.FlexibleSpace();
                        }
                        if (textureCombined)
                        {
                            metallicChannel = (SourceChannel)EditorGUILayout.EnumPopup(metallicChannel, GUILayout.Width(100));
                        }
                    }
                    using (new GUILayout.VerticalScope(GUILayout.Width(100)))
                    {
                        EditorGUILayout.LabelField(occlusionLabel, labelStyle, GUILayout.Width(100));
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            textureOcclusion = (Texture2D)EditorGUILayout.ObjectField(textureOcclusion, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
                            string path = textureOcclusion != null ? AssetDatabase.GetAssetPath(textureOcclusion) : "None";
                            GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", path));
                            GUILayout.FlexibleSpace();
                        }
                        if (textureCombined)
                        {
                            occlusionChannel = (SourceChannel)EditorGUILayout.EnumPopup(occlusionChannel, GUILayout.Width(100));
                        }
                    }
                    using (new GUILayout.VerticalScope(GUILayout.Width(100)))
                    {
                        EditorGUILayout.LabelField(emissionLabel, labelStyle, GUILayout.Width(100));
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            switch (emissionSource)
                            {
                                case EmissionSource.Texture:
                                    //if the user changed to texture, change textureEmission to textureEmissionUser
                                    textureEmission = textureEmissionUser;
                                    textureEmission = (Texture2D)EditorGUILayout.ObjectField(textureEmission, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
                                    string path = textureEmission != null ? AssetDatabase.GetAssetPath(textureEmission) : "None";
                                    GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", path));
                                    textureEmissionUser = textureEmission;
                                    break;
                                case EmissionSource.Black:
                                    textureEmission = Texture2D.blackTexture;
                                    //read only object field to show the texture
                                    EditorGUILayout.ObjectField(Texture2D.blackTexture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
                                    break;
                                default:
                                    textureEmission = Texture2D.whiteTexture;
                                    //read only object field to show the texture
                                    EditorGUILayout.ObjectField(Texture2D.whiteTexture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
                                    break;
                            }
                            GUILayout.FlexibleSpace();
                        }
                        if (textureCombined)
                        {
                            emissionChannel = (SourceChannel)EditorGUILayout.EnumPopup(emissionChannel, GUILayout.Width(100));
                        }
                        if (emissionChannel == SourceChannel.None)
                        {
                            emissionSource = (EmissionSource)EditorGUILayout.EnumPopup(emissionSource, GUILayout.Width(100));
                        }
                    }
                    using (new GUILayout.VerticalScope(GUILayout.Width(100)))
                    {
                        EditorGUILayout.LabelField(smoothnessLabel, labelStyle, GUILayout.Width(100));
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            textureSmoothness = (Texture2D)EditorGUILayout.ObjectField(textureSmoothness, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
                            string path = textureSmoothness != null ? AssetDatabase.GetAssetPath(textureSmoothness) : "None";
                            GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", path));
                            GUILayout.FlexibleSpace();
                        }

                        if (textureCombined)
                        {
                            smoothnessChannel = (SourceChannel)EditorGUILayout.EnumPopup(smoothnessChannel, GUILayout.Width(100));
                        }
                        if ((textureCombined == null || smoothnessChannel == SourceChannel.None) && textureSmoothness == null)
                        {
                            smoothnessSource = (SmoothnessSource)EditorGUILayout.EnumPopup(smoothnessSource, GUILayout.Width(100));
                        }
                        invertSmoothness = (SmoothnessInvert)EditorGUILayout.EnumPopup(invertSmoothness, GUILayout.Width(100));
                    }
                    using (new GUILayout.VerticalScope(GUILayout.Width(100)))
                    {
                        EditorGUILayout.LabelField(combinedLabel, labelStyle, GUILayout.Width(100));
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            textureCombined = (Texture2D)EditorGUILayout.ObjectField(textureCombined, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
                            string path = textureCombined != null ? AssetDatabase.GetAssetPath(textureCombined) : "None";
                            GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", path));
                            GUILayout.FlexibleSpace();
                        }
                        if (textureCombined && GUILayout.Button("Clear"))
                        {
                            textureCombined = null;
                        }
                    }
                    GUILayout.FlexibleSpace();
                }

                GUILayout.FlexibleSpace();

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    using (new GUILayout.VerticalScope())
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Combine Textures", GUILayout.Width(200)))
                            {
                                CombineTextures();
                            }
                            GUILayout.FlexibleSpace();
                        }

                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            EditorGUILayout.LabelField("Output MOES", labelStyle, GUILayout.Width(100));
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            textureMOES = (Texture2D)EditorGUILayout.ObjectField(textureMOES, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
                            string path = textureMOES != null ? AssetDatabase.GetAssetPath(textureMOES) : "None";
                            GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", path));
                            GUILayout.FlexibleSpace();
                        }

                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("Clear All", GUILayout.Width(200)))
                            {
                                textureBase = null;
                                textureMetallic = null;
                                textureOcclusion = null;
                                textureEmission = null;
                                textureEmissionUser = null;
                                textureSmoothness = null;
                                textureCombined = null;
                                textureMOES = null;
                            }
                            GUILayout.FlexibleSpace();
                        }
                    }
                    GUILayout.FlexibleSpace();
                }
            }
        }

        private Texture2D SplitTexture(Texture2D texture, SourceChannel channel)
        {
            if (channel == SourceChannel.None) return null;
            // make sure the texture is read/write enabled, otherwise GetPixels() will fail
            EnsureReadWriteState(texture, true);
            Texture2D result = new Texture2D(texture.width, texture.height);
            var pixels = texture.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = IsolateChannelValue(pixels[i], channel);
            }
            result.SetPixels(pixels);
            return result;
        }

        private Color IsolateChannelValue(Color color, SourceChannel channel)
        {
            Color result = new Color(0f, 0f, 0f, 0f);
            switch (channel)
            {
                case SourceChannel.Red:
                    result.r = result.g = result.b = result.a = color.r;
                    break;
                case SourceChannel.Green:
                    result.r = result.g = result.b = result.a = color.g;
                    break;
                case SourceChannel.Blue:
                    result.r = result.g = result.b = result.a = color.b;
                    break;
                case SourceChannel.Alpha:
                    result.r = result.g = result.b = result.a = color.a;
                    break;
            }
            return result;
        }

        private void EnsureReadWriteState(Texture2D texture, bool state)
        {
            string path = AssetDatabase.GetAssetPath(texture);
            if (path == null) return;
            if (AssetImporter.GetAtPath(path) is TextureImporter textureImporter)
            {
                if (!textureImporter.isReadable != state) textureImporter.isReadable = state;
                textureImporter.SaveAndReimport();
            }
            else
            {
                Debug.LogError($"The texture at path {path} can't be set to read/write enabled automatically! Make sure the texture is extracted from the asset, or set it to read/write enabled manually.");
            }
        }

        private void CombineTextures()
        {
            //textureMoes is RGBA
            //metallic is R
            //occlusion is G
            //emission is B
            //smoothness is A

            //make the path  the same folder as the base texture
            string path = AssetDatabase.GetAssetPath(textureBase);
            //remove the extension from the path
            path = path.Substring(0, path.LastIndexOf('.'));

            if (textureCombined)
            {
                if (metallicChannel != SourceChannel.None)
                {
                    string fullPath = path + "_Metallic.png";
                    System.IO.File.WriteAllBytes(fullPath, SplitTexture(textureCombined, metallicChannel).EncodeToPNG());
                    AssetDatabase.ImportAsset(fullPath);
                    var guid = AssetDatabase.AssetPathToGUID(fullPath);
                    textureMetallic = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guid));
                }
                if (occlusionChannel != SourceChannel.None)
                {
                    string fullPath = path + "_Occlusion.png";
                    System.IO.File.WriteAllBytes(fullPath, SplitTexture(textureCombined, occlusionChannel).EncodeToPNG());
                    AssetDatabase.ImportAsset(fullPath);
                    var guid = AssetDatabase.AssetPathToGUID(fullPath);
                    textureOcclusion = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guid));
                }
                if (emissionChannel != SourceChannel.None)
                {
                    string fullPath = path + "_Emission.png";
                    System.IO.File.WriteAllBytes(fullPath, SplitTexture(textureCombined, emissionChannel).EncodeToPNG());
                    AssetDatabase.ImportAsset(fullPath);
                    var guid = AssetDatabase.AssetPathToGUID(fullPath);
                    textureEmission = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guid));
                }
                if (smoothnessChannel != SourceChannel.None)
                {
                    string fullPath = path + "_Smoothness.png";
                    System.IO.File.WriteAllBytes(fullPath, SplitTexture(textureCombined, smoothnessChannel).EncodeToPNG());
                    AssetDatabase.ImportAsset(fullPath);
                    var guid = AssetDatabase.AssetPathToGUID(fullPath);
                    textureSmoothness = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guid));
                }
            }

            if (path.Length != 0)
            {
                MoesTexture moes = new MoesTexture();

                moes.path = path;
                moes.metallic = textureMetallic;
                moes.occlusion = textureOcclusion;
                moes.emission = textureEmission;
                moes.smoothness = textureSmoothness;
                moes.moesExists = false;
                moes.isAutodesk = invertSmoothness == SmoothnessInvert.Invert;
                if (!textureSmoothness)
                {
                    if (smoothnessSource == SmoothnessSource.BaseAlpha)
                    {
                        moes.smoothness = textureBase;
                        moes.smoothnessIsAlpha = true;
                    }
                    else
                    {
                        moes.smoothness = textureMetallic;
                        moes.smoothnessIsAlpha = false;
                    }
                }
                MOESConvertWindow.Initialize();
                MOESConvertWindow.CreateTexture(moes);
                //should be created at moes.path_MOES.png
                //read the texture from the path and set it to textureMOES
                string guid = AssetDatabase.AssetPathToGUID($"{moes.path}_MOES.png");
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guid));
                if (texture)
                {
                    textureMOES = texture;
                    // ensure that the output MOES texture isn't set to read/write enabled, since the game doesn't need it and it can save memory
                    EnsureReadWriteState(textureMOES, false);
                }
            }

        }

        //a function that will invert the colours of a pixel
        private static Color InvertColor(Color color) => new(1f - color.r, 1f - color.g, 1f - color.b, color.a);
    }
}

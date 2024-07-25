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
        private Texture2D textureOcclusion;
        private Texture2D textureEmission;
        private Texture2D textureEmissionUser;
        private EmissionSource emissionSource;
        private Texture2D textureSmoothness;
        private SmoothnessSource smoothnessSource;
        private bool invertSmoothness;
        private Texture2D textureMOES;

        private enum EmissionSource
        {
            Black,
            White,
            Texture
        }

        private enum SmoothnessSource
        {
            BaseAlpha,
            MetallicAlpha
        }

        [MenuItem("Asset/Invert Color")]
        public static void InvertColor()
        {
            foreach (Object o in Selection.objects)
            {
                if (o is Texture2D)
                {
                    var path = AssetDatabase.GetAssetPath(o);
                    //mark the texture as read/write
                    var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (textureImporter == null) continue;
                    bool currentReadable = textureImporter.isReadable;
                    textureImporter.isReadable = true;
                    var format = textureImporter.GetAutomaticFormat("Default");
                    var platformSettings = textureImporter.GetDefaultPlatformTextureSettings();
                    platformSettings.format = format;
                    textureImporter.SaveAndReimport();
                    var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                    var pixels = texture.GetPixels();
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
            EditorWindow.GetWindow(typeof(MoesMaker));
        }

        void OnGUI()
        {
            EditorGUIUtility.labelWidth = 300f;
            EditorGUILayout.BeginVertical(GUILayout.Width(600));
            {
                EditorGUILayout.Space();
                //heading 
                EditorGUILayout.LabelField("MOES Texture Maker", EditorStyles.boldLabel);
                //line
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                //button to combine textures
                if (GUILayout.Button("Combine Textures"))
                {
                    CombineTextures();
                }
                //button to clear textures
                if (GUILayout.Button("Clear Textures"))
                {
                    textureBase = null;
                    textureMetallic = null;
                    textureOcclusion = null;
                    textureEmission = null;
                    textureEmissionUser = null;
                    textureSmoothness = null;
                    textureMOES = null;
                }

                string baseLabel = textureBase ? $"Base: {textureBase.name}" : "Base";
                string metallicLabel = textureMetallic ? $"Metallic: {textureMetallic.name}" : "Metallic";
                string occlusionLabel = textureOcclusion ? $"Occlusion: {textureOcclusion.name}" : "Occlusion";
                string emissionLabel = textureEmission ? $"Emission: {textureEmission.name}" : "Emission";
                string smoothnessLabel = textureSmoothness ? $"Smoothness: {textureSmoothness.name}" : "Smoothness";

                textureBase = (Texture2D)EditorGUILayout.ObjectField(baseLabel, textureBase, typeof(Texture2D), false);
                textureMetallic = (Texture2D)EditorGUILayout.ObjectField(metallicLabel, textureMetallic, typeof(Texture2D), false);
                textureOcclusion = (Texture2D)EditorGUILayout.ObjectField(occlusionLabel, textureOcclusion, typeof(Texture2D), false);
                switch (emissionSource)
                {
                    case EmissionSource.Texture:
                        //if the user changed to texture, change textureEmission to textureEmissionUser
                        textureEmission = textureEmissionUser;
                        textureEmission = (Texture2D)EditorGUILayout.ObjectField(emissionLabel, textureEmission, typeof(Texture2D), false);
                        textureEmissionUser = textureEmission;
                        break;
                    case EmissionSource.Black:
                        textureEmission = Texture2D.blackTexture;
                        //read only object field to show the texture
                        EditorGUILayout.ObjectField(emissionLabel, Texture2D.blackTexture, typeof(Texture2D), false);
                        break;
                    default:
                        textureEmission = Texture2D.whiteTexture;
                        //read only object field to show the texture
                        EditorGUILayout.ObjectField(emissionLabel, Texture2D.whiteTexture, typeof(Texture2D), false);
                        break;
                }
                emissionSource = (EmissionSource)EditorGUILayout.EnumPopup("Emission Source", emissionSource);

                textureSmoothness = (Texture2D)EditorGUILayout.ObjectField(smoothnessLabel, textureSmoothness, typeof(Texture2D), false);

                //if smoothness is null then ask user if they want to use the alpha channel of the metallic or the base map
                if (!textureSmoothness)
                {
                    smoothnessSource = (SmoothnessSource)EditorGUILayout.EnumPopup("Smoothness Source", smoothnessSource);
                }
                else
                {
                    //ask the user if they want to invert it, if its a roughnessmap instead of smoothness
                    invertSmoothness = EditorGUILayout.Toggle("Invert Smoothness", invertSmoothness);
                }

                string moesLabel = textureMOES ? $"MOES: {textureMOES.name}" : "MOES";

                textureMOES = (Texture2D)EditorGUILayout.ObjectField(moesLabel, textureMOES, typeof(Texture2D), false);
            }
            EditorGUILayout.EndVertical();


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
            if (path.Length != 0)
            {
                MoesTexture moes = new MoesTexture();

                moes.path = path;
                moes.metallic = textureMetallic;
                moes.occlusion = textureOcclusion;
                moes.emission = textureEmission;
                moes.smoothness = textureSmoothness;
                moes.moesExists = false;
                moes.isAutodesk = invertSmoothness;
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
                var guid = AssetDatabase.AssetPathToGUID($"{moes.path}_MOES.png");
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guid));
                if (texture)
                {
                    textureMOES = texture;
                }
            }

        }

        //a function that will invert the colours of a pixel
        private static Color InvertColor(Color color)
        {
            return new Color(1f - color.r, 1f - color.g, 1f - color.b, color.a);
        }
    }
}

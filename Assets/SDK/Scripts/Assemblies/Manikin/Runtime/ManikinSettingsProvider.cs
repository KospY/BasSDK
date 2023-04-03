#if UNITY_EDITOR
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Manikin
{
    // Create a new type of Settings asset.
    public class ManikinSettingsProvider : ScriptableObject
    {
        public static readonly string[] k_ManikinSettingsPaths = { "Assets/Editor/ManikinSettings.asset" };

        [System.Serializable]
        public enum VectorChannel { X,Y,Z,W }

        [System.Serializable]
        public enum VectorSize { Vector2, Vector3, Vector4 }

        [System.Serializable]
        public enum VertexChannel { UV0, UV1, UV2, UV3, Color }

        public VectorChannel vertexOcclusionVectorChannel { get { return _vertexOcclusionVectorChannel; } }
        public VectorSize vertexOcclusionVectorSize { get { return _vertexOcclusionVectorSize; } }
        public VertexChannel vertexOcclusionVertexChannel { get { return _vertexOcclusionVertexChannel; } }

        [SerializeField, Tooltip("The vector channel on the selected Vertex Attribute that the data will be saved to.")]
        private VectorChannel _vertexOcclusionVectorChannel = VectorChannel.X;
        [SerializeField, Tooltip("The size of the vector for this vertex attribute.")]
        private VectorSize _vertexOcclusionVectorSize = VectorSize.Vector2;
        [SerializeField, Tooltip("Vertex Attribute that the data will be saved to.")]
        private VertexChannel _vertexOcclusionVertexChannel = VertexChannel.UV1;

        //Asset Preview
        public Vector3 meshRotation { get { return _meshRotation; } }
        public Color backgroundcolor { get { return _backgroundColor; } }
        public float cameraDistanceFactor { get { return _cameraDistanceFactor; } }
        public float cameraRotationX { get { return _cameraRotationX; } }
        public float cameraRotationY { get { return _cameraRotationY; } }
        public bool cachePreviews { get { return _cachePreviews; } }
        public int previewDimensions { get { return _previewDimensions; } }
        public string previewsFolder { get { return _previewsFolder; } }

        [SerializeField, Tooltip("Rotation applied to the meshes in the part previews.")]
        private Vector3 _meshRotation = new Vector3(-90, 180, 0);
        [SerializeField, Tooltip("Color of the background on the buttons for the parts selection.")]
        private Color _backgroundColor = new Color(0.345f, 0.345f, 0.345f);
        [SerializeField, Tooltip("Multiplying factor for the camera distance when generating the asset preview based on the meshes bounding box.")]
        float _cameraDistanceFactor = 3;
        [SerializeField, Tooltip("Camera rotation around the X axis for generating the asset preview.")]
        float _cameraRotationX = -45;
        [SerializeField, Tooltip("Camera rotation around the Y axis for generating the asset preview.")]
        float _cameraRotationY = 45;
        [SerializeField, Tooltip("Turn on or off preview caching. If off, the selection window will always generate new previews and not write them to disk.")]
        bool _cachePreviews = true;
        [SerializeField, Tooltip("Width and Height dimension of the preview textures.")]
        int _previewDimensions = 64;
        [SerializeField, Tooltip("Folder to store the cached Manikin Previews.")]
        string _previewsFolder = "Assets/ManikinPreviews";


        public static ManikinSettingsProvider GetSettings()
        {
            ManikinSettingsProvider settings = null;

            string loadpath = EditorPrefs.GetString("ManikinSettingsProviderPath", "");
            if(!string.IsNullOrEmpty(loadpath))
            {
                settings = AssetDatabase.LoadAssetAtPath<ManikinSettingsProvider>(loadpath);
            }

            if (settings == null)
            {
                for (int i = 0; i < k_ManikinSettingsPaths.Length; i++)
                {
                    settings = AssetDatabase.LoadAssetAtPath<ManikinSettingsProvider>(k_ManikinSettingsPaths[i]);
                    if (settings != null)
                        break;
                }
            }

            if (settings == null)
            {
                string[] results = AssetDatabase.FindAssets("ManikinSettings t:ManikinSettingsProvider", null);
                if(results.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(results[0]);
                    settings = AssetDatabase.LoadAssetAtPath<ManikinSettingsProvider>(path);

                    EditorPrefs.SetString("ManikinSettingsProviderPath", path);
                }
            }

            return settings;
        }

        internal static ManikinSettingsProvider GetOrCreateSettings()
        {
            var settings = ManikinSettingsProvider.GetSettings();

            if (settings == null)
            {
                Debug.LogWarning("Creating new manikin settings...");
                settings = ScriptableObject.CreateInstance<ManikinSettingsProvider>();
                AssetDatabase.CreateAsset(settings, k_ManikinSettingsPaths[0]);
                AssetDatabase.SaveAssets();
                EditorPrefs.SetString("ManikinSettingsProviderPath", k_ManikinSettingsPaths[0]);
            }
            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }

    // Register a SettingsProvider using IMGUI for the drawing framework:
    static class ManikinSettingsIMGUIRegister
    {
        [SettingsProvider]
        public static SettingsProvider CreateManikinSettingsProvider()
        {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider("Project/Manikin Settings", SettingsScope.Project)
            {
                // By default the last token of the path is used as display name if no label is provided.
                label = "Manikin",
                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) =>
                {
                    var settings = ManikinSettingsProvider.GetSerializedSettings();
                    settings.Update();
                    EditorGUI.indentLevel++;

                    EditorGUILayout.LabelField("Vertex Occlusion", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(settings.FindProperty("_vertexOcclusionVertexChannel"));
                    EditorGUILayout.PropertyField(settings.FindProperty("_vertexOcclusionVectorSize"));
                    EditorGUILayout.PropertyField(settings.FindProperty("_vertexOcclusionVectorChannel"));
                    EditorGUI.indentLevel--;

                    GUILayout.Space(10);
                    EditorGUILayout.LabelField("Asset Preview", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(settings.FindProperty("_meshRotation"));
                    EditorGUILayout.PropertyField(settings.FindProperty("_backgroundColor"));
                    EditorGUILayout.PropertyField(settings.FindProperty("_cameraDistanceFactor"));
                    EditorGUILayout.PropertyField(settings.FindProperty("_cameraRotationX"));
                    EditorGUILayout.PropertyField(settings.FindProperty("_cameraRotationY"));
                    EditorGUILayout.PropertyField(settings.FindProperty("_cachePreviews"));
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(settings.FindProperty("_previewDimensions"));
                    if(EditorGUI.EndChangeCheck())
                    {
                        SerializedProperty dimensions = settings.FindProperty("_previewDimensions");
                        dimensions.intValue = Mathf.ClosestPowerOfTwo(dimensions.intValue);
                    }

                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty previewFolder = settings.FindProperty("_previewsFolder");
                    EditorGUILayout.PropertyField(previewFolder);
                    if(GUILayout.Button("Select"))
                    {
                        string absolutePath = EditorUtility.OpenFolderPanel("Select Folder", "Assets", "");
                        if (!string.IsNullOrEmpty(absolutePath) && absolutePath.StartsWith(Application.dataPath))
                        {
                            previewFolder.stringValue = "Assets" + absolutePath.Substring(Application.dataPath.Length);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    if(!AssetDatabase.IsValidFolder(previewFolder.stringValue))
                    {
                        EditorGUILayout.HelpBox("Preview Folder not found!", MessageType.Warning);
                    }

                    EditorGUI.indentLevel--;

                    EditorGUI.indentLevel--;
                    settings.ApplyModifiedProperties();
                },

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "vertex", "occlusion", "preview" })
            };

            return provider;
        }
    }
}
#endif
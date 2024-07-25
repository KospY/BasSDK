using UnityEngine;
using UnityEditor;

namespace ThunderRoad
{
    [CustomEditor(typeof(LightProbeVolumeGenerator))]
    public class LightProbeVolumeGeneratorEditor : Editor
    {
        readonly string[] exclude = new string[] { "textureFormat", "textureResInterval", "layerToSet" };
        const int numTextures = 4;
        const int bytesPerPixel = 8;

        SerializedProperty textureResolution;
        SerializedProperty lightProbeResolution;
        SerializedProperty autoUpdateProbes;
        SerializedProperty autoUpdateBoxCollider;
        SerializedProperty useMipmaps;
        SerializedProperty textureFormat;
        SerializedProperty textureResInterval;
        SerializedProperty layerToSet;
        SerializedProperty boxColliderSizeOffset;
        SerializedProperty autoSetLayer;
        SerializedProperty visualizeIntervals;
        SerializedProperty visualizeDataPlane;
        SerializedProperty dataPlaneAxisPosition;

        void OnEnable()
        {
            textureResolution = serializedObject.FindProperty("textureResolution");
            lightProbeResolution = serializedObject.FindProperty("lightprobeResolution");
            autoUpdateProbes = serializedObject.FindProperty("autoUpdateLightProbes");
            useMipmaps = serializedObject.FindProperty("useMipmaps");
            textureFormat = serializedObject.FindProperty("textureFormat");
            textureResInterval = serializedObject.FindProperty("textureResInterval");
            layerToSet = serializedObject.FindProperty("layerToSet");
            autoSetLayer = serializedObject.FindProperty("autoSetLayer");
            autoUpdateBoxCollider = serializedObject.FindProperty("autoUpdateBoxCollider");
            boxColliderSizeOffset = serializedObject.FindProperty("boxColliderSizeOffset");
            visualizeIntervals = serializedObject.FindProperty("visualizeIntervals");
            visualizeDataPlane = serializedObject.FindProperty("visualizeDataPlane");
            dataPlaneAxisPosition = serializedObject.FindProperty("dataPlaneAxisPosition");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            LightProbeVolumeGenerator probeVolumeGenerator = target as LightProbeVolumeGenerator;
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            centeredStyle.fontStyle = FontStyle.Bold;

            //DrawPropertiesExcluding(serializedObject, exclude);

            #region Texture Options
            EditorGUILayout.BeginVertical("HelpBox");
            EditorGUILayout.LabelField("Texture Volume", centeredStyle);
            EditorGUILayout.PropertyField(textureResolution);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(textureResInterval);
            if (GUILayout.Button("Calculate Texture Resolution", GUILayout.MaxWidth(200)))
            {
                Vector3 size = probeVolumeGenerator.lightProbeVolume.size;
                Vector3Int textureSize = new Vector3Int();
                textureSize.x = Mathf.ClosestPowerOfTwo((int)(size.x / textureResInterval.vector2Value.x));
                textureSize.y = Mathf.ClosestPowerOfTwo((int)(size.y / textureResInterval.vector2Value.y));
                textureSize.z = Mathf.ClosestPowerOfTwo((int)(size.z / textureResInterval.vector2Value.x)); //x is horizontal so we use it for size.x and size.z;
                textureResolution.vector3IntValue = textureSize;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(textureFormat);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(useMipmaps);

            long estimated = (probeVolumeGenerator.textureResolution.x * probeVolumeGenerator.textureResolution.y * probeVolumeGenerator.textureResolution.z);
            if (estimated > (1024.0f / numTextures / bytesPerPixel))
            {
                if (estimated > (1048576.0f / numTextures / bytesPerPixel))
                {
                    EditorGUILayout.LabelField("ProbeVolume total textures size", (estimated * numTextures * (bytesPerPixel / 1048576.0f)) + " mb  + mip chain");
                }
                else
                {
                    EditorGUILayout.LabelField("ProbeVolume total textures size", (estimated * numTextures * (bytesPerPixel / 1024.0f)) + " kb  + mip chain");
                }
            }
            else
            {
                EditorGUILayout.LabelField("ProbeVolume total textures size", (estimated * numTextures * bytesPerPixel) + " bytes + mip chain");
            }

            if (GUILayout.Button("Create 3D Textures"))
            {
                bool doCreate = true;
                if (estimated > 30000000)
                {
                    doCreate = EditorUtility.DisplayDialog("Warning", "This will be a very large texture. Are you sure you want to continue?", "OK", "Cancel");
                }

                if (doCreate)
                {
                    probeVolumeGenerator.Generate3dTextures();
                }
            }
            EditorGUILayout.EndVertical();
            #endregion

            EditorGUILayout.BeginVertical("HelpBox");
            EditorGUILayout.LabelField("Lightprobe Tools", centeredStyle);
            EditorGUILayout.PropertyField(autoUpdateProbes);
            EditorGUILayout.PropertyField(lightProbeResolution);
            if (!autoUpdateProbes.boolValue)
            {
                if (GUILayout.Button("Update LightProbe Group"))
                {
                    probeVolumeGenerator.UpdateLightProbes();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("HelpBox");
            EditorGUILayout.LabelField("BoxCollider Tools", centeredStyle);
            EditorGUILayout.PropertyField(autoUpdateBoxCollider);
            EditorGUILayout.PropertyField(boxColliderSizeOffset);
            if (!autoUpdateBoxCollider.boolValue)
            {
                if (GUILayout.Button("Update Box Collider"))
                {
                    probeVolumeGenerator.UpdateBoxCollider();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("HelpBox");
            EditorGUILayout.LabelField("Misc", centeredStyle);
            EditorGUILayout.PropertyField(visualizeIntervals);
            EditorGUILayout.PropertyField(visualizeDataPlane);

            if (visualizeDataPlane.boolValue)
            {
                EditorGUILayout.PropertyField(dataPlaneAxisPosition);
            }

            EditorGUILayout.PropertyField(autoSetLayer);
            if (autoSetLayer.boolValue)
            {
                layerToSet.intValue = EditorGUILayout.LayerField(layerToSet.displayName, layerToSet.intValue);
            }
            EditorGUILayout.EndVertical();

            Collider collider = probeVolumeGenerator.GetComponentInChildren<Collider>(true);
            if (collider == null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("No Collider found! Recommend at least one trigger on this object or a child.", MessageType.Warning);
                if (GUILayout.Button("Fix Now", GUILayout.MaxWidth(100)))
                {
                    BoxCollider boxCollider = probeVolumeGenerator.gameObject.AddComponent<BoxCollider>();
                    boxCollider.isTrigger = true;
                    probeVolumeGenerator.UpdateBoxCollider();
                }
                EditorGUILayout.EndHorizontal();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}

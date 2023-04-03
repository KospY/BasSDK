using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Manikin
{
    [CustomEditor(typeof(ManikinMorphAsset))]
    public class ManikinMorphAssetEditor : Editor
    {
        SerializedProperty morphName;
        SerializedProperty meshGuid;
        SerializedProperty fileID;

        string[] blendShapeNames;

        int selectedIndex = 0;

        private void OnEnable()
        {
            morphName = serializedObject.FindProperty("morphName");
            meshGuid = serializedObject.FindProperty("meshGuid");
            fileID = serializedObject.FindProperty("fileID");
            UpdateBlendShapeNames();
        }

        private void UpdateBlendShapeNames()
        {
            ManikinMorphAsset morph = target as ManikinMorphAsset;
            blendShapeNames = ManikinMorphAsset.BlendShapeNamesFromMesh(morph.targetMesh);
            selectedIndex = 0;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ManikinMorphAsset morph = target as ManikinMorphAsset;

            EditorGUI.BeginChangeCheck();
            Mesh targetMesh = EditorGUILayout.ObjectField("Target Mesh", morph.targetMesh, typeof(Mesh), false) as Mesh;
            if(EditorGUI.EndChangeCheck())
            {
                if(targetMesh != null && AssetDatabase.TryGetGUIDAndLocalFileIdentifier(targetMesh, out string guid, out long localId))
                {
                    meshGuid.stringValue = guid;
                    fileID.longValue = localId;
                }
                UpdateBlendShapeNames();                
            }

            EditorGUILayout.PropertyField(morphName);
            EditorGUILayout.LabelField("Morph Hash", morph.MorphHash.ToString());
            EditorGUILayout.LabelField("Target Mesh GUID", meshGuid.stringValue);
            EditorGUILayout.LabelField("Target Mesh FildID", fileID.longValue.ToString());

            selectedIndex = EditorGUILayout.Popup("Available BlendShapes", selectedIndex, blendShapeNames);

            if(GUILayout.Button("Extract Blendshape from Mesh"))
            {
                if (morph.targetMesh == null)
                {
                    Debug.LogError("Morph Target is null!");
                }
                else
                {
                    if (EditorUtility.DisplayDialog("Extract Morph Data", "This will overwrite any existing data. Are you sure?", "Yes", "Cancel"))
                    {
                        if (morph.ExtractBlendShape(blendShapeNames[selectedIndex], morph.targetMesh))
                        {
                            if (EditorUtility.DisplayDialog("Update Name", "Update asset name to extracted BlendShape name?", "Yes", "No"))
                            {
                                morph.name = blendShapeNames[selectedIndex];

                                string pathName = AssetDatabase.GetAssetPath(morph);
                                string error = AssetDatabase.RenameAsset(pathName, morph.name);
                                if (!string.IsNullOrEmpty(error))
                                    Debug.LogError(error);
                                AssetDatabase.Refresh();
                            }
                        }
                        else
                        {
                            Debug.LogError("Extract blendshape failed!", morph);
                        }
                    }
                }
            }

            EditorGUILayout.LabelField("Delta Vertices Count: " + morph.deltaVertices.Length);
            EditorGUILayout.LabelField("Delta Normals Count: " + morph.deltaNormals.Length);
            EditorGUILayout.LabelField("Delta Tangents Count: " + morph.deltaTangents.Length);
            EditorGUILayout.LabelField("Memory Size: " + (((sizeof(float) * 3) * 3) * morph.deltaVertices.Length) / 1000.0 + " KB");
            serializedObject.ApplyModifiedProperties();
        }
    }
}

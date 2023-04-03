using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ThunderRoad.Manikin
{
    [CustomEditor(typeof(ManikinProperties))]
    public class ManikinPropertiesEditor : Editor
    {
        bool autoUpdate = true;

        SerializedProperty useSRPBatcher;
        SerializedProperty renderer;
        SerializedProperty parentProperties;
        SerializedProperty partList;
        SerializedProperty childrenProperties;
        ReorderableList propertiesList;
        
        SerializedProperty occlusionID;
        SerializedProperty occlusionIDHash;
        SerializedProperty occlusionBitmaskProperty;
        SerializedProperty occlusionBitmask;

        SerializedProperty applyBitmaskEveryUpdate;

        private void OnEnable()
        {
            autoUpdate = EditorPrefs.GetBool("Manikin_PropertiesAutoUpdate", true);

            useSRPBatcher = serializedObject.FindProperty("useSRPBatcher");
            renderer = serializedObject.FindProperty("_renderer");
            parentProperties = serializedObject.FindProperty("parentProperties");
            partList = serializedObject.FindProperty("partList");
            childrenProperties = serializedObject.FindProperty("childrenProperties");
            propertiesList = new ReorderableList(serializedObject, serializedObject.FindProperty("properties"), true, true, true, true);
            propertiesList.drawElementCallback = DrawElement;
            propertiesList.elementHeightCallback = ElementHeight;
            propertiesList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Properties");
            };

            occlusionID = serializedObject.FindProperty("occlusionID");
            occlusionIDHash = serializedObject.FindProperty("occlusionIDHash");
            occlusionBitmaskProperty = serializedObject.FindProperty("occlusionBitmaskProperty");
            occlusionBitmask = serializedObject.FindProperty("occlusionBitmask");

            applyBitmaskEveryUpdate = serializedObject.FindProperty("applyBitmaskEveryUpdate");
        }

        private float ElementHeight(int index)
        {
            return ((EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = propertiesList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(rect, element);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            bool propertiesChanged = false;

            ManikinProperties properties = target as ManikinProperties;

            EditorGUILayout.BeginVertical("HelpBox");
            EditorGUILayout.LabelField("Editor Settings");
            EditorGUI.BeginChangeCheck();
            autoUpdate = EditorGUILayout.Toggle("AutoUpdate", autoUpdate);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool("Manikin_PropertiesAutoUpdate", autoUpdate);
            }
            if (autoUpdate) { EditorGUILayout.HelpBox("Color pickers will update and close immediately on selection while AutoUpdate is on.", MessageType.Warning); }
            EditorGUILayout.EndVertical();

            EditorGUILayout.PropertyField(useSRPBatcher);
            EditorGUILayout.PropertyField(applyBitmaskEveryUpdate);
            EditorGUILayout.PropertyField(occlusionBitmaskProperty);
            EditorGUILayout.PropertyField(occlusionID);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(occlusionIDHash);
            EditorGUILayout.PropertyField(occlusionBitmask);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginChangeCheck();
            EditorGUIUtility.labelWidth = 100;
            propertiesList.DoLayoutList();
            EditorGUIUtility.labelWidth = 0;
            if (EditorGUI.EndChangeCheck())
            {
                propertiesChanged = true;
            }

            EditorGUI.BeginDisabledGroup(autoUpdate);
            if (GUILayout.Button("Update Properties"))
            {
                ((ManikinProperties)target).UpdateProperties();
            }
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Save to file"))
            {
                string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, "manikin_properties", "json");
                if (!string.IsNullOrEmpty(path))
                {
                    System.IO.File.WriteAllText(path, properties.ToJson(true));
                    AssetDatabase.Refresh();
                }
            }

            if (GUILayout.Button("Load from file"))
            {
                string path = EditorUtility.OpenFilePanel("Load", Application.dataPath, "json");
                if (!string.IsNullOrEmpty(path))
                {
                    string json = System.IO.File.ReadAllText(path);
                    properties.FromJson(json);
                    propertiesChanged = true;
                }
            }

            if (autoUpdate && propertiesChanged)
            {
                serializedObject.ApplyModifiedProperties();
                ((ManikinProperties)target).UpdateProperties();
            }

            EditorGUI.BeginDisabledGroup(true);
            if (renderer.objectReferenceValue != null)
            {
                EditorGUILayout.PropertyField(renderer);
            }
            EditorGUILayout.PropertyField(parentProperties);
            EditorGUILayout.PropertyField(partList);
            EditorGUILayout.PropertyField(childrenProperties);
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Plugins
{
    [CustomEditor(typeof(MaterialInstance))]
    public class MaterialInstanceEditor : Editor
    {
        readonly static string[] excludes = new string[] { "defaultMaterials", "instanceMaterials" };

        SerializedProperty defaultMaterials;
        SerializedProperty instanceMaterials;

        private void OnEnable()
        {
            defaultMaterials = serializedObject.FindProperty("defaultMaterials");
            instanceMaterials = serializedObject.FindProperty("instanceMaterials");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(defaultMaterials, true);
            EditorGUILayout.PropertyField(instanceMaterials, true);
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
}

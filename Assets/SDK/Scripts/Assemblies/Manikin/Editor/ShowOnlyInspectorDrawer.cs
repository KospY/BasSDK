using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Manikin
{
    [CustomPropertyDrawer(typeof(ShowOnlyInspectorAttribute))]
    public class ShowOnlyInspectorDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginDisabledGroup(true);
            //GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            //GUI.enabled = true;
            EditorGUI.EndDisabledGroup();
        }
    }
}

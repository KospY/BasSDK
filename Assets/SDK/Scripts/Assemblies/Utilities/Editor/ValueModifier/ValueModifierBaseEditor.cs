using UnityEditor;
using UnityEngine;

namespace ThunderRoad.Utilities
{
    [CustomEditor(typeof(ValueModifierBase), true)]
    public class ValueModifierBaseEditor : Editor
    {
        private static readonly string[] propertyToExclude = {"m_Script"};
        protected ValueModifierBase valueModifier;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CustomOnInspectorGUI(); 
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void CustomOnInspectorGUI()
        {
            valueModifier = target as ValueModifierBase;

            EditorGUILayout.LabelField(valueModifier.MenuName, EditorStyles.boldLabel);
            DrawPropertiesExcluding(serializedObject, propertyToExclude);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.TextArea(valueModifier.description, EditorStyles.label);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
    }
}
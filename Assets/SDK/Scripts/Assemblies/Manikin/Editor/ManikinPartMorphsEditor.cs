using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Manikin
{
    [CustomEditor(typeof(ManikinPartMorphs))]
    public class ManikinPartMorphsEditor : Editor
    {
        SerializedProperty isDirty;
        SerializedProperty morphAssets;

        SerializedProperty morphKeys;
        SerializedProperty morphValues;

        private void OnEnable()
        {
            isDirty = serializedObject.FindProperty("isDirty");
            morphAssets = serializedObject.FindProperty("morphAssets");

            morphKeys = serializedObject.FindProperty("morphKeys");
            morphValues = serializedObject.FindProperty("morphValues");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ManikinPartMorphs morpher = target as ManikinPartMorphs;

            //base.OnInspectorGUI();
            EditorGUILayout.PropertyField(morphAssets);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(isDirty);

            List<KeyValuePair<int, float>> morphs = morpher.GetMorphKeyValuePairs();
            foreach (var kvp in morphs)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(kvp.Key.ToString());
                EditorGUILayout.LabelField(kvp.Value.ToString());
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}

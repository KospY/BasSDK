using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Manikin
{
    [CustomEditor(typeof(VertexBitmaskVolume))]
    public class VertexBitmaskVolumeEditor : Editor
    {
        SerializedProperty uvChannel;
        SerializedProperty vectorChannel;
        SerializedProperty volumeType;
        SerializedProperty radius;
        SerializedProperty position;
        SerializedProperty rotation;
        SerializedProperty size;
        SerializedProperty bitmask;

        string[] displayOptions = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23","24","25","26","27","28","29","30" };

        private void OnEnable()
        {
            uvChannel = serializedObject.FindProperty("uvChannel");
            vectorChannel = serializedObject.FindProperty("vectorChannel");
            volumeType = serializedObject.FindProperty("volumeType");
            radius = serializedObject.FindProperty("radius");
            position = serializedObject.FindProperty("position");
            rotation = serializedObject.FindProperty("rotation");
            size = serializedObject.FindProperty("size");
            bitmask = serializedObject.FindProperty("bitmaskValue");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(uvChannel);
            EditorGUILayout.PropertyField(vectorChannel);
            EditorGUILayout.PropertyField(volumeType);
            EditorGUILayout.PropertyField(position);

            switch(volumeType.enumValueIndex)
            {
                case (int)VertexBitmaskVolume.VolumeType.Box:
                    EditorGUILayout.PropertyField(rotation);
                    EditorGUILayout.PropertyField(size);
                    break;
                case (int)VertexBitmaskVolume.VolumeType.Sphere:
                    EditorGUILayout.PropertyField(radius);
                    break;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(bitmask, new GUIContent("Bitmask int Value"));
            bitmask.intValue = EditorGUILayout.MaskField(GUIContent.none, bitmask.intValue, displayOptions, GUILayout.MaxWidth(200));
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}

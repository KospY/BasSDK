using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Manikin
{
    [CustomEditor(typeof(VertexBitmaskDataViewer))]
    public class VertexBitmaskDataViewerEditor : Editor
    {
        SerializedProperty bitmask;

        string[] displayOptions = new string[] { "1 (1)", "2 (2)", "3 (4)", "4 (8)", "5 (16)", "6 (32)", "7 (64)", "8 (128)","9 (256)",
            "10 (512)","11 (1024)","12 (2048)","13 (4096)","14 (8192)","15 (16384)","16 (32768)","17 (65536)","18 (131072)",
            "19 (262144)","20 (524288)","21 (1048576)","22 (2097152)","23 (4194304)","24 (8388608)","25 (16777216)","26 (33554432)","27 (67108864)","28 (134217728)","29 (268435456)","30 (536870912)" };

        private void OnEnable()
        {
            bitmask = serializedObject.FindProperty("bitmask");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            VertexBitmaskDataViewer viewer = target as VertexBitmaskDataViewer;

            DrawPropertiesExcluding(serializedObject, new string[] { "m_Script", "bitmask" });

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(bitmask, new GUIContent("Bitmask int Value"));
            bitmask.intValue = EditorGUILayout.MaskField(GUIContent.none, bitmask.intValue, displayOptions, GUILayout.MaxWidth(200));
            EditorGUILayout.EndHorizontal();

            if (viewer.skinnedMeshRenderer == null)
                EditorGUILayout.HelpBox("No Skinned Mesh Renderer found on this GameObject!", MessageType.Error);

            if(viewer.vertices == null)
                EditorGUILayout.HelpBox("No Vertices found on this GameObject!", MessageType.Error);

            if(viewer.uvs == null || viewer.uvs.Count == 0)
                EditorGUILayout.HelpBox("No UV data for the selected channel found on this GameObject!", MessageType.Error);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

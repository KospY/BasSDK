using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Reveal
{
    
    public class RevealMaterialControllerEditor : Editor
    {
        SerializedProperty maskPropertyName;
        SerializedProperty width;
        SerializedProperty height;
        SerializedProperty volumeDepth;
        SerializedProperty antiAliasing;
        SerializedProperty renderTextureFormat;
        SerializedProperty filterMode;
        SerializedProperty depthBufferBits;
        SerializedProperty restoreMaterialsOnReset;
        SerializedProperty generateMipMaps;

        GUIContent[] dimensionLabels = new GUIContent[] { new GUIContent("32"), new GUIContent("64"), new GUIContent("128"), new GUIContent("256"),
            new GUIContent("512"), new GUIContent("1024"), new GUIContent("2048"), new GUIContent("4096") };
        int[] dimensionValues = new int[] { 32, 64, 128, 256, 512, 1024, 2048, 4096 };

        GUIContent[] aaLabels = new GUIContent[] { new GUIContent("None"), new GUIContent("2 Samples"), new GUIContent("4 Samples"), new GUIContent("8 Samples") };
        int[] aaValues = new int[] { 1, 2, 4, 8 };

        GUIContent[] depthLabels = new GUIContent[] { new GUIContent("No depth buffer"), new GUIContent("At least 16 bits depth (no stencil)"), new GUIContent("At least 24 bits depth (with stencil)") };
        int[] depthValues = new int[] { 0, 16, 24 };

        private void OnEnable()
        {
            maskPropertyName = serializedObject.FindProperty("maskPropertyName");
            width = serializedObject.FindProperty("width");
            height = serializedObject.FindProperty("height");
            volumeDepth = serializedObject.FindProperty("volumeDepth");
            antiAliasing = serializedObject.FindProperty("antiAliasing");
            renderTextureFormat = serializedObject.FindProperty("renderTextureFormat");
            filterMode = serializedObject.FindProperty("filterMode");
            depthBufferBits = serializedObject.FindProperty("depthBufferBits");
            restoreMaterialsOnReset = serializedObject.FindProperty("restoreMaterialsOnReset");
            generateMipMaps = serializedObject.FindProperty("generateMipMaps");
        }

        public override void OnInspectorGUI()
        {
            
        }

        private void QuickSetDimensions(float scale)
        {
            
        }
    }
}

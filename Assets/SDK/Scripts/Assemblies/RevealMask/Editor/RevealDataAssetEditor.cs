using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Reveal
{
    
    public class RevealDataAssetEditor : Editor
    {
        SerializedProperty texture;
        SerializedProperty revealData;

        private void OnEnable()
        {
            texture = serializedObject.FindProperty("texture");
            revealData = serializedObject.FindProperty("revealData");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(texture);

            if (texture.objectReferenceValue != null)
            {
                float spacing = 10f;
                float widthOverFour = ((EditorGUIUtility.currentViewWidth - 20) / 4f) - spacing;

                Rect labelRect = EditorGUILayout.GetControlRect(false, 20f);
                EditorGUI.LabelField(labelRect, "Red");
                labelRect.x += widthOverFour + spacing;
                EditorGUI.LabelField(labelRect, "Green");
                labelRect.x += widthOverFour + spacing;
                EditorGUI.LabelField(labelRect, "Blue");
                labelRect.x += widthOverFour + spacing;
                EditorGUI.LabelField(labelRect, "Alpha");

                Rect previewRect = EditorGUILayout.GetControlRect(false, widthOverFour );
                previewRect.width = widthOverFour;
                previewRect.height = widthOverFour;

                EditorGUI.DrawPreviewTexture(previewRect, texture.objectReferenceValue as Texture, null, ScaleMode.ScaleToFit, 0, -1, UnityEngine.Rendering.ColorWriteMask.Red);

                previewRect.x += widthOverFour + spacing;
                EditorGUI.DrawPreviewTexture(previewRect, texture.objectReferenceValue as Texture, null, ScaleMode.ScaleToFit, 0, -1, UnityEngine.Rendering.ColorWriteMask.Green);

                previewRect.x += widthOverFour + spacing;
                EditorGUI.DrawPreviewTexture(previewRect, texture.objectReferenceValue as Texture, null, ScaleMode.ScaleToFit, 0, -1, UnityEngine.Rendering.ColorWriteMask.Blue);

                previewRect.x += widthOverFour + spacing;   
                EditorGUI.DrawTextureAlpha(previewRect, texture.objectReferenceValue as Texture, ScaleMode.ScaleToFit, 0, -1);
            }

            GUILayout.Space(20);

            EditorGUILayout.HelpBox("Pay attention to the default values for new RevealData elements. The Add button will create a new element with defaults values that make sense.", MessageType.Info);
            if(GUILayout.Button("Add New Data"))
            {
                revealData.arraySize++;
                SetRevealDataDefaults(revealData.GetArrayElementAtIndex(revealData.arraySize - 1));
            }

            EditorGUILayout.PropertyField(revealData);

            serializedObject.ApplyModifiedProperties();
        }

        private void SetRevealDataDefaults(SerializedProperty element)
        {
            SerializedProperty blendOp = element.FindPropertyRelative("blendOp");
            blendOp.enumValueIndex = 0;

            SerializedProperty colorMask = element.FindPropertyRelative("colorMask");
            colorMask.enumValueIndex = 4;

            SerializedProperty deltaMultiplier = element.FindPropertyRelative("deltaMultiplier");
            deltaMultiplier.floatValue = 1f;
        }
    }
}

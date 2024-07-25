using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Reveal
{
    [CustomPropertyDrawer(typeof(RevealData))]
    public class RevealDataPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int numLines = 5;

            if(property.FindPropertyRelative("overTime").floatValue > 0) { numLines += 7; }

            if(property.FindPropertyRelative("swizzleAlphaRed").boolValue) { numLines += 1; }

            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * numLines;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            EditorGUI.indentLevel++;

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var blendOpRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var colorMaskRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var delayRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var overTimeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var deltaRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var widthRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var heightRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var formatRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var mipmapsRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var swizzleRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var overTimeSwizzleColorMaskRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(blendOpRect, property.FindPropertyRelative("blendOp"));
            EditorGUI.PropertyField(colorMaskRect, property.FindPropertyRelative("colorMask"));
            EditorGUI.PropertyField(delayRect, property.FindPropertyRelative("delay"));

            SerializedProperty overTime = property.FindPropertyRelative("overTime");
            EditorGUI.PropertyField(overTimeRect, overTime);

            if (overTime.floatValue > 0)
            {
                EditorGUI.LabelField(labelRect, "Over Time Settings");
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(deltaRect, property.FindPropertyRelative("deltaMultiplier"));
                EditorGUI.PropertyField(widthRect, property.FindPropertyRelative("widthMultiplier"));
                EditorGUI.PropertyField(heightRect, property.FindPropertyRelative("heightMultiplier"));
                EditorGUI.PropertyField(formatRect, property.FindPropertyRelative("overTimeRenderTextureFormat"));
                EditorGUI.PropertyField(mipmapsRect, property.FindPropertyRelative("generateMipMaps"));

                SerializedProperty swizzleAlphaRed = property.FindPropertyRelative("swizzleAlphaRed");
                EditorGUI.PropertyField(swizzleRect, swizzleAlphaRed);

                if (swizzleAlphaRed.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.PropertyField(overTimeSwizzleColorMaskRect, property.FindPropertyRelative("overTimeSwizzleColorMask"));
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }
    }
}

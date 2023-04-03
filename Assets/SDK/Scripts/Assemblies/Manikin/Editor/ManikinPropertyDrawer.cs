using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Manikin
{
    [CustomPropertyDrawer(typeof(ManikinProperty))]
    public class ManikinPropertyDrawer : PropertyDrawer
    {
        static string[] indicesDisplay = new string[31] { "0","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29","30" };

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty propertyType = property.FindPropertyRelative("propertyType");
            SerializedProperty values = property.FindPropertyRelative("values");
            SerializedProperty apply = property.FindPropertyRelative("apply");
            SerializedProperty indices = property.FindPropertyRelative("materialIndices");

            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var objRect = new Rect(position.x, position.y, position.width * 0.5f, EditorGUIUtility.singleLineHeight);
            var indicesRect = new Rect(position.x + 10 + (position.width * 0.5f), position.y + EditorGUIUtility.singleLineHeight, (position.width - 20) * 0.5f, EditorGUIUtility.singleLineHeight);
            var typeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width * 0.5f, EditorGUIUtility.singleLineHeight);
            var applyRect = new Rect(position.x + 10 + (position.width * 0.5f), position.y, position.width * 0.5f, EditorGUIUtility.singleLineHeight);
            var valuesRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, position.width, EditorGUIUtility.singleLineHeight);

            float quarterWidth = position.width * 0.25f;
            var values0Rect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 2, quarterWidth, EditorGUIUtility.singleLineHeight);
            var values1Rect = new Rect(position.x + quarterWidth, position.y + EditorGUIUtility.singleLineHeight * 2, quarterWidth, EditorGUIUtility.singleLineHeight);
            var values2Rect = new Rect(position.x + (quarterWidth * 2f), position.y + EditorGUIUtility.singleLineHeight * 2, quarterWidth, EditorGUIUtility.singleLineHeight);
            var values3Rect = new Rect(position.x + (quarterWidth * 3f), position.y + EditorGUIUtility.singleLineHeight * 2, quarterWidth, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(objRect, property.FindPropertyRelative("set"), GUIContent.none);
            if (apply.boolValue)
            {
                indices.intValue = EditorGUI.MaskField(indicesRect, "Mat Indices", indices.intValue, indicesDisplay);
            }
            EditorGUIUtility.labelWidth = 40;
            EditorGUI.PropertyField(typeRect, propertyType, new GUIContent("Type"));
            EditorGUI.PropertyField(applyRect, apply);
            EditorGUIUtility.labelWidth = 0; //reset to default

            switch ((ManikinProperty.PropertyType)propertyType.enumValueIndex)
            {
                case ManikinProperty.PropertyType.Float_Signed:
                    values.arraySize = 1;
                    SerializedProperty value0 = values.GetArrayElementAtIndex(0);
                    value0.floatValue = EditorGUI.Slider(valuesRect, value0.floatValue, -1f, 1f);
                    break;
                case ManikinProperty.PropertyType.Float_Unsigned:
                    values.arraySize = 1;
                    value0 = values.GetArrayElementAtIndex(0);
                    value0.floatValue = EditorGUI.Slider(valuesRect, value0.floatValue, 0f, 1f);
                    break;
                    
                case ManikinProperty.PropertyType.Float_Unclamped:
                    values.arraySize = 1;
                    value0 = values.GetArrayElementAtIndex(0);
                    value0.floatValue = EditorGUI.DelayedFloatField(valuesRect, value0.floatValue);
                    break;
                
                case ManikinProperty.PropertyType.Int:
                    values.arraySize = 1;
                    value0 = values.GetArrayElementAtIndex(0);
                    value0.floatValue = EditorGUI.DelayedIntField(valuesRect, (int)value0.floatValue);
                    break;
                
                case ManikinProperty.PropertyType.Float4:
                    values.arraySize = 4;
                    value0 = values.GetArrayElementAtIndex(0);
                    SerializedProperty value1 = values.GetArrayElementAtIndex(1);
                    SerializedProperty value2 = values.GetArrayElementAtIndex(2);
                    SerializedProperty value3 = values.GetArrayElementAtIndex(3);

                    EditorGUIUtility.labelWidth = 15;
                    value0.floatValue = EditorGUI.DelayedFloatField(values0Rect, "X:", value0.floatValue);
                    value1.floatValue = EditorGUI.DelayedFloatField(values1Rect, "Y:", value1.floatValue);
                    value2.floatValue = EditorGUI.DelayedFloatField(values2Rect, "Z:", value2.floatValue);
                    value3.floatValue = EditorGUI.DelayedFloatField(values3Rect, "W:", value3.floatValue);
                    EditorGUIUtility.labelWidth = 0;
                    break;

                case ManikinProperty.PropertyType.Color:
                case ManikinProperty.PropertyType.Color_HDR:
                    values.arraySize = 4;
                    value0 = values.GetArrayElementAtIndex(0);
                    value1 = values.GetArrayElementAtIndex(1);
                    value2 = values.GetArrayElementAtIndex(2);
                    value3 = values.GetArrayElementAtIndex(3);

                    EditorGUI.BeginChangeCheck();
                    Color color = new Color(value0.floatValue, value1.floatValue, value2.floatValue, value3.floatValue);
                    color = EditorGUI.ColorField(valuesRect, GUIContent.none, color, true, true, (ManikinProperty.PropertyType)propertyType.enumValueIndex == ManikinProperty.PropertyType.Color_HDR);
                    if (EditorGUI.EndChangeCheck())
                    {
                        value0.floatValue = color.r;
                        value1.floatValue = color.g;
                        value2.floatValue = color.b;
                        value3.floatValue = color.a;
                    }
                    break;
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}

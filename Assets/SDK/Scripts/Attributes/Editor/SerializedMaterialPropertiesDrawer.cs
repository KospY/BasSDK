#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializedMaterialProperties))]
public class SerializedMaterialPropertiesDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var propertyMatProperties = property.FindPropertyRelative("_properties");
        return 18f * propertyMatProperties.arraySize + 16.0f;
    }

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        Rect tempRect = new Rect(rect);
        tempRect.height = 16.0f;

        var propertyMat = property.FindPropertyRelative("_material");

        EditorGUI.BeginChangeCheck();
        propertyMat.objectReferenceValue = EditorGUI.ObjectField(tempRect, "Material", propertyMat.objectReferenceValue, typeof(Material), false);
        if (EditorGUI.EndChangeCheck())
        {
            Object targetObject = property.serializedObject.targetObject;
            object field = fieldInfo.GetValue(property.serializedObject.targetObject);

            System.Type type = typeof(SerializedMaterialProperties);
            MethodInfo UpdateMaterialPropertiesMethod = type.GetMethod("UpdateMaterialProperties");
            if (field is SerializedMaterialProperties serializedProperty)
            {
                property.serializedObject.ApplyModifiedProperties();
                serializedProperty.UpdateMaterialProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);
                AssetDatabase.SaveAssets();
                return;
            }
        }

        tempRect.y += tempRect.height + 2.0f;

        Rect rotatePropertyRect = new Rect(tempRect.x, tempRect.y, (tempRect.width * 3.0f/4.0f) - 1.0f, tempRect.height);
        Rect rotationModifierRect = new Rect(rotatePropertyRect.x + rotatePropertyRect.width + 2.0f, tempRect.y, (tempRect.width / 4.0f) - 1.0f, tempRect.height);

        GUIContent rotateLabel = new GUIContent("R", "is affected with rotation");

        var propertyMatProperties = property.FindPropertyRelative("_properties");
        int matPropertiesCount = propertyMatProperties.arraySize;

        for (int i = 0; i < matPropertiesCount; i++)
        {
            var propertyMatProperty = propertyMatProperties.GetArrayElementAtIndex(i);

            var propertyName = propertyMatProperty.FindPropertyRelative("_displayName");

            var propertyType = propertyMatProperty.FindPropertyRelative("_type");
            switch (propertyType.intValue)
            {
                case (int) SerializedMaterialProperties.SerializedMaterialProperty.Type.Color:
                    {
                        var propertyColor = propertyMatProperty.FindPropertyRelative("_colorValue");
                        propertyColor.colorValue = EditorGUI.ColorField(tempRect, propertyName.stringValue, propertyColor.colorValue);
                    }
                    break;

                case (int)SerializedMaterialProperties.SerializedMaterialProperty.Type.Vector:
                    {
                        var propertyVector = propertyMatProperty.FindPropertyRelative("_vectorValue");
                        propertyVector.vector4Value = EditorGUI.Vector4Field(tempRect, propertyName.stringValue, propertyVector.vector4Value);
                    }
                    break;

                case (int)SerializedMaterialProperties.SerializedMaterialProperty.Type.Float:
                    {
                        var propertyFloat = propertyMatProperty.FindPropertyRelative("_floatValue");
                        var propertyRotationModifier = propertyMatProperty.FindPropertyRelative("_rotationModifier");
                        var propertyIsRange = propertyMatProperty.FindPropertyRelative("_isRange");
                        if (propertyIsRange.boolValue)
                        {
                            var propertyRange = propertyMatProperty.FindPropertyRelative("_rangeLimits");
                            Vector2 rangeLimits = propertyRange.vector2Value;
                            propertyFloat.floatValue = EditorGUI.Slider(rotatePropertyRect, propertyName.stringValue, propertyFloat.floatValue, rangeLimits.x, rangeLimits.y);
                            SerializedMaterialProperties.SerializedMaterialProperty.RotationModifier enumValue = (SerializedMaterialProperties.SerializedMaterialProperty.RotationModifier) propertyRotationModifier.intValue;
                            propertyRotationModifier.intValue = (int)(SerializedMaterialProperties.SerializedMaterialProperty.RotationModifier)EditorGUI.EnumPopup(rotationModifierRect, enumValue);
                        }
                        else
                        {
                            propertyFloat.floatValue = EditorGUI.FloatField(rotatePropertyRect, propertyName.stringValue, propertyFloat.floatValue);
                            SerializedMaterialProperties.SerializedMaterialProperty.RotationModifier enumValue = (SerializedMaterialProperties.SerializedMaterialProperty.RotationModifier)propertyRotationModifier.intValue;
                            propertyRotationModifier.intValue = (int)(SerializedMaterialProperties.SerializedMaterialProperty.RotationModifier)EditorGUI.EnumPopup(rotationModifierRect, enumValue);
                        }
                    }
                    break;

                case (int)SerializedMaterialProperties.SerializedMaterialProperty.Type.Texture:
                    {
                        var propertyTexture = propertyMatProperty.FindPropertyRelative("_textureValue");
                        propertyTexture.objectReferenceValue = EditorGUI.ObjectField(tempRect, propertyName.stringValue, propertyTexture.objectReferenceValue, typeof(Texture), false);
                    }
                    break;

                case (int)SerializedMaterialProperties.SerializedMaterialProperty.Type.Int:
                    {
                        var propertyInt = propertyMatProperty.FindPropertyRelative("_intValue");
                        propertyInt.intValue = EditorGUI.IntField(tempRect, propertyName.stringValue, propertyInt.intValue);
                    }
                    break;

                default:
                    break;
            }

            tempRect.y += tempRect.height + 2.0f;
            rotatePropertyRect.y += rotatePropertyRect.height + 2.0f;
            rotationModifierRect.y += rotationModifierRect.height + 2.0f;
        }
    }
}

#endif
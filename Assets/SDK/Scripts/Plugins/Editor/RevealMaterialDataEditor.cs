using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Plugins
{
    [CustomEditor(typeof(RevealMaterialData))]
    public class RevealMaterialDataEditor : Editor
    {
        SerializedProperty shader;
        SerializedProperty revealColorProperties;
        SerializedProperty revealFloatProperties;
        SerializedProperty revealTextureProperties;
        SerializedProperty revealVector4Properties;

        string[] displayOptions;

        private void OnEnable()
        {
            shader = serializedObject.FindProperty("shader");
            revealColorProperties = serializedObject.FindProperty("revealColorProperties");
            revealFloatProperties = serializedObject.FindProperty("revealFloatProperties");
            revealTextureProperties = serializedObject.FindProperty("revealTextureProperties");
            revealVector4Properties = serializedObject.FindProperty("revealVector4Properties");

            displayOptions = GetAllShaderProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            RevealMaterialData data = target as RevealMaterialData;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(shader);
            if(EditorGUI.EndChangeCheck())
            {
                displayOptions = GetAllShaderProperties();
            }

            int selected = EditorGUILayout.Popup("Property to add", 0, displayOptions);
            if(selected > 0)
            {
                ShaderUtil.ShaderPropertyType propertyType = ShaderUtil.GetPropertyType(shader.objectReferenceValue as Shader, selected - 1);
                switch(propertyType)
                {
                    case ShaderUtil.ShaderPropertyType.Color:
                        TryAddProperty(revealColorProperties, displayOptions[selected]);
                        break;
                    case ShaderUtil.ShaderPropertyType.Float:
                    case ShaderUtil.ShaderPropertyType.Range:
                        TryAddProperty(revealFloatProperties, displayOptions[selected]);
                        break;
                    case ShaderUtil.ShaderPropertyType.TexEnv:
                        TryAddProperty(revealTextureProperties, displayOptions[selected]);
                        break;
                    case ShaderUtil.ShaderPropertyType.Vector:
                        TryAddProperty(revealVector4Properties, displayOptions[selected]);
                        break;
                }
            }

            if(GUILayout.Button("Copy Property Values from Material"))
            {
                EditorGUIUtility.ShowObjectPicker<Material>(null, false, "", EditorGUIUtility.GetControlID(FocusType.Passive));
            }

            if(Event.current.commandName == "ObjectSelectorClosed")
            {
                Material material = EditorGUIUtility.GetObjectPickerObject() as Material;
                if (material != null)
                {
                    data.GetPropertyValuesFromMaterial(material);
                }
            }

            EditorGUILayout.PropertyField(revealFloatProperties, true);
            EditorGUILayout.PropertyField(revealColorProperties, true);
            EditorGUILayout.PropertyField(revealVector4Properties, true);
            EditorGUILayout.PropertyField(revealTextureProperties, true);

            serializedObject.ApplyModifiedProperties();
        }

        void TryAddProperty(SerializedProperty serializedArray, string name)
        {
            if (!ContainsPropertyName(serializedArray, name))
            {
                serializedArray.arraySize++;
                serializedArray.GetArrayElementAtIndex(serializedArray.arraySize - 1).FindPropertyRelative("name").stringValue = name;
            }
            else
            {
                Debug.LogWarning("Already contains " + name);
            }
        }

        bool ContainsPropertyName(SerializedProperty serializedArray, string name)
        {
            for(int i = 0; i < serializedArray.arraySize; i++)
            {
                if (serializedArray.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue.Equals(name))
                    return true;
            }
            return false;
        }

        string[] GetAllShaderProperties()
        {
            if(shader.objectReferenceValue != null)
            {
                int propertyCount = ShaderUtil.GetPropertyCount(shader.objectReferenceValue as Shader);
                string[] options = new string[propertyCount + 1];
                options[0] = "Select Property to add.";

                for (int i = 0; i < propertyCount; i++)
                {
                    options[i+1] = ShaderUtil.GetPropertyName(shader.objectReferenceValue as Shader, i);
                }
                return options;
            }
            return new string[1] { "Select Property to add." };
        }
    }
}

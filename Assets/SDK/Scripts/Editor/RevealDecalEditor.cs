using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad
{
    [CustomEditor(typeof(RevealDecal))]
    public class RevealDecalEditor : Editor
    {
        readonly static string[] excludeProperties = new string[] { "textureProperties", "colorProperties", "floatProperties", "vectorProperties" };

        List<string> displayDesc = new List<string>();
        List<string> displayOptions = new List<string>();
        List<ShaderUtil.ShaderPropertyType> displayTypes = new List<ShaderUtil.ShaderPropertyType>();

        SerializedProperty textureProperties;
        SerializedProperty colorProperties;
        SerializedProperty floatProperties;
        SerializedProperty vectorProperties;

        private void OnEnable()
        {
            textureProperties = serializedObject.FindProperty("textureProperties");
            colorProperties = serializedObject.FindProperty("colorProperties");
            floatProperties = serializedObject.FindProperty("floatProperties");
            vectorProperties = serializedObject.FindProperty("vectorProperties");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Material[] materials = null;
            RevealDecal revealDecal = target as RevealDecal;
            if(revealDecal.TryGetComponent(out Renderer renderer))
            {
                materials = renderer.sharedMaterials;
            }
            else
            {
                EditorGUILayout.HelpBox("No renderer found!", MessageType.Error);
            }

            displayDesc.Clear();
            displayOptions.Clear();
            displayTypes.Clear();

            displayDesc.Add("Select to add properties");

            if(materials != null)
            {
                foreach (Material material in materials)
                {
                    if(material == null) { continue; }

                    Shader s = material.shader;
                    int propertyCount = ShaderUtil.GetPropertyCount(s);
                    for (int i = 0; i < propertyCount; i++)
                    {
                        string propertyName = ShaderUtil.GetPropertyName(s, i);
                        if (!displayOptions.Contains(propertyName))
                        {
                            displayOptions.Add(propertyName);
                            displayTypes.Add(ShaderUtil.GetPropertyType(s, i));
                            displayDesc.Add(ShaderUtil.GetPropertyDescription(s, i));
                        }
                    }
                }
            }

            DrawPropertiesExcluding(serializedObject, excludeProperties);

            int selectedIndex = EditorGUILayout.Popup(0, displayDesc.ToArray());
            if(selectedIndex > 0)
            {
                switch(displayTypes[selectedIndex - 1])
                {
                    case ShaderUtil.ShaderPropertyType.Color:
                        if(!revealDecal.colorProperties.Contains(displayOptions[selectedIndex-1]))
                        {
                            revealDecal.colorProperties.Add(displayOptions[selectedIndex-1]);
                        }
                        break;
                    case ShaderUtil.ShaderPropertyType.Float:
                    case ShaderUtil.ShaderPropertyType.Range:
                        if (!revealDecal.floatProperties.Contains(displayOptions[selectedIndex-1]))
                        {
                            revealDecal.floatProperties.Add(displayOptions[selectedIndex-1]);
                        }
                        break;
                    case ShaderUtil.ShaderPropertyType.TexEnv:
                        if (!revealDecal.textureProperties.Contains(displayOptions[selectedIndex-1]))
                        {
                            revealDecal.textureProperties.Add(displayOptions[selectedIndex-1]);
                        }
                        break;
                    case ShaderUtil.ShaderPropertyType.Vector:
                        if (!revealDecal.vectorProperties.Contains(displayOptions[selectedIndex-1]))
                        {
                            revealDecal.vectorProperties.Add(displayOptions[selectedIndex-1]);
                        }
                        break;
                }
            }

            EditorGUILayout.PropertyField(textureProperties, true);
            EditorGUILayout.PropertyField(colorProperties, true);
            EditorGUILayout.PropertyField(floatProperties, true);
            EditorGUILayout.PropertyField(vectorProperties, true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}

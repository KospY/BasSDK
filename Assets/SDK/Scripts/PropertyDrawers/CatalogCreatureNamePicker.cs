using System;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThunderRoad
{
    public class CatalogCreatureNamePicker : PropertyAttribute
    {
        public bool showPopup;
        public CatalogCreatureNamePicker()
        {
            if (Catalog.gameData != null)
            {
                showPopup = true;
            }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(CatalogCreatureNamePicker))]
    public class CatalogCreatureNamePickerDrawer : PropertyDrawer
    {    
        private string[] list;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return 0; } // Hacky, yet simple and effective way to prevent spacing

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                CatalogCreatureNamePicker catalogCreatureNamePicker = attribute as CatalogCreatureNamePicker;

                if (catalogCreatureNamePicker.showPopup)
                {
                    if (Catalog.gameData == null)
                    {
                        Catalog.LoadAllJson();
                    }
                    if (list == null)
                    {
                        List<string> selectList = new List<string>();
                        foreach (CreatureData creatureData in Catalog.GetDataList(Catalog.Category.Creature))
                        {
                            if (selectList.Exists(s => s == creatureData.name))
                            {
                                continue;
                            }
                            selectList.Add(creatureData.name);
                        }
                        list = selectList.ToArray();
                    }
                }

                EditorGUILayout.BeginHorizontal();

                if (catalogCreatureNamePicker.showPopup && list.Length > 0)
                {
                    int index = Mathf.Max(0, Array.IndexOf(list, property.stringValue));
                    index = EditorGUILayout.Popup(property.displayName, index, list);
                    property.stringValue = list[index];
                }
                else
                {
                    property.stringValue = EditorGUILayout.TextField(property.displayName, property.stringValue);
                }

                catalogCreatureNamePicker.showPopup = GUILayout.Toggle(catalogCreatureNamePicker.showPopup, "Catalog Picker", "Button", GUILayout.Width(110));

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                base.OnGUI(position, property, label);
            }
        }
    }
#endif
}
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThunderRoad
{
    public class CatalogPicker : PropertyAttribute
    {
        public bool showPopup;
        public Catalog.Category category;

        public CatalogPicker(Catalog.Category category)
        {
            this.category = category;
            if (Catalog.gameData != null)
            {
                showPopup = true;
            }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(CatalogPicker))]
    public class CatalogPickerDrawer : PropertyDrawer
    {
        private string[] list;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return 0; } // Hacky, yet simple and effective way to prevent spacing

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                CatalogPicker catalogPicker = attribute as CatalogPicker;

                if (catalogPicker.showPopup)
                {
                    if (Catalog.gameData == null)
                    {
                        Catalog.LoadAllJson();
                    }

                    var allId = Catalog.GetAllID(catalogPicker.category);
                    list = new string[allId.Count + 1];
                    list[0] = "None";
                    allId.CopyTo(list, 1);
                }

                EditorGUILayout.BeginHorizontal();

                if (catalogPicker.showPopup && list.Length > 0)
                {
                    int index = Mathf.Max(0, Array.IndexOf(list, property.stringValue));
                    index = EditorGUILayout.Popup(property.displayName, index, list);
                    property.stringValue = list[index] == "None" ? "" : list[index];
                }
                else
                {
                    property.stringValue = EditorGUILayout.TextField(property.displayName, property.stringValue);
                }

                catalogPicker.showPopup = GUILayout.Toggle(catalogPicker.showPopup, "Catalog Picker", "Button", GUILayout.Width(110));

                EditorGUILayout.EndHorizontal();


                /* Alternative that work in default Unity lists
                CatalogPicker catalogPicker = attribute as CatalogPicker;

                if (catalogPicker.showPopup)
                {
                    if (Catalog.gameData == null)
                    {
                        Catalog.LoadAllJson();
                    }

                    var allId = Catalog.GetAllID(catalogPicker.category);
                    list = new string[allId.Count + 1];
                    list[0] = "None";
                    allId.CopyTo(list, 1);
                }

                var rect = EditorGUI.PrefixLabel(position, label);

                var leftRect = new Rect(rect.x, rect.y, rect.width - buttonWidth - 5, rect.height);
                var rightRect = new Rect(rect.x + rect.width - buttonWidth, rect.y, buttonWidth - 2, rect.height);

                if (catalogPicker.showPopup && list.Length > 0)
                {
                    int index = Mathf.Max(0, Array.IndexOf(list, property.stringValue));
                    index = EditorGUI.Popup(leftRect, index, list);
                    property.stringValue = list[index] == "None" ? "" : list[index];
                }
                else
                {
                    property.stringValue = EditorGUI.TextField(leftRect, property.stringValue);
                }

                catalogPicker.showPopup = EditorGUI.Toggle(rightRect, catalogPicker.showPopup, "Button");
                EditorGUI.LabelField(rightRect, "Toggle picker");
                */
            }
            else
            {
                base.OnGUI(position, property, label);
            }
        }
    }
#endif
}
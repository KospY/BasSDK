using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

// This class just slightly modifies the default list drawer by adding a dropdown whenever
// the add button is pressed that displays all the subclasses of the list's class
namespace ThunderRoad
{
    public class SubclassListDrawer 
    {
        private static readonly Rect infinityRect = new(float.NegativeInfinity, float.NegativeInfinity, float.PositiveInfinity, float.PositiveInfinity);
        private static readonly Dictionary<string, SubclassListDrawer> drawerCache = new();

        private readonly object list;
        private readonly MethodInfo drawMethod;

        public static SubclassListDrawer GetDrawer(SerializedProperty property)
        {
            string key = property.propertyPath + property.serializedObject.targetObject.GetInstanceID().ToString();
            if (drawerCache.TryGetValue(key, out SubclassListDrawer drawer))
                return drawer;
            drawerCache[key] = new SubclassListDrawer(property);
            return drawerCache[key];
        }

        public SubclassListDrawer(SerializedProperty prop)
        {
            prop = prop.Copy();

            // Again, I have NO idea why this is internal. It's super useful to just change one or 2 things about the
            // default editor, but instead u have to do a ton of stuff to do something simple
            Type reorderableListWrapperType = typeof(ReorderableList).Assembly.GetType("UnityEditorInternal.ReorderableListWrapper");
            drawMethod = reorderableListWrapperType.GetMethod("Draw");
            list = Activator.CreateInstance(reorderableListWrapperType, prop, null, true);
            object reorderableList = reorderableListWrapperType.GetField("m_ReorderableList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(list);

            // Grab the true type of the list and the FieldInfo for the list
            // Get the fieldInfo for the list by tracing the path of the property
            FieldInfo arrayField = null;
            object subclassObj = null;
            object nextObj = prop.serializedObject.targetObject;

            string[] paths = prop.propertyPath.Replace("Array.data[", "[").Split('.');
            foreach (string path in paths)
            {
                // This is to deal with arrays in the property path
                if (path[0] == '[')
                {
                    int index = int.Parse(path[1..^1]);
                    nextObj = ((IList)nextObj)[index];
                    continue;
                }
                arrayField = nextObj.GetType().GetField(path, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                subclassObj = nextObj;
                nextObj = arrayField.GetValue(nextObj);
            }

            // Supporting any type of arrays will be pain, so just no support for them
            if (arrayField.FieldType.GenericTypeArguments.Length == 0)
                return;
            Type elementType = arrayField.FieldType.GenericTypeArguments[0];

            // Check if any rows in the list also need a subclass drawer
            InitNestedLists(prop, reorderableList);

            // Add the dropdown to the add button
            InitDropDown(prop, arrayField, subclassObj, elementType, reorderableList);
        }

        private void InitNestedLists(SerializedProperty prop, object reorderableList) 
        {
            ReorderableList.ElementCallbackDelegate drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty currProp = prop.GetArrayElementAtIndex(index);
                // Draw top label
                GUIContent label = currProp.propertyType == SerializedPropertyType.ManagedReference
                    ? new($"Element {index} ({currProp.managedReferenceValue?.GetType().Name})")
                    : new("Element " + index);
                rect.height = GUI.skin.label.CalcHeight(label, rect.width);

                if (!currProp.hasVisibleChildren)
                {
                    EditorGUI.PropertyField(rect, currProp, label);
                    return;
                }

                currProp.isExpanded = EditorGUI.Foldout(rect, currProp.isExpanded, label, true);
                if (currProp.isExpanded)
                {
                    rect.y += rect.height + 2;
                    rect.x += EditorStyles.foldout.padding.left;
                    rect.width -= EditorStyles.foldout.padding.left;
                    // Draw each child property
                    SerializedProperty endProp = currProp.GetEndProperty(true);
                    if (!currProp.NextVisible(true))
                        return;

                    do
                    {
                        rect.height = EditorGUI.GetPropertyHeight(currProp);
                        if (currProp.isArray && currProp.propertyType == SerializedPropertyType.Generic)
                            GetDrawer(currProp).Draw(rect, null);
                        else
                            EditorGUI.PropertyField(rect, currProp, null, true);
                        rect.y += rect.height + 2;
                    }
                    while (currProp.NextVisible(false) && !SerializedProperty.EqualContents(currProp, endProp));
                }
            };

            FieldInfo drawElementCallbackField = typeof(ReorderableList).GetField("drawElementCallback");
            drawElementCallbackField.SetValue(reorderableList, Delegate.Combine((Delegate)drawElementCallbackField.GetValue(reorderableList), drawElementCallback));
        }

        private void InitDropDown(SerializedProperty prop, FieldInfo arrayField, object subclassObj, Type elementType, object reorderableList)
        {
            // Check if any valid subclasses exist. If not, then dont add a dropdown callback
            bool anyValid = false;
            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes()))
                if (type.IsSubclassOf(elementType) && !type.IsInterface && !type.IsAbstract)
                    anyValid = true;
            if (!anyValid)
                return;

            void AddItem(object newItem)
            {
                IList fieldList = (IList)arrayField.GetValue(subclassObj);
                if (fieldList == null)
                {
                    fieldList = (IList)Activator.CreateInstance(arrayField.FieldType);
                    arrayField.SetValue(subclassObj, fieldList);
                }

                prop.arraySize = fieldList.Count + 1;

                fieldList.Add(newItem);
            }

            ReorderableList.AddDropdownCallbackDelegate dropDownCallback = (Rect buttonRect, ReorderableList originalList) =>
            {
                GenericMenu menu = new();
                if (!elementType.IsInterface && !elementType.IsAbstract)
                    menu.AddItem(new GUIContent(elementType.Name), false, () => AddItem(Activator.CreateInstance(elementType)));

                foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes()))
                {
                    if (type.IsSubclassOf(elementType) && !type.IsInterface && !type.IsAbstract)
                    {
                        Type tempType = type;
                        menu.AddItem(new GUIContent(type.Name), false, () => AddItem(Activator.CreateInstance(tempType)));
                    }
                }
                menu.DropDown(buttonRect);
            };

            FieldInfo dropDownCallbackField = typeof(ReorderableList).GetField("onAddDropdownCallback");
            dropDownCallbackField.SetValue(reorderableList, Delegate.Combine((Delegate)dropDownCallbackField.GetValue(reorderableList), dropDownCallback));
        }

        public void Draw(Rect position, GUIContent label)
            => drawMethod.Invoke(list, new object[] { label, position, infinityRect, label?.tooltip, true });
    }
}

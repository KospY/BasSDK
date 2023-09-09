using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

// This class just slightly modifies the default list drawer by adding a dropdown whenever
// the add button is pressed that displays all the subclasses of the list's class
namespace ThunderRoad
{
    public class SubclassListDrawer 
    {
        private readonly object list;
        private readonly MethodInfo drawMethod;

        private static readonly Rect infinityRect = new(float.NegativeInfinity, float.NegativeInfinity, float.PositiveInfinity, float.PositiveInfinity);

        public SubclassListDrawer(SerializedProperty prop)
        {
            // Again, I have NO idea why this is internal. It's super useful to just change one or 2 things about the
            // default editor, but instead u have to do a ton of stuff to do something simple
            Type reorderableListWrapperType = typeof(ReorderableList).Assembly.GetType("UnityEditorInternal.ReorderableListWrapper");
            drawMethod = reorderableListWrapperType.GetMethod("Draw");
            list = Activator.CreateInstance(reorderableListWrapperType, prop, null, true);
            object reorderableList = reorderableListWrapperType.GetField("m_ReorderableList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(list);

            // Grab the true type of the list and the FieldInfo for the list
            // Get the fieldInfo for the list by tracing the path of the property
            FieldInfo arrayField = null;
            object nextObj = prop.serializedObject.targetObject;

            string[] pathes = prop.propertyPath.Split('.');
            foreach (string path in pathes)
            {
                arrayField = nextObj.GetType().GetField(path, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                nextObj = arrayField.GetValue(nextObj);
            }
            Type elementType = arrayField.FieldType.GenericTypeArguments[0];

            void AddItem(object newItem)
            {
                prop.managedReferenceValue ??= Activator.CreateInstance(arrayField.FieldType);
                IList fieldList = (IList)prop.managedReferenceValue;

                prop.arraySize = fieldList.Count + 1;
                //list.index = elements.arraySize - 1;

                fieldList.Add(newItem);
            }

            ReorderableList.AddDropdownCallbackDelegate dropDownCallback = (Rect buttonRect, ReorderableList originalList) =>
            {
                GenericMenu menu = new();
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
        {
            drawMethod.Invoke(list, new object[] { label, position, infinityRect, label.tooltip, true });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ThunderRoad.Utilities
{
    [CustomEditor(typeof(ValueModifierSequence))]
    public class ValueModifierSequenceEditor : Editor
    {
        private static readonly string[] propertyToExclude = {"m_Script", "modifiers"};
        private ValueModifierSequence valueModifierSequence;

        private ReorderableList modifiersList;
        private SerializedProperty modifiers;
        private ValueModifierBaseEditor valueModifiers;
        GenericMenu contextMenuAdd;

        public static Color floatColor = new Color(.2f, .6f, 1f);
        public static Color integerColor = new Color(.6f, .2f, 1f);
        public static Color booleanColor = new Color(.2f, 1f, .8f);
        public static Color vector2Color = new Color(1f, 1f, .2f);
        public static Color vector3Color = new Color(1f, .7f, .4f);
        public static Color colorColor = new Color(1f, .2f, .4f);

        private static readonly Dictionary<ValueModifierSequence.ValueModifierType, string> colors
            = new Dictionary<ValueModifierSequence.ValueModifierType, string>
            {
                {ValueModifierSequence.ValueModifierType.Float, ColorUtility.ToHtmlStringRGB(floatColor)},
                {ValueModifierSequence.ValueModifierType.Integer, ColorUtility.ToHtmlStringRGB(integerColor)},
                {ValueModifierSequence.ValueModifierType.Boolean, ColorUtility.ToHtmlStringRGB(booleanColor)},
                {ValueModifierSequence.ValueModifierType.Vector2, ColorUtility.ToHtmlStringRGB(vector2Color)},
                {ValueModifierSequence.ValueModifierType.Vector3, ColorUtility.ToHtmlStringRGB(vector3Color)},
                {ValueModifierSequence.ValueModifierType.Color, ColorUtility.ToHtmlStringRGB(colorColor)} 
            };

        public static IEnumerable<object> FindConcreteSubclasses(Type objectType = null)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(objectType))
                .Select(CreateInstance);
        }

        public void BuildMenu()
        {
            contextMenuAdd = new GenericMenu();
            foreach (var obj in FindConcreteSubclasses(typeof(ValueModifierBase)).ToArray())
            {
                name = (obj as ValueModifierBase)?.MenuPath;

                contextMenuAdd.AddItem(new GUIContent(name), false, AddModifier, obj);
            }
        }

        public void AddModifier(object o)
        {
            var t = o.GetType();
            var obj = valueModifierSequence.gameObject;
            var c = Undo.AddComponent(obj, t);
            (c as ValueModifierBase).order = modifiersList.count;

            serializedObject.ApplyModifiedProperties();
            valueModifierSequence.Refresh();
            Repaint();
        }

        public void RemoveElement(ReorderableList list)
        {
            int index = modifiersList.index;
            var element = modifiers.GetArrayElementAtIndex(index).objectReferenceValue;
            if (element != null)
                Undo.DestroyObjectImmediate(element);
            else
                Undo.RecordObject(serializedObject.targetObject, "Remove null entry");

            modifiers.DeleteArrayElementAtIndex(index); // Remove list entry
            UpdateSelection(-1);
        }

        private void OnEnable()
        {
            BuildMenu();
            valueModifierSequence = target as ValueModifierSequence;
            modifiers = serializedObject.FindProperty("modifiers");

            modifiersList = new ReorderableList(serializedObject, modifiers, true, true, true, true)
            {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawElement,
                onRemoveCallback = RemoveElement,
                onAddCallback = AddElement,
                onSelectCallback = SelectElement,
                onReorderCallbackWithDetails = ReorderElement
            };
        }

        private void ReorderElement(ReorderableList list, int oldIndex, int newIndex)
        {
            valueModifierSequence.modifiers[newIndex].order = oldIndex;
            valueModifierSequence.modifiers[oldIndex].order = newIndex;

            serializedObject.ApplyModifiedProperties();
            valueModifierSequence.Refresh();
            Repaint();
        }

        public void DrawHeader(Rect rect)
        {
            GUI.Label(rect, "Value modifiers");
        }

        public void AddElement(ReorderableList list)
        {
            contextMenuAdd.ShowAsContext();
        }

        public void SelectElement(ReorderableList list)
        {
            UpdateSelection(list.index);
        }

        public void UpdateSelection(int selected)
        {
            if (selected >= 0)
            {
                var editor = CreateEditor(modifiers.GetArrayElementAtIndex(selected).objectReferenceValue);
                valueModifiers = editor as ValueModifierBaseEditor;
            }
            else
                valueModifiers = null;
        }

        public void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var target = modifiers.GetArrayElementAtIndex(index).objectReferenceValue as ValueModifierBase;
            if (!target) return;

            var label =
                $"<b><color=#{colors[target.inputType]}>{target.inputType} → </color></b> {string.Join(" ", Regex.Split(target.MenuName, @"(?<!^)(?=[A-Z,0-9])"))} <b><color=#{colors[target.outputType]}>→ {target.outputType}</color></b>";
            GUI.Label(rect, label,
                new GUIStyle(EditorStyles.label) {padding = new RectOffset(20, 0, 2, 0), richText = true});
        }

        public override bool RequiresConstantRepaint()
        {
            return Application.isPlaying;
        }

        public override void OnInspectorGUI()
        {
            valueModifierSequence = target as ValueModifierSequence;

            serializedObject.Update();

            modifiersList?.DoLayoutList();
            if (valueModifiers) valueModifiers.OnInspectorGUI();
            DrawPropertiesExcluding(serializedObject, propertyToExclude);

            serializedObject.ApplyModifiedProperties();

            if (!valueModifierSequence) return;
            if (valueModifierSequence.lastOutput == null) return;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Output: ");
            EditorGUILayout.TextArea(valueModifierSequence.lastOutput.ToString());
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("value history: ");
            for (var i = 0; i < valueModifierSequence.lastOutput.valueHistory.Count; i++)
            {
                var value = valueModifierSequence.lastOutput.valueHistory[i];
                EditorGUILayout.LabelField(value.ToString());
                EditorGUILayout.LabelField("↓");
            }

            EditorGUILayout.Space();
        }
    }
}
using UnityEditor;
using UnityEditor.UI;

namespace ThunderRoad
{
    [CustomEditor(typeof(UIDropdown))]
    public class UIDropdownEditor : DropdownEditor
    {
        SerializedProperty title;
        SerializedProperty arrowImage;
        SerializedProperty itemButton;
        SerializedProperty updateTitlePosition;
        SerializedProperty openedTitlePosition;
        SerializedProperty closedTitlePosition;
        SerializedProperty OnDropdownShow;
        SerializedProperty OnDropdownHide;

        protected new void OnEnable()
        {
            base.OnEnable();

            title = serializedObject.FindProperty("title");
            arrowImage = serializedObject.FindProperty("arrowImage");
            itemButton = serializedObject.FindProperty("itemButton");
            updateTitlePosition = serializedObject.FindProperty("updateTitlePosition");
            openedTitlePosition = serializedObject.FindProperty("openedTitlePosition");
            closedTitlePosition = serializedObject.FindProperty("closedTitlePosition");
            OnDropdownShow = serializedObject.FindProperty("OnDropdownShow");
            OnDropdownHide = serializedObject.FindProperty("OnDropdownHide");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(title);
            EditorGUILayout.PropertyField(arrowImage);
            EditorGUILayout.PropertyField(itemButton);
            EditorGUILayout.PropertyField(updateTitlePosition);
            EditorGUILayout.PropertyField(openedTitlePosition);
            EditorGUILayout.PropertyField(closedTitlePosition);
            EditorGUILayout.PropertyField(OnDropdownShow);
            EditorGUILayout.PropertyField(OnDropdownHide);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
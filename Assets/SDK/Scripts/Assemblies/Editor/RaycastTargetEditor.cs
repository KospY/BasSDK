namespace ThunderRoad
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.UI;
 
    [CanEditMultipleObjects, CustomEditor(typeof(RaycastTarget), false)]
    public class RaycastTargetEditor : GraphicEditor
    {
        public override void OnInspectorGUI ()
        {
            base.serializedObject.Update();
            EditorGUILayout.PropertyField(base.m_Script, new GUILayoutOption[0]);
            // skipping AppearanceControlsGUI
            base.RaycastControlsGUI();
            base.serializedObject.ApplyModifiedProperties();
        }
    }
    
}

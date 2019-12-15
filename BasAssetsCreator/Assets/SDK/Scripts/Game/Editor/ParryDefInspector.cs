using System;
using UnityEngine;
using UnityEditor;

namespace BS
{
    [CustomEditor(typeof(ParryTargetDefinition))]
    public class ParryDefInspector : Editor
    {
        bool toolsHidden = false;
        Tool previousTool;
        Vector3 HandlePoint1;
        Vector3 HandlePoint2;
        bool centerTransform = true;

        public override void OnInspectorGUI()
        {
            ParryTargetDefinition parry = (ParryTargetDefinition)target;
            if (centerTransform)
            {
                if (GUILayout.Button("Enable Point to Point transforms"))
                {
                    centerTransform = false;
                    //parry.transform.hideFlags = HideFlags.NotEditable;
                }
            }
            else
            {
                if (GUILayout.Button("Enable Center Transform"))
                {
                    centerTransform = true;
                    //parry.transform.hideFlags = 0;
                }
            }
            base.OnInspectorGUI();

            if (!centerTransform)
            {
                HandlePoint1 = parry.GetLineStart();
                HandlePoint2 = parry.GetLineEnd();
            }

        }

        private void OnEnable()
        {
            ParryTargetDefinition parry = (ParryTargetDefinition)target;
            HandlePoint1 = parry.GetLineStart();
            HandlePoint2 = parry.GetLineEnd();
            //parry.transform.hideFlags = HideFlags.NotEditable;
        }
        private void OnSceneGUI()
        {
            ParryTargetDefinition parry = (ParryTargetDefinition)target;

            if (parry.length > 0 && !centerTransform)
            {
                if (Tools.current != Tool.None)
                {
                    previousTool = Tools.current;
                    Tools.current = Tool.None;
                    toolsHidden = true;
                }
                Undo.RecordObject(this, "Moved Handle");
                HandlePoint1 = Handles.DoPositionHandle(HandlePoint1, Quaternion.LookRotation(Vector3.forward, Vector3.up));
                HandlePoint2 = Handles.DoPositionHandle(HandlePoint2, Quaternion.LookRotation(Vector3.forward, Vector3.up));
                if (EditorWindow.focusedWindow && EditorWindow.focusedWindow.ToString() == " (UnityEditor.SceneView)")
                {
                    parry.transform.position = Vector3.Lerp(HandlePoint1, HandlePoint2, 0.5f);
                    parry.length = (HandlePoint1 - HandlePoint2).magnitude / 2;
                    parry.transform.rotation = Quaternion.LookRotation(HandlePoint2 - HandlePoint1, Vector3.forward) * Quaternion.AngleAxis(-90, Vector3.right);
                }
            }
            else if (toolsHidden)
            {
                Tools.current = previousTool;
                toolsHidden = false;
            }
        }

        private void OnDisable()
        {
            if (target) EditorUtility.SetDirty(target);
            if (toolsHidden)
            {
                Tools.current = previousTool;
                toolsHidden = false;
            }
        }
    }
}
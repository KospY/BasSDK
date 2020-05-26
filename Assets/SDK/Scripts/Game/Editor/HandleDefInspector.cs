using System;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad
{
    [CustomEditor(typeof(HandleDefinition))]
    public class HandleDefInspector : Editor
    {
        bool toolsHidden = false;
        Tool previousTool;
        Vector3 HandlePoint1;
        Vector3 HandlePoint2;
        Quaternion axis;
        bool centerTransform = true;

        public override void OnInspectorGUI()
        {

            HandleDefinition handle = (HandleDefinition)target;
            ItemDefinition item = handle.transform.GetComponentInParent<ItemDefinition>();

            if (centerTransform)
            {
                if (GUILayout.Button("Enable Point to Point transforms"))
                {
                    centerTransform = false;
                    handle.transform.hideFlags = HideFlags.NotEditable;
                }
            }
            else
            {
                if (GUILayout.Button("Enable Center Transform"))
                {
                    centerTransform = true;
                    handle.transform.hideFlags = 0;
                }
            }

            base.OnInspectorGUI();

            if (GUILayout.Button("Calculate Reach") && item)
            {
                handle.CalculateReach();
            }

            if (handle.transform.localScale != Vector3.one)
            {
                EditorGUILayout.HelpBox("Handle object scale must be set to 1.", MessageType.Error);
            }

            if (handle.axisLength < 0)
            {
                EditorGUILayout.HelpBox("Handle axis length must be a positive number or zero.", MessageType.Error);
            }

            if (handle.touchRadius <= 0)
            {
                EditorGUILayout.HelpBox("Handle touch radius must be a positive number.", MessageType.Error);
            }

            if (handle.reach <= 0)
            {
                EditorGUILayout.HelpBox("Handle reach must be a positive number.", MessageType.Error);
            }

            if (handle.slideToHandleOffset <= 0)
            {
                EditorGUILayout.HelpBox("Slide to handle offset must be a positive number.", MessageType.Error);
            }

            foreach (HandleDefinition.Orientation orientations in handle.allowedOrientations)
            {
                for (int i = 0; i < handle.allowedOrientations.Count; i++)
                {
                    if (handle.allowedOrientations.IndexOf(orientations) < i && handle.allowedOrientations[i].rotation == handle.allowedOrientations[handle.allowedOrientations.IndexOf(orientations)].rotation && handle.allowedOrientations[i].allowedHand == handle.allowedOrientations[handle.allowedOrientations.IndexOf(orientations)].allowedHand && handle.allowedOrientations[i].isDefault == handle.allowedOrientations[handle.allowedOrientations.IndexOf(orientations)].isDefault)
                    {
                        EditorGUILayout.HelpBox("Allowed orientations " + handle.allowedOrientations.IndexOf(orientations) + " and " + i + " are equal.", MessageType.Warning);
                    }
                }
            }

            for (int i = 0; i < handle.allowedOrientations.Count; i++)
            {
                if (handle.allowedOrientations[i].allowedHand == HandleDefinition.HandSide.Left && (handle.allowedOrientations[i].isDefault == HandleDefinition.HandSide.Right || handle.allowedOrientations[i].isDefault == HandleDefinition.HandSide.Both))
                {
                    EditorGUILayout.HelpBox("Handle orientation " + i + " must have 'Is Default' set to None or Left if 'Allowed Hand' is set to Left.", MessageType.Warning);
                }
                if (handle.allowedOrientations[i].allowedHand == HandleDefinition.HandSide.Right && (handle.allowedOrientations[i].isDefault == HandleDefinition.HandSide.Left || handle.allowedOrientations[i].isDefault == HandleDefinition.HandSide.Both))
                {
                    EditorGUILayout.HelpBox("Handle orientation " + i + " must have 'Is Default' set to None or Right if 'Allowed Hand' is set to Right.", MessageType.Warning);
                }
            }
        }

        private void OnEnable()
        {
            HandleDefinition handle = (HandleDefinition)target;

            HandlePoint1 = handle.transform.position + (handle.transform.up * (handle.axisLength * 0.5f));
            HandlePoint2 = handle.transform.position + (handle.transform.up * -(handle.axisLength * 0.5f));

            handle.transform.hideFlags = HideFlags.NotEditable;
        }
        private void OnSceneGUI()
        {
            HandleDefinition handle = (HandleDefinition)target;

            if (handle.axisLength > 0 && !centerTransform)
            {
                if (Tools.current != Tool.None)
                {
                    previousTool = Tools.current;
                    Tools.current = Tool.None;
                    toolsHidden = true;
                }
                HandlePoint1 = Handles.DoPositionHandle(HandlePoint1, Quaternion.LookRotation(handle.transform.forward, handle.transform.up));
                HandlePoint2 = Handles.DoPositionHandle(HandlePoint2, Quaternion.LookRotation(handle.transform.forward, handle.transform.up));
                Handles.color = Color.green;
                try
                {
                    if (EditorWindow.mouseOverWindow && EditorWindow.mouseOverWindow.ToString() == " (UnityEditor.SceneView)")
                    {
                        handle.transform.position = Vector3.Lerp(HandlePoint1, HandlePoint2, 0.5f);
                        handle.axisLength = (HandlePoint1 - HandlePoint2).magnitude;

                        if (Event.current.control)
                        {
                            axis = Handles.Disc(handle.transform.rotation, handle.transform.position, (HandlePoint1 - HandlePoint2), HandleUtility.GetHandleSize(handle.transform.position), false, 15f);
                        }
                        else
                        {
                            axis = Handles.Disc(handle.transform.rotation, handle.transform.position, (HandlePoint1 - HandlePoint2), HandleUtility.GetHandleSize(handle.transform.position), false, 0.1f);
                        }
                        handle.transform.rotation = Quaternion.LookRotation(HandlePoint2 - HandlePoint1, axis * Vector3.forward) * Quaternion.AngleAxis(-90, Vector3.right);
                    }
                    else
                    {
                        axis = Handles.Disc(handle.transform.rotation, handle.transform.position, (HandlePoint1 - HandlePoint2), HandleUtility.GetHandleSize(handle.transform.position), false, 0.1f);
                    }
                }
                catch (Exception) { }
            }
            else if (toolsHidden)
            {
                Tools.current = previousTool;
                toolsHidden = false;
            }
        }

        private void OnDisable()
        {
            EditorUtility.SetDirty(target);
            if (toolsHidden)
            {
                Tools.current = previousTool;
                toolsHidden = false;
            }
        }
    }
}
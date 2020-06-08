using System;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad
{
    [CustomEditor(typeof(HandleDefinition))]
    public class HandleDefInspector : PointToPoint
    {
        HandleDefinition handle;

        public override void OnInspectorGUI()
        {
            script = handle = (HandleDefinition)target;
            ItemDefinition item = handle.transform.GetComponentInParent<ItemDefinition>();

            PointToPointButton(handle.axisLength);

            base.OnInspectorGUI();

            if (GUILayout.Button("Calculate Reach") && item)
            {
                handle.CalculateReach();
            }

            if (GUILayout.Button("Update to new orientations") && item)
            {
                handle.CheckOrientations();
            }

            if (handle.allowedOrientations.Count > 0)
            {
                EditorGUILayout.HelpBox("The allowed orientations list is obsolete, Use child HandleOrientation instead.", MessageType.Warning);
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
        }

        private void OnEnable()
        {
            script = handle = (HandleDefinition)target;
            ResetPoints(handle.axisLength);
        }
        private void OnSceneGUI()
        {
            script = handle = (HandleDefinition)target;
            UpdatePoints(ref handle.axisLength);
        }
    }
}
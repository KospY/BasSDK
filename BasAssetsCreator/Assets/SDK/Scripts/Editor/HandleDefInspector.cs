using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(BS.HandleDefinition))]
public class HandleDefInspector : Editor
{

    public override void OnInspectorGUI()
    {

        BS.HandleDefinition handle = (BS.HandleDefinition)target;
        BS.ItemDefinition item = handle.transform.GetComponentInParent<BS.ItemDefinition>();

        if (showCenterTransform)
        {
            if (GUILayout.Button("Enable Point to Point transforms"))
            {
                showCenterTransform = false;
            }
        }
        else
        {
            if (GUILayout.Button("Enable Center Transform"))
            {
                showCenterTransform = true;
            }
        }

        base.OnInspectorGUI();

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

        foreach (BS.HandleDefinition.Orientation orientations in handle.allowedOrientations)
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
            if (handle.allowedOrientations[i].allowedHand == BS.HandleDefinition.HandSide.Left && (handle.allowedOrientations[i].isDefault == BS.HandleDefinition.HandSide.Right || handle.allowedOrientations[i].isDefault == BS.HandleDefinition.HandSide.Both))
            {
                EditorGUILayout.HelpBox("Handle orientation " + i + " must have 'Is Default' set to None or Left if 'Allowed Hand' is set to Left.", MessageType.Warning);
            }
            if (handle.allowedOrientations[i].allowedHand == BS.HandleDefinition.HandSide.Right && (handle.allowedOrientations[i].isDefault == BS.HandleDefinition.HandSide.Left || handle.allowedOrientations[i].isDefault == BS.HandleDefinition.HandSide.Both))
            {
                EditorGUILayout.HelpBox("Handle orientation " + i + " must have 'Is Default' set to None or Right if 'Allowed Hand' is set to Right.", MessageType.Warning);
            }
        }

        if (GUILayout.Button("Calculate Reach") && item)
        {
            handle.CalculateReach();
        }

        //POINT TO POINT HANDLE TRANSFORM
        if (GUI.changed || handle.transform.hasChanged || Event.current.type == EventType.MouseDown)
        {
            HandlePoint1 = handle.transform.rotation * new Vector3(0, handle.axisLength / 2, 0) + handle.transform.position;
            HandlePoint2 = handle.transform.rotation * new Vector3(0, -handle.axisLength / 2, 0) + handle.transform.position;
        }
    }

    bool toolsHidden = false;
    Tool previousTool;
    Vector3 HandlePoint1;
    Vector3 HandlePoint2;
    bool showCenterTransform = false;

    private void OnEnable()
    {
        BS.HandleDefinition handle = (BS.HandleDefinition)target;

        HandlePoint1 = handle.transform.rotation * new Vector3(0, handle.axisLength / 2, 0) + handle.transform.position;
        HandlePoint2 = handle.transform.rotation * new Vector3(0, -handle.axisLength / 2, 0) + handle.transform.position;
    }
    private void OnSceneGUI()
    {
        BS.HandleDefinition handle = (BS.HandleDefinition)target;

        if (handle.axisLength > 0 && !showCenterTransform)
        {
            if (Tools.current != Tool.None)
            {
                previousTool = Tools.current;
                Tools.current = Tool.None;
                toolsHidden = true;
            }
            HandlePoint1 = Handles.DoPositionHandle(HandlePoint1, Quaternion.identity);
            HandlePoint2 = Handles.DoPositionHandle(HandlePoint2, Quaternion.identity);
            if (EditorWindow.mouseOverWindow && EditorWindow.mouseOverWindow.ToString() == " (UnityEditor.SceneView)")
            {
                handle.transform.position = (HandlePoint1 + HandlePoint2) / 2;
                handle.axisLength = (HandlePoint1 - HandlePoint2).magnitude;
                handle.transform.rotation = Quaternion.LookRotation(HandlePoint2 - HandlePoint1, Vector3.forward) * Quaternion.AngleAxis(-90, Vector3.right);
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
        EditorUtility.SetDirty(target);
        if (toolsHidden)
        {
            Tools.current = previousTool;
            toolsHidden = false;
        }
    }
}

[CustomEditor(typeof(BS.ParryDefinition))]
public class ParryDefInspector : Editor
{
    public override void OnInspectorGUI()
    {

        if (showCenterTransform)
        {
            if (GUILayout.Button("Enable Point to Point transforms"))
            {
                showCenterTransform = false;
            }
        }
        else
        {
            if (GUILayout.Button("Enable Center Transform"))
            {
                showCenterTransform = true;
            }
        }

        BS.ParryDefinition parry = (BS.ParryDefinition)target;

        base.OnInspectorGUI();

        //POINT TO POINT HANDLE TRANSFORM
        if (GUI.changed || parry.transform.hasChanged || Event.current.type == EventType.MouseDown)
        {
            HandlePoint1 = parry.transform.rotation * new Vector3(0, parry.length / 2, 0) + parry.transform.position;
            HandlePoint2 = parry.transform.rotation * new Vector3(0, -parry.length / 2, 0) + parry.transform.position;
        }
    }

    bool toolsHidden = false;
    Tool previousTool;
    Vector3 HandlePoint1;
    Vector3 HandlePoint2;
    bool showCenterTransform = false;

    private void OnEnable()
    {
        BS.ParryDefinition parry = (BS.ParryDefinition)target;

        HandlePoint1 = parry.transform.rotation * new Vector3(0, parry.length / 2, 0) + parry.transform.position;
        HandlePoint2 = parry.transform.rotation * new Vector3(0, -parry.length / 2, 0) + parry.transform.position;
    }
    private void OnSceneGUI()
    {
        BS.ParryDefinition parry = (BS.ParryDefinition)target;

        if (parry.length > 0 && !showCenterTransform)
        {
            if (Tools.current != Tool.None)
            {
                previousTool = Tools.current;
                Tools.current = Tool.None;
                toolsHidden = true;
            }
            HandlePoint1 = Handles.DoPositionHandle(HandlePoint1, Quaternion.identity);
            HandlePoint2 = Handles.DoPositionHandle(HandlePoint2, Quaternion.identity);
            if (EditorWindow.mouseOverWindow && EditorWindow.mouseOverWindow.ToString() == " (UnityEditor.SceneView)")
            {
                parry.transform.position = (HandlePoint1 + HandlePoint2) / 2;
                parry.length = (HandlePoint1 - HandlePoint2).magnitude;
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
        EditorUtility.SetDirty(target);
        if (toolsHidden)
        {
            Tools.current = previousTool;
            toolsHidden = false;
        }
    }
}
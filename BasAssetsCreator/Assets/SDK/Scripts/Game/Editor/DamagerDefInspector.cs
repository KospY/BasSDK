using UnityEngine;
using UnityEditor;
using System;

namespace BS
{
    [CustomEditor(typeof(DamagerDefinition))]
    public class DamagerDefInspector : Editor
    {
        Vector3 depthPoint;
        Vector3 lengthPoint1;
        Vector3 lengthPoint2;
        Quaternion axis;
        bool toolsHidden = false;
        bool centerTransform = true;
        Tool previousTool;
        string damagerType = "";

        public override void OnInspectorGUI()
        {
            DamagerDefinition damager = (DamagerDefinition)target;
            ItemDefinition item = damager.transform.GetComponentInParent<BS.ItemDefinition>();
            damager.transform.localScale = Vector3.one;

            if (damager.penetrationLength > 0)
            {
                if (centerTransform)
                {
                    if (GUILayout.Button("Enable Point to Point transforms"))
                    {
                        centerTransform = false;
                        //damager.transform.hideFlags = HideFlags.NotEditable;
                    }
                }
                else
                {
                    if (GUILayout.Button("Enable Center Transform"))
                    {
                        centerTransform = true;
                        //damager.transform.hideFlags = 0;
                    }
                }
            }
            else
            {
                if (!centerTransform)
                {
                    centerTransform = true;
                }
            }

            EditorGUILayout.HelpBox("This Damager is set up for " + damagerType + " damage.", MessageType.Info);

            base.OnInspectorGUI();

            if (damager.colliderGroup == null)
            {
                EditorGUILayout.HelpBox("Do not forget to assign a valid Collider Group.", MessageType.Error);
            }

            if (damager.penetrationDepth < 0)
            {
                EditorGUILayout.HelpBox("Penetration Depth can not be a negative number.", MessageType.Error);
            }
            if (damager.penetrationLength < 0)
            {
                EditorGUILayout.HelpBox("Penetration Length can not be a negative number.", MessageType.Error);
            }

            if (damager.penetrationLength == 0 && damager.penetrationDepth == 0)
            {
                damagerType = "Blunt";
                if (damager.direction != BS.DamagerDefinition.Direction.All)
                {
                    EditorGUILayout.HelpBox("Direction for Blunt damagers should be set to 'All'", MessageType.Warning);
                }
            }
            else if (damager.penetrationLength == 0 && damager.penetrationDepth != 0)
            {
                damagerType = "Piercing";
                if (damager.direction != BS.DamagerDefinition.Direction.Forward)
                {
                    EditorGUILayout.HelpBox("Direction for Piercing damagers should be set to 'Forward'", MessageType.Warning);
                }
            }
            else
            {
                damagerType = "Slashing";
                if (damager.direction == BS.DamagerDefinition.Direction.All)
                {
                    EditorGUILayout.HelpBox("Direction for Slicing damagers should be set to 'Forward' or 'Forward and Backward'", MessageType.Warning);
                }
                if (damager.penetrationDepth == 0)
                {
                    EditorGUILayout.HelpBox("Slashing damagers must have Depth greater than zero.", MessageType.Warning);
                }
            }
            if (!centerTransform)
            {
                lengthPoint1 = damager.transform.position + (damager.transform.up * (damager.penetrationLength * 0.5f));
                lengthPoint2 = damager.transform.position + (damager.transform.up * -(damager.penetrationLength * 0.5f));
            }

        }

        private void OnEnable()
        {
            DamagerDefinition damager = (BS.DamagerDefinition)target;

            depthPoint = damager.GetMaxDepthPosition(false);
            lengthPoint1 = damager.transform.position + (damager.transform.up * (damager.penetrationLength * 0.5f));
            lengthPoint2 = damager.transform.position + (damager.transform.up * -(damager.penetrationLength * 0.5f));

            //damager.transform.hideFlags = HideFlags.NotEditable;
        }
        
        //private void OnSceneGUI()
        //{
        //    DamagerDefinition damager = (DamagerDefinition)target;
        //    if (toolsHidden && centerTransform)
        //    {
        //        Tools.current = previousTool;
        //        toolsHidden = false;
        //    }
        //    if (damagerType == "Piercing" && !centerTransform && damager.penetrationDepth >= 0)
        //    {
        //        if (toolsHidden)
        //        {
        //            Tools.current = previousTool;
        //            toolsHidden = false;
        //        }
        //        Vector3 tempPoint = depthPoint;
        //        tempPoint = Handles.PositionHandle(depthPoint, Quaternion.LookRotation(damager.transform.forward, damager.transform.up));

        //        depthPoint = tempPoint;
        //        damager.penetrationDepth = (depthPoint - damager.transform.position).magnitude;
        //    }
        //    else if (damagerType == "Slashing" && !centerTransform && damager.penetrationDepth >= 0 && damager.penetrationLength >= 0)
        //    {
        //        if (Tools.current != Tool.None)
        //        {
        //            previousTool = Tools.current;
        //            Tools.current = Tool.None;
        //            toolsHidden = true;
        //        }
        //        lengthPoint1 = Handles.PositionHandle(lengthPoint1, Quaternion.LookRotation(Vector3.forward, Vector3.up));
        //        lengthPoint2 = Handles.PositionHandle(lengthPoint2, Quaternion.LookRotation(Vector3.forward, Vector3.up));

        //        damager.transform.position = Vector3.Lerp(lengthPoint1, lengthPoint2, 0.5f);
        //        damager.penetrationLength = (lengthPoint1 - lengthPoint2).magnitude;

        //        Handles.color = Color.green;
        //        if (Event.current.control)
        //        {
        //            axis = Handles.Disc(damager.transform.rotation, damager.transform.position, (lengthPoint1 - lengthPoint2), HandleUtility.GetHandleSize(damager.transform.position), false, 15f);
        //        }
        //        else
        //        {
        //            axis = Handles.Disc(damager.transform.rotation, damager.transform.position, (lengthPoint1 - lengthPoint2), HandleUtility.GetHandleSize(damager.transform.position), false, 0.1f);
        //        }
        //    }
        //    else if (toolsHidden)
        //    {
        //        Tools.current = previousTool;
        //        toolsHidden = false;
        //    }

        //}
        private void OnSceneGUI()
        {

            DamagerDefinition damager = (DamagerDefinition)target;

            if (damager.penetrationLength > 0 && !centerTransform)
            {
                if (Tools.current != Tool.None)
                {
                    previousTool = Tools.current;
                    Tools.current = Tool.None;
                    toolsHidden = true;
                }

                EditorGUI.BeginChangeCheck();
                lengthPoint1 = Handles.DoPositionHandle(lengthPoint1, Quaternion.LookRotation(Vector3.forward, Vector3.up));
                lengthPoint2 = Handles.DoPositionHandle(lengthPoint2, Quaternion.LookRotation(Vector3.forward, Vector3.up));
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Moved Handle");
                }
                Handles.color = Color.green;
                try
                {
                    if (EditorWindow.focusedWindow && EditorWindow.focusedWindow.ToString() == " (UnityEditor.SceneView)")
                    {
                        damager.transform.position = Vector3.Lerp(lengthPoint1, lengthPoint2, 0.5f);
                        damager.penetrationLength = (lengthPoint1 - lengthPoint2).magnitude;

                        if (Event.current.control)
                        {
                            axis = Handles.Disc(damager.transform.rotation, damager.transform.position, (lengthPoint1 - lengthPoint2), HandleUtility.GetHandleSize(damager.transform.position), false, 15f);
                        }
                        else
                        {
                            axis = Handles.Disc(damager.transform.rotation, damager.transform.position, (lengthPoint1 - lengthPoint2), HandleUtility.GetHandleSize(damager.transform.position), false, 0.1f);
                        }
                        damager.transform.rotation = Quaternion.LookRotation(lengthPoint2 - lengthPoint1, axis * Vector3.forward) * Quaternion.AngleAxis(-90, Vector3.right);
                    }
                    else
                    {
                        axis = Handles.Disc(damager.transform.rotation, damager.transform.position, (lengthPoint1 - lengthPoint2), HandleUtility.GetHandleSize(damager.transform.position), false, 0.1f);
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

            if (target) EditorUtility.SetDirty(target);
            if (toolsHidden)
            {
                Tools.current = previousTool;
                toolsHidden = false;
            }
        }

    }
}
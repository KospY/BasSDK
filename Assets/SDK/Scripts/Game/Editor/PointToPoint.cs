using UnityEngine;
using UnityEditor;
using System;

namespace ThunderRoad
{
    public class PointToPoint : Editor
    {
        protected Vector3 lengthPoint1;
        protected Vector3 lengthPoint2;
        protected Quaternion axis;
        protected bool toolsHidden = false;
        protected bool centerTransform = true;
        protected Tool previousTool;
        protected bool centerRotation;
        protected MonoBehaviour script;

        protected void PointToPointButton(float length)
        {
            if (length > 0)
            {
                if (centerTransform)
                {
                    if (GUILayout.Button("Enable Point to Point transforms"))
                    {
                        centerTransform = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Enable Center Transform"))
                    {
                        centerTransform = true;
                        script.transform.hideFlags = 0;
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
            if (!centerTransform)
            {
                ResetPoints(length);
            }
        }

        protected void ResetPoints(float length)
        {
            lengthPoint1 = script.transform.position + (script.transform.up * (length * 0.5f));
            lengthPoint2 = script.transform.position + (script.transform.up * -(length * 0.5f));
        }

        protected void UpdatePoints(ref float length)
        {
            if (length > 0 && !centerTransform)
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
                        script.transform.position = Vector3.Lerp(lengthPoint1, lengthPoint2, 0.5f);
                        length = (lengthPoint1 - lengthPoint2).magnitude;

                        if (Event.current.control)
                        {
                            axis = Handles.Disc(script.transform.rotation, script.transform.position, (lengthPoint1 - lengthPoint2), HandleUtility.GetHandleSize(script.transform.position), false, 15f);
                        }
                        else
                        {
                            axis = Handles.Disc(script.transform.rotation, script.transform.position, (lengthPoint1 - lengthPoint2), HandleUtility.GetHandleSize(script.transform.position), false, 0.1f);
                        }
                        script.transform.rotation = Quaternion.LookRotation(lengthPoint2 - lengthPoint1, axis * Vector3.forward) * Quaternion.AngleAxis(-90, Vector3.right);
                    }
                    else
                    {
                        axis = Handles.Disc(script.transform.rotation, script.transform.position, (lengthPoint1 - lengthPoint2), HandleUtility.GetHandleSize(script.transform.position), false, 0.1f);
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
        
        public void OnDisable()
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

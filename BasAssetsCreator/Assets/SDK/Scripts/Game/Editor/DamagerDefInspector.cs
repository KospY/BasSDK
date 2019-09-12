using UnityEngine;
using UnityEditor;

namespace BS
{
    [CustomEditor(typeof(DamagerDefinition))]
    public class DamagerDefInspector : Editor
    {
        string damagerType = "";
        bool pointToPointDisabled = false;
        public override void OnInspectorGUI()
        {
            DamagerDefinition damager = (DamagerDefinition)target;
            ItemDefinition item = damager.transform.GetComponentInParent<BS.ItemDefinition>();
            damager.transform.localScale = Vector3.one;

            bool buttonsPressed = false;
            if (centerTransform)
            {
                GUI.enabled = !pointToPointDisabled;
                if (GUILayout.Button("Enable Point to Point transforms"))
                {
                    centerTransform = false;
                    buttonsPressed = true;
                    damager.transform.hideFlags = HideFlags.NotEditable;
                }
                GUI.enabled = true;
            }
            else
            {
                if (GUILayout.Button("Enable Center Transform"))
                {
                    centerTransform = true;
                    buttonsPressed = true;
                    damager.transform.hideFlags = 0;
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

            if (pointToPointDisabled)
            {
                foreach (Transform parent in damager.transform.GetComponentsInParent<Transform>())
                {
                    if (parent.rotation != Quaternion.identity && parent.gameObject != damager.gameObject)
                    {
                        EditorGUILayout.HelpBox("Object " + parent.name + " is rotated. Please make sure all the parents of the damagers have rotation set to 0. Point-to-Point transform has been disabled.", MessageType.Warning);
                        damager.transform.hideFlags = 0;
                    }
                }

            }

            //POINTER POINT
            if (damager.transform.hasChanged || GUI.changed || centerTransform || !buttonsPressed)
            {
                depthPoint = damager.transform.rotation * new Vector3(0, 0, -damager.penetrationDepth) + damager.transform.position;
                lengthPoint1 = damager.transform.rotation * new Vector3(0, -damager.penetrationLength / 2, 0) + damager.transform.position;
                lengthPoint2 = damager.transform.rotation * new Vector3(0, damager.penetrationLength / 2, 0) + damager.transform.position;
                damager.transform.hasChanged = false;
            }

        }
        Vector3 depthPoint;
        Vector3 lengthPoint1;
        Vector3 lengthPoint2;
        bool toolsHidden = false;
        bool centerTransform;
        Tool previousTool;
        private void Awake()
        {
            DamagerDefinition damager = (BS.DamagerDefinition)target;

            foreach (Transform parent in damager.transform.GetComponentsInParent<Transform>())
            {
                if (parent.rotation != Quaternion.identity && parent.gameObject != damager.gameObject)
                {
                    centerTransform = true;
                    pointToPointDisabled = true;
                }
            }

            depthPoint = damager.transform.rotation * new Vector3(0, 0, -damager.penetrationDepth) + damager.transform.position;
            lengthPoint1 = damager.transform.rotation * new Vector3(0, -damager.penetrationLength / 2, 0) + damager.transform.position;
            lengthPoint2 = damager.transform.rotation * new Vector3(0, damager.penetrationLength / 2, 0) + damager.transform.position;

            damager.transform.hideFlags = HideFlags.NotEditable;
        }
        Quaternion axis;
        private void OnSceneGUI()
        {
            DamagerDefinition damager = (DamagerDefinition)target;
            if (toolsHidden && centerTransform)
            {
                Tools.current = previousTool;
                toolsHidden = false;
            }
            if (damagerType == "Piercing" && !centerTransform && damager.penetrationDepth >= 0)
            {
                if (toolsHidden)
                {
                    Tools.current = previousTool;
                    toolsHidden = false;
                }
                Vector3 tempPoint = depthPoint;
                tempPoint = Handles.PositionHandle(depthPoint, Quaternion.identity);

                depthPoint = tempPoint;
                if (!damager.transform.hasChanged)
                {
                    damager.penetrationDepth = (depthPoint - damager.transform.position).magnitude;
                    damager.transform.rotation = Quaternion.LookRotation(depthPoint - damager.transform.position) * Quaternion.AngleAxis(180, Vector3.right);
                    damager.transform.hasChanged = false;
                }
            }
            else if (damagerType == "Slashing" && !centerTransform && damager.penetrationDepth >= 0 && damager.penetrationLength >= 0)
            {
                if (Tools.current != Tool.None)
                {
                    previousTool = Tools.current;
                    Tools.current = Tool.None;
                    toolsHidden = true;
                }
                lengthPoint1 = Handles.PositionHandle(lengthPoint1, Quaternion.identity);
                lengthPoint2 = Handles.PositionHandle(lengthPoint2, Quaternion.identity);

                damager.transform.position = (lengthPoint1 + lengthPoint2) / 2;
                damager.penetrationLength = (lengthPoint1 - lengthPoint2).magnitude;
                damager.transform.hasChanged = false;
                Handles.color = Color.green;
                if (Event.current.control)
                {
                    axis = Handles.Disc(damager.transform.rotation, damager.transform.position, (lengthPoint1 - lengthPoint2), HandleUtility.GetHandleSize(damager.transform.position), false, 15f);
                }
                else
                {
                    axis = Handles.Disc(damager.transform.rotation, damager.transform.position, (lengthPoint1 - lengthPoint2), HandleUtility.GetHandleSize(damager.transform.position), false, 0.1f);
                }
                damager.transform.rotation = Quaternion.LookRotation(lengthPoint1 - lengthPoint2, axis * Vector3.forward) * Quaternion.AngleAxis(-90, Vector3.right);
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
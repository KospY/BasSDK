using UnityEditor;
using UnityEngine;

namespace ThunderRoad
{
    [CustomEditor(typeof(DragArea))]
    public class DragAreaEditor : Editor
    {
        private static readonly string[] propertyToExclude = {"m_Script", "rbToGetVelocityFrom", "velocityAverageFrames", "velocityOrigin"};

        public override void OnInspectorGUI()
        {
            var dragArea = target as DragArea;
            if (!dragArea) return;

            serializedObject.Update();
            
            if (dragArea.velocityType == DragArea.VelocityType.Estimated)
            {
                dragArea.velocityAverageFrames = EditorGUILayout.IntField("velocityAverageFrames", dragArea.velocityAverageFrames);
                dragArea.velocityOrigin = (Transform) EditorGUILayout.ObjectField("velocityOrigin",
                    dragArea.velocityOrigin, typeof(Transform), true);
            }
            else if (dragArea.velocityType == DragArea.VelocityType.FromRigidbody)
            {
                dragArea.rbToGetVelocityFrom = (Rigidbody) EditorGUILayout.ObjectField("rbToGetVelocityFrom",
                    dragArea.rbToGetVelocityFrom, typeof(Rigidbody), true);
            }
            
            DrawPropertiesExcluding(serializedObject, propertyToExclude);
            
            serializedObject.ApplyModifiedProperties();

        }
    }
}
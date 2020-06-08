using System;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad
{
    [CustomEditor(typeof(ParryTargetDefinition))]
    public class ParryDefInspector : PointToPoint
    {
        ParryTargetDefinition parry;

        public override void OnInspectorGUI()
        {
            script = parry = (ParryTargetDefinition)target;

            PointToPointButton(parry.GetLineStart(), parry.GetLineEnd());

            base.OnInspectorGUI();

            if (parry.length < 0)
            {
                EditorGUILayout.HelpBox("Parry Length can not be a negative number.", MessageType.Error);
            }

        }

        private void OnEnable()
        {
            script = parry = (ParryTargetDefinition)target;
            ResetPoints(parry.GetLineStart(), parry.GetLineEnd());
        }
        private void OnSceneGUI()
        {
            script = parry = (ParryTargetDefinition)target;
            UpdatePoints(ref parry.length, true);
        }
    }
}
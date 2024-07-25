using System;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad
{
    [CustomEditor(typeof(ParryTarget))]
    public class ParryDefInspector : PointToPoint
    {
        ParryTarget parry;

        public override void OnInspectorGUI()
        {
            script = parry = (ParryTarget)target;

            PointToPointButton(parry.GetLineStart(), parry.GetLineEnd());

            base.OnInspectorGUI();

            if (parry.length < 0)
            {
                EditorGUILayout.HelpBox("Parry Length can not be a negative number.", MessageType.Error);
            }

        }

        private void OnEnable()
        {
            script = parry = (ParryTarget)target;
            ResetPoints(parry.GetLineStart(), parry.GetLineEnd());
        }
        private void OnSceneGUI()
        {
            script = parry = (ParryTarget)target;
            UpdatePoints(ref parry.length, true);
        }
    }
}
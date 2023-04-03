using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Manikin
{
    [CustomEditor(typeof(ManikinSmrPart))]
    public class ManikinSmrPartEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            if (GUILayout.Button("Initialize"))
            {
                (target as ManikinSmrPart).Initialize();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

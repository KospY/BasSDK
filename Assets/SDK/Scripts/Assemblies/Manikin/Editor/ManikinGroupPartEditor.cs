using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Manikin
{
    [CustomEditor(typeof(ManikinGroupPart)), CanEditMultipleObjects]
    public class ManikinGroupPartEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            if(GUILayout.Button("Initialize"))
            {
                (target as ManikinGroupPart).Initialize();
            }

            if (GUILayout.Button("Initialize LODs"))
            {
                (target as ManikinGroupPart).InitializeLODs();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

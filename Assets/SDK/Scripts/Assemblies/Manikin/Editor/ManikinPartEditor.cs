using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderRoad.Manikin
{
    [CustomEditor(typeof(ManikinPart))]
    public class ManikinPartEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
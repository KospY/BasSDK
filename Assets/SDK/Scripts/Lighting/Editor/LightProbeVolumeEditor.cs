using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ThunderRoad
{
    [CustomEditor(typeof(LightProbeVolume))]
    public class LightProbeVolumeEditor : Editor
    {
        private BoxBoundsHandle boundsHandle = new BoxBoundsHandle();
        private LightProbeVolume lightProbeVolume;

        public override void OnInspectorGUI()
        {
            lightProbeVolume = target as LightProbeVolume;
            if (!lightProbeVolume) return;

            lightProbeVolume.editingSizeThroughEditor =
                EditorGUILayout.Toggle("Edit size", lightProbeVolume.editingSizeThroughEditor);
            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            lightProbeVolume = target as LightProbeVolume;
            if (!lightProbeVolume) return;
            if (!lightProbeVolume.editingSizeThroughEditor) return;

            Handles.color = new Color(.2f, .8f, .1f);

            boundsHandle.center = lightProbeVolume.transform.position;
            boundsHandle.size = lightProbeVolume.size;

            // draw the handle
            EditorGUI.BeginChangeCheck();
            boundsHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                // record the target object before setting new values so changes can be undone/redone
                Undo.RecordObject(lightProbeVolume, "Change Size from handles");

                lightProbeVolume.transform.position = boundsHandle.center;
                lightProbeVolume.size = boundsHandle.size;
            }
        }
    }
}
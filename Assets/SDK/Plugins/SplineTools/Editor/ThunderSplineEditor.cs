using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Splines;

namespace ThunderRoad.Splines
{
    [CustomEditor(typeof(ThunderSpline))]
    public class ThunderSplineEditor : Editor
    {
        private BoxBoundsHandle boxBoundsHandle = new BoxBoundsHandle();
        private ThunderSpline thunderSpline;

        void OnEnable()
        {
            thunderSpline = (ThunderSpline)target;
        }

        protected virtual void OnSceneGUI()
        {
            if (thunderSpline.focusKnot < 0) return;
            if (thunderSpline.focusKnot >= thunderSpline.primarySpline.Count) return;

            Matrix4x4 localToWorld = thunderSpline.transform.localToWorldMatrix;
            Matrix4x4 worldToLocal = thunderSpline.transform.worldToLocalMatrix;
            EditorGUI.BeginChangeCheck();

            BezierKnot knot = thunderSpline.primarySpline[thunderSpline.focusKnot];

            Vector4 positionV4 = new Vector4(knot.Position.x, knot.Position.y, knot.Position.z, 1.0f);

            Vector4 tangeantInV4 = new Vector4(knot.TangentIn.x, knot.TangentIn.y, knot.TangentIn.z, 0.0f) + positionV4;
            tangeantInV4 = localToWorld * tangeantInV4;
            Vector4 tangeantOutV4 = new Vector4(knot.TangentOut.x, knot.TangentOut.y, knot.TangentOut.z, 0.0f) + positionV4;
            tangeantOutV4 = localToWorld * tangeantOutV4;

            positionV4 = localToWorld * positionV4;

            Vector3 splinePosition = Handles.PositionHandle(positionV4, Quaternion.identity);
            Vector3 splineTangeantIn = Handles.PositionHandle(tangeantInV4, Quaternion.identity);
            Vector3 splineTangeantOut = Handles.PositionHandle(tangeantOutV4, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                positionV4 = new Vector4(splinePosition.x, splinePosition.y, splinePosition.z, 1.0f);
                positionV4 = worldToLocal * positionV4;

                 tangeantInV4 = new Vector4(splineTangeantIn.x, splineTangeantIn.y, splineTangeantIn.z, 1.0f);
                tangeantInV4 = worldToLocal * tangeantInV4;
                tangeantInV4 -= positionV4;

                tangeantOutV4 = new Vector4(splineTangeantOut.x, splineTangeantOut.y, splineTangeantOut.z, 1.0f);
                tangeantOutV4 = worldToLocal * tangeantOutV4;
                tangeantOutV4 -= positionV4;

                knot = new BezierKnot((Vector3)positionV4, (Vector3)tangeantInV4, (Vector3)tangeantOutV4, Quaternion.identity);
                thunderSpline.primarySpline[thunderSpline.focusKnot] = knot;
            }
        }
    }
}

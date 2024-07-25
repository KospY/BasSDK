using System.Collections.Generic;

using Shadowood.RaycastTexture;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

using ThunderRoad.Splines;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
//#if UNITY_EDITOR
using UnityEngine.Splines;

//#endif

[ExecuteAlways]
public class ThunderSplineWithin : MonoBehaviour
{
    public bool debug;

    public ThunderSpline thunderSpline;

    public bool drawAlways = true;

    public int gridCount = 20;

    public float debugScale = 0.1f;

    public float maxDistance = 0;


    //public Transform transformTarget;


    public bool updateWhenChanged = true;

    public RaycastTexture raycastTexture;

    public UnityEvent splineChanged = new UnityEvent();

    //public Vector3 targetPosition;
    //public Quaternion targetRotation;
    //public float targetScale;
    public Matrix4x4 debugMatrix;
    private Matrix4x4 lastMatrix;

    //private Vector3 targetPositionLast;

#if UNITY_EDITOR

    [Button]
    void UpdateOceanRenderer()
    {
        if (raycastTexture)
        {
            raycastTexture.oceanSplineMask = this;
            raycastTexture.oceanRenderer.thunderSplineWithin = this;
            raycastTexture.oceanRenderer.RunWithChosenPlatform();
        }
    }

    [Button]
    void UpdateOceanTextures()
    {
        if (raycastTexture)
        {
            raycastTexture.oceanSplineMask = this;
            raycastTexture.oceanRenderer.thunderSplineWithin = this;
            raycastTexture.RenderNow();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (drawAlways) return;
        Draw();
    }

    void OnDrawGizmos()
    {
        if (!drawAlways) return;
        Draw();
    }


    void OnEnable()
    {
        UnityEditor.Splines.EditorSplineUtility.AfterSplineWasModified += SplineChanged;
    }

    void OnDisable()
    {
        UnityEditor.Splines.EditorSplineUtility.AfterSplineWasModified -= SplineChanged;
    }


    private void SplineChanged(Spline spline)
    {
        if (!isActiveAndEnabled) return;
        if (!updateWhenChanged) return;
        if (spline != thunderSpline.primarySpline) return;
        if (debug) Debug.Log("SplineChanged", this);
        splineChanged.Invoke();
    }


    void Update()
    {
        //if (lastMatrix != transformTarget.localToWorldMatrix) transformTarget.hasChanged = true;
        //if (targetPosition != targetPositionLast)
        //if (targetPosition.hasChanged)
        if (lastMatrix != debugMatrix)
        {
            lastMatrix = debugMatrix;
            //targetPositionLast = targetPosition;
            //lastMatrix = transformTarget.localToWorldMatrix;
            //targetPosition.hasChanged = false;
            Run();
        }
    }

    private List<Vector3> cachedPos = new List<Vector3>();


    void Reset()
    {
        //transformTarget = transform;
    }

    [Button, ContextMenu("Run")]
    public void Run()
    {
        cachedPos.Clear();

        for (int x = 0; x < gridCount; x++)
        {
            for (int y = 0; y < gridCount; y++)
            {
                var pos = new Vector3((x - gridCount / 2.0f) / (float) gridCount, 0, (y - gridCount / 2.0f) / (float) gridCount);
                //pos = transformTarget.TransformPoint(pos);
                //var mat = Matrix4x4.Translate(targetPosition);
                var mat = thunderSpline.transform.localToWorldMatrix * debugMatrix;
                pos = mat.MultiplyPoint(pos);


                if (thunderSpline)
                {
                    //Gizmos.color = Color.white;
                    var inside = IsInsideSpline(thunderSpline.transform.InverseTransformPoint(pos), thunderSpline, maxDistance);
                    //if (inside) Gizmos.color = Color.red;
                    if (inside) cachedPos.Add(pos);
                }
            }
        }
    }

    public void Draw()
    {
        if (!debug) return;
        if (thunderSpline == null || thunderSpline.primarySpline == null) return;
        Gizmos.matrix = Matrix4x4.identity;

        var pos = debugMatrix.GetPosition(); //transformTarget.TransformPoint(Vector3.zero);


        var mat = thunderSpline.transform.localToWorldMatrix; // * debugMatrix ;
        pos = mat.MultiplyPoint(pos);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(pos, 0.5f);


        SplineUtility.GetNearestPoint(thunderSpline.primarySpline, thunderSpline.transform.InverseTransformPoint(pos), out var splinePoint, out var t);
        splinePoint = thunderSpline.transform.TransformPoint(splinePoint);


        //Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pos, splinePoint);
        Gizmos.DrawSphere(splinePoint, 0.5f);

        Gizmos.color = Color.green;
        thunderSpline.primarySpline.Evaluate(t, out var position, out var tangent, out _);
        Vector3 tan = tangent;
        Gizmos.DrawRay(pos, tan.normalized);

        Gizmos.color = Color.cyan;
        position = thunderSpline.transform.TransformPoint(position);
        Gizmos.DrawLine(pos, position);
        Gizmos.DrawSphere(position, 0.5f);

        foreach (var o in cachedPos)
        {
            Gizmos.DrawSphere(o, debugScale);
        }
    }
#endif
    public static bool IsInsideSpline(float3 point, ThunderSpline thunderSpline, float maxDistance = 20)
    {
        SplineUtility.GetNearestPoint(thunderSpline.primarySpline, point, out var splinePoint, out var t);
        thunderSpline.primarySpline.Evaluate(t, out _, out var tangent, out _);
        float3 cross = math.cross(math.up(), math.normalize(tangent));

        if (maxDistance > 0)
        {
            //splinePoint = thunderSpline.transform.TransformPoint(splinePoint);
            if (Vector3.Distance(splinePoint, point) > maxDistance) return false;
        }

        return math.dot(splinePoint - point, cross) < 0;
    }

}

public static class Util{
    public static Vector3 GetScale2(this Matrix4x4 m)
    {
        var x = Mathf.Sqrt(m.m00 * m.m00 + m.m01 * m.m01 + m.m02 * m.m02);
        var y = Mathf.Sqrt(m.m10 * m.m10 + m.m11 * m.m11 + m.m12 * m.m12);
        var z = Mathf.Sqrt(m.m20 * m.m20 + m.m21 * m.m21 + m.m22 * m.m22);

        return new Vector3(x, y, z);
    }
}

#if UNITY_EDITOR
namespace UnityEditor
{
    
    [CustomEditor(typeof(ThunderSplineWithin)), CanEditMultipleObjects]
    public class ThunderSplineWithinEditor : Editor
    {
        protected virtual void OnSceneGUI()
        {
            var example = (ThunderSplineWithin) target;

            if (!example.debug) return;
            EditorGUI.BeginChangeCheck();


            //Handles.TransformHandle(ref example.targetPosition, ref example.targetRotation, ref example.targetScale);
            var pos = example.debugMatrix.GetPosition();
            var rot = Quaternion.identity; //example.debugMatrix.GetRotation();
            var scale = example.debugMatrix.GetScale2();

            Handles.matrix = example.transform.localToWorldMatrix;

            Handles.TransformHandle(ref pos, ref rot, ref scale);
            {
                Debug.Log("Change Transform", target);
                Undo.RecordObject(example, "Change Transform");
                //example.targetPosition.hasChanged = true;
                //example.targetPosition.position = targetPositionPosition;
                //example.targetPosition.rotation = targetPositionRotation;
                //example.targetPosition.localScale = scale;
                pos = new Vector3(pos.x, 0, pos.z);

                example.debugMatrix = Matrix4x4.TRS(pos, Quaternion.identity, scale);

                //example.targetPosition = new ;
            }
        }
    }
}
#endif

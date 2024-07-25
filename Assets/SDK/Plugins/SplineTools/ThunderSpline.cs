using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
//using UnityEditor.Splines;
#endif //UNITY_EDITOR

namespace ThunderRoad.Splines
{
    [ExecuteInEditMode]
    public class ThunderSpline : MonoBehaviour, ISplineContainer
    {
        public bool debug;
        
        public Spline[] unitySplines;
        public Spline primarySpline => unitySplines.IsNullOrEmpty() || knotsCount == 0 ? (unitySplines = new Spline[1] { new Spline(GetDefaultKnots()) })[0] : unitySplines[0];
        [Header("Spline")]
        public bool capStart;
        public bool capEnd;
        public UnityEvent onSplineChangeEvent;
        public UnityEvent OnReachFirstKnotEvent;
        public UnityEvent OnReachLastKnotEvent;
        protected int knotsCount;

        [Header("Sampling")]
        public int samplingResolution = 10;
        public int samplingIterations = 3;
        public int lengthCalculatorArrayResolution = 1000;

        [Header("Forces")]
        public bool forceAlongSpline = false;
        public int currentForceCurveIndex = 0;
        public List<ForceCurve> forceCurves = new List<ForceCurve>();

        [Serializable]
        public class ForceCurve
        {
            [Range(0f, 1f)]
            public float splineForceTarget = 0.5f;
            public ForceMode forceMode = ForceMode.Force;
            public float maxForce = 10f;
            [Tooltip("If true the curve need to be set for real distance as input. If false curve input will be between 0-1")]
            public bool useRealDistance= true;
            public AnimationCurve forceByDistance = AnimationCurve.Linear(0f, 0.5f, 1f, 0.5f);
        }

        [Header("Friction")]
        public bool splineFriction = true;
        [Tooltip("This curve determines how much friction force an object will experience based on its speed. The first key should always be 0, 0. Objects moving faster than the highest speed (time) on the curve are affected by the amount of friction force as defined by the value at the highest speed (time).")]
        public AnimationCurve speedFrictionForceCurve = AnimationCurve.Linear(0f, 0f, 100f, 100f);
        [Tooltip("This curve should go from time 0 to 1, with values from 0 to 1. Any times/values outside of this range will be clamped! To increase the friction force, increase the max friction force value.")]
        public AnimationCurve splineFrictionStrengthCurve = AnimationCurve.Linear(0f, 0f, 0f, 0f);

#if UNITY_EDITOR
        [Header("Debug")]
        [Range(0f, 1f), Tooltip("Shows a set of lines at the T sample you input, to show the up, right, and forward (usually hard to see) directions at the given point.")]
        public float splineSampleT = 0.5f;
        [Tooltip("Change how many draw steps are done for each of the two gizmos. Higher numbers means higher \"resolution\" but also means worse performance in the editor.")]
        public int gizmoDrawSteps = 100;
        public bool drawForceGizmo = false;
        [Tooltip("Change this value if the force along spline gizmo is hard to see.")]
        public Vector3 forceCurveGizmoDrawOffset = new Vector3(0f, 0.5f, 0f);
        public bool drawFrictionGizmo = false;
        [Tooltip("Change this value if the spline friction gizmo is hard to see.")]
        public Vector3 frictionCurveGizmoDrawOffset = new Vector3(0f, -0.5f, 0f);
        [Tooltip("This value can't be changed, it exits purely to display the length of your spline.")]
        public float splineLength;

        [Header("Focus knot for Edit")]
        public int focusKnot = -1;
#endif

        [SerializeField, HideInInspector]
        KnotLinkCollection m_Knots = new KnotLinkCollection();

        public float length { get; protected set; }
        public bool closed => primarySpline.Closed;

        public float maxForceDistance => _maxForceDistance ?? (float)(_maxForceDistance = forceCurves[currentForceCurveIndex].forceByDistance.GetLastTime());
        private float? _maxForceDistance;

        /// <summary>
        /// The list of all splines attached to that container.
        /// </summary>
        public IReadOnlyList<Spline> Splines
        {
            get => new ReadOnlyCollection<Spline>(unitySplines ?? (unitySplines = new Spline[1] { new Spline() }));
            set
            {
                if (value == null)
                {
                    unitySplines = new Spline[0];
                    return;
                }

                unitySplines = new Spline[value.Count];
                for (int i = 0; i < unitySplines.Length; ++i)
                    unitySplines[i] = value[i];
            }
        }

        /// <summary>
        /// A collection of all linked knots. Linked knots can be on different splines. However, knots can
        /// only link to other knots within the same container. This collection is used to maintain
        /// the validity of the links when operations such as knot insertions or removals are performed on the splines.
        /// </summary>
        public KnotLinkCollection KnotLinkCollection => m_Knots;

        protected float[] interpolateLengths;

        public delegate void SplineChangeEvent();
        public event SplineChangeEvent OnSplineChange;

        private void OnValidate()
        {
            CountKnots();
        }

        private void Awake()
        {
            CountKnots();
        }

        protected void Start()
        {
            Spline.Changed += SplineChanged;
            #if UNITY_EDITOR
                //UnityEditor.Splines.EditorSplineUtility.AfterSplineWasModified += o=>CalculateSplineInfos();
            #endif
            CalculateSplineInfos();
        }

        public void CountKnots()
        {
            int result = 0;
            foreach (var knot in unitySplines[0].Knots) result++;
            knotsCount = result;
        }

        protected IEnumerable<BezierKnot> GetDefaultKnots()
        {
            yield return new BezierKnot(transform.position);
            yield return new BezierKnot(transform.position + transform.forward);
        }

        private void SplineChanged(Spline spline, int index, SplineModification modificationType)
        {
            var splineIndex = Array.IndexOf(unitySplines, spline);
            if (splineIndex < 0)
                return;

            switch (modificationType)
            {
                case SplineModification.KnotModified:
                    this.SetLinkedKnotPosition(new SplineKnotIndex(splineIndex, index));
                    break;

                case SplineModification.KnotInserted:
                    KnotLinkCollection.KnotInserted(splineIndex, index);
                    break;

                case SplineModification.KnotRemoved:
                    KnotLinkCollection.KnotRemoved(splineIndex, index);
                    break;
            }
            CalculateSplineInfos();
        }

        protected void CalculateSplineInfos()
        {
            if(debug)Debug.Log("CalculateSplineInfos: " + name, gameObject);
            float lastLength = length;
            length = primarySpline.GetLength();
            interpolateLengths = new float[lengthCalculatorArrayResolution + 1];
            for (int i = 0; i < lengthCalculatorArrayResolution; i++)
            {
                interpolateLengths[i] = i * (length / lengthCalculatorArrayResolution);
            }
            interpolateLengths[lengthCalculatorArrayResolution] = length;
            float scalar = length / lastLength;
            for (int i = splineFrictionStrengthCurve.keys.Length - 1; i >= 0; i--)
            {
                Keyframe key = splineFrictionStrengthCurve.keys[i];
                key.time *= scalar;
            }
            CountKnots();
            OnSplineChange?.Invoke();
            onSplineChangeEvent?.Invoke();
        }

        public float RangeClampAlmost01(float t) => Mathf.Clamp(t, 0.000001f, 1f - 0.000001f);

        /// <summary>
        /// Gets the world-space position and world-oriented directions for the spline point nearest to the input position
        /// </summary>
        /// <param name="t">The normalized length along the spline</param>
        /// <param name="position">The world-space position on the spline</param>
        /// <param name="forward">The world-oriented forward direction of the point on the spline, similar to transform.forward</param>
        /// <param name="up">The world-oriented up direction of the point on the spline, similar to transform.up</param>
        /// <param name="right">The world-oriented right direction of the point on the spline, similar to transform.right</param>
        public void GetSplinePointInfo(float t, out Vector3 position, out Vector3 forward, out Vector3 up, out Vector3 right)
        {
            t = RangeClampAlmost01(t);
            position = transform.TransformPoint(primarySpline.EvaluatePosition(t));
            forward = transform.TransformDirection(primarySpline.EvaluateTangent(t)).normalized;
            up = transform.TransformDirection(primarySpline.EvaluateUpVector(t)).normalized;
            right = Vector3.Cross(up, forward).normalized;
        }

        public void GetSplinePointCurvatureInfo(float t, out float curvature, out Vector3 center)
        {
            curvature = primarySpline.EvaluateCurvature(t);
            center = primarySpline.EvaluateCurvatureCenter(t);
        }

        /// <summary>
        /// Gets the world-space position and world-oriented directions for the spline point nearest to the input position
        /// </summary>
        /// <param name="from">The position to find the closest point from</param>
        /// <param name="t">The normalized length along the spline. If an object is off the spline, this can be less than 0 or greater than 1</param>
        /// <param name="position">The world-space position on the spline</param>
        /// <param name="forward">The world-oriented forward direction of the point on the spline, similar to transform.forward</param>
        /// <param name="up">The world-oriented up direction of the point on the spline, similar to transform.up</param>
        /// <param name="right">The world-oriented right direction of the point on the spline, similar to transform.right</param>
        public void GetClosestSplinePoint(Vector3 from, out float t, out Vector3 position, out Vector3 forward, out Vector3 up, out Vector3 right)
        {
            SplineUtility.GetNearestPoint(primarySpline, transform.InverseTransformPoint(from), out _, out t, samplingResolution, samplingIterations);
            GetSplinePointInfo(t, out position, out forward, out up, out right);
        }

        /// <summary>
        /// Gets the world-space position and world-oriented directions for the spline point nearest to the input position
        /// </summary>
        /// <param name="from">The position to find the closest point from</param>
        /// <param name="t">The normalized length along the spline</param>
        /// <param name="position">The world-space position on the spline</param>
        /// <param name="forward">The world-oriented forward direction of the point on the spline, similar to transform.forward</param>
        /// <param name="up">The world-oriented up direction of the point on the spline, similar to transform.up</param>
        /// <param name="right">The world-oriented right direction of the point on the spline, similar to transform.right</param>
        /// <returns>True if the output spline position is within the specified distance from the input position</returns>
        public bool SplinePointWithinDistance(Vector3 from, float distance, out float t, out Vector3 position, out Vector3 forward, out Vector3 up, out Vector3 right)
        {
            GetClosestSplinePoint(from, out t, out position, out forward, out up, out right);
            return (position - from).sqrMagnitude <= distance * distance;
        }

        /// <summary>
        /// Get the distance along the spline between two points on the spline. Uses pre-calculated information, not fully accurate
        /// </summary>
        /// <param name="a">One endpoint along the spline, as a normalized distance from start to end (0 to 1)</param>
        /// <param name="b">Another endpoint along the spline, as a normalized distance from start to end (0 to 1)</param>
        /// <returns>The length in meters between the two points on the spline</returns>
        public float DistanceBetweenNormalizedPoints(float a, float b)
        {
            a = RangeClampAlmost01(a);
            b = RangeClampAlmost01(b);
            int aIndex = Mathf.RoundToInt(lengthCalculatorArrayResolution * a);
            int bIndex = Mathf.RoundToInt(lengthCalculatorArrayResolution * b);
            return Mathf.Abs(interpolateLengths[bIndex] - interpolateLengths[aIndex]);
        }

        /// <summary>
        /// Get the force along the spline at normalized position t
        /// </summary>
        /// <param name="t">The normalized position/time along the spline</param>
        /// <returns>A Vector3 representing the force as defined towards the target position</returns>
        public Vector3 GetSplineForceAtNormalizedPoint(float t)
        {
            GetSplinePointInfo(t, out _, out var forward, out _, out _);
            return GetSplineForceAtNormalizedPointWithForward(t, forward);
        }

        /// <summary>
        /// Get the force along the spline at normalized position t, using a pre-sampled world forward (To avoid excess evaluation of the spline)
        /// </summary>
        /// <param name="t">The normalized position/time along the spline</param>
        /// <param name="worldForward">The world forward for the normalized position being sampled</param>
        /// <returns></returns>
        public Vector3 GetSplineForceAtNormalizedPointWithForward(float t, Vector3 worldForward)
        {
            if(t <= forceCurves[currentForceCurveIndex].splineForceTarget)
            {
                return worldForward * GetSplineForceMagnitudeAtNormalizedPoint(t);
            }

            return - worldForward * GetSplineForceMagnitudeAtNormalizedPoint(t);
        }

        public float GetSplineForceMagnitudeAtNormalizedPoint(float t)
        {
            float distance;
            if (forceCurves[currentForceCurveIndex].useRealDistance)
            {
                distance = DistanceBetweenNormalizedPoints(t, forceCurves[currentForceCurveIndex].splineForceTarget);
                return GetSplineForceMagnitudeByDistance(distance);
            }

            distance = Mathf.Abs(t - forceCurves[currentForceCurveIndex].splineForceTarget);
            return GetSplineForceMagnitudeByDistance(distance);
        }

        public float GetSplineForceMagnitudeByDistance(float dist)
        {
            return forceCurves[currentForceCurveIndex].forceByDistance.Evaluate(dist) * forceCurves[currentForceCurveIndex].maxForce;
        }

        public Vector3 GetSplineFrictionForBodyAtT(float t, Vector3 velocity) => -speedFrictionForceCurve.Evaluate(Mathf.Clamp(velocity.magnitude, 0f, speedFrictionForceCurve.GetLastTime())) * splineFrictionStrengthCurve.Evaluate(Mathf.Clamp(t, 0, length)) * velocity.normalized;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            GetSplinePointInfo(splineSampleT, out var position, out var forward, out var up, out var right);
            Gizmos.DrawSphere(position, 0.02f);

            splineLength = length;
            if ((forceAlongSpline && drawForceGizmo) || (splineFriction && drawFrictionGizmo))
            {
                if (forceAlongSpline && drawForceGizmo && forceCurves.CountCheck(count => currentForceCurveIndex < count))
                {
                    Gizmos.color = Color.white;
                    GetSplinePointInfo(forceCurves[currentForceCurveIndex].splineForceTarget, out var target, out _, out _, out _);
                    Gizmos.DrawWireSphere(target + forceCurveGizmoDrawOffset, 0.1f);
                }
                CalculateSplineInfos();
                float stepSize = (1f / gizmoDrawSteps);
                float tCurrent = 0f;
                Vector3 current = Vector3.zero;
                float tNext = 0f;
                Vector3 next = Vector3.zero;
                GetSplinePointInfo(0f, out current, out _, out _, out _);
                for (int i = 0; i < gizmoDrawSteps; i++)
                {
                    tNext = tCurrent + stepSize;
                    GetSplinePointInfo(tNext, out next, out _, out _, out _);
                    float tBetween = tCurrent + (stepSize / 2f);
                    if (forceAlongSpline && drawForceGizmo && (forceCurves?.Count ?? 0) > 0)
                    {
                        Gizmos.color = Extensions.ThreeColorLerp(Color.red, Color.yellow, Color.green, Mathf.Clamp01(GetSplineForceMagnitudeAtNormalizedPoint(tBetween) / Mathf.Clamp(forceCurves[currentForceCurveIndex].maxForce, 0.00001f, Mathf.Infinity)));
                        Gizmos.DrawLine(current + forceCurveGizmoDrawOffset, next + forceCurveGizmoDrawOffset);
                    }
                    if (splineFriction && drawFrictionGizmo)
                    {
                        Gizmos.color = Extensions.ThreeColorLerp(Color.green, Color.yellow, Color.red, splineFrictionStrengthCurve.Evaluate(Mathf.Clamp01(tBetween) * length));
                        Gizmos.DrawLine(current + frictionCurveGizmoDrawOffset, next + frictionCurveGizmoDrawOffset);
                    }
                    tCurrent = tNext;
                    current = next;
                }
            }
        }
#endif
    }

    public static class Extensions
    {
        public static float GetFirstTime(this AnimationCurve animationCurve)
        {
            return (animationCurve.length == 0) ? 0 : animationCurve[0].time;
        }

        public static float GetLastTime(this AnimationCurve animationCurve)
        {
            return (animationCurve.length == 0) ? 0 : animationCurve[animationCurve.length - 1].time;
        }

        public static bool CountCheck(this ICollection collection, Func<int, bool> check)
        {
            if (collection == null) return false;
            return check(collection.Count);
        }

        public static bool IsNullOrEmpty(this ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static Color ThreeColorLerp(Color min, Color mid, Color max, float t) => ThreePointLerp(min, mid, max, t);

        public static Vector4 ThreePointLerp(Vector4 min, Vector4 mid, Vector4 max, float t) => MultiPointLerp(t, min, mid, max);

        public static Vector4 MultiPointLerp(float t, params Vector4[] orderedPoints)
        {
            t = Mathf.Clamp01(t);
            int zones = orderedPoints.Length - 1;
            float step = 1f / zones;
            int lower = Mathf.FloorToInt(zones * t);
            int upper = Mathf.CeilToInt(zones * t);
            return Vector4.Lerp(orderedPoints[lower], orderedPoints[upper], Mathf.InverseLerp(step * lower, step * upper, t));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering.Universal;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public static class Utils
    {
        static Vector3[] cardinalXYZ = new Vector3[] { Vector3.left, Vector3.right, Vector3.forward, Vector3.back, Vector3.up, Vector3.down };
        static Vector3[] cardinalXZ = new Vector3[] { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        static Vector3[] cardinalX = new Vector3[] { Vector3.left, Vector3.right };
        static Vector3[] cardinalZ = new Vector3[] { Vector3.forward, Vector3.back };

        /// <summary>
        /// Returns the smallest radius or axis of a collider
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static bool TryGetSmallestRadius(Collider collider, out float radius)
        {
            switch (collider)
            {
                case SphereCollider sphereCollider:
                    radius = sphereCollider.radius;
                    return true;
                case CapsuleCollider capsuleCollider: {
                    float capRadius = capsuleCollider.radius;
                    float height = capsuleCollider.height;
                    radius = Mathf.Min(capRadius, height / 2f);
                    return true;
                }
                case BoxCollider boxCollider: {
                    Vector3 size = boxCollider.size;
                    radius = Mathf.Min(size.x, size.y, size.z) / 2;
                    return true;
                }
                default:
                    radius = 0f;
                    return false;
            }
        }
        
        //Bit of reflection to get the SRD, try to not do this much.
        public static bool TryGetScriptableRendererData(int universalRendererDataIndex, out ScriptableRendererData scriptableRendererData)
        {
            var pipeline = ((UniversalRenderPipelineAsset)QualitySettings.renderPipeline);
            FieldInfo propertyInfo = pipeline.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
            ScriptableRendererData[] scriptableRendererDatas = ((ScriptableRendererData[])propertyInfo?.GetValue(pipeline));
            if (scriptableRendererDatas != null && universalRendererDataIndex < scriptableRendererDatas.Length)
            {
                scriptableRendererData = scriptableRendererDatas[universalRendererDataIndex];
                return true;
            }
            else
            {
                scriptableRendererData = null;
                return false;
            }
        }

        public static bool TryGetFeature(string name, ScriptableRendererData scriptableRendererData, out ScriptableRendererFeature feature)
        {
            feature = null;
            if (scriptableRendererData == null) return false;
            foreach (var rendererFeature in scriptableRendererData.rendererFeatures)
            {
                if (rendererFeature.name.Equals(name))
                {
                    feature = rendererFeature;
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Is the target collection null or empty?
        /// </summary>
        public static bool IsNullOrEmpty(this ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }

        /// <summary>
        /// Is the target array null or empty?
        /// </summary>
        public static bool IsNullOrEmpty(this Array array)
        {
            return array == null || array.Length == 0;
        }

        public static bool IsNullAndEmpty(this AnimationCurve animationCurve)
        {
            return animationCurve != null && animationCurve.keys.Length == 0;
        }

        public static void SetAndApplyGamma(this Texture2D tex, float gamma = 2.2f)
        {
            //IDEA: use raw pixel data
            float invVal = 1 / gamma;
            Color[] colors = tex.GetPixels();
            for (int i = 0; i < colors.Length; i++)
                colors[i] = new Color(
                    Mathf.Pow(colors[i].r, invVal),
                    Mathf.Pow(colors[i].g, invVal),
                    Mathf.Pow(colors[i].b, invVal),
                    colors[i].a);
            tex.SetPixels(colors);
            tex.Apply();
        }

        public static string ToDetailedString(this Vector3 v)
        {
            return System.String.Format("<{0}, {1}, {2}>", v.x, v.y, v.z);
        }

        public static float RoundDownToNearest(float value, float roundto)
        {
            if (roundto == 0)
            {
                return value;
            }
            else
            {
                return Mathf.Floor(value / roundto) * roundto;
            }
        }

        public static Vector2 GetAngles(Transform reference, Vector3 targetPosition)
        {
            Vector3 targetDirection = targetPosition - reference.position;
            targetDirection = reference.InverseTransformDirection(targetDirection);
            float angleOnX = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
            float angleOnY = Mathf.Atan2(targetDirection.y, targetDirection.z) * Mathf.Rad2Deg;
            return new Vector2(angleOnX, angleOnY);
        }

        public static Quaternion LookRotation(Vector3 direction, Vector3 localDirection, Vector3 upward)
        {
            return Quaternion.LookRotation(direction, upward) * Quaternion.FromToRotation(Vector3.forward, -localDirection);
        }

        public static Quaternion QuaternionFromTo(Quaternion from, Quaternion to) => from.To(to);

        public static Vector3 ClampMagnitude(Vector3 v, float max, float min)
        {
            double sm = v.sqrMagnitude;
            if (sm > (double)max * (double)max)
                return v.normalized * max;
            else if (sm < (double)min * (double)min)
                return v.normalized * min;
            return v;
        }

        public static Vector3 GetLinearVelocityAtPoint(Rigidbody rigidbody, Vector3 position, Vector3 velocity, Vector3 angularVelocity)
        {
            Vector3 worldCenterOfMass = rigidbody.transform.TransformPoint(rigidbody.centerOfMass);
            Vector3 linearVelocity = Vector3.Cross(angularVelocity, position - worldCenterOfMass);
            return (linearVelocity + velocity);
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

        public static string GetTransformPath(Transform root, Transform transform)
        {
            string path = transform.name;
            while (transform.parent != root)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }
            return path;
        }

        public static AxisDirection GetAxisDirection(Vector2 axis, float deadzone = 0)
        {
            if (axis.magnitude > deadzone)
            {
                float axisAngle = GetAxisAngle(axis);
                if (axisAngle > 315 || axisAngle <= 45)
                    return AxisDirection.Up;
                else if (axisAngle > 45 && axisAngle <= 135)
                    return AxisDirection.Right;
                else if (axisAngle > 135 && axisAngle <= 225)
                    return AxisDirection.Down;
                else if (axisAngle > 225 && axisAngle <= 315)
                    return AxisDirection.Left;
            }
            return AxisDirection.None;
        }

        public static float GetAxisAngle(Vector2 axis)
        {
            float angle = Mathf.Atan2(axis.y, axis.x) * Mathf.Rad2Deg;
            angle = 90.0f - angle;
            if (angle < 0)
            {
                angle += 360.0f;
            }
            return angle;
        }

        public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }

        public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
        {
            float startVolume = 0.2f;

            audioSource.volume = 0;
            audioSource.Play();

            while (audioSource.volume < 1.0f)
            {
                audioSource.volume += startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.volume = 1f;
        }

        public static bool RadiusTouching(Vector3 pointA, float radiusA, Vector3 pointB, float radiusB) => pointA.PointInRadius(pointB, radiusA + radiusB);
        public static bool ClosestPointsOnTwoLines(Vector3 lineStart1, Vector3 lineEnd1, Vector3 lineStart2, Vector3 lineEnd2, out Vector3 closestPointLine1, out Vector3 closestPointLine2)
        {
            // Find closest point on two infinite lines
            Vector3 lineVec1 = (lineEnd1 - lineStart1).normalized;
            Vector3 lineVec2 = (lineEnd2 - lineStart2).normalized;
            closestPointLine1 = Vector3.zero;
            closestPointLine2 = Vector3.zero;
            float a = Vector3.Dot(lineVec1, lineVec1);
            float b = Vector3.Dot(lineVec1, lineVec2);
            float e = Vector3.Dot(lineVec2, lineVec2);
            float d = a * e - b * b;
            if (d != 0.0f)
            {
                //lines are not parallel
                Vector3 r = lineStart1 - lineStart2;
                float c = Vector3.Dot(lineVec1, r);
                float f = Vector3.Dot(lineVec2, r);
                float s = (b * f - c * e) / d;
                float t = (a * f - c * b) / d;
                closestPointLine1 = lineStart1 + lineVec1 * Mathf.Clamp(s, 0, Vector3.Distance(lineStart1, lineEnd1));
                closestPointLine2 = lineStart2 + lineVec2 * Mathf.Clamp(t, 0, Vector3.Distance(lineStart2, lineEnd2));
                Vector3 closest1 = ClosestPointOnLine(lineStart2, lineEnd2, closestPointLine1);
                Vector3 closest2 = ClosestPointOnLine(lineStart1, lineEnd1, closestPointLine2);
                if ((closestPointLine1 - closest1).sqrMagnitude < (closestPointLine2 - closest2).sqrMagnitude)
                {
                    closestPointLine2 = closest1;
                }
                else
                {
                    closestPointLine1 = closest2;
                }
                return true;
            }
            else
            {
                // Lines are parallel (workaround more or less good)
                Vector3 lineCenter1 = Vector3.Lerp(lineStart1, lineEnd1, 0.5f);
                Vector3 lineCenter2 = Vector3.Lerp(lineStart2, lineEnd2, 0.5f);
                Matrix4x4 matrix2 = Matrix4x4.TRS(lineCenter2, Quaternion.LookRotation(lineVec2), Vector3.one).inverse;
                Vector3 centerLocalPos2 = matrix2.MultiplyPoint3x4(lineCenter1);
                float lenght2 = Vector3.Distance(lineStart2, lineEnd2);
                closestPointLine1 = lineCenter1;
                closestPointLine2 = ClosestPointOnLine(lineStart2, lineEnd2, lineCenter1);
                if (centerLocalPos2.z > lenght2 * 0.5f)
                {
                    closestPointLine1 = ClosestPointOnLine(lineStart1, lineEnd1, lineEnd2);
                    closestPointLine2 = lineEnd2;
                }
                else if (centerLocalPos2.z < -lenght2 * 0.5f)
                {
                    closestPointLine1 = ClosestPointOnLine(lineStart1, lineEnd1, lineStart2);
                    closestPointLine2 = lineStart2;
                }
                return false;
            }
        }

        public static Vector3 ClosestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            var line = (lineEnd - lineStart);
            var len = line.magnitude;
            line.Normalize();
            var v = point - lineStart;
            var d = Vector3.Dot(v, line);
            d = Mathf.Clamp(d, 0f, len);
            return lineStart + line * d;
        }

        public static float InverseLerpVector3(Vector3 a, Vector3 b, Vector3 value)
        {
            Vector3 AB = b - a;
            Vector3 AV = value - a;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }

        public static bool IsInside(this Vector3 point, BoxCollider boxCollider)
        {
            point = boxCollider.transform.InverseTransformPoint(point) - boxCollider.center;
            var size = boxCollider.size;
            float l_HalfX = (size.x * 0.5f);
            float l_HalfY = (size.y * 0.5f);
            float l_HalfZ = (size.z * 0.5f);
            return (point.x < l_HalfX && point.x > -l_HalfX &&
                    point.y < l_HalfY && point.y > -l_HalfY &&
                    point.z < l_HalfZ && point.z > -l_HalfZ);
        }

        public static bool IsInside(this Vector3 point, SphereCollider sphereCollider)
        {
            Vector3 center = sphereCollider.transform.TransformPoint(sphereCollider.center);
            return Vector3.Distance(center, point) < sphereCollider.radius;
        }

        public static bool IsInside(this Vector3 point, CapsuleCollider capsuleCollider)
        {
            Transform colliderTransform = capsuleCollider.transform;
            float halfHeight = Mathf.Clamp(capsuleCollider.height * 0.5f - capsuleCollider.radius, 0, Mathf.Infinity);
            if (halfHeight > 0)
            {
                Vector3 lineHeight = new Vector3(capsuleCollider.direction == 0 ? halfHeight : 0, capsuleCollider.direction == 1 ? halfHeight : 0, capsuleCollider.direction == 2 ? halfHeight : 0);
                Vector3 colLineStart = colliderTransform.TransformPoint(capsuleCollider.center - lineHeight);
                Vector3 colLineEnd = colliderTransform.TransformPoint(capsuleCollider.center + lineHeight);
                Vector3 closestCenterOnLine = ClosestPointOnLine(colLineStart, colLineEnd, point);
                if (Vector3.Distance(closestCenterOnLine, point) < capsuleCollider.radius)
                {
                    return true;
                }
            }
            else
            {
                // Is spherical
                Vector3 center = colliderTransform.TransformPoint(capsuleCollider.center);
                if (Vector3.Distance(center, point) < capsuleCollider.radius)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool InsideCollider(Collider collider, Vector3 point)
        {
            switch (collider)
            {
                case CapsuleCollider capsuleCollider:
                    {
                        if (point.IsInside(capsuleCollider)) return true;
                        break;
                    }
                case SphereCollider sphereCollider:
                    {
                        if (point.IsInside(sphereCollider)) return true;
                        break;
                    }
                case BoxCollider boxCollider:
                    if (point.IsInside(boxCollider)) return true;
                    break;
                case MeshCollider meshCollider:
                case TerrainCollider terrainCollider:
                case WheelCollider wheelCollider:
                case CharacterController characterController:
                    break;
            }
            return false;
        }


        public static Vector3 ClosestPointOnSurface(Collider collider, Vector3 point, out bool isInside)
        {
            isInside = false;
            CapsuleCollider capsuleCollider = collider as CapsuleCollider;
            if (capsuleCollider)
            {
                float demiHeight = Mathf.Clamp(capsuleCollider.height * 0.5f - capsuleCollider.radius, 0, Mathf.Infinity);
                if (demiHeight > 0)
                {
                    Vector3 lineHeight = new Vector3(capsuleCollider.direction == 0 ? demiHeight : 0, capsuleCollider.direction == 1 ? demiHeight : 0, capsuleCollider.direction == 2 ? demiHeight : 0);
                    Vector3 colLineStart = collider.transform.TransformPoint(capsuleCollider.center - lineHeight);
                    Vector3 colLineEnd = collider.transform.TransformPoint(capsuleCollider.center + lineHeight);
                    Debug.DrawLine(colLineStart, colLineEnd, Color.white);
                    Vector3 closestCenterOnLine = ClosestPointOnLine(colLineStart, colLineEnd, point);
                    if (Vector3.Distance(closestCenterOnLine, point) < capsuleCollider.radius)
                        isInside = true;
                    return closestCenterOnLine + (point - closestCenterOnLine).normalized * capsuleCollider.radius;
                }
                else
                {
                    // Is spherical
                    Vector3 center = collider.transform.TransformPoint(capsuleCollider.center);
                    if (Vector3.Distance(center, point) < capsuleCollider.radius)
                        isInside = true;
                    return center + (point - center).normalized * capsuleCollider.radius;
                }
            }
            else
            {
                SphereCollider sphereCollider = collider as SphereCollider;
                if (sphereCollider)
                {
                    Vector3 center = collider.transform.TransformPoint(sphereCollider.center);
                    if (Vector3.Distance(center, point) < capsuleCollider.radius)
                        isInside = true;
                    return center + (point - center).normalized * sphereCollider.radius;
                }
            }
            Debug.LogError("This collider type is not supported");
            return Vector3.zero;
        }

        public static void ClosestPointOnSurface(Collider collider, Vector3 lineStart, Vector3 lineStop, out Vector3 colliderPoint, out Vector3 linePoint, out bool isInside)
        {
            isInside = false;
            CapsuleCollider capsuleCollider = collider as CapsuleCollider;
            if (capsuleCollider)
            {
                float demiHeight = Mathf.Clamp(capsuleCollider.height * 0.5f - capsuleCollider.radius, 0, Mathf.Infinity);
                if (demiHeight > 0)
                {
                    Vector3 lineHeight = new Vector3(capsuleCollider.direction == 0 ? demiHeight : 0, capsuleCollider.direction == 1 ? demiHeight : 0, capsuleCollider.direction == 2 ? demiHeight : 0);
                    Vector3 colLineStart = collider.transform.TransformPoint(capsuleCollider.center - lineHeight);
                    Vector3 colLineEnd = collider.transform.TransformPoint(capsuleCollider.center + lineHeight);
                    Debug.DrawLine(colLineStart, colLineEnd, Color.white);
                    ClosestPointsOnTwoLines(lineStart, lineStop, colLineStart, colLineEnd, out linePoint, out colliderPoint);
                    if (Vector3.Distance(colliderPoint, linePoint) < capsuleCollider.radius)
                        isInside = true;
                    colliderPoint = colliderPoint + (linePoint - colliderPoint).normalized * capsuleCollider.radius;
                    return;
                }
                else
                {
                    // Is spherical
                    Vector3 center = collider.transform.TransformPoint(capsuleCollider.center);
                    linePoint = ClosestPointOnLine(lineStart, lineStop, center);
                    if (Vector3.Distance(linePoint, center) < capsuleCollider.radius)
                        isInside = true;
                    colliderPoint = center + (linePoint - center).normalized * capsuleCollider.radius;
                    return;
                }
            }
            else
            {
                SphereCollider sphereCollider = collider as SphereCollider;
                if (sphereCollider)
                {
                    Vector3 center = collider.transform.TransformPoint(sphereCollider.center);
                    linePoint = ClosestPointOnLine(lineStart, lineStop, center);
                    if (Vector3.Distance(linePoint, center) < sphereCollider.radius)
                        isInside = true;
                    colliderPoint = center + (linePoint - center).normalized * sphereCollider.radius;
                    return;
                }
            }
            Debug.LogError("This collider type is not supported");
            colliderPoint = Vector3.zero;
            linePoint = Vector3.zero;
            return;
        }
        public static Bounds GetCombinedBoundingBox(Transform root)
        {
            if (root == null)
            {
                throw new ArgumentException("The supplied transform was null");
            }

            var colliders = root.GetComponentsInChildren<Collider>();
            if (colliders.Length == 0)
            {
                throw new ArgumentException("The supplied transform " + root?.name + " does not have any children with colliders");
            }

            Bounds totalBBox = colliders[0].bounds;
            foreach (var collider in colliders)
            {
                totalBBox.Encapsulate(collider.bounds);
            }
            return totalBBox;
        }

        public static string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) || (preserveAcronyms && char.IsUpper(text[i - 1]) && i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }


        public static Color Rainbow(float progress)
        {
            float div = (Mathf.Abs(progress % 1) * 6);
            int ascending = (int)((div % 1) * 255);
            int descending = 255 - ascending;

            switch ((int)div)
            {
                case 0:
                    return FromArgb(255, 255, ascending, 0);
                case 1:
                    return FromArgb(255, descending, 255, 0);
                case 2:
                    return FromArgb(255, 0, 255, ascending);
                case 3:
                    return FromArgb(255, 0, descending, 255);
                case 4:
                    return FromArgb(255, ascending, 0, 255);
                default: // case 5:
                    return FromArgb(255, 255, 0, descending);
            }
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return FromArgb(255, v, t, p);
            else if (hi == 1)
                return FromArgb(255, q, v, p);
            else if (hi == 2)
                return FromArgb(255, p, v, t);
            else if (hi == 3)
                return FromArgb(255, p, q, v);
            else if (hi == 4)
                return FromArgb(255, t, p, v);
            else
                return FromArgb(255, v, p, q);
        }

        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            float fa = ((float)alpha) / 255.0f;
            float fr = ((float)red) / 255.0f;
            float fg = ((float)green) / 255.0f;
            float fb = ((float)blue) / 255.0f;
            return new Color(fr, fg, fb, fa);
        }

        public static Vector3 ClosestDirection(Vector3 direction, Cardinal cardinal)
        {
            float maxDot = -Mathf.Infinity;
            Vector3 ret = Vector3.zero;
            foreach (Vector3 dir in (cardinal == Cardinal.XYZ ? cardinalXYZ : cardinal == Cardinal.XZ ? cardinalXZ : cardinal == Cardinal.X ? cardinalX : cardinalZ))
            {
                var t = Vector3.Dot(direction, dir);
                if (t > maxDot)
                {
                    ret = dir;
                    maxDot = t;
                }
            }
            return ret;
        }

        public static void IgnoreLayerCollision(int layer, LayerMask layerMask)
        {
            for (int layerValue = 0; layerValue < 32; layerValue++)
            {
                if (LayerMask.LayerToName(layerValue) != "")
                {
                    Physics.IgnoreLayerCollision(layer, layerValue, false);
                    if (layerMask == (layerMask | (1 << layerValue)))
                    {
                        Physics.IgnoreLayerCollision(layer, layerValue, true);
                    }
                }
            }
        }

        public static float PercentageBetween(float start, float current, float end)
        {
            return ((current - start) / (end - start));
        }

        public static float CalculateRatio(float input, float inputMin, float inputMax, float outputMin, float outputMax)
        {
            //Making sure bounderies arent broken...
            if (input > inputMax)
            {
                input = inputMax;
            }
            if (input < inputMin)
            {
                input = inputMin;
            }
            //Return value in relation to min og max
            float position = (input - inputMin) / (inputMax - inputMin);
            float relativeValue = (position * (outputMax - outputMin)) + outputMin;
            return relativeValue;
        }
        public static LayerMask GetLayer(LayerType layerType)
        {
            return LayerMask.NameToLayer(layerType.ToString());
        }

        public static void SetPositiveLocalScale(Transform transform)
        {
            transform.transform.localScale = new Vector3(Mathf.Abs(transform.transform.localScale.x), Mathf.Abs(transform.transform.localScale.y), Mathf.Abs(transform.transform.localScale.z));
        }

        public static Component GetClosest(Vector3 position, Component[] others)
        {
            if (others.Length == 1)
                return others[0];
            Component bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            int count = others.Length;
            for (int i = 0; i < count; i++)
            {
                Component potentialTarget = others[i];
                if (TryGetCloserTarget(position, potentialTarget.transform.position, ref closestDistanceSqr))
                    bestTarget = potentialTarget;
            }
            return bestTarget;
        }

        /// <summary>
        /// This will return true if the distanceSqr between position and targetPosition is less than closestDistanceSqr and will update closestDistanceSqr with the new distanceSqr
        /// </summary>
        /// <param name="position"></param>
        /// <param name="targetPosition"></param>
        /// <param name="closestDistanceSqr"></param>
        /// <returns></returns>
        public static bool TryGetCloserTarget(Vector3 position, Vector3 targetPosition, ref float closestDistanceSqr)
        {
            float dSqrToTarget = (targetPosition - position).sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                return true;
            }
            return false;
        }

        public static Collider GetClosest(Vector3 position, Collider[] others, out Vector3 closestColliderPoint, int arrayLength = 0)
        {
            closestColliderPoint = Vector3.zero;
            if (others.Length == 1)
            {
                closestColliderPoint = others[0].ClosestPoint(position);
                return others[0];
            }
            Collider closestCollider = null;
            float closestDistanceSqr = Mathf.Infinity;
            for (int i = 0; i < (arrayLength < 1 ? others.Length : arrayLength); i++)
            {
                if (others[i] is MeshCollider && !(others[i] as MeshCollider).convex)
                    continue;
                Vector3 closestPoint = others[i].ClosestPoint(position);
                if (TryGetCloserTarget(position, closestPoint, ref closestDistanceSqr))
                {
                    closestColliderPoint = closestPoint;
                    closestCollider = others[i];
                }
            }
            return closestCollider;
        }

        public static MonoBehaviour GetClosest(Transform source, List<MonoBehaviour> others)
        {
            return GetClosest(source, others.ToArray());
        }
        public static MonoBehaviour GetClosest(Vector3 position, List<MonoBehaviour> others)
        {
            return GetClosest(position, others.ToArray());
        }
        public static MonoBehaviour GetClosest(Transform source, MonoBehaviour[] others)
        {
            return GetClosest(source.position, others as Component[]) as MonoBehaviour;
        }

        public static MonoBehaviour GetClosest(Vector3 position, MonoBehaviour[] others)
        {
            return GetClosest(position, others as Component[]) as MonoBehaviour;
        }

        public static List<Transform> GetClosestList(Transform source, List<Transform> others)
        {
            return others.OrderBy(o => (o.position - source.position).sqrMagnitude).ToList();
        }
        public static Vector3 RoundVector3(Vector3 vector3, int dp = 10)
        {
            return new Vector3(Mathf.Round(vector3.x * dp) / dp, Mathf.Round(vector3.y * dp) / dp, Mathf.Round(vector3.z * dp) / dp);
        }

        public static void MoveToFrontHead(Transform transf, Transform head, float distance, Transform finger = null)
        {
            if (finger)
            {
                Vector3 viewDirection = finger.position - head.position;
                transf.position = finger.position + (viewDirection.normalized * distance);
                transf.rotation = Quaternion.LookRotation(viewDirection);
            }
            else
            {
                transf.position = head.position + (head.forward.normalized * distance);
                transf.rotation = Quaternion.LookRotation(head.forward);
            }
        }

        public static CharacterJoint CloneCharacterJoint(CharacterJoint source, GameObject destination, Rigidbody newRigidbody = null)
        {
            CharacterJoint characterJoint = destination.AddComponent<CharacterJoint>();
            characterJoint.connectedBody = newRigidbody ? newRigidbody : source.connectedBody;
            characterJoint.anchor = source.anchor;
            characterJoint.axis = source.axis;
            characterJoint.autoConfigureConnectedAnchor = source.autoConfigureConnectedAnchor;
            characterJoint.connectedAnchor = source.connectedAnchor;
            characterJoint.swingAxis = source.swingAxis;
            characterJoint.twistLimitSpring = source.twistLimitSpring;
            characterJoint.lowTwistLimit = source.lowTwistLimit;
            characterJoint.highTwistLimit = source.highTwistLimit;
            characterJoint.swingLimitSpring = source.swingLimitSpring;
            characterJoint.swing1Limit = source.swing1Limit;
            characterJoint.swing2Limit = source.swing2Limit;
            characterJoint.enableProjection = source.enableProjection;
            characterJoint.projectionDistance = source.projectionDistance;
            characterJoint.projectionAngle = source.projectionAngle;
            characterJoint.breakForce = source.breakForce;
            characterJoint.breakTorque = source.breakTorque;
            characterJoint.enableCollision = source.enableCollision;
            characterJoint.enablePreprocessing = source.enablePreprocessing;
            characterJoint.massScale = source.massScale;
            characterJoint.connectedMassScale = source.connectedMassScale;
            return characterJoint;
        }

        public static Component CopyComponent(Component original, GameObject destination)
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            // Copied fields can be restricted with BindingFlags
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy;
        }

        public static void CopyComponentFieldsValues(Component sourceComponent, Component destinationComponent, bool publicOnly, bool readOnlyAttribute)
        {
            foreach (KeyValuePair<System.Reflection.FieldInfo, object> fieldValue in GetComponentFieldsValues(sourceComponent))
            {
                if (publicOnly && !fieldValue.Key.IsPublic)
                    continue;
#if ODIN_INSPECTOR
                if (readOnlyAttribute && Attribute.IsDefined(fieldValue.Key, typeof(ReadOnlyAttribute)))
                    continue; 
#endif
                fieldValue.Key.SetValue(destinationComponent, fieldValue.Value);
            }
        }

        public static Dictionary<System.Reflection.FieldInfo, object> GetComponentFieldsValues(Component component)
        {
            Dictionary<System.Reflection.FieldInfo, object> fieldsValues = new Dictionary<System.Reflection.FieldInfo, object>();
            foreach (System.Reflection.FieldInfo field in component.GetType().GetFields())
            {
                fieldsValues.Add(field, field.GetValue(component));
            }
            return fieldsValues;
        }

        public static void SetComponentFieldsValues(Component component, Dictionary<System.Reflection.FieldInfo, object> fieldsValues)
        {
            foreach (KeyValuePair<System.Reflection.FieldInfo, object> fieldValue in fieldsValues)
            {
                fieldValue.Key.SetValue(component, fieldValue.Value);
            }
        }

        public static Vector3 TransformPointUnscaled(this Transform transform, Vector3 position)
        {
            var localToWorldMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            return localToWorldMatrix.MultiplyPoint3x4(position);
        }

        public static Vector3 InverseTransformPointUnscaled(this Transform transform, Vector3 position)
        {
            var worldToLocalMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).inverse;
            return worldToLocalMatrix.MultiplyPoint3x4(position);
        }

        public static AnimationCurve Curve(params float[] values)
        {
            var curve = new AnimationCurve();
            int i = 0;
            foreach (var value in values)
            {
                curve.AddKey(i / ((float)values.Length - 1), value);
                i++;
            }

            return curve;
        }

        public class CurveBuilder
        {
            public List<Keyframe> keys;

            public CurveBuilder() { keys = new List<Keyframe>(); }

            public CurveBuilder WithKey(float time, float value)
            {
                keys.Add(new Keyframe(time, value));
                return this;
            }

            public CurveBuilder WithKey(float time, float value, float inTangent, float outTangent)
            {
                keys.Add(new Keyframe(time, value, inTangent, outTangent));
                return this;
            }

            public AnimationCurve Build() => new AnimationCurve(keys.ToArray());
        }

        public static Gradient CreateGradient(Color start, Color end)
        {
            var gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(start, 0), new GradientColorKey(end, 1) },
                new GradientAlphaKey[] { new GradientAlphaKey(start.a, 0), new GradientAlphaKey(end.a, 1) }
            );
            return gradient;
        }

        public static string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

        public static string UnicodeToInternationalCharacters(string str)
        {
            var mc = Regex.Matches(str, "([\\w]+)|(\\\\u([\\w]{4}))");
            if (mc.Count <= 0) return str;

            var sb = new StringBuilder();
            foreach (Match m2 in mc)
            {
                var v = m2.Value;
                if (v.StartsWith("\\"))
                {
                    var word = v.Substring(2);
                    var codes = new byte[2];
                    var code = Convert.ToInt32(word.Substring(0, 2), 16);
                    var code2 = Convert.ToInt32(word.Substring(2), 16);
                    codes[0] = (byte)code2;
                    codes[1] = (byte)code;
                    sb.Append(Encoding.Unicode.GetString(codes));
                }
                else
                {
                    sb.Append(v);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Convert audio mixer volume decibels in the scale [-80,0] to [0-100]%
        /// </summary>
        /// <param name="decibels">Decibels = [-80,0]</param>
        /// <returns>[0,100] percentage value</returns>
        public static int DecibelsToPercentage(float decibels)
        {
            if (decibels <= -80)
            {
                return 0;
            }
            return (int)(Mathf.Pow(10f, decibels / 20f) * 100f);
        }

        /// <summary>
        /// Convert audio mixer volume slider value in the scale [0-100]% to [-80,0] decibels
        /// </summary>
        /// <param name="percentage">Percentage = [0,100]</param>
        /// <returns>[-80,0] decibel value</returns>
        public static float PercentageToDecibels(float percentage)
        {
            if (percentage <= 0) return -80;
            return 20 * Mathf.Log10(percentage / 100f);
        }

        /// <summary>
        /// Clone a directory.
        /// </summary>
        public static bool CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
            { return false; }

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath, true);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the flat distance (ignoring the y axs) between two positions. 
        /// </summary>
        public static float FlatDistance(Vector3 a, Vector3 b)
        {
            var flatA = a.ToXZ();
            var flatB = b.ToXZ();
            var heading = flatA - flatB;
            return heading.sqrMagnitude;
        }

        /// <summary>
        /// Returns true if this string is null, empty, or contains only whitespace.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>True if this string is null, empty, or contains only whitespace. Otherwise, it returns false.</returns>
        public static bool IsNullOrEmptyOrWhitespace(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                for (var index = 0; index < str.Length; ++index)
                {
                    if (!char.IsWhiteSpace(str[index])) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Remove the tab characters from a string
        /// </summary>
        /// <returns>The same text with the tab characters replaced by empty space characters.</returns>
        public static string RemoveTabs(string text)
        {
            var tab = '\u0009';
            return text.Replace(tab, ' ');
        }
    }
}

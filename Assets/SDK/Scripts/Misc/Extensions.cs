using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThunderRoad
{
    public static class Extensions
    {
        public static bool TryGetOrAddComponentInChildren<T>(this GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponentInChildren<T>();
            if (component == null)
            {   //add it to a child gameobject
                var holder = new GameObject();
                holder.transform.SetParent(gameObject.transform);
                component = holder.AddComponent<T>();
            }
            return component;
        }
        public static bool TryGetOrAddComponent<T>(this GameObject gameObject, out T component) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out component))
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static PhysicBody GetPhysicBodyInParent(this Component component) => component.gameObject.GetPhysicBodyInParent();

        public static PhysicBody GetPhysicBodyInParent(this GameObject gameObject)
        {
            PhysicBody physicBody = gameObject.GetPhysicBody();
            if (physicBody != null)
            {
                return physicBody;
            }
            Transform t = gameObject.transform;
            while (t.parent != null)
            {
                physicBody = t.parent.gameObject.GetPhysicBody();
                if (physicBody != null)
                {
                    return physicBody;
                }
                t = t.parent.transform;
            }
            return null;
        }

        public static PhysicBody[] GetPhysicBodiesInChildren(this Component component, bool includeInactive = false) => component.gameObject.GetPhysicBodiesInChildren(includeInactive);

        public static PhysicBody[] GetPhysicBodiesInChildren(this GameObject gameObject, bool includeInactive = false)
        {
            var rbs = gameObject.GetComponentsInChildren<Rigidbody>(includeInactive);
            int rbsLength = rbs.Length;
            var abs = gameObject.GetComponentsInChildren<ArticulationBody>(includeInactive);
            PhysicBody[] pbs = new PhysicBody[rbsLength + abs.Length];
            for (int i = 0; i < pbs.Length; i++)
            {
                if (i < rbsLength) pbs[i] = rbs[i].GetPhysicBody();
                else pbs[i] = abs[i - rbsLength].GetPhysicBody(); 
            }
            return pbs;
        }

        public static bool IsPhysicBody(this RaycastHit raycastHit, PhysicBody physicBody)
        {
            if (physicBody.isArticulationBody)
            {
                if (physicBody.articulationBody == raycastHit.articulationBody) return true;
            }
            else
            {
                if (physicBody.rigidBody == raycastHit.rigidbody) return true;
            }
            return false;
        }

        public static bool TryGetPhysicBody(this Collider collider, out PhysicBody physicBody)
        {
            if (collider.attachedRigidbody)
            {
                physicBody = new PhysicBody(collider.attachedRigidbody);
                return true;
            }
            if (collider.attachedArticulationBody)
            {
                physicBody = new PhysicBody(collider.attachedArticulationBody);
                return true;
            }
            physicBody = null;
            return false;
        }

        public static PhysicBody AsPhysicBody(this Rigidbody rb) => new PhysicBody(rb);

        public static PhysicBody AsPhysicBody(this ArticulationBody ab) => new PhysicBody(ab);

        public static PhysicBody GetPhysicBody(this Component component)
        {
            if (component is Rigidbody rb)
            {
                return new PhysicBody(rb);
            }
            if (component is ArticulationBody ab)
            {
                return new PhysicBody(ab);
            }
            if (component is Collider col)
            {
                if (col.attachedRigidbody != null) return col.attachedRigidbody.AsPhysicBody();
                if (col.attachedArticulationBody != null) return col.attachedArticulationBody.AsPhysicBody();
            }
            return component.gameObject.GetPhysicBody();
        }

        public static PhysicBody GetPhysicBody(this GameObject gameObject)
        {
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            if (rigidbody)
            {
                return new PhysicBody(rigidbody);
            }
            ArticulationBody articulationBody = gameObject.GetComponent<ArticulationBody>();
            if (articulationBody)
            {
                return new PhysicBody(articulationBody);
            }
            return null;
        }

        public static void SetConnectedPhysicBody(this Joint joint, PhysicBody physicBody)
        {
            if (physicBody.isArticulationBody)
            {
                joint.connectedArticulationBody = physicBody.articulationBody;
            }
            else
            {
                joint.connectedBody = physicBody.rigidBody;
            }
        }

        public static PhysicBody GetConnectedPhysicBody(this Joint joint)
        {
            if (joint.connectedArticulationBody)
            {
                return joint.connectedArticulationBody.gameObject.GetPhysicBody();
            }
            return joint.connectedBody.gameObject.GetPhysicBody();
        }

        /// <summary>
        /// This is a O(1) way of removing things from a list by moving them to the end
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="listCount"></param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveAtIgnoreOrder<T>(this IList<T> list, int index, int listCount)
        {
            int last = listCount - 1;
            if (last != index)
            {
                //copy the reference of the last one to the index we are removing
                //overwriting the one we want to remove
                list[index] = list[last];
            }
            //Remove the last thing in the list since its now a duplicate reference to list[index]
            list.RemoveAt(last);
        }
        
        /// <summary>
        /// This is a O(1) way of removing things from a list by moving them to the end
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveAtIgnoreOrder<T>(this IList<T> list, int index)
        {
            list.RemoveAtIgnoreOrder(index, list.Count);
        }


        public static List<T> Shuffle<T>(this List<T> list)
        {
            /// With the Fisher-Yates shuffle, we randomly sort elements. This is an accurate, effective shuffling method for all array types
            System.Random _random = new System.Random();
            T obj;
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int) (_random.NextDouble() * (n - i));
                obj = list[r];
                list[r] = list[i];
                list[i] = obj;
            }
            return list;
        }

        public static T[] Shuffle<T>(this T[] array)
        {
            /// With the Fisher-Yates shuffle, we randomly sort elements. This is an accurate, effective shuffling method for all array types
            System.Random _random = new System.Random();
            T obj;
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int) (_random.NextDouble() * (n - i));
                obj = array[r];
                array[r] = array[i];
                array[i] = obj;
            }
            return array;
        }

        public static void BlackAndWhiteSort<T>(this List<T> list, int count, Func<T, bool> validCheck, Action<T> validAction, Action<T> invalidAction) => BlackAndWhiteSort<T>(list, count, validCheck, validAction, invalidAction, out _);

        public static void BlackAndWhiteSort<T>(this List<T> list, int count, Func<T, bool> validCheck, Action<T> validAction, Action<T> invalidAction, out int validNumber)
        {
            int currentIndex = 0;
            int validIndex = 0;
            int invalidIndex = count - 1;
            // Black and white sort means all valid things get moved to the front, and all invalid things get moved to the back, we can safely say the array is sorted once the index (which only increases if nothing moves) is greater than the invalid index (which moves closer to 0 as the sort proceeds)
            while (currentIndex <= invalidIndex)
            {
                int moveIndex;
                T item = list[currentIndex];
                if (validCheck(item))
                {
                    // is valid. perform valid action (if any) then move on
                    if (validAction != null) validAction(item);
                    moveIndex = validIndex;
                    validIndex++;
                }
                else
                {
                    // is NOT valid. perform invalid action (if any) then move on
                    if (invalidAction != null) invalidAction(item);
                    moveIndex = invalidIndex;
                    invalidIndex--;
                }
                if (currentIndex != moveIndex)
                {
                    // move if needed
                    T move = list[moveIndex];
                    list[moveIndex] = list[currentIndex];
                    list[currentIndex] = move;
                }
                else
                {
                    // otherwise, move to the next thing
                    currentIndex++;
                }
            }
            validNumber = validIndex;
        }

        public static void BlackAndWhiteSort<T>(this T[] array, int count, Func<T, bool> validCheck, Action<T> validAction, Action<T> invalidAction) => BlackAndWhiteSort<T>(array, count, validCheck, validAction, invalidAction, out _);

        public static void BlackAndWhiteSort<T>(this T[] array, int count, Func<T, bool> validCheck, Action<T> validAction, Action<T> invalidAction, out int validNumber)
        {
            int currentIndex = 0;
            int validIndex = 0;
            int invalidIndex = count - 1;
            // Black and white sort means all valid things get moved to the front, and all invalid things get moved to the back, we can safely say the array is sorted once the index (which only increases if nothing moves) is greater than the invalid index (which moves closer to 0 as the sort proceeds)
            while (currentIndex <= invalidIndex)
            {
                int moveIndex;
                T item = array[currentIndex];
                if (validCheck(item))
                {
                    // is valid. perform valid action (if any) then move on
                    if (validAction != null) validAction(item);
                    moveIndex = validIndex;
                    validIndex++;
                }
                else
                {
                    // is NOT valid. perform invalid action (if any) then move on
                    if (invalidAction != null) invalidAction(item);
                    moveIndex = invalidIndex;
                    invalidIndex--;
                }
                if (currentIndex != moveIndex)
                {
                    // move if needed
                    T move = array[moveIndex];
                    array[moveIndex] = array[currentIndex];
                    array[currentIndex] = move;
                }
                else
                {
                    // otherwise, move to the next thing
                    currentIndex++;
                }
            }
            validNumber = validIndex;
        }

        public static byte[] Serialize<T>(this T source)
        {
            JsonSerializer ser = Catalog.GetJsonNetSerializer();
            using (MemoryStream s = new MemoryStream())
            {
                //serialize to the stream, leave the stream open
                using (var writer = new StreamWriter(stream: s, encoding: Encoding.UTF8, bufferSize: 4096))
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    ser.Serialize(jsonWriter, source);
                    jsonWriter.Flush();
                }

                return s.ToArray();
            }
        }
        
        public static T Deserialize<T>(this byte[] source)
        {
            JsonSerializer ser = Catalog.GetJsonNetSerializer();
            using (MemoryStream s = new MemoryStream(source))
            {
                //deserialize from the stream
                using (StreamReader reader = new StreamReader(s))
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    T output = ser.Deserialize<T>(jsonReader);
                    s.Dispose();
                    return output;
                }
            }
        }
        
        public static async Task<T> CloneJsonAsync<T>(this T source)
        {
            return await Task.Run(() => source.CloneJson());
        }

        public static T CloneJson<T>(this T source)
        {
            JsonSerializer ser = Catalog.GetJsonNetSerializer();
            using (MemoryStream s = new MemoryStream())
            {
                //serialize to the stream, leave the stream open
                using (var writer = new StreamWriter(stream: s, encoding: Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    ser.Serialize(jsonWriter, source);
                    jsonWriter.Flush();
                }
                //seek to the start of the stream
                s.Seek(0, SeekOrigin.Begin);
                //deserialize from the stream
                using (StreamReader reader = new StreamReader(s))
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    T output = ser.Deserialize<T>(jsonReader);
                    s.Dispose();
                    return output;
                }
            }
        }

        public static IEnumerator AsIEnumerator(this Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }
            if (task.IsFaulted)
            {
                throw task.Exception;
            }
        }

        /// <summary>
        /// Forces iteration through an IEnumerator, useful to make coroutines synchronous
        /// </summary>
        /// <param name="enumerator"></param>
        public static void AsSynchronous(this IEnumerator enumerator)
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is IEnumerator next)
                {
                    next.AsSynchronous();
                }
            }
        }

        public static bool IsApproximately(this float a, float b) => Mathf.Approximately(a, b);

        public static bool IsNormalized(this float f) => f >= 0f && f <= 1f;

        public static float DistanceSqr(this Vector3 a, Vector3 b)
        {
            return GetDistanceSqr(a, b);
        }
        
        /// <summary>
        /// Returns the Squared Distance between two vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Squared Distance</returns>
        public static float GetDistanceSqr(Vector3 a, Vector3 b)
        {
            float num1 = a.x - b.x;
            float num2 = a.y - b.y;
            float num3 = a.z - b.z;
            return num1 * num1 + num2 * num2 + num3 * num3;
        }
        
        public static bool PointInRadius(this Vector3 vectorA, Vector3 vectorB, float radius)
        {
            // Best performance to check radius
            float sqrMagnitude = vectorA.DistanceSqr(vectorB);
            if (sqrMagnitude < radius * radius)
            {
                return true;
            }
            return false;
        }

        public static bool PointInRadius(this Vector3 vectorA, Vector3 vectorB, float radius, out float radiusDistanceRatio)
        {
            // Best performance to check radius
            float sqrMagnitude = vectorA.DistanceSqr(vectorB);
            if (sqrMagnitude < radius * radius)
            {
                radiusDistanceRatio = 1 - (sqrMagnitude / (radius * radius));
                return true;
            }
            radiusDistanceRatio = 0;
            return false;
        }

        public static (Vector3 inDirection, Vector3 notInDirection) ProjectAndSubtract(this Vector3 vector, Vector3 normal)
        {
            (Vector3 inDirection, Vector3 notInDirection) result = (Vector3.Project(vector, normal), Vector3.zero);
            result.notInDirection = vector - result.inDirection;
            return result;
        }

        public static Vector3 GetClosestPoint(this Vector3 origin, params Vector3[] checkPoints)
        {
            Vector3 closest = checkPoints[0];
            for (int i = 1; i < checkPoints.Length; i++)
            {
                if ((checkPoints[i] - origin).sqrMagnitude < (closest - origin).sqrMagnitude) closest = checkPoints[i];
            }
            return closest;
        }

        public static T GetClosestObject<T>(this Vector3 origin, params T[] objects) where T : Component
        {
            T closest = objects[0];
            for (int i = 1; i < objects.Length; i++)
            {
                if ((objects[i].transform.position - origin).sqrMagnitude < (closest.transform.position - origin).sqrMagnitude) closest = objects[i];
            }
            return closest;
        }

        public static Vector3 GetFurthestPoint(this Vector3 origin, params Vector3[] checkPoints)
        {
            Vector3 furthest = checkPoints[0];
            for (int i = 1; i < checkPoints.Length; i++)
            {
                if ((checkPoints[i] - origin).sqrMagnitude > (furthest - origin).sqrMagnitude) furthest = checkPoints[i];
            }
            return furthest;
        }

        public static T GetFurthestObject<T>(this Vector3 origin, params T[] objects) where T : Component
        {
            T furthest = objects[0];
            for (int i = 1; i < objects.Length; i++)
            {
                if ((objects[i].transform.position - origin).sqrMagnitude > (furthest.transform.position - origin).sqrMagnitude) furthest = objects[i];
            }
            return furthest;
        }

        public static bool HasFlagNoGC(this EffectModule.PlatformFilter flags, EffectModule.PlatformFilter value)
        {
            return ((flags & value) > 0);
        }

        public static bool HasFlagNoGC(this BuildSettings.ContentFlag flags, BuildSettings.ContentFlag value)
        {
            return ((flags & value) > 0);
        }

        public static bool CheckContentActive(this BuildSettings.ContentFlag flags, BuildSettings.ContentFlag content)
        {
            if (flags == BuildSettings.ContentFlag.None) return false;
            if (content == BuildSettings.ContentFlag.None) return true;
            return (flags & content) == content;
        }

        public static BuildSettings.ContentFlag AsRealFlag(this BuildSettings.SingleContentFlag flag)
        {
            return (BuildSettings.ContentFlag)(1 << (int)flag);
        }

        public static Collider Clone(this Collider collider, GameObject gameObject)
        {
            if (collider is SphereCollider) Clone(collider as SphereCollider, gameObject);
            else if (collider is CapsuleCollider) Clone(collider as CapsuleCollider, gameObject);
            else if (collider is BoxCollider) Clone(collider as BoxCollider, gameObject);
            else if (collider is MeshCollider) Clone(collider as MeshCollider, gameObject);
            return null;
        }

        public static SphereCollider Clone(this SphereCollider collider, GameObject gameObject)
        {
            SphereCollider clonedCollider = gameObject.AddComponent<SphereCollider>();
            clonedCollider.center = collider.center;
            clonedCollider.radius = collider.radius;
            clonedCollider.sharedMaterial = collider.sharedMaterial;
            clonedCollider.isTrigger = collider.isTrigger;
            return clonedCollider;
        }

        public static CapsuleCollider Clone(this CapsuleCollider collider, GameObject gameObject)
        {
            CapsuleCollider clonedCollider = gameObject.AddComponent<CapsuleCollider>();
            clonedCollider.center = collider.center;
            clonedCollider.radius = collider.radius;
            clonedCollider.height = collider.height;
            clonedCollider.sharedMaterial = collider.sharedMaterial;
            clonedCollider.isTrigger = collider.isTrigger;
            return clonedCollider;
        }

        public static BoxCollider Clone(this BoxCollider collider, GameObject gameObject)
        {
            BoxCollider clonedCollider = gameObject.AddComponent<BoxCollider>();
            clonedCollider.center = collider.center;
            clonedCollider.size = collider.size;
            clonedCollider.sharedMaterial = collider.sharedMaterial;
            clonedCollider.isTrigger = collider.isTrigger;
            return clonedCollider;
        }

        public static MeshCollider Clone(this MeshCollider collider, GameObject gameObject)
        {
            MeshCollider clonedCollider = gameObject.AddComponent<MeshCollider>();
            clonedCollider.convex = collider.convex;
            clonedCollider.cookingOptions = collider.cookingOptions;
            clonedCollider.sharedMesh = collider.sharedMesh;
            clonedCollider.sharedMaterial = collider.sharedMaterial;
            clonedCollider.isTrigger = collider.isTrigger;
            return clonedCollider;
        }

        public static void FocusPoints(this CapsuleCollider capsule, out Vector3 pointA, out Vector3 pointB)
        {
            float halfHeightMinusRadius = (capsule.height / 2f) - capsule.radius;
            if (halfHeightMinusRadius <= 0f)
            {
                pointA = pointB = capsule.transform.TransformPoint(capsule.center);
                return;
            }
            Vector3 capsuleDir = new Vector3(capsule.direction == 0 ? 1f : 0f, capsule.direction == 1 ? 1f : 0f, capsule.direction == 2 ? 1f : 0f);
            pointA = capsule.transform.TransformPoint(capsule.center + (capsuleDir * halfHeightMinusRadius));
            pointB = capsule.transform.TransformPoint(capsule.center + (capsuleDir * -halfHeightMinusRadius));
        }

        public static float GetFirstValue(this AnimationCurve animationCurve)
        {
            return (animationCurve.length == 0) ? 0 : animationCurve[0].value;
        }

        public static float GetLastValue(this AnimationCurve animationCurve)
        {
            return (animationCurve.length == 0) ? 0 : animationCurve[animationCurve.length - 1].value;
        }

        public static float GetFirstTime(this AnimationCurve animationCurve)
        {
            return (animationCurve.length == 0) ? 0 : animationCurve[0].time;
        }

        public static float GetLastTime(this AnimationCurve animationCurve)
        {
            return (animationCurve.length == 0) ? 0 : animationCurve[animationCurve.length - 1].time;
        }

        public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }

        public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
        {
            float multiplier = 1;
            for (int i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }
            return new Vector3(Mathf.Round(vector3.x * multiplier) / multiplier, Mathf.Round(vector3.y * multiplier) / multiplier, Mathf.Round(vector3.z * multiplier) / multiplier);
        }

        public static float SignedAngleFromDirection(this Vector3 fromdir, Vector3 todir, Vector3 referenceup)
        {
            // calculates the the angle between two direction vectors, with a referenceup a sign in which direction it points can be calculated (clockwise is positive and counter clockwise is negative)
            Vector3 planenormal = Vector3.Cross(fromdir, todir); // calculate the planenormal (perpendicular vector)
            float angle = Vector3.Angle(fromdir, todir); // calculate the angle between the 2 direction vectors (note: its always the smaller one smaller than 180°)
            float orientationdot = Vector3.Dot(planenormal, referenceup); // calculate wether the normal and the referenceup point in the same direction (>0) or not (<0), http://docs.unity3d.com/Documentation/Manual/ComputingNormalPerpendicularVector.html
            if (orientationdot > 0.0f) // the angle is positive (clockwise orientation seen from referenceup)
                return angle;
            return -angle; // the angle is negative (counter-clockwise orientation seen from referenceup)
        }

        public static Vector3 ToXZ(this Vector3 fromdir)
        {
            fromdir.y = 0;
            return fromdir;
        }

        public static Vector3 ToYZ(this Vector3 fromdir)
        {
            fromdir.x = 0;
            return fromdir;
        }

        public static Vector3 ClampMagnitude(this Vector3 vector, float minMagnitude, float maxMagnitude)
        {
            if (vector.sqrMagnitude < minMagnitude * minMagnitude) return vector.normalized * minMagnitude;
            if (vector.sqrMagnitude > maxMagnitude * maxMagnitude) return vector.normalized * maxMagnitude;
            return vector;
        }

        public static Transform FindOrAddTransform(this Transform parent, string name, Vector3 position, Quaternion? rotation = null, Vector3? scale = null)
        {
            Transform returnable = parent.Find(name);
            if (!returnable)
            {
                returnable = new GameObject(name).transform;
                returnable.parent = parent;
                returnable.position = position;
                returnable.rotation = rotation != null ? rotation.Value : Quaternion.identity;
                returnable.localScale = scale != null ? scale.Value : Vector3.one;
            }
            return returnable;
        }

        public static Quaternion To(this Quaternion from, Quaternion to)
        {
            return to * Quaternion.Inverse(from);
        }

        /// <summary>
        /// Sets a joint's targetRotation to match a given local rotation.
        /// The joint transform's local rotation must be cached on Start and passed into this method.
        /// </summary>
        public static void SetTargetRotationLocal(this ConfigurableJoint joint, Quaternion targetLocalRotation, Quaternion startLocalRotation)
        {
            if (joint.configuredInWorldSpace)
            {
                Debug.LogError("SetTargetRotationLocal should not be used with joints that are configured in world space. For world space joints, use SetTargetRotation.", joint);
            }
            SetTargetRotationInternal(joint, targetLocalRotation, startLocalRotation, Space.Self);
        }

        /// <summary>
        /// Sets a joint's targetRotation to match a given world rotation.
        /// The joint transform's world rotation must be cached on Start and passed into this method.
        /// </summary>
        public static void SetTargetRotation(this ConfigurableJoint joint, Quaternion targetWorldRotation, Quaternion startWorldRotation)
        {
            if (!joint.configuredInWorldSpace)
            {
                Debug.LogError("SetTargetRotation must be used with joints that are configured in world space. For local space joints, use SetTargetRotationLocal.", joint);
            }
            SetTargetRotationInternal(joint, targetWorldRotation, startWorldRotation, Space.World);
        }

        static void SetTargetRotationInternal(ConfigurableJoint joint, Quaternion targetRotation, Quaternion startRotation, Space space)
        {
            // Calculate the rotation expressed by the joint's axis and secondary axis
            var right = joint.axis;
            var forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
            var up = Vector3.Cross(forward, right).normalized;
            Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);

            // Transform into world space
            Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);

            // Counter-rotate and apply the new local rotation.
            // Joint space is the inverse of world space, so we need to invert our value
            if (space == Space.World)
            {
                resultRotation *= startRotation * Quaternion.Inverse(targetRotation);
            }
            else
            {
                resultRotation *= Quaternion.Inverse(targetRotation) * startRotation;
            }

            // Transform back into joint space
            resultRotation *= worldToJointSpace;

            // Set target rotation to our newly calculated rotation
            joint.targetRotation = resultRotation;
        }

        public static void SetAnimatorDefault(this Animator animator, bool keepState = true)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                // Skip any parameters controlled by curves
                if (animator.IsParameterControlledByCurve(param.nameHash)) continue;
                switch (param.type)
                {
                    case AnimatorControllerParameterType.Trigger:
                        animator.ResetTrigger(param.nameHash);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        animator.SetBool(param.nameHash, param.defaultBool);
                        break;
                    case AnimatorControllerParameterType.Int:
                        animator.SetInteger(param.nameHash, param.defaultInt);
                        break;
                    case AnimatorControllerParameterType.Float:
                        animator.SetFloat(param.nameHash, param.defaultFloat);
                        break;
                }
            }
            animator.Update(0f);
            animator.keepAnimatorStateOnDisable = keepState;
        }
        
        public static bool IsStatic(this MemberInfo member)
        {
            if (member is null) return false;
            switch (member)
            {
                case FieldInfo fieldInfo:
                    return fieldInfo.IsStatic;
                case PropertyInfo propertyInfo:
                    return !propertyInfo.CanRead ? propertyInfo.GetSetMethod(true).IsStatic : propertyInfo.GetGetMethod(true).IsStatic;
                case MethodBase methodBase:
                    return methodBase.IsStatic;
                case EventInfo eventInfo:
                    return eventInfo.GetRaiseMethod(true).IsStatic;
                case Type type:
                    return type.IsSealed && type.IsAbstract;
                default:
                    throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Unable to determine IsStatic for member {0}.{1}MemberType was {2} but only fields, properties, methods, events and types are supported.", (object) member.DeclaringType.FullName, (object) member.Name, (object) member.GetType().FullName));
            }
        }
    }
}

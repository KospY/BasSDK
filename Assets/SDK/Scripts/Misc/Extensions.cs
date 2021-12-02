using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ThunderRoad
{
    public static class Extensions
    {
        public static List<T> Shuffle<T>(this List<T> list) where T : UnityEngine.Object
        {
            /// With the Fisher-Yates shuffle, we randomly sort elements. This is an accurate, effective shuffling method for all array types
            System.Random _random = new System.Random();
            T obj;
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int)(_random.NextDouble() * (n - i));
                obj = list[r];
                list[r] = list[i];
                list[i] = obj;
            }
            return list;
        }

        public static T[] Shuffle<T>(this T[] array) where T : UnityEngine.Object
        {
            /// With the Fisher-Yates shuffle, we randomly sort elements. This is an accurate, effective shuffling method for all array types
            System.Random _random = new System.Random();
            T obj;
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int)(_random.NextDouble() * (n - i));
                obj = array[r];
                array[r] = array[i];
                array[i] = obj;
            }
            return array;
        }

        public static T CloneJson<T>(this T source)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
            jsonSerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
            string json = JsonConvert.SerializeObject(source, typeof(T), jsonSerializerSettings);
            return JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);
        }

        public static bool PointInRadius(this Vector3 vectorA, Vector3 vectorB, float radius)
        {
            // Best performance to check radius
            Vector3 offset = vectorA - vectorB;
            float sqrMagnitude = offset.sqrMagnitude;
            if (sqrMagnitude < radius * radius)
            {
                return true;
            }
            return false;
        }

        public static bool PointInRadius(this Vector3 vectorA, Vector3 vectorB, float radius, out float radiusDistanceRatio)
        {
            // Best performance to check radius
            Vector3 offset = vectorA - vectorB;
            float sqrMagnitude = offset.sqrMagnitude;
            if (sqrMagnitude < radius * radius)
            {
                radiusDistanceRatio = 1 - (sqrMagnitude / (radius * radius));
                return true;
            }
            radiusDistanceRatio = 0;
            return false;
        }

        public static string FormatBytes(this long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

#if PrivateSDK
        public static bool HasFlagNoGC(this RagdollPart.Type flags, RagdollPart.Type value)
        {
            return ((flags & value) > 0);
        }

        public static bool HasFlagNoGC(this DamagerData.PenetrationTempModifier flags, DamagerData.PenetrationTempModifier value)
        {
            return ((flags & value) > 0);
        }

        public static bool HasFlagNoGC(this DamageModifierData.Modifier.TierFilter flags, DamageModifierData.Modifier.TierFilter value)
        {
            return ((flags & value) > 0);
        }

        public static bool HasFlagNoGC(this ColliderGroupData.Modifier.TierFilter flags, ColliderGroupData.Modifier.TierFilter value)
        {
            return ((flags & value) > 0);
        }

        public static bool HasFlagNoGC(this EffectModuleReveal.TypeFilter flags, EffectModuleReveal.TypeFilter value)
        {
            return ((flags & value) > 0);
        }

        public static bool HasFlagNoGC(this EffectModule.DamageTypeFilter flags, EffectModule.DamageTypeFilter value)
        {
            return ((flags & value) > 0);
        }

        public static bool HasFlagNoGC(this EffectModule.PenetrationFilter flags, EffectModule.PenetrationFilter value)
        {
            return ((flags & value) > 0);
        }

        public static bool HasFlagNoGC(this EffectModule.DamagerFilter flags, EffectModule.DamagerFilter value)
        {
            return ((flags & value) > 0);
        }

        public static bool HasFlagNoGC(this EffectModule.PlateformFilter flags, EffectModule.PlateformFilter value)
        {
            return ((flags & value) > 0);
        }

        public static bool HasFlagNoGC(this HandleRagdollData.ForceLiftCondition flags, HandleRagdollData.ForceLiftCondition value)
        {
            return ((flags & value) > 0);
        }
#endif

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
            Vector3 planenormal = Vector3.Cross(fromdir, todir);             // calculate the planenormal (perpendicular vector)
            float angle = Vector3.Angle(fromdir, todir);                     // calculate the angle between the 2 direction vectors (note: its always the smaller one smaller than 180°)
            float orientationdot = Vector3.Dot(planenormal, referenceup);    // calculate wether the normal and the referenceup point in the same direction (>0) or not (<0), http://docs.unity3d.com/Documentation/Manual/ComputingNormalPerpendicularVector.html
            if (orientationdot > 0.0f)                                         // the angle is positive (clockwise orientation seen from referenceup)
                return angle;
            return -angle;  // the angle is negative (counter-clockwise orientation seen from referenceup)
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
    }
}

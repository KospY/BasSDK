using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ThunderRoad
{
    public enum Side
    {
        Right,
        Left,
    }

    public enum LayerType
    {
        Touch,
        Body,
        Ragdoll,
        Object,
        Stair,
        Avatar,
        AvatarObject,
        FloatingHand,
        Damage,
        Creature,
        UI,
        Player,
    }

    public enum AxisDirection
    {
        None,
        Up,
        Down,
        Left,
        Right,
    }

    public enum HueColorName
    {
        Lime,
        Green,
        Aqua,
        Blue,
        Navy,
        Purple,
        Pink,
        Red,
        Orange,
        Yellow,
        White
    }

    public enum AudioMixerName
    {
        Master,
        Effect,
        Ambient,
        UI,
        Voice,
        Music,
        SlowMotion,
    }

    public enum LayerName
    {
        None,
        Default,
        PlayerLocomotion,
        BodyLocomotion,
        Touch,
        MovingObject,
        DroppedObject,
        NPCGrabbedObject,
        TouchObject,
        Avatar,
        NPC,
        FPVHide,
        LocomotionOnly,
        Ragdoll,
        PostProcess,
        PlayerHandAndFoot,
        PlayerLocomotionObject,
        LiquidFlow,
        Zone,
    }

    public enum Cardinal
    {
        XYZ,
        XZ,
        X,
        Z,
    }

    public enum TagFilter
    {
        AllExcept,
        NoneExcept,
    }

    public enum EffectTarget
    {
        None,
        Main,
        Secondary,
    }

    public enum EffectLink
    {
        Intensity,
        Speed,
    }

    public enum AnimEffectType
    {
        None,
        Shock,
        ShockDead,
        Burning,
        Choke,
    }

    public enum SavedValueID
    {
        Rack,
        Holder,
        PotionFill,
        Ammo,
    }

    public enum Finger
    {
        Thumb,
        Index,
        Middle,
        Ring,
        Little,
    }

    public enum EventTime
    {
        OnStart,
        OnEnd,
    }

    public enum DamageType
    {
        Pierce,
        Slash,
        Blunt,
        Energy,
    }

    [Serializable]
    public class CustomReference
    {
        public string name;
        public Transform transform;
    }

    public static class Common
    {
        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }

        public static string GetStringBetween(this string text, string start, string end)
        {
            int pFrom = text.IndexOf(start) + start.Length;
            int pTo = text.LastIndexOf(end);
            return text.Substring(pFrom, pTo - pFrom);
        }

        public static void SetParentOrigin(this Transform transform, Transform parent)
        {
            transform.parent = parent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void SetParentOrigin(this Transform transform, Transform parent, Vector3? localPosition = null, Quaternion? localRotation = null, Vector3? localScale = null)
        {
            transform.parent = parent;
            transform.localPosition = localPosition != null ? Vector3.zero : (Vector3)localPosition;
            transform.localRotation = localRotation != null ? Quaternion.identity : (Quaternion)localRotation;
            transform.localScale = localScale != null ? Vector3.one : (Vector3)localScale;
        }

        public static void MoveAlign(this Transform transform, Transform child, Transform target, Transform parent = null)
        {
            transform.MoveAlign(child, target.position, target.rotation, parent);
        }

        public static void MoveAlign(this Transform transform, Transform child, Vector3 targetPosition, Quaternion targetRotation, Transform parent = null)
        {
            // Align connector rotations
            Quaternion deltaRotation = targetRotation * Quaternion.Inverse(child.rotation);
            transform.transform.rotation = deltaRotation * transform.transform.rotation;
            // Align connector positions
            Vector3 displacement = targetPosition - child.position;
            transform.transform.position += displacement;
            // parenting
            if (parent) transform.transform.SetParent(parent, true);
        }

        public static void MirrorChilds(this Transform transform, Vector3 mirrorAxis)
        {
            foreach (Transform child in transform.GetComponentsInChildren<Transform>())
            {
                if (child == transform) continue;
                Transform orgParent = child.parent;
                Transform mirror = new GameObject("Mirror").transform;
                mirror.SetParent(orgParent, false);
                mirror.localPosition = Vector3.zero;
                mirror.localRotation = Quaternion.identity;
                mirror.localScale = Vector3.one;
                child.SetParent(mirror, true);
                mirror.localScale = Vector3.Scale(mirrorAxis, transform.localScale);
                child.SetParent(orgParent, true);
                if (UnityEngine.Application.isPlaying) GameObject.Destroy(mirror.gameObject);
                else GameObject.DestroyImmediate(mirror.gameObject);
                child.localScale = Vector3.Scale(mirrorAxis, transform.localScale);
            }
        }

        public static void MirrorRelativeToParent(this Transform transform, Vector3 mirrorAxis)
        {
            Transform root = new GameObject("MirrorRoot").transform;
            root.SetParent(transform.parent);
            root.localPosition = Vector3.zero;
            root.localRotation = Quaternion.identity;
            root.localScale = Vector3.one;
            transform.SetParent(root, true);
            root.MirrorChilds(mirrorAxis);
            transform.SetParent(root.parent, true);
            if (UnityEngine.Application.isPlaying) GameObject.Destroy(root.gameObject);
            else GameObject.DestroyImmediate(root.gameObject);
        }

        private static Hashtable hueColourValues = new Hashtable{
         { HueColorName.Lime,     new Color32( 166 , 254 , 0, 255 ) },
         { HueColorName.Green,     new Color32( 0 , 254 , 111, 255 ) },
         { HueColorName.Aqua,     new Color32( 0 , 201 , 254, 255 ) },
         { HueColorName.Blue,     new Color32( 0 , 122 , 254, 255 ) },
         { HueColorName.Navy,     new Color32( 60 , 0 , 254, 255 ) },
         { HueColorName.Purple, new Color32( 143 , 0 , 254, 255 ) },
         { HueColorName.Pink,     new Color32( 232 , 0 , 254, 255 ) },
         { HueColorName.Red,     new Color32( 254 , 9 , 0, 255 ) },
         { HueColorName.Orange, new Color32( 254 , 161 , 0, 255 ) },
         { HueColorName.Yellow, new Color32( 254 , 224 , 0, 255 ) },
         { HueColorName.White, new Color32( 255 , 255 , 255, 255 ) },
        };

        public static Color32 HueColourValue(HueColorName color)
        {
            return (Color32)hueColourValues[color];
        }

        public static void DrawGizmoArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, bool dim3 = false)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 forward = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
            Vector3 backward = Quaternion.LookRotation(direction) * Quaternion.Euler(180 - arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
            if (dim3)
            {
                Gizmos.DrawRay(pos + direction, forward * arrowHeadLength);
                Gizmos.DrawRay(pos + direction, backward * arrowHeadLength);
#if UNITY_EDITOR
                UnityEditor.Handles.color = color;
                UnityEditor.Handles.DrawWireDisc(pos + new Vector3(0, arrowHeadLength * (1 - Mathf.Cos(arrowHeadAngle * Mathf.Deg2Rad)), 0), direction, arrowHeadLength * Mathf.Sin(arrowHeadAngle * Mathf.Deg2Rad));
#endif
            }
        }
        public static void DrawGizmoCapsule(Vector3 pos, Vector3 direction, Color color, float capsuleLength, float capsuleRadius)
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(pos + (direction.normalized) * capsuleLength, capsuleRadius);
            Gizmos.DrawWireSphere(pos - (direction.normalized) * capsuleLength, capsuleRadius);

            Gizmos.DrawRay((Vector3.right) * capsuleRadius, (Vector3.up) * capsuleLength);
            Gizmos.DrawRay((Vector3.right) * capsuleRadius, (Vector3.down) * capsuleLength);

            Gizmos.DrawRay((Vector3.left) * capsuleRadius, (Vector3.up) * capsuleLength);
            Gizmos.DrawRay((Vector3.left) * capsuleRadius, (Vector3.down) * capsuleLength);

            Gizmos.DrawRay((Vector3.forward) * capsuleRadius, (Vector3.up) * capsuleLength);
            Gizmos.DrawRay((Vector3.forward) * capsuleRadius, (Vector3.down) * capsuleLength);

            Gizmos.DrawRay((Vector3.back) * capsuleRadius, (Vector3.up) * capsuleLength);
            Gizmos.DrawRay((Vector3.back) * capsuleRadius, (Vector3.down) * capsuleLength);
        }
    }
}
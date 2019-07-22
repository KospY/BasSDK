using System.Collections;
using UnityEngine;

namespace BS
{
    public enum Side
    {
        Right,
        Left,
    }

    public enum HueColorNames
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
        Yellow
    }

    public static class Common
    {
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

        public static void Mirror(this Transform transform, Vector3 mirrorAxis)
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
                GameObject.Destroy(mirror.gameObject);
                child.localScale = Vector3.Scale(mirrorAxis, transform.localScale);
            }
        }

        private static Hashtable hueColourValues = new Hashtable{
         { HueColorNames.Lime,     new Color32( 166 , 254 , 0, 255 ) },
         { HueColorNames.Green,     new Color32( 0 , 254 , 111, 255 ) },
         { HueColorNames.Aqua,     new Color32( 0 , 201 , 254, 255 ) },
         { HueColorNames.Blue,     new Color32( 0 , 122 , 254, 255 ) },
         { HueColorNames.Navy,     new Color32( 60 , 0 , 254, 255 ) },
         { HueColorNames.Purple, new Color32( 143 , 0 , 254, 255 ) },
         { HueColorNames.Pink,     new Color32( 232 , 0 , 254, 255 ) },
         { HueColorNames.Red,     new Color32( 254 , 9 , 0, 255 ) },
         { HueColorNames.Orange, new Color32( 254 , 161 , 0, 255 ) },
         { HueColorNames.Yellow, new Color32( 254 , 224 , 0, 255 ) },
        };

        public static Color32 HueColourValue(HueColorNames color)
        {
            return (Color32)hueColourValues[color];
        }

        public static void DrawGizmoArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
        public static void DrawGizmoCapsule(Vector3 pos, Vector3 direction, Color color, float capsuleLength, float capsuleRadius)
        {

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
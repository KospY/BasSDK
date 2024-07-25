namespace ThunderRoad
{
    using UnityEngine;

    public class AreaRotationHelper
    {
        public enum Face
        {
            Back = 0,
            Front = 2,
            Left = 1,
            Right = 3,
            Up = 4,
            Down = 5
        }

        public enum Rotation
        {
            Back = 2,
            Front = 0,
            Left = 3,
            Right = 1
        }

        public static Rotation RotateRotation(Rotation initial, Rotation rotation)
        {
            int rotatedRotation = (int)initial;
            rotatedRotation += (int)rotation;
            rotatedRotation %= 4;

            return (Rotation) rotatedRotation;
        }

        public static Face RotateFace(Face face, Rotation rotation)
        {
            if(face == Face.Up || face == Face.Down)
            {
                return face; // No rotation for up and down
            }

            int rotatedFace = (int)face;
            rotatedFace += (int)rotation;
            rotatedFace %= 4;
            return (Face)rotatedFace;
        }

        public static Face InvertRotateFace(Face face, Rotation rotation)
        {
            if (face == Face.Up || face == Face.Down)
            {
                return face; // No rotation for up and down
            }

            int rotatedFace = (int)face;
            rotatedFace -= (int)rotation;
            if(rotatedFace < 0)
            {
                rotatedFace += 4;
            }

            return (Face)rotatedFace;
        }

        public static Rotation GetRotationFromFace(Face face)
        {
            switch (face)
            {
                case Face.Front:
                    return Rotation.Front;

                case Face.Left:
                    return Rotation.Left;

                case Face.Right:
                    return Rotation.Right;

                case Face.Back:
                    return Rotation.Back;

                default:
                    return Rotation.Front;
            }
        }

        public static Face GetOppositeFace(Face face)
        {
            switch (face)
            {
                case Face.Back:
                    return Face.Front;

                case Face.Left:
                    return Face.Right;

                case Face.Front:
                    return Face.Back;

                case Face.Right:
                    return Face.Left;

                case Face.Down:
                    return Face.Up;

                case Face.Up:
                    return Face.Down;

                default:
                    return Face.Front;
            }
        }

        public static Quaternion RotateDirectionalLight(Quaternion directionalLightLocalRotation, Rotation AreaRotation)
        {
            switch (AreaRotation)
            {
                case Rotation.Front:
                    {
                        return directionalLightLocalRotation;
                    }

                case Rotation.Right:
                    {
                        Vector3 eulerAngle = directionalLightLocalRotation.eulerAngles;
                        eulerAngle.y += 90;
                        return Quaternion.Euler(eulerAngle);
                    }

                case Rotation.Back:
                    {
                        Vector3 eulerAngle = directionalLightLocalRotation.eulerAngles;
                        eulerAngle.y += 180;
                        return Quaternion.Euler(eulerAngle);
                    }

                case Rotation.Left:
                    {
                        Vector3 eulerAngle = directionalLightLocalRotation.eulerAngles;
                        eulerAngle.y -= 90;
                        return Quaternion.Euler(eulerAngle);
                    }
            }

            return Quaternion.identity;
        }

        public static Quaternion InverseRotateDirectionalLight(Quaternion directionalLightLocalRotation, Rotation AreaRotation)
        {
            switch (AreaRotation)
            {
                case Rotation.Front:
                    {
                        return directionalLightLocalRotation;
                    }

                case Rotation.Right:
                    {
                        Vector3 eulerAngle = directionalLightLocalRotation.eulerAngles;
                        eulerAngle.y -= 90; // Light rotation turns left inverse
                        return Quaternion.Euler(eulerAngle);
                    }

                case Rotation.Back:
                    {
                        Vector3 eulerAngle = directionalLightLocalRotation.eulerAngles;
                        eulerAngle.y += 180;
                        return Quaternion.Euler(eulerAngle);
                    }

                case Rotation.Left:
                    {
                        Vector3 eulerAngle = directionalLightLocalRotation.eulerAngles;
                        eulerAngle.y += 90; // Light rotation turns right inverse
                        return Quaternion.Euler(eulerAngle);
                    }
            }

            return Quaternion.identity;
        }

        public static Rotation GetRotationFromFaceToFace(Face faceStart, Face faceEnd)
        {
            int rotation = GetRotationFromFace(faceEnd) - GetRotationFromFace(faceStart);
            if(rotation < 0)
            {
                rotation += 4;
            }

            return (Rotation)rotation;
        }

        public static Quaternion GetRotationQuaternionFromRotation(Rotation rotation)
        {
            switch (rotation)
            {
                case Rotation.Front:
                    return Quaternion.identity;

                case Rotation.Back:
                    return Quaternion.Euler(Vector3.up * 180);

                case Rotation.Left:
                    return Quaternion.Euler(Vector3.up * (-90));

                case Rotation.Right:
                    return Quaternion.Euler(Vector3.up * 90);
            }

            return Quaternion.identity;
        }

        public static float GetRotationDegreeFromRotation(Rotation rotation)
        {
            switch (rotation)
            {
                case Rotation.Front:
                    return 0.0f;

                case Rotation.Back:
                    return 180.0f;

                case Rotation.Left:
                    return -90.0f;

                case Rotation.Right:
                    return 90.0f;
            }

            return 0.0f;
        }

        public static Quaternion GetRotationQuaternionFromFace(Face face)
        {
            switch (face)
            {
                case Face.Front:
                    return Quaternion.identity;

                case Face.Back:
                    return Quaternion.Euler(Vector3.up * 180);

                case Face.Left:
                    return Quaternion.Euler(Vector3.up * (-90));

                case Face.Right:
                    return Quaternion.Euler(Vector3.up * 90);

                case Face.Down:
                    return Quaternion.identity;

                case Face.Up:
                    return Quaternion.Euler(Vector3.forward * 180);
            }

            return Quaternion.identity;
        }

        public static Vector3 GetBoundsSizeFromRotation(Vector3 boundsSize, Rotation rotation)
        {
            switch (rotation)
            {
                case Rotation.Left:
                case Rotation.Right:
                    return new Vector3(boundsSize.z, boundsSize.y, boundsSize.x);

                case Rotation.Back:
                case Rotation.Front:
                default:
                    return boundsSize;
            } 
        }

        public static bool TryGetFaceFromQuaterion(Quaternion rotation, out Face face)
        {
            Vector3 forward = rotation * Vector3.forward;
            if(forward.y > 0.5f)
            {
                face = Face.Up;
                return true;
            }

            if(forward.y < -0.5f)
            {
                face = Face.Down;
                return true;
            }

            if (forward.x > 0.5f)
            {
                face = Face.Right;
                return true;
            }

            if (forward.x < -0.5f)
            {
                face = Face.Left;
                return true;
            }

            if (forward.z > 0.5f)
            {
                face = Face.Front;
                return true;
            }

            if (forward.z < -0.5f)
            {
                face = Face.Back;
                return true;
            }

            face = Face.Front;
            return false;
        }
    }
}

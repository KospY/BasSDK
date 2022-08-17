using System.Collections;
using UnityEngine;
#if ODIN_INSPECTOR
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/RotateThumbLatch")]
    public class RotateThumbLatch : MonoBehaviour
    {
        [Header("Thumb latch rotation")]
        public Transform transformToRotate; // Transform that will be rotated by the script
        public Vector3 localRotationFrontHandleOffset; // Local Euleurs offset, will be used by front handle's latch 
        public Vector3 localRotationBackHandleGoalOffset; // Local Euleurs offset, will be used by back handle's latch 
        public AnimationCurve rotationEasing; // Curve to ease the latch's rotation and pose blending
        public float timeToRotate = .1f; // Time to rotate from one offset to the other


        [Header("Latch translation")]
        public Transform latchToTranslate; // Transform that will be translated by the script
        public Vector3 localTranslationOffset; // Local position offset 
        public AnimationCurve translationEasing; // Curve to ease the latch's rotation and pose blending
        public float timeToTranslate = .1f; // Time to translate from one offset to the other


        [Header("Handles")]
        public Handle frontHandle; // Front handle of the door

        public Handle backHandle; // Back handle of the door


        // Rotates to front handle's goal in timeToMatch seconds
        public void RotateToFrontHandleGoal(float timeToMatch)
        {
        }

        // Rotates to back handle's goal in timeToMatch seconds
        public void RotateToBackHandleGoal(float timeToMatch)
        {
        }

        // Rotates to default rotation in timeToMatch seconds
        public void RotateToDefault(float timeToMatch)
        {
        }

        // Check if the handle is one of the front/back, and if it is, rotate and blends hand poses
        // Called as a callback from the HingeDrive script: player pressing latch button event
        public void Rotate(float angle, HingeDrive.HingeDriveSpeedState speedState, Handle handle)
        {
        }

        // Check if the handle is one of the front/back, and if it is, rotate and blends hand poses toward defaults
        // Called as a callback from the HingeDrive script: player pressing latch button event
        public void RotateToDefault(float angle, HingeDrive.HingeDriveSpeedState speedState, Handle handle)
        {
        }

        // Rotates to default rotation in timeToMatch seconds
        public void TranslateToDefault(float timeToMatch)
        {
        }

        // Translates to default position in timeToMatch seconds
        public void TranslateToGoal(float timeToMatch)
        {
        }

    }
}
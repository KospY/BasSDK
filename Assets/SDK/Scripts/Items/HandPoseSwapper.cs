using UnityEngine;
using System.Collections.Generic;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/HandPoseSwapper")]
    [AddComponentMenu("ThunderRoad/Hand Pose Swapper")]
    [RequireComponent(typeof(HandlePose))]
    public class HandPoseSwapper : MonoBehaviour
    {
        public List<string> alternateHandPoseIDs = new List<string>();

        public void SwapDefaultPose(int newIDIndex)
        {
        }

        public void SwapTargetPose(int newIDIndex)
        {
        }

    }
}

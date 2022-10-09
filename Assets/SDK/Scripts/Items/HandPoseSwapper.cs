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

        public void SetDefaultPoseByIndex(int newIDIndex) => SetDefaultPose(alternateHandPoseIDs[newIDIndex]);

        public void SetDefaultPose(string newPoseID)
        {
        }

        public void SetTargetPoseByIndex(int newIDIndex) => SetTargetPose(alternateHandPoseIDs[newIDIndex]);

        public void SetTargetPose(string newPoseID)
        {
        }
    }
}

using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/SpringForceEffect")]
    [AddComponentMenu("ThunderRoad/Effects/Spring force effect")]
    public class SpringForceEffect : MonoBehaviour
    {
        public SpringJoint springJoint;
        public string effectId;
        public float minForce = 1;
        public float maxForce = 5;
        public float connectedBodyMinSpeed = 0.5f;
        public float connectedBodyMaxSpeed = 3;

    }
}

using UnityEngine;
using UnityEditor;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ExposureTransitionZone")]
    [RequireComponent(typeof(BoxCollider))]
    public class ExposureTransitionZone : ExposureSetterZone
    {

        [Tooltip("The exposure to set when the player leaves zone via red portal.")]
        public float exposureOnExit = 0f;
        
    }
}

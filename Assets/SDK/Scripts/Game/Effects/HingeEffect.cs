using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Effects/Hinge effect")]
    public class HingeEffect : MonoBehaviour
    {
        public new HingeJoint hingeJoint;

    }
}

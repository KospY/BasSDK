using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class HingeEffect : MonoBehaviour
    {
        public new HingeJoint hingeJoint;

    }
}

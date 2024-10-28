using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [System.Serializable]
    public class FirearmPose : IdlePose
    {
        public override bool customID => false;

    }
}

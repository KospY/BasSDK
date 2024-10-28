
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [System.Serializable]
    public class FirearmAction : StanceNode
    {
        public override bool customID => false;

    }
}

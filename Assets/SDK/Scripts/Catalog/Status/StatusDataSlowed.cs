#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class StatusDataSlowed : StatusData
    {
        [BoxGroup("Slow Effect")]
        public float baseSlowMult = 0.3f;
    }

}
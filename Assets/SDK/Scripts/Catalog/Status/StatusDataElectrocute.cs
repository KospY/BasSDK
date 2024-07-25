#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad {
    public class StatusDataElectrocute : StatusData
    {
        [BoxGroup("Player Speeds")]
        public float playerSpeedModifier = 0.2f;
        [BoxGroup("Player Speeds")]
        public float chargeSpeedModifier = 0.2f;
    }
}

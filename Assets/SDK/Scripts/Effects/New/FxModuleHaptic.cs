using System.Collections;
using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/FxModuleHaptic")]
    public class FxModuleHaptic : FxModule
    {
        [Header("Haptics")] public Handle.HandSide handSide = Handle.HandSide.Both;
        public FxModuleAudio.PlayEvent playEvent = FxModuleAudio.PlayEvent.Play;
        public GameData.HapticClip clip;

        [Header("Curves")] public FxBlendCurves hapticIntensityCurve = new FxBlendCurves();

    }
}
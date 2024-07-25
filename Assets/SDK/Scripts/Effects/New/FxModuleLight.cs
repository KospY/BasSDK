using UnityEngine;
using System.Collections;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/FxModuleLight.html")]
    [RequireComponent(typeof(Light))]
    public class FxModuleLight : FxModule
    {
        public float duration = 0;
        public AnimationCurve intensityCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        public bool flicker;
        public float flickerIntensityReduction = 0.2f;
        public float flickerMinSpeed = 0.01f;
        public float flickerMaxSpeed = 0.1f;
#pragma warning disable 0109 // unity this.light is obsolete, we can override this
        protected new Light light;
#pragma warning restore 0109
        protected float orgLightIntensity;
        protected float currentLightIntensity;

    }
}

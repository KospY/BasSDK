using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/EffectLight.html")]
	[ExecuteInEditMode]
    public class EffectLight : Effect
    {
        public Light pointLight;
        public AnimationCurve intensityCurve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));
        public AnimationCurve rangeCurve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));
        public Gradient colorCurve = new Gradient();
        public float intensitySmoothFactor = 0;
        public float rangeSmoothFactor = 0;
        public float colorSmoothFactor = 0;
        public AnimationCurve flickerIntensityCurve = new AnimationCurve(new Keyframe(0, 0.5f), new Keyframe(1, 1.5f));
        public AnimationCurve flickerRateCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public float loopFadeDelay;
        public float playTime;
        public float stopTime;
        private bool stopping;
        private float stopIntensity;

        private float intensity;
        private float range;
        private Color color;
        private float targetIntensity;
        private float targetRange;
        private Color targetColor;

    }
}

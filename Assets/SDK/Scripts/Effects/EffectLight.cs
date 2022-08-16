using UnityEngine;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/EffectLight")]
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

        private void Awake()
        {
        }

        public override void Play()
        {
            base.Play();
            pointLight.enabled = true;
            playTime = Time.time;
            stopTime = Time.time;
            stopping = step != Step.Loop;
            stopIntensity = intensity;
        }

        public override void Stop()
        {
            pointLight.enabled = false;
        }

        public override void End(bool loopOnly = false)
        {
            stopIntensity = intensity;
            stopTime = Time.time;
            stopping = true;
        }

        protected override ManagedLoops ManagedLoops => ManagedLoops.Update;

        protected internal override void ManagedUpdate()
        {
            if (intensitySmoothFactor > 0)
                intensity = Mathf.Lerp(intensity, targetIntensity, Time.deltaTime * intensitySmoothFactor);
            if (rangeSmoothFactor > 0)
                range = Mathf.Lerp(range, targetRange, Time.deltaTime * rangeSmoothFactor);
            if (colorSmoothFactor > 0)
                color = Color.Lerp(color, targetColor, Time.deltaTime * colorSmoothFactor);

            pointLight.intensity = intensity * flickerIntensityCurve.Evaluate(Mathf.PerlinNoise(Time.time * flickerRateCurve.Evaluate(intensity), 0));
            pointLight.range = range;
            pointLight.color = color;

            float amount;
            if (stopping)
            {
                if ((amount = Time.time - stopTime) < loopFadeDelay)
                {
                    SetIntensity((1 - amount / loopFadeDelay) * stopIntensity);
                }
                else
                {
                    stopping = false;
                    pointLight.enabled = false;
                }
            }
        }

        public override void SetIntensity(float value, bool loopOnly = false)
        {
            base.SetIntensity(value, loopOnly);
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                targetIntensity = intensityCurve.Evaluate(value);
                if (intensitySmoothFactor == 0)
                {
                    intensity = targetIntensity;
                }
                targetColor = colorCurve.Evaluate(value);
                if (colorSmoothFactor == 0)
                {
                    color = targetColor;
                }
                targetRange = rangeCurve.Evaluate(value);
                if (rangeSmoothFactor == 0)
                {
                    range = targetRange;
                }
            }
        }

        public override void SetMainGradient(Gradient gradient)
        {
            base.SetMainGradient(gradient);
            colorCurve = gradient;
        }

        public override void Despawn()
        {
            base.Despawn();
            pointLight.enabled = false;
            InvokeDespawnCallback();
            if (Application.isPlaying)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
        }
    }
}

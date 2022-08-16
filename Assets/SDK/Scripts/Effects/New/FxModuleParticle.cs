using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
        [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/FxModuleParticle")]
	public class FxModuleParticle : FxModule
    {
        [Header("Emission Rate Over Time")]
#if ODIN_INSPECTOR
        [LabelText("Link")]
#endif
        public Link emissionRateOverTimeLink = Link.None;
#if ODIN_INSPECTOR
        [HideIf("emissionRateOverTimeLink", Link.None), LabelText("Curve")]
#endif
        public AnimationCurve emissionRateOverTimeCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
#if ODIN_INSPECTOR
        [HideIf("emissionRateOverTimeLink", Link.None), LabelText("Range")]
#endif
        public Vector2 emissionRateOverTimeRange = new Vector2(0, 10);
#if ODIN_INSPECTOR
        [HideIf("emissionRateOverTimeLink", Link.None), LabelText("Reduction")]
#endif
        public float emissionRateOverTimeReduction;


        [Header("Emission Rate Over Distance")]
#if ODIN_INSPECTOR
        [LabelText("Link")]
#endif
        public Link emissionRateOverDistanceLink = Link.None;
#if ODIN_INSPECTOR
        [HideIf("emissionRateOverDistanceLink", Link.None), LabelText("Curve")]
#endif
        public AnimationCurve emissionRateOverDistanceCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
#if ODIN_INSPECTOR
        [HideIf("emissionRateOverDistanceLink", Link.None), LabelText("Range")]
#endif
        public Vector2 emissionRateOverDistanceRange = new Vector2(0, 10);
#if ODIN_INSPECTOR
        [HideIf("emissionRateOverDistanceLink", Link.None), LabelText("Reduction")]
#endif
        public float emissionRateOverDistanceReduction;


        [Header("Life time")]
#if ODIN_INSPECTOR
        [LabelText("Link")]
#endif
        public Link lifeTimeLink = Link.None;
#if ODIN_INSPECTOR
        [HideIf("lifeTimeLink", Link.None), LabelText("Curve")]
#endif
        public AnimationCurve curveLifeTime;
#if ODIN_INSPECTOR
        [HideIf("lifeTimeLink", Link.None), LabelText("Range")]
#endif
        public Vector2 lifeTimeRange = new Vector2(0, 10);
#if ODIN_INSPECTOR
        [HideIf("lifeTimeLink", Link.None), LabelText("Random reduction")]
#endif
        public float lifeTimeReduction;


        [Header("Emission burst")]
#if ODIN_INSPECTOR
        [LabelText("Link")]
#endif
        public Link emissionBurstLink = Link.None;
#if ODIN_INSPECTOR
        [HideIf("emissionBurstLink", Link.None), LabelText("Curve")]
#endif
        public AnimationCurve emissionBurstCurve;
#if ODIN_INSPECTOR
        [HideIf("emissionBurstLink", Link.None), LabelText("RandomRange")]
#endif
        public short emissionBurstRandomRange;


        [Header("Scale")]
#if ODIN_INSPECTOR
        [LabelText("Link")]
#endif
        public Link scaleLink = Link.None;
#if ODIN_INSPECTOR
        [HideIf("scaleLink", Link.None), LabelText("Curve")]
#endif
        public AnimationCurve scaleCurve;
#if ODIN_INSPECTOR
        [HideIf("scaleLink", Link.None), LabelText("Weight")]
#endif
        public Vector3 scaleMultiplier = Vector3.one;


        [Header("Start speed")]
#if ODIN_INSPECTOR
        [LabelText("Link")]
#endif
        public Link startSpeedLink = Link.None;
#if ODIN_INSPECTOR
        [HideIf("startSpeedLink", Link.None), LabelText("Curve")]
#endif
        public AnimationCurve startSpeedLinkCurve;
#if ODIN_INSPECTOR
        [HideIf("emissionBurstLink", Link.None), LabelText("Min speed ratio"), Range(0,1)]
#endif
        public float startSpeedMinSpeedRatio = 1;

        protected new ParticleSystem particleSystem;
        protected ParticleSystem.MainModule particleSystemMain;
        protected ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve();

        private void OnValidate()
        {
            Awake();
        }

        private void Awake()
        {
            particleSystem = this.GetComponent<ParticleSystem>();
            particleSystemMain = particleSystem.main;
        }

        public override bool IsPlaying()
        {
            return particleSystem.isPlaying;
        }

        public override void Play()
        {
            base.Play();
            particleSystem.Play();
        }

        public override void SetIntensity(float intensity)
        {
            if (emissionRateOverTimeLink == Link.Intensity)
            {
                SetParticleEmissionRateOverTime(intensity);
            }
            if (emissionRateOverDistanceLink == Link.Intensity)
            {
                SetParticleEmissionRateOverDistance(intensity);
            }
            if (lifeTimeLink == Link.Intensity)
            {
                SetParticleLifetime(intensity);
            }
            if (emissionBurstLink == Link.Intensity)
            {
                SetParticleBurst(intensity);
            }
            if (scaleLink == Link.Intensity)
            {
                SetParticleScale(intensity);
            }
            if (startSpeedLink == Link.Intensity)
            {
                SetParticleSpeed(intensity);
            }
        }

        public override void SetSpeed(float speed)
        {
            if (emissionRateOverTimeLink == Link.Speed)
            {
                SetParticleEmissionRateOverTime(speed);
            }
            if (emissionRateOverDistanceLink == Link.Speed)
            {
                SetParticleEmissionRateOverDistance(speed);
            }
            if (lifeTimeLink == Link.Speed)
            {
                SetParticleLifetime(speed);
            }
            if (emissionBurstLink == Link.Speed)
            {
                SetParticleBurst(speed);
            }
            if (scaleLink == Link.Speed)
            {
                SetParticleScale(speed);
            }
            if (startSpeedLink == Link.Speed)
            {
                SetParticleSpeed(speed);
            }
        }

        protected void SetParticleEmissionRateOverTime(float value)
        {
            minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
            float rate = Mathf.Lerp(emissionRateOverTimeRange.x, emissionRateOverTimeRange.y, emissionRateOverTimeCurve.Evaluate(value));
            minMaxCurve.constantMin = Mathf.Clamp(rate - emissionRateOverTimeReduction, emissionRateOverTimeRange.x, emissionRateOverTimeRange.y);
            minMaxCurve.constantMax = rate;
            ParticleSystem.EmissionModule particleEmission = particleSystem.emission;
            particleEmission.rateOverTime = minMaxCurve;
        }

        protected void SetParticleEmissionRateOverDistance(float value)
        {
            minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
            float rate = Mathf.Lerp(emissionRateOverDistanceRange.x, emissionRateOverDistanceRange.y, emissionRateOverDistanceCurve.Evaluate(value));
            minMaxCurve.constantMin = Mathf.Clamp(rate - emissionRateOverDistanceReduction, emissionRateOverDistanceRange.x, emissionRateOverDistanceRange.y);
            minMaxCurve.constantMax = rate;
            ParticleSystem.EmissionModule particleEmission = particleSystem.emission;
            particleEmission.rateOverDistance = minMaxCurve;
        }

        protected void SetParticleLifetime(float value)
        {
            minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
            float lifeTime = Mathf.Lerp(lifeTimeRange.x, lifeTimeRange.y, curveLifeTime.Evaluate(value));
            minMaxCurve.constantMin = Mathf.Clamp(lifeTime - lifeTimeReduction, lifeTimeRange.x, lifeTimeRange.y);
            minMaxCurve.constantMax = lifeTime;
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startLifetime = minMaxCurve;
        }

        protected void SetParticleBurst(float value)
        {
            short burst = (short)emissionBurstCurve.Evaluate(value);
            ParticleSystem.Burst particleBurst = new ParticleSystem.Burst(0, (short)Mathf.Clamp(burst - emissionBurstRandomRange, 0, Mathf.Infinity), (short)Mathf.Clamp(burst, 0, Mathf.Infinity));
            ParticleSystem.EmissionModule particleEmission = particleSystem.emission;
            particleEmission.SetBurst(0, particleBurst);
        }

        protected void SetParticleScale(float value)
        {
            float size = scaleCurve.Evaluate(value);
            this.transform.localScale = new Vector3(scaleMultiplier.x * size, scaleMultiplier.y * size, scaleMultiplier.z * size);
        }

        protected void SetParticleSpeed(float value)
        {
            minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
            float speed = startSpeedLinkCurve.Evaluate(value);
            minMaxCurve.constantMin = Mathf.Lerp(startSpeedLinkCurve.GetFirstValue(), speed, startSpeedMinSpeedRatio);
            minMaxCurve.constantMax = speed;
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startSpeed = minMaxCurve;
        }

        public override void Stop()
        {
            particleSystem.Stop();
        }
    }
}

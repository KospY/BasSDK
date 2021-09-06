using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class FxModuleParticle : FxModule
    {
        [Header("Emission Rate Over Time")]
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

        protected new ParticleSystem particleSystem;
        protected ParticleSystem.MainModule particleSystemMain;
        protected ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve();

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
            particleSystem.Play();
        }

        public override void SetIntensity(float intensity)
        {
            if (emissionRateOverTimeLink == Link.Intensity)
            {
                SetEmissionRateOverTime(intensity);
            }
            if (emissionRateOverDistanceLink == Link.Intensity)
            {
                SetEmissionRateOverDistance(intensity);
            }
        }

        public override void SetSpeed(float speed)
        {
            if (emissionRateOverTimeLink == Link.Speed)
            {
                SetEmissionRateOverTime(speed);
            }
            if (emissionRateOverDistanceLink == Link.Speed)
            {
                SetEmissionRateOverDistance(speed);
            }
        }

        protected void SetEmissionRateOverTime(float value)
        {
            minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
            float rate = Mathf.Lerp(emissionRateOverTimeRange.x, emissionRateOverTimeRange.y, emissionRateOverTimeCurve.Evaluate(value));
            minMaxCurve.constantMin = Mathf.Clamp(rate - emissionRateOverTimeReduction, emissionRateOverTimeRange.x, emissionRateOverTimeRange.y);
            minMaxCurve.constantMax = rate;
            ParticleSystem.EmissionModule particleEmission = particleSystem.emission;
            particleEmission.rateOverTime = minMaxCurve;
        }

        protected void SetEmissionRateOverDistance(float value)
        {
            minMaxCurve.mode = ParticleSystemCurveMode.TwoConstants;
            float rate = Mathf.Lerp(emissionRateOverDistanceRange.x, emissionRateOverDistanceRange.y, emissionRateOverDistanceCurve.Evaluate(value));
            minMaxCurve.constantMin = Mathf.Clamp(rate - emissionRateOverDistanceReduction, emissionRateOverDistanceRange.x, emissionRateOverDistanceRange.y);
            minMaxCurve.constantMax = rate;
            ParticleSystem.EmissionModule particleEmission = particleSystem.emission;
            particleEmission.rateOverDistance = minMaxCurve;
        }

        public override void Stop()
        {
            particleSystem.Stop();
        }
    }
}

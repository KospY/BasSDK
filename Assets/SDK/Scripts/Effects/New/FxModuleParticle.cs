using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/FxModuleParticle.html")]
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
        [HideIf("emissionBurstLink", Link.None), LabelText("Min speed ratio"), Range(0, 1)]
#endif
        public float startSpeedMinSpeedRatio = 1;

        protected MaterialPropertyBlock materialPropertyBlock;
#pragma warning disable 0109 // unity this.particleSystem is obsolete, we can override this
        protected new ParticleSystem particleSystem;
#pragma warning restore 0109        
        protected ParticleSystemRenderer particleSystemRenderer;
        protected ParticleSystem.MainModule particleSystemMain;
        protected ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve();

    }
}

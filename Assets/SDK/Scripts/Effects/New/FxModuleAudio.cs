using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/FxModuleAudio.html")]
    public class FxModuleAudio : FxModule
    {
        [Header("Audio")]
        public string audioContainerAddress;
        public AssetReferenceAudioContainer audioContainerReference;
        public AudioClip audioClip;
        public bool abnormalNoise;
        public float volumeDb;
        [Range(0f, 1f)]
        public float spatialBlend = 1f;
        public float playDelay;
        public float stopDelay;
        public float fadeInDuration = 0;
        public float fadeOutDuration = 0;
        public bool useRandomTime;
        public bool allowAdditivePlay;
        public PlayEvent playEvent = PlayEvent.Play;
        public AudioMixerName audioMixer = AudioMixerName.Effect;

        [Header("Curves")]
        public FxBlendCurves volume = new FxBlendCurves();

        public bool randomPitch = false;
        public FxBlendCurves pitch = new FxBlendCurves();

        public FxBlendCurves lowPassCutoffFrequency = new FxBlendCurves(new AnimationCurve(new Keyframe(0, 22000), new Keyframe(1, 22000)));
        public FxBlendCurves lowPassResonanceQCurve = new FxBlendCurves();

        public FxBlendCurves highPassCutoffFrequency = new FxBlendCurves(new AnimationCurve(new Keyframe(0, 10), new Keyframe(1, 10)));
        public FxBlendCurves highPassResonanceQCurve = new FxBlendCurves();

        public FxBlendCurves reverbDryLevel = new FxBlendCurves();

        public enum PlayEvent
        {
            Play,
            Loop,
            Stop,
        }

        [NonSerialized]
        public AudioSource audioSource;


        protected AudioLowPassFilter lowPassFilter;
        protected AudioHighPassFilter highPassFilter;
        protected AudioReverbFilter reverbFilter;
        public AudioContainer audioContainer;
        protected bool playOnLoad;
        protected bool clipLoadedFromAddressable;

        protected float intensity;
        protected float speed;

        protected bool wasPlaying;

        protected Coroutine fadeCoroutine;


        public void Refresh()
        {
        }


        public static float LinearToDecibel(float linear)
        {
            float dB;
            if (linear != 0)
            {
                dB = 20.0f * Mathf.Log10(linear);
            }
            else
            {
                dB = -144.0f;
            }

            return dB;
        }

        public static float DecibelToLinear(float dB)
        {
            float linear = Mathf.Pow(10.0f, dB / 20.0f);
            return linear;
        }
    }
}

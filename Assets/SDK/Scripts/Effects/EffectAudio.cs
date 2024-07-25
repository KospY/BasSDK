using UnityEngine;
using System;
using System.Collections;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;


namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/EffectAudio.html")]
    public class EffectAudio : Effect
    {
        public AudioContainer audioContainer;

        public float globalVolumeDb = 0;
        public float globalPitch = 1;
        public float reverbZoneMix = 1;

        public HapticDevice hapticDevice = HapticDevice.None;
        public GameData.HapticClip hapticClipFallBack;

        public bool loopCustomStep;
            
        public bool doNoise;
        protected bool hasNoise;


        public bool useVolumeIntensity;
        public AnimationCurve volumeIntensityCurve;
        public bool useVolumeSpeed;
        public AnimationCurve volumeSpeedCurve;
        public BlendMode volumeBlendMode;
        public float loopFadeDelay;
        public float fadeInTime = 0;

        public EffectLink pitchEffectLink = EffectLink.Intensity;
        public bool randomPitch;
        public bool linkEffectPitch;
        public AnimationCurve pitchCurve;

        public int intensitySmoothingIndex;
        public float[] intensitySmoothingBuffer;
        public int speedSmoothingIndex;
        public float[] speedSmoothingBuffer;


        [NonSerialized]
        public float playTime;

        public float playDelay;

        [Header("Dynamic Music")]
        public bool onDynamicMusic = false;
        public Music.MusicTransition.TransitionType dynamicMusicTiming = Music.MusicTransition.TransitionType.OnBeat;


        [Header("Random play")]
        public bool randomPlay;
        public float randomMinTime = 2;
        public float randomMaxTime = 5;

        [Header("Random Time")]
        public bool randomTime;

        [Header("Low pass filter")]
        public bool useLowPassFilter;
        public EffectLink lowPassEffectLink = EffectLink.Intensity;
        public AnimationCurve lowPassCutoffFrequencyCurve = new AnimationCurve(new Keyframe(0, 22000), new Keyframe(1, 22000));
        public AnimationCurve lowPassResonanceQCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        [Header("High pass filter")]
        public bool useHighPassFilter;
        public EffectLink highPassEffectLink = EffectLink.Intensity;
        public AnimationCurve highPassCutoffFrequencyCurve = new AnimationCurve(new Keyframe(0, 10), new Keyframe(1, 10));
        public AnimationCurve highPassResonanceQCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        [Header("Reverb filter")]
        public bool useReverbFilter;
        public EffectLink reverbEffectLink = EffectLink.Intensity;
        public AnimationCurve reverbDryLevelCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));

        [NonSerialized]
        public AudioSource audioSource;
        [NonSerialized]
        public AudioLowPassFilter lowPassFilter;
        [NonSerialized]
        public AudioHighPassFilter highPassFilter;
        [NonSerialized]
        public AudioReverbFilter reverbFilter;

        protected float fadeInMult = 1;


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

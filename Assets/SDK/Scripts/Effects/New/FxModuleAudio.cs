using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.VFX;

namespace ThunderRoad
{
    public class FxModuleAudio : FxModule
    {
        [Header("Audio")]
        public string playAudioContainerAddress;
        protected AudioContainer playAudioContainer;
        public float playVolumeDb = 0;

        public string loopAudioContainerAddress;
        protected AudioContainer loopAudioContainer;
        public float loopVolumeDb = 0;

        public string stopAudioContainerAddress;
        protected AudioContainer stopAudioContainer;
        public float stopVolumeDb = 0;

        public bool doNoise;
        protected bool hasNoise;

        public AudioMixerName audioMixer = AudioMixerName.Effect;

        [Header("Loop volume")]
        public AnimationCurve volumeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
        public EffectLink volumeEffectLink = EffectLink.Intensity;

        [Header("Loop pitch")]
        public EffectLink pitchEffectLink = EffectLink.Intensity;
        public AnimationCurve pitchCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        [Header("Loop low pass filter")]
        public bool useLowPassFilter;
        public EffectLink lowPassFilterEffectLink = EffectLink.Intensity;
        public AnimationCurve lowPassCutoffFrequencyCurve = new AnimationCurve(new Keyframe(0, 22000), new Keyframe(1, 22000));
        public AnimationCurve lowPassResonanceQCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        [Header("Loop high pass filter")]
        public bool useHighPassFilter;
        public EffectLink highPassFilterEffectLink = EffectLink.Intensity;
        public AnimationCurve highPassCutoffFrequencyCurve = new AnimationCurve(new Keyframe(0, 10), new Keyframe(1, 10));
        public AnimationCurve highPassResonanceQCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        [Header("Loop reverb filter")]
        public bool useReverbFilter;
        public EffectLink reverbFilterEffectLink = EffectLink.Intensity;
        public AnimationCurve reverbDryLevelCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));

        protected AudioSource loopAudioSource;
        protected AudioLowPassFilter lowPassFilter;
        protected AudioHighPassFilter highPassFilter;
        protected AudioReverbFilter reverbFilter;

        protected AudioSource oneShotAudioSource;
        protected bool isPlaying;

        private void OnDestroy()
        {
            if (playAudioContainer) Addressables.Release(playAudioContainer);
            if (loopAudioContainer) Addressables.Release(loopAudioContainer);
            if (stopAudioContainer) Addressables.Release(stopAudioContainer);
        }

        private void Awake()
        {
            loopAudioSource = this.GetComponent<AudioSource>();
            if (!loopAudioSource) loopAudioSource = this.gameObject.AddComponent<AudioSource>();
            loopAudioSource.spatialBlend = 1;
            loopAudioSource.dopplerLevel = 0;
            loopAudioSource.playOnAwake = false;
            loopAudioSource.loop = true;
            if (useLowPassFilter)
            {
                lowPassFilter = this.GetComponent<AudioLowPassFilter>();
                if (!lowPassFilter) lowPassFilter = this.gameObject.AddComponent<AudioLowPassFilter>();
            }
            if (useHighPassFilter)
            {
                highPassFilter = this.GetComponent<AudioHighPassFilter>();
                if (!highPassFilter) highPassFilter = this.gameObject.AddComponent<AudioHighPassFilter>();
            }
            if (useReverbFilter)
            {
                reverbFilter = this.GetComponent<AudioReverbFilter>();
                if (!reverbFilter) reverbFilter = this.gameObject.AddComponent<AudioReverbFilter>();
                reverbFilter.reverbPreset = AudioReverbPreset.Off;
            }

            if (playAudioContainerAddress != null && playAudioContainerAddress != "")
            {
                Addressables.LoadAssetAsync<AudioContainer>(playAudioContainerAddress).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        playAudioContainer = handle.Result;
                        TryCreateOneShootAudioSource();
                        if (isPlaying && oneShotAudioSource && playAudioContainer)
                        {
                            oneShotAudioSource.PlayOneShot(playAudioContainer.PickAudioClip(), DecibelToLinear(playVolumeDb));
                        }
                    }
                };
            }
            if (loopAudioContainerAddress != null && loopAudioContainerAddress != "")
            {
                Addressables.LoadAssetAsync<AudioContainer>(loopAudioContainerAddress).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        loopAudioContainer = handle.Result;
                        if (isPlaying)
                        {
                            loopAudioSource.clip = loopAudioContainer.PickAudioClip();
                            loopAudioSource.Play();
                        }
                    }
                };
            }
            if (stopAudioContainerAddress != null && stopAudioContainerAddress != "")
            {
                Addressables.LoadAssetAsync<AudioContainer>(stopAudioContainerAddress).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        stopAudioContainer = handle.Result;
                        TryCreateOneShootAudioSource();
                    }
                };
            }
        }

        protected void TryCreateOneShootAudioSource()
        {
            if (!oneShotAudioSource)
            {
                oneShotAudioSource = Common.CloneComponent(loopAudioSource, this.gameObject, false) as AudioSource;
                oneShotAudioSource.spatialBlend = 1;
                oneShotAudioSource.dopplerLevel = 0;
                oneShotAudioSource.playOnAwake = false;
                oneShotAudioSource.loop = false;
            }
        }

        public override void Play()
        {
            if (loopAudioContainer)
            {
                loopAudioSource.clip = loopAudioContainer.PickAudioClip();
                loopAudioSource.Play();
            }
            if (oneShotAudioSource && playAudioContainer)
            {
                oneShotAudioSource.PlayOneShot(playAudioContainer.PickAudioClip(), DecibelToLinear(playVolumeDb));
            }
            isPlaying = true;
        }

        public override void SetIntensity(float intensity)
        {
            SetVariation(intensity, EffectLink.Intensity);
        }

        public override void SetSpeed(float speed)
        {
            SetVariation(speed, EffectLink.Speed);
        }

        protected void SetVariation(float value, EffectLink effectLink)
        {
            if (volumeEffectLink == effectLink)
            {
                loopAudioSource.volume = volumeCurve.Evaluate(value) * DecibelToLinear(loopVolumeDb);
            }
            if (pitchEffectLink == effectLink)
            {
                loopAudioSource.pitch = pitchCurve.Evaluate(value);
            }
            if (pitchEffectLink == effectLink)
            {
                loopAudioSource.pitch = pitchCurve.Evaluate(value);
            }
            if (useLowPassFilter && lowPassFilterEffectLink == effectLink)
            {
                lowPassFilter.cutoffFrequency = lowPassCutoffFrequencyCurve.Evaluate(value);
                lowPassFilter.lowpassResonanceQ = lowPassResonanceQCurve.Evaluate(value);
            }
            if (useHighPassFilter && highPassFilterEffectLink == effectLink)
            {
                highPassFilter.cutoffFrequency = highPassCutoffFrequencyCurve.Evaluate(value);
                highPassFilter.highpassResonanceQ = highPassResonanceQCurve.Evaluate(value);
            }
            if (useReverbFilter && reverbFilterEffectLink == effectLink)
            {
                reverbFilter.dryLevel = reverbDryLevelCurve.Evaluate(value);
            }
        }

        public override void Stop()
        {
            if (loopAudioSource) loopAudioSource.Stop();
            if (oneShotAudioSource && stopAudioContainer)
            {
                oneShotAudioSource.PlayOneShot(stopAudioContainer.PickAudioClip(), DecibelToLinear(stopVolumeDb));
            }
            isPlaying = false;
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

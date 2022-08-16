using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

#if ODIN_INSPECTOR
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/FxModuleAudio")]
	public class FxModuleAudio : FxModule
    {
        [Header("Audio")]
        public string audioContainerAddress;
        public AssetReferenceAudioContainer audioContainerReference;
        public bool abnormalNoise;
        public float volumeDb;
        public bool useRandomTime;
        public PlayEvent playEvent = PlayEvent.Play;
        public AudioMixerName audioMixer = AudioMixerName.Effect;

        [Header("Curves")]
        public FxBlendCurves volume = new FxBlendCurves();

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
        protected AudioContainer audioContainer;
        protected bool playOnLoad;
        protected bool clipLoadedFromAddressable;

        protected float intensity;
        protected float speed;

        protected bool wasPlaying;

        private void OnDestroy()
        {
            if (clipLoadedFromAddressable && audioSource.clip) Addressables.Release(audioContainer);
        }

        private void OnValidate()
        {
            audioSource = this.GetComponent<AudioSource>();
        }

        private void OnDisable()
        {
            wasPlaying = audioSource.isPlaying;
        }

        private void OnEnable()
        {
            if (wasPlaying)
            {
                audioSource.Play();
            }
        }

        private void Awake()
        {
            audioSource = this.GetComponent<AudioSource>();
            if (!audioSource) audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
            audioSource.playOnAwake = false;
            audioSource.loop = false;


            // Add extra components
            if (lowPassCutoffFrequency.IsUsed() || lowPassResonanceQCurve.IsUsed())
            {
                lowPassFilter = this.GetComponent<AudioLowPassFilter>();
                if (!lowPassFilter) lowPassFilter = this.gameObject.AddComponent<AudioLowPassFilter>();
            }

            if (highPassCutoffFrequency.IsUsed() || highPassResonanceQCurve.IsUsed())
            {
                highPassFilter = this.GetComponent<AudioHighPassFilter>();
                if (!highPassFilter) highPassFilter = this.gameObject.AddComponent<AudioHighPassFilter>();
            }

            if (reverbDryLevel.IsUsed())
            {
                reverbFilter = this.GetComponent<AudioReverbFilter>();
                if (!reverbFilter) reverbFilter = this.gameObject.AddComponent<AudioReverbFilter>();
                reverbFilter.reverbPreset = AudioReverbPreset.Off;
            }

            // Load audioclip
            if (audioContainerAddress != null && audioContainerAddress != "")
            {
                Addressables.LoadAssetAsync<AudioContainer>(audioContainerAddress).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        audioContainer = handle.Result;
                        audioSource.clip = audioContainer.PickAudioClip();
                        if (playOnLoad) Play();
                        playOnLoad = false;
                        clipLoadedFromAddressable = true;
                    }
                };
            }
            else if (audioContainerReference != null && !string.IsNullOrEmpty(audioContainerReference.AssetGUID))
            {
                Addressables.LoadAssetAsync<AudioContainer>(audioContainerReference).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        audioContainer = handle.Result;
                        audioSource.clip = audioContainer.PickAudioClip();
                        if (playOnLoad) Play();
                        playOnLoad = false;
                        clipLoadedFromAddressable = true;
                    }
                };
            }
        }

        public override bool IsPlaying()
        {
            return audioSource.isPlaying;
        }

        public override void SetIntensity(float intensity)
        {
            this.intensity = intensity;
            Refresh();
        }

        public override void SetSpeed(float speed)
        {
            this.speed = speed;
            Refresh();
        }

        protected void Refresh()
        {
            if (volume.TryGetValue(intensity, speed, out float value))
            {
                audioSource.volume = value * DecibelToLinear(volumeDb);
            }
            if (pitch.TryGetValue(intensity, speed, out value))
            {
                audioSource.pitch = value;
            }
            if (lowPassCutoffFrequency.TryGetValue(intensity, speed, out value))
            {
                lowPassFilter.cutoffFrequency = value;
            }
            if (lowPassResonanceQCurve.TryGetValue(intensity, speed, out value))
            {
                lowPassFilter.lowpassResonanceQ = value;
            }
            if (highPassCutoffFrequency.TryGetValue(intensity, speed, out value))
            {
                highPassFilter.cutoffFrequency = value;
            }
            if (highPassCutoffFrequency.TryGetValue(intensity, speed, out value))
            {
                highPassFilter.highpassResonanceQ = value;
            }
            if (reverbDryLevel.TryGetValue(intensity, speed, out value))
            {
                reverbFilter.dryLevel = value;
            }
        }

        public override void Play()
        {
            if (playEvent == PlayEvent.Play || playEvent == PlayEvent.Loop)
            {
                if (audioContainer)
                {
                    AudioClip audioClip = audioContainer.PickAudioClip();
                    if (audioSource.clip != audioClip)
                    {
                        audioSource.clip = audioClip;
                    }

                    if (useRandomTime)
                    {
                        var randomStartTime = UnityEngine.Random.Range(0, audioSource.clip.samples - 1);
                        audioSource.timeSamples = randomStartTime;
                    }

                    audioSource.loop = (playEvent == PlayEvent.Loop);
                    audioSource.Play();
                }
                else
                {
                    playOnLoad = true;
                }
            }
        }

        public override void Stop()
        {
            if (playEvent == PlayEvent.Stop)
            {
                if (audioSource.clip != null)
                {
                    audioSource.Play();
                }
                else
                {
                    playOnLoad = true;
                }
            }
            else
            {
                audioSource.Stop();
            }
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

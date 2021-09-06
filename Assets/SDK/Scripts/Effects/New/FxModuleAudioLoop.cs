using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class FxModuleAudioLoop : FxModule
    {
        [Header("Audio")]
        public string audioClipAddress;
        public AssetReference audioClipReference;
        public bool abnormalNoise;
        public float volumeDb;


        public AudioMixerName audioMixer = AudioMixerName.Effect;

        [Header("Volume")]
        public Link volumeLink = Link.None;
#if ODIN_INSPECTOR
        [HideIf("volumeLink", Link.None)]
#endif
        public AnimationCurve volumeCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        [Header("Volume Multiplier")]
        public Link volumeMultiplierLink = Link.None;
#if ODIN_INSPECTOR
        [HideIf("volumeMultiplierLink", Link.None)]
#endif
        public AnimationCurve volumeMultiplierCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));


        [Header("Loop pitch")]
        public Link pitchLink = Link.None;
#if ODIN_INSPECTOR
        [HideIf("pitchLink", Link.None)]
#endif
        public AnimationCurve pitchCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        [Header("Low pass filter")]
        public Link lowPassFilterLink = Link.None;
#if ODIN_INSPECTOR
        [HideIf("lowPassFilterLink", Link.None)]
#endif
        public AnimationCurve lowPassCutoffFrequencyCurve = new AnimationCurve(new Keyframe(0, 22000), new Keyframe(1, 22000));
#if ODIN_INSPECTOR
        [HideIf("lowPassFilterLink", Link.None)]
#endif
        public AnimationCurve lowPassResonanceQCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        [Header("High pass filter")]
        public Link highPassFilterLink = Link.None;
#if ODIN_INSPECTOR
        [HideIf("highPassFilterLink", Link.None)]
#endif
        public AnimationCurve highPassCutoffFrequencyCurve = new AnimationCurve(new Keyframe(0, 10), new Keyframe(1, 10));
#if ODIN_INSPECTOR
        [HideIf("highPassFilterLink", Link.None)]
#endif
        public AnimationCurve highPassResonanceQCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        [Header("Reverb filter")]
        public Link reverbFilterLink = Link.None;
#if ODIN_INSPECTOR
        [HideIf("reverbFilterLink", Link.None)]
#endif
        public AnimationCurve reverbDryLevelCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));

        protected AudioSource audioSource;
        protected AudioLowPassFilter lowPassFilter;
        protected AudioHighPassFilter highPassFilter;
        protected AudioReverbFilter reverbFilter;
        protected bool playOnLoad;
        protected bool clipLoadedFromAddressable;
        protected float volume = 1;
        protected float volumeMultiplier = 1;

        private void OnDestroy()
        {
            if (clipLoadedFromAddressable & audioSource.clip) Addressables.Release(audioSource.clip);
        }

        private void Awake()
        {
            audioSource = this.GetComponent<AudioSource>();
            if (!audioSource) audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
            audioSource.playOnAwake = false;
            audioSource.loop = true;

            // Add extra components
            if (lowPassFilterLink != Link.None)
            {
                lowPassFilter = this.GetComponent<AudioLowPassFilter>();
                if (!lowPassFilter) lowPassFilter = this.gameObject.AddComponent<AudioLowPassFilter>();
            }
            if (highPassFilterLink != Link.None)
            {
                highPassFilter = this.GetComponent<AudioHighPassFilter>();
                if (!highPassFilter) highPassFilter = this.gameObject.AddComponent<AudioHighPassFilter>();
            }
            if (reverbFilterLink != Link.None)
            {
                reverbFilter = this.GetComponent<AudioReverbFilter>();
                if (!reverbFilter) reverbFilter = this.gameObject.AddComponent<AudioReverbFilter>();
                reverbFilter.reverbPreset = AudioReverbPreset.Off;
            }

            // Load audioclip
            if (audioClipAddress != null && audioClipAddress != "")
            {
                Addressables.LoadAssetAsync<AudioClip>(audioClipAddress).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        audioSource.clip = handle.Result;
                        if (playOnLoad) audioSource.Play();
                        clipLoadedFromAddressable = true;
                    }
                };
            }
            else if (audioClipReference != null)
            {
                Addressables.LoadAssetAsync<AudioClip>(audioClipReference).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        audioSource.clip = handle.Result;
                        if (playOnLoad) audioSource.Play();
                        clipLoadedFromAddressable = true;
                    }
                };
            }
        }

        public override void Play()
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

        public override bool IsPlaying()
        {
            return audioSource.isPlaying;
        }

        public override void SetIntensity(float intensity)
        {
            SetVariation(intensity, Link.Intensity);
        }

        public override void SetSpeed(float speed)
        {
            SetVariation(speed, Link.Speed);
        }

        protected void SetVariation(float value, Link link)
        {
            if (volumeLink == link)
            {
                volume = volumeCurve.Evaluate(value) * DecibelToLinear(volumeDb);
            }
            if (volumeMultiplierLink == link)
            {
                volumeMultiplier = volumeMultiplierCurve.Evaluate(value);
            }
            audioSource.volume = volume * volumeMultiplier;

            if (pitchLink == link)
            {
                audioSource.pitch = pitchCurve.Evaluate(value);
            }
            if (pitchLink == link)
            {
                audioSource.pitch = pitchCurve.Evaluate(value);
            }
            if (lowPassFilterLink == link)
            {
                lowPassFilter.cutoffFrequency = lowPassCutoffFrequencyCurve.Evaluate(value);
                lowPassFilter.lowpassResonanceQ = lowPassResonanceQCurve.Evaluate(value);
            }
            if (highPassFilterLink == link)
            {
                highPassFilter.cutoffFrequency = highPassCutoffFrequencyCurve.Evaluate(value);
                highPassFilter.highpassResonanceQ = highPassResonanceQCurve.Evaluate(value);
            }
            if (reverbFilterLink == link)
            {
                reverbFilter.dryLevel = reverbDryLevelCurve.Evaluate(value);
            }
        }

        public override void Stop()
        {
            audioSource.Stop();
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

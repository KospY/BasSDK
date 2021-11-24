using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if PrivateSDK
using SteamAudio;
#endif

namespace ThunderRoad
{
    public class EffectAudio : Effect
    {
        public AudioContainer audioContainer;

        public float globalVolumeDb = 0;
        public float globalPitch = 1;

        public bool doNoise;
        protected bool hasNoise;

        public AnimationCurve volumeCurve;
        public float loopFadeDelay;
        public EffectLink effectLink = EffectLink.Intensity;

        public EffectLink pitchEffectLink = EffectLink.Intensity;
        public bool randomPitch;
        public AnimationCurve pitchCurve;

        [NonSerialized]
        public float playTime;

        public float playDelay;

        [Header("Random play")]
        public bool randomPlay;
        public float randomMinTime = 2;
        public float randomMaxTime = 5;

        [Header("Low pass filter")]
        public bool useLowPassFilter;
        public AnimationCurve lowPassCutoffFrequencyCurve = new AnimationCurve(new Keyframe(0, 22000), new Keyframe(1, 22000));
        public AnimationCurve lowPassResonanceQCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        [Header("High pass filter")]
        public bool useHighPassFilter;
        public AnimationCurve highPassCutoffFrequencyCurve = new AnimationCurve(new Keyframe(0, 10), new Keyframe(1, 10));
        public AnimationCurve highPassResonanceQCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

        [Header("Reverb filter")]
        public bool useReverbFilter;
        public AnimationCurve reverbDryLevelCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));

        [NonSerialized]
        public AudioSource audioSource;
        [NonSerialized]
        public AudioLowPassFilter lowPassFilter;
        [NonSerialized]
        public AudioHighPassFilter highPassFilter;
        [NonSerialized]
        public AudioReverbFilter reverbFilter;

#if PrivateSDK
        [NonSerialized]
        public SteamAudioSource steamAudioSource;
#endif

        private void Awake()
        {
            audioSource = this.GetComponent<AudioSource>();
            if (!audioSource) audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
            audioSource.playOnAwake = false;
            if (AudioSettings.GetSpatializerPluginName() != null) audioSource.spatialize = true;
            lowPassFilter = this.GetComponent<AudioLowPassFilter>();
            if (!lowPassFilter) lowPassFilter = this.gameObject.AddComponent<AudioLowPassFilter>();
            lowPassFilter.enabled = false;
            highPassFilter = this.GetComponent<AudioHighPassFilter>();
            if (!highPassFilter) highPassFilter = this.gameObject.AddComponent<AudioHighPassFilter>();
            highPassFilter.enabled = false;
            reverbFilter = this.GetComponent<AudioReverbFilter>();
            if (!reverbFilter) reverbFilter = this.gameObject.AddComponent<AudioReverbFilter>();
            reverbFilter.reverbPreset = AudioReverbPreset.Off;
            reverbFilter.enabled = false;
        }

        public override void Play()
        {
            if (!audioContainer)
            {
                return;
            }
            CancelInvoke();
            StopAllCoroutines();

            //Debug.Log("Play " + (module as EffectModuleAudio).audioContainerAddress + " volume: " + audioSource.volume + " parent: " + this.transform.parent + " parent2: " + this.transform.parent?.parent + " parent3: " + this.transform.parent?.parent?.parent);
            audioSource.clip = audioContainer.PickAudioClip();

            if (audioSource.clip == null)
            {
                Debug.LogError("Picked audio from audioContainer [" + audioContainer.name + "] is null ");
                return;
            }

            if (randomPitch)
            {
                audioSource.pitch = pitchCurve.Evaluate(UnityEngine.Random.Range(0f, 1f));
            }
            audioSource.pitch *= globalPitch;

            if (step == Step.Start || step == Step.End)
            {
                Invoke("Despawn", audioSource.clip.length + playDelay + 0.1f);
            }

            if (randomPlay)
            {
                audioSource.loop = false;
                audioSource.clip = audioContainer.PickAudioClip();
                float randomDelay = UnityEngine.Random.Range(randomMinTime, randomMaxTime);
                Invoke("RandomPlay", randomDelay);
            }
            else
            {
                audioSource.loop = step == Step.Loop ? true : false;
                if (playDelay > 0)
                {
                    audioSource.Play((ulong)(audioSource.clip.frequency * playDelay));
                }
                else
                {
                    audioSource.Play();
                }
            }

            playTime = Time.time;
        }

        protected void RandomPlay()
        {
            audioSource.clip = audioContainer.PickAudioClip();
            if (!audioSource.isPlaying) audioSource.Play();
            float randomDelay = UnityEngine.Random.Range(randomMinTime, randomMaxTime);
            Invoke("RandomPlay", randomDelay);
            Debug.Log(randomDelay);
        }

        public override void Stop()
        {
            audioSource.Stop();
            hasNoise = false;
        }

        public override void End(bool loopOnly = false)
        {
            CancelInvoke("RandomPlay");
            if (loopFadeDelay > 0)
            {
                StopAllCoroutines();
                StartCoroutine(AudioFadeOut());
            }
            else if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                Despawn();
            }
        }

        public override void SetNoise(bool noise)
        {
            doNoise = noise;
        }

        public override void SetIntensity(float value, bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                if (effectLink == EffectLink.Intensity)
                {
                    SetVariation(value, loopOnly);
                }
                if (pitchEffectLink == EffectLink.Intensity)
                {
                    audioSource.pitch = pitchCurve.Evaluate(value) * globalPitch;
                }
            }
        }

        public override void SetSpeed(float value, bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                if (effectLink == EffectLink.Speed)
                {
                    SetVariation(value, loopOnly);
                }
                if (pitchEffectLink == EffectLink.Speed)
                {
                    audioSource.pitch = pitchCurve.Evaluate(value) * globalPitch;
                }
            }
        }

        public void SetVariation(float value, bool loopOnly = false)
        {
            audioSource.volume = volumeCurve.Evaluate(value) * DecibelToLinear(globalVolumeDb);

            if (useLowPassFilter)
            {
                lowPassFilter.cutoffFrequency = lowPassCutoffFrequencyCurve.Evaluate(value);
                lowPassFilter.lowpassResonanceQ = lowPassResonanceQCurve.Evaluate(value);
            }
            if (useHighPassFilter)
            {
                highPassFilter.cutoffFrequency = highPassCutoffFrequencyCurve.Evaluate(value);
                highPassFilter.highpassResonanceQ = highPassResonanceQCurve.Evaluate(value);
            }
            if (useLowPassFilter)
            {
                reverbFilter.dryLevel = reverbDryLevelCurve.Evaluate(value);
            }
        }

        protected IEnumerator AudioFadeOut()
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= Time.deltaTime / loopFadeDelay;
                yield return new WaitForEndOfFrame();
            }
            Despawn();
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


        public override void Despawn()
        {
            Stop();
            CancelInvoke();
            StopAllCoroutines();
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

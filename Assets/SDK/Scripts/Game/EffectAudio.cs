using UnityEngine;
using System;
using System.Collections;

namespace BS
{
    public class EffectAudio : Effect
    {
        public bool randomPitch;
        public AnimationCurve pitchCurve;
        public AnimationCurve volumeCurve;
        public float loopFadeDelay;

        [NonSerialized]
        public float playTime;

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

        private void Awake()
        {
            audioSource = this.GetComponent<AudioSource>();
            if (!audioSource) audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1;
            audioSource.playOnAwake = false;
            audioSource.spatialize = true;
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
            CancelInvoke();
            StopAllCoroutines();
            audioSource.loop = step == Step.Loop ? true : false;
            if (randomPitch)
            {
                audioSource.pitch = pitchCurve.Evaluate(UnityEngine.Random.Range(0f, 1f));
            }
            if (step != Step.Loop)
            {
                Invoke("Despawn", audioSource.clip.length + 1);
            }
            audioSource.Play();
            playTime = Time.time;
        }

        public override void Stop(bool loopOnly = false)
        {
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

        public override void SetIntensity(float value, bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                audioSource.pitch = pitchCurve.Evaluate(value);
                audioSource.volume = volumeCurve.Evaluate(value);
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

        public override void Despawn()
        {
            CancelInvoke();
            StopAllCoroutines();
            audioSource.Stop();
            if (Application.isPlaying)
            {
                EffectModuleAudio.Despawn(this);
                InvokeDespawnCallback();
            }
        }
    }
}

using UnityEngine;
using System;
using System.Collections;
using Debug = UnityEngine.Debug;
#if PrivateSDK
using ThunderRoad.Pools;
using SteamAudio;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/EffectAudio")]
    public class EffectAudio : Effect
    {
        public AudioContainer audioContainer;

        public float globalVolumeDb = 0;
        public float globalPitch = 1;

        public HapticDevice hapticDevice = HapticDevice.None;

        public bool doNoise;
        protected bool hasNoise;

        public bool useVolumeIntensity;
        public AnimationCurve volumeIntensityCurve;
        public bool useVolumeSpeed;
        public AnimationCurve volumeSpeedCurve;
        public BlendMode volumeBlendMode;
        public float loopFadeDelay;

        public EffectLink pitchEffectLink = EffectLink.Intensity;
        public bool randomPitch;
        public AnimationCurve pitchCurve;

        public int intensitySmoothingIndex;
        public float[] intensitySmoothingBuffer;
        public int speedSmoothingIndex;
        public float[] speedSmoothingBuffer;


        [NonSerialized]
        public float playTime;

        public float playDelay;

        [Header("Random play")]
        public bool randomPlay;
        public float randomMinTime = 2;
        public float randomMaxTime = 5;

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

#if PrivateSDK
        [NonSerialized]
        public SteamAudioSource steamAudioSource;

        protected float intensity;
        protected float speed;

        private void Awake()
        {
            audioSource = this.GetComponent<AudioSource>();
            if (!audioSource) audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
            audioSource.playOnAwake = false;
            intensitySmoothingBuffer = new float[0];
            speedSmoothingBuffer = new float[0];
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

            //Reset the spatialBlend back to what it is in the effectModuleAudio
            audioSource.spatialBlend = (module as EffectModuleAudio).spatialBlend;
            if (containingInstance != null)
            {
                if (!containingInstance.fromPlayer && (module as EffectModuleAudio).globalOnPlayerOnly)
                {
                    audioSource.spatialBlend = 1;
                }
            }

            if (audioSource.clip == null)
            {
                Debug.LogWarning(
                    $"No Audioclip set on EffectAudio for [{audioContainer.name}]. Is this Effect not using pooling");
                if (audioContainer.TryPickAudioClip(out AudioClip clip))
                {
                    audioSource.clip = clip;
                }
                else
                {
                    Debug.LogError("Picked audio from audioContainer [" + audioContainer.name + "] is null ");
                    return;
                }
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
                Debug.LogWarning($"EffectAudio {this.gameObject.name} for [{audioContainer.name}] uses randomPlay. This effect may not be pooled");
                audioSource.loop = false;
                RandomPlay();
            }
            else
            {
                audioSource.loop = step == Step.Loop ? true : false;
                if (playDelay > 0)
                {
                    audioSource.PlayDelayed(playDelay);
                }
                else
                {
                    audioSource.Play();
                }
            }

            if ((module as EffectModuleAudio).useAudioForHaptic)
            {
                if (hapticDevice == HapticDevice.LeftController)
                {
                    PlayerControl.handLeft.Haptic(audioContainer.GetPcmData(audioSource.clip));
                }
                else if (hapticDevice == HapticDevice.RightController)
                {
                    PlayerControl.handRight.Haptic(audioContainer.GetPcmData(audioSource.clip));
                }
            }

            hasNoise = false;
            if (doNoise)
            {
                if (audioSource.loop)
                {
                    noise = NoiseManager.AddLoopNoise(audioSource, containingInstance?.source);
                    if (noise != null) hasNoise = true;
                }
                else
                {
                    noise = NoiseManager.AddNoise(this.transform.position, audioSource.volume, containingInstance?.source);
                    if (noise != null) hasNoise = true;
                }
            }
            playTime = Time.time;
        }

        protected void RandomPlay()
        {
            if (audioContainer.TryPickAudioClip(out AudioClip audioClip))
            {
                audioSource.clip = audioClip;
                if (!audioSource.isPlaying) audioSource.Play();
                float randomDelay = UnityEngine.Random.Range(randomMinTime, randomMaxTime);
                Invoke("RandomPlay", randomDelay);
                Debug.Log(randomDelay);
            }
        }

        public override void Stop()
        {
            audioSource.Stop();

            if (audioSource.loop)
            {
                NoiseManager.RemoveLoopNoise(audioSource);
            }

            noise = null;
            hasNoise = false;
            hapticDevice = HapticDevice.None;
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

        public override void SetHapticDevice(HapticDevice hapticDevice)
        {
            this.hapticDevice = hapticDevice;
        }

        public override void SetNoise(bool noise)
        {
            doNoise = noise;
        }

        public float Smooth(float intensity, ref float[] buffer, ref int sampleIndex, int samples)
        {

            // Inspector value has changed, we resize the buffer to match it
            if (buffer.Length != samples)
            {
                Array.Resize(ref buffer, samples);
                sampleIndex = 0;
            }

            var avg = intensity;

            if (buffer.Length > 0)
            {
                buffer[sampleIndex] = intensity;
                // Circular access in the buffer
                sampleIndex = (++sampleIndex) % buffer.Length;

                // Compute the buffer average
                var sum = 0f;
                for (var i = 0; i < buffer.Length; i++)
                    sum += buffer[i];

                avg = sum / buffer.Length;
            }

            return Mathf.Abs(avg);
        }

        public override void SetIntensity(float value, bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                if (((module as EffectModuleAudio)?.intensitySmoothingSampleCount ?? 0) > 0)
                {
                    value = Smooth(value, ref intensitySmoothingBuffer, ref intensitySmoothingIndex, (module as EffectModuleAudio).intensitySmoothingSampleCount);
                }
                intensity = value;
                Refresh();
            }
        }

        public override void SetSpeed(float value, bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                if (((module as EffectModuleAudio)?.speedSmoothingSampleCount ?? 0) > 0)
                {
                    value = Smooth(value, ref speedSmoothingBuffer, ref speedSmoothingIndex, (module as EffectModuleAudio).speedSmoothingSampleCount);
                }
                speed = value;
                Refresh();
            }
        }

        public void Refresh()
        {
            float linearVolume = DecibelToLinear(globalVolumeDb);
            if (useVolumeIntensity && useVolumeSpeed)
            {
                switch (volumeBlendMode)
                {
                    case BlendMode.Min:
                        audioSource.volume = Mathf.Min(volumeIntensityCurve.Evaluate(intensity), volumeSpeedCurve.Evaluate(speed)) * linearVolume;
                        break;
                    case BlendMode.Max:
                        audioSource.volume = Mathf.Max(volumeIntensityCurve.Evaluate(intensity), volumeSpeedCurve.Evaluate(speed)) * linearVolume;
                        break;
                    case BlendMode.Average:
                        audioSource.volume = Mathf.Lerp(volumeIntensityCurve.Evaluate(intensity), volumeSpeedCurve.Evaluate(speed), 0.5f) * linearVolume;
                        break;
                    case BlendMode.Multiply:
                        audioSource.volume = volumeIntensityCurve.Evaluate(intensity) * volumeIntensityCurve.Evaluate(speed) * linearVolume;
                        break;
                }
            }
            else if (useVolumeIntensity)
            {
                audioSource.volume = volumeIntensityCurve.Evaluate(intensity) * linearVolume;
            }
            else if (useVolumeSpeed)
            {
                audioSource.volume = volumeSpeedCurve.Evaluate(speed) * linearVolume;
            }

            switch (pitchEffectLink)
            {
                case EffectLink.Intensity:
                    audioSource.pitch = pitchCurve.Evaluate(intensity) * globalPitch;
                    break;
                case EffectLink.Speed:
                    audioSource.pitch = pitchCurve.Evaluate(speed) * globalPitch;
                    break;
            }

            // Update noise volume to max played
            if (NoiseManager.isActive && hasNoise)
            {
                noise.UpdateVolume(audioSource.volume);
            }

            if (useLowPassFilter)
            {
                float value = lowPassEffectLink switch
                {
                    EffectLink.Intensity => intensity,
                    EffectLink.Speed => speed,
                    _ => 0
                };

                lowPassFilter.cutoffFrequency = lowPassCutoffFrequencyCurve.Evaluate(value);
                lowPassFilter.lowpassResonanceQ = lowPassResonanceQCurve.Evaluate(value);
            }

            if (useHighPassFilter)
            {
                float value = highPassEffectLink switch
                {
                    EffectLink.Intensity => intensity,
                    EffectLink.Speed => speed,
                    _ => 0
                };
                highPassFilter.cutoffFrequency = highPassCutoffFrequencyCurve.Evaluate(value);
                highPassFilter.highpassResonanceQ = highPassResonanceQCurve.Evaluate(value);
            }

            if (useReverbFilter)
            {
                float value = reverbEffectLink switch
                {
                    EffectLink.Intensity => intensity,
                    EffectLink.Speed => speed,
                    _ => 0
                };
                reverbFilter.dryLevel = reverbDryLevelCurve.Evaluate(value);
                reverbFilter.dryLevel = reverbDryLevelCurve.Evaluate(value);
            }
        }

        protected IEnumerator AudioFadeOut()
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= Time.deltaTime / loopFadeDelay;
                yield return Yielders.EndOfFrame;
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

        public void FullStop()
        {
            Stop();
            CancelInvoke();
            StopAllCoroutines();
        }

        public override void Despawn()
        {
            intensity = speed = 0;
            hapticDevice = HapticDevice.None;
            FullStop();
            if (Application.isPlaying)
            {
                EffectModuleAudio.Despawn(this);
                InvokeDespawnCallback();
            }

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
#endif
    }
}

using UnityEngine;
using System;
using System.Collections;
using Debug = UnityEngine.Debug;


namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/EffectAudio")]
    public class EffectAudio : Effect
    {
        public AudioContainer audioContainer;

        public float globalVolumeDb = 0;
        public float globalPitch = 1;
        public float reverbZoneMix = 1;

        public HapticDevice hapticDevice = HapticDevice.None;
        public GameData.HapticClip hapticClipFallBack;

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

        [Header("Dynamic Music")]
        public bool onDynamicMusic = false;
        public Music.MusicTransition.TransitionType dynamicMusicTiming = Music.MusicTransition.TransitionType.OnBeat;


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
            if (invokeRandomPlay)
            {
                CancelInvoke("RandomPlay");
                invokeRandomPlay = false;
            }
            if (invokeDespawn)
            {
                CancelInvoke("Despawn");
                invokeDespawn = false;
            }
            StopAllCoroutines();

            //Debug.Log("Play " + (module as EffectModuleAudio).audioContainerAddress + " volume: " + audioSource.volume + " parent: " + this.transform.parent + " parent2: " + this.transform.parent?.parent + " parent3: " + this.transform.parent?.parent?.parent);

            //Reset the spatialBlend back to what it is in the effectModuleAudio

            if (module is EffectModuleAudio effectModuleAudio)
            {
                audioSource.spatialBlend = effectModuleAudio.spatialBlend;


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

                float pitch = audioSource.pitch;
                if (randomPitch)
                {
                    pitch = pitchCurve.Evaluate(UnityEngine.Random.Range(0f, 1f));
                }
                pitch *= globalPitch;
                audioSource.pitch = pitch;
                
                float delay = playDelay;
                if (onDynamicMusic)
                {
                }
                else if (randomPlay)
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

                if (step == Step.Start || step == Step.End)
                {
                    Invoke("Despawn", audioSource.clip.length + delay + 0.1f);
                    invokeDespawn = true;
                }

                playTime = Time.time;
            }
        }
        public bool invokeDespawn { get; private set; }

        protected void RandomPlay()
        {
            if (audioContainer.TryPickAudioClip(out AudioClip audioClip))
            {
                audioSource.clip = audioClip;
                if (!audioSource.isPlaying) audioSource.Play();
                float randomDelay = UnityEngine.Random.Range(randomMinTime, randomMaxTime);
                Invoke("RandomPlay", randomDelay);
                invokeRandomPlay = true;
                Debug.Log(randomDelay);
            }
        }
        public bool invokeRandomPlay { get; private set; }

        public override void Stop()
        {
            audioSource.Stop();
            hasNoise = false;
            hapticDevice = HapticDevice.None;
        }

        public override void End(bool loopOnly = false)
        {
            if (invokeRandomPlay)
            {
                CancelInvoke("RandomPlay");
                invokeRandomPlay = false;
            }
            
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

        public override void SetHaptic(HapticDevice hapticDevice, GameData.HapticClip hapticClipFallBack)
        {
            this.hapticDevice = hapticDevice;
            this.hapticClipFallBack = hapticClipFallBack;
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
            base.SetIntensity(value, loopOnly);
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                if (((module as EffectModuleAudio)?.intensitySmoothingSampleCount ?? 0) > 0)
                {
                    value = Smooth(value, ref intensitySmoothingBuffer, ref intensitySmoothingIndex, (module as EffectModuleAudio).intensitySmoothingSampleCount);
                }
                effectIntensity = value;
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
                effectSpeed = value;
                Refresh();
            }
        }

        public static float CalculateVolume(float globalVolumeDb, bool useVolumeIntensity, bool useVolumeSpeed, BlendMode volumeBlendMode, float intensity, float speed, AnimationCurve volumeIntensityCurve, AnimationCurve volumeSpeedCurve)
        {
            volumeIntensityCurve ??= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
            volumeSpeedCurve ??= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
            float linearVolume = DecibelToLinear(globalVolumeDb);
            if (useVolumeIntensity && useVolumeSpeed)
            {
                switch (volumeBlendMode)
                {
                    case BlendMode.Min:
                        return Mathf.Min(volumeIntensityCurve.Evaluate(intensity), volumeSpeedCurve.Evaluate(speed)) * linearVolume;
                    case BlendMode.Max:
                        return Mathf.Max(volumeIntensityCurve.Evaluate(intensity), volumeSpeedCurve.Evaluate(speed)) * linearVolume;
                    case BlendMode.Average:
                        return Mathf.Lerp(volumeIntensityCurve.Evaluate(intensity), volumeSpeedCurve.Evaluate(speed), 0.5f) * linearVolume;
                    case BlendMode.Multiply:
                        return volumeIntensityCurve.Evaluate(intensity) * volumeIntensityCurve.Evaluate(speed) * linearVolume;
                }
            }
            else if (useVolumeIntensity)
            {
                return volumeIntensityCurve.Evaluate(intensity) * linearVolume;
            }
            else if (useVolumeSpeed)
            {
                return volumeSpeedCurve.Evaluate(speed) * linearVolume;
            }
            return 0;
        }
        
        public void Refresh()
        {
            audioSource.volume = CalculateVolume(globalVolumeDb, useVolumeIntensity, useVolumeSpeed, volumeBlendMode, effectIntensity, effectSpeed, volumeIntensityCurve, volumeSpeedCurve);

            switch (pitchEffectLink)
            {
                case EffectLink.Intensity:
                    audioSource.pitch = pitchCurve.Evaluate(effectIntensity) * globalPitch;
                    break;
                case EffectLink.Speed:
                    audioSource.pitch = pitchCurve.Evaluate(effectSpeed) * globalPitch;
                    break;
            }


            if (useLowPassFilter)
            {
                float value = lowPassEffectLink switch
                {
                    EffectLink.Intensity => effectIntensity,
                    EffectLink.Speed => effectSpeed,
                    _ => 0
                };

                lowPassFilter.cutoffFrequency = lowPassCutoffFrequencyCurve.Evaluate(value);
                lowPassFilter.lowpassResonanceQ = lowPassResonanceQCurve.Evaluate(value);
            }

            if (useHighPassFilter)
            {
                float value = highPassEffectLink switch
                {
                    EffectLink.Intensity => effectIntensity,
                    EffectLink.Speed => effectSpeed,
                    _ => 0
                };
                highPassFilter.cutoffFrequency = highPassCutoffFrequencyCurve.Evaluate(value);
                highPassFilter.highpassResonanceQ = highPassResonanceQCurve.Evaluate(value);
            }

            audioSource.reverbZoneMix = reverbZoneMix;

            if (useReverbFilter)
            {
                float value = reverbEffectLink switch
                {
                    EffectLink.Intensity => effectIntensity,
                    EffectLink.Speed => effectSpeed,
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

        public void FullStop()
        {
            Stop();
            if (invokeRandomPlay)
            {
                CancelInvoke("RandomPlay");
                invokeRandomPlay = false;
            }
            if (invokeDespawn)
            {
                CancelInvoke("Despawn");
                invokeDespawn = false;
            }
            StopAllCoroutines();
        }

        /// <summary>
        /// This is used by the pooling system to sort of fake despawn the effect without returning it to the pool, because we are going to use it again right away
        /// </summary>
        public void FakeDespawn()
        {
            effectIntensity = effectSpeed = 0;
            hapticDevice = HapticDevice.None;
            FullStop();
        }
        
        public override void Despawn()
        {
            effectIntensity = effectSpeed = 0;
            hapticDevice = HapticDevice.None;
            FullStop();
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
